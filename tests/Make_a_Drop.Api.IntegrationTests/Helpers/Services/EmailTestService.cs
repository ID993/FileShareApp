using System.Threading.Tasks;
using Make_a_Drop.Application.Common.Email;
using Make_a_Drop.Application.Services;

namespace Make_a_Drop.Api.IntegrationTests.Helpers.Services;

public class EmailTestService : IEmailService
{
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await Task.Delay(100);
    }
}
