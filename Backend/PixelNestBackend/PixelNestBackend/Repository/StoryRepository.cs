using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Repository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly DataContext _dataContext;
        public StoryRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ICollection<GroupedStoriesDto>> GetStories(string username)
        {
            try
            {
                    var groupedStories = await _dataContext
               .Stories
               .Where(s => s.Username != username)
               .GroupBy(s => s.Username)
               .Select(group => new GroupedStoriesDto
               {
                   OwnerUsername = group.Key,
                   Stories = group.Select(s => new ResponseStoryDto
                   {
                       OwnerUsername = s.Username,
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

                return groupedStories;
            }
            catch(SqlException ex)
            {
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
                ExpirationDate = DateTime.Now.AddMinutes(1)
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
    }
}
