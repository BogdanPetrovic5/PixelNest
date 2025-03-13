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
        private readonly BlobStorageUpload _blobStorageUpload;
        public StoryService(
            IFileUpload fileUpload,
            FolderGenerator folderGenerator,
            IStoryRepository storyRepository,
            UserUtility userUtility,
            BlobStorageUpload blobStorageUpload


            )
        {
            _basedFolderPath = Path.Combine("wwwroot", "Photos");
            _fileUpload = fileUpload;
            _folderGenerator = folderGenerator;
            _storyRepository = storyRepository;
            _userUtility = userUtility;
            _blobStorageUpload = blobStorageUpload;
        }

        public async Task<ICollection<GroupedStoriesDto>> GetStories(bool forCurrentUser, string userGuid)
        {
            if (forCurrentUser)
            {
                return await _storyRepository.GetCurrentUserStories(userGuid);
            }
            return await _storyRepository.GetStories(userGuid);
        }

        public ICollection<ResponseViewersDto> GetViewers(ViewersDto viewersDto, string userGuid)
        {
            return this._storyRepository.GetViewers(viewersDto, userGuid);
        }

        public StoryResponse MarkStoryAsSeen(SeenDto seenDto, string userGuid)
        {
           
            Seen seen = new Seen
            {
                StoryGuid = seenDto.StoryID,
                StoryID = -1,
                UserGuid = Guid.Parse(userGuid),
                UserID = -1
            };
            return _storyRepository.MarkStoryAsSeen(seen);
        }

        public async Task<StoryResponse> PublishStory(StoryDto storyDto, string storyGuid)
        {
            
            string userFolderName = storyGuid;
            string userFolderPath = Path.Combine(_basedFolderPath, userFolderName, "Stories");

            if (!_folderGenerator.CheckIfFolderExists(userFolderPath))
            {
                _folderGenerator.GenerateNewFolder(userFolderPath);
            }
            
            StoryResponse response = await _storyRepository
                .PublishStory(storyDto, Guid.Parse(storyGuid));
            if(response != null)
            {
                if (response.IsSuccessful)
                {
                    Guid storyID = response.StoryID;
                    bool isUploaded = await _fileUpload.StoreImages(null, storyDto,null, userFolderPath, storyID, null);
                    //bool isUploaded = await _blobStorageUpload.StoreImages(null, storyDto,null, userID, storyID);

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
