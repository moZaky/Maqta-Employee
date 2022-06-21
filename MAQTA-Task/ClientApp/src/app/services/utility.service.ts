import { Injectable } from '@angular/core';
import * as moment from 'moment';

@Injectable({
  providedIn: 'root',
})
export class UtilityService {
  public baseUrl: string;
  constructor() {
    this.baseUrl = this.getBaseUrl();
  }

  public getBaseUrl(): string {
    if (this.baseUrl != null || this.baseUrl != '')
      this.baseUrl = window.location.origin;
    return this.baseUrl;
  }

  checkIfObjectHasValues(data: any) {
    return Object.values(data).filter((info) => info).length > 0;
  }
  public getDiffDays(startDate: Date, endDate: Date = new Date()): number {
    return moment(endDate).diff(moment(startDate), 'days');
  }
}
