using FluentValidation;
using Make_a_Drop.Application.Models.Collaboration;

namespace Make_a_Drop.Application.Validators.Collaborations
{
    public class CreateCollaborationModelValidator : AbstractValidator<CreateCollaborationModel>
    {
        public CreateCollaborationModelValidator()
        {

            RuleFor(d => d.Name)
                .MinimumLength(CollaborationValidatorConfiguration.MinimumNameLength)
                .WithMessage($"Name should have minimum {CollaborationValidatorConfiguration.MinimumNameLength} characters")
                .MaximumLength(CollaborationValidatorConfiguration.MaximumNameLength)
                .WithMessage($"Name should have maximum {CollaborationValidatorConfiguration.MaximumNameLength} characters");

        }

    }
}
