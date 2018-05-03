using System.Threading.Tasks;

namespace XamarinCognitiveServices.Interfaces
{
    interface ITextTranslationService
    {
        /// <summary>
        /// Translates the text async.
        /// </summary>
        /// <returns>The text async.</returns>
        /// <param name="text">Text.</param>
        Task<string> TranslateTextAsync(string text);
    }
}
