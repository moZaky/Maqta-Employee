import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApplicationUser } from '../common/models/application-user';
import { CacheService } from './cache.service';
import { API_URLS } from '../utility/api-urls';
import { ResponseModel } from '../common/models/response-model';
import { LocalStorageItem } from '../common/models/local-storage-item';
import { UtilityService } from './utility.service';
import { UserService } from './user.service';
import { LoginModel, RegisterModel } from '../common/models/accounts-model';
import { StatusCode } from '../common/enums/status-codes';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  constructor(private http: HttpClient, private userService: UserService) {}

  public login(model: LoginModel): Observable<ResponseModel<string>> {
    return this.http.post<ResponseModel<string>>(API_URLS.ACCOUNT_LOGIN, model);
  }

  public register(model: RegisterModel): Observable<ResponseModel<string>> {
    return this.http.post<ResponseModel<string>>(
      API_URLS.ACCOUNT_REGISTER,
      model
    );
  }
  public update(model: ApplicationUser): Observable<ResponseModel<StatusCode>> {
    return this.http.post<ResponseModel<StatusCode>>(
      API_URLS.ACCOUNT_UPDATE,
      model
    );
  }
  public activate(email: string): Observable<ResponseModel<StatusCode>> {
    return this.http.get<ResponseModel<StatusCode>>(
      `${API_URLS.ACCOUNT_ACTIVATE}/?email=${email}`
    );
  }
  public deActivate(email: string): Observable<ResponseModel<StatusCode>> {
    return this.http.get<ResponseModel<StatusCode>>(
      `${API_URLS.ACCOUNT_DEACTIVATE}/?email=${email}`
    );
  }
}
