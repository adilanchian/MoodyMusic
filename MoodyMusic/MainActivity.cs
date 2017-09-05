using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Threading.Tasks;
using System.Json;
using System.Net;
using System.IO;
using System.Collections;
using Android.Support.V7.Widget;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MoodyMusic
{
    [Activity(Label = "MoodyMusic", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        private string bearer = "22dfba5cea004c11a63531f9dfd4b3d4:ee471489041d4f5fa36e7b0d1125dfa7";
        private string spotifyAppToken = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
 
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            // Proprties //
            EditText moodInput = FindViewById<EditText>(Resource.Id.moodInput);
            TextView moodTitle = FindViewById<TextView>(Resource.Id.moodTitle);
            ArrayList playlists = new ArrayList();
            RecyclerView playlistRecylerView = FindViewById<RecyclerView>(Resource.Id.playlistRecyclerView);
            RecyclerView.LayoutManager recyclerManager;
            PlaylistAdapter playlistAdapter;

            // Get reference to the submit button and input text //
            Button moodSubmitButton = FindViewById<Button>(Resource.Id.moodSubmitButton);
            moodSubmitButton.Click += async (sender, error) =>
            {
                // Clear ArrayList //
                playlists.Clear();

                // Get oAuth Token //
                spotifyAppToken = GetSpotifyToken("https://accounts.spotify.com/api/token");

                // Get text from input //
                string enteredMood = moodInput.Text;

                // Build URL endpoint //
                string endpoint = "https://api.spotify.com/v1/search?q=" + enteredMood + "&type=playlist"+"&limit=10";
                JsonValue json = await FetchSpotifyPlaylist(endpoint);

                // Add layout manager //
                recyclerManager = new LinearLayoutManager(this);
                playlistRecylerView.SetLayoutManager(recyclerManager);

                foreach (JsonValue item in json["playlists"]["items"])
                {
                    // Create playlist object //
                    Playlist playlist = new Playlist();

                    // Set properties of playlist //
                    playlist.Name = item["name"];
                    playlist.ExternalUrl = item["external_urls"]["spotify"];
                    playlist.Images = item["images"];
                    playlist.OwnerID = item["owner"]["id"];

                    playlists.Add(playlist);
                }

                // Instantiate adapter //
                playlistAdapter = new PlaylistAdapter(playlists);
                playlistRecylerView.SetAdapter(playlistAdapter);

                // Clear and update RecyclerView //
                playlistAdapter.NotifyDataSetChanged();

                // Remove text from input to start over //
                moodInput.Text = "";

                moodTitle.Text = "When you are feeling "+enteredMood.ToLower();
            };
        }

        private string GetSpotifyToken(string url)
        {
            var webClient = new WebClient();

            var postparams = new NameValueCollection();

            postparams.Add("grant_type", "client_credentials");

            var authHeader = Convert.ToBase64String(Encoding.Default.GetBytes(bearer));

            webClient.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authHeader);

            var tokenResponse = webClient.UploadValues(url, postparams);

            var textResponse = Encoding.UTF8.GetString(tokenResponse);
            Dictionary<string, string> respoonse = JsonConvert.DeserializeObject<Dictionary<string, string>>(textResponse);

            return respoonse["access_token"];
        }

        byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private async Task<JsonValue> FetchSpotifyPlaylist(string url)
        {
            ServicePointManager.DnsRefreshTimeout = 0;
            WebRequest myRequest = WebRequest.Create(url);
            myRequest.ContentType = "application/json";
            myRequest.Method = "GET";
            myRequest.Headers.Add("Authorization: Bearer "+spotifyAppToken);

            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";
            request.Headers.Add("Authorization: Bearer "+ this.spotifyAppToken);

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

