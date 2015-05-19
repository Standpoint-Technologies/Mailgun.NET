using Newtonsoft.Json;

namespace Mailgun
{
    public class MailgunAddressParts
    {
        [JsonProperty("local_part")]
        public string LocalPart { get; set; }

        public string Domain { get; set; }
    }
}
