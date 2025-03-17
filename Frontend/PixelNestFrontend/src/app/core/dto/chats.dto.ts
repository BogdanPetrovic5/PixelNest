import { Message } from "./message.dto";

export interface Chats{
    chatID:string;
    userID:string;
    messages:Message[];
}