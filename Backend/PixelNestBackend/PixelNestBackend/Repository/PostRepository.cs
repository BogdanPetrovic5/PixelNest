
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using PixelNestBackend.Security;
using PixelNestBackend.Utility;


namespace PixelNestBackend.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PostRepository> _logger;
        private readonly SASTokenGenerator _SAStokenGenerator;
        private readonly UserUtility _userUtility;
        private readonly IMemoryCache _memoryCache;
        private const string PostsCacheKey = "Posts_{0}";
        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10); 
        public PostRepository(
            DataContext dataContext,
            IConfiguration configuration,
            ILogger<PostRepository> logger,
            SASTokenGenerator SASTokenGenerator,
            UserUtility userUtility,
            IMemoryCache memoryCache
            )
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _logger = logger;
            _SAStokenGenerator = SASTokenGenerator;
            _userUtility = userUtility;
            _memoryCache = memoryCache;
        }

      
        public bool SavePost(int userID, SavePostDto savePostDto, bool isDuplicate)
        {
            Post? post = _dataContext.Posts.Where(pid => savePostDto.PostID == pid.PostID).FirstOrDefault();
            var obj = new SavedPosts
            {
                UserID = userID,
                PostID = savePostDto.PostID,
                
            };
            var cacheKey = string.Format(PostsCacheKey);
            if (!isDuplicate)
            {
                _memoryCache.Remove(cacheKey);
                _dataContext.SavedPosts.Add(obj);
                if(post != null) post.LastModified = DateTime.UtcNow;

                return _dataContext.SaveChanges() > 0; 
            }
            var existingSave = _dataContext.SavedPosts.FirstOrDefault(sp => sp.PostID == savePostDto.PostID && sp.UserID == userID);
            if (existingSave != null) 
            {
              
               
                _memoryCache.Remove(cacheKey);
                _dataContext.SavedPosts.Remove(existingSave);
                return _dataContext.SaveChanges() > 0;
            }
            return false;
        }
        public async Task<PostResponse> PublishPost(PostDto postDto,int userID)
        {
            //if (postDto == null)
            //{
            //    return new PostResponse
            //    {
            //        IsSuccessfull = false,
            //        Message = "PostDto is null!"
            //    };
            //}

            //string newPostQuery = @"INSERT INTO Posts(UserID, PostDescription, TotalComments, TotalLikes, PublishDate, Location) 
            //                VALUES(@UserID, @PostDescription, @TotalComments, @TotalLikes, GETDATE(), @Location);
            //                SELECT CAST(SCOPE_IDENTITY() as int)";
            //string connectionString = _configuration.GetConnectionString("DefaultConnection");

            //try
            //{
            //    using (SqlConnection connection = new SqlConnection(connectionString))
            //    {
            //        await connection.OpenAsync();

            //        using (SqlCommand command = new SqlCommand(newPostQuery, connection))
            //        {

            //            command.Parameters.AddWithValue("@UserID", userID);


            //            command.Parameters.AddWithValue("@TotalComments", 0);
            //            command.Parameters.AddWithValue("@TotalLikes", 0);
            //            if (string.IsNullOrEmpty(postDto.Location))
            //            {
            //                command.Parameters.AddWithValue("@Location", DBNull.Value); 
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@Location", postDto.Location);
            //            }

            //            if (string.IsNullOrEmpty(postDto.PostDescription))
            //            {
            //                command.Parameters.AddWithValue("@PostDescription", DBNull.Value);
            //            }
            //            else
            //            {
            //                command.Parameters.AddWithValue("@PostDescription", postDto.PostDescription);
            //            }
            //            int postID = (int)await command.ExecuteScalarAsync();
            //            User user = _dataContext.Users.FirstOrDefault(u => u.Username == postDto.OwnerUsername);
            //            if(user != null)
            //            {
            //                user.TotalPosts += 1;
            //            }
            //            var cacheKey = string.Format(PostsCacheKey, postDto.OwnerUsername);
            //            var versionKey = $"{cacheKey}_Version";
            //            _memoryCache.Remove(cacheKey);
            //            return new PostResponse
            //            {
            //                IsSuccessfull = true,
            //                PostID = postID,
            //                Message = "Post was successfully added"
            //            };
            //        }
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    _logger.LogError($"Database error: {ex.Message}");
            //    return new PostResponse
            //    {
            //        IsSuccessfull = false,
            //        Message = $"Database error: {ex.Message}"
            //    };
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"General error: {ex.Message}");
            //    return new PostResponse
            //    {
            //        IsSuccessfull = false,
            //        Message = $"An unexpected error occurred: {ex.Message}"
            //    };
            //}
            if (postDto == null)
            {
                return new PostResponse
                {
                    IsSuccessfull = false,
                    Message = "PostDto is null!"
                };
            }

            try
            {
               
                var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserID == userID);
                if (user == null)
                {
                    return new PostResponse
                    {
                        IsSuccessfull = false,
                        Message = "User not found!"
                    };
                }

              
                var newPost = new Post
                {
                    UserID = userID,
                    PostDescription = string.IsNullOrEmpty(postDto.PostDescription) ? null : postDto.PostDescription,
                    TotalComments = 0,
                    TotalLikes = 0,
                    PublishDate = DateTime.UtcNow, 
                    Location = string.IsNullOrEmpty(postDto.Location) ? null : postDto.Location
                };


                _dataContext.Posts.Add(newPost);
                await _dataContext.SaveChangesAsync();

               
                user.TotalPosts += 1;
                await _dataContext.SaveChangesAsync();

               
            

                return new PostResponse
                {
                    IsSuccessfull = true,
                    PostID = newPost.PostID,
                    Message = "Post was successfully added"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return new PostResponse
                {
                    IsSuccessfull = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
        public async Task<ICollection<ResponsePostDto>> GetPostsByUsername(string username, string currentLoggedUser)
        {
            try
            {
               
                    int userID = _userUtility.GetUserID(username);
                    int currentLoggedUserID = _userUtility.GetUserID(currentLoggedUser);
                    ICollection<ResponsePostDto> posts = await _dataContext.Posts
                        .Where(u => u.UserID == userID)
                       .Select(a => new ResponsePostDto
                       {
                           PostDescription = a.PostDescription,
                           OwnerUsername = a.User.Username,
                           TotalComments = a.TotalComments,
                           TotalLikes = a.TotalLikes,
                           PostID = a.PostID,
                           IsDeletable = a.UserID == currentLoggedUserID,
                           PublishDate = a.PublishDate,
                           ImagePaths = a.ImagePaths.Select(l => new ResponseImageDto
                           {
                               Path = l.Path,
                               PhotoDisplay = l.PhotoDisplay,
                               PathID = l.PathID

                           }).ToList(),
                           Location = a.Location,
                           LikedByUsers = a.LikedPosts.Select(l => new LikeDto
                           {
                               Username = l.User.Username

                           }).ToList(),
                           SavedByUsers = a.SavedPosts.Select(s => new SavePostDto
                           {
                               Username = s.User.Username

                           }).ToList()
                       })
                    .AsSplitQuery()
                    .ToListAsync();
                    foreach (var post in posts)
                    {
                        _SAStokenGenerator.appendSasToken(post.ImagePaths);
                    }
                   
                    _logger.LogInformation("Posts retrieved successfully.");
                
                 
                
                return posts;
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Database error while retrieving posts: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }
        public async Task<ICollection<ResponsePostDto>> GetPostsByLocation(string location, string username)
        {
            try
            {
               
                    int userID = _userUtility.GetUserID(username);
                    ICollection<ResponsePostDto> posts = await _dataContext.Posts
                       .Where(l => l.Location.ToLower().Contains(location.ToLower()))
                       .Select(a => new ResponsePostDto
                       {
                           PostDescription = a.PostDescription,
                           OwnerUsername = a.User.Username,
                           TotalComments = a.TotalComments,
                           TotalLikes = a.TotalLikes,
                           IsDeletable = a.UserID == userID,
                           PostID = a.PostID,
                           PublishDate = a.PublishDate,
                           ImagePaths = a.ImagePaths.Select(l => new ResponseImageDto
                           {
                               Path = l.Path,
                               PhotoDisplay = l.PhotoDisplay,
                               PathID = l.PathID

                           }).ToList(),
                           Location = a.Location,
                           LikedByUsers = a.LikedPosts.Select(l => new LikeDto
                           {
                               Username = l.User.Username

                           }).ToList(),
                           SavedByUsers = a.SavedPosts.Select(s => new SavePostDto
                           {
                               Username = s.User.Username

                           }).ToList()
                       })
                    .AsSplitQuery()
                    .ToListAsync();
                    foreach (var post in posts)
                    {
                        _SAStokenGenerator.appendSasToken(post.ImagePaths);
                    }
                  
                
                return posts;
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Database error while retrieving posts: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }
        public async Task<ICollection<ResponsePostDto>> GetPosts(string username)
        {

            try
            {
                var cacheKey = string.Format(PostsCacheKey, username);

                var versionKey = $"{cacheKey}_Version";
                Console.WriteLine("\nVersion key: " + versionKey + "\n");

              
              
                if (!_memoryCache.TryGetValue(cacheKey, out ICollection<ResponsePostDto> posts) || CacheChange(username))
                {
                    Console.WriteLine("\nPonovo je pokrenuo query!\n");
                  posts = await _dataContext.Posts
                        .Select(a => new ResponsePostDto
                        {
                            PostDescription = a.PostDescription,

                            TotalComments = a.TotalComments,
                            TotalLikes = a.TotalLikes,
                            OwnerUsername = a.User.Username,
                            PostID = a.PostID,
                            IsDeletable = a.User.Username == username,
                            PublishDate = a.PublishDate,
                            ImagePaths = a.ImagePaths.Select(l => new ResponseImageDto
                            {
                                Path = l.Path,
                                PhotoDisplay = l.PhotoDisplay,
                                PathID = l.PathID

                            }).ToList(),
                            Location = a.Location,
                            LikedByUsers = a.LikedPosts.Select(l => new LikeDto
                            {
                                Username = l.User.Username

                            }).ToList(),
                            SavedByUsers = a.SavedPosts.Select(s => new SavePostDto
                            {
                                Username = s.User.Username

                            }).ToList()
                        })
                        .AsSplitQuery()
                        .ToListAsync();
                    foreach (var post in posts)
                    {
                        _SAStokenGenerator.appendSasToken(post.ImagePaths);
                    }

                    _memoryCache.Set(cacheKey, posts, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = CacheDuration
                    });

                    _memoryCache.Set(versionKey, _getLatestVersion(), new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = CacheDuration
                    });

                    _logger.LogInformation("Posts retrieved successfully.");
                }
              
                return posts;
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Database error while retrieving posts: {ex.Message}");
                throw;
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                throw; 
            }

        }
        
        public PostResponse LikePost(LikeDto likeDto, bool isLiked, int userID)
        {

            try
            {
                bool doubleAction = false;
                LikedPosts likedPost = _dataContext.LikedPosts
                    .FirstOrDefault(lp => lp.UserID == userID && lp.PostID == likeDto.PostID);

                User user = _dataContext.Users.Where(u => u.UserID == userID).FirstOrDefault();
                Post post = _dataContext.Posts.Include(u => u.User).Where(pid => pid.PostID == likeDto.PostID).FirstOrDefault();
                Notification notification = _dataContext.Notifications.Where(ru => ru.ReceiverID == post.UserID && ru.SenderID == userID && ru.PostID == likeDto.PostID).FirstOrDefault();
                if (notification != null) _dataContext.Notifications.Remove(notification);
                if (likedPost == null)
                {
                    Notification newNotification = new Notification
                    {
                        Message = $"liked your photo.",
                        SenderID = userID,
                        ReceiverID = post.User.UserID,
                        DateTime = DateTime.UtcNow,
                        PostID = post.PostID,
                        IsNew = true

                    };
                    if (newNotification.SenderID != newNotification.ReceiverID) _dataContext.Notifications.Add(newNotification);
                       
                    var newLikedPost = new LikedPosts
                    {
                        UserID = userID,
                        PostID = likeDto.PostID,
                        DateLiked = DateTime.UtcNow
                    };
                  
                    _dataContext.LikedPosts.Add(newLikedPost);
                }
                else
                {
                    _dataContext.LikedPosts.Remove(likedPost);
                    doubleAction = true;
                }
                post.LastModified = DateTime.UtcNow;
                int changes = _dataContext.SaveChanges();

                if (changes > 0)
                {
                   
                   


                    return new PostResponse
                    {
                        IsSuccessfull = true,
                        Message = "Successfully liked.",
                        User = user.Username,
                        TargetUser = post.User.Username,
                        DoubleAction = doubleAction ? true : false

                    };
                }
                else
                {
                    return new PostResponse
                    {
                        IsSuccessfull = false,
                        Message = "No changes were made."
                    };
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Database related error: {ex.Message}");
                return new PostResponse
                {
                    IsSuccessfull = false,
                    Message = "An error occurred while interacting with the database."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"General error: {ex.Message}");
                return new PostResponse
                {
                    IsSuccessfull = false,
                    Message = "An unexpected error occurred."
                };
            }


        } 
        public PostResponse Comment(Comment comment)
        {
            try
            {
                Post post = _dataContext.Posts.Include(u=>u.User).FirstOrDefault(p => p.PostID == comment.PostID);
                User user = _dataContext.Users.FirstOrDefault(u => u.UserID == comment.UserID);
                Comment parentComment = null;
                if (post == null || user == null)
                {
                    return new PostResponse
                    {
                        Message = "Post or user not found!",
                        IsSuccessfull = false
                    };
                }
                post.TotalComments += 1;
                post.LastModified = DateTime.UtcNow;
                
                if (comment.ParentCommentID.HasValue)
                {
                    parentComment = _dataContext.Comments
                        .Include(c => c.User)
                        .FirstOrDefault(c => c.CommentID == comment.ParentCommentID.Value && c.ParentCommentID == null);

                    if (parentComment != null)
                    {
                        parentComment.TotalReplies += 1;

                        this._createNotification(
                            receiverID: parentComment.User.UserID,
                            senderID: comment.UserID,
                            postID: post.PostID,
                            parentCommentID: comment.ParentCommentID,
                            message: $"replied to your comment."
                        );
                    }
                }
                _dataContext.Comments.Add(comment);
                int result = _dataContext.SaveChanges();

                if (result > 0)
                {
                    
                    this._createNotification(
                        receiverID: post.UserID,
                        senderID: comment.UserID,
                        postID: post.PostID,
                        message: $"commented on your photo.",
                        commentID: comment.CommentID
                    );

                    
                 

                    return new PostResponse
                    {
                        Message = "Comment added successfully!",
                        IsSuccessfull = true,
                        TargetUser = comment.ParentCommentID.HasValue ? parentComment.User.Username : post.User.Username,
                        DoubleAction = false


                    };
                }

                return new PostResponse
                {
                    Message = "Failed to save comment.",
                    IsSuccessfull = false
                };

            }
            catch(SqlException ex)
            {
                _logger.LogError($"Database related error: {ex.Message}");
                return null;
            }catch(Exception ex)
            {
                _logger.LogError($"General error in repo: {ex.Message}");
                return null;
            }
        }
        private void _createNotification(int receiverID, int senderID, int postID, string message, int? commentID = null, int? parentCommentID = null, int? likeID = null)
        {
            var notification = new Notification
            {
                ReceiverID = receiverID,
                SenderID = senderID,
                PostID = postID,
                LikeID = likeID,
                Message = message,
                DateTime = DateTime.UtcNow,
                CommentID = commentID,
                ParentCommentID = parentCommentID
            };
            if(notification.ReceiverID != notification.SenderID)
            {
                _dataContext.Notifications.Add(notification);
                _dataContext.SaveChanges();
            }
           
        }
        public async Task<DeleteResponse> DeletePost(int postID)
        {
            try
            {
                var post = _dataContext.Posts.FirstOrDefault(a => a.PostID == postID);
                var userID = _dataContext.Posts
                    .Where(p => p.PostID == postID)
                    .Select(p => p.UserID)
                    .FirstOrDefault();
                if (post == null)
                {
                    return new DeleteResponse
                    {
                        IsSuccess = false,
                        IsValid = true,
                        Message = "No post found!"
                    };
                }
                
                _dataContext.Comments.Where(a => a.PostID == postID).ExecuteDelete();
                _dataContext.LikedPosts.Where(a => a.PostID == postID).ExecuteDelete();
                _dataContext.SavedPosts.Where(a => a.PostID == postID).ExecuteDelete();
                _dataContext.ImagePaths.Where(a => a.PostID == postID).ExecuteDelete();
                _dataContext.Notifications.Where(a => a.PostID == postID).ExecuteDelete();
                User? user = _dataContext.Users.FirstOrDefault(u => u.UserID == userID);
                if (user != null) user.TotalPosts -= 1;
               
                _dataContext.Posts.Remove(post);
                
                await _dataContext.SaveChangesAsync();
                
                return new DeleteResponse
                {
                    IsSuccess = true,
                    IsValid = true,
                    Message = "Successfuly deleted!"
                };

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new DeleteResponse
                {
                    IsSuccess = false,
                    IsValid = true,
                    Message = "Server failed!"
                };
            }
        }

        public int ExtractUserID(int postID)
        {
            int userID = _dataContext.Posts.Where(a => a.PostID == postID)
                .Select(userID => userID.UserID).FirstOrDefault();

            return userID;
        }

        public bool CheckIntegrity(int userID, string email)
        {
            try
            {
                bool isValid = _dataContext.Users.Where(u => u.UserID == userID).Any(e => e.Email == email);
                return isValid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<ResponsePostDto> GetSinglePost(int postID, string currentLoggedUser)
        {
            try
            {
                var cacheKey = string.Format(PostsCacheKey, currentLoggedUser);
                var versionKey = $"{cacheKey}_Version";
                int currentLoggedUserID = _userUtility.GetUserID(currentLoggedUser);
                if (_memoryCache.TryGetValue(cacheKey, out List<ResponsePostDto>? allPosts) && allPosts != null)
                {
                    var cachedPost = allPosts.FirstOrDefault(p => p.PostID == postID);
                    if (cachedPost != null)
                    {
                        Console.WriteLine("\n uzeo iz kesa \n");
                        return cachedPost; 
                    }
                }
               
                ResponsePostDto? post = await _dataContext.Posts
                    .Where(u => u.PostID == postID)
                   .Select(a => new ResponsePostDto
                   {
                       PostDescription = a.PostDescription,
                       OwnerUsername = a.User.Username,
                       TotalComments = a.TotalComments,
                       TotalLikes = a.TotalLikes,
                       PostID = a.PostID,
                       IsDeletable = a.UserID == currentLoggedUserID,
                       PublishDate = a.PublishDate,
                       ImagePaths = a.ImagePaths.Select(l => new ResponseImageDto
                       {
                           Path = l.Path,
                           PhotoDisplay = l.PhotoDisplay,
                           PathID = l.PathID

                       }).ToList(),
                       Location = a.Location,
                       LikedByUsers = a.LikedPosts.Select(l => new LikeDto
                       {
                           Username = l.User.Username

                       }).ToList(),
                       SavedByUsers = a.SavedPosts.Select(s => new SavePostDto
                       {
                           Username = s.User.Username

                       }).ToList()
                   }).FirstOrDefaultAsync();
              
                
              
                 if(post != null)
                {
                    _SAStokenGenerator.appendSasToken(post.ImagePaths);
                }
                 
                

                _logger.LogInformation("Posts retrieved successfully.");



                return post;
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Database error while retrieving posts: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred: {ex.Message}");
                throw;
            }
        }

        public bool CacheChange(string username)
        {
            Console.WriteLine(username);
            var cacheKey = string.Format(PostsCacheKey,username);
            var versionKey = $"{cacheKey}_Version";
            Console.WriteLine("\nCacheChange: " + versionKey + "\n");
            if (!_memoryCache.TryGetValue(versionKey, out DateTime cachedVersion))
            {
                cachedVersion = DateTime.MinValue; 
            }

            var latestVersion = _getLatestVersion();

            bool hasChanged = cachedVersion < latestVersion;

            Console.WriteLine("\nLatest version date: " + latestVersion + "\n");
            Console.WriteLine("\nCached version date: " + cachedVersion + "\n");
            Console.WriteLine("\nCache change: " + hasChanged + "\n");
           
          

            return hasChanged;

        }
        private DateTime _getLatestVersion()
        {
            return _dataContext.Posts
                .OrderByDescending(p => p.LastModified) 
                .Select(p => p.LastModified)
                .FirstOrDefault();
        }
    }
}
