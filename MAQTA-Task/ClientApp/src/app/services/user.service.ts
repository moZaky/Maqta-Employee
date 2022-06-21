import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApplicationUser } from '../common/models/application-user';
import { CacheService } from './cache.service';
import { API_URLS } from './../utility/api-urls';
import { ResponseModel } from '../common/models/response-model';
import { LocalStorageItem } from '../common/models/local-storage-item';
import { UtilityService } from './utility.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  public user?: ApplicationUser;
  private userSubject: Subject<ApplicationUser | undefined> = new Subject<
    ApplicationUser | undefined
  >();
  private isLoadingCurrentUser = false;

  constructor(
    private http: HttpClient,
    private _cacheService: CacheService,
    private utilityService: UtilityService,
    private router: Router
  ) {}

  getUser(): Observable<ApplicationUser | undefined> {
    this.loadUser();
    return this.userSubject.asObservable();
  }

  isLoggedIn(): boolean {
    let user = this._cacheService.getCustomLocalStorageItem<any>('token');
    return user != null && user.data != null;
  }

  private loadUser(): void {
    var item =
      this._cacheService.getCustomLocalStorageItem<ApplicationUser>('user');
    if (
      item != null &&
      this.utilityService.getDiffDays(item.date, new Date()) <= 1
    ) {
      this.user = item.data;
      setTimeout(() => {
        return this.userSubject.next(this.user);
      });
    } else {
      if (!this.isLoadingCurrentUser) {
        this.isLoadingCurrentUser = true;
        this.http.get<ApplicationUser>(API_URLS.ACCOUNT_CURRENT_USER).subscribe(
          (result) => {
            if (result) {
              this.user = result;

              var item: LocalStorageItem<ApplicationUser> = {
                date: new Date(),
                data: result,
              };
              this._cacheService.setCustomLocalStorageItem<ApplicationUser>(
                'user',
                item
              );
              this.userSubject.next(this.user);
            }
          },
          (error) => {
            console.error('loadUser', error);
          },
          () => {
            this.isLoadingCurrentUser = false;
          }
        );
      }
    }
  }

  logOut() {
    return this.http.post<ResponseModel<any>>(API_URLS.ACCOUNT_LOGOUT, {});
  }
  getToken() {
    const token = this._cacheService.getCustomLocalStorageItem<string>('token');
    return token?.data;
  }
  setToken(token: string) {
    var item: LocalStorageItem<string> = {
      date: new Date(),
      data: token,
    };
    return this._cacheService.setCustomLocalStorageItem<string>('token', item);
  }
  clearAndLogOut(): void {
    this.logOut().subscribe((data) => {
      this._cacheService.removeItemFromStorage('token');
      this._cacheService.removeItemFromStorage('user');
      this.router.navigateByUrl('/login');
    });
  }
}
