using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResourceService.Application.Commons.DTOs.Posts;
using ResourceService.Application.Interfaces;
using ResourceService.Domain.Enums;
using SharedLibrary.Jwt;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController(IApplicationServiceProvider applicationServiceProvider,IJwtService jwtService) : ControllerBase
    {
        private readonly IApplicationServiceProvider _serviceProviders = applicationServiceProvider;
        private readonly IJwtService _jwtService = jwtService;

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Instructor))]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostDTO createPostDto)
        {
            var authorId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.PostService.CreatePost(createPostDto, authorId);
            return result.ToActionResult();
        }
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PostsFilterDTO postsFilterDTO)
        {
            var result = await _serviceProviders.PostService.GetPosts(postsFilterDTO);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(UserRole.Inspector) + "," + nameof(UserRole.Admin))]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostSDTO updatePostSDTO)
        {
            var reviewerId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
           var result = await _serviceProviders.PostService.UpdatePost(id, reviewerId, updatePostSDTO);
            return result.ToActionResult();
        }

        [HttpPost("{id}/reactions")]
        [Authorize(Roles = nameof(UserRole.NoviceDriver))]
        public async Task<IActionResult> ReactToPost(Guid id, [FromBody] ReactPostDTO reactPostDTO)
        {
            var driverId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.PostService.ReactToPost(id, driverId, reactPostDTO);
            return result.ToActionResult();
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> CommentOnPost(Guid id, [FromBody] CommentPostDTO request)
        {
            var result = await _serviceProviders.PostService.CommentOnPost(id, request);
            return result.ToActionResult();
        }
        [HttpPut("{id}/reject-post")]
     //   [Authorize(Roles = nameof(UserRole.Inspector) + "," + nameof(UserRole.Admin))]
        public async Task<IActionResult> RejectPost(Guid id, [FromBody] RejectPostDTO rejectPostDTO)
        {
            var reviewerId = await _jwtService.ExtractUserIdFromToken(Request.Headers["Authorization"].ToString());
            var result = await _serviceProviders.PostService.RejectPost(id, rejectPostDTO, reviewerId);
            return result.ToActionResult();
        }

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePost(Guid id)
        //{
        //    var result = await _serviceProviders.PostService.DeletePost(id);
        //    return result.ToActionResult();
        //}
    }
}

