using AutoMapper;
using ResourceService.Application.Commons;
using ResourceService.Application.Commons.DTOs.Posts;
using ResourceService.Application.Interfaces.Services;
using ResourceService.Domain.Entities;
using ResourceService.Domain.Enums;
using SharedLibrary.CloudinaryStorage;
using SharedLibrary.Email;
using SharedLibrary.SharedKernel.Enum;
using SharedLibrary.SharedKernel.Http.DTOs.ApiResponse;
using SharedLibrary.SharedKernel.Http.DTOs.User;
using SharedLibrary.SharedKernel.Pagination;
using SharedLibrary.SharedKernel.ServiceResult;

namespace ResourceService.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICloudinaryServiceProvider _cloudinary;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEmailService _emailService;
        private readonly string _userServiceUrl;

        public PostService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICloudinaryServiceProvider cloudinary,
            IHttpClientFactory httpClientFactory,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cloudinary = cloudinary;
            _httpClientFactory = httpClientFactory;
            _emailService = emailService;
            _userServiceUrl = configuration["USERSERVICE:URL"];
        }
        public async Task<Result<Guid>> CommentOnPost(Guid postId, CommentPostDTO request)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(postId);

            var comment = new PostComment
            {
                PostId = postId,
                AuthorId = request.UserId,
                Content = request.Content,
                ParentCommentId = request.ParentCommentId,
            };

            await _unitOfWork.Repository<PostComment>().CreateAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            return Result<Guid>.Success(comment.Id);
        }

        public async Task<Result<bool>> CreatePost(CreatePostDTO createPostDto, Guid authorId)
        {

            var postId = Guid.NewGuid();
            var post = new Post
            {
                Id = postId,
                Title = createPostDto.Title,
                Content = createPostDto.Content,
                AuthorId = authorId,
                Status = PostStatus.Pending,
                Images = new List<PostImage>(),
                Videos = new List<PostVideo>(),
                Reviews = new List<PostReview>(),
                Tags = new List<Tag>(),
                Categories = new List<Category>()
            };
            if (createPostDto.Images != null && createPostDto.Images.Any())
            {
                var imageOrders = createPostDto.ImageOrders ?? new List<int>();
                for (int i = 0; i < createPostDto.Images.Count; i++)
                {
                    var img = createPostDto.Images[i];
                    if (img != null && img.Length > 0)
                    {
                        var url = _cloudinary.UploadImageFormFileResourceToCloudinary(img, $"post-img-{postId}-{i}");
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            var order = i < imageOrders.Count ? imageOrders[i] : i;
                            post.Images.Add(new PostImage
                            {
                                PostId = postId,
                                Url = url,
                                Order = order,
                            });
                        }
                    }
                }
            }

            if (createPostDto.Videos != null && createPostDto.Videos.Any())
            {
                var videoOrders = createPostDto.VideoOrders ?? new List<int>();
                for (int i = 0; i < createPostDto.Videos.Count; i++)
                {
                    var vid = createPostDto.Videos[i];
                    if (vid != null && vid.Length > 0)
                    {
                        var url = _cloudinary.UploadVideoFormFileResourceToCloudinary(vid, $"post-vid-{postId}-{i}");
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            var order = i < videoOrders.Count ? videoOrders[i] : i;
                            post.Videos.Add(new PostVideo
                            {
                                PostId = postId,
                                Url = url,
                                Order = order,
                            });
                        }
                    }
                }
            }

            // Load and assign Tags
            if (createPostDto.TagIds != null && createPostDto.TagIds.Any())
            {
                var tagIds = createPostDto.TagIds.Distinct().ToList();
                var tags = await _unitOfWork.Repository<Tag>().GetAllAsync(
                    filter: t => tagIds.Contains(t.Id) && !t.IsDeleted);
                post.Tags = tags.ToList();
            }

            // Load and assign Categories
            if (createPostDto.CategoryIds != null && createPostDto.CategoryIds.Any())
            {
                var categoryIds = createPostDto.CategoryIds.Distinct().ToList();
                var categories = await _unitOfWork.Repository<Category>().GetAllAsync(
                    filter: c => categoryIds.Contains(c.Id) && !c.IsDelete);
                post.Categories = categories.ToList();
            }

            await _unitOfWork.PostRepository.CreateAsync(post);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<PaginatedList<PostsDTO>>> GetPosts(PostsFilterDTO postsFilterDTO)
        {

            var filter = postsFilterDTO ?? new PostsFilterDTO();
            var page = filter.PageNumber <= 0 ? 1 : filter.PageNumber;
            var pageSize = filter.PageSize <= 0 ? 10 : filter.PageSize;

            var posts = await _unitOfWork.PostRepository.GetAllAsync(
                filter: p =>
                    !p.IsDeleted &&
                    (string.IsNullOrEmpty(filter.Title) || p.Title.Contains(filter.Title)) &&
                    (!filter.Status.HasValue || p.Status == filter.Status.Value) &&
                    (string.IsNullOrEmpty(filter.Tag) || p.Tags != null && p.Tags.Any(t => !t.IsDeleted && (t.Name.Contains(filter.Tag) || t.Slug.Contains(filter.Tag)))) &&
                    (string.IsNullOrEmpty(filter.Category) || p.Categories != null && p.Categories.Any(c => !c.IsDelete && c.Name.Contains(filter.Category))),
                orderBy: q => q.OrderByDescending(p => p.CreatedAt),
                include_properties: "Images,Videos,Tags,Categories,Comments,Reactions",
                disable_tracking: true);

            var paged = PaginatedList<Post>.Create(posts, page, pageSize);

            // Lấy tất cả userIds từ authors, comments, và reactions
            var authorIds = paged.PageContent.Select(p => p.AuthorId).Distinct().ToList();
            var commentAuthorIds = paged.PageContent
                .Where(p => p.Comments != null)
                .SelectMany(p => p.Comments.Where(c => !c.IsDeleted).Select(c => c.AuthorId))
                .Distinct()
                .ToList();
            var reactionUserIds = paged.PageContent
                .Where(p => p.Reactions != null)
                .SelectMany(p => p.Reactions.Where(r => !r.IsDeleted).Select(r => r.UserId))
                .Distinct()
                .ToList();

            var allUserIds = authorIds.Union(commentAuthorIds).Union(reactionUserIds).Distinct().ToList();
            var users = await GetUsersByIdsAsync(allUserIds);
            var userDict = users?.ToDictionary(u => u.UserId, u => u) 
                ?? new Dictionary<Guid, UserDetailDTO>();

            var dtoItems = paged.PageContent.Select(p => 
            {
                userDict.TryGetValue(p.AuthorId, out var author);
                
                // Map comments
                var comments = p.Comments?
                    .Where(c => !c.IsDeleted)
                    .Select(c =>
                    {
                        userDict.TryGetValue(c.AuthorId, out var commentAuthor);
                        return new CommentDTO
                        {
                            CommentId = c.Id,
                            AuthorId = c.AuthorId,
                            AuthorName = commentAuthor?.FullName ?? string.Empty,
                            AuthorAvatar = commentAuthor?.AvatarUrl ?? string.Empty,
                            Content = c.Content,
                            ParentCommentId = c.ParentCommentId,
                            CreatedAt = c.CreatedAt
                        };
                    })
                    .ToList() ?? new List<CommentDTO>();

                // Map reactions
                var reactions = p.Reactions?
                    .Where(r => !r.IsDeleted)
                    .Select(r =>
                    {
                        userDict.TryGetValue(r.UserId, out var reactionUser);
                        return new ReactionDTO
                        {
                            UserId = r.UserId,
                            UserName = reactionUser?.FullName ?? string.Empty,
                            UserAvatar = reactionUser?.AvatarUrl ?? string.Empty,
                            Type = r.Type,
                            CreatedAt = r.CreatedAt
                        };
                    })
                    .ToList() ?? new List<ReactionDTO>();

                return new PostsDTO
                {
                    PostId = p.Id,
                    Title = p.Title,
                    AuthorId = p.AuthorId,
                    AuthorName = author?.FullName ?? string.Empty,
                    AuthorAvatar = author?.AvatarUrl ?? string.Empty,
                    Content = p.Content,
                    Images = p.Images?.OrderBy(i => i.Order).Select(i => new Image { Url = i.Url, Order = i.Order }).ToList() ?? new List<Image>(),
                    Videos = p.Videos?.OrderBy(v => v.Order).Select(v => new Videos { Url = v.Url, Order = v.Order }).ToList() ?? new List<Videos>(),
                    Status = p.Status,
                    CommentCount = comments.Count,
                    LikeCount = reactions.Count(r => r.Type == ReactionType.Like),
                    Comments = comments,
                    Reactions = reactions,
                    CreatedAt = p.CreatedAt,
                    LastModifiedAt = p.LastModifiedAt
                };
            }).ToList();

            var result = new PaginatedList<PostsDTO>
            {
                CurrentPage = paged.CurrentPage,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount,
                PageContent = dtoItems
            };

            return Result<PaginatedList<PostsDTO>>.Success(result);

        }

        private async Task<List<UserDetailDTO>?> GetUsersByIdsAsync(IEnumerable<Guid> userIds)
        {
            var ids = userIds.Where(x => x != Guid.Empty).Distinct().ToList();
            if (!ids.Any()) return null;

            try
            {
                var client = _httpClientFactory.CreateClient();
                var url = $"{_userServiceUrl}/api/users/ids";
                var resp = await client.PostAsJsonAsync(url, ids);
                if (!resp.IsSuccessStatusCode) return null;

                var result = await resp.Content.ReadFromJsonAsync<DefaultApiResponse<List<UserDetailDTO>>>();
                return result?.Value;
            }
            catch
            {
                return null;
            }
        }


        public async Task<Result<bool>> ReactToPost(Guid postId, Guid driverId, ReactPostDTO request)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(postId);
            if (post == null || post.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{postId}"), "Post not found");
            }

            var reaction = new PostReaction
            {
                PostId = postId,
                UserId = driverId,
                Type = request.Type,
            };

            await _unitOfWork.Repository<PostReaction>().CreateAsync(reaction);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UpdatePost(Guid postId, Guid reviewId, UpdatePostSDTO updatePostSDTO)
        {

            var result = await _unitOfWork.PostRepository.UpdateStatusAsync(
                postId,
                updatePostSDTO.Status.Value,
                updatePostSDTO.Reason,
                reviewId);
            await _unitOfWork.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> RejectPost(Guid postId, RejectPostDTO rejectPostDTO, Guid? reviewerId = null)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(postId);
            if (post == null || post.IsDeleted)
            {
                return Result<bool>.Failure(ServiceError.NotFoundError($"{postId}"), "Post not found");
            }

            var reason = rejectPostDTO?.Reason ?? "Bài viết không đáp ứng tiêu chuẩn nội dung của DriveMate";

            var updateResult = await _unitOfWork.PostRepository.UpdateStatusAsync(
                postId,
                PostStatus.Rejected,
                reason,
                reviewerId);
            
            if (!updateResult)
            {
                return Result<bool>.Failure(ServiceError.UnhandledException("Failed to update post status"), "Failed to reject post");
            }

            await _unitOfWork.SaveChangesAsync();

            var author = await GetUsersByIdsAsync(new[] { post.AuthorId });
            var authorInfo = author?.FirstOrDefault();

            if (authorInfo != null && !string.IsNullOrEmpty(authorInfo.Email))
            {
                var replaceTerms = new Dictionary<string, string>
                {
                    { "username", authorInfo.FullName ?? authorInfo.Email },
                    { "post_title", post.Title },
                    { "post_content", post.Content.Length > 100 ? post.Content.Substring(0, 100) + "..." : post.Content },
                    { "reason", reason }
                };

                // Send rejection email
                var emailSent = await _emailService.SendingEmail(
                    authorInfo.Email,
                    replaceTerms,
                    "[DriveMate] Thông báo: Bài viết của bạn đã bị từ chối",
                    EmailType.PostRejected);

                if (!emailSent)
                {
                    // Log email failure but don't fail the operation
                    Console.WriteLine($"Failed to send rejection email to {authorInfo.Email}");
                }
            }

            return Result<bool>.Success(true);
        }


    }
}
