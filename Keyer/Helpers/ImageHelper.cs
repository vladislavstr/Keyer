using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;

namespace Keyer.Helpers
{
    public static class ImageHelper
    {
        public static Bitmap LoadFromResource(Uri resourceUri)
        {
            return new Bitmap(AssetLoader.Open(resourceUri));
        }
    }
}
