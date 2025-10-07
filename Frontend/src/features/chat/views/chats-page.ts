import {Component, OnInit, signal} from '@angular/core';
import {RegisterForm} from "../../register/components/register-form/register-form";
import {Sidebar} from "../../../shared/components/sidebar/sidebar";
import {ChatListComponent} from '../components/chat-list/chat-list.component';
import {ChatBodyComponent} from '../components/chat-body/chat-body.component';
import {UserService} from '../services/user.service';
import {ChatDto} from '../dto/chat.dto';
import {UserDto} from '../../../shared/dto/user/user.dto';
import {SignalRService} from '../services/signar-r.service';

export enum Mode {
  users,
  chats
}

@Component({
  selector: 'app-chats-page',
  imports: [
    ChatListComponent,
    ChatBodyComponent
  ],
  templateUrl: './chats-page.html',
  styleUrl: './chats-page.css'
})
export class ChatsPage implements OnInit {
  constructor(private userService: UserService, private signalrService: SignalRService) {
  }

  userId = signal<string>("");
  mode = signal<Mode>(Mode.users);
  selectedChat?: ChatDto;

  ngOnInit(): void {
    this.userService
      .whoAmI()
      .subscribe(user => {
        this.userId.set(user.id);
        this.signalrService.startConnection();
      });
  }

  onChatSelected(chat: ChatDto): void {
    this.selectedChat = chat;
    this.mode.set(Mode.chats);
  }

  onBackButtonPressed(): void {
    this.mode.set(Mode.users);
  }

  protected readonly Mode = Mode;
}

