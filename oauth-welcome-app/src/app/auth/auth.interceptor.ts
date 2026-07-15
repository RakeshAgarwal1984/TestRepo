import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { apiConfig } from '../core/api.config';
import { AuthService } from './auth.service';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const authService = inject(AuthService);
  const token = authService.accessToken();
  const isApiRequest = request.url.startsWith(apiConfig.apiBaseUrl);

  if (!token || !isApiRequest) {
    return next(request);
  }

  return next(
    request.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    })
  );
};
