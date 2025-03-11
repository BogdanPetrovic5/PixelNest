import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FollowingsDto } from 'src/app/core/dto/followings.dto';
import { UserService } from 'src/app/core/services/user/user.service';

@Component({
  selector: 'app-followings',
  templateUrl: './followings.component.html',
  styleUrls: ['./followings.component.scss']
})
export class FollowingsComponent implements OnInit{
  @Input() clientGuid!:string;
  @Output() closeFollowingTab: EventEmitter<void> = new EventEmitter<void>();
  constructor(private _userService:UserService){}

  followings:FollowingsDto[] = []
  ngOnInit(): void {
      this._loadFollowings();
  }
  
  close(){
    this.closeFollowingTab.emit()
  }
  private _loadFollowings(){
    this._userService.getFollowings(this.clientGuid).subscribe({
      next:response=>{
       console.log(response)
        this.followings = response
      },
      error:error=>{
        console.log(error)
      }
    })
  }
 
}
