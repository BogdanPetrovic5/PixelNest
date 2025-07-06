import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-typing-indicator',
  templateUrl: './typing-indicator.component.html',
  styleUrls: ['./typing-indicator.component.scss']
})
export class TypingIndicatorComponent {
    @Input() clientGuid:string = '';
    @Input() username:string = '';
}
