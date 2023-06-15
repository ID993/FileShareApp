using AutoMapper;
using Make_a_Drop.Application.Models.Comment;
using Make_a_Drop.Core.Entities;

namespace Make_a_Drop.Application.MappingProfiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CreateCommentModel>();
            CreateMap<CreateCommentModel, Comment>();
            CreateMap<Comment, CommentResponseModel>();
            CreateMap<CommentResponseModel, Comment>();
        }
    }
}
