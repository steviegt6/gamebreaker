using Avalonia.Controls;
using Avalonia.Media.Imaging;

namespace GameBreaker.Editor.Util.Extensions;

public static class BitmapExtensions {
    public static Image AsImage(this Bitmap bitmap) {
        return new Image {
            Source = bitmap,
        };
    }
}
