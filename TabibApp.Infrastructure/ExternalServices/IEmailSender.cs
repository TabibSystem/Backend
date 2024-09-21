using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailService
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        void CreateAccountConfirmationEmail(ApplicationUser user, string callbackUrl);
        void CreatePasswordConfirmationEmail(ApplicationUser user, string callbackUrl);

    }
}
