using Android.App;
using Android.Widget;
using Android.OS;
using System.Json;
using System.Collections;
using Android.Support.V7.Widget;
using MoodyMusicSharedProject;

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
            ArrayList playlists = new ArrayList();
            RecyclerView playlistRecylerView = FindViewById<RecyclerView>(Resource.Id.playlistRecyclerView);
            RecyclerView.LayoutManager recyclerManager;
            PlaylistAdapter playlistAdapter;
            SpotifyServices spotify = new SpotifyServices();

            // Get reference to the submit button and input text //
            Button moodSubmitButton = FindViewById<Button>(Resource.Id.moodSubmitButton);
            moodSubmitButton.Click += async (sender, error) =>
            {
                // Clear ArrayList //
                playlists.Clear();

                // Get text from input //
                string enteredMood = moodInput.Text;

                // Build URL endpoint //
                string endpoint = "https://api.spotify.com/v1/search?q=" + enteredMood + "&type=playlist" + "&limit=10";
                JsonValue json = await spotify.Search(endpoint);

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
                //playlistAdapter.NotifyDataSetChanged();
                playlistAdapter.NotifyItemRangeChanged(0, 10);

                // Remove text from input to start over //
                moodInput.Text = "";

                moodTitle.Text = "When you are feeling " + enteredMood.ToLower();
            };
        }
    }
}

