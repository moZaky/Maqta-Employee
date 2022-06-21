import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApplicationUser } from '../common/models/application-user';
import { ResponseModel } from '../common/models/response-model';
import { API_URLS } from '../utility/api-urls';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  constructor(private http: HttpClient) {}

  public getUsers(): Observable<ApplicationUser[]> {
    return this.http.get<ApplicationUser[]>(API_URLS.ADMIN_GET_USERS);
  }
}
