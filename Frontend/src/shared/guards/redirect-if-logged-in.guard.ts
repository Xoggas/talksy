import {Injectable} from '@angular/core';
import {CanActivate, Router} from '@angular/router';
import {AuthService} from '../services/auth.service';

@Injectable({providedIn: 'root'})
export class RedirectIfLoggedInGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {
  }

  async canActivate(): Promise<boolean> {
    if (this.authService.isAuthorized()) {
      await this.router.navigate(['/chats']);
      return false;
    } else {
      return true;
    }
  }
}
