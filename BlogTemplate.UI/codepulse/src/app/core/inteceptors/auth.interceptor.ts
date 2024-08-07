import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const cookie = inject(CookieService);
  if(req.url.includes('addAuth=true')) {
    const request = req.clone({
      setHeaders: {
        Authorization: cookie.get('Authorization'),
    }});
    return next(request);
  }
  return next(req);
 
};
