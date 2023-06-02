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
        public double? LowerThreshold
        {
            get;
            private set;
        }
        public double? UpperThreshold
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
        public double? LowerVolume
        {
            get;
            private set;
        }
        public double? UpperVolume
        {
            get;
            private set;
        }
        public double? BigContourVolume
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

        //Base image
        public void SetBaseImage(Image baseImage)
        {
            BaseImage = Conversion.BitmapToMat(baseImage);
        }



        //Thresholding
        private void CheckStoredValues()
        {
            if (LowerThreshold != null && UpperThreshold != null)
                ThresholdValuesStored = true;
            else
                ThresholdValuesStored = false;

            if (LowerVolume != null && UpperThreshold != null && BigContourVolume != null)
                ContourVolumeStored = true;
            else
                ContourVolumeStored = false;
        }
        public void ChangeLowerThreshold(string lowerThreshold)
        {
            LowerThreshold = Convert.ToDouble(lowerThreshold);
            CheckStoredValues();
        }
        public void ChangeUpperThreshold(string upperThreshold)
        {
            UpperThreshold = Convert.ToDouble(upperThreshold);
            CheckStoredValues();
        }
        public void SetGreyScaleImage(Mat greyScaleImage)
        {
            GreyScaleImage = greyScaleImage;
        }



        //volumes
        public void SetContoursVolumeRange(string lowerVolume, string upperVolume, string bigContourVolume)
        {
            LowerVolume = Convert.ToInt32(lowerVolume);
            UpperVolume = Convert.ToInt32(upperVolume);
            BigContourVolume = Convert.ToInt32(bigContourVolume);
            ContourVolumeStored = true;
        }
        public void ChangeLowerVolume(string lowerVolume)
        {
            LowerVolume = Convert.ToDouble(lowerVolume);
            CheckStoredValues();
        }
        public void ChangeUpperVolume(string upperVolume)
        {
            UpperVolume = Convert.ToDouble(upperVolume);
            CheckStoredValues();
        }
        public void ChangeBigVolume(string bigContourVolume)
        {
            BigContourVolume = Convert.ToDouble(bigContourVolume);
        }




        //events
    }
}
