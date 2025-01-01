import { LikedByUsers } from "./likedByUsers.dto";

  
export interface Replies {
    commentID?: number;
    commentText?: string;
    likedByUsers?: LikedByUsers[];
    parentCommentID?: number;
    postID?: number;
    replies: Replies[] | [];
    totalLikes?: number;
    userID?: number;
    username: string;
}
  