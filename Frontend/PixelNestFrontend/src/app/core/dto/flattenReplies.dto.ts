
export interface FlattenReplies {
    commentID?: number;
    commentText?: string;
    likedByUsers?:[];
    parentCommentID?: number | null;
    postID?: number;
    replies?: [];
    totalLikes?: number;
    clientGuid: string;
    username: string;
    isReplyBox?:boolean;
}