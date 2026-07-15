export type AppRole = 'Admin' | 'Employee';

export interface AppUser {
  id: string;
  name: string;
  email: string;
  role: AppRole;
  accessToken: string;
}

export interface TokenResponse {
  access_token: string;
  id_token?: string;
  token_type: string;
  expires_in: number;
  scope?: string;
}
