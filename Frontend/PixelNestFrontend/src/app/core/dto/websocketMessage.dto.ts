export interface WebSocketMessage{
    Type: string;
    RoomID: string;
    TargetUser: string;
    Content: string;
    SenderUsername: string;
    SenderUser: string;
}