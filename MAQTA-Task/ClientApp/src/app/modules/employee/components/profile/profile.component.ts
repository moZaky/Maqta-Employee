import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import * as moment from 'moment';
import { StatusCode } from 'src/app/common/enums/status-codes';
import { AccountService } from 'src/app/services/account.service';

import { UserService } from 'src/app/services/user.service';
import { ApplicationUser } from './../../../../common/models/application-user';
import { APP_CONSTANTS } from './../../../../utility/app-constants';
import { CacheService } from './../../../../services/cache.service';
@Component({
  selector: 'app-employee-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  showEmptyMsg: boolean = false;
  isLoading: boolean = true;
  model: ApplicationUser = {} as ApplicationUser;
  emptyMsg: string = '';
  form: FormGroup;
  showErrorMsg: boolean = false;
  errorMsg: string = '';
  constructor(
    private service: UserService,
    private _accountService: AccountService,
    private _cacheService: CacheService
  ) {
    this.form = new FormGroup({
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      phoneNumber: new FormControl('', [Validators.maxLength(12)]),
      email: new FormControl('', [Validators.email]),
    });
  }

  ngOnInit(): void {
    this.loadUser();
  }

  get email() {
    return this.form.get('email') as FormControl;
  }
  get phone() {
    return this.form.get('phoneNumber') as FormControl;
  }
  get firstName() {
    return this.form.get('firstName') as FormControl;
  }
  get lastName() {
    return this.form.get('lastName') as FormControl;
  }
  loadUser(): void {
    this.isLoading = true;
    this.service.getUser().subscribe({
      next: (result) => {
        if (result) {
          this.model = result;
          this.form.patchValue(this.model);
        } else {
          this.showEmptyMsg = true;
          this.emptyMsg = APP_CONSTANTS.SOMETHING_WENT_WRONG;
        }
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.emptyMsg = APP_CONSTANTS.SOMETHING_WENT_WRONG;
      },
    });
  }
  onFormSubmit(event: Event) {
    event.preventDefault();
    this.isLoading = true;

    let formValues = {
      ...this.form.getRawValue(),
      userName: this.model.userName,
    };

    if (this.form.invalid) {
      this.isLoading = false;

      return;
    }
    this._accountService.update(formValues).subscribe({
      next: (result) => {
        if (result.status === StatusCode.Succeeded) {
          this._cacheService.setCustomLocalStorageItem('user', formValues);
          this.model = formValues;
          this.form.patchValue(formValues);
          this.showErrorMsg = false;
        } else {
          this.showErrorMsg = true;
          this.errorMsg = result.message;
        }
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
      },
    });
  }
  deActivate() {
    this.isLoading = true;

    this._accountService.deActivate(this.model.userName).subscribe((data) => {
      this.service.clearAndLogOut();
    });
    this.isLoading = false;
  }
}
