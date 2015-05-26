using System.Collections.Generic;
using Mailgun.Internal;

namespace Mailgun
{
    public class MailgunWebhook
    {
        public MailgunWebhookType Type { get; set; }

        public string Url { get; set; }


        internal IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            var ret = new Dictionary<string, string>();
            ret.Add("id", Utilities.GetEnumStringValue(Type));
            ret.Add("url", Url);
            return ret;
        }
    }
}
