import {Component} from '@angular/core';
import {Sidebar} from '../../../shared/components/sidebar/sidebar';
import {LoginForm} from '../components/login-form/login-form';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login-page',
  imports: [
    LoginForm,
    Sidebar
  ],
  templateUrl: './login-page.html',
  styleUrl: './login-page.css'
})
export class LoginPage {
  constructor(private router: Router) {
  }

  onSuccessfulLogin(jwtToken: string): void {
    localStorage.setItem('token', jwtToken);
    this.router.navigate(['/chats']).then();
  }

  onFailedLogin(err: any): void {
    console.log("Failed", err);
  }
}
