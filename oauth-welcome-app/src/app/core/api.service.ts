import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { apiConfig } from './api.config';
import { InterviewQuestion, WelcomeResponse } from './api.models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private readonly http: HttpClient) {}

  getWelcome(): Observable<WelcomeResponse> {
    return this.http.get<WelcomeResponse>(`${apiConfig.apiBaseUrl}/welcome`);
  }

  getInterviewQuestions(): Observable<InterviewQuestion[]> {
    return this.http.get<InterviewQuestion[]>(`${apiConfig.apiBaseUrl}/interview-questions`);
  }
}
