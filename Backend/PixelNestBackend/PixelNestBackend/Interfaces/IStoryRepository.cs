using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IStoryRepository
    {
        Task<ICollection<ResponseStoryDto>> GetStories(string username);
        Task<StoryResponse> PublishStory(StoryDto storyDto,int userID);
    }
}
