
namespace Mailgun.WebhookEvents
{
    public class DeliveryEvent : WebhookEventBase
    {
        public string MessageId { get; set; }
    }
}
