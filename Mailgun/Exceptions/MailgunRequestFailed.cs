using System;

namespace Mailgun.Exceptions
{
    public class MailgunRequestFailed : Exception
    {
        internal MailgunRequestFailed(string message)
            : base(message)
        { }
    }
}
