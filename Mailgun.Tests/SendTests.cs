using System;
using System.Configuration;
using System.Threading.Tasks;
using Mailgun.Attachments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mailgun.Tests
{
    [TestClass]
    public class SendTests
    {
        private IMailgunClient _client;
        private string _domain;
        private string _testRecipient;
        private string _testSender;
        private string _testDomain;
        private string _testSubDomain;

        [TestInitialize]
        public void Initialize()
        {
            _client = ObjectFactory.GetMailgunClient();
            _domain = ConfigurationManager.AppSettings["Domain"];
            _testRecipient = ConfigurationManager.AppSettings["TestRecipient"];
            _testSender = ConfigurationManager.AppSettings["TestSender"];
            _testDomain = ConfigurationManager.AppSettings["TestDomain"];
            _testSubDomain = ConfigurationManager.AppSettings["TestSubDomain"];
        }

        [TestCleanup]
        public void Cleanup()
        {
            _client.Dispose();
        }

        [TestMethod]
        public async Task TestSimpleMessage()
        {
            var message = new MailgunMessage(_domain)
            {
                From = new MailgunAddress(_testSender, "Unit Testing"),
                Text = "Hello World",
                Subject = "Test",
                TestMode = true
            };

            message.To.Add(new MailgunAddress(_testRecipient, "test recipient"));

            var response = await _client.SendMessageAsync(message);

            Assert.IsNotNull(response);
            Assert.IsTrue(!String.IsNullOrEmpty(response.ID));
            Assert.IsTrue(!String.IsNullOrEmpty(response.Message));
        }

        [TestMethod]
        public async Task TestSameRecipientMultipleTimes()
        {
            var message = new MailgunMessage(_domain)
            {
                From = new MailgunAddress(_testSender, "Unit Testing"),
                Text = "Hello World",
                Subject = "Test",
                TestMode = true
            };

            message.To.Add(new MailgunAddress(_testRecipient, "test recipient"));
            message.CC.Add(new MailgunAddress(_testRecipient, "test recipient"));
            message.BCC.Add(new MailgunAddress(_testRecipient, "test recipient"));

            var response = await _client.SendMessageAsync(message);

            Assert.IsNotNull(response);
            Assert.IsTrue(!String.IsNullOrEmpty(response.ID));
            Assert.IsTrue(!String.IsNullOrEmpty(response.Message));
        }

        [TestMethod]
        public async Task TestMessageAttachment()
        {
            var message = new MailgunMessage(_domain)
            {
                From = new MailgunAddress(_testSender, "Unit Testing"),
                Text = "Hello World",
                Subject = "Test",
                TestMode = true
            };

            message.To.Add(new MailgunAddress(_testRecipient, "test recipient"));
            message.Attachments.Add(new MailgunAttachment("test.tiff", System.IO.File.Open("test-image.tiff", System.IO.FileMode.Open)));

            var response = await _client.SendMessageAsync(message);

            Assert.IsNotNull(response);
            Assert.IsTrue(!String.IsNullOrEmpty(response.ID));
            Assert.IsTrue(!String.IsNullOrEmpty(response.Message));
        }

        [TestMethod]
        public async Task TestMessageByteAttachment()
        {
            var message = new MailgunMessage(_domain)
            {
                From = new MailgunAddress(_testSender, "Unit Testing"),
                Text = "Hello World",
                Subject = "Test",
                TestMode = true
            };

            message.To.Add(new MailgunAddress(_testRecipient, "test recipient"));
            message.Attachments.Add(new MailgunByteAttachment("test.tiff", System.IO.File.ReadAllBytes("test-image.tiff")));

            var response = await _client.SendMessageAsync(message);

            Assert.IsNotNull(response);
            Assert.IsTrue(!String.IsNullOrEmpty(response.ID));
            Assert.IsTrue(!String.IsNullOrEmpty(response.Message));
        }
    }
}
