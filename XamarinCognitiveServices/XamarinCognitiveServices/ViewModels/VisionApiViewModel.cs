using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using XamarinCognitiveServices.Views;

namespace XamarinCognitiveServices.ViewModels
{
    public class VisionApiViewModel : BaseViewModel
    {
        MediaFile photo;

        public ICommand TakePhotoCommand
        {
            private set; get;
        }

        public ICommand ImportPhotoCommand
        {
            private set; get;
        }

        public VisionApiViewModel()
        {
            TakePhotoCommand = new Command(OnTakePhoto);
            ImportPhotoCommand = new Command(OnImportPhoto);
        }

        async void OnImportPhoto(object obj)
        {
            await CrossMedia.Current.Initialize();

            // Take photo
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                photo = await CrossMedia.Current.PickPhotoAsync();

                if (photo != null)
                {
                    await Navigation.PushAsync(new VisionResultView(photo));
                }
            }
            else
            {
                DisplayAlert("Import unavailable.");
            }
        }

        async void OnTakePhoto(object obj)
        {
            await CrossMedia.Current.Initialize();

            // Take photo
            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    AllowCropping = true,
                    SaveToAlbum = true,
                    Name = $"{DateTime.UtcNow}.jpg"
                });

                if (photo != null)
                {
                    await Navigation.PushAsync(new VisionResultView(photo));
                }
            }
            else
            {
                DisplayAlert("Camera unavailable.");
            }
        }

    }
}
