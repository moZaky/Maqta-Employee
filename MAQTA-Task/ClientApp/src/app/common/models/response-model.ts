import { StatusCode } from '../enums/status-codes';

export interface ResponseModel<T> {
  isSucceeded: boolean;
  data: T;
  status: StatusCode;
  message: string;
}
