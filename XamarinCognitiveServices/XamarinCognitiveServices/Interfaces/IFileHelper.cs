namespace XamarinCognitiveServices.Interfaces
{
    public interface IFileHelper
    {
        /// <summary>
        /// Gets the local file path.
        /// </summary>
        /// <returns>The local file path.</returns>
        /// <param name="filename">Filename.</param>
        string GetLocalFilePath(string filename);
    }
}
