import { Component, OnInit, inject, signal } from '@angular/core';
import { forkJoin } from 'rxjs';
import { AuthService } from '../../auth/auth.service';
import { ApiService } from '../../core/api.service';
import { InterviewQuestion, WelcomeResponse } from '../../core/api.models';

@Component({
  selector: 'app-welcome-page',
  templateUrl: './welcome-page.html',
  styleUrl: './welcome-page.scss'
})
export class WelcomePage implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly apiService = inject(ApiService);

  protected readonly user = this.authService.currentUser;
  protected readonly welcome = signal<WelcomeResponse | null>(null);
  protected readonly questions = signal<InterviewQuestion[]>([]);
  protected readonly apiStatus = signal<'loading' | 'connected' | 'unavailable'>('loading');
  protected readonly apiMessage = signal('Connecting to the .NET API...');

  ngOnInit(): void {
    forkJoin({
      welcome: this.apiService.getWelcome(),
      questions: this.apiService.getInterviewQuestions()
    }).subscribe({
      next: ({ welcome, questions }) => {
        this.welcome.set(welcome);
        this.questions.set(questions);
        this.apiStatus.set('connected');
        this.apiMessage.set('Connected to the secured .NET API.');
      },
      error: () => {
        this.apiStatus.set('unavailable');
        this.apiMessage.set('API data is unavailable. Use a real OAuth2 access token, then keep the API running on localhost:5058.');
      }
    });
  }

  protected logout(): void {
    this.authService.logout();
  }
}
