
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfigration emailConfigration;

        public EmailSender(EmailConfigration _emailConfigration)
        {
            emailConfigration = _emailConfigration;
        }
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);

        }

     

        public void CreateAccountConfirmationEmail(ApplicationUser user, string callbackUrl)
        {
            var content = @$"
<html>
   <body>
      <h1> {user.FirstName}  مرحبا<h1> 
<h2>شكرًا لانضمامك إلى تطبيق طبيب. لتفعيل حسابك، يرجى الضغط على الرابط التالي لتأكيد بريدك الإلكتروني <h2>
<h3><a href='{callbackUrl}'>تأكيد البريد الالكترونى</a><h3>
<br>
<h4>مع تحيات </h4>
 <h4>فريق تطبيق طبيب</h4>
<img src='C:\Users\LAPTOP WORLD\OneDrive\Desktop\photo_2024-06-13_01-25-59.jpg'/>
   </body>
</html>

      ";
             
            
          

            var message = new Message(new[] { user.Email }, "تأكيد حسابك في تطبيق طبيب", content, user.FirstName);
            
            
            SendEmail(message);
        }

        public void CreatePasswordConfirmationEmail(ApplicationUser user, string callbackUrl)
        {
            var content = @$"
<html>
   <body>
      <h1>مرحبا {user.FirstName}</h1> 
      <h2>لقد طلبت إعادة تعيين كلمة المرور الخاصة بك.</h2>
      <h3>لتعيين كلمة مرور جديدة، يرجى الضغط على الرابط التالي:</h3>
      <h3><a href='{callbackUrl}'>إعادة تعيين كلمة المرور</a></h3>
      <br>
      <h4>مع تحيات،</h4>
      <h4>فريق تطبيق طبيب</h4>
   </body>
</html>";

       
            // Create the email message
            var message = new Message(new[] { user.Email }, "إعادة تعيين كلمة المرور", content, user.FirstName, null);
    
            // Send the email
            SendEmail(message);
        }


        private void Send(MimeMessage emailMessage)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(emailConfigration.SmtpServer, emailConfigration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(emailConfigration.UserName, emailConfigration.Password);
                    client.Send(emailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }

        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("From TabibApp", emailConfigration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message.Content
            };
            foreach (var attachment in message.Attachments)
            {
                bodyBuilder.Attachments.Add(attachment);
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }
    }
}
