import { LikedByUsers } from "./likedByUsers.dto";
import { Replies } from "./replies.dto";


export interface CommentDto {
    commentID?: number;
    commentText?: string;
    likedByUsers?: LikedByUsers[]; 
    parentCommentID?: number | null;
    postID?: string;
    replies?: Replies[] | [];
    totalLikes?: number;
    clientGuid:string
    username: string;
    totalReplies?:number;
}
  
