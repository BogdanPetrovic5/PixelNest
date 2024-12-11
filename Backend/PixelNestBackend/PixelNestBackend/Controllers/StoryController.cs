using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Responses;

namespace PixelNestBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        public StoryController(IStoryService storyService)
        {
            _storyService = storyService;
        }
        [HttpGet("GetStories")]
        public async Task<ActionResult<GroupedStoriesDto>> GetStories(bool forCurrentUser, string username, int currentPage, int maximum = 10)
        {
            ICollection<GroupedStoriesDto> stories = await _storyService.GetStories(forCurrentUser, username);
         
            if (stories != null)
            {
                return Ok(stories);
            } else return NotFound();
        }

        [HttpPost("PublishStory")]
        public async Task<ActionResult<StoryResponse>> PublishStory([FromForm] StoryDto storyDto)
        {
            if (storyDto == null)
            {
                return BadRequest(new StoryResponse { IsSuccessful = false, Message = "Bad request body." });
            }
            StoryResponse response = await _storyService.PublishStory(storyDto);
            if (response != null)
            {
                if (response.IsSuccessful)
                {
                    return Ok(new StoryResponse { IsSuccessful = true, Message = response.Message });
                }
                else return NotFound(new StoryResponse { IsSuccessful = false, Message = response.Message });
            }
            else return NotFound(new StoryResponse { IsSuccessful = false, Message = "No response." });
        }

        [HttpPost("MarkStoryAsSeen")]
        public ActionResult<StoryResponse> MarkStoryAsSeen(SeenDto seenDto){
            StoryResponse storyResponse = _storyService.MarkStoryAsSeen(seenDto);
            if(storyResponse == null || storyResponse.IsSuccessful == false)
            {
                return NotFound(new StoryResponse { IsSuccessful = false, Message = "No response." });
            }
            
            return Ok(new StoryResponse { IsSuccessful = true, Message = storyResponse.Message });
            
        }
    }
}
