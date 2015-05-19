using System;

namespace Mailgun.Exceptions
{
    class MailgunNotFoundException : Exception
    {
        public Uri Resource { get; private set; }

        internal MailgunNotFoundException(Uri resource)
            : base()
        {
            Resource = resource;
        }

        public override string Message
        {
            get
            {
                return String.Format("No resource found at location {0}", Resource);
            }
        }
    }
}
