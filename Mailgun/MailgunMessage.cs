using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Mailgun.Attachments;
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

        public ICollection<MailgunAttachment> Attachments { get; set; }


        public MailgunMessage()
        {
            To = new List<MailgunAddress>();
            CC = new List<MailgunAddress>();
            BCC = new List<MailgunAddress>();
            Tags = new List<string>();
            CustomHeaders = new Dictionary<string, string>();
            Attachments = new List<MailgunAttachment>();
        }


        /// <summary>
        /// Converts the MailgunMessage into a MultipartFormDataContent for sending to Mailgun's API.
        /// </summary>
        /// <returns></returns>
        internal MultipartFormDataContent ToMultipartForm()
        {
            var v = new MultipartFormDataContent();

            if (From != null)
            {
                v.Add(new StringContent(From.ToString()), "from");
            }

            if (Subject != null)
            {
                v.Add(new StringContent(Subject), "subject");
            }

            if (Text != null)
            {
                v.Add(new StringContent(Text), "text");
            }

            if (HTML != null)
            {
                v.Add(new StringContent(HTML), "html");
            }

            if (Campaign != null)
            {
                v.Add(new StringContent(Campaign), "o:campaign");
            }

            if (TestMode)
            {
                v.Add(new StringContent("yes"), "o:testmode");
            }

            if (Tracking.HasValue)
            {
                v.Add(new StringContent(Tracking.Value ? "yes" : "no"), "o:tracking");
            }

            if (TrackingClicks.HasValue)
            {
                v.Add(new StringContent(Utilities.GetEnumStringValue(TrackingClicks.Value)), "o:tracking-clicks");
            }


            if (TrackingOpens.HasValue)
            {
                v.Add(new StringContent(TrackingOpens.Value ? "yes" : "no"), "o:tracking-opens");
            }

            if (DeliveryTime.HasValue)
            {
                v.Add(new StringContent(DeliveryTime.Value.ToUniversalTime().ToString("ddd, dd MMM yyyy hh:mm:ss GMT")), "o:deliverytime");
            }

            if (DKIM.HasValue)
            {
                v.Add(new StringContent(DKIM.Value ? "yes" : "no"), "o:dkim");
            }

            addAddressesToFormContent(v, To, "to");
            addAddressesToFormContent(v, CC, "cc");
            addAddressesToFormContent(v, BCC, "bcc");

            var recipientVariables = Enumerable.Empty<MailgunAddress>().Union(To).Union(CC).Union(BCC).ToDictionary(x => x.EmailAddress, x => x.RecipientVariables);

            if (recipientVariables.Any())
            {
                //var recipientVariables = recipients.ToDictionary(x => x.EmailAddress, x => x.RecipientVariables);
                v.Add(new StringContent(JsonConvert.SerializeObject(recipientVariables)), "recipient-variables");
            }

            if (Tags != null)
            {
                foreach (var tag in Tags)
                {
                    v.Add(new StringContent(tag), "o:tag");
                }
            }

            if (CustomHeaders != null)
            {
                foreach (var header in CustomHeaders)
                {
                    v.Add(new StringContent(header.Value), "h:" + header.Key);
                }
            }

            if (Data != null)
            {
                foreach (var d in Data)
                {
                    v.Add(new StringContent(JsonConvert.SerializeObject(d.Value)), "v:" + d.Key);
                }
            }

            if (Attachments != null)
            {
                foreach (var a in Attachments)
                {
                    v.Add(new StreamContent(a.FileContentStream), "attachment", a.FileName);
                }
            }

            return v;
        }


        /// <summary>
        /// Adds a collection of addresses to the multi-part form content under the specified type.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="addresses"></param>
        /// <param name="type"></param>
        private void addAddressesToFormContent(MultipartFormDataContent content, ICollection<MailgunAddress> addresses, string type)
        {
            if (addresses != null)
            {
                foreach (var address in addresses)
                {
                    content.Add(new StringContent(address.ToString()), type);
                }
            }
        }
    }
}
