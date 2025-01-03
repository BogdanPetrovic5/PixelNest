import { Component, Input, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ProfileStateService } from 'src/app/core/services/states/profile-state.service';
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
  constructor(private _userService:UserService,
    private _profileState:ProfileStateService
  ){

  }
   ngOnInit(): void {
      this.subscription.add(
        this._profileState.currentProfileUrl$.subscribe({
          next:response =>{
            this.stringUrl = response
          }
        })
      )
      this._userService.getProfilePicture(this.username).subscribe({next:response=>{
        if(response.path.length > 0){
          this.stringUrl = environment.blobStorageBaseUrl + response.path;
        }
      }}) 
   }

}
