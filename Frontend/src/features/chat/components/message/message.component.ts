import {Component, ElementRef, HostBinding, input, OnInit, Renderer2} from '@angular/core';
import {MessageDto} from '../../dto/message.dto';

@Component({
  selector: 'app-message',
  imports: [],
  templateUrl: './message.component.html',
  styleUrl: './message.component.css'
})
export class MessageComponent implements OnInit {
  userId = input.required<string>();
  message = input.required<MessageDto>();

  constructor(private el: ElementRef, private renderer: Renderer2) {
  }

  ngOnInit() {
    if (this.message().sender.id === this.userId()) {
      this.renderer.addClass(this.el.nativeElement, 'sent-by-you');
    }
    else {
      this.renderer.addClass(this.el.nativeElement, 'sent-by-other');
    }
  }

  get initials(): string {
    return this.message()
      .sender.name
      .split(' ')
      .map(x => x[0].toUpperCase())
      .slice(0, 2)
      .join('');
  }
}
