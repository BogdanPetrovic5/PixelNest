import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { NotificationDto } from '../../dto/notification.dto';
import { NotificationOpened } from '../../dto/notificationOpened.dto';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private _httpClient:HttpClient) { }
  
  getNotifications():Observable<NotificationDto[]>{
    const url = `${environment.apiUrl}/api/notification/notifications`

    return this._httpClient.get<NotificationDto[]>(url);
  }
  countNotifications():Observable<number>{
    const url = `${environment.apiUrl}/api/notification/unread/count`
    return this._httpClient.get<number>(url);
  }
  markAsOpened(markAsRead:NotificationOpened):Observable<boolean>{
    const url = `${environment.apiUrl}/api/notification/open`
    return this._httpClient.post<boolean>(url, markAsRead)
  }
}
