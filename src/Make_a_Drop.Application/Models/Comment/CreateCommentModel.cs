using System.ComponentModel.DataAnnotations;

namespace Make_a_Drop.Application.Models.Comment
{
    public class CreateCommentModel
    {
        [Required]
        public string Text { get; set; }

    }
}
