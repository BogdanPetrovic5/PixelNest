using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _memoryCache;
        private const string StoryCacheKey = "Stories";
        private const string UserStoryCacheKey = "UserStories_{0}";
        private readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
        public StoryRepository(DataContext dataContext, 
            SASTokenGenerator sASTokenGenerator,
            IMemoryCache memoryCache
            )
        {
            _dataContext = dataContext;
            _SASTokenGenerator = sASTokenGenerator;
            _memoryCache = memoryCache;
        }

        public async Task<ICollection<GroupedStoriesDto>> GetStories(string userGuid)
        {
            try
            {

                Guid parsedUserGuid = Guid.Parse(userGuid);
                ICollection<GroupedStoriesDto> groupedStories = null;
                    
                   
                if (userGuid != null)
                {
                    groupedStories = await _dataContext
                         .Stories
                         .Where(s => s.UserGuid != parsedUserGuid && s.ExpirationDate >= DateTime.Now)
                         .GroupBy(s => s.UserGuid)
                         .Select(group => new GroupedStoriesDto
                         {
                             ClientGuid = _dataContext.Users
                                                 .Where(u => u.UserGuid == group.Key)
                                                 .Select(u => u.ClientGuid)
                                                 .FirstOrDefault(),
                             OwnerUsername = _dataContext.Users
                                                 .Where(u => u.UserGuid == group.Key)
                                                 .Select(u => u.Username)
                                                 .FirstOrDefault(),
                             Stories = group.Select(s => new ResponseStoryDto
                             {
                                 OwnerUsername = s.User.Username,
                                 SeenByUser = _dataContext.Seen.Any(a => a.UserGuid == parsedUserGuid && s.StoryGuid == a.StoryGuid),
                                 ClientGuid = s.User.ClientGuid,
                                 ImagePaths = s.ImagePath
                                                .Select(i => new ResponseImageDto
                                                {
                                                    Path = i.Path,
                                                    PhotoDisplay = i.PhotoDisplay,
                                                }).ToList(),
                                 StoryID = s.StoryGuid
                             }).ToList()
                         })
                         .AsSplitQuery()
                         .ToListAsync();

                }
                //this._appendToken(groupedStories);

                return groupedStories;
            }
            catch(SqlException ex)
            {
                return null;
            }   
        }
        public async Task<ICollection<GroupedStoriesDto>> GetCurrentUserStories(string userGuid)
        {
            try
            {

                Guid parsedUserGuid = Guid.Parse(userGuid);
                ICollection<GroupedStoriesDto> groupedStories = null;
                   
                if (userGuid != null)
                {
                    groupedStories = await _dataContext
                            .Stories
                            .Where(s => s.UserGuid == parsedUserGuid && s.ExpirationDate >= DateTime.Now)
                            .GroupBy(s => s.UserGuid)
                            .Select(group => new GroupedStoriesDto
                            {
                                ClientGuid = _dataContext.Users
                                                .Where(u => u.UserGuid == group.Key)
                                                .Select(u => u.ClientGuid)
                                                .FirstOrDefault(),
                                OwnerUsername = _dataContext.Users
                                                    .Where(u => u.UserGuid == group.Key)
                                                    .Select(u => u.Username)
                                                    .FirstOrDefault(),
                                Stories = group.Select(s => new ResponseStoryDto
                                {
                                    OwnerUsername = s.User.Username,
                                    ClientGuid = s.User.ClientGuid,
                                    SeenByUser = _dataContext.Seen.Any(a => a.UserGuid == parsedUserGuid && s.StoryGuid == a.StoryGuid),
                                    ImagePaths = s.ImagePath
                                                .Select(i => new ResponseImageDto
                                                {
                                                    Path = i.Path,
                                                    PhotoDisplay = i.PhotoDisplay,
                                                }).ToList(),
                                    StoryID = s.StoryGuid
                                }).ToList()
                            })
                            .AsSplitQuery()
                            .ToListAsync();
                       
                }
                //this._appendToken(groupedStories);

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
                if(!_dataContext.Seen.Any(a => a.UserGuid == seen.UserGuid && a.StoryGuid == seen.StoryGuid))
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

        public async Task<StoryResponse> PublishStory(StoryDto storyDto, Guid userID)
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
               
                UserGuid = userID,
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(1)
            };
            _dataContext.Stories.Add(story);
            int result = await _dataContext.SaveChangesAsync();

            if(result > 0)
            {
                Guid storyID = story.StoryGuid;
                
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

        public ICollection<ResponseViewersDto> GetViewers(ViewersDto viewersDto, string userGuid)
        {
            try
            {
                ICollection<ResponseViewersDto> viewers = null;
                Story story = _dataContext.Stories.Where(u => u.UserGuid.ToString() == userGuid && u.StoryGuid == viewersDto.StoryID).FirstOrDefault();
                if(story != null)
                {
                    viewers = this._dataContext.Seen
                    .Include(u => u.User)
                    .Where(a => a.UserGuid.ToString() != userGuid && viewersDto.StoryID == a.StoryGuid)
                    .Select(v => new ResponseViewersDto
                    {
                        Username = v.User.Username,
                        ClientGuid = v.User.ClientGuid.ToString()

                    }).ToList();
                    return viewers;
                }
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
        private void _appendToken(ICollection<GroupedStoriesDto> groupedStories)
        {
            foreach(var stories in groupedStories)
            {
                foreach(var story in stories.Stories)
                {
                    if (story.ImagePaths != null)
                    {
                        _SASTokenGenerator.appendSasToken(story.ImagePaths);
                    }
                }
            }
            
        }

    }



}
