using Make_a_Drop.Application.Models.Comment;
using Make_a_Drop.Core.Entities;

namespace Make_a_Drop.Application.Services
{
    public interface ICommentService
    {
        Task<CommentResponseModel> CreateAsync(CreateCommentModel createCommentModel, Guid dropId,
             CancellationToken cancellationToken = default);
        Task<List<CommentResponseModel>> GetAllAsync();

        Task<List<CommentResponseModel>> GetAllByDropIdAsync(Guid dropId);

        Task<List<CommentResponseModel>> GetAllByUserIdAsync(Guid userId);

        Task<CommentResponseModel> GetByIdAsync(Guid? commentId);

        Task<Comment> DeleteAsync(Guid? commentId);

        List<CommentResponseModel> OrderByDateAsc(Guid dropId);

        List<CommentResponseModel> OrderByDateDesc(Guid dropId);

        List<CommentResponseModel> PaginatedList(int page, int pageSize, Guid dropId);
    }
}
