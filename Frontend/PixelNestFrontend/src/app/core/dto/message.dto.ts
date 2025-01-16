export interface Message{
    message:string;
    receiver:string;
    sender:string;
    roomID:string;
    dateSent:Date;
    source:string;
    messageID:number;
    isSeen:boolean;
}