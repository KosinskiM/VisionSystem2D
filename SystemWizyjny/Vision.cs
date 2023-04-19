using NumSharp;
using OpenCvSharp;
using System;
using System.Drawing;

namespace SystemWizyjny
{
    internal class Vision
    {
        //capture klatki opcja save nie zrobiona !!!!!11
        public static Mat Capture(FrameSource frameSource,bool save)
        {
            // inicjalizacja obiektu mat do przechowania 

            Mat img = new Mat();

            //grab frame to img var
            frameSource.NextFrame(img);

            return img;
        }

        //converter z mat na bitmape 
        public static Bitmap MatToBitmap(Mat image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
        }


        //znalezienie konturow
        public static OpenCvSharp.Point[][] GetContours(Mat image,int LowerThresh, int UpperThresh)
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
        public static (Mat ,int ,int) showContours(Mat imageClone, OpenCvSharp.Point[][] contours, int Slower, int Mid, int Bupper)
        {
            int small=0 ,connected = 0;
            
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
        public static (OpenCvSharp.Point[][], OpenCvSharp.Point[][]) SmallBigContours(OpenCvSharp.Point[][] contours, int Slower, int Mid, int Bupper , int Scount , int Bcount)
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
            return (smallContours , bigContours);
        }


        //sortowanie indeksow od lewej CENTER OF MASS

        public static (OpenCvSharp.Point[][], OpenCvSharp.Point[][]) LeftSortContours(OpenCvSharp.Point[][] smallContours, OpenCvSharp.Point[][] bigContours, Mat imageClone)
        {


            OpenCvSharp.Point2f[] ftops = new OpenCvSharp.Point2f[4];
            OpenCvSharp.Point[] tops = new OpenCvSharp.Point[4];
            OpenCvSharp.Point[] temp;

            //sorotwanie przez wybor

            //pojedyncze
            for(int i= 0; i < smallContours.Length - 1 ; i++)
            {
                int min = i;
                
                for(int j= i+1; j < smallContours.Length; j++)
                {
                    OpenCvSharp.Point center = Vision.GetCenter(smallContours[min]);
                    OpenCvSharp.Point center1 = Vision.GetCenter(smallContours[j]);

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
                    OpenCvSharp.Point center = Vision.GetCenter(bigContours[min]);
                    OpenCvSharp.Point center1 = Vision.GetCenter(bigContours[j]);

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
        public static string ContourColor(Mat imageClone , OpenCvSharp.Point[] Contour , int low_low_red, int low_upper_red , int upper_low_red , int upper_upper_red , int lower_green , int upper_green , int lower_blue , int upper_blue)
        {

            Vec3b vector;
            OpenCvSharp.Point center;
            string result;
            Mat hsv = new Mat();

            //zamiana na hsv
            Cv2.CvtColor(imageClone, hsv, ColorConversionCodes.BGR2HSV);

            //momenty
            var M = Cv2.Moments(Contour, false);
            center.X = Convert.ToInt32(M.M10 / M.M00);
            center.Y = Convert.ToInt32(M.M01 / M.M00);

            vector = hsv.At<Vec3b>(center.Y, center.X);
            Scalar color = new Scalar(vector.Item0, vector.Item1, vector.Item2);


            //red
            if ((color.Val0 >= low_low_red && color.Val0 <= low_upper_red) || (color.Val0 >= upper_low_red && color.Val0 <= upper_upper_red))
            {
                result = "red";
            }
            //green
            else if (color.Val0 >= lower_green && color.Val0 <= upper_green)
            {
                result = "green";
            }
            //blue
            else if(color.Val0 > lower_blue && color.Val0 <= upper_blue)
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

        //center
        public static OpenCvSharp.Point GetCenter(OpenCvSharp.Point[] contour)
        {
            OpenCvSharp.Point center;
            var M = Cv2.Moments(contour, false);
            center.X = Convert.ToInt32(M.M10 / M.M00);
            center.Y = Convert.ToInt32(M.M01 / M.M00);

            return center;
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


        //angle of rotation
        public static int GetAngle(Mat imageClone , OpenCvSharp.Point[] contour)
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
            //konwersja na inty z float pointa
            int angle = Convert.ToInt32(ang);

            Cv2.Circle(imageClone, tops[0], 3, new Scalar(255, 0, 0), 2);
            Cv2.Circle(imageClone, tops[1], 3, new Scalar(255, 0, 0), 2);
            Cv2.Circle(imageClone, tops[2], 3, new Scalar(255, 0, 0), 2);
            Cv2.Circle(imageClone, tops[3], 3, new Scalar(255, 0, 0), 2);

            return angle;
        }





        //tool points UP and Down ------------------------------------------ MASKA NARZEDZIA 
        public static (OpenCvSharp.Point[][], OpenCvSharp.Point[][], Mat) ToolPoints(Mat imageClone,ContourOb[] singleConts ,double ppm)
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

                //kolejny sposob DZIAŁAAAA Dodatnie kolejnego punktu
                double AB = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) / ppm;

                //odleglosc zalezna od pixelpermetrics
                double AC = AB + 10;

                OpenCvSharp.Point three;
                three.X = Convert.ToInt32(x1 + (AC * (x2 - x1) / AB));
                three.Y = Convert.ToInt32(y1 + (AC * (y2 - y1) / AB));

                x1 = singleConts[i].corners[3].X;
                x2 = singleConts[i].corners[2].X;
                y1 = singleConts[i].corners[3].Y;
                y2 = singleConts[i].corners[2].Y;


                //kolejny sposob DZIAŁAAAA Dodatnie kolejnego punktu
                AB = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) / ppm;

                //odleglosc zalezna od pixelpermetrics
                AC = AB + 10;

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

                toolUpContours[i] = toolUp;


                //LOWER tool -----------------------------------------------------------------------------------------------------------

                x1 = singleConts[i].corners[1].X;
                x2 = singleConts[i].corners[0].X;
                y1 = singleConts[i].corners[1].Y;
                y2 = singleConts[i].corners[0].Y;

                //kolejny sposob DZIAŁAAAA Dodatnie kolejnego punktu
                AB = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)) / ppm;

                //odleglosc zalezna od pixelpermetrics
                AC = AB + 10;


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
                AC = AB + 10;


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






        public static int[][] GetSingleCollisions(ContourOb[] singleConts , OpenCvSharp.Point[][] toolUpSingleContours ,OpenCvSharp.Point[][] toolDownSingleContours)
        {
            int[][] collisons = new int[singleConts.Length][];
            //PETLA ilosc konturow
            for (int i = 0; i < singleConts.Length; i++)
            {
                int[] with = new int[30];
                int cnt = 0;
                
                //PETLA ILOSC punktow konturu
                for (int j = 0; j < singleConts[i].contour.Length; j++)
                {

                    //punkty konturu
                    OpenCvSharp.Point2f temp;
                    temp.X = singleConts[i].contour[j].X;
                    temp.Y = singleConts[i].contour[j].Y;

                    //PETLA czy punkt znajduje sie w obszarze toolUP
                    for (int u = 0; u < toolUpSingleContours.Length; u++)
                    { 
                        if (i != u)
                        {
                            double infoUp = Cv2.PointPolygonTest(toolUpSingleContours[u], temp, false);

                            if (cnt > 0)
                            {
                                //Jezeli 1 albo 0 to !
                                if ((infoUp == 1 || infoUp == 0) && with[cnt - 1] != u)
                                {
                                    with[cnt] = u;
                                    cnt++;
                                }
                            }
                            else
                            {
                                if ((infoUp == 1 || infoUp == 0))
                                {
                                    with[cnt] = u;
                                    cnt++;
                                }
                            }
                        }
                    }


                    //PETLA czy punkt znajduje sie w obszarze toolDOWN
                    for (int d = 0; d < toolDownSingleContours.Length; d++)
                    {
                        if (i != d)
                        {
                            double infoDown = Cv2.PointPolygonTest(toolDownSingleContours[d], temp, false);

                            if (cnt > 0)
                            {
                                //Jezeli 1 albo 0 to !
                                if ((infoDown == 1 || infoDown == 0) && with[cnt - 1] != d )
                                {
                                    with[cnt] = d;
                                    cnt++;
                                }
                            }
                            else
                            {
                                if ((infoDown == 1 || infoDown == 0))
                                {
                                    with[cnt] = d;
                                    cnt++;
                                }
                            }

                        }
                    }

                    int counter = 0;
                    for(int w =0; w< with.Length; w++)
                    {
                        if(with[w] != 0)
                        {
                            counter++;
                        }
                    }
                    int[] sorted = new int[counter];

                    counter = 0;
                    for (int w = 0; w < with.Length; w++)
                    {
                        if (with[w] != 0)
                        {
                            sorted[counter] = with[w];
                            counter++;
                        }
                    }

                    collisons[i] = sorted;
                }
            }
            return collisons;
        }

        /*
        public static int[][] GSCollisions(ContourOb[] singleConts, OpenCvSharp.Point[][] toolUpSingleContours, OpenCvSharp.Point[][] toolDownSingleContours)
        {
            int[][] collisons = new int[singleConts.Length][];



            //PETLA czy punkt znajduje sie w obszarze toolUPa
            for (int u = 0; u < singleConts.Length; u++)
            {
                //petla ilosc konturow
                for (int i = 0; i < singleConts.Length; i++)
                {

                    //PETLA ILOSC punktow konturu
                    for (int j = 0; j < singleConts[i].contour.Length; j++)
                    {

                        //punkty konturu
                        OpenCvSharp.Point2f temp;
                        temp.X = singleConts[i].contour[j].X;
                        temp.Y = singleConts[i].contour[j].Y;

                        if (i != u)
                        {
                            double infoUp = Cv2.PointPolygonTest(toolUpSingleContours[u], temp, false);
                            //Jezeli 1 albo 0 to !
                            if ((infoUp == 1 || infoUp == 0) && detected != 1)
                            {
                                with[cnt] = u;
                                cnt++;
                                detected = 1;
                            }

                        }

                    }

                }
            }

            //PETLA czy punkt znajduje sie w obszarze toolDOWNa
            for (int d = 0; d < toolDownSingleContours.Length; d++)
            {
                //petla ilosc konturow
                for (int i = 0; i < singleConts.Length; i++)
                {

                    if (i != d)
                    {
                        double infoDown = Cv2.PointPolygonTest(toolDownSingleContours[d], temp, false);

                        //Jezeli 1 albo 0 to !
                        if ((infoDown == 1 || infoDown == 0) && detected != 1)
                        {
                            with[cnt] = d;
                            cnt++;
                            detected = 1;
                        }
                    }

                }
            }











                //PETLA ilosc konturow
                for (int i = 0; i < singleConts.Length; i++)
            {
                int[] with = new int[singleConts.Length];
                int cnt = 0;
                int detected = 0;

                //PETLA ILOSC punktow konturu
                for (int j = 0; j < singleConts[i].contour.Length; j++)
                {
                    //punkty konturu
                    OpenCvSharp.Point2f temp;
                    temp.X = singleConts[i].contour[j].X;
                    temp.Y = singleConts[i].contour[j].Y;

                    //PETLA czy punkt znajduje sie w obszarze toolUP
                    for (int u = 0; u < toolUpSingleContours.Length; u++)
                    {

                        if (i != u)
                        {
                            double infoUp = Cv2.PointPolygonTest(toolUpSingleContours[u], temp, false);
                            //Jezeli 1 albo 0 to !
                            if ((infoUp == 1 || infoUp == 0) && detected != 1)
                            {
                                with[cnt] = u;
                                cnt++;
                                detected = 1;
                            }

                        }

                    }


                    //PETLA czy punkt znajduje sie w obszarze toolDOWN
                    for (int d = 0; d < toolDownSingleContours.Length; d++)
                    {

                        if (i != d)
                        {
                            double infoDown = Cv2.PointPolygonTest(toolDownSingleContours[d], temp, false);

                            //Jezeli 1 albo 0 to !
                            if ((infoDown == 1 || infoDown == 0) && detected != 1)
                            {
                                with[cnt] = d;
                                cnt++;
                                detected = 1;
                            }
                        }
                    }


                    collisons[i] = with;


                }


            }

            return collisons;
        }
        */


    }
}















/*
 *    // nie dzislaaaaaaaa
            //sortowanie indeksow od lewej WLASNA OPCJa
            public static (OpenCvSharp.Point[][], OpenCvSharp.Point[][]) LSortContours(OpenCvSharp.Point[][] smallContours, OpenCvSharp.Point[][] bigContours)
        {
            //small
            float[] Ssum = new float[smallContours.Length];
            float[] Sdiff = new float[smallContours.Length];

            //big
            float[] Bsum = new float[bigContours.Length];
            float[] Bdiff = new float[bigContours.Length];



            //wierzcholki
            int[,] SmallPeaks = new int[smallContours.Length, 4];
            int[,] BigPeaks = new int[smallContours.Length, 4];
            
            //sorted conturs
            OpenCvSharp.Point[][] SortedSmallContours = new OpenCvSharp.Point[smallContours.Length][];
            OpenCvSharp.Point[][] SortedBigContours = new OpenCvSharp.Point[bigContours.Length][];


            //center point
            OpenCvSharp.Point[] temp;

            //male kontury
            for (int j = 0; j < smallContours.Length; j++)
            {
                for (int i = 0; i < smallContours[j].Length; i++)
                {
                    Ssum[i] = smallContours[j][i].X + smallContours[j][i].Y;
                    Sdiff[i] = smallContours[j][i].X - smallContours[j][i].Y;
                }

                //wierzcholki
                SmallPeaks[j, 0] = np.argmin(Ssum);   //topleft
                SmallPeaks[j, 1] = np.argmax(Ssum);   //bottom right
                SmallPeaks[j, 2] = np.argmin(Sdiff);    //top right
                SmallPeaks[j, 3] = np.argmax(Sdiff);      //bottom left

            }

            for (int i = 0; i < smallContours.Length; i++)
            {
                for (int j = 0; j < smallContours.Length - 1; j++)
                {
                    if (smallContours[j][SmallPeaks[j, 0]].X > smallContours[j + 1][SmallPeaks[j, 0]].X)
                    {
                        temp = smallContours[j];
                        smallContours[j] = smallContours[j + 1];
                        smallContours[j + 1] = temp;
                    }
                }
            }









            return (smallContours, bigContours);
        }
 * 
 * 
 */



/*
 * //richTextBox1.Text += "\n nr cont: " + Convert.ToString(i) + "nr pkt: " + Convert.ToString(j) + "info: " + Convert.ToString(info);



                    //PETLA czy punkt znajduje sie w obszarze toolUP
                    for (int u = 0; u< toolUpSingleContours.Length;u++)
                    {
                        if (i != u)
                        {
                            double infoUp = Cv2.PointPolygonTest(toolUpSingleContours[u], temp, false);

                            //Jezeli 1 albo 0 to !
                            if (infoUp == 1 || infoUp == 0)
                            {
                                richTextBox1.Text += "\n";
                                richTextBox1.Text += "\n nr cont:"  + Convert.ToString(i) + " " + "nr pkt: " + Convert.ToString(j) + " " + "nr toolUpa:" + Convert.ToString(u) + " " + "info: " + Convert.ToString(infoUp);

                            }
                        }
                        
                    }



                    //PETLA czy punkt znajduje sie w obszarze toolDOWN

                    for (int d = 0; d < toolUpSingleContours.Length; d++)
                    {
                        if (i != d)
                        {
                            double infoDown = Cv2.PointPolygonTest(toolDownSingleContours[d], temp, false);


                            //Jezeli 1 albo 0 to !
                            if (infoDown == 1 || infoDown == 0)
                            {
                                richTextBox1.Text += "\n";
                                richTextBox1.Text += "\n nr cont: " + Convert.ToString(i) + " " + " nr pkt: " + Convert.ToString(j) + " " + " nr toolDowna: " + Convert.ToString(d) + " " + " info: " + Convert.ToString(infoDown);
                            }
                        }
                    }
*/

/*
 * 
            /*
            for (int i = 0; i < singleConts.Length; i++)
            {
                Cv2.Circle(imageClone, singleConts[i].corners[0], 1, new Scalar(255, 0, 0), 1);
                Cv2.PutText(imageClone, "0", singleConts[i].corners[0], HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255), 1);

                Cv2.Circle(imageClone, singleConts[i].corners[1], 1, new Scalar(255, 0, 0), 1);
                Cv2.PutText(imageClone, "1", singleConts[i].corners[1], HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255), 1);

                Cv2.Circle(imageClone, singleConts[i].corners[2], 1, new Scalar(255, 0, 0), 1);
                Cv2.PutText(imageClone, "2", singleConts[i].corners[2], HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255), 1);

                Cv2.Circle(imageClone, singleConts[i].corners[3], 1, new Scalar(255, 0, 0), 1);
                Cv2.PutText(imageClone, "3", singleConts[i].corners[3], HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255), 1);
            }

            */

