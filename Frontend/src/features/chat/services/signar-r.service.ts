import {Injectable} from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {MessageDto} from '../dto/message.dto';
import {Subject} from 'rxjs';
import {ChatDto} from '../dto/chat.dto';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;

  private pendingGotNewMessageSubject = new Subject<MessageDto>();
  public gotNewMessage = this.pendingGotNewMessageSubject.asObservable();

  private pendingChatCreatedSubject = new Subject<ChatDto>();
  public chatCreated = this.pendingChatCreatedSubject.asObservable();

  constructor() {
    const token = localStorage.getItem('token');

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}/chat-hub`, {
        accessTokenFactory: () => token || ''
      })
      .withAutomaticReconnect()
      .build();
  }

  public startConnection = () => {
    this.hubConnection
      .start()
      .then(() => console.log('SignalR Connection started'))
      .catch(err => console.log('Error establishing SignalR connection: ' + err));

    this.hubConnection.on('GotNewMessage', this.onGotNewMessage.bind(this));
    this.hubConnection.on('ChatCreated', this.onChatCreated.bind(this));
  }

  private onGotNewMessage = (message: MessageDto) => {
    this.pendingGotNewMessageSubject.next(message);
  }

  private onChatCreated = (chat: ChatDto) => {
    this.pendingChatCreatedSubject.next(chat);
  }
}
