using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MoodyMusicSharedProject
{
    class SpotifyServices
    {
        private string clientId = "22dfba5cea004c11a63531f9dfd4b3d4";
        private string secret = "ee471489041d4f5fa36e7b0d1125dfa7";
        private string authToken = "";

        private void SetAuthToken()
        {
            var webClient = new WebClient();

            var postparams = new NameValueCollection();

            postparams.Add("grant_type", "client_credentials");

            var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes(this.clientId+":"+this.secret));

            webClient.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authHeader);

            var tokenResponse = webClient.UploadValues("https://accounts.spotify.com/api/token", postparams);

            var textResponse = Encoding.UTF8.GetString(tokenResponse);
            Dictionary<string, string> respoonse = JsonConvert.DeserializeObject<Dictionary<string, string>>(textResponse);

            this.authToken = respoonse["access_token"];
        }

        public async Task<JsonValue> Search(string url)
        {
            // Set auth token //
            this.SetAuthToken();

            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers.Add("Authorization: Bearer " + this.authToken);

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue newJson = JsonValue.Load(stream);

                    // Return the JSON document:
                    return newJson;
                }
            }
        }
    }
}
