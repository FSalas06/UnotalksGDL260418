using System;
using System.Collections.ObjectModel;
using System.IO;
using Plugin.Media.Abstractions;
using XamarinCognitiveServices.Services;

namespace XamarinCognitiveServices.ViewModels
{
    public class VisionResultViewModel : BaseViewModel
    {
        MediaFile photo;
        byte[] photoBytes;
        VisionAnalizeService _visioServices;
        ObservableCollection<string> _values;

        public VisionResultViewModel(MediaFile photo)
        {
            this.photo = photo;
            _visioServices = new VisionAnalizeService();
        }

		public override void OnAppearing()
		{
            base.OnAppearing();
            photoBytes = MediaFileToByteArray(photo);
            AnalizeImage();
		}

        byte[] MediaFileToByteArray(MediaFile photoMediaFile)
        {
            using (var memStream = new MemoryStream())
            {
                photoMediaFile.GetStream().CopyTo(memStream);
                return memStream.ToArray();
            }
        }

        async void AnalizeImage()
        {
            _values = await _visioServices.FetchHandwrittenWordList(photoBytes);
        }
    }
}
