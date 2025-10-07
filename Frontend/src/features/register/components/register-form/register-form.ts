import {Component, EventEmitter, Input, output, Output} from '@angular/core';
import {RouterLink} from '@angular/router';
import {InputField} from '../../../../shared/components/input-field/input-field';
import {Button} from '../../../../shared/components/button/button';
import {AuthService} from '../../../../shared/services/auth.service';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {UserRegisterDto} from '../../../../shared/dto/user/user-register.dto';

@Component({
  selector: 'app-register-form',
  imports: [
    InputField,
    Button,
    RouterLink,
    ReactiveFormsModule,
  ],
  templateUrl: './register-form.html',
  styleUrl: './register-form.css'
})
export class RegisterForm {
  showError: boolean = false;
  errorText: string = '';
  succeeded = output<string>();
  failed = output<any>();

  registerForm = new FormGroup({
    name: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(50),
      Validators.pattern(/^[\p{L}\p{Nd} ]+$/u),
    ]),
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

  get name() {
    return this.registerForm.get('name');
  }

  get nameIsInvalid(): boolean {
    return (this.name?.invalid && (this.name?.dirty || this.name?.touched)) ?? true;
  }

  get username() {
    return this.registerForm.get('username');
  }

  get usernameIsInvalid(): boolean {
    return (this.username?.invalid && (this.username?.dirty || this.username?.touched)) ?? true;
  }

  get password() {
    return this.registerForm.get('password');
  }

  get passwordIsInvalid(): boolean {
    return (this.password?.invalid && (this.password?.dirty || this.password?.touched)) ?? true;
  }

  onSubmit() {
    const form = this.registerForm.value;
    const userRegisterDto: UserRegisterDto = {
      name: form.name ?? "",
      username: form.username ?? "",
      password: form.password ?? "",
    };

    this.authService
      .register(userRegisterDto)
      .subscribe({
        next: result => {
          this.showError = false;
          this.errorText = '';
          this.succeeded.emit(result.token);
        },
        error: (err: { status: number }) => {
          this.showError = true;
          this.failed.emit(err);

          if (err.status === 409) {
            this.errorText = 'Пользователь с таким логином уже существует';
          }
        }
      });
  }
}
