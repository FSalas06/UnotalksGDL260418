using System;
namespace CognitiveServices
{
    public static class Constants
    {
        public static readonly string AuthenticationTokenEndpoint = "https://api.cognitive.microsoft.com/sts/v1.0";

        //Spell Check API
        public static readonly string BingSpellCheckApiKey = "<Here your Key>";
        public static readonly string BingSpellCheckEndpoint = "https://api.cognitive.microsoft.com/bing/v7.0/SpellCheck";

        //Text Translation API
        public static readonly string TextTranslatorApiKey = "<Here your Key>";
        public static readonly string TextTranslatorEndpoint = "https://api.microsofttranslator.com/v2/http.svc/translate";

        // Face API
        public static readonly string FaceApiKey = "<Here your Key>";
        public static readonly string FaceEndpoint = "https://australiaeast.api.cognitive.microsoft.com/face/v1.0";

    }
}
