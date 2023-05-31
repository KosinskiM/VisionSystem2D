using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionSystemLibrary.ImageProcessing;

namespace VisionSystem.Models
{
    public class VisionSystemController
    {
        //Images
        public Mat BaseImage
        {
            get;
            private set;
        }
        public Mat GreyScaleImage
        {
            get;
            private set;
        }






        //Vision Calibration Properties

        //threshold
        public bool ThresholdValuesStored
        {
            get;
            private set;
        }
        public int LowerThreshold
        {
            get;
            private set;
        }
        public int UpperThreshold
        {
            get;
            private set;
        }

        //Contours volume
        public bool ContourVolumeStored
        {
            get;
            private set;
        }
        public int LowerVolume
        {
            get;
            private set;
        }
        public int UpperVolume
        {
            get;
            private set;
        }
        public int BigContourVolume
        {
            get;
            private set;
        }

        //Contours
        //All contours
        public OpenCvSharp.Point[][] contours;

        //Sorted contours on volume
        public OpenCvSharp.Point[][] smallContours;
        public OpenCvSharp.Point[][] connectedContours;






        public OpenCvSharp.Point[][] toolUpSingleContours;     //maska narzedzia gorna
        public OpenCvSharp.Point[][] toolDownSingleContours;       //maska narzedzia dolna








        public VisionSystemController()
        {
            ThresholdValuesStored = false;
            ContourVolumeStored = false;
        }

        public void SetThreshold(string lowerThreshold, string upperThreshold)
        {
            LowerThreshold = Convert.ToInt32(lowerThreshold);
            UpperThreshold = Convert.ToInt32(upperThreshold);
            ThresholdValuesStored = true;
        }
        public void SetBaseImage(Image baseImage)
        {
            BaseImage = Conversion.BitmapToMat(baseImage);
        }
        public void SetGreyScaleImage(Mat greyScaleImage)
        {
            GreyScaleImage = greyScaleImage;
        }
        public void SetContoursVolumeRange(string lowerVolume, string upperVolume, string bigContourVolume)
        {
            LowerVolume = Convert.ToInt32(lowerVolume);
            UpperVolume = Convert.ToInt32(upperVolume);
            BigContourVolume = Convert.ToInt32(bigContourVolume);
            ContourVolumeStored = true;
        }

        //events
    }
}
