using System.Collections.Generic;

namespace Mailgun
{
    public class MailgunAddress
    {
        public string EmailAddress { get; set; }

        public string Name { get; set; }

        public IDictionary<string, string> RecipientVariables { get; set; }


        public MailgunAddress()
        {
            RecipientVariables = new Dictionary<string, string>();
        }

        public MailgunAddress(string emailAddress, string name)
            : this()
        {
            EmailAddress = emailAddress;
            Name = name;
        }


        public override string ToString()
        {
            return string.Format("{0} <{1}>", Name, EmailAddress);
        }
    }
}
