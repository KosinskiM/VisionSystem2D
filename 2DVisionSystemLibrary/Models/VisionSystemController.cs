﻿using MathNet.Numerics.LinearAlgebra;
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


        //Colors Range
        public double? FromRed { get; private set; }
        public double? ToRed { get; private set; }
        public double? FromTwoRed { get; private set; }
        public double? ToTwoRed { get; private set; }
        public double? FromGreen { get; private set; }
        public double? ToGreen { get; private set; }
        public double? FromBlue { get; private set; }
        public double? ToBlue { get; private set; }


        //real values properties
        public OpenCvSharp.Point2f[][] MarkerOut { get; set; }
        public double? Ppm { get; set; }
        public Matrix<double> Abcd { get; set; }


        //Contours
        //All contours
        public OpenCvSharp.Point[][] Contours;

        //Sorted contours on volume
        public OpenCvSharp.Point[][] SmallContoursSorted;
        public OpenCvSharp.Point[][] ConnectedContoursSorted;






        public OpenCvSharp.Point[][] ToolUpSingleContours;     //maska narzedzia gorna
        public OpenCvSharp.Point[][] ToolDownSingleContours;       //maska narzedzia dolna




        //created and stored Element
        public List<StoredElement> StoredSmallElement = new List<StoredElement>();





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


        //Colors
        public void ChangeFromRedRange(string range)
        {
            FromRed = Convert.ToDouble(range);
        }
        public void ChangeToRedRange(string range)
        {
            ToRed = Convert.ToDouble(range);
        }
        public void ChangeFromTwoRedRange(string range)
        {
            FromTwoRed = Convert.ToDouble(range);
        }
        public void ChangeToTwoRedRange(string range)
        {
            ToTwoRed = Convert.ToDouble(range);
        }
        public void ChangeFromGreenRange(string range)
        {
            FromGreen = Convert.ToDouble(range);
        }
        public void ChangeToGreenRange(string range)
        {
            ToGreen = Convert.ToDouble(range);
        }
        public void ChangeFromBlueRange(string range)
        {
            FromBlue = Convert.ToDouble(range);
        }
        public void ChangeToBlueRange(string range)
        {
            ToBlue = Convert.ToDouble(range);
        }


        //real values
        public void SetPpm(double ppm)
        {
            Ppm = Ppm;
        }

        public void SetAbcd(Matrix<double> abcd)
        {
            Abcd = abcd;
        }

        public void SetMarkerOut(OpenCvSharp.Point2f[][] markerOut)
        {
            MarkerOut = markerOut;
        }


        //events
    }
}
