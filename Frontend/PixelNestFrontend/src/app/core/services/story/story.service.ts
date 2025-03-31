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

  getStories(forCurrentUser:boolean):Observable<StoriesDto[]>{
    const url = `${environment.apiUrl}/api/Story/GetStories?forCurrentUser=${forCurrentUser}`
    return this._httpClient.get<StoriesDto[]>(url);
  }

  publishStory(formData:FormData):Observable<any>{
    const url = `${environment.apiUrl}/api/Story/PublishStory`;
    return this._httpClient.post(url, formData);
  }
  
  marStoryAsSeen(storyID:string):Observable<any>{
    const url = `${environment.apiUrl}/api/Story/MarkStoryAsSeen`;
    return this._httpClient.post(url, {
        StoryID:storyID,
       
    });
  }

  getViewers(storyID:string):Observable<ViewersDto[]>{
    const url = `${environment.apiUrl}/api/Story/GetViewers?storyID=${storyID}`;
    return this._httpClient.get<ViewersDto[]>(url);
  }
}
