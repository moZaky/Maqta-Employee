import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';
import { Router } from '@angular/router';
import { CacheService } from 'src/app/services/cache.service';
import { ApplicationUser } from './../../common/models/application-user';
import { UserType } from 'src/app/common/enums/user-types';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent implements OnInit {
  isAdmin: boolean = false;
  constructor(
    public _userService: UserService,
    public router: Router,
    private _cacheService: CacheService
  ) {
    this._userService.getUser();
    this.isAdmin = this._userService.user?.userType === UserType.Admin;
  }

  ngOnInit(): void {}
  logOut() {
    this._userService.clearAndLogOut();
  }
}
