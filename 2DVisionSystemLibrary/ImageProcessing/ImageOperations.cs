using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionSystemLibrary.ImageProcessing
{
    public class ImageOperations
    {

        public static Image ConvertToGreyScale(Image image,string lowerThreshold, string upperThreshold)
        {
            Mat original = Conversion.BitmapToMat(image);
            Mat refGray = new Mat();
            Mat gaus = new Mat();
            Mat thresh = new Mat();

            //TODO prevent thresholding greyscale image !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //konwersja na odcienie szarosci
            Cv2.CvtColor(original, refGray, ColorConversionCodes.BGR2GRAY);
            //blur 
            OpenCvSharp.Size kernel = new OpenCvSharp.Size(3, 3);
            //Cv2.GaussianBlur(refGray, gaus, kernel,0,0);
            Cv2.MorphologyEx(refGray, gaus, MorphTypes.Close, null);
            //tresh 127 - 255
            Cv2.Threshold(gaus, thresh, Convert.ToInt32(lowerThreshold), Convert.ToInt32(upperThreshold), ThresholdTypes.BinaryInv);

            Image output = Conversion.MatToBitmap(thresh);

            return output;
        }

    }
}
