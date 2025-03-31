import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { LikedByUsers } from 'src/app/core/dto/likedByUsers.dto';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';

@Component({
  selector: 'app-likes',
  templateUrl: './likes.component.html',
  styleUrls: ['./likes.component.scss']
})
export class LikesComponent implements OnInit{

  @Input() likes:LikedByUsers[] = []

  @Output() closeLikesTab: EventEmitter<void> = new EventEmitter<void>();
  constructor(
    private _dashboardState:DashboardStateService,
    private _router:Router
  ){}
  ngOnInit(): void {
   
  }
  close(){
    this.closeLikesTab.emit()
  }
  navigateToProfile(username:string){
    this._router.navigate([`/Profile/${username}`])
  }
}
