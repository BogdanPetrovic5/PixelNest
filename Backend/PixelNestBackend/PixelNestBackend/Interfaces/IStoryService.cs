using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IStoryService
    {
        Task<ICollection<GroupedStoriesDto>> GetStories(bool forCurrentUser,string username);
       
        Task<StoryResponse> PublishStory(StoryDto storyDto);
        StoryResponse MarkStoryAsSeen(SeenDto seenDto);
        ICollection<ResponseViewersDto> GetViewers(ViewersDto viewersDto);
    }
}
