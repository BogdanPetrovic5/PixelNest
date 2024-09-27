import { Component, OnInit } from '@angular/core';
import { PostDto } from 'src/app/core/dto/post.dto';
import { ProfileUser } from 'src/app/core/dto/profileUser.dto';
import { PostService } from 'src/app/core/services/post/post.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { UserService } from 'src/app/core/services/user/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit{
  username:string = ""
  user!:ProfileUser

  posts:PostDto[] = []
  constructor(
    private _userSessions:UserSessionService,
    private _userService:UserService,
    private _postService:PostService
  ){}
  ngOnInit(): void {
    this._initilizeApp()
    this.loadData()
  }
  loadData(){
    this._userService.getUserData(this.username).subscribe({
      next:response=>{
        console.log(response);
        this.user = response
      },
      error: error => {
        console.error('An error occurred:', error);
      }
    })

    this._postService.getPostsByUsername(this.username).subscribe({
      next:response=>{
        console.log(response)
        this.posts = response
      },
      error:error=>{
        console.log(error);
      }
    })
  }
  private _initilizeApp(){
    this.username = this._userSessions.getFromCookie("profileUsername")
  }
  
}
