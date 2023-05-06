using NumSharp;
using OpenCvSharp;
using System;
using System.Drawing;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Clear_version_robotApp
{
    internal class VISION
    {

        //converter z mat na bitmape 
        public static Bitmap MatToBitmap(Mat image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
        }

        

















        //znalezienie konturow
        public static OpenCvSharp.Point[][] GetContours(Mat image, int LowerThresh, int UpperThresh)
        {
            Mat imageClone = image.Clone();
            Mat refGray = new Mat();
            Mat thresh = new Mat();
            Mat gaus = new Mat();

            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hIndx;

            //maska
            var kernel = new OpenCvSharp.Size(3, 3);
            //konwersja na odcienie szarosci
            Cv2.CvtColor(imageClone, refGray, ColorConversionCodes.BGR2GRAY);
            //Cv2.GaussianBlur(refGray, gaus, kernel,0,0);
            Cv2.MorphologyEx(refGray, gaus, MorphTypes.Close, null);
            // piksel wiekszy niz thresh 255
            Cv2.Threshold(refGray, thresh, LowerThresh, UpperThresh, ThresholdTypes.BinaryInv);
            Cv2.FindContours(thresh, out contours, out hIndx, RetrievalModes.List, ContourApproximationModes.ApproxNone);

            return contours;
        }



        //wyswitlanie konturow
        public static (Mat, int, int) showContours(Mat imageClone, OpenCvSharp.Point[][] contours, int Slower, int Mid, int Bupper)
        {
            int small = 0, connected = 0;

            for (int i = 0; i < contours.Length; i++)
            {
                if (Cv2.ContourArea(contours[i]) > Slower && Cv2.ContourArea(contours[i]) <= Mid)
                {
                    Cv2.DrawContours(imageClone, contours, i, new Scalar(0, 255, 0), thickness: 4);
                    small++;
                }
                else if (Cv2.ContourArea(contours[i]) > Mid && Cv2.ContourArea(contours[i]) <= Bupper)
                {
                    Cv2.DrawContours(imageClone, contours, i, new Scalar(255, 0, 0), thickness: 4);
                    connected++;
                }

            }

            //return obraz z narysowanymi konturami + liczbe malych i zloczonych
            return (imageClone, small, connected);
        }



        //indeksowanie konturow osobnych i polaczonych
        public static (OpenCvSharp.Point[][], OpenCvSharp.Point[][]) SmallBigContours(OpenCvSharp.Point[][] contours, int Slower, int Mid, int Bupper, int Scount, int Bcount)
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












        //center
        public static OpenCvSharp.Point GetCenter(OpenCvSharp.Point[] contour)
        {
            OpenCvSharp.Point center;
            var M = Cv2.Moments(contour, false);
            center.X = Convert.ToInt32(M.M10 / M.M00);
            center.Y = Convert.ToInt32(M.M01 / M.M00);

            return center;
        }

        //sortowanie indeksow od lewej CENTER OF MASS
        public static (OpenCvSharp.Point[][], OpenCvSharp.Point[][]) LeftSortContours(OpenCvSharp.Point[][] smallContours, OpenCvSharp.Point[][] bigContours, Mat imageClone)
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
                    OpenCvSharp.Point center = VISION.GetCenter(smallContours[min]);
                    OpenCvSharp.Point center1 = VISION.GetCenter(smallContours[j]);

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
                    OpenCvSharp.Point center = VISION.GetCenter(bigContours[min]);
                    OpenCvSharp.Point center1 = VISION.GetCenter(bigContours[j]);

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







        //rozpoznawanie kolorow
        public static string ContourColor(Mat imageClone, OpenCvSharp.Point[] Contour, int low_low_red, int low_upper_red, int upper_low_red, int upper_upper_red, int lower_green, int upper_green, int lower_blue, int upper_blue)
        {

            Vec3b vector;
            OpenCvSharp.Point center;
            string result;
            Mat hsv = new Mat();

            //zamiana na hsv
            Cv2.CvtColor(imageClone, hsv, ColorConversionCodes.BGR2HSV);
            Cv2.Split(hsv, out Mat[] mv);
            mv[2] = mv[2] * 3;
            Cv2.Merge(mv, hsv);
            

            //momenty
            var M = Cv2.Moments(Contour, false);
            center.X = Convert.ToInt32(M.M10 / M.M00);
            center.Y = Convert.ToInt32(M.M01 / M.M00);

            vector = hsv.At<Vec3b>(center.Y, center.X);
            Scalar color = new Scalar(vector.Item0, vector.Item1, vector.Item2);


            //red
            if ((color.Val1 >= low_low_red && color.Val1 <= low_upper_red) ||
              (color.Val1 >= upper_low_red && color.Val1 <= upper_upper_red))
            {
                result = "red";
            }
            //green
            else if (color.Val1 >= lower_green && color.Val1 <= upper_green)
            {
                result = "green";
            }
            //blue
            else if (color.Val1 > lower_blue && color.Val1 <= upper_blue)
            {
                result = "blue";
            }
            else
            {
                result = "none";
            }

            return result;
        }







        //rozpoznawanie ksztaltow
        public static string GetShape(OpenCvSharp.Point[] contour)
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








        //angle of rotation
        public static int GetAngle(Mat imageClone, OpenCvSharp.Point[] contour)
        {
            //zmienne
            float ang;
            OpenCvSharp.Point center;
            OpenCvSharp.Point2f[] ftops;
            OpenCvSharp.Point[] tops = new OpenCvSharp.Point[4];

            //min area rectangle with angle
            RotatedRect rot = Cv2.MinAreaRect(contour);

            //4 cornery
            ftops = rot.Points();

            //srodek z momentow
            var M = Cv2.Moments(contour, false);
            center.X = Convert.ToInt32(M.M10 / M.M00);
            center.Y = Convert.ToInt32(M.M01 / M.M00);


            //srodek konturu
            Cv2.Circle(imageClone, center, 3, new Scalar(0, 255, 0), 1);

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
            


            /*
            //dostosowanie kata obrotu do obrotu chwytaka
            if(ang > 90)
            {
                ang = ang - 180;
            }
            */



            //konwersja na inty z float pointa
            int angle = Convert.ToInt32(ang);

            Cv2.Circle(imageClone, tops[0], 3, new Scalar(255, 0, 0), 2);
            Cv2.Circle(imageClone, tops[1], 3, new Scalar(255, 0, 0), 2);
            Cv2.Circle(imageClone, tops[2], 3, new Scalar(255, 0, 0), 2);
            Cv2.Circle(imageClone, tops[3], 3, new Scalar(255, 0, 0), 2);

            return angle;
        }












        //corners
        public static OpenCvSharp.Point[] GetCorners(OpenCvSharp.Point[] contour)
        {
            OpenCvSharp.Point2f[] ftops;
            OpenCvSharp.Point[] tops = new OpenCvSharp.Point[4];

            //min area rectangle with angle
            RotatedRect rot = Cv2.MinAreaRect(contour);

            //4 cornery
            ftops = rot.Points();

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

            return tops;
        }




        //find Aruco Markers
        public static (OpenCvSharp.Point2f[][], int[]) findArucoMarkers(Mat wejthresh, int lowerThresh, int upperThresh)
        {

            OpenCvSharp.Point2f[][] Rejected = new OpenCvSharp.Point2f[10][];
            OpenCvSharp.Point2f[][] MarkerOut;
            int[] MarkerId = new int[10];

            Mat refGray = new Mat();
            Mat gaus = new Mat();
            Mat thresh = new Mat();
            //konwersja na odcienie szarosci
            Cv2.CvtColor(wejthresh, refGray, ColorConversionCodes.BGR2GRAY);
            //blur 
            OpenCvSharp.Size kernel = new OpenCvSharp.Size(3, 3);
            //Cv2.GaussianBlur(refGray, gaus, kernel,0,0);
            Cv2.MorphologyEx(refGray, gaus, MorphTypes.Close, null);
            //tresh 127 - 255
            Cv2.Threshold(gaus, thresh, lowerThresh, upperThresh, ThresholdTypes.BinaryInv);

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

            OpenCvSharp.Aruco.CvAruco.DetectMarkers(wejthresh, ArucoDict, out MarkerOut, out MarkerId, arucoParam, out Rejected);

            return (MarkerOut, MarkerId);
        }


        public static (Matrix<double>,double) coordinatesABCD(OpenCvSharp.Point2f[][] MarkerOut, int[] MarkerId)
        {

            //pixels per metric
            //20 na 20 aruco tag
            RotatedRect rot = Cv2.MinAreaRect(MarkerOut[0]);

            //punkt kalibracji
            OpenCvSharp.Point odniesienie1 = new OpenCvSharp.Point();
            OpenCvSharp.Point odniesienie2 = new OpenCvSharp.Point();
            odniesienie1.X = Convert.ToInt32(MarkerOut[0][0].X);
            odniesienie1.Y = Convert.ToInt32(MarkerOut[0][0].Y);
            odniesienie2.X = Convert.ToInt32(MarkerOut[1][0].X);
            odniesienie2.Y = Convert.ToInt32(MarkerOut[1][0].Y);

            double width = 20;
            double obwidth = Convert.ToDouble(rot.Size.Height);
            double obheight = Convert.ToDouble(rot.Size.Width);

            double ppm = obwidth / width;
            double dimA = obwidth / ppm;
            double dimB = obheight / ppm;

            //wspolczynniki abcd ze strony how to map points between 2d coordinate systems bez minusów
            //zamienione x i y, obrocony uklad wspolrzednych


            // +367.08,-0.09,+209.58,+87.56,+176.32,+603.28,+0.00,R,A,O
            Matrix<double> u = DenseMatrix.OfArray(new double[,]{
                {608.88},       //x1
                {1135.15},      //add1
                {367.08},       //x2
                {603.28}        //add2
            });

            Matrix<double> M = DenseMatrix.OfArray(new double[,]{
                 { odniesienie2.X,odniesienie2.Y, 1, 0 },
                 { -odniesienie2.X, odniesienie2.X, 0, 1},
                 { odniesienie1.X,odniesienie1.Y, 1, 0 },
                 { -odniesienie1.Y, odniesienie1.X, 0, 1}
            });

            var Mt = M.Inverse();
            Matrix<double> abcd = Mt.Multiply(u);

            //mapowanie
            //x'=ax+by+c
            //y'=bx-ay+d
            return (abcd,ppm);
        }



        //mapowanie wspolrzednych z kamery do przestrzeni robota
        public static (ContourOb[],ContourOb[]) mapCoordinates(ContourOb[] singleConts, ContourOb[] connectedConts,Matrix<double> abcd)
        {
            //mapowanie
            //x'=ax+by+c
            //y'=bx-ay+d

            double x = 0, y = 0;
            for(int i = 0; i < singleConts.Length; i++)
            {
                x = singleConts[i].center.X;
                y = singleConts[i].center.Y;
                singleConts[i].realCenter.Y = (abcd[0, 0] * x + abcd[1, 0] * y + abcd[2, 0]); //-1
                singleConts[i].realCenter.X = abcd[1, 0] * x - abcd[0, 0] * y + abcd[3, 0];
            }

            for (int i = 0; i < connectedConts.Length; i++)
            {
                x = connectedConts[i].center.X;
                y = connectedConts[i].center.Y;
                connectedConts[i].realCenter.Y = (abcd[0, 0] * x + abcd[1, 0] * y + abcd[2, 0]); //-1
                connectedConts[i].realCenter.X = abcd[1, 0] * x - abcd[0, 0] * y + abcd[3, 0];
            }

            return (singleConts,connectedConts);
        }



        //tool points UP and Down ------------------------------------------ MASKA NARZEDZIA 
        public static (OpenCvSharp.Point[][], OpenCvSharp.Point[][], Mat) ToolPoints(Mat imageClone, ContourOb[] singleConts, double ppm)
        {

            int x1, x2, y1, y2;



            OpenCvSharp.Point[][] toolUpContours = new OpenCvSharp.Point[singleConts.Length][];
            OpenCvSharp.Point[][] toolDownContours = new OpenCvSharp.Point[singleConts.Length][];

            for (int i = 0; i < singleConts.Length; i++)
            {
                OpenCvSharp.Point[] toolUp = new OpenCvSharp.Point[4];
                OpenCvSharp.Point[] toolDown = new OpenCvSharp.Point[4];


                //UPPER TOOOL -------------------------------------------------------------------------------------------------------

                x1 = singleConts[i].corners[0].X;
                x2 = singleConts[i].corners[1].X;
                y1 = singleConts[i].corners[0].Y;
                y2 = singleConts[i].corners[1].Y;



                //kolejny sposob DZIAŁAAAA Dodanie kolejnego punktu
                double AB = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2))/ ppm;

                //odleglosc zalezna od pixelpermetrics
                double AC = AB + 20;

                OpenCvSharp.Point three;
                three.X = Convert.ToInt32(x1 + (AC * (x2 - x1) / AB));
                three.Y = Convert.ToInt32(y1 + (AC * (y2 - y1) / AB));
                

                x1 = singleConts[i].corners[3].X;
                x2 = singleConts[i].corners[2].X;
                y1 = singleConts[i].corners[3].Y;
                y2 = singleConts[i].corners[2].Y;

                //kolejny sposob DZIAŁAAAA Dodatnie kolejnego punktu
                AB = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2))/ ppm;

                //odleglosc zalezna od pixelpermetrics
                AC = AB + 20;

                OpenCvSharp.Point fourth;
                fourth.X = Convert.ToInt32(x1 + (AC * (x2 - x1) / AB));
                fourth.Y = Convert.ToInt32(y1 + (AC * (y2 - y1) / AB));

                //rysowanie na zdjeciu   \/

                Cv2.Circle(imageClone, singleConts[i].corners[1], 1, new Scalar(255, 0, 0), 3);
                Cv2.Circle(imageClone, singleConts[i].corners[2], 1, new Scalar(255, 0, 0), 3);

                Cv2.Circle(imageClone, three, 1, new Scalar(0, 0, 255), 3);
                Cv2.Circle(imageClone, fourth, 1, new Scalar(0, 0, 255), 3);

                toolUp[0] = singleConts[i].corners[2];
                toolUp[1] = fourth;
                toolUp[2] = three;
                toolUp[3] = singleConts[i].corners[1];



                /*
                //----------------------nowe

                x1 = toolUp[0].X;
                x2 = toolUp[1].X;
                y1 = toolUp[0].Y;
                y2 = toolUp[1].Y;


                //kolejny sposob DZIAŁAAAA Dodanie kolejnego punktu
                AB = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) / ppm;

                //odleglosc zalezna od pixelpermetrics
                AC = AB + 20;

                three.X = Convert.ToInt32(x1 + (AC * (x2 - x1) / AB)) + 10;
                three.Y = Convert.ToInt32(y1 + (AC * (y2 - y1) / AB)) + 10;

                x1 = toolUp[3].X;
                x2 = toolDown[2].X;
                y1 = toolUp[3].Y;
                y2 = toolDown[2].Y;

                //kolejny sposob DZIAŁAAAA Dodatnie kolejnego punktu
                AB = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) / ppm;

                //odleglosc zalezna od pixelpermetrics
                AC = AB + 20;

                fourth.X = Convert.ToInt32(x1 + (AC * (x2 - x1) / AB)) + 10;
                fourth.Y = Convert.ToInt32(y1 + (AC * (y2 - y1) / AB)) + 10;

                //rysowanie na zdjeciu   \/

                Cv2.Circle(imageClone, singleConts[i].corners[1], 1, new Scalar(255, 0, 0), 3);
                Cv2.Circle(imageClone, singleConts[i].corners[2], 1, new Scalar(255, 0, 0), 3);

                Cv2.Circle(imageClone, three, 1, new Scalar(0, 0, 255), 3);
                Cv2.Circle(imageClone, fourth, 1, new Scalar(0, 0, 255), 3);

                toolUp[0] = singleConts[i].corners[2];
                toolUp[1] = fourth;
                toolUp[2] = three;
                toolUp[3] = singleConts[i].corners[1];

                //----------------------koniec nowe

                */








                toolUpContours[i] = toolUp;























                //LOWER tool -----------------------------------------------------------------------------------------------------------

                x1 = singleConts[i].corners[1].X;
                x2 = singleConts[i].corners[0].X;
                y1 = singleConts[i].corners[1].Y;
                y2 = singleConts[i].corners[0].Y;

                //kolejny sposob DZIAŁAAAA Dodatnie kolejnego punktu
                AB = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) / ppm;

                //odleglosc zalezna od pixelpermetrics
                AC = AB + 20;


                //third point
                three.X = Convert.ToInt32(x1 + (AC * (x2 - x1) / AB));
                three.Y = Convert.ToInt32(y1 + (AC * (y2 - y1) / AB));

                x1 = singleConts[i].corners[2].X;
                x2 = singleConts[i].corners[3].X;
                y1 = singleConts[i].corners[2].Y;
                y2 = singleConts[i].corners[3].Y;


                //kolejny sposob DZIAŁAAAA Dodatnie kolejnego punktu
                AB = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) / ppm;

                //odleglosc zalezna od pixelpermetrics
                AC = AB + 20;


                //fourth point of rectangle
                fourth.X = Convert.ToInt32(x1 + (AC * (x2 - x1) / AB));
                fourth.Y = Convert.ToInt32(y1 + (AC * (y2 - y1) / AB));

                //rysowanie na zdjeciu   \/

                Cv2.Circle(imageClone, singleConts[i].corners[1], 1, new Scalar(255, 0, 0), 1);
                Cv2.Circle(imageClone, singleConts[i].corners[2], 1, new Scalar(255, 0, 0), 1);

                Cv2.Circle(imageClone, three, 1, new Scalar(0, 0, 255), 3);
                Cv2.Circle(imageClone, fourth, 1, new Scalar(0, 0, 255), 3);


                toolDown[0] = singleConts[i].corners[3];
                toolDown[1] = fourth;
                toolDown[2] = three;
                toolDown[3] = singleConts[i].corners[0];

                toolDownContours[i] = toolDown;



                //Upper tool 
                Cv2.Line(imageClone, toolUp[0], toolUp[1], new Scalar(255, 0, 0), 1);
                Cv2.Line(imageClone, toolUp[1], toolUp[2], new Scalar(255, 0, 0), 1);
                Cv2.Line(imageClone, toolUp[2], toolUp[3], new Scalar(255, 0, 0), 1);
                Cv2.Line(imageClone, toolUp[3], toolUp[0], new Scalar(255, 0, 0), 1);


                //Lower tool
                Cv2.Line(imageClone, toolDown[0], toolDown[1], new Scalar(255, 0, 0), 1);
                Cv2.Line(imageClone, toolDown[1], toolDown[2], new Scalar(255, 0, 0), 1);
                Cv2.Line(imageClone, toolDown[2], toolDown[3], new Scalar(255, 0, 0), 1);
                Cv2.Line(imageClone, toolDown[3], toolDown[0], new Scalar(255, 0, 0), 1);
            }

            return (toolUpContours, toolDownContours, imageClone);
        }


        public static int[][] GetSingleCollisions(ContourOb[] singleConts, OpenCvSharp.Point[][] toolUpSingleContours, OpenCvSharp.Point[][] toolDownSingleContours)
        {
            //zbior kolizji
            int[][] kolizje = new int[singleConts.Length][];



            //PETLA ilosc konturow
            for (int i = 0; i < singleConts.Length; i++)
            {
                //kolizje
                int[] collWith = new int[10];

                for(int s = 0; s < collWith.Length;s++)
                {
                    collWith[s] = 1000;
                }

                int cnt = 0;
                //------------------------------------------------------------------------ TOOLUP
                //PETLA sprawdzanie kazdego z punktow tool upa
                for (int j = 0; j < toolUpSingleContours[i].Length; j++)
                {

                    //punkt toolupa
                    OpenCvSharp.Point2f temp;
                    temp.X = toolUpSingleContours[i][j].X;
                    temp.Y = toolUpSingleContours[i][j].Y;


                    //PETLA sprawdzenie z punktami innych konturow
                    for (int k = 0; k < singleConts.Length; k++)
                    {
                        for (int l = 0; l < singleConts[k].contour.Length; l++)
                        {
                            //sprawdzenie 
                            double infoUp = Cv2.PointPolygonTest(singleConts[k].contour, temp, false);

                            if(infoUp == 1)
                            {
                                collWith[cnt] = k;      //index outside the bound of the array
                                cnt++;
                                break;
                            }
                        }
                    }
                }

                //------------------------------------------------------------------------ TOOLDOWN
                //PETLA sprawdzanie kazdego z punktow tool downa
                for (int j = 0; j < toolDownSingleContours[i].Length; j++)
                {
                    //punkt tooldowna
                    OpenCvSharp.Point2f temp;
                    temp.X = toolDownSingleContours[i][j].X;
                    temp.Y = toolDownSingleContours[i][j].Y;

                    //PETLA sprawdzenie z punktami innych konturow
                    for (int k = 0; k < singleConts.Length; k++)
                    {
                        for (int l = 0; l < singleConts[k].contour.Length; l++)
                        {
                            //sprawdzenie 
                            double infoDown = Cv2.PointPolygonTest(singleConts[k].contour, temp, false);

                            if (infoDown == 1)
                            {
                                collWith[cnt] = k;
                                cnt++;
                                break;
                            }
                        }
                    }

                }

                if(collWith.Length > 0)
                {
                    kolizje[i] = collWith;
                }
                else
                {
                    kolizje[i][0] = 1000;
                }

                

            }
            
            return kolizje;
        }

    }

}
