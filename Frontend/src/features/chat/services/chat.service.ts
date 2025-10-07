import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {ChatDto} from '../dto/chat.dto';
import {HttpClient} from '@angular/common/http';
import {MessageDto} from '../dto/message.dto';
import {MessageCreateDto} from '../dto/message-create.dto';
import {UserDto} from '../../../shared/dto/user/user.dto';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  constructor(private httpClient: HttpClient) {
  }

  getChats(): Observable<ChatDto[]> {
    return this.httpClient.get<ChatDto[]>('chat');
  }

  createChat(receiverId: string): Observable<ChatDto> {
    return this.httpClient.post<ChatDto>(`chat/${receiverId}`, {});
  }

  getMessagesFromChat(chatId: string): Observable<MessageDto[]> {
    return this.httpClient.get<MessageDto[]>(`chat/message/${chatId}`);
  }

  getLastMessageFromChat(chatId: string): Observable<MessageDto> {
    return this.httpClient.get<MessageDto>(`chat/message/${chatId}/last`);
  }

  sendMessage(dto: MessageCreateDto): Observable<MessageDto> {
    return this.httpClient.post<MessageDto>(`chat/message`, dto);
  }
}
