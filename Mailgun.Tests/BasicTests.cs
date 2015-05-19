using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Mailgun.Attachments;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mailgun.Tests
{
    [TestClass]
    public class BasicTests
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
        public async Task TestAddDomain()
        {
            var domain = new MailgunDomain()
            {
                Name = _testSubDomain
            };

            var newDomain = await _client.AddDomainAsync(domain);
            Assert.IsNotNull(newDomain);
            Assert.IsNotNull(newDomain.Domain);
            Assert.AreEqual(newDomain.Message, "Domain has been created");
            Assert.AreEqual(domain.Name, newDomain.Domain.Name);
        }

        [TestMethod]
        public async Task TestDeleteDomain()
        {
            await _client.DeleteDomainAsync(_testSubDomain);
        }

        [TestMethod]
        public async Task TestGetDomain()
        {
            var domain = await _client.GetDomainAsync(_testDomain);
            Assert.IsNotNull(domain);
            Assert.IsNotNull(domain.Domain);
            Assert.AreEqual(_testDomain, domain.Domain.Name);
            Assert.AreEqual(MailgunSpamAction.Disabled, domain.Domain.SpamAction);
            Assert.AreEqual(MailgunDomainType.Custom, domain.Domain.Type);
            Assert.AreEqual(MailgunDomainState.Active, domain.Domain.State);
        }

        [TestMethod]
        public async Task TestGetDomains()
        {
            var domains = await _client.GetDomainsAsync();
            Assert.IsNotNull(domains);
        }

        [TestMethod]
        public async Task TestValidateAddress()
        {
            var validation = await _client.ValidateAddressAsync(_testRecipient);
            Assert.IsTrue(validation.IsValid);
            Assert.AreEqual(_testRecipient, validation.Address);
            Assert.AreEqual(_testRecipient.Substring(_testRecipient.IndexOf('@') + 1), validation.Parts.Domain);
            Assert.AreEqual(_testRecipient.Substring(0, _testRecipient.IndexOf('@')), validation.Parts.LocalPart);
        }

        [TestMethod]
        public async Task TestParseAddresses()
        {
            var validation = await _client.ParseAddressesAsync(new string[] { _testRecipient, _testDomain });
            Assert.AreEqual(1, validation.Parsed.Count);
            Assert.AreEqual(1, validation.Unparseable.Count);
            Assert.AreEqual(_testRecipient, validation.Parsed.First());
            Assert.AreEqual(_testDomain, validation.Unparseable.First());
        }
    }
}
