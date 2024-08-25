import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.scss']
})
export class RegisterFormComponent {
  constructor(private _router:Router){}
  navigateToLogin(){
    this._router.navigate(['/Authentication/Login'])
  }
}
