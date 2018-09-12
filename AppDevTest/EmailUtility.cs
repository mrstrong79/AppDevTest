using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace AppDevTest
{
    class EmailUtility
    {

        public static void CreateMessage(string server)
        {
            MailAddress from = new MailAddress("ben@contoso.com", "Ben Miller");
            MailAddress to = new MailAddress("jane@contoso.com", "Jane Clayton");
            MailMessage message = new MailMessage(from, to);
            MailAddress bcc = new MailAddress("manager1@contoso.com");
            message.Bcc.Add(bcc);
            message.Subject = "Using the SmtpClient class.";
            message.Body = @"Using this feature, you can send an e-mail message from an application very easily.";
            SmtpClient client = new SmtpClient(server);
            client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.Send(message);

            //You can add multiple recipients by separating the addresses with a comma (,) when passing the address to an overload that accepts a string
            //MailMessage message = new MailMessage();
            //message.To.Add("jane@contoso.com,bob@contoso.com");
            //or
            //MailMessage message = new MailMessage("ben@contoso.com", "jane@contoso.com,bob@contoso.com"); // from ben, to jane & bob
        }

        /// <summary>
        /// Create a mail message that contains an excel file as an attachment
        /// </summary>
        /// <param name="server"></param>
        public static void CreateMessageWithAttachment(string server)
        {
            // Specify the file to be attached and sent.
            // This example assumes that a file named Data.xls exists in the
            // current working directory.
            string file = "data.xls";
            // Create a message and set up the recipients.
            MailMessage message = new MailMessage(
               "jane@contoso.com",
               "ben@contoso.com",
               "Quarterly data report.",
               "See the attached spreadsheet.");

            // Create  the file attachment for this e-mail message.
            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet); // Stream or String constructors - depending on the attachment
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
            // Add the file attachment to this e-mail message.
            message.Attachments.Add(data);
            //Send the message.
            SmtpClient client = new SmtpClient(server);
            // Add credentials if the SMTP server requires them.
            client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.Send(message);
            // Display the values in the ContentDisposition for the attachment.
            ContentDisposition cd = data.ContentDisposition;
            Console.WriteLine("Content disposition");
            Console.WriteLine(cd.ToString());
            Console.WriteLine("File {0}", cd.FileName);
            Console.WriteLine("Size {0}", cd.Size);
            Console.WriteLine("Creation {0}", cd.CreationDate);
            Console.WriteLine("Modification {0}", cd.ModificationDate);
            Console.WriteLine("Read {0}", cd.ReadDate);
            Console.WriteLine("Inline {0}", cd.Inline);
            Console.WriteLine("Parameters: {0}", cd.Parameters.Count);
            foreach (DictionaryEntry d in cd.Parameters)
            {
                Console.WriteLine("{0} = {1}", d.Key, d.Value);
            }

            data.Dispose();
        }


        /// <summary>
        /// Example below creates a MailMessage and uses the LinkedResource class to embed an image in the message.
        /// The LinkedResource class is mainly used for creating embedded images. 
        /// To create an embedded image you will need to:
        /// 1) create a Html formatted AlternateView
        /// 2) Within that alternate view you create an  tag, that points to the ContentId (CID) of the LinkedResource
        /// 3) You then create a LinkedResource object and add it to the AlternateView's LinkedResources collection.
        /// </summary>
        public static void CreateMessageWithEmbeddedResource()
        {

            //create the mail message
            MailMessage mail = new MailMessage();

            //set the addresses
            mail.From = new MailAddress("me@mycompany.com");
            mail.To.Add("you@yourcompany.com");

            //set the content
            mail.Subject = "This is an email";

            //first we create the Plain Text part
            AlternateView plainView =
                AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content, viewable by those clients that don't support html", null,
                    "text/plain");

            //then we create the Html part
            //to embed images, we need to use the prefix 'cid' in the img src value
            //the cid value will map to the Content-Id of a Linked resource.
            //thus <img src='cid:companylogo'> will map to a LinkedResource with a ContentId of 'companylogo'
            AlternateView htmlView =
                AlternateView.CreateAlternateViewFromString("Here is an embedded image.<img src=cid:companylogo>", null,
                                                            "text/html");

            //create the LinkedResource (embedded image)
            LinkedResource logo = new LinkedResource("c:\\temp\\logo.gif");
            logo.ContentId = "companylogo";
            //add the LinkedResource to the appropriate view
            htmlView.LinkedResources.Add(logo);

            //add the views
            mail.AlternateViews.Add(plainView);
            mail.AlternateViews.Add(htmlView);


            //send the message
            SmtpClient smtp = new SmtpClient("127.0.0.1"); //specify the mail server address
            smtp.Send(mail);

        }
    }
}
