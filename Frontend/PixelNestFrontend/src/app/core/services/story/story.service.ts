import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StoryDto } from '../../dto/story.dto';
import { environment } from 'src/environments/environment.development';
import { StoriesDto } from '../../dto/stories.dto';
import { ViewersDto } from '../../dto/viewers.dto';

@Injectable({
  providedIn: 'root'
})
export class StoryService {

  constructor(private _httpClient:HttpClient) { }

  getStories(username:String, forCurrentUser:boolean):Observable<StoriesDto[]>{
    const url = `${environment.apiUrl}/api/Story/GetStories?username=${username}&forCurrentUser=${forCurrentUser}`
    return this._httpClient.get<StoriesDto[]>(url);
  }

  publishStory(formData:FormData):Observable<any>{
    const url = `${environment.apiUrl}/api/Story/PublishStory`;
    return this._httpClient.post(url, formData);
  }
  
  marStoryAsSeen(storyID:number, username:string):Observable<any>{
    const url = `${environment.apiUrl}/api/Story/MarkStoryAsSeen`;
    return this._httpClient.post(url, {
        StoryID:storyID,
        Username:username
    });
  }

  getViewers(username:string, storyID:number):Observable<ViewersDto[]>{
    const url = `${environment.apiUrl}/api/Story/GetViewers?username=${username}&storyID=${storyID}`;
    return this._httpClient.get<ViewersDto[]>(url);
  }
}
