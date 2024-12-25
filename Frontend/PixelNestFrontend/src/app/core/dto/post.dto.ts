import { CommentDto } from "./comment.dto";
import { LikedByUsers } from "./likedByUsers.dto";
import { SavedPosts } from "./savePost.dto";

export interface PostDto {
    postID: number;
    userID: number;
    ownerUsername: string;
    postDescription: string;
    totalComments: number;
    isDeletable:boolean;
    totalLikes: number;
    likedByUsers:LikedByUsers[];
    allComments:CommentDto[];
    imagePaths: ImagePathDto[];
    publishDate:Date;
    savedByUsers:SavedPosts[];
    location:string;
}
  
export interface ImagePathDto {
    path: string;
    photoDisplay:string;
}
  