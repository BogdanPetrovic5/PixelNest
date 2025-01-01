import { Component, Input, OnInit } from '@angular/core';
import { UserService } from 'src/app/core/services/user/user.service';

@Component({
  selector: 'app-profile-image',
  templateUrl: './profile-image.component.html',
  styleUrls: ['./profile-image.component.scss']
})
export class ProfileImageComponent implements OnInit{
   @Input() stringUrl:string = ""
   @Input() username:string = ""

  constructor(private _userService:UserService){

  }
   ngOnInit(): void {
      this._userService.getProfilePicture(this.username).subscribe({next:response=>{
        
        if(response.path.length > 0){
          this.stringUrl = 'http://localhost:7157/Photos/' + response.path;
        }
       
        
      }}) 
   }

}
