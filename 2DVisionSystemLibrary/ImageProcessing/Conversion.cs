using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionSystemLibrary.ImageProcessing
{
    public class Conversion
    {
        //converter z mat na bitmape 
        public static Bitmap MatToBitmap(Mat image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
        }

        public static Mat BitmapToMat(Image image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToMat((Bitmap)image);
        }


    }
}
