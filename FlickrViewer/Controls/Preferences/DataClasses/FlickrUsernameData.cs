
namespace FlickrViewer.Controls.Preferences
{
    public class FlickrUsernameData : PreferenceData
    {
        public string FlickrUsername { get; set; }

        public FlickrUsernameData(string flickUsername)
        {
            this.FlickrUsername = flickUsername;
        }
    }
}
