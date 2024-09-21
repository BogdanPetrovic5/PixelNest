namespace PixelNestBackend.Dto
{
    public class ResponseReplyCommentDto
    {
        public int CommentID { get; set; }
        public int TotalLikes { get; set; }
        public int UserID { get; set; }
        public string CommentText { get; set; }

        public string Username { get; set; }

        public int PostID { get; set; }
        public int TotalReplies { get; set; }
        public int? ParentCommentID { get; set; }
        public ICollection<LikeCommentDto>? LikedByUsers { get; set; }

        public ICollection<ResponseReplyCommentDto>? Replies { get; set; }
    }
}
