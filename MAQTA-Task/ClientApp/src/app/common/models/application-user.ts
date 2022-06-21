import { UserType } from '../enums/user-types';

export interface ApplicationUser {
  isActive: boolean;
  email: string;
  phoneNumber: string;
  userName: string;
  id: number;
  userType: UserType;
  lastName: string;
  firstName: string;
}
