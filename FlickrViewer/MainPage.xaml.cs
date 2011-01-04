using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using FlickrNet;
using FlickrViewer.Helpers;
using FlickrViewer.Controls.Preferences;

namespace FlickrViewer
{
    public partial class MainPage : UserControl
    {
        #region Fields

        #endregion

        #region Properties

        public int CurrentImage { get; set; }

        public FlickrManager FlickrManager { get; set; }

        public List<AnimationElements> UnusedAnimationElements { get; set; }

        public ObservableCollection<Photo> Photos { get; set; }

        public PreferencesManager PreferencesManager { get; set; }

        #endregion

        #region Methods

        public MainPage()
        {
            InitializeComponent();

            this.UnusedAnimationElements = new List<AnimationElements>();
            this.CurrentImage = 0;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500); // 500 Milliseconds
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Start();

            this.FlickrManager = new FlickrManager();
            this.FlickrManager.PhotosLoaded += new PhotosLoaded(flickrManager_PhotosLoaded);
            this.FlickrManager.GetUserPhotos();

            this.PreferencesManager = new PreferencesManager(App.Current.Host.Content.ActualHeight, App.Current.Host.Content.ActualWidth, this.UserPreferences);
            this.PreferencesManager.PreferenceChanged += new PreferenceControls.PreferenceChanged(PreferencesManager_PreferenceChanged);
        }

        void PreferencesManager_PreferenceChanged(Controls.Preferences.PreferenceData preferenceData)
        {
            if (preferenceData.GetType() == typeof(FlickrUsernameData))
            {
                this.Photos = new ObservableCollection<Photo>();
                this.CurrentImage = 0;
                this.FlickrManager.FlickrEmail = this.PreferencesManager.CurrentFlickrEmail;
                this.FlickrManager.GetUserPhotos();
            }
        }

        private void DoImageAnimation()
        {
            if (this.Photos != null)
            {
                AnimationElements animationElements = Associator.Instance.GetAnimationElements(this.GetUniqueResourceId());

                if (animationElements.Storyboard.Children[0] is DoubleAnimation)
                {
                    DoubleAnimation doubleAnimation = animationElements.Storyboard.Children[0] as DoubleAnimation;

                    animationElements.Image.SetValue(Canvas.LeftProperty, doubleAnimation.From);
                }

                // check to see if the elements have been added to the screen
                if (this.LayoutRoot.Resources.Contains(animationElements.Image.Name) == false)
                {
                    this.LayoutRoot.Resources.Add(animationElements.Image.Name, animationElements.Storyboard);
                    this.cnvImages.Children.Add(animationElements.Image);
                    animationElements.Storyboard.Completed += new EventHandler(storyBoard_Completed);
                }

                int topMargin = RandomNumber.Next((int)(App.Current.Host.Content.ActualHeight - animationElements.Image.Height));
                animationElements.Image.Margin = new Thickness(-100, topMargin, 0, 0);

                if (this.CurrentImage < this.Photos.Count)
                {
                    animationElements.Image.Source = new BitmapImage(new Uri(this.Photos[this.CurrentImage].SmallUrl));
                    this.CurrentImage++;
                }

                animationElements.Storyboard.Begin();

                if (this.CurrentImage == this.Photos.Count - 10)
                {
                    this.FlickrManager.GetNextPage();
                }
            }
        }

        private string GetUniqueResourceId()
        {
            string key = "unique_id" + RandomNumber.Next(Environment.TickCount);

            while (this.LayoutRoot.Resources[key] != null)
            {
                key = "unique_id" + RandomNumber.Next(Environment.TickCount);
            }

            return key;
        }

        #endregion

        #region Events

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DoImageAnimation();
        }

        void flickrManager_PhotosLoaded(ObservableCollection<Photo> photos)
        {
            if (this.Photos == null)
            {
                this.Photos = photos;
            }
            else
            {
                foreach (var photo in photos)
                {
                    this.Photos.Add(photo);
                }
            }
        }

        void storyBoard_Completed(object sender, EventArgs e)
        {
            Associator.Instance.StoryboardCompleted(sender as Storyboard);
        }

        void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            this.DoImageAnimation();
        }

        #endregion
    }
}
