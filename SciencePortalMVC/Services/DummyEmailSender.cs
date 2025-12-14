using Microsoft.AspNetCore.Identity.UI.Services;

namespace SciencePortalMVC.Services
{
    public class DummyEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Просто возвращаем завершенную задачу, ничего не отправляя
            return Task.CompletedTask;
        }
    }
}