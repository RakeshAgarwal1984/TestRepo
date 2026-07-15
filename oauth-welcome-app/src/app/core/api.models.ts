export interface WelcomeResponse {
  message: string;
  role: string;
  visibleFeatures: string[];
}

export interface InterviewQuestion {
  id: string;
  title: string;
  answer: string;
  difficulty: 'Easy' | 'Medium' | 'Hard';
  category: string;
  adminOnly: boolean;
}
