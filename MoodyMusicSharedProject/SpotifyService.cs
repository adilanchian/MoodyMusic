using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace MoodyMusicSharedProject
{
    class SpotifyService
    {
        private string authToken = "";
        HttpClient client;

        public SpotifyService()
        {
            client = new HttpClient();
        }

        public async void SetAuthToken()
        {
            // Set parameters to include with post //
            var postparams = new Dictionary<string, string>();
            postparams.Add("grant_type", "client_credentials");
            FormUrlEncodedContent content = new FormUrlEncodedContent(postparams);

            // Encode header values //
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(Constants.SpotifyClientId + ":" + Constants.SpotifySecret));

            // Add to client header //
            client.DefaultRequestHeaders.Add("Authorization", "Basic "+authHeader);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Get response //
                var postResponse = await client.PostAsync("https://accounts.spotify.com/api/token", content);

                // Get content //
                var value = await postResponse.Content.ReadAsStringAsync();

                // Get json //
                var json = JsonConvert.DeserializeObject<JObject>(value);

                // Set auth token property //
                this.authToken = (string)json["access_token"];
            }
            catch(Exception error)
            {
                Console.WriteLine("Getting auth token failed: "+error);
            }

        }

        public async void Search(string url, Action<JObject> callback)
        {
            // Clear headers from old Request //
            client.DefaultRequestHeaders.Clear();

            // Set properties //
            JObject responseValue = new JObject();
            Uri searchUrl = new Uri(url);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.authToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var getResponse = await client.GetAsync(searchUrl);

                // Check to see if success or fail //
               if (getResponse.IsSuccessStatusCode)
                {
                    // Get content //
                    var content = await getResponse.Content.ReadAsStringAsync();

                    // Deserialize //
                    JObject json = JsonConvert.DeserializeObject<JObject>(content);
                    callback(json);
                }
            }
            catch(Exception error)
            {
                Console.WriteLine("Response was unsuccessful: " + error);
            }
        }
    }
}
