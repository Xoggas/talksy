import { HttpInterceptorFn } from '@angular/common/http';
import {environment} from '../../environments/environment';

export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  if (req.url.startsWith('http')) {
    return next(req);
  }

  const token = localStorage.getItem('token');

  let headers = req.headers;
  if (token) {
    headers = headers.set('Authorization', `Bearer ${token}`);
  }

  const apiReq = req.clone({
    url: `${environment.apiUrl}/api/${req.url}`,
    headers
  });

  return next(apiReq);
};
