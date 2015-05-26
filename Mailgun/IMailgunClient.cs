using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mailgun
{
    public interface IMailgunClient : IDisposable
    {
        /// <summary>
        /// Adds a new domain.
        /// </summary>
        /// <param name="domain">The domain to add.</param>
        /// <returns></returns>
        Task<MailgunDomainDetails> AddDomainAsync(MailgunDomain domain);

        /// <summary>
        /// Adds a new webhook to the domain.
        /// </summary>
        /// <param name="domain">The domain to add a webhook to.</param>
        /// <param name="webhook">The webhook configuration.</param>
        /// <returns>The created MailgunWebhook object.</returns>
        Task<MailgunWebhook> AddWebhookAsync(string domain, MailgunWebhook webhook);

        /// <summary>
        /// Deletes a domain.
        /// </summary>
        /// <param name="domain">The domain to delete.</param>
        /// <returns></returns>
        Task DeleteDomainAsync(string domain);

        /// <summary>
        /// Deletes a webhook from the domain.
        /// </summary>
        /// <param name="domain">The domain the webhook is configured for.</param>
        /// <param name="webhookType">The type of webhook to delete.</param>
        /// <returns></returns>
        Task DeleteWebhookAsync(string domain, MailgunWebhookType webhookType);

        /// <summary>
        /// Gets a single domain.
        /// </summary>
        /// <param name="domain">The domain to retrieve.</param>
        /// <returns>A MailgunDomain object containing the domain details.</returns>
        Task<MailgunDomainDetails> GetDomainAsync(string domain);

        /// <summary>
        /// Gets all domains on an account.
        /// </summary>
        /// <param name="limit">The max number of records to return.</param>
        /// <param name="skip">The number of records to skip. Used for paging.</param>
        /// <returns>A collection of MailgunDomain objects containing the domain details.</returns>
        Task<IEnumerable<MailgunDomain>> GetDomainsAsync(int? limit = null, int? skip = null);

        /// <summary>
        /// Gets the webhook configuration for the specified type on the domain.
        /// </summary>
        /// <param name="domain">The domain to retrieve the webhook for.</param>
        /// <param name="webhookType">The webhook type to retrieve.</param>
        /// <returns>A MailgunWebhook object with the webhook configuration.</returns>
        Task<MailgunWebhook> GetWebhookAsync(string domain, MailgunWebhookType webhookType);

        /// <summary>
        /// Gets all of the webhooks configured for a domain.
        /// </summary>
        /// <param name="domain">The domain to retrieve webhooks for.</param>
        /// <returns>A collection of MailgunWebhook objects containing the configured webhooks.</returns>
        Task<IEnumerable<MailgunWebhook>> GetWebhooksAsync(string domain);

        /// <summary>
        /// Parses a collection of addresses and returns the result.
        /// </summary>
        /// <param name="addresses">The addresses to parse.</param>
        /// <param name="syntaxOnly">True to only check the syntax, false to also perform DNS and ESP validation.</param>
        /// <returns>A MailgunAddressParseResponse object containing the parsed and unparseable results.</returns>
        Task<MailgunAddressParseResponse> ParseAddressesAsync(IEnumerable<string> addresses, bool syntaxOnly = true);

        /// <summary>
        /// Sends an email message.
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <returns>A MailgunSentResponse object containing the details about the sent message.</returns>
        Task<MailgunSentResponse> SendMessageAsync(MailgunMessage message);

        /// <summary>
        /// Updates a webhook for the domain.
        /// </summary>
        /// <param name="domain">The domain to update the webhook for.</param>
        /// <param name="webhook">The new webhook configuration</param>
        /// <returns>The updated MailgunWebhook object.</returns>
        Task<MailgunWebhook> UpdateWebhookAsync(string domain, MailgunWebhook webhook);

        /// <summary>
        /// Validates an email address.
        /// </summary>
        /// <param name="address">The address to validate.</param>
        /// <returns>A MailgunAddressValidationResponse object containing the details of the validated address.</returns>
        Task<MailgunAddressValidationResponse> ValidateAddressAsync(string address);
    }
}
