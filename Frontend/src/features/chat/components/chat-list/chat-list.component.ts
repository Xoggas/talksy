import {ChangeDetectorRef, Component, effect, HostBinding, Input, input, OnInit, output, signal} from '@angular/core';
import {ChatService} from '../../services/chat.service';
import {ChatDto} from '../../dto/chat.dto';
import {MessageDto} from '../../dto/message.dto';
import {catchError, forkJoin, map, of, switchMap} from 'rxjs';
import {ChatListItemComponent} from '../chat-list-item/chat-list-item.component';
import {UserDto} from '../../../../shared/dto/user/user.dto';
import {UserService} from '../../services/user.service';
import {UserListItemComponent} from '../user-list-item/user-list-item.component';
import {FormsModule} from '@angular/forms';
import {SignalRService} from '../../services/signar-r.service';

@Component({
  selector: 'app-chat-list',
  imports: [
    ChatListItemComponent,
    UserListItemComponent,
    FormsModule
  ],
  templateUrl: './chat-list.component.html',
  styleUrl: './chat-list.component.css'
})
export class ChatListComponent implements OnInit {
  @HostBinding('class.is-active')
  @Input()
  isActive: boolean = true;

  userId = input.required<string>();
  searchString = signal<string>("");
  chatSelected = output<ChatDto>();

  constructor(private chatService: ChatService,
              private cd: ChangeDetectorRef,
              private userService: UserService,
              private signalrService: SignalRService) {
    effect(() => {
      const username = this.searchString().trim();
      if (username) {
        this.userService.findUsersByUsername(username).subscribe(users => {
          this.foundUsers = users.filter(user => user.id !== this.userId());
          this.cd.markForCheck();
        });
      }
    });
  }

  chats: ChatDto[] = [];
  lastMessages: Map<ChatDto, MessageDto> = new Map<ChatDto, MessageDto>();
  selectedChat?: ChatDto;
  foundUsers: UserDto[] = [];

  ngOnInit(): void {
    this.chatService
      .getChats()
      .pipe(
        switchMap(chats => {
          const requests = chats.map(chat =>
            this.chatService.getLastMessageFromChat(chat.id)
              .pipe(
                catchError(_ => of(null)),
                map(message => [chat, message] as [ChatDto, MessageDto | null])
              )
          );

          return forkJoin(requests);
        }))
      .subscribe(results => {
        this.chats = results.map(result => result[0]);
        this.lastMessages = new Map(results.map(result => [result[0], result[1]] as [ChatDto, MessageDto]));
        this.cd.markForCheck();
      });

    this.signalrService.chatCreated.subscribe(chat => {
      if (chat.chatMember2.id == this.userId()) {
        this.chats.push(chat);
        this.cd.markForCheck();
      }
    });

    this.signalrService.gotNewMessage.subscribe(message => {
      const chat = this.chats.find(chat => chat.id === message.chat.id);
      if (chat) {
        this.lastMessages.set(chat, message);
        this.cd.markForCheck();
      }
    });
  }

  openOrCreateChat(userDto: UserDto) {
    const existingChat = this.chats.find(chat =>
      chat.chatMember1.id === userDto.id || chat.chatMember2.id === userDto.id);

    if (existingChat) {
      this.onChatSelected(existingChat);
    }
    else {
      this.chatService.createChat(userDto.id).subscribe(chat => {
        this.chats.push(chat);
        this.cd.markForCheck();
        this.onChatSelected(chat);
      });
    }

    this.searchString.set("");
  }

  onChatSelected(chat: ChatDto) {
    this.chatSelected.emit(chat);
    this.selectedChat = chat;
  }
}
