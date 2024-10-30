import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StoryDto } from '../../dto/story.dto';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class StoryService {

  constructor(private _httpClient:HttpClient) { }

  getStories(username:String):Observable<StoryDto[]>{
    const url = `${environment.apiUrl}/api/Story/GetStories?username=${username}`
    return this._httpClient.get<StoryDto[]>(url);
  }

  publishStory(formData:FormData):Observable<any>{
    const url = `${environment.apiUrl}/api/Story/PublishStory`;
    return this._httpClient.post(url, formData);
  }
}
