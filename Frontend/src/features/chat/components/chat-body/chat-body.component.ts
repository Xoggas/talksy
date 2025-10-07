import {ChangeDetectorRef, Component, effect, HostBinding, Input, input, OnInit, output, signal} from '@angular/core';
import {MessageDto} from '../../dto/message.dto';
import {ChatService} from '../../services/chat.service';
import {ChatDto} from '../../dto/chat.dto';
import {MessageComponent} from '../message/message.component';
import {FormsModule} from '@angular/forms';
import {MessageCreateDto} from '../../dto/message-create.dto';
import {SignalRService} from '../../services/signar-r.service';

@Component({
  selector: 'app-chat-body',
  imports: [
    MessageComponent,
    FormsModule
  ],
  templateUrl: './chat-body.component.html',
  styleUrl: './chat-body.component.css'
})
export class ChatBodyComponent implements OnInit {
  @HostBinding('class.is-active')
  @Input()
  isActive: boolean = true;

  userId = input.required<string>();
  chat = input<ChatDto>();
  messageText = signal<string>("");
  backButtonPressed = output();

  constructor(private chatService: ChatService,
              private cd: ChangeDetectorRef,
              private signalrService: SignalRService) {
    effect(() => {
      this.chatService
        .getMessagesFromChat(this.chat()?.id ?? "")
        .subscribe({
          next: messages => {
            this.messages = messages;
            this.cd.markForCheck();
          },
          error: _ => {
            this.messages = [];
            this.cd.markForCheck();
          }
        });
    });
  }

  ngOnInit() {
    this.signalrService.gotNewMessage.subscribe(msg => {
      if (this.chat() == undefined || this.userId() == msg.sender.id) {
        return;
      }

      if (msg.chat.id == this.chat()?.id) {
        this.messages.push(msg);
        this.cd.markForCheck();
      }
    });
  }

  messages: MessageDto[] = [];

  get otherPersonName(): string {
    if (this.chat() == undefined) {
      return '';
    }

    if (this.chat()!.chatMember1.id == this.userId()) {
      return this.chat()!.chatMember2.name;
    }
    else {
      return this.chat()!.chatMember1.name;
    }
  }

  onKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.sendMessage();
    }
  }

  sendMessage() {
    const dto: MessageCreateDto = {
      chatId: this.chat()!.id,
      senderId: this.userId(),
      content: this.messageText()
    };

    this.messageText.set("");
    this.chatService.sendMessage(dto).subscribe(msg => {
      this.messages.push(msg);
      this.cd.markForCheck();
    });
  }
}
