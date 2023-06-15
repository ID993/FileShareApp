using AutoMapper;
using Make_a_Drop.Application.Exceptions;
using Make_a_Drop.Application.Models.Comment;
using Make_a_Drop.Core.Entities;
using Make_a_Drop.Core.Entities.Identity;
using Make_a_Drop.DataAccess.Repositories;
using Make_a_Drop.Shared.Services;
using Microsoft.AspNetCore.Identity;

namespace Make_a_Drop.Application.Services.Impl
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly IDropRepository _dropRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IClaimService _claimService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentService(IDropRepository dropRepository, IClaimService claimService,
                       IMapper mapper, UserManager<ApplicationUser> userManager, 
                       ICommentRepository commentRepository)
        {
            _dropRepository = dropRepository;
            _claimService = claimService;
            _mapper = mapper;
            _userManager = userManager;
            _commentRepository = commentRepository;


        }

        public async Task<CommentResponseModel> CreateAsync(CreateCommentModel createCommentModel, Guid dropId,
            CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"\n\ndropid: {dropId}\n\ntext: {createCommentModel.Text}");
            var user = await _userManager.FindByIdAsync(_claimService.GetUserId());
            var drop = await _dropRepository.GetFirstAsync(d => d.Id == dropId);

            var comment = new Comment
            {
                Text = createCommentModel.Text,
                User = user,
                Drop = drop
            };
            var savedComment = await _commentRepository.AddAsync(comment);

            return new CommentResponseModel
            {
                Id = savedComment.Id
            };
        }

        public async Task<Comment> DeleteAsync(Guid? commentId)
        {
            var comment = await _commentRepository.GetFirstAsync(c => c.Id == commentId) ?? throw new NotFoundException("Comment not found");
            Console.WriteLine($"\n\ncomment: {comment.Drop.Id}\n\n");
            return await _commentRepository.DeleteAsync(comment);
        }

        public async Task<List<CommentResponseModel>> GetAllAsync()
        {
            var currentUserId = _claimService.GetUserId() ?? throw new NotFoundException("User not found.");
            var commentList = await _commentRepository.GetAllAsync(dl => dl.User.Id == currentUserId);
            return _mapper.Map<List<CommentResponseModel>>(commentList);
        }

        public async Task<List<CommentResponseModel>> GetAllByDropIdAsync(Guid dropId)
        {
            var commentList = await _commentRepository.GetAllByDropIdAsync(c => c.Drop.Id == dropId);
            return _mapper.Map<List<CommentResponseModel>>(commentList);
        }

        public async Task<List<CommentResponseModel>> GetAllByUserIdAsync(Guid userId)
        {
            var commentList = await _commentRepository.GetAllAsync(c => c.User.Id == userId.ToString());
            return _mapper.Map<List<CommentResponseModel>>(commentList);
        }

        public async Task<CommentResponseModel> GetByIdAsync(Guid? commentId)
        {
            var comment = await _commentRepository.GetFirstAsync(c => c.Id == commentId);
            return _mapper.Map<CommentResponseModel>(comment);
        }

        public List<CommentResponseModel> OrderByDateAsc(Guid dropId)
        {
            var commentList = _commentRepository.OrderByDateAsc(cl => cl.Drop.Id == dropId);
            return _mapper.Map<List<CommentResponseModel>>(commentList);
        }

        public List<CommentResponseModel> OrderByDateDesc(Guid dropId)
        {
            var commentList = _commentRepository.OrderByDateDesc(cl => cl.Drop.Id == dropId);
            return _mapper.Map<List<CommentResponseModel>>(commentList);
        }

        public List<CommentResponseModel> PaginatedList(int page, int pageSize, Guid dropId)
        {
            var commentList = _commentRepository.PaginatedList(dl => dl.Drop.Id == dropId, page, pageSize);
            return _mapper.Map<List<CommentResponseModel>>(commentList);
        }
    }
}
