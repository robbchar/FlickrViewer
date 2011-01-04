using System.Windows.Controls;
using FlickrViewer.Controls;
using FlickrViewer.PreferenceControls;
using System;
using FlickrViewer.Controls.Preferences;

namespace FlickrViewer.Helpers
{
    public class PreferencesManager
    {
        private const string defaultFlickrEmail = "robb_c@yahoo.com";

        private string currentFlickrEmail = string.Empty;

        public event PreferenceChanged PreferenceChanged;

        public PreferencesContainer UserPreferencesContainer { get; set; }

        public string CurrentFlickrEmail
        {
            get
            {
                if (string.Compare(this.currentFlickrEmail, string.Empty) == 0)
                {
                    return defaultFlickrEmail;
                }

                return currentFlickrEmail;
            }

            set
            {
                this.currentFlickrEmail = value;
            }
        }

        public PreferencesManager(double top, double right, PreferencesContainer preferencesContainer)
        {
            this.CreateBasePanel(top, right, preferencesContainer);
        }

        private void CreateBasePanel(double top, double right, PreferencesContainer preferencesContainer)
        {
            this.UserPreferencesContainer = preferencesContainer;

            this.AddControls();
        }

        private void AddControls()
        {
            BasePreference flickrUserName = new FlickrUsername(this.CurrentFlickrEmail);
            flickrUserName.PreferenceChanged += new PreferenceChanged(flickrUserName_PreferenceChanged);
            this.UserPreferencesContainer.AddPreferenceControl(flickrUserName);
        }

        void flickrUserName_PreferenceChanged(PreferenceData preferenceData)
        {
            // set the properties internally
            if (preferenceData.GetType() == typeof(FlickrUsernameData))
            {
                FlickrUsernameData flickrUsernameData = preferenceData as FlickrUsernameData;
                this.CurrentFlickrEmail = flickrUsernameData.FlickrUsername;
            }

            // send a message to clients
            if (this.PreferenceChanged != null)
            {
                this.PreferenceChanged(preferenceData);
            }
        }
    }
}
