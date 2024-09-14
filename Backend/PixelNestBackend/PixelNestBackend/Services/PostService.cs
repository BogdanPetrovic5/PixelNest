using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;

namespace PixelNestBackend.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;
        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task StoreImages(PostDto postDto, string userFolder, int postFolder)
        {
            string postFolderID = postFolder.ToString();
            string postFolderPath = Path.Combine(userFolder, postFolderID);
            Directory.CreateDirectory(postFolderPath);

            foreach (var formFile in postDto.Photos)
            {
                if(formFile != null || formFile.Length != 0)
                {
                    string fileName = formFile.FileName;
                    string filePath = Path.Combine(postFolderPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    var imagePaths = new ImagePath
                    {
                        PostID = postFolder,
                        PhotoDisplay = postDto.PhotoDisplay,
                        Path = Path.Combine(postDto.OwnerUsername, postFolderID, fileName)

                    };
                    _dataContext.ImagePaths.Add(imagePaths);
                    _dataContext.SaveChanges();
                }
           
            }
        }

    }
}
