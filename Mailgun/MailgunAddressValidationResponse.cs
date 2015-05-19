using Newtonsoft.Json;

namespace Mailgun
{
    public class MailgunAddressValidationResponse
    {
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }

        public string Address { get; set; }

        public MailgunAddressParts Parts { get; set; }

        [JsonProperty("did_you_mean")]
        public string DidYouMean { get; set; }
    }
}
