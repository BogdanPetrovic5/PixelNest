import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { StoryDto } from 'src/app/core/dto/story.dto';
import { StoryService } from 'src/app/core/services/story/story.service';

@Component({
  selector: 'app-story',
  templateUrl: './story.component.html',
  styleUrls: ['./story.component.scss']
})
export class StoryComponent implements OnInit{
  username:String = ""
  stories:StoryDto[] = []
  constructor(
    private _storyService:StoryService,
    private _cookieService:CookieService

  ){}  
  ngOnInit(): void {
    this.username = this._cookieService.get("username");
    console.log(this.username)
    this._storyService.getStories(this.username).subscribe({
      next:response=>{
        this.stories = response
        
      }
    })      
  }
}
