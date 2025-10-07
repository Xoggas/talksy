import {Injectable} from '@angular/core';
import {CanActivate, Router} from '@angular/router';
import {AuthService} from '../services/auth.service';

@Injectable({providedIn: 'root'})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {
  }

  async canActivate(): Promise<boolean> {
    if (this.authService.isAuthorized()) {
      return true;
    } else {
      await this.router.navigate(['/login']);
      return false;
    }
  }
}
