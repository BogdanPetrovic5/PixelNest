using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Responses;
using PixelNestBackend.Utility;

namespace PixelNestBackend.Services
{

    public class StoryService : IStoryService
    {
        private readonly string _basedFolderPath;
        private readonly IFileUpload _fileUpload;
        private readonly FolderGenerator _folderGenerator;
        private readonly IStoryRepository _storyRepository;
        private readonly UserUtility _userUtility;
        public StoryService(
            IFileUpload fileUpload,
            FolderGenerator folderGenerator,
            IStoryRepository storyRepository,
            UserUtility userUtility
            )
        {
            _basedFolderPath = Path.Combine("wwwroot", "Photos");
            _fileUpload = fileUpload;
            _folderGenerator = folderGenerator;
            _storyRepository = storyRepository;
            _userUtility = userUtility;
        }

        public async Task<ICollection<ResponseStoryDto>> GetStories(string username)
        {
            return await _storyRepository.GetStories(username);
        }

        public async Task<StoryResponse> PublishStory(StoryDto storyDto)
        {
            string userFolderName = storyDto.Username;
            string userFolderPath = Path.Combine(_basedFolderPath, userFolderName, "Stories");

            if (!_folderGenerator.CheckIfFolderExists(userFolderPath))
            {
                _folderGenerator.GenerateNewFolder(userFolderPath);
            }
            int userID = _userUtility.GetUserID(storyDto.Username);
            StoryResponse response = await _storyRepository
                .PublishStory(storyDto, userID);
            if(response != null)
            {
                if (response.IsSuccessful)
                {
                    int storyID = response.StoryID;
                    Console.WriteLine(storyID);
                    bool isUploaded = await _fileUpload.StoreImages(null, storyDto, userFolderPath, storyID);
                    Console.WriteLine("da li je uploadovano: " + isUploaded);
                    if (isUploaded)
                    {
                        return new StoryResponse
                        {
                            IsSuccessful = true,
                            Message = response.Message
                        };
                    }
                    return new StoryResponse {
                        IsSuccessful = false,
                        Message = "Image failed to upload!"
                        
                    };
                }
                return new StoryResponse
                {
                    IsSuccessful = false,
                    Message = response.Message
                };
            }
            return new StoryResponse
            {
                IsSuccessful = false,
                Message = "Internal server error!"
            };

        }
    }
}
