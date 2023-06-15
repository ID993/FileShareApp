using Make_a_Drop.Core.Entities.Identity;

namespace Make_a_Drop.Application.Models.Comment
{
    public class CommentResponseModel : BaseResponseModel
    {
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }
        public ApplicationUser User { get; set; }
    }
}
