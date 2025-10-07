import {Component, HostBinding, Input, input} from '@angular/core';
import {ChatDto} from '../../dto/chat.dto';
import {MessageDto} from '../../dto/message.dto';

@Component({
  selector: 'app-chat-list-item',
  imports: [],
  templateUrl: './chat-list-item.component.html',
  styleUrl: './chat-list-item.component.css'
})
export class ChatListItemComponent {
  userId = input.required<string>();
  chat = input.required<ChatDto>();
  lastMessage = input<MessageDto>();

  @HostBinding('class.active')
  @Input()
  isSelected = false;

  get otherPersonName(): string {
    if (this.chat().chatMember1.id == this.userId()) {
      return this.chat().chatMember2.name;
    }
    else {
      return this.chat().chatMember1.name;
    }
  }

  get initials(): string {
    return this.otherPersonName
      .split(' ')
      .map(x => x[0].toUpperCase())
      .slice(0, 2)
      .join('');
  }
}
