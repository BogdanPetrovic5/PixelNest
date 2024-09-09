export interface PostDto {
    postID: number;
    userID: number;
    ownerUsername: string;
    postDescription: string;
    totalComments: number;
    totalLikes: number;
    imagePaths: ImagePathDto[];
    publishDate:Date;
    comments: CommentDto[];
}
  
export interface ImagePathDto {
    path: string;
    photoDisplay:string;
}
  
export interface CommentDto {
    commentText: string;
    totalLikes: number;
    userID: number;
    username: string;
}