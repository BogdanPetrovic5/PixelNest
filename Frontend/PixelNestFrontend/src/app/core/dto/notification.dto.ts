import { ImagePathDto } from "./imagePath.dto";

export interface NotificationDto{
    username:string;
    message:string;
    clientGuid:string;
    date:Date;
    postID:number;
    notificationID:number;
    imagePath:ImagePathDto[];
}