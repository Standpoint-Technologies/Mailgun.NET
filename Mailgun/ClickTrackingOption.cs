using System.ComponentModel;

namespace Mailgun
{
    public enum ClickTrackingOption
    {
        [Description("yes")]
        Yes,

        [Description("no")]
        No,

        [Description("htmlonly")]
        HtmlOnly
    }
}
