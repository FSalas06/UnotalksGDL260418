using System.Threading.Tasks;
using XamarinCognitiveServices.Models;

namespace XamarinCognitiveServices.Interfaces
{
    interface IBingSpellCheckService
    {
        Task<SpellCheckResult> SpellCheckTextAsync(string text);
    }
}
