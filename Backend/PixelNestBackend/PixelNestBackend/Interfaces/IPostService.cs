﻿using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IPostService
    {

        Task<PostResponse> PublishPost(PostDto postDto);
        Task<ICollection<ResponsePostDto>> GetPosts(string? username, string? location, string email);
        Task<ResponsePostDto> GetSinglePost(Guid postID, string email);
        bool SavePost(SavePostDto savePostDto);
        bool CacheChange(string email);
        PostResponse LikePost(LikeDto likeDto);
        PostResponse Comment(CommentDto commentDto);
        Task<DeleteResponse> DeletePost(string email, Guid postID);
        bool CheckIntegrity(string email, Guid postID);
    }
}
