using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IStoryRepository
    {
        Task<ICollection<GroupedStoriesDto>> GetStories(string username);
        Task<ICollection<GroupedStoriesDto>> GetCurrentUserStories(string userGuid);
        Task<StoryResponse> PublishStory(StoryDto storyDto,Guid userID);
        StoryResponse MarkStoryAsSeen(Seen seen);
        ICollection<ResponseViewersDto> GetViewers(ViewersDto viewersDto, string userGuid);
    }
}
