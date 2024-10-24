using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PixelNestBackend.Dto;
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
        [HttpPost("PublishStory")]
        public async Task<ActionResult<StoryResponse>> PublishStory([FromForm]StoryDto storyDto)
        {
            if(storyDto == null)
            {
                return BadRequest(new StoryResponse { IsSuccessful = false, Message = "Bad request body." });
            }
            StoryResponse response = await _storyService.PublishStory(storyDto);
            if(response != null)
            {
                if (response.IsSuccessful)
                {
                    return Ok(new StoryResponse { IsSuccessful = true, Message = response.Message });
                }
                else return NotFound(new StoryResponse { IsSuccessful = false, Message = response.Message });
            }
            else return NotFound(new StoryResponse { IsSuccessful = false, Message = "No response." });
        }
    }
}
