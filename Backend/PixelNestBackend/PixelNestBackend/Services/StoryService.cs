using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Gateaway;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Models;
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

        public async Task<ICollection<GroupedStoriesDto>> GetStories(bool forCurrentUser, string username)
        {
            if (forCurrentUser)
            {
                return await _storyRepository.GetCurrentUserStories(username);
            }
            return await _storyRepository.GetStories(username);
        }

        public ICollection<ResponseViewersDto> GetViewers(ViewersDto viewersDto)
        {
            return this._storyRepository.GetViewers(viewersDto);
        }

        public StoryResponse MarkStoryAsSeen(SeenDto seenDto)
        {
            int userID = _userUtility.GetUserID(seenDto.Username);
            Seen seen = new Seen
            {
                StoryID = seenDto.StoryID,
               
                UserID = userID
            };
            return _storyRepository.MarkStoryAsSeen(seen);
        }

        public async Task<StoryResponse> PublishStory(StoryDto storyDto)
        {
            int userID = _userUtility.GetUserID(storyDto.Username);
            string userFolderName =userID.ToString();
            string userFolderPath = Path.Combine(_basedFolderPath, userFolderName, "Stories");

            if (!_folderGenerator.CheckIfFolderExists(userFolderPath))
            {
                _folderGenerator.GenerateNewFolder(userFolderPath);
            }
            
            StoryResponse response = await _storyRepository
                .PublishStory(storyDto, userID);
            if(response != null)
            {
                if (response.IsSuccessful)
                {
                    int storyID = response.StoryID;
                    bool isUploaded = await _fileUpload.StoreImages(null, storyDto,null, userFolderPath, storyID,null);
                    //bool isUploaded = await _blobStorageUpload.StoreImages(null, storyDto, storyDto.Username, storyID);

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
