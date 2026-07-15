import { Component, signal } from '@angular/core';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.html',
  styleUrl: './login-page.scss'
})
export class LoginPage {
  protected readonly error = signal('');

  constructor(private readonly authService: AuthService) {}

  protected async signInWithOAuth(): Promise<void> {
    this.error.set('');

    try {
      await this.authService.loginWithOAuth();
    } catch (error) {
      this.error.set(error instanceof Error ? error.message : 'Sign-in could not be started.');
    }
  }

  protected loginAsAdmin(): void {
    this.authService.loginAsDemoUser('Admin');
  }

  protected loginAsEmployee(): void {
    this.authService.loginAsDemoUser('Employee');
  }
}
