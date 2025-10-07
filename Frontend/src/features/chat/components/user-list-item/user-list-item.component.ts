import {Component, HostBinding, Input, input} from '@angular/core';
import {ChatDto} from '../../dto/chat.dto';
import {MessageDto} from '../../dto/message.dto';
import {UserDto} from '../../../../shared/dto/user/user.dto';

@Component({
  selector: 'app-user-list-item',
  imports: [],
  templateUrl: './user-list-item.component.html',
  styleUrl: './user-list-item.component.css'
})
export class UserListItemComponent {
  user = input.required<UserDto>();

  get initials(): string {
    return this.user().name
      .split(' ')
      .map(x => x[0].toUpperCase())
      .slice(0, 2)
      .join('');
  }
}
