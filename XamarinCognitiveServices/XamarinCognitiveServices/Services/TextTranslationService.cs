using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Linq;
using XamarinCognitiveServices.Interfaces;

namespace XamarinCognitiveServices.Services
{
    class TextTranslationService : ITextTranslationService
    {
        #region private properties
        IAuthenticationService _authenticationService;
        HttpClient _httpClient;
        #endregion

        #region public methods
        /// <summary>
        /// Initializes a new instance of the <see cref="T:XamarinCognitiveServices.Services.TextTranslationService"/> class.
        /// </summary>
        /// <param name="authService">Auth service.</param>
        public TextTranslationService(IAuthenticationService authService)
        {
            _authenticationService = authService;
        }

        /// <summary>
        /// Translates the text async.
        /// </summary>
        /// <returns>The text async.</returns>
        /// <param name="text">Text.</param>
        public async Task<string> TranslateTextAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(_authenticationService.GetAccessToken()))
            {
                await _authenticationService.InitializeAsync();
            }

            string requestUri = GenerateRequestUri(Constants.TextTranslatorEndpoint, text, "es", "en");
            string accessToken = _authenticationService.GetAccessToken();
            var response = await SendRequestAsync(requestUri, accessToken);
            var content = XElement.Parse(response).Value;
            return content;

        }
        #endregion

        #region private methods
        /// <summary>
        /// Generates the request URI.
        /// </summary>
        /// <returns>The request URI.</returns>
        /// <param name="endpoint">Endpoint.</param>
        /// <param name="text">Text.</param>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        string GenerateRequestUri(string endpoint, string text, string from, string to)
        {
            string requestUri = endpoint;
            requestUri += string.Format("?text={0}", Uri.EscapeUriString(text));
            requestUri += string.Format("&from={0}", from);
            requestUri += string.Format("&to={0}", to);
            return requestUri;
        }

        /// <summary>
        /// Sends the request async.
        /// </summary>
        /// <returns>The request async.</returns>
        /// <param name="url">URL.</param>
        /// <param name="bearerToken">Bearer token.</param>
        async Task<string> SendRequestAsync(string url, string bearerToken)
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient();
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var response = await _httpClient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
        #endregion
    }
}
