import { CommentDto } from "./comment.dto";
import { LikedByUsers } from "./likedByUsers.dto";

export interface PostDto {
    postID: number;
    userID: number;
    ownerUsername: string;
    postDescription: string;
    totalComments: number;
    totalLikes: number;
    likedByUsers:LikedByUsers[];
    allComments:CommentDto[];
    imagePaths: ImagePathDto[];
    publishDate:Date;
   
}
  
export interface ImagePathDto {
    path: string;
    photoDisplay:string;
}
  