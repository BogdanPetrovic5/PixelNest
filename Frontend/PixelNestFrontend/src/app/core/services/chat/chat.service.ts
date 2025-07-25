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
    const url = `${environment.apiUrl}/api/chat/unread-messages`

    return this._httpClient.get<any>(url);
  }
  getMessages(chatID:string):Observable<Message[]>{
    const url = `${environment.apiUrl}/api/chat/${chatID}`

    return this._httpClient.get<Message[]>(url);
  }
  sendMessage(message:Message):Observable<any>{
      const url =`${environment.apiUrl}/api/chat/message/send`

      return this._httpClient.post(url, {
        Message:message.message,
        ClientGuid:message.receiver
      })
  }

  markAsRead(messageSeenDto:MessageSeen){
    const url = `${environment.apiUrl}/api/chat/message/read`
    return this._httpClient.post(url, messageSeenDto)
  }

  getChats():Observable<Chats[]>{
    const url = `${environment.apiUrl}/api/chat/chats`

    return this._httpClient.get<Chats[]>(url)
  }

  joinRoom(targetClientGuid:string):Observable<any>{
    const url = `${environment.apiUrl}/api/chat/room/join/${targetClientGuid}`
    return this._httpClient.post(url,{})
  }
  leaveRoom(targetClientGuid:string):Observable<any>{
    const url = `${environment.apiUrl}/api/chat/room/leave/${targetClientGuid}`
    return this._httpClient.post(url,{})
  }
  findChats(searchParameter:string):Observable<Chats[]>{
    const url = `${environment.apiUrl}/api/chat/search?searchParameter=${searchParameter}`
    return this._httpClient.get<Chats[]>(url);
  }
  deleteForMe(messageID:number):Observable<boolean>{
    const url = `${environment.apiUrl}/api/chat/message/${messageID}/delete-for-me`;
    return this._httpClient.patch<boolean>(url,{});
  }
  unsend(messageID:number):Observable<boolean>{
    const url = `${environment.apiUrl}/api/chat/message/${messageID}/unsend`;
    return this._httpClient.patch<boolean>(url,{});
  }
}
