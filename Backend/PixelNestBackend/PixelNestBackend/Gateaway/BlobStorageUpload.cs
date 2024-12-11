using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using PixelNestBackend.Data;
using PixelNestBackend.Dto;
using PixelNestBackend.Models;
using Microsoft.AspNetCore.Http;

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
       

        public async Task<bool> StoreImages(PostDto? postDto, StoryDto? storyDto, string userFolder, int folder)
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
                            var blobName = $"{userFolder}/Posts/{postID}/{formFile.FileName}";
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
                        var blobName = $"{userFolder}/Story/{storyID}/{formFile.FileName}";
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
