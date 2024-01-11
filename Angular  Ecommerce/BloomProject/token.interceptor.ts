import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from 'src/app/Services/auth.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const userToken = this.authService.getToken();

    if (userToken) {
      const clonedRequest = req.clone({
        setHeaders: {
          Authorization: `Bearer ${userToken}`
        }
      });

      return next.handle(clonedRequest);
    }

    return next.handle(req);
  }
}
