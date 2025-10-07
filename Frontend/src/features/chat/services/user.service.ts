import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {UserDto} from '../../../shared/dto/user/user.dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private httpClient: HttpClient) {
  }

  whoAmI(): Observable<UserDto> {
    return this.httpClient.get<UserDto>('user/whoami');
  }

  findUsersByUsername(username: string): Observable<UserDto[]> {
    return this.httpClient.get<UserDto[]>(`user/find/${username}`);
  }
}
