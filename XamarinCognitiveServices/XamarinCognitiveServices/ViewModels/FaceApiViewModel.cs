using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace XamarinCognitiveServices.ViewModels
{
    public class FaceApiViewModel : BaseViewModel
    {
        #region private properties
        readonly IFaceServiceClient _faceServiceClient;
        MediaFile _photo;
        ImageSource _faceApiImage;
        string _emotionString;
        string _ageString;
        string _glassesString;
        string _hairString;
        string _genderString;
        string _smileString;
        bool _isBusy;
        #endregion

        #region public properties
        /// <summary>
        /// Gets or sets the face API image.
        /// </summary>
        /// <value>The face API image.</value>
        public ImageSource FaceApiImage
        {
            get
            {
                return _faceApiImage;
            }
            set
            {
                SetObservableProperty(ref _faceApiImage, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this
        /// <see cref="T:XamarinCognitiveServices.ViewModels.FaceApiViewModel"/> is busy.
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
        /// Gets or sets the emotion string.
        /// </summary>
        /// <value>The emotion string.</value>
        public string EmotionString
        {
            get
            {
                return _emotionString;
            }
            set
            {
                SetObservableProperty(ref _emotionString, value);
            }
        }

        /// <summary>
        /// Gets or sets the age string.
        /// </summary>
        /// <value>The age string.</value>
        public string AgeString
        {
            get
            {
                return _ageString;
            }
            set
            {
                SetObservableProperty(ref _ageString, value);
            }
        }

        /// <summary>
        /// Gets or sets the glasses string.
        /// </summary>
        /// <value>The glasses string.</value>
        public string GlassesString
        {
            get
            {
                return _glassesString;
            }
            set
            {
                SetObservableProperty(ref _glassesString, value);
            }
        }

        /// <summary>
        /// Gets or sets the hair string.
        /// </summary>
        /// <value>The hair string.</value>
        public string HairString
        {
            get
            {
                return _hairString;
            }
            set
            {
                SetObservableProperty(ref _hairString, value);
            }
        }

        /// <summary>
        /// Gets or sets the smile string.
        /// </summary>
        /// <value>The smile string.</value>
        public string SmileString
        {
            get
            {
                return _smileString;
            }
            set
            {
                SetObservableProperty(ref _smileString, value);
            }
        }

        /// <summary>
        /// Gets or sets the gender string.
        /// </summary>
        /// <value>The gender string.</value>
        public string GenderString
        {
            get
            {
                return _genderString;
            }
            set
            {
                SetObservableProperty(ref _genderString, value);
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
        #endregion

        #region public methods
        /// <summary>
        /// Initializes a new instance of the <see cref="T:XamarinCognitiveServices.ViewModels.FaceApiViewModel"/> class.
        /// </summary>
        public FaceApiViewModel()
        {
            _faceServiceClient = new FaceServiceClient(Constants.FaceApiKey, Constants.FaceEndpoint);
            TakePhotoCommand = new Command(OnTakePhoto);
            ImportPhotoCommand = new Command(OnImportPhoto);
            IsBusy = false;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Ons the import photo.
        /// </summary>
        async void OnImportPhoto()
        {
            IsBusy = true;
            await CrossMedia.Current.Initialize();

            // Take photo
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                _photo = await CrossMedia.Current.PickPhotoAsync();

                if (_photo != null)
                {
                    FaceApiImage = ImageSource.FromStream(_photo.GetStream);
                }
            }
            else
            {
                DisplayAlert("Camera unavailable.");
            }

            await FaceApiDetectAsync();
            IsBusy = false;
        }

        /// <summary>
        /// Ons the take photo.
        /// </summary>
        async void OnTakePhoto()
        {
            IsBusy = true;
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
                    FaceApiImage = ImageSource.FromStream(_photo.GetStream);
                }
            }
            else
            {
                DisplayAlert("Camera unavailable.");
            }

            await FaceApiDetectAsync();
            IsBusy = false;
        }

        /// <summary>
        /// Faces the API detect async.
        /// </summary>
        /// <returns>The API detect async.</returns>
        async Task FaceApiDetectAsync()
        {
            try
            {
                if (_photo != null)
                {
                    var faceAttributes = new FaceAttributeType[]
                    {
                        //You can include more FaceatributeType
                        FaceAttributeType.Emotion,
                        FaceAttributeType.Age,
                        FaceAttributeType.Glasses,
                        FaceAttributeType.Gender,
                        FaceAttributeType.Smile
                    };

                    using (var photoStream = _photo.GetStream())
                    {
                        Face[] faces = await _faceServiceClient.DetectAsync(photoStream, true, false, faceAttributes);
                        if (faces.Any())
                        {
                            //Emotions detected are happiness, sadness, surprise, anger, fear, contempt, disgust, or neutral.
                            EmotionString = faces.FirstOrDefault().FaceAttributes.Emotion.ToRankedList().FirstOrDefault().Key;
                            AgeString = faces.FirstOrDefault().FaceAttributes.Age.ToString();
                            GlassesString = faces.FirstOrDefault().FaceAttributes.Glasses.ToString();
                            GenderString = faces.FirstOrDefault().FaceAttributes.Gender;
                            SmileString = faces.FirstOrDefault().FaceAttributes.Smile > 0.5 ? "Smiling :)" : "Not Smiling :(";
                        }
                        _photo.Dispose();
                    }
                }
            }
            catch (FaceAPIException fx)
            {
                Debug.WriteLine(fx.Message);
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsBusy = false;
            }
        }
        #endregion
    }
}
