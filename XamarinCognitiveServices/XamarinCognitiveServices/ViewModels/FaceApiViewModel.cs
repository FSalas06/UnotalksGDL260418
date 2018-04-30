using System;
using System.Diagnostics;
using System.Linq;
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
        readonly IFaceServiceClient faceServiceClient;
        MediaFile photo;
        ImageSource _faceApiImage;
        string _emotionString;
        string _ageString;
        string _glassesString;
        string _hairString;
        string _genderString;
        string _smileString;
        bool _isBusy;


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

        public ICommand TakePhotoCommand
        {
            private set; get;
        }

        public ICommand ImportPhotoCommand
        {
            private set; get;
        }

        public FaceApiViewModel()
        {
            faceServiceClient = new FaceServiceClient(Constants.FaceApiKey, Constants.FaceEndpoint);
            TakePhotoCommand = new Command(OnTakePhoto);
            ImportPhotoCommand = new Command(OnImportPhoto);
            IsBusy = false;
        }

        async void OnImportPhoto()
        {
            IsBusy = true;
            await CrossMedia.Current.Initialize();

            // Take photo
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                photo = await CrossMedia.Current.PickPhotoAsync();

                if (photo != null)
                {
                    FaceApiImage = ImageSource.FromStream(photo.GetStream);
                }
            }
            else
            {
                DisplayAlert("Camera unavailable.");
            }

            await FaceApiDetectAsync();
            IsBusy = false;
        }

        async void OnTakePhoto()
        {
            IsBusy = true;
            await CrossMedia.Current.Initialize();

            // Take photo
            if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
            {
                photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Name = "emotion.jpg",
                    PhotoSize = PhotoSize.Small,
                });

                if (photo != null)
                {
                    FaceApiImage = ImageSource.FromStream(photo.GetStream);
                }
            }
            else
            {
                DisplayAlert("Camera unavailable.");
            }

            await FaceApiDetectAsync();
            IsBusy = false;
        }

        async System.Threading.Tasks.Task FaceApiDetectAsync()
        {
            try
            {
                if (photo != null)
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

                    using (var photoStream = photo.GetStream())
                    {
                        Face[] faces = await faceServiceClient.DetectAsync(photoStream, true, false, faceAttributes);
                        if (faces.Any())
                        {
                            //Emotions detected are happiness, sadness, surprise, anger, fear, contempt, disgust, or neutral.
                            EmotionString = faces.FirstOrDefault().FaceAttributes.Emotion.ToRankedList().FirstOrDefault().Key;
                            AgeString = faces.FirstOrDefault().FaceAttributes.Age.ToString();
                            GlassesString = faces.FirstOrDefault().FaceAttributes.Glasses.ToString();
                            GenderString = faces.FirstOrDefault().FaceAttributes.Gender;
                            SmileString = faces.FirstOrDefault().FaceAttributes.Smile > 0.5 ? "Smiling :)" : "Not Smiling :(";
                        }
                        photo.Dispose();
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
    }
}
