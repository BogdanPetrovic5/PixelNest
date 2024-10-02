import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { DashboardStateService } from 'src/app/core/services/states/dashboard-state.service';

@Component({
  selector: 'app-likes',
  templateUrl: './likes.component.html',
  styleUrls: ['./likes.component.scss']
})
export class LikesComponent implements OnInit{

  @Input() likes:any

  @Output() closeLikesTab: EventEmitter<void> = new EventEmitter<void>();
  constructor(
    private _dashboardState:DashboardStateService,
    private _router:Router
  ){}
  ngOnInit(): void {
    console.log(this.likes);
  }
  close(){
    this.closeLikesTab.emit()
  }
  navigateToProfile(username:string){
    this._router.navigate([`/Profile/${username}`])
  }
}
