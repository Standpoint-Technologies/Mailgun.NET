using Newtonsoft.Json;

namespace Mailgun
{
    public class MailgunSendingDnsRecord
    {
        public string Name { get; set; }

        [JsonProperty("record_type")]
        public string RecordType { get; set; }

        public string Valid { get; set; }

        public string Value { get; set; }
    }
}
