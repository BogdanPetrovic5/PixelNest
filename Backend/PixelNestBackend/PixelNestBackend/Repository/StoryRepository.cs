using PixelNestBackend.Data;
using PixelNestBackend.Dto;
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
