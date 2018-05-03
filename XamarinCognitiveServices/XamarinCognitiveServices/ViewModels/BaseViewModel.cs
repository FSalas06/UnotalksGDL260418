using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acr.UserDialogs;
using Xamarin.Forms;

namespace XamarinCognitiveServices.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region public properties
        /// <summary>
        /// Occurs when property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the navigation.
        /// </summary>
        /// <value>The navigation.</value>
        public INavigation Navigation { get; set; }
        #endregion

        #region public methods
        /// <summary>
        /// Displaies the alert.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="okText">Ok text.</param>
        public async void DisplayAlert(string message, string okText = "OK")
        {
            await UserDialogs.Instance.AlertAsync(message, Constants.NameApplication, okText);
        }

        /// <summary>
        /// Ons the appearing.
        /// </summary>
        public virtual async void OnAppearing()
        {
            await Task.Delay(0);
        }
        #endregion

        #region protected methods
        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="property">Property.</param>
        protected void OnPropertyChanged([CallerMemberName] string property = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        /// <summary>
        /// Sets the observable property.
        /// </summary>
        /// <param name="field">Field.</param>
        /// <param name="value">Value.</param>
        /// <param name="propertyName">Property name.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected void SetObservableProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(propertyName);
        }
        #endregion
    }
}
