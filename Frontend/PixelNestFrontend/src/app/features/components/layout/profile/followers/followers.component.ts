import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FollowersDto } from 'src/app/core/dto/followers.dto';
import { UserService } from 'src/app/core/services/user/user.service';

@Component({
  selector: 'app-followers',
  templateUrl: './followers.component.html',
  styleUrls: ['./followers.component.scss']
})
export class FollowersComponent implements OnInit{
  followers:FollowersDto[]=[]
  @Input() username!:string;

  @Output() closeFollowersTab: EventEmitter<void> = new EventEmitter<void>();
  constructor(
    private _userService:UserService

  ){}  
  ngOnInit(): void {
    this._loadFollowers();  
  }
  close(){
    this.closeFollowersTab.emit();
  }
  private _loadFollowers(){
    
    this._userService.getFollowers(this.username).subscribe({
      next:response=>{
        this.followers = response
        console.log(this.followers);
      },
      error:error=>{
        console.log(error);
      }
    })
  }

}