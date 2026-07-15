# OAuth Integration Notes

## API

The API validates OAuth2 access tokens with JWT bearer authentication.

Configure these values in `src/InterviewPrep.Api/appsettings.Development.json`:

```json
"Authentication": {
  "Authority": "http://localhost:8080/realms/interview-prep",
  "Audience": "",
  "RequiredScopes": []
}
```

Expected token claims:

- `role` or `roles`: `Admin` or `Employee`

Swagger has an `Authorize` button. Paste a valid access token there to call secured endpoints.

## Angular

Configure these values in `oauth-welcome-app/src/app/auth/oauth.config.ts`:

```ts
issuer: 'http://localhost:8080/realms/interview-prep',
clientId: 'angular-client',
scope: 'openid profile email',
audience: 'interview-prep-api'
```

For first local testing, the API does not validate `aud`. After the login flow works, add a Keycloak audience mapper that emits `interview-prep-api`, then set API `Audience` back to `interview-prep-api`.

The Angular app sends the access token to:

```text
http://localhost:5058/api
```

Update `oauth-welcome-app/src/app/core/api.config.ts` if the API port changes.
