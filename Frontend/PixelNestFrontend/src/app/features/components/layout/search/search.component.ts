import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { debounceTime, distinctUntilChanged, Subject, switchMap, tap } from 'rxjs';
import { Users } from 'src/app/core/dto/users.dto';
import { UserService } from 'src/app/core/services/user/user.service';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit{
  private _searchTerms = new Subject<string>();
  focus:boolean = false;
  isInit:boolean = false;
  users:Users[] = [];
  
  constructor(
    private _userService:UserService,
    private _router:Router
  ){

  }
  ngOnInit(): void {
    this._searchTerms.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      
      switchMap((term:string)=>
        this._userService.findUsers(term)
      ),
      tap((users)=> {
        
      })
    ).subscribe(
      (users)=>{
        if(this.users.length > 0){
          setTimeout(() => {
           
            this._modifyUsers(users);
          }, 500);
         
        }else this.users = users;
        
       
      }
    )
  }
  private _modifyUsers(users:Users[]){
    for (let i = this.users.length - 1; i >= 0; i--) {
    
      if (!users.some(user => user.username === this.users[i].username)) {
        this.users[i].anim = true
        setTimeout(() => {
          this.users.splice(i, 1);
          this.users[i].anim = false
        }, 300);
       
      }
    }
    users.forEach(user => {
      if (!this.users.some(existingUser => existingUser.username === user.username)) {
        this.users.push({ ...user, anim: false });
      }
    });
  }
  onBlur() {
    this.focus = false;
  }
  onFocus() {
    this.focus = true;
  }

  onSearch(event: Event){
    const input = event.target as HTMLInputElement;
    
    if(input.value.length > 0){
      this._searchTerms.next(input.value)
    }
    
  }
  navigate(username:string){
    this._router.navigate([`profile/${username}`])
  }
}
