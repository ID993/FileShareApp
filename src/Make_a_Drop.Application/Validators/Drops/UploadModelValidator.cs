using FluentValidation;
using Make_a_Drop.Application.Models.Drop;

namespace Make_a_Drop.Application.Validators.Drops
{
    public class UploadModelValidator : AbstractValidator<UploadModel>
    {
        public UploadModelValidator()
        {

            RuleFor(d => d.Name)
                .MinimumLength(DropValidatorConfiguration.MinimumNameLength)
                .WithMessage($"Name should have minimum {DropValidatorConfiguration.MinimumNameLength} characters")
                .MaximumLength(DropValidatorConfiguration.MaximumNameLength)
                .WithMessage($"Name should have maximum {DropValidatorConfiguration.MaximumNameLength} characters");


            RuleFor(d => d.SecretKey)
                .MinimumLength(DropValidatorConfiguration.MinimumSecretKeyLength)
                .WithMessage($"Password should have minimum {DropValidatorConfiguration.MinimumSecretKeyLength} characters")
                .MaximumLength(DropValidatorConfiguration.MaximumSecretKeyLength)
                .WithMessage($"Password should have maximum {DropValidatorConfiguration.MaximumSecretKeyLength} characters");

            RuleFor(d => d.File)
                .NotEmpty()
                .WithMessage("File is required")
                .Must(f => f.Length >= DropValidatorConfiguration.MinimumFileLength)
                .WithMessage($"File should have minimum {DropValidatorConfiguration.MinimumFileLength} files")
                .Must(f => f.Length <= DropValidatorConfiguration.MaximumFileLength)
                .WithMessage($"File should have maximum {DropValidatorConfiguration.MaximumFileLength} files");
                
        }

    }
}
