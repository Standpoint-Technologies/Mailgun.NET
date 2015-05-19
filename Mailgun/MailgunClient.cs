using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mailgun.Exceptions;
using Mailgun.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Mailgun
{
    public class MailgunClient : IMailgunClient
    {
        private readonly string _apiBaseUrl;
        private readonly string _domain;
        private readonly string _privateApiKey;
        private readonly string _publicApiKey;
        private readonly JsonSerializerSettings _jsonSettings;


        public MailgunClient(string apiBaseUrl, string domain, string privateApiKey, string publicApiKey)
        {
            _apiBaseUrl = apiBaseUrl;
            _domain = domain;
            _publicApiKey = publicApiKey;
            _privateApiKey = privateApiKey;

            _jsonSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            _jsonSettings.Converters.Add(new StringEnumConverter());
        }


        public void Dispose()
        {
        }

        public async Task<MailgunDomainDetails> AddDomainAsync(MailgunDomain domain)
        {
            if (domain == null)
            {
                throw new ArgumentException("Domain cannot be null", "domain");
            }

            if (String.IsNullOrEmpty(domain.Name))
            {
                throw new ArgumentException("Domain name cannot be empty", "domain");
            }

            using (var client = createHttpClient())
            {
                var v = domain.ToKeyValuePair();

                var content = new FormUrlEncodedContent(v);
                using (var response = await client.PostAsync("domains", content))
                {
                    var statusException = await checkStatusCode(response);
                    if (statusException != null)
                    {
                        throw statusException;
                    }

                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MailgunDomainDetails>(responseBody, _jsonSettings);
                }
            }
        }

        public async Task DeleteDomainAsync(string domain)
        {
            if (String.IsNullOrEmpty(domain))
            {
                throw new ArgumentException("Domain must have a value", domain);
            }

            using (var client = createHttpClient())
            {
                using (var response = await client.DeleteAsync("domains/" + HttpUtility.UrlEncode(domain)))
                {
                    var statusException = await checkStatusCode(response);
                    if (statusException != null)
                    {
                        throw statusException;
                    }

                    await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<MailgunDomainDetails> GetDomainAsync(string domain)
        {
            if (String.IsNullOrEmpty(domain))
            {
                throw new ArgumentException("Domain must have a value", domain);
            }

            using (var client = createHttpClient())
            {
                using (var response = await client.GetAsync("domains/" + HttpUtility.UrlEncode(domain)))
                {
                    var statusException = await checkStatusCode(response);
                    if (statusException != null)
                    {
                        throw statusException;
                    }

                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MailgunDomainDetails>(responseBody, _jsonSettings);
                }
            }
        }

        public async Task<IEnumerable<MailgunDomain>> GetDomainsAsync(int? limit = null, int? skip = null)
        {
            if (limit.HasValue && limit.Value <= 0)
            {
                throw new ArgumentException("limit must be greater than 0", "limit");
            }

            if (skip.HasValue && skip.Value < 0)
            {
                throw new ArgumentException("skip must be greater than or equal to 0", "skip");
            }

            using (var client = createHttpClient())
            {
                var v = new NameValueCollection();

                if (limit.HasValue)
                {
                    v.Add("limit", limit.ToString());
                }

                if (skip.HasValue)
                {
                    v.Add("skip", skip.ToString());
                }

                using (var response = await client.GetAsync("domains?" + toQueryString(v)))
                {
                    var statusException = await checkStatusCode(response);
                    if (statusException != null)
                    {
                        throw statusException;
                    }

                    var responseBody = await response.Content.ReadAsStringAsync();
                    var parsed = JsonConvert.DeserializeObject<MailgunCollectionWrapper<MailgunDomain>>(responseBody, _jsonSettings);
                    return parsed.Items;
                }
            }
        }

        public async Task<MailgunAddressParseResponse> ParseAddressesAsync(IEnumerable<string> addresses, bool syntaxOnly = true)
        {
            if (addresses.Any(address => address.Length > 512))
            {
                var first = addresses.First(address => address.Length > 512);
                throw new ArgumentException("Address cannot be longer than 512 characters: " + first, "addresses");
            }

            using (var client = createHttpClient(false))
            {
                var v = new NameValueCollection();
                v.Add("addresses", String.Join(";", addresses));
                v.Add("syntax_only", syntaxOnly.ToString());

                using (var response = await client.GetAsync("address/parse?" + toQueryString(v)))
                {
                    var statusException = await checkStatusCode(response);
                    if (statusException != null)
                    {
                        throw statusException;
                    }

                    var responseBody = await response.Content.ReadAsStringAsync();
                    var parsed = JsonConvert.DeserializeObject<MailgunAddressParseResponse>(responseBody, _jsonSettings);
                    return parsed;
                }
            }
        }

        public async Task<MailgunSentResponse> SendMessageAsync(MailgunMessage message)
        {
            if (message == null)
            {
                throw new ArgumentException("message cannot be null", "message");
            }

            if (message.From == null)
            {
                throw new ArgumentException("Message must have a from address", "message");
            }

            if (message.DeliveryTime.HasValue && message.DeliveryTime.Value.ToUniversalTime() > DateTime.UtcNow.AddDays(3))
            {
                throw new ArgumentException("Message delivery time cannot be more than 3 days in the future", "message");
            }

            if (message.To.Count + message.CC.Count + message.BCC.Count > 1000)
            {
                throw new ArgumentException("Message has a maximum of 1000 recipients", "message");
            }

            using (var client = createHttpClient())
            {
                using (var content = message.ToMultipartForm())
                {
                    using (var response = await client.PostAsync(_domain + "/messages", content))
                    {
                        var statusException = await checkStatusCode(response);
                        if (statusException != null)
                        {
                            throw statusException;
                        }

                        var responseBody = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<MailgunSentResponse>(responseBody, _jsonSettings);
                    }
                }
            }
        }

        public async Task<MailgunAddressValidationResponse> ValidateAddressAsync(string address)
        {
            if (String.IsNullOrEmpty(address))
            {
                throw new ArgumentException("Address cannot be null or empty", "address");
            }

            if (address.Length > 512)
            {
                throw new ArgumentException("Address cannot be longer than 512 characters", "address");
            }

            using (var client = createHttpClient(false))
            {
                var v = new NameValueCollection();
                v.Add("address", address);

                using (var response = await client.GetAsync("address/validate?" + toQueryString(v)))
                {
                    var statusException = await checkStatusCode(response);
                    if (statusException != null)
                    {
                        throw statusException;
                    }

                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MailgunAddressValidationResponse>(responseBody, _jsonSettings);
                }
            }
        }


        /// <summary>
        /// Creates an HttpClient with the defaults set for Mailgun. Be sure to wrap this in a using.
        /// </summary>
        /// <returns></returns>
        private HttpClient createHttpClient(bool usePrivateApiKey = true)
        {
            var apiKey = usePrivateApiKey ? _privateApiKey : _publicApiKey;
            var client = new HttpClient();
            client.BaseAddress = new Uri(_apiBaseUrl);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("api:" + apiKey)));
            return client;
        }

        /// <summary>
        /// Checks the response status code and returns an exception to throw if it is not successful.
        /// </summary>
        /// <param name="response">The response from the Mailgun request.</param>
        /// <returns>An exception if the response has an error code, null otherwise.</returns>
        private async Task<Exception> checkStatusCode(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return new MailgunUnauthorizedException(_privateApiKey);
                case HttpStatusCode.BadRequest:
                    return new MailgunBadRequest(await response.Content.ReadAsStringAsync());
                case HttpStatusCode.PaymentRequired:
                    return new MailgunRequestFailed(await response.Content.ReadAsStringAsync());
                case HttpStatusCode.NotFound:
                    return new MailgunNotFoundException(response.RequestMessage.RequestUri);
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                    return new MailgunServerException(await response.Content.ReadAsStringAsync());
                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts a NameValueCollection to a query string encoding.
        /// </summary>
        /// <param name="nvc"></param>
        /// <returns></returns>
        private string toQueryString(NameValueCollection nvc)
        {
            var array = nvc.AllKeys.SelectMany(x => nvc.GetValues(x).Select(y => string.Format("{0}={1}", HttpUtility.UrlEncode(x), HttpUtility.UrlEncode(y)))).ToArray();
            return string.Join("&", array);
        }
    }
}
