using PixelNestBackend.Dto;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IStoryService
    {

        Task<StoryResponse> PublishStory(StoryDto storyDto);

    }
}
