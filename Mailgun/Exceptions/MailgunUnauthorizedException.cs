using System;

namespace Mailgun.Exceptions
{
    public class MailgunUnauthorizedException : Exception
    {
        public string ApiKey { get; private set; }

        internal MailgunUnauthorizedException(string apiKey)
            : base()
        {
            ApiKey = apiKey;
        }

        public override string Message
        {
            get
            {
                return String.Format("Invalid API key: {0}", ApiKey);
            }
        }
    }
}
