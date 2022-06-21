import { ApplicationUser } from './application-user';

export interface LoginModel {
  userName: string;
  password: string;
}
export interface RegisterModel extends ApplicationUser {
  password: string;
}
