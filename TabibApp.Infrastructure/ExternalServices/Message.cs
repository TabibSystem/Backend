using MimeKit;

namespace EmailService
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }

        public string Content { get; set; }
        public List<MimePart> Attachments { get; set; } // Add this property


        public Message(IEnumerable<string> to, string subject, string content, string displayName = null, List<MimePart> attachments = null)
        {
            // Create the MailboxAddress instances with optional display names
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(email => new MailboxAddress(displayName, email)));

            Subject = subject;
            Content = content;
            Attachments = attachments ?? new List<MimePart>(); // Initialize if null

        }

    }
}
