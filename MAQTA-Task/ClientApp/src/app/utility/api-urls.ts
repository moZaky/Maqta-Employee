import { APP_CONSTANTS } from './app-constants';

export const API_URLS = Object.freeze({
  ACCOUNT_LOGIN: `${APP_CONSTANTS.API}Account/Login`,
  ACCOUNT_CURRENT_USER: `${APP_CONSTANTS.API}Account/LoggedInUser`,
  ACCOUNT_REGISTER: `${APP_CONSTANTS.API}Account/Register`,
  ACCOUNT_ACTIVATE: `${APP_CONSTANTS.API}Account/Activate`,
  ACCOUNT_DEACTIVATE: `${APP_CONSTANTS.API}Account/Deactivate`,
  ACCOUNT_UPDATE: `${APP_CONSTANTS.API}Account/Update`,
  ACCOUNT_LOGOUT: `${APP_CONSTANTS.API}Account/LogOut`,
  ADMIN_GET_USERS: `${APP_CONSTANTS.API}Admin/UsersList`,
} as const);
