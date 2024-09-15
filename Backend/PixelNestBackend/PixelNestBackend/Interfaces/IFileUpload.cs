using PixelNestBackend.Dto;

namespace PixelNestBackend.Interfaces
{
    public interface IFileUpload
    {
        Task<bool> StoreImages(PostDto postDto, string userFolder, int postFolder);
    }
}
