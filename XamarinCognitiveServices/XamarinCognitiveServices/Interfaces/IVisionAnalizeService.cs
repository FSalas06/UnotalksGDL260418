using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace XamarinCognitiveServices.Interfaces
{
    public interface IVisionAnalizeService
    {
        /// <summary>
        /// Fetchs the printed word list.
        /// </summary>
        /// <returns>The printed word list.</returns>
        /// <param name="photo">Photo.</param>
        /// <param name="id">Identifier.</param>
        Task<ObservableCollection<string>> FetchPrintedWordList(byte[] photo, string id);

        /// <summary>
        /// Fetchs the handwritten word list.
        /// </summary>
        /// <returns>The handwritten word list.</returns>
        /// <param name="photo">Photo.</param>
        /// <param name="id">Identifier.</param>
        Task<ObservableCollection<string>> FetchHandwrittenWordList(byte[] photo, string id);

        /// <summary>
        /// Images the analize.
        /// </summary>
        /// <returns>The analize.</returns>
        /// <param name="photo">Photo.</param>
        /// <param name="id">Identifier.</param>
        Task<string> ImageAnalize(byte[] photo, string id);
    }
}
