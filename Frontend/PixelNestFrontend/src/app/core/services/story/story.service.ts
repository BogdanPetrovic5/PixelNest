import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StoryDto } from '../../dto/story.dto';
import { environment } from 'src/environments/environment.development';
import { StoriesDto } from '../../dto/stories.dto';

@Injectable({
  providedIn: 'root'
})
export class StoryService {

  constructor(private _httpClient:HttpClient) { }

  getStories(username:String):Observable<StoriesDto[]>{
    const url = `${environment.apiUrl}/api/Story/GetStories?username=${username}`
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
}
