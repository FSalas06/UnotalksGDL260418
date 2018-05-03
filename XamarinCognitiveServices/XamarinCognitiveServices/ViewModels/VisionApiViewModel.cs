using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using XamarinCognitiveServices.Enumeration;
using XamarinCognitiveServices.Interfaces;
using XamarinCognitiveServices.Models;
using XamarinCognitiveServices.Services;

namespace XamarinCognitiveServices.ViewModels
{
    public class VisionApiViewModel : BaseViewModel
    {
        #region private properties
        byte[] _photoBytes;
        string _jsonResponse;
        bool _isBusy;
        List<PickerItem> _ocrList;
        PickerItem _selectedFunction;
        ObservableCollection<string> _values;
        MediaFile _photo;
        IVisionAnalizeService _visioServices;
        ImageSource _analizedImage;
        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets the analized image.
        /// </summary>
        /// <value>The analized image.</value>
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

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>The values.</value>
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

        /// <summary>
        /// Gets or sets the json response.
        /// </summary>
        /// <value>The json response.</value>
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

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:XamarinCognitiveServices.ViewModels.VisionApiViewModel"/> is busy.
        /// </summary>
        /// <value><c>true</c> if is busy; otherwise, <c>false</c>.</value>
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

        /// <summary>
        /// Gets the take photo command.
        /// </summary>
        /// <value>The take photo command.</value>
        public ICommand TakePhotoCommand
        {
            private set; get;
        }

        /// <summary>
        /// Gets the import photo command.
        /// </summary>
        /// <value>The import photo command.</value>
        public ICommand ImportPhotoCommand
        {
            private set; get;
        }

        /// <summary>
        /// Gets or sets the ocr list.
        /// </summary>
        /// <value>The ocr list.</value>
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

        /// <summary>
        /// Gets or sets the selected function.
        /// </summary>
        /// <value>The selected function.</value>
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
        #endregion

        #region public methods
        /// <summary>
        /// Initializes a new instance of the <see cref="T:XamarinCognitiveServices.ViewModels.VisionApiViewModel"/> class.
        /// </summary>
        public VisionApiViewModel()
        {
            IsBusy = false;

            Values = new ObservableCollection<string>();
            _visioServices = new VisionAnalizeService();

            TakePhotoCommand = new Command(OnTakePhoto);
            ImportPhotoCommand = new Command(OnImportPhoto);
            FillPicker();
        }
        #endregion

        #region private methods
        /// <summary>
        /// Fills the picker.
        /// </summary>
        void FillPicker()
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

        /// <summary>
        /// Ons the import photo.
        /// </summary>
        /// <param name="obj">Object.</param>
        async void OnImportPhoto(object obj)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                // Take photo
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    _photo = await CrossMedia.Current.PickPhotoAsync();

                    if (_photo != null)
                    {
                        if (SelectedFunction != null)
                        {
                            IsBusy = true;
                            AnalizedImage = ImageSource.FromStream(_photo.GetStream);
                            _photoBytes = MediaFileToByteArray(_photo);
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
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Ons the take photo.
        /// </summary>
        /// <param name="obj">Object.</param>
        async void OnTakePhoto(object obj)
        {
            await CrossMedia.Current.Initialize();

            // Take photo
            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                _photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    AllowCropping = true,
                    SaveToAlbum = true,
                    Name = $"{DateTime.UtcNow}.jpg"
                });

                if (_photo != null)
                {
                    if (SelectedFunction != null)
                    {
                        IsBusy = true;
                        AnalizedImage = ImageSource.FromStream(_photo.GetStream);
                        _photoBytes = MediaFileToByteArray(_photo);
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

        /// <summary>
        /// Medias the file to byte array.
        /// </summary>
        /// <returns>The file to byte array.</returns>
        /// <param name="photoMediaFile">Photo media file.</param>
        byte[] MediaFileToByteArray(MediaFile photoMediaFile)
        {
            using (var memStream = new MemoryStream())
            {
                photoMediaFile.GetStream().CopyTo(memStream);
                return memStream.ToArray();
            }
        }

        /// <summary>
        /// Fetchs the image.
        /// </summary>
        async void FetchImage()
        {
            try
            {
                switch (SelectedFunction.Id)
                {
                    case "0":
                        Values = await _visioServices.FetchPrintedWordList(_photoBytes, SelectedFunction.Id);
                        JsonResponse = string.Empty;
                        if (!Values.Any())
                        {
                            DisplayAlert("The services no found words");
                            await Navigation.PopAsync(true);
                        }
                        break;
                    case "1":
                        Values = await _visioServices.FetchHandwrittenWordList(_photoBytes, SelectedFunction.Id);
                        JsonResponse = string.Empty;
                        if (!Values.Any())
                        {
                            DisplayAlert("The services no found words");
                            await Navigation.PopAsync(true);
                        }
                        break;
                    case "2":
                        JsonResponse = await _visioServices.ImageAnalize(_photoBytes, SelectedFunction.Id);
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
        #endregion
    }
}
