using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;

namespace PixelNestBackend.Gateaway
{
    public class FileUpload : IFileUpload
    {
        public Task StoreImages(PostDto postDto, string userFolder, int postFolder)
        {
            throw new NotImplementedException();
        }
    }
}
