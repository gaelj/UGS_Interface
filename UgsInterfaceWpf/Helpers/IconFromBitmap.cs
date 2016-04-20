using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace UGS.Helpers
{
    public static class IconFromBitmapHelper
    {
        public static System.Drawing.Icon GetIconFromBitmap(string path)
        {
            using (Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/UGS;component/" + path)).Stream)
                return System.Drawing.Icon.FromHandle((new System.Drawing.Bitmap(iconStream)).GetHicon());
        }
    }
}
