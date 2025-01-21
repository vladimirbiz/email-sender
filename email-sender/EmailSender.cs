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
        // Create the email message
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Vladimir", username));
        message.To.Add(new MailboxAddress(obj.Name, obj.Email));
        message.Subject = $"Improvement idea for {obj.Name} :)";

        // Email body based on criteria
        string body = GenerateBody(obj, criteria);  // Pass criteria here

        var bodyBuilder = new BodyBuilder { HtmlBody = body };

        // Attach the image (removed ContentTransferEncoding)
        var imagePath = "signature.png";
        var image = new MimePart("image", "png")
        {
            Content = new MimeContent(File.OpenRead(imagePath)),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            FileName = Path.GetFileName(imagePath)
        };
        bodyBuilder.Attachments.Add(image);

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
                Hi,<br><br>
                I looked at your business and I can see that you are doing an amazing job. But I noticed that you don't have social media accounts linked on your website.<br>
                If you want, I can help you set that up for free and also tell you about some other improvements that can be made.<br>
                Feel free to schedule a call with me and let's talk more :) <br>
                <a href='https://calendly.com/tasevskimarketingagency/intro'>Here is a link to my Calendar</a> <br><br>
                Best Regards,<br>
                Vladimir";
                break;
            case 2:
                body = @"
                Hi,<br><br>
                I looked at your business and I can see that you are doing an amazing job. But I noticed that you don't have an Instagram account linked on your website.<br>
                If you want, I can help you set that up for free and also tell you about some other improvements that can be made.<br>
                Feel free to schedule a call with me and let's talk more :) <br>
                <a href='https://calendly.com/tasevskimarketingagency/intro'>Here is a link to my Calendar</a> <br><br>
                Best Regards,<br>
                Vladimir";
                break;
            case 3:
                body = @"
                Hi,<br><br>
                I looked at your business and I can see that you are doing an amazing job. But I noticed that you don't have a Facebook account linked on your website.<br>
                If you want, I can help you set that up for free and also tell you about some other improvements that can be made.<br>
                Feel free to schedule a call with me and let's talk more :) <br>
                <a href='https://calendly.com/tasevskimarketingagency/intro'>Here is a link to my Calendar</a> <br><br>
                Best Regards,<br>
                Vladimir";
                break;
            case 4:
                body = $@"
                Hi,<br><br>
                I looked at your business and I can see that you are doing an amazing job. But I noticed that you have {obj.Reviews} reviews on Google :(<br>
                I work with businesses just like yours helping them get more 5* reviews.<br>
                If you want, I can help you set that up for free and also tell you about some other improvements that can be made.<br>
                Feel free to schedule a call with me and let's talk more :) <br>
                <a href='https://calendly.com/tasevskimarketingagency/intro'>Here is a link to my Calendar</a> <br><br>
                Best Regards,<br>
                Vladimir";
                break;
            case 5:
                body = $@"
                Hi,<br><br>
                I looked at your business and I can see that you are doing an amazing job. But I noticed that your average rating on Google is {obj.Avg_Review} :(<br>
                I work with businesses just like yours helping them get more 5* reviews.<br>
                If you want, I can help you set that up for free and also tell you about some other improvements that can be made.<br>
                Feel free to schedule a call with me and let's talk more :) <br>
                <a href='https://calendly.com/tasevskimarketingagency/intro'>Here is a link to my Calendar</a> <br><br>
                Best Regards,<br>
                Vladimir";
                break;
            default:
                body = @"
                Hi,<br><br>
                I looked at your business and I can see that you are doing an amazing job. But I noticed that your website could use an update.<br>
                I work with businesses just like yours helping them build an amazing online presence.<br>
                If you want, let's get on a call so that I can tell you more about my ideas and if it makes sense we might end up working together.<br>
                Feel free to schedule a call with me and let's talk more :) <br>
                <a href='https://calendly.com/tasevskimarketingagency/intro'>Here is a link to my Calendar</a> <br><br>
                Best Regards,<br>
                Vladimir";
                break;
        }

        return body;
    }
}
