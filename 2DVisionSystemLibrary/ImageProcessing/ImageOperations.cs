using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionSystem.Models;

namespace VisionSystemLibrary.ImageProcessing
{
    public class ImageOperations
    {
        public static Image ApplyGreyScale(VisionSystemController VisionController)
        {
            Mat baseImageCopy = new Mat();
            VisionController.BaseImage.CopyTo(baseImageCopy);
            Mat refGray = new Mat();
            Mat gaus = new Mat();
            Mat thresh = new Mat();

            //TODO prevent thresholding greyscale image !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //konwersja na odcienie szarosci
            Cv2.CvtColor(baseImageCopy, refGray, ColorConversionCodes.BGR2GRAY);
            //blur 
            OpenCvSharp.Size kernel = new OpenCvSharp.Size(3, 3);
            //Cv2.GaussianBlur(refGray, gaus, kernel,0,0);
            Cv2.MorphologyEx(refGray, gaus, MorphTypes.Close, null);
            //tresh 127 - 255
            Cv2.Threshold(gaus, thresh, (double)VisionController.LowerThreshold, (double)VisionController.UpperThreshold, ThresholdTypes.BinaryInv);
            VisionController.SetGreyScaleImage(thresh);
            Image output = Conversion.MatToBitmap(thresh);

            return output;
        }
        public static Image FindContours(VisionSystemController VisionController)
        {
            Mat baseImageCopy = new Mat();
            VisionController.BaseImage.CopyTo(baseImageCopy);
            HierarchyIndex[] hIndx;
            OpenCvSharp.Point[][] contours;
            
            Cv2.FindContours(VisionController.GreyScaleImage, out contours, out hIndx, RetrievalModes.List, ContourApproximationModes.ApproxNone);
            Cv2.DrawContours(baseImageCopy, contours, -1, new Scalar(0, 0, 255), thickness: 3);

            double a;
            int area;
            for (int i = 0; i < contours.Length; i++)
            {
                Cv2.Circle(baseImageCopy, contours[i][0], 1, new Scalar(0, 255, 0), 1);
                a = Cv2.ContourArea(contours[i]);
                area = Convert.ToInt32(a);
                Cv2.PutText(baseImageCopy, Convert.ToString(area), contours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);
            
            }
            VisionController.Contours = contours;
            return Conversion.MatToBitmap(baseImageCopy);
        }
        public static Image SortContoursOnVolume(VisionSystemController VisionController)
        {
            Mat baseImageCopy = new Mat();
            VisionController.BaseImage.CopyTo(baseImageCopy);
            OpenCvSharp.Point[][] contours = VisionController.Contours;
            double lowerVolume = (double)VisionController.LowerVolume;
            double upperVolume = (double)VisionController.UpperVolume;
            double bigContourVolume = (double)VisionController.BigContourVolume;


            int smallCount, connectedCount;
            OpenCvSharp.Point[][] smallContours;
            OpenCvSharp.Point[][] connectedContours;
            OpenCvSharp.Point[][] smallContoursSorted;
            OpenCvSharp.Point[][] connectedContoursSorted;

            (baseImageCopy, smallCount, connectedCount) = DrawContoursOnVolume(baseImageCopy, contours, lowerVolume, upperVolume, bigContourVolume);
            (smallContours, connectedContours) = StoreContoursOnVolume(contours, lowerVolume, upperVolume, bigContourVolume, smallCount, connectedCount);
            (smallContoursSorted, connectedContoursSorted) = LeftSortContours(smallContours, connectedContours);
            baseImageCopy = DrawContourInfo(baseImageCopy, smallContoursSorted, connectedContoursSorted);

            VisionController.SmallContoursSorted =  smallContoursSorted;
            VisionController.ConnectedContoursSorted = connectedContoursSorted;

            return Conversion.MatToBitmap(baseImageCopy);
        }


        /// <summary>
        /// contours in two sizes, normal and bigger(connected)
        /// </summary>
        /// <param name="imageClone"></param>
        /// <param name="contours"></param>
        /// <param name="Slower"></param>
        /// <param name="Mid"></param>
        /// <param name="Bupper"></param>
        /// <returns></returns>
        public static (Mat, int, int) DrawContoursOnVolume(Mat imageClone, OpenCvSharp.Point[][] contours, double Slower, double Mid, double Bupper)
        {
            //contours counters
            int smallContoursCounter = 0;
            int connectedContoursCounter = 0;

            for (int i = 0; i < contours.Length; i++)
            {
                var contourVolume = Cv2.ContourArea(contours[i]);

                if (contourVolume > Slower && contourVolume <= Mid)
                {
                    Cv2.DrawContours(imageClone, contours, i, new Scalar(0, 255, 0), thickness: 4);
                    smallContoursCounter++;
                }
                else if (contourVolume > Mid && contourVolume <= Bupper)
                {
                    Cv2.DrawContours(imageClone, contours, i, new Scalar(255, 0, 0), thickness: 4);
                    connectedContoursCounter++;
                }
            }
            //return obraz z narysowanymi konturami + liczbe malych i zloczonych
            return (imageClone, smallContoursCounter, connectedContoursCounter);
        }
        public static (OpenCvSharp.Point[][], OpenCvSharp.Point[][]) StoreContoursOnVolume(OpenCvSharp.Point[][] contours, double Slower, double Mid, double Bupper, int Scount, int Bcount)
        {
            int small = 0, connected = 0;
            OpenCvSharp.Point[][] smallContours = new OpenCvSharp.Point[Scount][];
            OpenCvSharp.Point[][] bigContours = new OpenCvSharp.Point[Bcount][];


            for (int i = 0; i < contours.Length; i++)
            {
                if (Cv2.ContourArea(contours[i]) > Slower && Cv2.ContourArea(contours[i]) <= Mid)
                {
                    smallContours[small] = contours[i];
                    small++;
                }
                else if (Cv2.ContourArea(contours[i]) > Mid && Cv2.ContourArea(contours[i]) <= Bupper)
                {
                    bigContours[connected] = contours[i];
                    connected++;
                }

            }
            return (smallContours, bigContours);
        }
        public static (OpenCvSharp.Point[][], OpenCvSharp.Point[][]) LeftSortContours(OpenCvSharp.Point[][] smallContours, OpenCvSharp.Point[][] bigContours)
        {


            OpenCvSharp.Point2f[] ftops = new OpenCvSharp.Point2f[4];
            OpenCvSharp.Point[] tops = new OpenCvSharp.Point[4];
            OpenCvSharp.Point[] temp;

            //sorotwanie przez wybor

            //pojedyncze
            for (int i = 0; i < smallContours.Length - 1; i++)
            {
                int min = i;

                for (int j = i + 1; j < smallContours.Length; j++)
                {
                    OpenCvSharp.Point center = GetCenter(smallContours[min]);
                    OpenCvSharp.Point center1 = GetCenter(smallContours[j]);

                    if (center.X > center1.X)
                    {
                        min = j;
                    }
                }
                if (min != i)
                {
                    temp = smallContours[i];
                    smallContours[i] = smallContours[min];
                    smallContours[min] = temp;
                }
            }

            //polaczone contury sortowanie
            for (int i = 0; i < bigContours.Length - 1; i++)
            {
                int min = i;

                for (int j = i + 1; j < bigContours.Length; j++)
                {
                    OpenCvSharp.Point center = GetCenter(bigContours[min]);
                    OpenCvSharp.Point center1 = GetCenter(bigContours[j]);

                    if (center.X > center1.X)
                    {
                        min = j;
                    }
                }
                if (min != i)
                {
                    temp = bigContours[i];
                    bigContours[i] = bigContours[min];
                    bigContours[min] = temp;
                }
            }


            return (smallContours, bigContours);
        }
        
        public static Mat DrawContourInfo(Mat baseImageCopy, OpenCvSharp.Point[][] smallContoursSorted, OpenCvSharp.Point[][] connectedContoursSorted)
        {
            OpenCvSharp.Point punkt;
            for (int i = 0; i < smallContoursSorted.Length; i++)
            {
                punkt = smallContoursSorted[i][0];
                //punkt.X = punkt.X - 100;
                //punkt.Y = punkt.Y - 100;
                Cv2.PutText(baseImageCopy, Convert.ToString(i), punkt, HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 255, 0), 1);
            }

            for (int i = 0; i < connectedContoursSorted.Length; i++)
            {
                punkt = connectedContoursSorted[i][0];
                //punkt.X = punkt.X - 100;
                //punkt.Y = punkt.Y - 100;
                Cv2.PutText(baseImageCopy, Convert.ToString(i), punkt, HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);
            }

            return baseImageCopy;
        }
        public static OpenCvSharp.Point GetCenter(OpenCvSharp.Point[] contour)
        {
            OpenCvSharp.Point center;
            var M = Cv2.Moments(contour, false);
            center.X = Convert.ToInt32(M.M10 / M.M00);
            center.Y = Convert.ToInt32(M.M01 / M.M00);

            return center;
        }



        public static Mat ContoursColorDetection(VisionSystemController VisionController)
        {

            Mat baseImageCopy = new Mat();
            VisionController.BaseImage.CopyTo(baseImageCopy);

            OpenCvSharp.Point[][] smallContoursSorted = VisionController.SmallContoursSorted;

            Mat hsv = new Mat();
            OpenCvSharp.Point center;
            Vec3b vector;

            Cv2.CvtColor(baseImageCopy, hsv, ColorConversionCodes.BGR2HSV_FULL);


            for (int i = 0; i < smallContoursSorted.Length; i++)
            {
                //TODO
                //string color = ContourColor(obliczenia, smallContoursSorted[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox13.Text), Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                //momenty
                var M = Cv2.Moments(smallContoursSorted[i], false);
                center.X = Convert.ToInt32(M.M10 / M.M00);
                center.Y = Convert.ToInt32(M.M01 / M.M00);

                vector = hsv.At<Vec3b>(center.Y, center.X);
                Scalar col = new Scalar(vector.Item0, vector.Item1, vector.Item2);

                Cv2.Circle(baseImageCopy, center, 3, new Scalar(0, 255, 0), 1);
                //Cv2.PutText(baseImageCopy, color + " " + Convert.ToString(col.Val1) + " " + Convert.ToString(col.Val2) + " " + Convert.ToString(col.Val3) + " ", smallContours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);


            }
            return baseImageCopy;
        }

        public static string ContourColor(OpenCvSharp.Point[] contour, Mat hsv,VisionSystemController VisionController)
        {
            double formRed = (double)VisionController.FromRed;
            double toRed = (double)VisionController.ToRed;
            double fromEwoRed = (double)VisionController.FromTwoRed;
            double toTwoRed = (double)VisionController.ToTwoRed; 
            double fromGreen = (double)VisionController.FromGreen;
            double toGreen = (double)VisionController.ToGreen;
            double fromBlue = (double)VisionController.FromBlue;
            double toBlue = (double)VisionController.ToBLue;

            //TODO contourcolor method

            //Vec3b vector;
            //OpenCvSharp.Point center;
            string result="";

            //Cv2.Split(hsv, out Mat[] mv);
            //mv[2] = mv[2] * 3;
            //Cv2.Merge(mv, hsv);


            ////momenty
            //var M = Cv2.Moments(contour, false);
            //center.X = Convert.ToInt32(M.M10 / M.M00);
            //center.Y = Convert.ToInt32(M.M01 / M.M00);

            //vector = hsv.At<Vec3b>(center.Y, center.X);
            //Scalar color = new Scalar(vector.Item0, vector.Item1, vector.Item2);


            ////red
            //if ((color.Val1 >= low_low_red && color.Val1 <= low_upper_red) ||
            //  (color.Val1 >= upper_low_red && color.Val1 <= upper_upper_red))
            //{
            //    result = "red";
            //}
            ////green
            //else if (color.Val1 >= lower_green && color.Val1 <= upper_green)
            //{
            //    result = "green";
            //}
            ////blue
            //else if (color.Val1 > lower_blue && color.Val1 <= upper_blue)
            //{
            //    result = "blue";
            //}
            //else
            //{
            //    result = "none";
            //}

            return result;
        }



    }
}
