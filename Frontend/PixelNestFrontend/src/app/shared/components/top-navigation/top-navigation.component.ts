import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-top-navigation',
  templateUrl: './top-navigation.component.html',
  styleUrls: ['./top-navigation.component.scss']
})
export class TopNavigationComponent {
    constructor(private _router:Router){}

    navigate(route:string){
      this._router.navigate([`/${route}`])
    }
}
