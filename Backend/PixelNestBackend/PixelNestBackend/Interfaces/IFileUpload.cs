using PixelNestBackend.Dto;

namespace PixelNestBackend.Interfaces
{
    public interface IFileUpload
    {
        Task StoreImages(PostDto postDto, string userFolder, int postFolder);
    }
}
