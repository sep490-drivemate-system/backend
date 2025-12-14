using ResourceService.Application.Commons.DTOs.Posts;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Application.Interfaces.Services
{
    public interface IPostService
    {
        Task<Result<bool>> CreatePost(CreatePostDTO createPostDto, Guid authorId);
        Task<Result<PaginatedList<PostsDTO>>> GetPosts(PostsFilterDTO postsFilterDTO);
        Task<Result<bool>> UpdatePost(Guid postId,Guid reviewId ,UpdatePostSDTO updatePostSDTO);
        Task<Result<bool>> ReactToPost(Guid postId, Guid driverId,ReactPostDTO request);
        Task<Result<Guid>> CommentOnPost(Guid postId, CommentPostDTO request);
      //  Task<Result<bool>> DeletePost(Guid postId);
    }
}
