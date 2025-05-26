using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PixelNestBackend.Dto;
using PixelNestBackend.Dto.Projections;
using PixelNestBackend.Interfaces;
using PixelNestBackend.Responses;
using System.Security.Claims;

namespace PixelNestBackend.Controllers
{
    [Route("api/story")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;
        public StoryController(IStoryService storyService)
        {
            _storyService = storyService;
        }
        [Authorize]
        [HttpGet("stories")]
        public async Task<ActionResult<GroupedStoriesDto>> GetStories(bool forCurrentUser, int currentPage, int maximum = 10)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ICollection<GroupedStoriesDto> stories = await _storyService.GetStories(forCurrentUser, userGuid);
         
            if (stories != null)
            {
                return Ok(stories);
            } else return NotFound();
        }
       
        [HttpPost("new-story")]
        public async Task<ActionResult<StoryResponse>> PublishStory([FromForm] StoryDto storyDto)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (storyDto == null)
            {
                return BadRequest(new StoryResponse { IsSuccessful = false, Message = "Bad request body." });
            }
            StoryResponse response = await _storyService.PublishStory(storyDto, userGuid);
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
        [Authorize]
        [HttpPost("seen")]
        public ActionResult<StoryResponse> MarkStoryAsSeen(SeenDto seenDto){
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            StoryResponse storyResponse = _storyService.MarkStoryAsSeen(seenDto, userGuid);
            if(storyResponse == null || storyResponse.IsSuccessful == false)
            {
                return NotFound(new StoryResponse { IsSuccessful = false, Message = "No response." });
            }
            
            return Ok(new StoryResponse { IsSuccessful = true, Message = storyResponse.Message });
            
        }
        [Authorize]
        [HttpGet("viewers")]
        public ActionResult<ICollection<ResponseViewersDto>> GetViewers([FromQuery]ViewersDto viewersDto)
        {
            string? userGuid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (viewersDto == null) return BadRequest(new { message = "Bad request" });
            
            ICollection<ResponseViewersDto> viewers = _storyService.GetViewers(viewersDto, userGuid);
            if (viewers != null)
            {
                return Ok(viewers);
            }
            else return NotFound(new { message = "No data!" });
        }
    }
}
