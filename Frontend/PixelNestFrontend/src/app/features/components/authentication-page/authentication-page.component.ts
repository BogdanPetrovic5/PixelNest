import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-authentication-page',
  templateUrl: './authentication-page.component.html',
  styleUrls: ['./authentication-page.component.scss']
})
export class AuthenticationPageComponent implements OnInit{
  subscriptions: Subscription[] = [];
  defaultRoute:string = 'Register'
  constructor(private _route:ActivatedRoute){}

  ngOnInit():void{
   
  }

}
