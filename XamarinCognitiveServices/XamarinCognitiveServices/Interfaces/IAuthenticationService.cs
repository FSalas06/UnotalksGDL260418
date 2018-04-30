using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinCognitiveServices.Interfaces
{
    interface IAuthenticationService
    {
        /// <summary>
        /// Initializes the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task InitializeAsync();

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <returns></returns>
        string GetAccessToken();
    }
}
