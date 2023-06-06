using MathNet.Numerics.LinearAlgebra;
using NumSharp;
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


        //Color detection
        public static Image ContoursColorDetection(VisionSystemController VisionController)
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
                string color = ContourColor(smallContoursSorted[i], hsv , VisionController);

                //momenty
                var M = Cv2.Moments(smallContoursSorted[i], false);
                center.X = Convert.ToInt32(M.M10 / M.M00);
                center.Y = Convert.ToInt32(M.M01 / M.M00);

                vector = hsv.At<Vec3b>(center.Y, center.X);
                Scalar col = new Scalar(vector.Item0, vector.Item1, vector.Item2);

                Cv2.Circle(baseImageCopy, center, 3, new Scalar(0, 255, 0), 1);

                string text = color + " " + Convert.ToString(col.Val1) + " " + Convert.ToString(col.Val2) + " " + Convert.ToString(col.Val3);
                Cv2.PutText(baseImageCopy, text, smallContoursSorted[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);
            }

            return Conversion.MatToBitmap(baseImageCopy);
        }
        public static string ContourColor(OpenCvSharp.Point[] contour, Mat hsv,VisionSystemController VisionController)
        {
            double fromRed = (double)VisionController.FromRed;
            double toRed = (double)VisionController.ToRed;
            double fromTwoRed = (double)VisionController.FromTwoRed;
            double toTwoRed = (double)VisionController.ToTwoRed; 
            double fromGreen = (double)VisionController.FromGreen;
            double toGreen = (double)VisionController.ToGreen;
            double fromBlue = (double)VisionController.FromBlue;
            double toBlue = (double)VisionController.ToBlue;

            Vec3b vector;
            OpenCvSharp.Point center;
            string result = "";

            Cv2.Split(hsv, out Mat[] mv);
            mv[2] = mv[2] * 3;
            Cv2.Merge(mv, hsv);


            //momenty
            var M = Cv2.Moments(contour, false);
            center.X = Convert.ToInt32(M.M10 / M.M00);
            center.Y = Convert.ToInt32(M.M01 / M.M00);

            vector = hsv.At<Vec3b>(center.Y, center.X);
            Scalar color = new Scalar(vector.Item0, vector.Item1, vector.Item2);


            //red
            if ((color.Val1 >= fromRed && color.Val1 <= toRed) ||
              (color.Val1 >= fromTwoRed && color.Val1 <= toTwoRed))
            {
                result = "red";
            }
            //green
            else if (color.Val1 >= fromGreen && color.Val1 <= toGreen)
            {
                result = "green";
            }
            //blue
            else if (color.Val1 > fromBlue && color.Val1 <= toBlue)
            {
                result = "blue";
            }
            else
            {
                result = "none";
            }

            return result;
        }


        //shape detection
        public static Image ContoursShapeDetection(VisionSystemController VisionController)
        {
            Mat baseImageCopy = new Mat();
            VisionController.BaseImage.CopyTo(baseImageCopy);

            OpenCvSharp.Point[][] smallContoursSorted = VisionController.SmallContoursSorted;

            for (int i = 0; i < smallContoursSorted.Length; i++)
            {
                string shape;
                shape = GetContourShape(smallContoursSorted[i]);

                if (shape == "triangle")
                {
                    Cv2.PutText(baseImageCopy, "triangle", smallContoursSorted[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                }
                else if (shape == "square")
                {
                    Cv2.PutText(baseImageCopy, "square", smallContoursSorted[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                }
                else if (shape == "rectangle")
                {
                    Cv2.PutText(baseImageCopy, "rectangle", smallContoursSorted[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                }
                else if (shape == "pentagon")
                {
                    Cv2.PutText(baseImageCopy, "pentagon", smallContoursSorted[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                }
                else if (shape == "circle")
                {
                    Cv2.PutText(baseImageCopy, "circle", smallContoursSorted[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                }
            }
            return Conversion.MatToBitmap(baseImageCopy);
        }
        public static string GetContourShape(OpenCvSharp.Point[] contour)
        {
            string shape;
            double perimeter;
            OpenCvSharp.Point[] approx;
            double A, B, C, w, h, aspectRatio;

            int[] peaks = new int[contour.Length];
            //smalindex 6 elementow
            for (int j = 0; j < contour.Length; j++)
            {
                float[] sum = new float[contour.Length];
                float[] diff = new float[contour.Length];
                for (int i = 0; i < contour.Length; i++)
                {
                    sum[i] = contour[i].X + contour[i].Y;
                    diff[i] = contour[i].X - contour[i].Y;
                }
                //wierzcholki
                peaks[0] = np.argmin(sum);   //topleft
                peaks[1] = np.argmax(sum);   //bottom right
                peaks[2] = np.argmin(diff);    //top right
                peaks[3] = np.argmax(diff);      //bottom left
            }

            //obwod
            perimeter = Cv2.ArcLength(contour, true);
            approx = Cv2.ApproxPolyDP(contour, 0.04 * perimeter, true);

            if (approx.Length == 3)
            {
                shape = "triangle";
            }
            else if (approx.Length == 4)
            {
                //dlugosc odcinka sqrt((x2 - x1)^2 + (y2 - y1)^2)
                A = Math.Pow((contour[peaks[3]].X - contour[peaks[0]].X), 2);
                B = Math.Pow((contour[peaks[3]].Y - contour[peaks[0]].Y), 2);
                C = A + B;
                w = Math.Sqrt(C);
                A = Math.Pow((contour[peaks[2]].X - contour[peaks[0]].X), 2);
                B = Math.Pow((contour[peaks[2]].Y - contour[peaks[0]].Y), 2);
                C = A + B;
                h = Math.Sqrt(C);
                aspectRatio = w / h;

                if (aspectRatio >= 0.95 && aspectRatio <= 1.05)
                {
                    shape = "square";  //square kwadrat
                }
                else
                {
                    shape = "rectangle";  //rectangle prostokat
                }
            }
            else if (approx.Length == 5)
            {
                shape = "pentagon"; //pentagon
            }
            else
            {
                shape = "circle";//circle
            }
            return shape;
        }


        //angle detection
        public static Image ContoursAngleDetection(VisionSystemController VisionController)
        {
            Mat baseImageCopy = new Mat();
            VisionController.BaseImage.CopyTo(baseImageCopy);

            OpenCvSharp.Point[][] smallContoursSorted = VisionController.SmallContoursSorted;

            //nowa metoda
            Rect r;
            float ang;

            for (int i = 0; i < smallContoursSorted.Length; i++)
            {
                int angle = 0;
                angle = GetAngle(smallContoursSorted[i]);
                Cv2.PutText(baseImageCopy, Convert.ToString(angle), smallContoursSorted[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 255), 1);

            }

            return Conversion.MatToBitmap(baseImageCopy);
        }
        public static int GetAngle(OpenCvSharp.Point[] contour)
        {
            float ang;
            OpenCvSharp.Point center;
            OpenCvSharp.Point2f[] ftops;
            OpenCvSharp.Point[] tops = new OpenCvSharp.Point[4];

            //min area rectangle with angle
            RotatedRect rot = Cv2.MinAreaRect(contour);

            //center
            var M = Cv2.Moments(contour, false);
            center.X = Convert.ToInt32(M.M10 / M.M00);
            center.Y = Convert.ToInt32(M.M01 / M.M00);

            //4 cornery
            ftops = rot.Points();

            //konwersja na int
            tops[0].X = Convert.ToInt32(ftops[0].X);
            tops[0].Y = Convert.ToInt32(ftops[0].Y);
            tops[1].X = Convert.ToInt32(ftops[1].X);
            tops[1].Y = Convert.ToInt32(ftops[1].Y);
            tops[2].X = Convert.ToInt32(ftops[2].X);
            tops[2].Y = Convert.ToInt32(ftops[2].Y);
            tops[3].X = Convert.ToInt32(ftops[3].X);
            tops[3].Y = Convert.ToInt32(ftops[3].Y);

            //poprawka na katy
            if (rot.Size.Width < rot.Size.Height)
            {
                ang = rot.Angle + 90;
            }
            else
            {
                ang = rot.Angle + 180;
            }

            //konwersja na inty z float pointa
            int angle = Convert.ToInt32(ang);


            //Rysowanie pomocnicze
            //Cv2.Circle(imageClone, center, 3, new Scalar(0, 255, 0), 1);
            //Cv2.Circle(imageClone, tops[0], 3, new Scalar(255, 0, 0), 2);
            //Cv2.Circle(imageClone, tops[1], 3, new Scalar(255, 0, 0), 2);
            //Cv2.Circle(imageClone, tops[2], 3, new Scalar(255, 0, 0), 2);
            //Cv2.Circle(imageClone, tops[3], 3, new Scalar(255, 0, 0), 2);

            return angle;
        }



        //Real Dimensions

        public static Image GetReferenceInformation(VisionSystemController VisionController)
        {
            //1. pixel permetric
            //reset
            Mat greyImage = Conversion.BitmapToMat(ApplyGreyScale(VisionController));
            Mat baseImageCopy = new Mat();
            VisionController.BaseImage.CopyTo(baseImageCopy);

            RotatedRect rot;
            Matrix<double> abcd;
            OpenCvSharp.Point2f[][] MarkerOut;
            int[] MarkerId = new int[10];
            //find markers on image
            (MarkerOut, MarkerId) = FindArucoMarkers(greyImage);

            OpenCvSharp.Aruco.CvAruco.DrawDetectedMarkers(baseImageCopy, MarkerOut, MarkerId, new Scalar(255, 0, 0));

            string text = "\n>??\n\n " + "X1: " + Convert.ToString(MarkerOut[0][0].X) + "\nY1 = " + Convert.ToString(MarkerOut[0][0].Y) + "\n";
            Cv2.PutText(baseImageCopy, text, (OpenCvSharp.Point)MarkerOut[0][MarkerOut[0].Length],HersheyFonts.HersheyComplex,2,new Scalar(255,255,255));

            text = "\n>??\n\n " + "X2: " + Convert.ToString(MarkerOut[1][0].X) + "\nY2 = " + Convert.ToString(MarkerOut[1][0].Y) + "\n";
            Cv2.PutText(baseImageCopy, text, (OpenCvSharp.Point)MarkerOut[0][MarkerOut[0].Length], HersheyFonts.HersheyComplex, 2, new Scalar(255, 255, 255));

            //TODO
            ////Macierz transforamcji ukladow wsplorzednych
            //(abcd, ppm) = VISION.coordinatesABCD(MarkerOut, MarkerId);

            //richTextBox3.Text += "\n\nppm : " + Convert.ToString(ppm);

            ////wspolrzedne srodka obietu we wspolrzednych robota
            //(singleConts, connectedConts) = VISION.mapCoordinates(singleConts, connectedConts, abcd);

            //for (int i = 0; i < singleConts.Length; i++)
            //{
            //    Cv2.Circle(baseImageCopy, singleConts[i].center, 2, new Scalar(0, 255, 0));
            //    Cv2.PutText(baseImageCopy, Convert.ToString(singleConts[i].realCenter), singleConts[i].center, HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255));
            //}
            //for (int i = 0; i < connectedConts.Length; i++)
            //{
            //    Cv2.Circle(baseImageCopy, connectedConts[i].center, 2, new Scalar(0, 255, 0));
            //    Cv2.PutText(baseImageCopy, Convert.ToString(connectedConts[i].realCenter), connectedConts[i].center, HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255));
            //}

            //wyswietlenie obrazu ze zmianami        
            return Conversion.MatToBitmap(baseImageCopy);
        }

        public static (OpenCvSharp.Point2f[][],int[]) FindArucoMarkers(Mat greyImage)
        {
            OpenCvSharp.Point2f[][] Rejected = new OpenCvSharp.Point2f[10][];
            OpenCvSharp.Point2f[][] MarkerOut;
            int[] MarkerId = new int[10];

            OpenCvSharp.Aruco.DetectorParameters arucoParam = OpenCvSharp.Aruco.DetectorParameters.Create();
            OpenCvSharp.Aruco.Dictionary ArucoDict = OpenCvSharp.Aruco.CvAruco.GetPredefinedDictionary(OpenCvSharp.Aruco.PredefinedDictionaryName.Dict4X4_50);

            arucoParam.AdaptiveThreshConstant = 6;
            arucoParam.AdaptiveThreshWinSizeMin = 3;
            arucoParam.AdaptiveThreshWinSizeMax = 20;
            arucoParam.AdaptiveThreshWinSizeStep = 1;
            arucoParam.PerspectiveRemovePixelPerCell = 10;
            arucoParam.PerspectiveRemoveIgnoredMarginPerCell = 0.2;
            arucoParam.MarkerBorderBits = 1;
            arucoParam.MaxErroneousBitsInBorderRate = 0.35;
            arucoParam.MaxMarkerPerimeterRate = 40.0;
            arucoParam.MinCornerDistanceRate = 0.05;
            arucoParam.MinDistanceToBorder = 3;
            arucoParam.MinMarkerDistanceRate = 0.05;
            arucoParam.MinMarkerPerimeterRate = 0.1;
            arucoParam.MinOtsuStdDev = 5.0;
            arucoParam.PerspectiveRemoveIgnoredMarginPerCell = 0.13;
            arucoParam.PerspectiveRemovePixelPerCell = 8;

            OpenCvSharp.Aruco.CvAruco.DetectMarkers(greyImage, ArucoDict, out MarkerOut, out MarkerId, arucoParam, out Rejected);

            return (MarkerOut, MarkerId);
        }




    }
}
