import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { StatusCode } from 'src/app/common/enums/status-codes';
import { AccountService } from 'src/app/services/account.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  form: FormGroup;
  errorMsg: string = '';
  showErrorMsg: boolean = false;
  isLoading: boolean = false;

  constructor(
    private _userService: UserService,
    private _accountService: AccountService,
    public router: Router
  ) {
    this.form = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      phoneNumber: new FormControl('', [Validators.maxLength(12)]),
      email: new FormControl('', [Validators.email]),
    });
  }

  ngOnInit(): void {
    if (this._userService.isLoggedIn()) this.router.navigate(['/profile']);
  }
  get frm() {
    return this.form.controls;
  }
  get username() {
    return this.form.get('username') as FormControl;
  }

  get password() {
    return this.form.get('password') as FormControl;
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
  onFormSubmit(event: Event) {
    event.preventDefault();
    this.isLoading = true;

    let formValues = this.form.getRawValue();
    if (this.form.invalid) {
      this.isLoading = false;

      return;
    }
    this._accountService.register(formValues).subscribe({
      next: (result) => {
        if (result.status === StatusCode.Created) {
          this.router.navigateByUrl('/login');
          this.form.reset();
          this.showErrorMsg = false;
        }
        if (result.status === StatusCode.Duplicate) {
          this.showErrorMsg = true;
          this.errorMsg = 'username already in use';
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
}
