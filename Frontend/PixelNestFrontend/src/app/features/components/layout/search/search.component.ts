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
      tap((users)=>console.log(users))
    ).subscribe(
      (users)=>(
        this.users = users
        
      )
    )
  }
  focus:boolean = false;
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
    this._router.navigate([`Profile/${username}`])
  }
}
