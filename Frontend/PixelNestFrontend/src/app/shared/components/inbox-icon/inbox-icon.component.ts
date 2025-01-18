import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { ChatService } from 'src/app/core/services/chat/chat.service';
import { ChatStateService } from 'src/app/core/services/states/chat-state.service';

@Component({
  selector: 'app-inbox-icon',
  templateUrl: './inbox-icon.component.html',
  styleUrls: ['./inbox-icon.component.scss']
})
export class InboxIconComponent implements OnInit{
  private destroy$ = new Subject<void>();
  constructor(
    private _router:Router,
    private _chatState:ChatStateService,
    private _chatService:ChatService
  ){}

    newMessages:number = 0;
    navigate(route:string){
      this._router.navigate([`/${route}`])
    }
    ngOnInit(): void {
      this._chatState.updateNewMessages$
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          
          this.newMessages = response;
        },
        error: (err) => {
          console.error('Error updating new messages:', err);
        },
      });

      this._chatService.getNumberOfMessages().subscribe({
        next:response=>{
          this.newMessages = response.newMessages
        }
      })
    }
    ngOnDestroy(): void {
      
      this.destroy$.complete(); 
    }
}
