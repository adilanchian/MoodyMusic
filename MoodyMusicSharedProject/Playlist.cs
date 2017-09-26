namespace MoodyMusic.Shared
{
    class Playlist
    {
        public string Name { get; set; }
        public string ExternalUrl { get; set; }
        public string Image { get; set; }
        public string OwnerID { get; set; }

        //-- Friendly Strings --//
        /* public string FriendlyName
        {
            get
            {
                return "Title: "+this.Name;
            }
        } */

        public string FriendlyOwnerID
        {
            get
            {
                return "Created by: "+this.OwnerID;
            }
        }
    }
}