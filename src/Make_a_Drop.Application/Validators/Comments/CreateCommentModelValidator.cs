using FluentValidation;
using Make_a_Drop.Application.Models.Comment;
using Make_a_Drop.Application.Validators.Comments;

namespace Make_a_Drop.Application.Validators.Comment
{
    public class CreateCommentModelValidator : AbstractValidator<CreateCommentModel>
    {
        public CreateCommentModelValidator()
        {

            RuleFor(d => d.Text)
                .MinimumLength(CommentValidatorConfiguration.MinimumTextLength)
                .WithMessage($"Name should have minimum {CommentValidatorConfiguration.MinimumTextLength} characters")
                .MaximumLength(CommentValidatorConfiguration.MaximumTextLength)
                .WithMessage($"Name should have maximum {CommentValidatorConfiguration.MaximumTextLength} characters");

        }

    }
}
