using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FlickrViewer
{
    public class AnimationManager
    {
        #region Methods

        public static DoubleAnimation GetAnimation()
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();

            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(RandomNumber.Next(5) + 5));

            int left = -300;
            int right = (int)App.Current.Host.Content.ActualWidth + 300;

            if (RandomNumber.Next(2) == 1)
            {
                doubleAnimation.From = left;
                doubleAnimation.To = right;
            }
            else
            {
                doubleAnimation.From = right;
                doubleAnimation.To = left;
            }

            return doubleAnimation;
        }

        public static Image GetImage()
        {
            Image image = new Image();

            image.Height = RandomNumber.Next(150) + 50;

            image.Stretch = Stretch.Uniform;

            return image;
        }

        public static Rectangle CreateRectangle()
        {
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = new SolidColorBrush(Colors.Red);

            rectangle.Height = 50;
            rectangle.Width = 50;

            return rectangle;
        }

        #endregion
    }
}
