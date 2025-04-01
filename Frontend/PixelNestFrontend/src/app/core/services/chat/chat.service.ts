import { Injectable } from '@angular/core';
import { Message } from '../../dto/message.dto';
import { environment } from 'src/environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Chats } from '../../dto/chats.dto';
import { MessageSeen } from '../../dto/messageSeen.dto';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  
  constructor(private _httpClient:HttpClient) { }
  getNumberOfMessages():Observable<any>{
    const url = `${environment.apiUrl}/api/Chat/GetNumberOfMessages`

    return this._httpClient.get<any>(url);
  }
  getMessages(chatID:string):Observable<Message[]>{
    const url = `${environment.apiUrl}/api/Chat/GetUserToUserMessages?chatID=${chatID}`

    return this._httpClient.get<Message[]>(url);
  }
  sendMessage(message:Message):Observable<any>{
      const url =`${environment.apiUrl}/api/Chat/SendMessage`

      return this._httpClient.post(url, {
        Message:message.message,
        ClientGuid:message.receiver
      })
  }

  markAsRead(messageSeenDto:MessageSeen){
    const url = `${environment.apiUrl}/api/Chat/MarkAsRead`
    return this._httpClient.post(url, messageSeenDto)
  }

  getChats():Observable<Chats[]>{
    const url = `${environment.apiUrl}/api/Chat/GetUserChats`

    return this._httpClient.get<Chats[]>(url)
  }

  joinRoom(targetClientGuid:string):Observable<any>{
    const url = `${environment.apiUrl}/api/Chat/JoinRoom?targetClientGuid=${targetClientGuid}`
    return this._httpClient.post(url,{})
  }
  leaveRoom(targetClientGuid:string):Observable<any>{
    const url = `${environment.apiUrl}/api/Chat/LeaveRoom?targetClientGuid=${targetClientGuid}`
    return this._httpClient.post(url,{})
  }
  findChats(searchParameter:string):Observable<Chats>{
    const url = `${environment.apiUrl}/api/Chat/FindChats?searchParameter=${searchParameter}`
    return this._httpClient.get<Chats>(url);
  }
}
