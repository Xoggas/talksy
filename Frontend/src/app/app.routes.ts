import {Routes} from '@angular/router';
import {RegisterPage} from '../features/register/views/register-page';
import {LoginPage} from '../features/login/views/login-page';
import {ChatsPage} from '../features/chat/views/chats-page';
import {AuthGuard} from '../shared/guards/auth.guard';
import {RedirectIfLoggedInGuard} from '../shared/guards/redirect-if-logged-in.guard';

export const routes: Routes = [
  {path: 'register', component: RegisterPage, title: 'Talksy', canActivate: [RedirectIfLoggedInGuard]},
  {path: 'login', component: LoginPage, title: 'Talksy', canActivate: [RedirectIfLoggedInGuard]},
  {path: 'chats', component: ChatsPage, title: 'Talksy', canActivate: [AuthGuard]},
  {path: '**', redirectTo: 'login'},
];
