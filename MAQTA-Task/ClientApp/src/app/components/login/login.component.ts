import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { StatusCode } from 'src/app/common/enums/status-codes';
import { AccountService } from 'src/app/services/account.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  errorMsg: string = '';
  isLoading: boolean = false;

  constructor(
    private _userService: UserService,
    private _accountService: AccountService,
    public router: Router
  ) {
    this.form = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
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
  onFormSubmit(event: Event) {
    event.preventDefault();
    this.isLoading = true;

    let formValues = this.form.getRawValue();
    if (this.form.invalid) return;
    this._accountService.login(formValues).subscribe({
      next: (result) => {
        if (result.status === StatusCode.Succeeded) {
          this._userService.setToken(result.data);
          this.router.navigateByUrl('profile');
          this.form.reset();
        } else {
          this.errorMsg = result.message;
        }
        this.isLoading = true;
      },
      error: (error) => {
        this.isLoading = false;
      },
    });
  }
}
