using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Android.Support.V7.Widget.RecyclerView;

namespace MoodyMusic
{
    class PlaylistViewHolder : Android.Support.V7.Widget.RecyclerView.ViewHolder
    {
        public ImageView PlaylistImage { get; private set; }
        public TextView PlaylistTitle { get; private set; }
        public TextView PlaylistCreator { get; private set; }

        public PlaylistViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            // Locate and cache view references:
            PlaylistImage = itemView.FindViewById<ImageView>(Resource.Id.playlistImage);
            PlaylistTitle = itemView.FindViewById<TextView>(Resource.Id.playlistTitle);
            PlaylistCreator = itemView.FindViewById<TextView>(Resource.Id.createdBy);

            // Add Click Event //
            itemView.Click += (sender, e) => listener(base.Position);
        }

        public void OpenPlaylist(string playlistUrl)
        {
            var uri = Android.Net.Uri.Parse(playlistUrl);
            var intent = new Intent(Intent.ActionView, uri);
            Application.Context.StartActivity(intent);
        }
    }
}