import {UserDto} from '../../../shared/dto/user/user.dto';

export interface ChatDto {
  id: string;
  chatMember1: UserDto;
  chatMember2: UserDto;
}
