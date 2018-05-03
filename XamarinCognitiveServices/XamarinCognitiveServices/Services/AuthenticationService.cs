using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using XamarinCognitiveServices.Interfaces;
using XamarinCognitiveServices.Utils;

namespace XamarinCognitiveServices.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        #region private properties
        string _subscriptionKey;
        string _token;
        Timer _accessTokenRenewer;
        const int _refreshTokenDuration = 9;
        HttpClient _httpClient;
        #endregion

        #region public methods
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationService"/> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public AuthenticationService(string apiKey)
        {
            _subscriptionKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add(Constants.OcpApimSubscriptionKey, apiKey);
        }

        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            _token = await FetchTokenAsync(Constants.AuthenticationTokenEndpoint);
            _accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(_refreshTokenDuration), TimeSpan.FromMilliseconds(-1));
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            return _token;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Renews the access token.
        /// </summary>
        /// <returns></returns>
        async Task RenewAccessToken()
        {
            _token = await FetchTokenAsync(Constants.AuthenticationTokenEndpoint);
            Debug.WriteLine("Renewed token.");
        }

        /// <summary>
        /// Called when [token expired callback].
        /// </summary>
        /// <param name="stateInfo">The state information.</param>
        /// <returns></returns>
        async Task OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                await RenewAccessToken();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Format("Failed to renew access token. Details: {0}", ex.Message));
            }
            finally
            {
                try
                {
                    _accessTokenRenewer.Change(TimeSpan.FromMinutes(_refreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// Fetches the token asynchronous.
        /// </summary>
        /// <param name="fetchUri">The fetch URI.</param>
        /// <returns></returns>
        async Task<string> FetchTokenAsync(string fetchUri)
        {
            UriBuilder uriBuilder = new UriBuilder(fetchUri);
            uriBuilder.Path += "/issueToken";

            var result = await _httpClient.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
            return await result.Content.ReadAsStringAsync();
        }
        #endregion
    }
}
