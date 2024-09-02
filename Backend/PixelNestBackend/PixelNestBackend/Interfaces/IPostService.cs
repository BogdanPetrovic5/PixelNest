using PixelNestBackend.Dto;

namespace PixelNestBackend.Interfaces
{
    public interface IPostService
    {
        Task StoreImages(PostDto postDto, string userFolder, int postFolder);
    }
}
