using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Interfaces
{
    public interface IStoryRepository
    {
        Task<ICollection<GroupedStoriesDto>> GetStories(string username);
        Task<StoryResponse> PublishStory(StoryDto storyDto,int userID);
        StoryResponse MarkStoryAsSeen(Seen seen);
    }
}
