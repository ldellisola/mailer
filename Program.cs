using System.CommandLine.DragonFruit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Mailer;

internal class Program
{
    /// <summary>
    /// A simple tool to send an email using smtp.
    /// </summary>
    /// <param name="fromAddress">Email address of the sender</param>
    /// <param name="toAddress">Email address of the receiver</param>
    /// <param name="subject">Subject of the email</param>
    /// <param name="content">Text content of the email</param>
    /// <param name="password">Password for the email provider</param>
    /// <param name="username">Username for the email provider</param>
    /// <param name="fromName">Name of the sender. It default to the sender's email</param>
    /// <param name="toName">Name of the receiver. It defaults to the receiver's email</param>
    /// <param name="server">Address of the email provider</param>
    /// <param name="port">Port of the email provider</param>
    /// <param name="useSsl">Whether to use ssl</param>
    private static void Main(
        string fromAddress, string toAddress, string subject, string content,
        string password, string? username = null,
        string? fromName = null,
        string? toName = null,
        string server = "smtp.gmail.com",
        int port = 587,
        bool useSsl = false
    )
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName ?? fromAddress, fromAddress));
            message.To.Add(new MailboxAddress(toName ?? toAddress, toAddress));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = content
            };

            using var client = new SmtpClient();
            client.Connect(server, port, useSsl);
            client.Authenticate(username ?? fromAddress, password);
            client.Send(message);
            client.Disconnect(true);
        }
        catch (AuthenticationException)
        {
            Console.WriteLine("Authentication failed");
        }
    }
}