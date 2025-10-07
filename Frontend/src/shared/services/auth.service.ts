import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {UserLoginDto} from '../dto/user/user-login.dto';
import {UserRegisterDto} from '../dto/user/user-register.dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private readonly httpClient: HttpClient) {
  }

  isAuthorized(): boolean {
    return localStorage.getItem('token') !== null;
  }

  login(dto: UserLoginDto): Observable<Jwt> {
    return this.httpClient.post<Jwt>('user/login', dto);
  }

  register(dto: UserRegisterDto): Observable<Jwt> {
    return this.httpClient.post<Jwt>('user/register', dto);
  }
}

export interface Jwt {
  token: string;
}
