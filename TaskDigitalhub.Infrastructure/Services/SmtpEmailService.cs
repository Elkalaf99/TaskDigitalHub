using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaskDigitalhub.Application.Common.Interfaces;

namespace TaskDigitalhub.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var host = _configuration["Email:SmtpHost"];
        var port = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
        var from = _configuration["Email:From"];
        var user = _configuration["Email:UserName"];
        var password = _configuration["Email:Password"];
        var enabled = bool.Parse(_configuration["Email:Enabled"] ?? "false");

        if (!enabled || string.IsNullOrEmpty(host))
        {
            _logger.LogInformation("Email disabled or not configured. Would send to {To}: {Subject}", to, subject);
            return;
        }

        try
        {
            using var client = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = !string.IsNullOrEmpty(user) ? new NetworkCredential(user, password) : null
            };

            var message = new MailMessage(
                from ?? "noreply@taskdigitalhub.com",
                to,
                subject,
                body)
            {
                IsBodyHtml = true
            };

            await client.SendMailAsync(message);
            _logger.LogInformation("Email sent to {To}: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To}", to);
            throw;
        }
    }
}
