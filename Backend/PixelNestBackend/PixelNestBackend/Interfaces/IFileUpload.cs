using PixelNestBackend.Dto;

namespace PixelNestBackend.Interfaces
{
    public interface IFileUpload
    {
        Task<bool> StoreImages(PostDto? postDto, StoryDto? storyDto, string userFolder, int folder);
    }
}
