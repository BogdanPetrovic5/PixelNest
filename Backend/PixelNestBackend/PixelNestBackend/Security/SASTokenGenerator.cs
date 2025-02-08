using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Models;
namespace PixelNestBackend.Security
{
    public class SASTokenGenerator
    {
        private readonly string _containerName;
        private readonly string _connectionString;
        public SASTokenGenerator(string containerName, string connectionString)
        {
            _containerName = containerName; 
            _connectionString = connectionString;
        }
        private string _GenerateTokenForImage()
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainer = blobServiceClient.GetBlobContainerClient(_containerName);

       
            
           

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerName,
               
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(7),
                
               
            };
            sasBuilder.Protocol = SasProtocol.HttpsAndHttp;
            sasBuilder.SetPermissions(BlobSasPermissions.Read | BlobSasPermissions.List);
         

           
            var sasToken = blobContainer.GenerateSasUri(sasBuilder).Query;
     
           
          
            return sasToken;
        }
        public void appendSasToken(ImagePath image) { 
        
                image.Path = $"{image.Path.Replace('\\', '/')}{this._GenerateTokenForImage()}";
            

        }
        public void appendSasToken(ICollection<ResponseImageDto> imagePaths)
        {
         
            
                foreach (var image in imagePaths)
                {

                    image.Path = $"{image.Path.Replace('\\', '/')}{this._GenerateTokenForImage()}";
                }
            
        }
    }
}
