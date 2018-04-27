using System;
using CognitiveServices.Views;
using Xamarin.Forms;

namespace CognitiveServices
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new CognitivePage());
        }
    }
}
