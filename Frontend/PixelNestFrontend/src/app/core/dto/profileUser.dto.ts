export interface ProfileUser{
    followings:number;
    followers:number;
    name:string;
    lastname:string;
    username:string;
    totalPosts:number;
    clientGuid:string;
    canFollow:boolean;
    canEdit:boolean;
    chatID:string,
    profileImagePath:string,
    email:string
}