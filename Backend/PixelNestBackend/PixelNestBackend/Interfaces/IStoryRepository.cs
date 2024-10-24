using PixelNestBackend.Dto;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IStoryRepository
    {
        Task<StoryResponse> PublishStory(StoryDto storyDto,int userID);
    }
}
