using System;

namespace FlickrViewer
{
    public class RandomNumber
    {
        #region Fields

        static Random random;

        #endregion

        #region Methods

        static RandomNumber()
        {
            random = new Random();
        }

        public static int Next(int max)
        {
            return Next(0, max);
        }

        public static int Next(int min, int max)
        {
            return random.Next(min, max);
        }

        #endregion
    }
}