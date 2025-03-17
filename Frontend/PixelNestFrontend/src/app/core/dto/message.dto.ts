export interface Message{
    message:string;
    receiver:string;
    sender:string;
    roomID:string;
    dateSent:Date;
    source:string;
    userID:string;
    messageID:number;
    isSeen:boolean;
}