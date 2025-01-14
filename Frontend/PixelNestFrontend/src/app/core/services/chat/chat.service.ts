import { Injectable } from '@angular/core';
import { Message } from '../../dto/message.dto';
import { environment } from 'src/environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Chats } from '../../dto/chats.dto';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  
  constructor(private _httpClient:HttpClient) { }

  getMessages(username:string):Observable<Message[]>{
    const url = `${environment.apiUrl}/api/Chat/GetUserToUserMessages?targetUsername=${username}`

    return this._httpClient.get<Message[]>(url);
  }
  sendMessage(message:Message):Observable<any>{
      const url =`${environment.apiUrl}/api/Chat/SendMessage`

      return this._httpClient.post(url, {
        Message:message.message,
        SenderUsername:message.sender,
        ReceiverUsername:message.receiver
      })
  }

  getChats():Observable<Chats[]>{
    const url = `${environment.apiUrl}/api/Chat/GetUserChats`

    return this._httpClient.get<Chats[]>(url)
  }

  joinRoom(receiverUsername:string):Observable<any>{
    const url = `${environment.apiUrl}/api/Chat/JoinRoom?receiverUsername=${receiverUsername}`
    return this._httpClient.post(url,{})
  }
}
