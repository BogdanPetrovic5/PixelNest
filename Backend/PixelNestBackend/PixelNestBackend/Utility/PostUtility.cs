﻿using PixelNestBackend.Data;
using PixelNestBackend.Dto;

namespace PixelNestBackend.Utility
{
    public class PostUtility
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _dataContext;
        public PostUtility(IConfiguration configuration, DataContext dataContext) {
            _configuration = configuration;
            _dataContext = dataContext;
        }
        public bool FindDuplicate(Guid postID, Guid userID, string tableName)
        {
            if(tableName == "likedPosts")
            {
                return _dataContext.LikedPosts.Any(
               lp => lp.PostGuid == postID && lp.UserGuid == userID);
            }else if(tableName == "savedPosts")
            {
                return _dataContext.SavedPosts.Any(
              lp => lp.PostGuid == postID && lp.UserGuid == userID);
            }
            throw new Exception("Invalid table name");
        }
        
    }
}
