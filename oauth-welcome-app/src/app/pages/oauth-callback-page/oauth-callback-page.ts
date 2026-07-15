import { Component, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-oauth-callback-page',
  imports: [RouterLink],
  templateUrl: './oauth-callback-page.html',
  styleUrl: './oauth-callback-page.scss'
})
export class OAuthCallbackPage implements OnInit {
  protected readonly error = signal('');

  constructor(private readonly authService: AuthService) {}

  async ngOnInit(): Promise<void> {
    const params = new URLSearchParams(window.location.search);
    const code = params.get('code');
    const state = params.get('state');

    if (!code || !state) {
      this.error.set('The sign-in response did not include the required OAuth2 code.');
      return;
    }

    try {
      await this.authService.completeOAuthLogin(code, state);
    } catch (error) {
      this.error.set(error instanceof Error ? error.message : 'Sign-in could not be completed.');
    }
  }
}
