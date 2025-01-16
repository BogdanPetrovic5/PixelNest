import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ChatStateService } from 'src/app/core/services/states/chat-state.service';

@Component({
  selector: 'app-inbox-icon',
  templateUrl: './inbox-icon.component.html',
  styleUrls: ['./inbox-icon.component.scss']
})
export class InboxIconComponent implements OnInit{
  constructor(
    private _router:Router,
    private _chatState:ChatStateService
  ){}

    newMessages:number = 0;
    navigate(route:string){
      this._router.navigate([`/${route}`])
    }
    ngOnInit(): void {
      this._chatState.updateNewMessages$.subscribe({
        next:response=>{
          this.newMessages = response
        }
      })
    }
}
