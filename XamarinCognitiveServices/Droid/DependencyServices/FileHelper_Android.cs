using System;
using System.IO;
using Xamarin.Forms;
using XamarinCognitiveServices.Droid.DependencyServices;
using XamarinCognitiveServices.Interfaces;

[assembly: Dependency(typeof(FileHelper_Android))]
namespace XamarinCognitiveServices.Droid.DependencyServices
{
    public class FileHelper_Android : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}
