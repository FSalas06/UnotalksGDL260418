using System;
using System.Collections.Generic;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using XamarinCognitiveServices.ViewModels;

namespace XamarinCognitiveServices.Views
{
    public partial class VisionResultView : ContentPage
    {
        VisionResultViewModel _viewModel;

        public VisionResultView(MediaFile photo)
        {
            InitializeComponent();
            BindingContext = _viewModel = new VisionResultViewModel(photo);
        }
    }
}
