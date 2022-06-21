import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserService } from 'src/app/services/user.service';
import { UtilityService } from './../../services/utility.service';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {
  constructor(
    private _userService: UserService,
    private _utilityService: UtilityService
  ) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // modify request
    const isLoggedIn = this._userService.isLoggedIn();
    if (isLoggedIn) {
      const token = this._userService.getToken();
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${token}` },
      });
    }

    return next.handle(request);
  }
}
