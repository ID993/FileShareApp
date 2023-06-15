
using Make_a_Drop.Application.Common.Email;

namespace Make_a_Drop.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);

}
