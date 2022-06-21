import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { AccountService } from 'src/app/services/account.service';
import { AdminService } from 'src/app/services/admin.service';

import { UserService } from 'src/app/services/user.service';
import { ApplicationUser } from '../../../../common/models/application-user';
import { APP_CONSTANTS } from '../../../../utility/app-constants';
@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})
export class AdminComponent implements OnInit {
  showEmptyMsg: boolean = false;
  isLoading: boolean = true;
  users: ApplicationUser[] = [];
  emptyMsg: string = '';
  constructor(
    private service: AdminService,
    private _accountService: AccountService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.service.getUsers().subscribe({
      next: (result) => {
        if (result.length > 0) {
          this.users = result;
        } else {
          this.showEmptyMsg = true;
          this.emptyMsg = APP_CONSTANTS.NO_DATA;
        }
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.showEmptyMsg = true;

        this.emptyMsg = APP_CONSTANTS.SOMETHING_WENT_WRONG;
      },
    });
  }
  deActivate(email: string) {
    this.isLoading = true;

    this._accountService.deActivate(email).subscribe((data) => {
      this.loadUsers();
    });
    this.isLoading = false;
  }
  activate(email: string) {
    this.isLoading = true;

    this._accountService.activate(email).subscribe((data) => {
      this.loadUsers();
    });
    this.isLoading = false;
  }
}
