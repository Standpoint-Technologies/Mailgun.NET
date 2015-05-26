using System.ComponentModel;

namespace Mailgun
{
    public enum MailgunWebhookType
    {
        [Description("bounce")]
        Bounce,

        [Description("deliver")]
        Deliver,

        [Description("drop")]
        Drop,

        [Description("spam")]
        Spam,

        [Description("unsubscribe")]
        Unsubscribe,

        [Description("click")]
        Click,

        [Description("open")]
        Open
    }
}
