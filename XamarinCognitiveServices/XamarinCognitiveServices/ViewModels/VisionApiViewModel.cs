using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using XamarinCognitiveServices.Enumeration;
using XamarinCognitiveServices.Interfaces;
using XamarinCognitiveServices.Models;
using XamarinCognitiveServices.Services;
using XamarinCognitiveServices.Views;

namespace XamarinCognitiveServices.ViewModels
{
    public class VisionApiViewModel : BaseViewModel
    {
        MediaFile photo;
        List<PickerItem> _ocrList;
        PickerItem _selectedFunction;

        byte[] photoBytes;
        IVisionAnalizeService _visioServices;
        ObservableCollection<string> _values;
        string _jsonResponse;
        ImageSource _analizedImage;
        bool _isBusy;

        public ImageSource AnalizedImage
        {
            get
            {
                return _analizedImage;
            }
            set
            {
                SetObservableProperty(ref _analizedImage, value);
            }
        }

        public ObservableCollection<string> Values
        {
            get
            {
                return _values;
            }
            set
            {
                SetObservableProperty(ref _values, value);
            }
        }

        public string JsonResponse
        {
            get
            {
                return _jsonResponse;
            }
            set
            {
                SetObservableProperty(ref _jsonResponse, value);
            }
        }

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                SetObservableProperty(ref _isBusy, value);
            }
        }

        public ICommand TakePhotoCommand
        {
            private set; get;
        }

        public ICommand ImportPhotoCommand
        {
            private set; get;
        }

        public List<PickerItem> OcrList
        {
            get
            {
                return _ocrList;
            }
            set
            {
                SetObservableProperty(ref _ocrList, value);
            }
        }

        public PickerItem SelectedFunction
        {
            get
            {
                return _selectedFunction;
            }
            set
            {
                SetObservableProperty(ref _selectedFunction, value);
            }
        }

        public VisionApiViewModel()
        {
            IsBusy = false;

            Values = new ObservableCollection<string>();
            _visioServices = new VisionAnalizeService();

            TakePhotoCommand = new Command(OnTakePhoto);
            ImportPhotoCommand = new Command(OnImportPhoto);
            FillPicker();
        }

        private void FillPicker()
        {
            var ocritems = Enum.GetValues(typeof(VisionEnum)).Cast<VisionEnum>();
            OcrList = new List<PickerItem>();
            OcrList = ocritems.Select(o => new PickerItem
            {
                Title = o.GetDescription(),
                Value = o.ToString(),
                Id = Convert.ToString((int)o)
            }).ToList();
        }

        async void OnImportPhoto(object obj)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                // Take photo
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    photo = await CrossMedia.Current.PickPhotoAsync();

                    if (photo != null)
                    {
                        if (SelectedFunction != null)
                        {
                            IsBusy = true;
                            AnalizedImage = ImageSource.FromStream(photo.GetStream);
                            photoBytes = MediaFileToByteArray(photo);
                            FetchImage();
                            IsBusy = false;
                        }
                        else
                        {
                            DisplayAlert("Select a Function please");
                            return;
                        }
                    }
                }
                else
                {
                    DisplayAlert("Import unavailable.");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
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
                    if (SelectedFunction != null)
                    {
                        IsBusy = true;
                        AnalizedImage = ImageSource.FromStream(photo.GetStream);
                        photoBytes = MediaFileToByteArray(photo);
                        FetchImage();
                        IsBusy = false;
                    }
                    else
                    {
                        DisplayAlert("Select a Function please");
                        return;
                    }
                }
            }
            else
            {
                DisplayAlert("Camera unavailable.");
            }
        }

        byte[] MediaFileToByteArray(MediaFile photoMediaFile)
        {
            using (var memStream = new MemoryStream())
            {
                photoMediaFile.GetStream().CopyTo(memStream);
                return memStream.ToArray();
            }
        }

        async void FetchImage()
        {
            try
            {
                switch (SelectedFunction.Id)
                {
                    case "0":
                        Values = await _visioServices.FetchPrintedWordList(photoBytes, SelectedFunction.Id);
                        JsonResponse = string.Empty;
                        if (!Values.Any())
                        {
                            DisplayAlert("The services no found words");
                            await Navigation.PopAsync(true);
                        }
                        break;
                    case "1":
                        Values = await _visioServices.FetchHandwrittenWordList(photoBytes, SelectedFunction.Id);
                        JsonResponse = string.Empty;
                        if (!Values.Any())
                        {
                            DisplayAlert("The services no found words");
                            await Navigation.PopAsync(true);
                        }
                        break;
                    case "2":
                        JsonResponse = await _visioServices.ImageAnalize(photoBytes, SelectedFunction.Id);
                        Values = new ObservableCollection<string>();
                        if (string.IsNullOrEmpty(JsonResponse))
                        {
                            DisplayAlert("The services nothing releated with this images");
                            await Navigation.PopAsync(true);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EX : " + ex.Message);
            }
        }
    }
}
