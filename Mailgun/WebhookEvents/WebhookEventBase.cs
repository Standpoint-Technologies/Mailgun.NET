using System.Collections.Generic;
using System.Collections.Specialized;

namespace Mailgun.WebhookEvents
{
    public abstract class WebhookEventBase
    {
        public string Event { get; set; }

        public string Recipient { get; set; }

        public string Domain { get; set; }

        public string MessageHeaders { get; set; }

        public int Timestamp { get; set; }

        public string Token { get; set; }

        public string Signature { get; set; }


        public bool VerifySignature(string apiKey)
        {
            // TODO: Verify signature with HMAC
            return true;
        }
    }
}
