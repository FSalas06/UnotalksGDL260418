using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using XamarinCognitiveServices.Interfaces;

namespace XamarinCognitiveServices.Services
{
    public class VisionAnalizeService : IVisionAnalizeService
    {
        HttpClient httpClient;

        public VisionAnalizeService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add(Constants.OcpApimSubscriptionKey, Constants.ComputerVisionApiKey);
        }

        string PrepareUri(string id)
        {
            string requestUri = Constants.ComputerVisionEndpoint;
            switch(id)
            {
                case "0":
                    requestUri += "/ocr?language=en&detectOrientation=true";
                    break;
                case "1":
                    requestUri += "/recognizeText/?handwriting=true";
                    break;
                case "2":
                    requestUri += "/analyze?visualFeatures=Categories,Description,Color&language=en";
                    break;
            }
            return requestUri;
        }

        public async Task<ObservableCollection<string>> FetchPrintedWordList(byte[] photo, string id)
        {
            ObservableCollection<string> wordList = new ObservableCollection<string>();
            if (photo != null)
            {
                HttpResponseMessage response = null;
                using (var content = new ByteArrayContent(photo))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    var requestUri = PrepareUri(id);
                    response = httpClient.PostAsync(requestUri, content).Result;
                }

                string ResponseString = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(ResponseString);

                IEnumerable<JToken> lines = json.SelectTokens("$.regions[*].lines[*]");
                if (lines != null)
                {
                    foreach (JToken line in lines)
                    {
                        IEnumerable<JToken> words = line.SelectTokens("$.words[*].text");
                        if (words != null)
                        {
                            wordList.Add(string.Join(" ", words.Select(x => x.ToString())));
                        }
                    }
                }
            }
            return wordList;
        }

        public async Task<ObservableCollection<string>> FetchHandwrittenWordList(byte[] photo, string id)
        {
            ObservableCollection<string> wordList = new ObservableCollection<string>();
            if (photo != null)
            {
                HttpResponseMessage response = null;
                using (var content = new ByteArrayContent(photo))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    var requestUri = PrepareUri(id);
                    response = httpClient.PostAsync(requestUri, content).Result;
                }

                IEnumerable<string> operationLocationValues;
                string statusUri = string.Empty;
                if (response.Headers.TryGetValues("Operation-Location", out operationLocationValues))
                {
                    statusUri = operationLocationValues.FirstOrDefault();

                    JObject obj = await FetchResultFromStatusUri(statusUri);
                    IEnumerable<JToken> strings = obj.SelectTokens("$.recognitionResult.lines[*].text");
                    foreach (string s in strings)
                    {
                        wordList.Add(s);
                    }
                }
            }
            return wordList;
        }

        public async Task<string> ImageAnalize(byte[] photo, string id)
        {
            try
            {
                if (photo != null)
                {
                    HttpResponseMessage response = null;
                    using (var content = new ByteArrayContent(photo))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        string requestUri = PrepareUri(id);
                        response = httpClient.PostAsync(requestUri, content).Result;
                    }

                    string ResponseString = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(ResponseString);

                    return json.ToString();
                }
                return string.Empty;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("ex : " + ex.Message);
                return string.Empty;
            }
        }

        async Task<JObject> FetchResultFromStatusUri(string statusUri)
        {
            JObject obj = null;
            int timeoutcounter = 0;
            HttpResponseMessage response = await httpClient.GetAsync(statusUri);
            string responseString = await response.Content.ReadAsStringAsync();
            obj = JObject.Parse(responseString);
            while ((!((string)obj.SelectToken("status")).Equals("Succeeded")) && (timeoutcounter++ < 60))
            {
                await Task.Delay(500);
                response = await httpClient.GetAsync(statusUri);
                responseString = await response.Content.ReadAsStringAsync();
                obj = JObject.Parse(responseString);
            }
            return obj;
        }


    }
}
