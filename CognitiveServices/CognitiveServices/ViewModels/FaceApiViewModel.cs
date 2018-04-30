using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;

namespace CognitiveServices.ViewModels
{
    public class FaceApiViewModel : INotifyPropertyChanged
    {
        ////readonly IFaceServiceClient faceServiceClient;
        //MediaFile photo;
        //private string _faceApiImage;

        //public string FaceApiImage
        //{
        //    get
        //    {
        //        return _faceApiImage;
        //    }
        //    set
        //    {
        //        _faceApiImage = value;
        //    }
        //}

        //public FaceApiViewModel()
        //{
        //    //faceServiceClient = new FaceServiceClient(Constants.FaceApiKey, Constants.FaceEndpoint);
        //}

        //public event PropertyChangedEventHandler PropertyChanged;

        //async void OnTakePhotoButtonClicked(object sender, EventArgs e)
        //{
        //    await CrossMedia.Current.Initialize();

        //    // Take photo
        //    if (CrossMedia.Current.IsCameraAvailable || CrossMedia.Current.IsTakePhotoSupported)
        //    {
        //        photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
        //        {
        //            Name = "emotion.jpg",
        //            PhotoSize = PhotoSize.Small
        //        });

        //        if (photo != null)
        //        {
        //            image.Source = ImageSource.FromStream(photo.GetStream);
        //        }
        //    }
        //    else
        //    {
        //        await DisplayAlert("No Camera", "Camera unavailable.", "OK");
        //    }

        //    try
        //    {
        //        if (photo != null)
        //        {
        //            var faceAttributes = new FaceAttributeType[]
        //            {
        //                FaceAttributeType.Emotion,
        //                FaceAttributeType.Age,
        //                FaceAttributeType.Glasses,
        //                FaceAttributeType.FacialHair,
        //                FaceAttributeType.Gender
        //            };

        //            using (var photoStream = photo.GetStream())
        //            {
        //                Face[] faces = await faceServiceClient.DetectAsync(photoStream, true, false, faceAttributes);
        //                if (faces.Any())
        //                {
        //                    // Emotions detected are happiness, sadness, surprise, anger, fear, contempt, disgust, or neutral.
        //                    emotionResultLabel.Text = faces.FirstOrDefault().FaceAttributes.Emotion.ToRankedList().FirstOrDefault().Key;
        //                    ageResultLabel.Text = faces.FirstOrDefault().FaceAttributes.Age.ToString();
        //                    glassResultLabel.Text = faces.FirstOrDefault().FaceAttributes.Glasses.ToString();
        //                    beardResultLabel.Text = faces.FirstOrDefault().FaceAttributes.FacialHair.Beard.ToString();
        //                    moustacheResultLabel.Text = faces.FirstOrDefault().FaceAttributes.FacialHair.Moustache.ToString();
        //                    genderResultLabel.Text = faces.FirstOrDefault().FaceAttributes.Gender.ToString();
        //                }
        //                photo.Dispose();
        //            }
        //        }
        //    }
        //    //catch (FaceAPIException fx)
        //    //{
        //    //    Debug.WriteLine(fx.Message);
        //    //}
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //}
    }
}
