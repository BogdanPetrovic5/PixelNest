
export interface FlattenReplies {
    commentID?: number;
    commentText?: string;
    likedByUsers?:[];
    parentCommentID?: number | null;
    postID?: number;
    replies?: [];
    totalLikes?: number;
    userID?: number;
    username?: string;
    isReplyBox?:boolean;
}