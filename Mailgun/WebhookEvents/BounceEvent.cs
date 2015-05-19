
namespace Mailgun.WebhookEvents
{
    public class BounceEvent : WebhookEventBase
    {
        public string Code { get; set; }

        public string Error { get; set; }

        public string Notification { get; set; }

        public string CampaignId { get; set; }

        public string CampaignName { get; set; }

        public string Tag { get; set; }

        public string MailingList { get; set; }

        // Attachments go here too, but they get parsed funny see https://github.com/Standpoint-Technologies/Mailgun.NET/issues/1
    }
}
