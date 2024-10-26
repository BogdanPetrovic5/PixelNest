using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IStoryService
    {
        Task<ICollection<ResponseStoryDto>> GetStories(string username);
        Task<StoryResponse> PublishStory(StoryDto storyDto);

    }
}
