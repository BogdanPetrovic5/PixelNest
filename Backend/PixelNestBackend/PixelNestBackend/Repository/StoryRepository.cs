using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;
using PixelNestBackend.Security;

namespace PixelNestBackend.Repository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly DataContext _dataContext;
        private readonly SASTokenGenerator _SASTokenGenerator;
        public StoryRepository(DataContext dataContext, SASTokenGenerator sASTokenGenerator)
        {
            _dataContext = dataContext;
            _SASTokenGenerator = sASTokenGenerator;
        }

        public async Task<ICollection<GroupedStoriesDto>> GetStories(string username)
        {
            try
            {
                ICollection<GroupedStoriesDto> groupedStories = null;
                User user = _dataContext.Users.Where(user => user.Username == username).FirstOrDefault();
                if (user != null)
                {
                    groupedStories = await _dataContext
                         .Stories
                         .Where(s => s.UserID != user.UserID && s.ExpirationDate >= DateTime.Now)
                         .GroupBy(s => s.UserID)
                         .Select(group => new GroupedStoriesDto
                         {
                             OwnerUsername = _dataContext.Users
                                                 .Where(u => u.UserID == group.Key)
                                                 .Select(u => u.Username)
                                                 .FirstOrDefault(), // Get the username of the story owner
                             Stories = group.Select(s => new ResponseStoryDto
                             {
                                 OwnerUsername = s.User.Username, // Ensure Stories table has the Username property
                                 SeenByUser = _dataContext.Seen.Any(a => a.UserID == user.UserID && s.StoryID == a.StoryID),
                                 ImagePaths = s.ImagePath // Assuming Stories has navigation property `ImagePaths`
                                                .Select(i => new ResponseImageDto
                                                {
                                                    Path = i.Path,
                                                    PhotoDisplay = i.PhotoDisplay,
                                                }).ToList(),
                                 StoryID = s.StoryID
                             }).ToList()
                         })
                         .AsSplitQuery()
                         .ToListAsync();
                }
                return groupedStories;
            }
            catch(SqlException ex)
            {
                return null;
            }   
        }
        public async Task<ICollection<GroupedStoriesDto>> GetCurrentUserStories(string username)
        {
            try
            {
                ICollection<GroupedStoriesDto> groupedStories = null;
                User user = _dataContext.Users.Where(user => user.Username == username).FirstOrDefault();
                if (user != null)
                {
                    groupedStories = await _dataContext
                         .Stories
                         .Where(s => s.UserID == user.UserID && s.ExpirationDate >= DateTime.Now)
                         .GroupBy(s => s.UserID)
                         .Select(group => new GroupedStoriesDto
                         {
                             OwnerUsername = _dataContext.Users
                                                 .Where(u => u.UserID == group.Key)
                                                 .Select(u => u.Username)
                                                 .FirstOrDefault(),
                             Stories = group.Select(s => new ResponseStoryDto
                             {
                                 OwnerUsername = s.User.Username, 
                                 SeenByUser = _dataContext.Seen.Any(a => a.UserID == user.UserID && s.StoryID == a.StoryID),
                                 ImagePaths = s.ImagePath 
                                                .Select(i => new ResponseImageDto
                                                {
                                                    Path = i.Path,
                                                    PhotoDisplay = i.PhotoDisplay,
                                                }).ToList(),
                                 StoryID = s.StoryID
                             }).ToList()
                         })
                         .AsSplitQuery()
                         .ToListAsync();
                }
                return groupedStories;
            }
            catch (SqlException ex)
            {
                return null;
            }
        }
        public StoryResponse MarkStoryAsSeen(Seen seen) 
        {
            try
            {
                if(!_dataContext.Seen.Any(a => a.UserID == seen.UserID && a.StoryID == seen.StoryID))
                {
                    _dataContext.Seen.Add(seen);
                    int result = _dataContext.SaveChanges();

                    if (result > 0)
                    {
                        return new StoryResponse
                        {
                            IsSuccessful = true,
                            Message = "Seen successfull"

                        };
                    }
                    return new StoryResponse
                    {
                        IsSuccessful = false,
                        Message = "Something wrong"

                    };
                }else return new StoryResponse
                {
                    IsSuccessful = false,
                    Message = "Already seen!"

                };

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<StoryResponse> PublishStory(StoryDto storyDto, int userID)
        {
            if(storyDto == null)
            {
                return new StoryResponse
                {
                    IsSuccessful = false,
                    Message = "StoryDto is null"

                };
            }
            Story story = new Story
            {
               
                UserID = userID,
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(1)
            };
            _dataContext.Stories.Add(story);
            int result = await _dataContext.SaveChangesAsync();

            if(result > 0)
            {
                int storyID = story.StoryID;
                return new StoryResponse
                {
                    IsSuccessful = true,
                    Message = "Story uploaded!",
                    StoryID = storyID
                };
            }
            return new StoryResponse
            {
                IsSuccessful = false,
                Message = "Story failed to upload!"
            };
        }

        public ICollection<ResponseViewersDto> GetViewers(ViewersDto viewersDto)
        {
            try
            {
                User user = _dataContext.Users.Where(u => u.Username.Equals(viewersDto.Username)).FirstOrDefault();
                ICollection<ResponseViewersDto> viewers = this._dataContext.Seen
                    .Where(a => a.UserID != user.UserID && viewersDto.StoryID == a.StoryID)
                    .Select(v => new ResponseViewersDto
                    {
                        Username = v.User.Username
                    }).ToList();
                return viewers;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
