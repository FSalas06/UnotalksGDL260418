using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using XamarinCognitiveServices.Enumeration;
using XamarinCognitiveServices.Interfaces;
using XamarinCognitiveServices.Models;

namespace XamarinCognitiveServices.Services
{
    public class BingSpellCheckService: IBingSpellCheckService
    {
        HttpClient httpClient;

        public BingSpellCheckService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(Constants.OcpApimSubscriptionKey, Constants.BingSpellCheckApiKey);
        }

        public async Task<SpellCheckResult> SpellCheckTextAsync(string text)
        {
            string requestUri = GenerateRequestUri(Constants.BingSpellCheckEndpoint, text, SpellCheckMode.Proof);
            var response = await SendRequestAsync(requestUri);
            var spellCheckResults = JsonConvert.DeserializeObject<SpellCheckResult>(response);
            return spellCheckResults;
        }

        string GenerateRequestUri(string spellCheckEndpoint, string text, SpellCheckMode mode)
        {
            string requestUri = spellCheckEndpoint;
            requestUri += string.Format("?text={0}", WebUtility.UrlEncode(text));   
            requestUri += string.Format("&mode={0}", mode.ToString().ToLower());    
            return requestUri;
        }

        async Task<string> SendRequestAsync(string url)
        {
            var response = await httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
