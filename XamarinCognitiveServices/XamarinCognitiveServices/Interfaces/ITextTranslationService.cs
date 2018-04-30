using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinCognitiveServices.Interfaces
{
    interface ITextTranslationService
    {
        System.Threading.Tasks.Task<string> TranslateTextAsync(string text);
    }
}
