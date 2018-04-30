using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using XamarinCognitiveServices.Interfaces;

namespace XamarinCognitiveServices.Services
{
    public class VisionAnalizeService
    {
        readonly HttpClient httpClient;

        public VisionAnalizeService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Constants.ComputerVisionApiKey);
        }

        public async Task<ObservableCollection<string>> FetchPrintedWordList(byte[] photo)
        {
            ObservableCollection<string> wordList = new ObservableCollection<string>();
            if (photo != null)
            {
                HttpResponseMessage response = null;
                using (var content = new ByteArrayContent(photo))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await httpClient.PostAsync(Constants.ComputerVisionApiOcrUrl, content);
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

        public async Task<ObservableCollection<string>> FetchHandwrittenWordList(byte[] photo)
        {
            ObservableCollection<string> wordList = new ObservableCollection<string>();
            if (photo != null)
            {
                // Make the POST request to the handwriting recognition URL
                HttpResponseMessage response = null;
                using (var content = new ByteArrayContent(photo))
                {
                    // The media type of the body sent to the API. 
                    // "application/octet-stream" defines an image represented 
                    // as a byte array
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await httpClient.PostAsync(Constants.ComputerVisionApiHandwritingUrl, content);
                }

                // Fetch results
                IEnumerable<string> operationLocationValues;
                string statusUri = string.Empty;
                if (response.Headers.TryGetValues("Operation-Location", out operationLocationValues))
                {
                    statusUri = operationLocationValues.FirstOrDefault();

                    // Ping status URL, wait for processing to finish 
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

        public async Task<string> ImageAnalize(byte[] photo)
        {
            if (photo != null)
            {
                HttpResponseMessage response = null;
                using (var content = new ByteArrayContent(photo))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await httpClient.PostAsync(Constants.ComputerVisionApiImageAnalize, content);
                }

                string ResponseString = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(ResponseString);

                return json.ToString();

            }
            return string.Empty;
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
