using System;
using System.Windows;
using FlickrViewer.Controls.Preferences;

namespace FlickrViewer.PreferenceControls
{
    public partial class FlickrUsername : BasePreference
    {
        public override event PreferenceChanged PreferenceChanged;

        public FlickrUsername(string currentFlickrUsername)
        {
            InitializeComponent();

            this.SetFlickrUsername(currentFlickrUsername);

            this.btnSet.Click += new RoutedEventHandler(btnSet_Click);
        }

        void btnSet_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.PreferenceChanged != null)
            {
                this.PreferenceChanged(new FlickrUsernameData(this.txbUsername.Text));
            }
        }

        private void SetFlickrUsername(string currentFlickrUsername)
        {
            this.txbUsername.Text = currentFlickrUsername;
        }
    }
}
