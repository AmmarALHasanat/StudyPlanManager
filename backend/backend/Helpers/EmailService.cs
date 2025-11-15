using MailKit.Net.Smtp;
using MimeKit;
namespace backend.Helpers
{
    public interface IEmailService
    {
        void Send(string to, string subject, string body);
        Task SendAsync(string to, string subject, string body);
    }
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config) { _config = config; }
        public void Send(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_config["Smtp:From"]));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            client.Connect(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"] ?? "587"), MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(_config["Smtp:User"], _config["Smtp:Pass"]);
            client.Send(message);
            client.Disconnect(true);
        }

        public Task SendAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_config["Smtp:From"]));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };


            using var client = new SmtpClient();
            client.Connect(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"] ?? "587"), MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(_config["Smtp:User"], _config["Smtp:Pass"]);
            client.Send(message);
            client.Disconnect(true);
            return Task.CompletedTask;
        }
    }
}
