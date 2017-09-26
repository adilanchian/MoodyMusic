using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections;
using Android.Support.V7.Widget;
using Newtonsoft.Json.Linq;
using MoodyMusicSharedProject;
using System.Threading.Tasks;
using System;

namespace MoodyMusic
{
    [Activity(Label = "MoodyMusic", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            // Proprties //
            EditText moodInput = FindViewById<EditText>(Resource.Id.moodInput);
            TextView moodTitle = FindViewById<TextView>(Resource.Id.moodTitle);
            RecyclerView playlistRecylerView = FindViewById<RecyclerView>(Resource.Id.playlistRecyclerView);
            RecyclerView.LayoutManager recyclerManager;
            SpotifyService spotify = new SpotifyService();

            spotify.SetAuthToken();

            // Get reference to the submit button and input text //
            Button moodSubmitButton = FindViewById<Button>(Resource.Id.moodSubmitButton);

            moodSubmitButton.Click += (sender, error) =>
            {
                // Get text from input //
                string enteredMood = moodInput.Text;

                // Add layout manager //
                recyclerManager = new LinearLayoutManager(this);
                playlistRecylerView.SetLayoutManager(recyclerManager);

                // Build URL endpoint //
                string endpoint = "https://api.spotify.com/v1/search?q=" + enteredMood + "&type=playlist" + "&limit=10";
                spotify.Search(endpoint, (json) => this.populateList(json, playlistRecylerView));

                // Remove text from input to start over //
                moodInput.Text = "";
                moodTitle.Text = "When you are feeling " + enteredMood.ToLower();
            };
        }

        private void populateList(JObject json, RecyclerView recyler)
        {
            ArrayList playlists = new ArrayList();
            PlaylistAdapter playlistAdapter;

            // Clear ArrayList //
            playlists.Clear();

            foreach (JObject item in json["playlists"]["items"])
            {
                // Create playlist object //
                Playlist playlist = new Playlist();

                // Set properties of playlist //
                playlist.Name = (string)item["name"];
                playlist.ExternalUrl = (string)item["external_urls"]["spotify"];
                playlist.Image = (string)item["images"][0]["url"];
                playlist.OwnerID = (string)item["owner"]["id"];
                playlists.Add(playlist);
            }


            // Instantiate adapter //
            playlistAdapter = new PlaylistAdapter(playlists);
            recyler.SetAdapter(playlistAdapter);

            // Clear and update RecyclerView //
            playlistAdapter.NotifyItemRangeChanged(0, 10);
        }
    }
}