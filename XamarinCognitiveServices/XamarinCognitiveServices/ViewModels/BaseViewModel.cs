using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace XamarinCognitiveServices.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string property = null) => 
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        protected void SetObservableProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(propertyName);
        }

        public async void DisplayAlert(string message, string okText = "OK")
        {
            await UserDialogs.Instance.AlertAsync(message, Constants.NameApplication, okText);
        }

        public virtual async void OnAppearing()
        {
            
        }
    }
}
