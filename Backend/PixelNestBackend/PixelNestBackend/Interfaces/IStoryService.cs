using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IStoryService
    {
        Task<ICollection<GroupedStoriesDto>> GetStories(bool forCurrentUser,string userGuid);
       
        Task<StoryResponse> PublishStory(StoryDto storyDto, string storyGuid);
        StoryResponse MarkStoryAsSeen(SeenDto seenDto, string userGuid);
        ICollection<ResponseViewersDto> GetViewers(ViewersDto viewersDto, string userGuid);
    }
}
