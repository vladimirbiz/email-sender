using System;
using System.IO;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Net.Imap;
using MailKit;
using MimeKit.Utils;

public class EmailSender
{
    private static string smtpServer = "smtp.hostinger.com";
    private static int smtpPort = 587;
    private static string username = "vladimir@tasevskimarketingagency.com";
    private static string password = "Tasevski/1999";
    private static string imapServer = "imap.hostinger.com";
    private static int imapPort = 993;

    public static void SendEmail(Business obj, int criteria)
    {
        Console.WriteLine();
        Console.WriteLine(obj);
        Console.WriteLine("This is the meail" + obj.Email);
        Console.WriteLine();
        // Create the email message
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Vladimir", username));
        message.To.Add(new MailboxAddress(obj.Name, obj.Email));
        message.Subject = $"Improvement idea for {obj.Name} :)";

        // Email body based on criteria
        string body = GenerateBody(obj, criteria);  // Pass criteria here

        var bodyBuilder = new BodyBuilder { HtmlBody = body };

        // Assign the body to the email message
        message.Body = bodyBuilder.ToMessageBody();

        // Send the email using SMTP
        using (var smtpClient = new SmtpClient())
        {
            smtpClient.Connect(smtpServer, smtpPort, false);
            smtpClient.Authenticate(username, password);
            smtpClient.Send(message);
            smtpClient.Disconnect(true);
        }

        Console.WriteLine("Email sent successfully!");

        // Connect to IMAP server and save the email in the Sent folder
        using (var imapClient = new ImapClient())
        {
            imapClient.Connect(imapServer, imapPort, true);
            imapClient.Authenticate(username, password);
            var sentFolder = imapClient.GetFolder(SpecialFolder.Sent);
            sentFolder.Append(message, MessageFlags.Seen);
            imapClient.Disconnect(true);
        }

        Console.WriteLine("Email saved in Sent folder.");
    }

    private static string GenerateBody(Business obj, int criteria)
    {
        string body = string.Empty;

        switch (criteria)
        {
            case 1:
                body = @"
                Hello,<br><br>
                I stumbled across your work and I saw that you are doing a great job. But I saw that there are some game changing improvements that can be made regarding your online presence.<br>
                If you are interested in hearing me out, let's talk :) <br>
                <a href='https://tasevskimarketing.com'>Here is the link to my agency's website</a> <br><br>
                Best Regards,<br>
                Vladimir";
                break;
            case 2:
                body = $@"
                Hello,<br><br>
                I stumbled across your work and I saw that you are doing a great job. But I saw that there are some game changing improvements that can be made regarding your online presence.<br>
                A big one is that you only have {obj.Reviews} reviews on Google.<br>
                We can help you implement a process that gets you hundreds of 5* reviews.<br>
                If you are interested in hearing me out, let's talk :) <br>
                <a href='https://tasevskimarketing.com'>Here is the link to my agency's website</a> <br><br>
                Best Regards,<br>
                Vladimir";
                break;
            case 3:
                body = $@"
                Hello,<br><br>
                I stumbled across your work and I saw that you are doing a great job. But I saw that there are some game changing improvements that can be made regarding your online presence.<br>
                A big one is that your average rating on Google is {obj.Avg_Review}.<br>
                We can help you implement a process that gets you hundreds of 5* reviews.<br>
                If you are interested in hearing me out, let's talk :) <br>
                <a href='https://tasevskimarketing.com'>Here is the link to my agency's website</a> <br><br>
                Best Regards,<br>
                Vladimir";
                break;
            case 4:
                body = @"
                Hello,<br><br>
                
                I stumbled across your work and I saw that you are doing a great job. But I noticed that you don't have a website, and having one could be a game changer.<br>
                If that is something that interests you, lets book a meeting where I can show you some designs.<br>
                If you like a design, we can set it up for you in a day :)<br>
                <a href='https://tasevskimarketing.com'>Here is the link to my agency's website</a> <br><br>
                Best Regards,<br>
                Vladimir";
                break;
        }

        return body;
    }
}
