import { Routes } from '@angular/router';
import { authGuard } from './auth/auth.guard';
import { LoginPage } from './pages/login-page/login-page';
import { OAuthCallbackPage } from './pages/oauth-callback-page/oauth-callback-page';
import { WelcomePage } from './pages/welcome-page/welcome-page';

export const routes: Routes = [
  { path: 'login', component: LoginPage },
  { path: 'auth/callback', component: OAuthCallbackPage },
  { path: 'welcome', component: WelcomePage, canActivate: [authGuard] },
  { path: '', pathMatch: 'full', redirectTo: 'welcome' },
  { path: '**', redirectTo: 'welcome' }
];
