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
using System.IO;



namespace single_photo
{
    public partial class Form1 : Form
    {

        Mat wej = new Mat();
        Mat raw = new Mat();


        public Form1()
        {
            InitializeComponent();



        }



        //converter z mat na bitmape 
        public static Bitmap MatToBitmap(Mat image)
        {
            return OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
        } // end of MatToBitmap function








        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

 
                string path = @"C:\mon";

                
                wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\shape.jpg", ImreadModes.Unchanged );
                raw = wej;


                Bitmap bitimg = MatToBitmap(wej);

            pictureBox1.Image = bitimg;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Mat refGray = new Mat();
            Mat thresh = new Mat();
            Mat imageClone = wej.Clone();

            //konwersja na odcienie szarosci
            Cv2.CvtColor(wej, refGray, ColorConversionCodes.BGR2GRAY);


            // piksel wiekszy niz thresh 255
            Cv2.Threshold(refGray, thresh, 80, 255, ThresholdTypes.BinaryInv);

            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hIndx;
            Cv2.FindContours(thresh, out contours, out hIndx, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

            Cv2.DrawContours(imageClone, contours, 20, new Scalar(0, 0, 255), thickness: 3);

            Bitmap bitimg = MatToBitmap(thresh);

            pictureBox1.Image = bitimg;
            
        }


        static OpenCvSharp.Point[][] AllCont(Mat img)
        {

            Mat refGray = new Mat();
            Mat thresh = new Mat();

            //konwersja na odcienie szarosci
            Cv2.CvtColor(img, refGray, ColorConversionCodes.BGR2GRAY);

            //działanie thresholdu :
            // piksel o wartosci mniejszej niz thresh ustawiany jest na 0,
            // piksel wiekszy niz thresh 255
            Cv2.Threshold(refGray, thresh, 80, 255, ThresholdTypes.BinaryInv);

            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hIndx;
            Cv2.FindContours(thresh, out contours, out hIndx, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

            return contours;
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Mat imageClone = wej.Clone();
            OpenCvSharp.Point[][] contours = AllCont(wej);

            Cv2.DrawContours(imageClone, contours, 20, new Scalar(0, 0, 255), thickness: 3);
            
            Bitmap bitimg = MatToBitmap(imageClone);

            pictureBox1.Image = bitimg;


        }
    }
}
