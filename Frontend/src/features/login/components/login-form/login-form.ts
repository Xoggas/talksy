import {Component, EventEmitter, Input, output, Output} from '@angular/core';
import {RouterLink} from '@angular/router';
import {InputField} from '../../../../shared/components/input-field/input-field';
import {Button} from '../../../../shared/components/button/button';
import {AuthService} from '../../../../shared/services/auth.service';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {UserRegisterDto} from '../../../../shared/dto/user/user-register.dto';
import {UserLoginDto} from '../../../../shared/dto/user/user-login.dto';

@Component({
  selector: 'app-login-form',
  imports: [
    InputField,
    Button,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './login-form.html',
  styleUrl: './login-form.css'
})
export class LoginForm {
  showError: boolean = false;
  errorText: string = '';
  succeeded = output<string>();
  failed = output<any>();

  loginForm = new FormGroup({
    username: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(50),
      Validators.pattern(/^[a-zA-Z0-9_]+$/u),
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(50),
      Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]+$/),
    ]),
  });

  constructor(private readonly authService: AuthService) {
  }

  get username() {
    return this.loginForm.get('username');
  }

  get usernameIsInvalid(): boolean {
    return (this.username?.invalid && (this.username?.dirty || this.username?.touched)) ?? true;
  }

  get password() {
    return this.loginForm.get('password');
  }

  get passwordIsInvalid(): boolean {
    return (this.password?.invalid && (this.password?.dirty || this.password?.touched)) ?? true;
  }

  onSubmit() {
    const form = this.loginForm.value;
    const userLoginDto: UserLoginDto = {
      username: form.username ?? "",
      password: form.password ?? "",
    };

    this.authService
      .login(userLoginDto)
      .subscribe({
        next: result => {
          this.showError = false;
          this.errorText = '';
          this.succeeded.emit(result.token);
        },
        error: (err: { status: number }) => {
          this.showError = true;
          this.failed.emit(err);

          if (err.status === 401) {
            this.errorText = 'Неверный логин или пароль';
          }
        }
      });
  }
}
