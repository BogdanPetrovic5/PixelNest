﻿using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;

namespace PixelNestBackend.Interfaces
{
    public interface ICommentRepository
    {
        ICollection<ResponseCommentDto> GetComments(int postID);
        ICollection<ResponseReplyCommentDto> GetReplies(int? initialParentID);
        bool LikeComment(int userID, LikeCommentDto likeCommentDto, bool isDuplicate);
    }
}
