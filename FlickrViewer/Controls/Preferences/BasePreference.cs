using System.Windows.Controls;
using FlickrViewer.Controls.Preferences;

namespace FlickrViewer.PreferenceControls
{
    public delegate void PreferenceChanged(PreferenceData preferenceData);

    public abstract class BasePreference : UserControl
    {
        public abstract event PreferenceChanged PreferenceChanged;

        public BasePreference()
        {
        }
    }
}
