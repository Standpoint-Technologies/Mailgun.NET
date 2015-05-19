using System;

namespace Mailgun.Exceptions
{
    public class MailgunServerException : Exception
    {
        internal MailgunServerException(string message)
            : base(message)
        { }
    }
}
