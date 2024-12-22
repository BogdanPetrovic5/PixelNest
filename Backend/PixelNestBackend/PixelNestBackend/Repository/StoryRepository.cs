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
                    var groupedStories = await _dataContext
               .Stories
               .Where(s => s.Username != username && s.ExpirationDate >= DateTime.Now)
               .GroupBy(s => s.Username)
               .Select(group => new GroupedStoriesDto
               {
                   OwnerUsername = group.Key,
                   Stories = group.Select(s => new ResponseStoryDto
                   {
                       OwnerUsername = s.Username,
                       SeenByUser = _dataContext.Seen.Any(a => a.Username == username && s.StoryID == a.StoryID),
                       ImagePaths = s.ImagePath.Select(i => new ResponseImageDto
                       {
                           Path = i.Path,
                           PhotoDisplay = i.PhotoDisplay,
                       }).ToList(),
                       StoryID = s.StoryID
                   }).ToList()
               })
               .AsSplitQuery()
               .ToListAsync();
                foreach (var post in groupedStories)
                {
                    foreach (var story in post.Stories)
                    {
                        if (story.ImagePaths != null)
                        {
                            _SASTokenGenerator.appendSasToken(story.ImagePaths);
                        }

                    }
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
                 var groupedStories = await _dataContext
                .Stories
                .Where(s => s.Username == username && s.ExpirationDate >= DateTime.Now)
                .GroupBy(s => s.Username)
                .Select(group => new GroupedStoriesDto
                {
                    OwnerUsername = group.Key,
                    Stories = group.Select(s => new ResponseStoryDto
                    {
                        OwnerUsername = s.Username,
                        SeenByUser = _dataContext.Seen.Any(a => a.Username == username && s.StoryID == a.StoryID),
                        ImagePaths = s.ImagePath.Select(i => new ResponseImageDto
                        {
                            Path = i.Path,
                            PhotoDisplay = i.PhotoDisplay,
                        }).ToList(),
                        StoryID = s.StoryID
                    }).ToList()
                })
                .AsSplitQuery()
                .ToListAsync();
                foreach (var post in groupedStories)
                {
                    foreach (var story in post.Stories)
                    {
                        if (story.ImagePaths != null)
                        {
                            _SASTokenGenerator.appendSasToken(story.ImagePaths);
                        }

                    }
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
                if(!_dataContext.Seen.Any(a => a.Username == seen.Username && a.StoryID == seen.StoryID))
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
                Username = storyDto.Username,
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
                ICollection<ResponseViewersDto> viewers = this._dataContext.Seen
                    .Where(a => a.Username != viewersDto.Username && viewersDto.StoryID == a.StoryID)
                    .Select(v => new ResponseViewersDto
                    {
                        Username = v.Username
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
