using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using NumSharp;

namespace npCV
{
    public partial class contury : Form
    {

        Mat wej = new Mat();
        Mat raw = new Mat();

        private FrameSource frameSource;


        public contury()
        {
            InitializeComponent();


           
        }

        //converter z mat na bitmape 
        public static Bitmap MatToBitmap(Mat image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
        } 
        


        //all contours method
        static OpenCvSharp.Point[][] AllCont(Mat img)
        {

            Mat refGray = new Mat();
            Mat thresh = new Mat();

            //konwersja na odcienie szarosci
            Cv2.CvtColor(img, refGray, ColorConversionCodes.BGR2GRAY);

            //działanie thresholdu :
            // piksel o wartosci mniejszej niz thresh ustawiany jest na 0,
            // piksel wiekszy niz thresh 255


            //tresh 127
            Cv2.Threshold(refGray, thresh, 127, 255, ThresholdTypes.BinaryInv);

            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hIndx;
            Cv2.FindContours(thresh, out contours, out hIndx, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

            return contours;
        }



        //capture function !

        private Mat Capture(bool save)
        {
            // inicjalizacja obiektu mat do przechowania 

            Mat img = new Mat();

            //grab frame to img var
            frameSource.NextFrame(img);

            return img;
        }


        private void button1_Click_1(object sender, EventArgs e)
        {

            wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\ag3.jpg", ImreadModes.Unchanged);

            //frameSource = Cv2.CreateFrameSource_Camera(0);
            //wej = Capture(false);


            
            raw = wej;



            Bitmap bitimg = MatToBitmap(wej);

            pictureBox1.Image = bitimg;

            

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            Mat refGray = new Mat();
            Mat thresh = new Mat();
            Mat imageClone = wej.Clone();

            var kernel = new OpenCvSharp.Size(7, 7);

            //konwersja na odcienie szarosci
            Cv2.CvtColor(wej, refGray, ColorConversionCodes.BGR2GRAY);

            // piksel wiekszy niz thresh 255
            Cv2.Threshold(refGray, thresh, 100, 255, ThresholdTypes.BinaryInv);

            Bitmap bitimg = MatToBitmap(thresh);

            pictureBox1.Image = bitimg;

            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hIndx;
            Cv2.FindContours(thresh, out contours, out hIndx, RetrievalModes.List , ContourApproximationModes.ApproxSimple);

            //wyswietlanie kontourow
            //Cv2.DrawContours(imageClone, contours, -1, new Scalar(0, 0, 255), thickness: 3);



            //---------------------------------------------------------------segregacja konturow--------------------------------------------

            //zmienne do przechowania

            int[] smallIndx = new int[contours.Length];
            int[] bigIndx = new int[contours.Length];

            for(int i = 0; i < contours.Length; i++)
            {
                if(Cv2.ContourArea(contours[i]) > 500 && Cv2.ContourArea(contours[i]) < 100000)
                {
                    Cv2.DrawContours(imageClone, contours, i, new Scalar(0, 0, 255), thickness: 3);

                    //przechowanie indexow malych kontorów
                    smallIndx[i] = i;
                }
                else
                {
                    //elementy nie przydzielone bez big indexu dostają 1000 zeby mozna je bylo odfiltorwac poki nie dziala big index
                    smallIndx[i] = 1000; 
                }

                // zlaczone klocki beda mialy wieksze pole!
                /*
                if (Cv2.ContourArea(contours[i]) > 2000 && Cv2.ContourArea(contours[i]) < 5000)
                {
                    Cv2.DrawContours(imageClone, contours, i, new Scalar(0, 255, 0), thickness: 3);

                    //przechowanie indx

                    bigIndx[i] = i;

                }
                */

            }

           //----------------------------------------------------------------------------------------------------------------------------

            int[,] peaks = new int[smallIndx.Length, 4];

            //smalindex 6 elementow
            for (int j =0; j < smallIndx.Length ; j++)
            {
                float[] sum = new float[contours[j].Length];
                float[] diff = new float[contours[j].Length];
                if (smallIndx[j] < 1000)
                {
                    for (int i = 0; i < contours[smallIndx[j]].Length; i++)     
                    {

                        sum[i] = contours[smallIndx[j]][i].X + contours[smallIndx[j]][i].Y;
                        diff[i] = contours[smallIndx[j]][i].X - contours[smallIndx[j]][i].Y;

                    }

                    //wierzcholki
                    peaks[j,0] = np.argmin(sum);   //topleft
                    peaks[j,1] = np.argmax(sum);   //bottom right
                    peaks[j,2] = np.argmin(diff);    //top right
                    peaks[j,3] = np.argmax(diff);      //bottom left
                    
                    //print wierzcholkow
                    Cv2.Circle(imageClone, contours[j][peaks[j, 0]], 3, new Scalar(0, 255, 0), 1);
                    Cv2.Circle(imageClone, contours[j][peaks[j, 1]], 3, new Scalar(0, 255, 0), 1);
                    Cv2.Circle(imageClone, contours[j][peaks[j, 2]], 3, new Scalar(0, 255, 0), 1);
                    Cv2.Circle(imageClone, contours[j][peaks[j, 3]], 3, new Scalar(0, 255, 0), 1);
                }
                else
                {
                    peaks[j, 0] = 1000;   //topleft
                    peaks[j, 1] = 1000;  //bottom right
                    peaks[j, 2] = 1000;    //top right
                    peaks[j, 3] = 1000;      //bottom left
                }
            }

            //----------------------------------------------------------------------------------------------------------------------------------------

            //wyszukanie wszystkich mozliwych midpointow
            int[ , ,] midPoints = new int[smallIndx.Length , 4 , 2];        // nr.konturu| x | y
            
            OpenCvSharp.Point punkt ;

            //formula X (x1 + x2 )* 0.5 , Y (y1 + y2) * 0.5
            
            for(int i = 0; i < smallIndx.Length ; i++)
            {
                if (smallIndx[i] < 1000)
                {
                    //zebranie srodkowych punktow konturu midPoints

                    midPoints[i ,0, 0] = Convert.ToInt32((contours[i][peaks[i, 0]].X + contours[i][peaks[i, 2]].X) * 0.5);    
                    midPoints[i ,0, 1] = Convert.ToInt32((contours[i][peaks[i, 0]].Y + contours[i][peaks[i, 2]].Y) * 0.5);

                    midPoints[i ,1 , 0] = Convert.ToInt32((contours[i][peaks[i, 1]].X + contours[i][peaks[i, 3]].X) * 0.5);    
                    midPoints[i ,1 , 1] = Convert.ToInt32((contours[i][peaks[i, 1]].Y + contours[i][peaks[i, 3]].Y) * 0.5);

                    midPoints[i, 2, 0] = Convert.ToInt32((contours[i][peaks[i, 0]].X + contours[i][peaks[i, 3]].X) * 0.5);    
                    midPoints[i, 2, 1] = Convert.ToInt32((contours[i][peaks[i, 0]].Y + contours[i][peaks[i, 3]].Y) * 0.5);

                    midPoints[i, 3, 0] = Convert.ToInt32((contours[i][peaks[i, 1]].X + contours[i][peaks[i, 2]].X) * 0.5);    
                    midPoints[i, 3, 1] = Convert.ToInt32((contours[i][peaks[i, 1]].Y + contours[i][peaks[i, 2]].Y) * 0.5);


                    //wyswietlenie punktów wraz z koordynatami-----------------------------------------------------------------------

                    //left center
                    punkt.X = midPoints[i, 0, 0];
                    punkt.Y = midPoints[i, 0, 1];
                    Cv2.Circle(imageClone, punkt , 3, new Scalar(255, 0, 0), 1);
                    Cv2.PutText(imageClone, Convert.ToString(punkt), punkt, HersheyFonts.HersheySimplex, 0.5, new Scalar(0, 255, 255), 1);

                    // right center
                    punkt.X = midPoints[i, 1, 0];
                    punkt.Y = midPoints[i, 1, 1];
                    Cv2.Circle(imageClone, punkt, 3, new Scalar(255, 0, 0), 1);
                    Cv2.PutText(imageClone, Convert.ToString(punkt), punkt, HersheyFonts.HersheySimplex, 0.5, new Scalar(255, 255, 255), 1);

                    //top center
                    punkt.X = midPoints[i, 2, 0];
                    punkt.Y = midPoints[i, 2, 1];
                    Cv2.Circle(imageClone, punkt, 3, new Scalar(255, 0, 0), 1);
                    Cv2.PutText(imageClone, Convert.ToString(punkt), punkt, HersheyFonts.HersheySimplex, 0.5, new Scalar(0, 0, 0), 1);

                    //bottom center
                    punkt.X = midPoints[i, 3, 0];
                    punkt.Y = midPoints[i, 3, 1];
                    Cv2.Circle(imageClone, punkt, 3, new Scalar(255, 0, 0), 1);
                    Cv2.PutText(imageClone, Convert.ToString(punkt), punkt, HersheyFonts.HersheySimplex, 0.5, new Scalar(0, 0, 0), 1);

                }
                else
                {
                    //szumy dostaja 1000 zeby mozna bylo je odfiltorwac
                    midPoints[i, 0, 0] = 1000;
                    midPoints[i, 0, 1] = 1000;
                    midPoints[i, 1, 0] = 1000;
                    midPoints[i, 1, 1] = 1000;
                    midPoints[i, 2, 0] = 1000;
                    midPoints[i, 2, 1] = 1000;
                    midPoints[i, 3, 0] = 1000;
                    midPoints[i, 3, 1] = 1000;
                }
            }



            
            //----------------------------------------------- shape / ksztalt ----------------------------------------------
            
            int[ , ] obiekty = new int[smallIndx.Length, 5];        
           
            // | nr.konturu | ksztalt| kolor | kat odchylenia | x.srodka | y.srodka |

            double perimeter;
            OpenCvSharp.Point[] approx;

            double A,B,C,w, h , aspectRatio;

            for (int i = 0; i < smallIndx.Length; i++)
            {
                if (smallIndx[i] < 1000)
                {
                    //obwod
                    perimeter = Cv2.ArcLength(contours[smallIndx[i]],true);

                    approx = Cv2.ApproxPolyDP(contours[smallIndx[i]], 0.04 * perimeter, true);

                    if(approx.Length == 3)
                    {
                        obiekty[i, 0] = 1;  //triangle
                        Cv2.PutText(imageClone, "triangle", contours[i][peaks[i, 3]], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);

                    }
                    else if(approx.Length == 4)
                    {
                        /*
                        //top left 
                        peaks[i,0]
                        //bottom left
                        peaks[i,3] 
                        //top right
                        peaks[i,2]
                        //bottom right
                        peaks[i,1] 
                        */

                        //dlugosc odcinka sqrt((x2 - x1)^2 + (y2 - y1)^2)

                        A = Math.Pow((contours[i][peaks[i, 3]].X - contours[i][peaks[i, 0]].X), 2);
                        B = Math.Pow((contours[i][peaks[i, 3]].Y - contours[i][peaks[i, 0]].Y), 2);
                        C = A + B;
                        w = Math.Sqrt(C);
                        A = Math.Pow((contours[i][peaks[i, 2]].X - contours[i][peaks[i, 0]].X), 2);
                        B = Math.Pow((contours[i][peaks[i, 2]].Y - contours[i][peaks[i, 0]].Y), 2);
                        C = A + B;
                        h = Math.Sqrt(C);

                        aspectRatio = w / h;

                        if(aspectRatio >= 0.95 && aspectRatio <= 1.05)
                        {
                            obiekty[i, 0] = 2;  //square kwadrat
                            Cv2.PutText(imageClone, "square", contours[i][peaks[i, 3]], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                        }
                        else
                        {
                            obiekty[i, 0] = 3;  //rectangle prostokat
                            Cv2.PutText(imageClone, "rectangle", contours[i][peaks[i, 3]], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                        }
                    }
                    else if(approx.Length == 5)
                    {
                        obiekty[i, 0] = 4; //pentagon
                        Cv2.PutText(imageClone, "pentagon", contours[i][peaks[i, 3]], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                    else
                    {
                        obiekty[i, 0] = 5;//circle
                        Cv2.PutText(imageClone, "circle", contours[i][peaks[i, 3]], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                }
                else
                {
                    //filtracja szumów
                    obiekty[i, 0] = 1000;
                }
            }


            //---------------------------------------------------rotacja i center of mass

            double dx, dy,angle;
            int degree;

            OpenCvSharp.Point ptext ,p1,p2;

            for (int i = 0; i < smallIndx.Length; i++)
            {
                if (smallIndx[i] < 1000)
                {
                    // right center
                    ptext.X = midPoints[i, 1, 0] + 30;
                    ptext.Y = midPoints[i, 1, 1];


                    dx = midPoints[i, 3, 0] - midPoints[i, 2, 0];
                    dy = midPoints[i, 3, 1] - midPoints[i, 2, 1];
                    angle = Math.Atan2(dx, dy);
                    degree = Convert.ToInt32(angle * 180 / Math.PI);
                    //if (degree < 0)
                    //{
                     //   degree = 360 + degree;
                    //}

                    //wyswietlenie kata
                    p1.X = midPoints[i, 2, 0];
                    p1.Y = midPoints[i, 2, 1];
                    p2.X = midPoints[i, 3, 0];
                    p2.Y = midPoints[i, 3, 1];

                    Cv2.Line(imageClone, p1, p2, new Scalar(0, 0, 255), 2);
                    Cv2.PutText(imageClone, Convert.ToString(degree), ptext, HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 255), 2) ;
                

                }
                else
                {
                    //filtracja szumów
                    obiekty[i, 0] = 1000;
                }
            }














                Bitmap bitimg2 = MatToBitmap(imageClone);

            pictureBox1.Image = bitimg2;















        }





    






        private void button2_Click_1(object sender, EventArgs e)
        {
            Mat imageClone = wej.Clone();
            OpenCvSharp.Point[][] contours = AllCont(wej);

            Cv2.DrawContours(imageClone, contours, 20, new Scalar(0, 0, 255), thickness: 3);

            Bitmap bitimg = MatToBitmap(imageClone);

            pictureBox1.Image = bitimg;

        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void contury_Load(object sender, EventArgs e)
        {

        }
    }
}




/*
//smooth
Cv2.GaussianBlur(refGray, refGray, kernel, 0);

//dylatacja
Cv2.Dilate(refGray, refGray, null);

//erozja
Cv2.Erode(refGray, refGray, null);

Cv2.Canny(wej, refGray, 100, 200);
*/




/*
richTextBox1.Text = Convert.ToString(contours[0][0])+ "\n" +
   Convert.ToString(contours[1][1]) + "\n" +
   Convert.ToString(contours[1][2]) + "\n" +
   Convert.ToString(contours[1][3]) + "\n" +
   Convert.ToString(contours[1][4]) + "\n" +
   Convert.ToString(contours[1][5]) + "\n" +
   Convert.ToString(contours[1][6]);
*/

/*
Cv2.Circle(imageClone, contours[1][1], 2, new Scalar(0, 0, 255) , 3);
Cv2.Circle(imageClone, contours[1][2], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][3], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][4], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][5], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][6], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][7], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][8], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][9], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][10], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][11], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][20], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][30], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][40], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][50], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][60], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][70], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][80], 2, new Scalar(0, 0, 255), 3);


OpenCvSharp.Point[][] rogi;

Cv2.ApproxPolyDP(contours[1],1,true);

Cv2.Circle(imageClone, contours[1][1], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][2], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][3], 2, new Scalar(0, 0, 255), 3);
Cv2.Circle(imageClone, contours[1][4], 2, new Scalar(0, 0, 255), 3);


*/

/*
for ( int i =1; i < contours[1].Length; i++)
{
    if(contours[1][i].X < )
        int xmin = 


}
*/


//punkty konturu
//OpenCvSharp.Point[] n1 = contours[0];
//Cv2.Circle(imageClone, n1, 1, 255, 3);

// momenty 
//var momenty = Cv2.Moments(contours[0]);
//int cx = Convert.ToInt32(momenty.M10 / momenty.M00);
//int cy = Convert.ToInt32(momenty.M01 / momenty.M00);

//OpenCvSharp.Point p = new OpenCvSharp.Point(cx, cy);

//Cv2.Circle(imageClone, p , 2, 5 , 3);

