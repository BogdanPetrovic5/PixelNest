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
        public async Task<bool> StoreImages(PostDto? postDto,StoryDto? storyDto, string userFolder, int folder)
        {
            try
            {
               
                if(postDto != null)
                {
                    string postFolderID = folder.ToString();
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
                                PostID = folder,
                                PhotoDisplay = postDto.PhotoDisplay,
                                Path = Path.Combine(postDto.OwnerUsername, postFolderID, fileName)
                            };

                            _dataContext.ImagePaths.Add(imagePaths);
                            await _dataContext.SaveChangesAsync();
                        }
                    }
                }else if(storyDto != null)
                {
                    var formFile = storyDto.StoryImage;
                    if(formFile != null)
                    {
                        
                        string storyFolderID = folder.ToString();
                        string storyFolderPath = Path.Combine(userFolder, storyFolderID);
                        Directory.CreateDirectory(storyFolderPath);
                        string fileName = formFile.FileName;
                        string filePath = Path.Combine(storyFolderPath, fileName);
                       
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                       
                        ImagePath imagePath = new ImagePath
                        {
                            StoryID = folder,
                            PhotoDisplay = storyDto.PhotoDisplay,
                            Path = Path.Combine(storyDto.Username, "Stories", storyFolderID, fileName)
                        };
                   
                        
                        _dataContext.ImagePaths.Add(imagePath);
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
