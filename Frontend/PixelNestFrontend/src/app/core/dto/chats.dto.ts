import { Message } from "./message.dto";

export interface Chats{
    chatID:string;
    userID:string;
    username:string;
    messages:Message[];
}