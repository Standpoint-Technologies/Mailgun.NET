using System;
using System.Collections.Generic;
using Mailgun.Internal;
using Newtonsoft.Json;

namespace Mailgun
{
    public class MailgunDomain
    {
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        public string Name { get; set; }

        [JsonProperty("smtp_login")]
        public string SmtpLogin { get; set; }

        [JsonProperty("smtp_password")]
        public string SmtpPassword { get; set; }

        [JsonProperty("spam_action")]
        public MailgunSpamAction SpamAction { get; set; }

        public MailgunDomainState State { get; set; }

        public MailgunDomainType Type { get; set; }

        public bool Wildcard { get; set; }


        /// <summary>
        /// Converts the MailgunDomain into a collection of key-value pairs for serialization.
        /// </summary>
        /// <returns></returns>
        internal ICollection<KeyValuePair<string, string>> ToKeyValuePair()
        {
            var v = new List<KeyValuePair<string, string>>();

            v.Add(new KeyValuePair<string, string>("name", Name));
            v.Add(new KeyValuePair<string, string>("spam_action", Utilities.GetEnumStringValue(SpamAction)));
            v.Add(new KeyValuePair<string, string>("wildcard", Wildcard.ToString()));

            if (!String.IsNullOrEmpty(SmtpPassword))
            {
                v.Add(new KeyValuePair<string, string>("smtp_password", SmtpPassword));
            }

            return v;
        }
    }
}
