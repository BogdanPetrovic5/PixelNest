import { LikedByUsers } from "./likedByUsers.dto";
import { Replies } from "./replies.dto";


export interface CommentDto {
    commentID?: number;
    commentText?: string;
    likedByUsers?: LikedByUsers[]; 
    parentCommentID?: number | null;
    postID?: number;
    replies?: Replies[] | [];
    totalLikes?: number;
    userID?: number;
    username: string;
    totalReplies?:number;
}
  
