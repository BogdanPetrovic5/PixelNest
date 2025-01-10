import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ProfileStateService } from 'src/app/core/services/states/profile-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { UserService } from 'src/app/core/services/user/user.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-profile-image',
  templateUrl: './profile-image.component.html',
  styleUrls: ['./profile-image.component.scss']
})
export class ProfileImageComponent implements OnInit{
   @Input() stringUrl:string = ""
   @Input() username:string = ""
  subscription:Subscription = new Subscription;
  constructor(
    private _userService:UserService,
    private _profileState:ProfileStateService,
    private _router:Router,
    private _userSession:UserSessionService
  ){

  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['username']) {
      const newUsername = changes['username'].currentValue;
      this._loadProfilePicture(newUsername);
    }
  }
  ngOnDestroy():void{
    this.subscription.unsubscribe();
  }
   ngOnInit(): void {
      if(this.username == this._userSession.getFromCookie("username")){
        this.subscription.add(
          this._profileState.currentProfileUrl$.subscribe({
            next:response =>{
              this.stringUrl = response
            }
          })
        )
      }
      
      // this._userService.getProfilePicture(this.username).subscribe({next:response=>{
      //   if(response.path.length > 0){
      //     this.stringUrl = environment.blobStorageBaseUrl + response.path;
      //   }
      // }}) 
   }

   navigateToProfile(){
    this._router.navigate([`Profile/${this.username}`])
   }

   private _loadProfilePicture(username:string){
    
    this._userService.getProfilePicture(username).subscribe({next:response=>{
      if(response.path.length > 0){
        this.stringUrl = environment.blobStorageBaseUrl + response.path;
      }
    }}) 
   }
}
