using System.Threading.Tasks;
using XamarinCognitiveServices.Models;

namespace XamarinCognitiveServices.Interfaces
{
    interface IBingSpellCheckService
    {
        /// <summary>
        /// Spells the check text async.
        /// </summary>
        /// <returns>The check text async.</returns>
        /// <param name="text">Text.</param>
        Task<SpellCheckResult> SpellCheckTextAsync(string text);
    }
}
