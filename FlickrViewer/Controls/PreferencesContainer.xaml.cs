using System.Collections.Generic;
using System.Windows.Controls;
using FlickrViewer.Controls.Preferences;
using FlickrViewer.Helpers;
using FlickrViewer.PreferenceControls;

namespace FlickrViewer.Controls
{
    public partial class PreferencesContainer : UserControl
    {
        public List<BasePreference> PreferenceControls { get; set; }

        public PreferencesManager PreferencesManager { get; set; }

        public PreferencesContainer()
        {
            this.PreferenceControls = new List<BasePreference>();

            InitializeComponent();
        }

        public void AddPreferenceControl(BasePreference basePreference)
        {
            this.PreferenceControls.Add(basePreference);

            this.stpPreferences.Children.Add(basePreference);

            this.LayoutRoot.Height = this.PreferenceControls.Count * 25 + 25;
        }
    }
}
