using System;

namespace Mailgun.Exceptions
{
    public class MailgunBadRequest : Exception
    {
        internal MailgunBadRequest(string message)
            : base(message)
        { }
    }
}
