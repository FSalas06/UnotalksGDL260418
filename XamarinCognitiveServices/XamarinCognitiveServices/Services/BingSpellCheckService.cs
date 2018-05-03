using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using XamarinCognitiveServices.Enumeration;
using XamarinCognitiveServices.Interfaces;
using XamarinCognitiveServices.Models;

namespace XamarinCognitiveServices.Services
{
    public class BingSpellCheckService : IBingSpellCheckService
    {
        
        #region private properties
        HttpClient _httpClient;
        #endregion

        #region public methods
        /// <summary>
        /// Initializes a new instance of the <see cref="T:XamarinCognitiveServices.Services.BingSpellCheckService"/> class.
        /// </summary>
        public BingSpellCheckService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add(Constants.OcpApimSubscriptionKey, Constants.BingSpellCheckApiKey);
        }

        /// <summary>
        /// Spells the check text async.
        /// </summary>
        /// <returns>The check text async.</returns>
        /// <param name="text">Text.</param>
        public async Task<SpellCheckResult> SpellCheckTextAsync(string text)
        {
            string requestUri = GenerateRequestUri(Constants.BingSpellCheckEndpoint, text, SpellCheckMode.Proof);
            var response = await SendRequestAsync(requestUri);
            var spellCheckResults = JsonConvert.DeserializeObject<SpellCheckResult>(response);
            return spellCheckResults;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Generates the request URI.
        /// </summary>
        /// <returns>The request URI.</returns>
        /// <param name="spellCheckEndpoint">Spell check endpoint.</param>
        /// <param name="text">Text.</param>
        /// <param name="mode">Mode.</param>
        string GenerateRequestUri(string spellCheckEndpoint, string text, SpellCheckMode mode)
        {
            string requestUri = spellCheckEndpoint;
            requestUri += string.Format("?text={0}", WebUtility.UrlEncode(text));
            requestUri += string.Format("&mode={0}", mode.ToString().ToLower());
            return requestUri;
        }

        /// <summary>
        /// Sends the request async.
        /// </summary>
        /// <returns>The request async.</returns>
        /// <param name="url">URL.</param>
        async Task<string> SendRequestAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}
