using PixelNestBackend.Dto;

namespace PixelNestBackend.Interfaces
{
    public interface IFileUpload
    {
        Task<bool> StoreImages(PostDto? postDto, StoryDto? storyDto,ProfileDto? profileDto, string userFolder, int? folder, int? userID);
    }
}
