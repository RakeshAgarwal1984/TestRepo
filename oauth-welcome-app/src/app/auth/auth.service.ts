import { Injectable, computed, signal } from '@angular/core';
import { Router } from '@angular/router';
import { AppRole, AppUser, TokenResponse } from './auth.models';
import { oauthConfig } from './oauth.config';

const USER_STORAGE_KEY = 'oauth-welcome-app:user';
const PKCE_STORAGE_KEY = 'oauth-welcome-app:pkce';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly currentUserSignal = signal<AppUser | null>(this.readStoredUser());

  readonly currentUser = this.currentUserSignal.asReadonly();
  readonly isAuthenticated = computed(() => !!this.currentUserSignal());
  readonly accessToken = computed(() => this.currentUserSignal()?.accessToken ?? '');

  constructor(private readonly router: Router) {}

  async loginWithOAuth(): Promise<void> {
    const verifier = this.randomString(64);
    const challenge = await this.createCodeChallenge(verifier);
    const state = this.randomString(32);

    sessionStorage.setItem(PKCE_STORAGE_KEY, JSON.stringify({ verifier, state }));

    const params = new URLSearchParams({
      response_type: oauthConfig.responseType,
      client_id: oauthConfig.clientId,
      redirect_uri: oauthConfig.redirectUri,
      scope: oauthConfig.scope,
      audience: oauthConfig.audience,
      code_challenge: challenge,
      code_challenge_method: 'S256',
      state
    });

    window.location.href = `${oauthConfig.issuer}/protocol/openid-connect/auth?${params.toString()}`;
  }

  async completeOAuthLogin(code: string, state: string): Promise<void> {
    const pkce = this.readPkceState();

    if (!pkce || pkce.state !== state) {
      throw new Error('The sign-in response could not be verified.');
    }

    const response = await fetch(`${oauthConfig.issuer}/protocol/openid-connect/token`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
      body: new URLSearchParams({
        grant_type: 'authorization_code',
        client_id: oauthConfig.clientId,
        code,
        redirect_uri: oauthConfig.redirectUri,
        code_verifier: pkce.verifier
      })
    });

    if (!response.ok) {
      throw new Error('The identity provider rejected the sign-in request.');
    }

    const token = (await response.json()) as TokenResponse;
    this.setUser(this.userFromToken(token));
    sessionStorage.removeItem(PKCE_STORAGE_KEY);
    await this.router.navigateByUrl('/welcome');
  }

  loginAsDemoUser(role: AppRole): void {
    const user: AppUser =
      role === 'Admin'
        ? {
            id: 'admin-demo',
            name: 'Avery Admin',
            email: 'avery.admin@contoso.com',
            role,
            accessToken: 'demo-admin-token'
          }
        : {
            id: 'employee-demo',
            name: 'Emery Employee',
            email: 'emery.employee@contoso.com',
            role,
            accessToken: 'demo-employee-token'
          };

    this.setUser(user);
    void this.router.navigateByUrl('/welcome');
  }

  logout(): void {
    localStorage.removeItem(USER_STORAGE_KEY);
    this.currentUserSignal.set(null);
    void this.router.navigateByUrl('/login');
  }

  private setUser(user: AppUser): void {
    localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(user));
    this.currentUserSignal.set(user);
  }

  private userFromToken(token: TokenResponse): AppUser {
    const claims = this.decodeJwt(token.id_token ?? token.access_token);
    const rawRole =
      claims['role'] ??
      claims['roles']?.[0] ??
      claims['realm_access']?.roles?.find((role: string) => role === 'Admin' || role === 'Employee') ??
      claims['resource_access']?.[oauthConfig.clientId]?.roles?.find((role: string) => role === 'Admin' || role === 'Employee') ??
      claims['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    const role: AppRole = rawRole === 'Admin' ? 'Admin' : 'Employee';

    return {
      id: String(claims['sub'] ?? claims['oid'] ?? claims['nameid'] ?? 'signed-in-user'),
      name: String(claims['name'] ?? claims['preferred_username'] ?? claims['email'] ?? 'Signed-in user'),
      email: String(claims['email'] ?? claims['preferred_username'] ?? ''),
      role,
      accessToken: token.access_token
    };
  }

  private decodeJwt(jwt: string): Record<string, any> {
    const payload = jwt.split('.')[1];

    if (!payload) {
      return {};
    }

    const normalized = payload.replace(/-/g, '+').replace(/_/g, '/');
    const padded = normalized.padEnd(normalized.length + ((4 - (normalized.length % 4)) % 4), '=');
    return JSON.parse(window.atob(padded));
  }

  private readStoredUser(): AppUser | null {
    const stored = localStorage.getItem(USER_STORAGE_KEY);

    if (!stored) {
      return null;
    }

    try {
      return JSON.parse(stored) as AppUser;
    } catch {
      localStorage.removeItem(USER_STORAGE_KEY);
      return null;
    }
  }

  private readPkceState(): { verifier: string; state: string } | null {
    const stored = sessionStorage.getItem(PKCE_STORAGE_KEY);
    return stored ? (JSON.parse(stored) as { verifier: string; state: string }) : null;
  }

  private randomString(length: number): string {
    const values = new Uint8Array(length);
    crypto.getRandomValues(values);
    return Array.from(values, (value) => ('0' + value.toString(16)).slice(-2)).join('');
  }

  private async createCodeChallenge(verifier: string): Promise<string> {
    const data = new TextEncoder().encode(verifier);
    const digest = await crypto.subtle.digest('SHA-256', data);
    return this.base64UrlEncode(digest);
  }

  private base64UrlEncode(buffer: ArrayBuffer): string {
    const bytes = new Uint8Array(buffer);
    const value = String.fromCharCode(...bytes);
    return btoa(value).replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');
  }
}
