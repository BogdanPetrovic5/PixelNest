using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;

namespace PixelNestBackend.Gateaway
{
    public class FileUpload : IFileUpload
    {
        private readonly DataContext _dataContext;
        public FileUpload(DataContext dataContext) { 
            _dataContext = dataContext;
        }
        public async Task<bool> StoreImages(PostDto postDto, string userFolder, int postFolder)
        {
            try
            {
                string postFolderID = postFolder.ToString();
                string postFolderPath = Path.Combine(userFolder, postFolderID);
                Directory.CreateDirectory(postFolderPath);

                foreach (var formFile in postDto.Photos)
                {
                    if (formFile != null && formFile.Length != 0)
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
                        await _dataContext.SaveChangesAsync();
                    }
                }

              
               
                return true;
            }
            catch (Exception)
            {
              
                return false;
            }
        }
    }
}
