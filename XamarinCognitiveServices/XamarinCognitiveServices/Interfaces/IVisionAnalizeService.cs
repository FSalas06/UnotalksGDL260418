using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace XamarinCognitiveServices.Interfaces
{
    public interface IVisionAnalizeService
    {
        Task<ObservableCollection<string>> FetchPrintedWordList(byte[] photo);
    }
}
