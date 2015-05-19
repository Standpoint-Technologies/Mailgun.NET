using System.Collections.Generic;

namespace Mailgun
{
    public class MailgunAddressParseResponse
    {
        public ICollection<string> Parsed { get; set; }

        public ICollection<string> Unparseable { get; set; }
    }
}
