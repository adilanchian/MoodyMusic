using Android.App;
using Android.Content;
using Android.Views;
using Android.Support.V7.Widget;
using System.Collections;
using Android.Graphics;
using System.Net;
using MoodyMusicSharedProject;

namespace MoodyMusic
{
    public class PlaylistAdapter : RecyclerView.Adapter
    {
        public ArrayList playlists;
        public PlaylistAdapter (ArrayList playlists)
        {
            this.playlists = playlists;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View playlistView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.Playlist_Cell, parent, false);

            PlaylistViewHolder holder = new PlaylistViewHolder(playlistView, PlaylistSelected);

            return holder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            PlaylistViewHolder viewHolder = holder as PlaylistViewHolder;

            Playlist currentPlaylist = playlists[position] as Playlist;

            // Load the photo image resource from the photo album:
            var bitmap = GetImageBitmapFromUrl(currentPlaylist.Image);

            // Set data to cells //
            viewHolder.PlaylistImage.SetImageBitmap(bitmap);
            viewHolder.PlaylistTitle.Text = currentPlaylist.Name;
            viewHolder.PlaylistCreator.Text = "Created by " + currentPlaylist.OwnerID;
        }

        public override int ItemCount
        {
            get { return playlists.Count; }
        }

        private void PlaylistSelected(int position)
        {
            Playlist currentPlaylist = playlists[position] as Playlist;
            var playlistUri = Android.Net.Uri.Parse(currentPlaylist.ExternalUrl);
            var browser = new Intent(Intent.ActionView, playlistUri);
            browser.AddFlags(ActivityFlags.NewTask);
            Application.Context.StartActivity(browser);
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        public void ClearList()
        {
            this.playlists.Clear();
        }
    }
}