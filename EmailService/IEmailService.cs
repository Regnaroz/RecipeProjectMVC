using RecipeProject.EmailService.Emails;

namespace RecipeProject.EmailService
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        Task SendUserOrderAsync(MailRequest mailRequest);
    }
}
