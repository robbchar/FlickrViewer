using System.Collections.ObjectModel;
using FlickrNet;

namespace FlickrViewer.Helpers
{
    public delegate void PhotosLoaded(ObservableCollection<Photo> photos);

    public class FlickrManager
    {
        #region Fields

        private const string MY_EMAIL = "robb_c@yahoo.com";

        public event PhotosLoaded PhotosLoaded;

        #endregion

        #region Properties

        public string FlickrEmail { get; set; }
        
        public Flickr FlickrInstance { get; set; }

        public int CurrentPage { get; set; }

        public FoundUser CurrentFoundUser { get; set; }

        public PhotoCollection CurrentPhotoCollection { get; set; }

        #endregion

        #region Methods

        public FlickrManager()
        {
            this.FlickrEmail = MY_EMAIL;

            this.FlickrInstance = new Flickr(App.Current.Resources["flickrApiKey"].ToString(), App.Current.Resources["flickrSecretKey"].ToString());
        }

        public void GetUserPhotos()
        {
            this.LoadUserFlickerImages();
        }

        public void GetNextPage()
        {
            this.GetFoundUserImages();
        }

        private void LoadFlickerImages()
        {
            this.CurrentFoundUser = null;
            PhotoSearchOptions options = new PhotoSearchOptions();
            options.Tags = "robb";
            this.FlickrInstance.PhotosSearchAsync(options, result =>
            {
                if (result.HasError)
                {
                    // use result.Code  or result.Error   
                    // to determine what the exception was.  
                }
                else
                {
                    PhotoCollection photos = result.Result;
                    this.GenerateItemsFromFlickrPhotos(photos);
                }
            });
        }

        private void LoadUserFlickerImages()
        {
            this.CurrentPage = 1;

            this.FlickrInstance.PeopleFindByEmailAsync(this.FlickrEmail,
                result =>
            {
                if (result.HasError)
                {
                    // use result.Code  or result.Error   
                    // to determine what the exception was.  
                }
                else
                {
                    this.CurrentFoundUser = result.Result;
                    this.GetFoundUserImages();
                }
            });
        }

        private void GetFoundUserImages()
        {
            PhotoSearchOptions options = new PhotoSearchOptions();
            options.UserId = this.CurrentFoundUser.UserId;
            options.Page = this.CurrentPage;

            this.FlickrInstance.PhotosSearchAsync(options, result =>
            {
                if (result.HasError)
                {
                    // use result.Code  or result.Error   
                    // to determine what the exception was.  
                }
                else
                {
                    PhotoCollection photos = result.Result;
                    this.GenerateItemsFromFlickrPhotos(photos);
                }
            });
        }

        private void GenerateItemsFromFlickrPhotos(PhotoCollection photos)
        {
            this.CurrentPhotoCollection = photos;
            ObservableCollection<Photo> photosCollection = new ObservableCollection<Photo>();
            for (int i = 0; i < photos.Count; i++)
            {
                var photo = photos[i];
                photosCollection.Add(photo);
            }

            if (this.PhotosLoaded != null)
            {
                this.PhotosLoaded(photosCollection);
            }

            if (this.CurrentPhotoCollection != null && this.CurrentPhotoCollection.Pages != this.CurrentPage)
            {
                this.CurrentPage++;
            }
            else
            {
                this.CurrentPage = 1;
            }
        }

        #endregion
    }
}
