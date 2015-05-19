
namespace Mailgun.WebhookEvents
{
    public class FailureEvent : WebhookEventBase
    {
        public string Reason { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        // Attachments go here too, but they get parsed funny see https://github.com/Standpoint-Technologies/Mailgun.NET/issues/1
    }
}
