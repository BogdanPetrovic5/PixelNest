import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PostDto } from 'src/app/core/dto/post.dto';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';
import { UserService } from 'src/app/core/services/user/user.service';

@Component({
  selector: 'app-user-tag',
  templateUrl: './user-tag.component.html',
  styleUrls: ['./user-tag.component.scss']
})
export class UserTagComponent implements OnInit{
  @Input() post?:PostDto
  @Input() username?:string | any 
  @Input() date?:Date
  @Input() location?:string 
  @Input() clientGuid!:string
  formattedDate:string = ""
  stringUrl:string | undefined = undefined
  constructor(
    private _router:Router,
    private _dashboardState:DashboardStateService,
    private _datePipe:DatePipe,
    private _userService:UserService
  ){}
  ngOnInit(): void {
    // this._userService.getProfilePicture(this.username).subscribe({
    //   next:response=>{
    //     this.stringUrl = response.path;
        
    //   }
    // })
  
    this._formatDate()
    
  }
  navigateToUserProfile(username:string){

    this._router.navigate([`/Profile/${username}`])
  }

  navigate(url:string){
    this._router.navigate([`Location/${url}`])
    this._dashboardState.setNewLocation(url);
  }

  private _formatDate(){
    if(this.date){
      const date = this.date;
      const dateObject = new Date(date)
      if (isNaN(dateObject.getTime())) {
        this.formattedDate = 'Invalid Date';
      } else {
        this.formattedDate = this._datePipe.transform(dateObject, 'd MMM \'at\' HH:mm') || 'Invalid Format';
      }
    }
      
  }
}
