using System;
using System.Collections.Generic;
using System.Linq;
using Mailgun.Internal;
using Newtonsoft.Json;

namespace Mailgun
{
    public class MailgunMessage
    {
        public MailgunAddress From { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public string HTML { get; set; }

        public bool TestMode { get; set; }

        public bool? Tracking { get; set; }

        public ClickTrackingOption? TrackingClicks { get; set; }

        public bool? TrackingOpens { get; set; }

        public bool? DKIM { get; set; }

        public DateTime? DeliveryTime { get; set; }

        public string Campaign { get; set; }

        public ICollection<MailgunAddress> To { get; set; }

        public ICollection<MailgunAddress> CC { get; set; }

        public ICollection<MailgunAddress> BCC { get; set; }

        public IDictionary<string, string> CustomHeaders { get; set; }

        public ICollection<string> Tags { get; set; }

        public IDictionary<string, object> Data { get; set; }


        public MailgunMessage()
        {
            To = new List<MailgunAddress>();
            CC = new List<MailgunAddress>();
            BCC = new List<MailgunAddress>();
            Tags = new List<string>();
            CustomHeaders = new Dictionary<string, string>();
        }


        /// <summary>
        /// Converts the MailgunMessage into a collection of key-value pairs for serialization.
        /// </summary>
        /// <returns></returns>
        internal ICollection<KeyValuePair<string, string>> ToKeyValuePair()
        {
            var v = new List<KeyValuePair<string, string>>();

            v.Add(new KeyValuePair<string, string>("from", From.ToString()));
            v.Add(new KeyValuePair<string, string>("subject", Subject));
            v.Add(new KeyValuePair<string, string>("text", Text));
            v.Add(new KeyValuePair<string, string>("html", HTML));
            v.Add(new KeyValuePair<string, string>("o:campaign", Campaign));

            if (TestMode)
            {
                v.Add(new KeyValuePair<string, string>("o:testmode", "yes"));
            }

            if (Tracking.HasValue)
            {
                v.Add(new KeyValuePair<string, string>("o:tracking", Tracking.Value ? "yes" : "no"));
            }

            if (TrackingClicks.HasValue)
            {
                v.Add(new KeyValuePair<string, string>("o:tracking-clicks", Utilities.GetEnumStringValue(TrackingClicks.Value)));
            }


            if (TrackingOpens.HasValue)
            {
                v.Add(new KeyValuePair<string, string>("o:tracking-opens", TrackingOpens.Value ? "yes" : "no"));
            }

            if (DeliveryTime.HasValue)
            {
                v.Add(new KeyValuePair<string, string>("o:deliverytime", DeliveryTime.Value.ToUniversalTime().ToString("ddd, dd MMM yyyy hh:mm:ss GMT")));
            }

            if (DKIM.HasValue)
            {
                v.Add(new KeyValuePair<string, string>("o:dkim", DKIM.Value ? "yes" : "no"));
            }

            addAddressesToKeyValueCollection(v, To, "to");
            addAddressesToKeyValueCollection(v, CC, "cc");
            addAddressesToKeyValueCollection(v, BCC, "bcc");

            var recipientVariables = Enumerable.Empty<MailgunAddress>().Union(To).Union(CC).Union(BCC).ToDictionary(x => x.EmailAddress, x => x.RecipientVariables);

            if (recipientVariables.Any())
            {
                //var recipientVariables = recipients.ToDictionary(x => x.EmailAddress, x => x.RecipientVariables);
                v.Add(new KeyValuePair<string, string>("recipient-variables", JsonConvert.SerializeObject(recipientVariables)));
            }

            if (Tags != null)
            {
                foreach (var tag in Tags)
                {
                    v.Add(new KeyValuePair<string, string>("o:tag", tag));
                }
            }

            if (CustomHeaders != null)
            {
                foreach (var header in CustomHeaders)
                {
                    v.Add(new KeyValuePair<string, string>("h:" + header.Key, header.Value));
                }
            }

            if (Data != null)
            {
                foreach (var d in Data)
                {
                    v.Add(new KeyValuePair<string, string>("v:" + d.Key, JsonConvert.SerializeObject(d.Value)));
                }
            }

            return v;
        }


        /// <summary>
        /// Adds a collection of addresses to the key-value pair collection under the specified type.
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="addresses"></param>
        /// <param name="type"></param>
        private void addAddressesToKeyValueCollection(ICollection<KeyValuePair<string, string>> collection, ICollection<MailgunAddress> addresses, string type)
        {
            if (addresses != null)
            {
                foreach (var address in addresses)
                {
                    collection.Add(new KeyValuePair<string, string>(type, address.ToString()));
                }
            }
        }
    }
}
