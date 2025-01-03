using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace PixelNestBackend.Gateaway
{
    public class BlobStorageUpload
    {
        private readonly DataContext _dataContext;
        private readonly string _blobConnectionString;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
     
        public BlobStorageUpload(DataContext dataContext, BlobServiceClient blobServiceClient, string containerName)
        {
            _blobServiceClient = blobServiceClient;
            _containerName = containerName;
            _dataContext = dataContext;
        }
       

        public async Task<bool> StoreImages(PostDto? postDto, StoryDto? storyDto, ProfileDto profileDto, int userFolder, int? folder)
        {

            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                await containerClient.CreateIfNotExistsAsync();


                string postID = folder.ToString();
                if (postDto != null)
                {

                    foreach (var formFile in postDto.Photos)
                    {
                        if(formFile != null && formFile.Length > 0)
                        {
                            var blobName = $"{userFolder.ToString()}/Posts/{postID}/{formFile.FileName}";
                            var blobClient = containerClient.GetBlobClient(blobName);

                            using (var stream = formFile.OpenReadStream())
                            {
                                await blobClient.UploadAsync(stream, overwrite: true);
                            }
                            var imagePaths = new ImagePath
                            {
                                PostID = folder,
                                PhotoDisplay = postDto.PhotoDisplay,
                                Path = blobName
                            };

                            _dataContext.ImagePaths.Add(imagePaths);
                            await _dataContext.SaveChangesAsync();
                        }
                        
                    }
                    return true;
                }else if(storyDto != null)
                {
                    string storyID = folder.ToString();
                    var formFile = storyDto.StoryImage;
                    if (formFile != null)
                    {
                        var blobName = $"{userFolder.ToString()}/Story/{storyID}/{formFile.FileName}";
                        var blobClient = containerClient.GetBlobClient(blobName);

                        using (var stream = formFile.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, overwrite: true);
                        }
                        var imagePaths = new ImagePath
                        {
                            StoryID = folder,
                            PhotoDisplay = storyDto.PhotoDisplay,
                            Path = blobName
                        };

                        _dataContext.ImagePaths.Add(imagePaths);
                        await _dataContext.SaveChangesAsync();
                    }

                    
                    return true;
                }else
                {
                    string userID = userFolder.ToString();
                    var formFile = profileDto.ProfilePicture;
                    if (formFile != null) { 
                        var blobName = $"{userID}/Profile/{formFile.FileName}";
                        var blobClient = containerClient.GetBlobClient(blobName);

                        using (var stream = formFile.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, overwrite: true);
                        }

                        var existingImagePath = await _dataContext.ImagePaths.FirstOrDefaultAsync(ip => ip.UserID == userFolder);
                        if (existingImagePath != null)
                        {
                            existingImagePath.Path = blobName;
                            existingImagePath.PhotoDisplay = "cover";
                            _dataContext.ImagePaths.Update(existingImagePath);
                        }
                        else
                        {
                            var imagePaths = new ImagePath
                            {
                                UserID = userFolder,
                                PhotoDisplay = "cover",
                                Path = blobName
                            };
                            _dataContext.ImagePaths.Add(imagePaths);
                        }
                       

                       
                       await _dataContext.SaveChangesAsync();
                       return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return false;
        }
    }
}
