using CognitiveServices.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CognitiveServices.Interfaces
{
    interface IBingSpellCheckService
    {
        Task<SpellCheckResult> SpellCheckTextAsync(string text);
    }
}
