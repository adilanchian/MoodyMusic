using MoodyMusic.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MoodyMusic.WinApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // Properites //
            SpotifyService spotify = new SpotifyService();

            // Spotify Auth Code //
            spotify.SetAuthToken();

            moodSubmit.Click += (sender, error) => 
            {
                // Get text from input //
                string enteredMood = moodInput.Text;

                // Build URL endpoint //
                string endpoint = "https://api.spotify.com/v1/search?q=" + enteredMood + "&type=playlist" + "&limit=10";
                spotify.Search(endpoint, (json) => this.populateList(json));

                // Remove text from input to start over //
                moodInput.Text = "";
                moodTitle.Text = "When you are feeling " + enteredMood.ToLower();
            };

            playlistTable.ItemClick += async (sender, item) =>
            {
                // On select create URI and open in browser //
                Playlist selectedPlaylist = (Playlist)item.ClickedItem;
                Uri uri = new Uri(selectedPlaylist.ExternalUrl);
                await Windows.System.Launcher.LaunchUriAsync(uri);
            };
        }

        private void populateList(JObject json)
        {
            ArrayList playlists = new ArrayList();

            // Clear ArrayList //
            playlists.Clear();

            foreach (JObject item in json["playlists"]["items"])
            {
                // Create playlist object //
                MoodyMusic.Shared.Playlist playlist = new MoodyMusic.Shared.Playlist();

                // Set properties of playlist //
                playlist.Name = (string)item["name"];
                playlist.ExternalUrl = (string)item["external_urls"]["spotify"];
                playlist.Image = (string)item["images"][0]["url"];
                playlist.OwnerID = (string)item["owner"]["id"];
                playlists.Add(playlist);
            }

            this.playlistTable.ItemsSource = playlists;
        }
    }
}
