using System;
using System.Configuration;
using System.Threading.Tasks;
using Mailgun.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mailgun.Tests
{
    [TestClass]
    public class ExceptionTests
    {
        private IMailgunClient _client;
        private string _domain;
        private string _testRecipient;
        private string _testSender;

        [TestInitialize]
        public void Initialize()
        {
            _client = ObjectFactory.GetMailgunClient();
            _domain = ConfigurationManager.AppSettings["Domain"];
            _testRecipient = ConfigurationManager.AppSettings["TestRecipient"];
            _testSender = ConfigurationManager.AppSettings["TestSender"];
        }

        [TestCleanup]
        public void Cleanup()
        {
            _client.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestNoFrom()
        {
            var message = new MailgunMessage(_domain)
            {
                Text = "Hello World",
                Subject = "Test",
                TestMode = true
            };

            message.To.Add(new MailgunAddress(_testRecipient, "test recipient"));

            await _client.SendMessageAsync(message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestLargeDeliveryTime()
        {
            var message = new MailgunMessage(_domain)
            {
                From = new MailgunAddress(_testSender, "Unit Testing"),
                Text = "Hello World",
                Subject = "Test",
                DeliveryTime = DateTime.UtcNow.AddDays(3).AddSeconds(1),
                TestMode = true
            };

            message.To.Add(new MailgunAddress(_testRecipient, "test recipient"));

            await _client.SendMessageAsync(message);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestMaxRecipients()
        {
            var message = new MailgunMessage(_domain)
            {
                From = new MailgunAddress(_testSender, "Unit Testing"),
                Text = "Hello World",
                Subject = "Test",
                TestMode = true
            };

            for (int i = 0; i < 335; i++)
            {
                message.To.Add(new MailgunAddress(_testRecipient, "test recipient"));
            }

            for (int i = 0; i < 333; i++)
            {
                message.CC.Add(new MailgunAddress(_testRecipient, "test recipient"));
            }

            for (int i = 0; i < 333; i++)
            {
                message.BCC.Add(new MailgunAddress(_testRecipient, "test recipient"));
            }

            await _client.SendMessageAsync(message);
        }

        [TestMethod]
        [ExpectedException(typeof(MailgunUnauthorizedException))]
        public async Task TestUnauthorized()
        {
            var client = ObjectFactory.GetBadKeyMailgunClient();
            await client.GetDomainsAsync();
        }
    }
}
