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
        /// Deletes a domain.
        /// </summary>
        /// <param name="domain">The domain to delete.</param>
        /// <returns></returns>
        Task DeleteDomainAsync(string domain);

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
        /// Validates an email address.
        /// </summary>
        /// <param name="address">The address to validate.</param>
        /// <returns>A MailgunAddressValidationResponse object containing the details of the validated address.</returns>
        Task<MailgunAddressValidationResponse> ValidateAddressAsync(string address);
    }
}
