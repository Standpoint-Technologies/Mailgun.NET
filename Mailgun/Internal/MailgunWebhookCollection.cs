using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mailgun.Internal
{
    class MailgunWebhookCollection
    {
        public IDictionary<string, MailgunWebhook> Webhooks { get; set; }

        [JsonIgnore]
        public IEnumerable<MailgunWebhook> ReconfiguredWebhooks
        {
            get
            {
                foreach (var webhook in Webhooks)
                {
                    MailgunWebhookType type;
                    if (Enum.TryParse(webhook.Key, out type))
                    {
                        webhook.Value.Type = type;
                    }
                    yield return webhook.Value;
                }
            }
        }
    }
}
