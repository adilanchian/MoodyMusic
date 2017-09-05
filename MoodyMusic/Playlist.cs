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
using System.Json;

namespace MoodyMusic
{
    class Playlist
    {
        public string Name;
        public string ExternalUrl;
        public JsonValue Images;
        public string OwnerID;
    }
}