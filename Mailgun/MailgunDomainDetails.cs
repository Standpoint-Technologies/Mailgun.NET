using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mailgun
{
    public class MailgunDomainDetails
    {
        public MailgunDomain Domain { get; set; }

        public string Message { get; set; }

        [JsonProperty("receiving_dns_records")]
        public ICollection<MailgunReceivingDnsRecord> ReceivingDnsRecords { get; set; }

        [JsonProperty("sending_dns_records")]
        public ICollection<MailgunSendingDnsRecord> SendingDnsRecords { get; set; }
    }
}
