import {ChatDto} from './chat.dto';
import {UserDto} from '../../../shared/dto/user/user.dto';

export interface MessageDto {
  id: string;
  sentAt: Date,
  chat: ChatDto;
  sender: UserDto;
  content: string;
}
