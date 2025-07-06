import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { ProfileStateService } from 'src/app/core/services/states/profile-state.service';
import { UserSessionService } from 'src/app/core/services/user-session/user-session.service';
import { UserService } from 'src/app/core/services/user/user.service';


@Component({
  selector: 'app-profile-image',
  templateUrl: './profile-image.component.html',
  styleUrls: ['./profile-image.component.scss']
})
export class ProfileImageComponent implements OnInit{
   @Input() stringUrl:string = ""
   @Input() clientGuid:string = ""
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
   
    if (changes['clientGuid']) {
      const newClientID = changes['clientGuid'].currentValue;
      if(newClientID) this._loadProfilePicture(newClientID);
      
    }
  }
  ngOnDestroy():void{
    this.subscription.unsubscribe();
  }
   ngOnInit(): void {
  
      if(this.clientGuid == this._userSession.getFromCookie("sessionID")){
        this.subscription.add(
          this._profileState.currentProfileUrl$.subscribe({
            next:response =>{
              this.stringUrl = response
           
            }
          })
        )
      }
   }

   navigateToProfile(){
    this._router.navigate([`profile/${this.clientGuid}/${this.username}`])
   }

   private _loadProfilePicture(clientGuid:string){
    this._userService.getProfilePicture(clientGuid).subscribe({next:response=>{
      if(response.path.length > 0){
        if(!response.path.includes("https")) this.stringUrl = "http://localhost:7157/Photos/" + response.path;
        else this.stringUrl = response.path
       
        
      }else this.stringUrl = "/assets/images/user.png"
    }}) 
   }
}
