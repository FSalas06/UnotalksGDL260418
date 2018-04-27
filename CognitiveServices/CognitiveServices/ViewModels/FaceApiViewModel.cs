using System;
using System.ComponentModel;

namespace CognitiveServices.ViewModels
{
    public class FaceApiViewModel : INotifyPropertyChanged
    {
        string _text;
        public string TEXTO
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(TEXTO)));
            }
        }

        public FaceApiViewModel()
        {
            TEXTO = "Take a Photo";
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
