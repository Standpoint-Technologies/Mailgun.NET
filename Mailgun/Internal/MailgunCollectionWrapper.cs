using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mailgun.Internal
{
    class MailgunCollectionWrapper<T>
    {
        [JsonProperty("items")]
        public List<T> Items { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}
