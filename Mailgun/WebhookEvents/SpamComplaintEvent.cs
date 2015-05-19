
namespace Mailgun.WebhookEvents
{
    public class SpamComplaintEvent : WebhookEventBase
    {
        public string CampaignId { get; set; }

        public string CampaignName { get; set; }

        public string Tag { get; set; }

        public string MailingList { get; set; }

        // Attachments go here too, but they get parsed funny see https://github.com/Standpoint-Technologies/Mailgun.NET/issues/1
    }
}
