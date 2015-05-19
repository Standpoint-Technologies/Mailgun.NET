using System.Configuration;

namespace Mailgun.Tests
{
    static class ObjectFactory
    {
        public static IMailgunClient GetMailgunClient()
        {
            return new MailgunClient(ConfigurationManager.AppSettings["ApiBaseUrl"], ConfigurationManager.AppSettings["PrivateApiKey"], ConfigurationManager.AppSettings["PublicApiKey"]);
        }

        public static IMailgunClient GetBadKeyMailgunClient()
        {
            return new MailgunClient(ConfigurationManager.AppSettings["ApiBaseUrl"], ConfigurationManager.AppSettings["PrivateApiKey"] + "1", ConfigurationManager.AppSettings["PublicApiKey"] + "1");
        }
    }
}
