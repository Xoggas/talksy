import {Component} from '@angular/core';
import {Sidebar} from '../../../shared/components/sidebar/sidebar';
import {RegisterForm} from '../components/register-form/register-form';
import {Router} from '@angular/router';

@Component({
  selector: 'app-register-page',
  imports: [Sidebar, RegisterForm],
  templateUrl: './register-page.html',
  styleUrl: './register-page.css'
})
export class RegisterPage {
  constructor(private router: Router) {
  }

  onSuccessfulRegistration(jwtToken: string): void {
    localStorage.setItem('token', jwtToken);
    this.router.navigate(['/chats']).then();
  }

  onFailedRegistration(err: any): void {
    console.log("Failed", err);
  }
}
