export const oauthConfig = {
  issuer: 'http://localhost:8080/realms/interview-prep',
  clientId: 'angular-client',
  redirectUri: `${window.location.origin}/auth/callback`,
  postLogoutRedirectUri: `${window.location.origin}/login`,
  scope: 'openid profile email',
  audience: 'interview-prep-api',
  responseType: 'code',
  useDiscoveryDocument: true
};
