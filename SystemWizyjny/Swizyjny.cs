using DirectShowLib;
using NumSharp;
using OpenCvSharp;
using System;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;




namespace SystemWizyjny
{
    public partial class Swizyjny : Form
    {
        

        //obiekty mat przechowujące zdjecia
        Mat wej = new Mat();
        Mat wejthresh = new Mat();



        Mat raw = new Mat();
        Mat clone = new Mat();

        //liczba roznych konturow
        int small = 0, connected = 0;

        OpenCvSharp.Point[][] contours;
        OpenCvSharp.Point[][] smallContours;
        OpenCvSharp.Point[][] connectedContours;
        OpenCvSharp.Point[][] sortedSmallContours;
        OpenCvSharp.Point[][] sortedConnectedContours;

        OpenCvSharp.Point[][] toolUpSingleContours;
        OpenCvSharp.Point[][] toolDownSingleContours;

        OpenCvSharp.Point2f[][] MarkerOut = new OpenCvSharp.Point2f[10][];

        //obiekty
        ContourOb[] singleConts;
        ContourOb[] connectedConts;

        //pixel per metrics
        double ppm;

        //wymiary jednej lapy narzedzia
        double toolSideWidth = 10;
        double toolSideHeight = 10;


        //RS232 connection
        static SerialPort port;
        static string portMessage;

        //framesource OpenCvSharp
        private FrameSource frameSource;



        //timer aplikacji 1
        static Timer timerOne = new Timer();
        static int alarmCounter = 1;
        static bool exitFlag = false;
        static bool storeFlag = false;

        //stored message from rs232
        static string storedMsg = "";

        //timer aplikacji 2
        static Timer timerTwo = new Timer();
        static int counterTwo = 0;
        static bool readFlag = false;
        static bool freeTwo = true;


        //tablica wiadomosci poki co na 20 klockow / pozycji
        string[] msgTab = new string[50];


        //-------------------------------------------------------- APP LOAD -----------------------------------------------------------------
        public Swizyjny()
        {
            InitializeComponent();
        }

        private void Swizyjny_Load(object sender, EventArgs e)
        {
            DsDevice[] captureDevices;
            string[] ports = SerialPort.GetPortNames();

            //zaladowanie dostepnych kamer
            // Get the set of directshow devices that are video inputs.
            captureDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);

            var devices = new List<DsDevice>(DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice));
            string[] cameraNames = new string[devices.Count];

            for (int i = 0; i < devices.Count; i++)
            {
                cameraNames[i] = devices[i].Name;
                comboBox3.Items.Add(cameraNames[i]);
            }


            //zaladowanie dostepnych portow
            if (ports.Length != 0)
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    comboBox1.Items.Add(ports[i]);
                }
            }
            else
            {
                comboBox1.Items.Add("none");
            }

        }

        //----------------------------------------------------------------------- TIMERY 1 ------------------------------------------------------------
        private void TimerEventProcessor1(Object sender, EventArgs myEventArgs)
        {
            if (storeFlag == false)
            {
                String RecievedData;
                RecievedData = port.ReadExisting();
                if (!(RecievedData == ""))
                {
                    richTextBox1.Text += RecievedData;
                }
            }
            else
            {
                String RecievedData = "pobrano";
                //RecievedData = port.ReadExisting();
                if (RecievedData != "")
                {
                    storedMsg = RecievedData;
                    richTextBox5.Text += "\n" + storedMsg;
                }
            }
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------

        //organizacja blokow aplikacji

        //------------------------------------------------------------------------------------------Okno Welcome----------------------------------------------------------------------

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        //wybor zdjecia do sprawdzenia
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //Zakladka welcome wgranie zdjecia
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //wybor zdjecia
                string photo = listBox1.Items[listBox1.SelectedIndex].ToString();

                if (photo == "photo 1")
                {
                    wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\sample1.jpg", ImreadModes.Unchanged);
                }
                else if (photo == "photo 2")
                {
                    wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\sample2.jpg", ImreadModes.Unchanged);
                }
                else if (photo == "photo 3")
                {
                    wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\sample3.jpg", ImreadModes.Unchanged);
                }
                else if (photo == "photo 4")
                {
                    wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\sample4.jpg", ImreadModes.Unchanged);
                }
                else if (photo == "photo 5")
                {
                    wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\fifth.jpg", ImreadModes.Unchanged);
                }
                else if (photo == "photo 6")
                {
                    wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\sixth.jpg", ImreadModes.Unchanged);
                }
                else if (photo == "photo 7")
                {
                    wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\seventh.jpg", ImreadModes.Unchanged);
                }
                else if (photo == "photo 8")
                {
                    wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\eight.jpg", ImreadModes.Unchanged);
                }

                //frameSource = Cv2.CreateFrameSource_Camera(0);
                //wej = Vision.Capture(frameSource, false);



                //obraz zakladka welcome
                pictureBox3.Height = wej.Height;
                pictureBox3.Width = wej.Width;

                //obraz zakladka kalibracja
                pictureBox1.Height = wej.Height;
                pictureBox1.Width = wej.Width;

                //obraz zakladka program
                pictureBox2.Height = wej.Height;
                pictureBox2.Width = wej.Width;

                richTextBox1.Text = Convert.ToString(wej.Width) + "\n" + Convert.ToString(wej.Height) + "\n";

                Bitmap bitimg = Vision.MatToBitmap(wej);
                pictureBox3.Image = bitimg;
                pictureBox1.Image = bitimg;
                pictureBox2.Image = bitimg;
            }
            catch(Exception ex)
            {
                richTextBox3.Text = Convert.ToString(ex);
            }
        }

        //defaultowe ustawienia
        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text = "55";
            textBox2.Text = "255";

            textBox3.Text = "3000";
            textBox4.Text = "4000";
            textBox5.Text = "4000";

            textBox9.Text = "0";
            textBox10.Text = "15";
            textBox12.Text = "160";
            textBox11.Text = "180";
            textBox14.Text = "40";
            textBox13.Text = "90";
            textBox16.Text = "90";
            textBox15.Text = "140";
        }


        //take a photo from a camera
        private void button17_Click(object sender, EventArgs e)
        {
            //frameSource = Cv2.CreateFrameSource_Camera(0);
            //wej = Vision.Capture(frameSource,false);


            
            var capture = new VideoCapture(0);
            capture.Open(0, 0);
            capture.Set(CaptureProperty.FrameWidth, 1280);
            capture.Set(CaptureProperty.FrameHeight, 720);



            if (capture.IsOpened() == false)
            {
                richTextBox3.Text = "kamera nie otwarta";
            }
            else
            {
                capture.Read(wej);

            }
            

            //obraz zakladka welcome
            pictureBox3.Height = wej.Height;
            pictureBox3.Width = wej.Width;

            //obraz zakladka kalibracja
            pictureBox1.Height = wej.Height;
            pictureBox1.Width = wej.Width;

            //obraz zakladka program
            pictureBox2.Height = wej.Height;
            pictureBox2.Width = wej.Width;

            Bitmap bitimg = Vision.MatToBitmap(wej);    //konwersja na bitimg do wpf

            pictureBox3.Image = bitimg;                 //wyswietlenie
            pictureBox2.Image = bitimg;
            pictureBox1.Image = bitimg;
        }

        


        //----------------------------------------------------------------------------------Okno Kalibracja--------------------------------------------------------------------

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //dobranie thresholda
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //reset
                wejthresh = wej;

                Mat refGray = new Mat();
                Mat gaus = new Mat();
                Mat thresh = new Mat();

                //konwersja na odcienie szarosci
                Cv2.CvtColor(wejthresh, refGray, ColorConversionCodes.BGR2GRAY);

                //blur 
                OpenCvSharp.Size kernel = new OpenCvSharp.Size(3, 3);
                //Cv2.GaussianBlur(refGray, gaus, kernel,0,0);
                Cv2.MorphologyEx(refGray, gaus, MorphTypes.Close,null) ;
                //tresh 127 - 255
                Cv2.Threshold(gaus, thresh, Convert.ToInt32(textBox1.Text) , Convert.ToInt32(textBox2.Text), ThresholdTypes.BinaryInv);

                Bitmap bitimg = Vision.MatToBitmap(thresh);
                pictureBox1.Image = bitimg;
            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }

        }


        // Tu trzeba zmienic coordynaty sporbujemy je wyswietlic

        //Znalezc kontury
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                OpenCvSharp.Point[][] kontury = null;
                raw = wej.Clone();

                kontury = Vision.GetContours(wej, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
                Cv2.DrawContours(raw, kontury, -1, new Scalar(0, 0, 255), thickness: 3);

                double a;
                int area;


                for(int i = 0; i< kontury.Length;i++)
                {
                    Cv2.Circle(raw, kontury[i][0], 1, new Scalar(0, 255, 0), 1);
                    a = Cv2.ContourArea(kontury[i]);
                    area = Convert.ToInt32(a);
                    Cv2.PutText(raw, Convert.ToString(area), kontury[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);
                }

                Bitmap bitimg = Vision.MatToBitmap(raw);
                pictureBox1.Image = bitimg;
                richTextBox1.Text = "liczba konturów : " + Convert.ToString(kontury.Length);
                contours = kontury;

            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        //Sortowanie na male i duze
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitimg = Vision.MatToBitmap(wej);
                pictureBox1.Image = bitimg;

                //klon
                Mat imageClone = wej.Clone();
                (imageClone, small, connected) = Vision.showContours(imageClone, contours, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text));

                bitimg = Vision.MatToBitmap(imageClone);
                pictureBox1.Image = bitimg;
                richTextBox1.Text = "liczba konturów osobnych" + Convert.ToString(small) + "\n" + "liczba konturów polaczonych: " + Convert.ToString(connected) + "\n";
            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }
        }


        //indeksowanie konturów
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bitimg = Vision.MatToBitmap(wej);
                Mat imageClone = wej.Clone();
                OpenCvSharp.Point punkt;

                (smallContours, connectedContours) = Vision.SmallBigContours(contours, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text), small, connected);

                for (int i = 0; i < smallContours.Length; i++)
                {
                    punkt = smallContours[i][0];
                    //punkt.X = punkt.X - 100;
                    //punkt.Y = punkt.Y - 100;
                    Cv2.PutText(imageClone, Convert.ToString(i), punkt, HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 255, 0), 1);
                }

                for (int i = 0; i < connectedContours.Length; i++)
                {
                    punkt = connectedContours[i][0];
                    //punkt.X = punkt.X - 100;
                    //punkt.Y = punkt.Y - 100;
                    Cv2.PutText(imageClone, Convert.ToString(i), punkt, HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);
                }
                Cv2.DrawContours(imageClone, smallContours, -1, new Scalar(0, 255, 0), thickness: 3);
                Cv2.DrawContours(imageClone, connectedContours, -1, new Scalar(255, 0, 0), thickness: 3);
                bitimg = Vision.MatToBitmap(imageClone);
                pictureBox1.Image = bitimg;
            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }
        }




        //Sortowanie indeksow od lewej  ZLE DZIALA !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                Mat imageClone = wej.Clone();
                OpenCvSharp.Point punkt;

                (sortedSmallContours, sortedConnectedContours) = Vision.LeftSortContours(smallContours, connectedContours, imageClone);
                //(sortedSmallContours, sortedConnectedContours) = Vision.LSortContours(smallContours, connectedContours);

                for (int i = 0; i < sortedSmallContours.Length; i++)
                {
                    punkt = sortedSmallContours[i][0];
                    //punkt.X = punkt.X - 100;
                    //punkt.Y = punkt.Y - 100;
                    Cv2.PutText(imageClone, Convert.ToString(i), punkt, HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 255, 0), 1);
                }

                for (int i = 0; i < sortedConnectedContours.Length; i++)
                {
                    punkt = sortedConnectedContours[i][0];
                    //punkt.X = punkt.X - 100;
                    //punkt.Y = punkt.Y - 100;
                    Cv2.PutText(imageClone, Convert.ToString(i), punkt, HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);
                }

                //zaznaczenie konturów
                Cv2.DrawContours(imageClone, sortedSmallContours, -1, new Scalar(0, 255, 0), thickness: 3);
                Cv2.DrawContours(imageClone, sortedConnectedContours, -1, new Scalar(255, 0, 0), thickness: 3);

                Bitmap bitimg = Vision.MatToBitmap(imageClone);
                pictureBox1.Image = bitimg;
            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }
        }





        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }




        //sprawdzenie kolorow konturu
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                Mat imageClone = wej.Clone();
                Mat hsv = new Mat();
                OpenCvSharp.Point center;
                Vec3b vector;

                Cv2.CvtColor(imageClone, hsv, ColorConversionCodes.BGR2HSV_FULL);


                for (int i = 0; i < sortedSmallContours.Length; i++)
                {
                    string color = Vision.ContourColor(imageClone, sortedSmallContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox13.Text), Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                    //momenty
                    var M = Cv2.Moments(sortedSmallContours[i], false);
                    center.X = Convert.ToInt32(M.M10 / M.M00);
                    center.Y = Convert.ToInt32(M.M01 / M.M00);

                    vector = hsv.At<Vec3b>(center.Y, center.X);
                    Scalar col = new Scalar(vector.Item0, vector.Item1, vector.Item2);

                    Cv2.Circle(imageClone, center, 3, new Scalar(0, 255, 0), 1);
                    Cv2.PutText(imageClone, color + " " + Convert.ToString(col.Val0) + " " + Convert.ToString(col.Val1) + " " + Convert.ToString(col.Val2) + " ", sortedSmallContours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);


                }
                Bitmap bitimg = Vision.MatToBitmap(imageClone);
                pictureBox1.Image = bitimg;
            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }
        }




        //shape recogniton

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                string shape;

                Mat imageClone = wej.Clone();

                for (int i = 0; i < sortedSmallContours.Length; i++)
                {
                    shape = Vision.GetShape(sortedSmallContours[i]);

                    if (shape == "triangle")
                    {
                        Cv2.PutText(imageClone, "triangle", sortedSmallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                    else if (shape == "square")
                    {
                        Cv2.PutText(imageClone, "square", sortedSmallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                    else if (shape == "rectangle")
                    {
                        Cv2.PutText(imageClone, "rectangle", sortedSmallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                    else if (shape == "pentagon")
                    {
                        Cv2.PutText(imageClone, "pentagon", sortedSmallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                    else if (shape == "circle")
                    {
                        Cv2.PutText(imageClone, "circle", sortedSmallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                }

                Bitmap bitimg = Vision.MatToBitmap(imageClone);
                pictureBox1.Image = bitimg;
            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }
        }


        //angle recognition
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                int angle = 0;
                int[] peaks;
                OpenCvSharp.Point center, mid, temp;
                int[] corners;

                //nowa metoda
                Rect r;
                float ang;

                Mat imageClone = wej.Clone();

                for (int i = 0; i < sortedSmallContours.Length; i++)
                {
                    angle = Vision.GetAngle(imageClone, sortedSmallContours[i]);

                    Cv2.PutText(imageClone, Convert.ToString(angle), sortedSmallContours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 255), 1);

                }

                Bitmap bitimg = Vision.MatToBitmap(imageClone);
                pictureBox1.Image = bitimg;
            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }
        }


        //creating objects for all contours
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                //przygotowanie zmiennych
                Mat imageClone = wej.Clone();

                string color, shape;
                int angle;
                OpenCvSharp.Point c = new OpenCvSharp.Point();
                OpenCvSharp.Point[] cor;

                singleConts = new ContourOb[sortedSmallContours.Length];
                connectedConts = new ContourOb[sortedConnectedContours.Length];



                for (int i = 0; i < sortedSmallContours.Length; i++)
                {
                    //center z momentow 
                    c = Vision.GetCenter(sortedSmallContours[i]);

                    //kolor
                    color = Vision.ContourColor(imageClone, sortedSmallContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox13.Text), Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                    //shape 
                    shape = Vision.GetShape(sortedSmallContours[i]);

                    //angle
                    angle = Vision.GetAngle(imageClone, sortedSmallContours[i]);

                    //corners
                    cor = Vision.GetCorners(sortedSmallContours[i]);

                    //tworzeni kazdego obiektu
                    singleConts[i] = new ContourOb()
                    {
                        indx = i,        //index 0,1,2,3,4...       
                        center = c,      //srodek konturu
                        angle = angle,   //obrot konturu od osi x
                        color = color,   //kolor konturu
                        shape = shape,   //ksztalt konturu
                        corners = cor,
                        contour = sortedSmallContours[i],   //wszystkie punkty konturu
                    };

                    
                    //organizacja naroznikow
                    OpenCvSharp.Point temp0, temp1, temp2, temp3;
                    if (singleConts[i].angle > 90)
                    {

                        temp0 = singleConts[i].corners[0];
                        temp1 = singleConts[i].corners[1];
                        temp2 = singleConts[i].corners[2];
                        temp3 = singleConts[i].corners[3];

                        singleConts[i].corners[0] = temp1;
                        singleConts[i].corners[1] = temp2;
                        singleConts[i].corners[2] = temp3;
                        singleConts[i].corners[3] = temp0;

                    }
                    

                }

                for (int i = 0; i < sortedConnectedContours.Length; i++)
                {
                    //center z momentow 
                    c = Vision.GetCenter(sortedConnectedContours[i]);

                    //kolor
                    color = Vision.ContourColor(imageClone, sortedConnectedContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox13.Text), Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                    //shape 
                    shape = Vision.GetShape(sortedConnectedContours[i]);

                    //angle
                    angle = Vision.GetAngle(imageClone, sortedConnectedContours[i]);

                    //corners
                    cor = Vision.GetCorners(sortedConnectedContours[i]);

                    //tworzeni kazdego obiektu
                    connectedConts[i] = new ContourOb()
                    {
                        indx = i,        //index 0,1,2,3,4...       
                        center = c,      //srodek konturu
                        angle = angle,   //obrot konturu od osi x
                        color = color,   //kolor konturu
                        shape = shape,   //ksztalt konturu
                        corners = cor,
                        contour = sortedConnectedContours[i],   //wszystkie punkty konturu
                    };


                    //organizacja naroznikow
                    OpenCvSharp.Point temp0, temp1,temp2,temp3;
                    if (connectedConts[i].angle > 90)
                    {

                        temp0 = connectedConts[i].corners[0];
                        temp1 = connectedConts[i].corners[1];
                        temp2 = connectedConts[i].corners[2];
                        temp3 = connectedConts[i].corners[3];

                        connectedConts[i].corners[0] = temp1;
                        connectedConts[i].corners[1] = temp2;
                        connectedConts[i].corners[2] = temp3;
                        connectedConts[i].corners[3] = temp0;


                    }

                }
            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }



        }

        //show single contours
        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                Mat imageClone = wej.Clone();

                richTextBox1.Text = "";

                for (int i = 0; i < singleConts.Length; i++)
                {
                    //indekswoanie na obrazie
                    Cv2.Circle(imageClone, singleConts[i].contour[0], 1, new Scalar(255, 0, 0), 1);

                    //puttext
                    Cv2.PutText(imageClone, Convert.ToString(singleConts[i].indx), singleConts[i].contour[0], HersheyFonts.HersheySimplex, 1, new Scalar(0, 255, 0), 1);

                    //opisy
                    richTextBox1.Text += "\n" + Convert.ToString(singleConts[i].indx) + "\n";
                    richTextBox1.Text += "center:  " + Convert.ToString(singleConts[i].center) + "\n";
                    richTextBox1.Text += "angle:   " + Convert.ToString(singleConts[i].angle) + "\n";
                    richTextBox1.Text += "color:   " + Convert.ToString(singleConts[i].color) + "\n";
                    richTextBox1.Text += "shape:   " + Convert.ToString(singleConts[i].shape) + "\n";
                }
                //zaznaczenie konturów
                Cv2.DrawContours(imageClone, sortedSmallContours, -1, new Scalar(0, 255, 0), thickness: 3);


                Bitmap bitimg = Vision.MatToBitmap(imageClone);
                pictureBox1.Image = bitimg;
            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }

        }
        
        //show connected contours
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                Mat imageClone = wej.Clone();

                richTextBox1.Text = "";

                for (int i = 0; i < connectedConts.Length; i++)
                {
                    //indekswoanie na obrazie
                    Cv2.Circle(imageClone, connectedConts[i].contour[0], 1, new Scalar(255, 0, 0), 1);

                    //puttext
                    Cv2.PutText(imageClone, Convert.ToString(connectedConts[i].indx), connectedConts[i].contour[0], HersheyFonts.HersheySimplex, 1, new Scalar(0, 255, 0), 1);

                    //opisy
                    richTextBox1.Text += "\n" + Convert.ToString(connectedConts[i].indx) + "\n";
                    richTextBox1.Text += "center:  " + Convert.ToString(connectedConts[i].center) + "\n";
                    richTextBox1.Text += "angle:   " + Convert.ToString(connectedConts[i].angle) + "\n";
                    richTextBox1.Text += "color:   " + Convert.ToString(connectedConts[i].color) + "\n";
                    richTextBox1.Text += "shape:   " + Convert.ToString(connectedConts[i].shape) + "\n";
                }

                Cv2.DrawContours(imageClone, sortedConnectedContours, -1, new Scalar(255, 0, 0), thickness: 3);

                Bitmap bitimg = Vision.MatToBitmap(imageClone);
                pictureBox1.Image = bitimg;
            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //------------------------------------------------------------- RS232 CONNECTION ----------------------------------

        //Connect button
        private void button16_Click(object sender, EventArgs e)
        {
            try
            {


                // Instantiate the communications
                // port with some basic settings
                port = new SerialPort("COM5", 9600, Parity.Even,8, StopBits.Two);
                port.Handshake = Handshake.None;
                port.ReadTimeout = 500;
                port.WriteTimeout = 500;
                // Open the port for communications
                port.Open();

                //po otwarciu portu odpalenie timera 1
                exitFlag = false;
                timerOne.Tick += new EventHandler(TimerEventProcessor1);
                timerOne.Interval = 500;
                timerOne.Start();

                //po otwarciu portu odpalenie timera 2
                exitFlag = false;
                timerTwo.Tick += new EventHandler(TimerEventProcessor2);
                timerTwo.Interval = 500;

                bool state = port.IsOpen;
                richTextBox1.Text = "\n port communication info : " + Convert.ToString(state);




            }
            catch(Exception ex)
            {
                richTextBox1.Text = Convert.ToString(ex);
            }
        }

        //wprowadzanie wiadomosci
        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        //Senda data 
        private void button23_Click(object sender, EventArgs e)
        {
            try
            {

                //tesotwe zmienne 

                int x = 400;
                int y = 3;
                int z = 500;
                int a = 0;
                int b = 0;
                int axis7 = 1035;
                bool f1 = false;
                bool f2 = false;




                //wersja string dziala super tylko czy to dobry ciag wynik testu = ?     ?

                //nie dziala 

                string message = textBox6.Text;

                //string message = "WH"; // + " +" + Convert.ToString(x) + ",+" + Convert.ToString(y) + ",+" + Convert.ToString(z) + ",0,0,+1035"; // +
                                //"," + Convert.ToString(a) + "," + Convert.ToString(b) + "," + Convert.ToString(axis7) + ",";
                /*
                if (f1 == false)
                {
                    message += "R" + ",";
                }
                else
                {
                    message += "L" + ",";
                }

                if (f2 == false)
                {
                    message += "A";
                }
                else
                {
                    message += "B" + ",";

                }
                */

                message += '\r';

                byte[] bytes = Encoding.Default.GetBytes(message);
                string str = Encoding.Default.GetString(bytes);


                richTextBox1.Text = "\n" + String.Join(" ",bytes) + "\n" + str + "\n";

                port.Write(bytes, 0, bytes.Length);


                /*
                byte[] byteMsg = new byte[100];

                //command
                byte M = (byte)'M';
                byte P = (byte)'P';

                //coordinates 
                byte[] X = Connect.IntToByte(x);
                byte[] Y = Connect.IntToByte(y);
                byte[] Z = Connect.IntToByte(z);

                //turning angle
                byte[] A = Connect.IntToByte(a);
                byte[] B = Connect.IntToByte(b);

                //additional axis
                byte[] Axis7 = Connect.IntToByte(axis7);

                byte F1, F2;

                if (f1 == false)
                {
                    F1 = (byte)'R';
                }
                else
                {
                    F1 = (byte)'L';
                }

                if (f2 == false)
                {
                    F2 = (byte)'R';
                }
                else
                {
                    F2 = (byte)'L';
                }

                //przygotowanie tablicy byte

                //skrot komendy
                byteMsg[0] = M;
                byteMsg[1] = P;

                //coordynaty X,Y,Z
                int counter = 2;
                for (int i = 0, j = counter; i < X.Length; i++, j++)
                {
                    byteMsg[j] = X[i];
                    counter = j;
                }
                for (int i = 0, j = counter; i < Y.Length; i++, j++)
                {
                    byteMsg[j] = Y[i];
                    counter = j;
                }
                for (int i = 0, j = counter; i < Z.Length; i++, j++)
                {
                    byteMsg[j] = Z[i];
                    counter = j;
                }

                //turn angles A,B
                for (int i = 0, j = counter; i < A.Length; i++, j++)
                {
                    byteMsg[j] = X[i];
                    counter = j;
                }
                for (int i = 0, j = counter; i < B.Length; i++, j++)
                {
                    byteMsg[j] = X[i];
                    counter = j;
                }


                //additional axis

                //additional axis
                for (int i = 0, j = counter; i < B.Length; i++, j++)
                {
                    byteMsg[j] = Axis7[i];
                    counter = j;
                }

                //flags
                byteMsg[counter + 1] = F1;
                byteMsg[counter + 2] = F2;

                richTextBox1.Text = "\n" + "Wiadomosc : " + "\n";

                string str = Encoding.

                for (int i = 0; i < byteMsg.Length; i++)
                {
                    richTextBox1.Text += " " + byteMsg[i];
                }







                */
                //----------------------------------------------------------------------------------wlasciwy sen dla tego buttona
                /*
                string message = textBox6.Text;
                
                if (textBox6.Text != "")
                {
                    message = message + " 13";
                    richTextBox1.Text = "\n" + message;

                    string[] splitedMessage = message.Split(' ');
                    richTextBox1.Text += "\n" + splitedMessage[0] + splitedMessage[1] + splitedMessage[2];

                    byte[] byteMsg = new byte[splitedMessage.Length];

                    for (int i = 0; i < splitedMessage.Length; i++)
                    {

                        byteMsg[i] = Convert.ToByte(splitedMessage[i]);
                         
                        
                    }

                    richTextBox1.Text += "\n" + ": " + byteMsg[0] + byteMsg[1] + byteMsg[2];

                */

                //port.Write(byteMsg, 0, byteMsg.Length);


                //byte[] byteMessage = Connect.StringToByte(msg);



                //TO DZIALA
                //byte[] bytestosend = { 87, 72 , 13 }; 
                //richTextBox1.Text += "\n" + ": " + bytestosend[0] + bytestosend[1] + bytestosend[2];
                //port.Write(bytestosend, 0, bytestosend.Length);
                //}
                //else
                //{
                //    richTextBox1.Text += "\n" + "Nie wprowadzono żadnej komendy !" + "\n";
                //}
            }
            catch (Exception ex)
            {
                richTextBox1.Text += Convert.ToString(ex);
            }
        }

        //recive info
        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                if (port.IsOpen)
                {
                    //Read text from port
                    richTextBox1.Text = "\n msg: " + port.ReadExisting();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //Disconnect 
        private void button24_Click(object sender, EventArgs e)
        {
            // Close the port
            port.Close();

            exitFlag = true;
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------














        //measurment of the biggest contour ZMIANA NA ARUCO MARKER
        private void button19_Click(object sender, EventArgs e)
        {
            //reset
            wejthresh = wej;
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
            Cv2.Threshold(gaus, thresh, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text), ThresholdTypes.BinaryInv);

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

            OpenCvSharp.Point2f[][] Rejected = new OpenCvSharp.Point2f[10][];
            int[] MarkerId = new int[10];
            OpenCvSharp.Aruco.CvAruco.DetectMarkers(wejthresh, ArucoDict, out MarkerOut, out MarkerId, arucoParam, out Rejected);

            if (MarkerId.Length == 2)
            {
                    OpenCvSharp.Aruco.CvAruco.DrawDetectedMarkers(wejthresh, MarkerOut, MarkerId, new Scalar(255, 0, 0));

                    richTextBox1.Text += "\n>??\n\n " + "X1: " + Convert.ToString(MarkerOut[0][0].X) + "\nY1 = " + Convert.ToString(MarkerOut[0][0].Y) + "\n";
                    richTextBox1.Text += "\n>??\n\n " + "X2: " + Convert.ToString(MarkerOut[1][0].X) + "\nY2 = " + Convert.ToString(MarkerOut[1][0].Y) + "\n";
            }
            else
            {
                OpenCvSharp.Aruco.CvAruco.DrawDetectedMarkers(wejthresh, Rejected, null, new Scalar(0, 255, 0));
                richTextBox1.Text += "\nERRORR wyswietlono bledne przypisania";
            }

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

            Cv2.Circle(wejthresh, odniesienie1, 1, new Scalar(0, 0, 255));
            Cv2.Circle(wejthresh, odniesienie2, 1, new Scalar(0, 0, 255));

            double width = 20;
            double obwidth = Convert.ToDouble(rot.Size.Height);
            double obheight = Convert.ToDouble(rot.Size.Width);

            ppm = obwidth / width;

            double dimA = obwidth / ppm;
            double dimB = obheight / ppm;

            richTextBox1.Text += "\n" + "height  " + Convert.ToString(rot.Size.Height) + "\n" + "width  " + Convert.ToString(rot.Size.Width);
            richTextBox1.Text += "\n\n" + Convert.ToString(ppm) + "\n" + "dimA:" + Convert.ToString(dimA) + "\n" + "dimB: " + Convert.ToString(dimB);



            //wspolczynniki abcd ze strony how to map points between 2d coordinate systems bez minusów
            //zamienione x i y, obrocony uklad wspolrzednych

            // +367.08,-0.09,+209.58,+87.56,+176.32,+603.28,+0.00,R,A,O
            Matrix<double> u = DenseMatrix.OfArray(new double[,]{
                {608.88},
                {1135.15},
                {367.08},
                {603.28}
            });

            Matrix<double> M = DenseMatrix.OfArray(new double[,]{
                 { odniesienie2.X,odniesienie2.Y, 1, 0 },
                 { -odniesienie2.X, odniesienie2.X, 0, 1},
                 { odniesienie1.X,odniesienie1.Y, 1, 0 }, 
                 { -odniesienie1.Y, odniesienie1.X, 0, 1}
            });

            var Mt = M.Inverse();
            var abcd = Mt.Multiply(u);

            richTextBox1.Text += "\n\n u: " + u.ToString() + " " + "\n\n M:" + M.ToString() + "\n\n Mt:" + Mt.ToString() + "\n\n abcd:" + abcd.ToString();

            //mapowanie
            //x'=ax+by+c
            //y'=bx-ay+d

            double x = 0, y = 0;
            for (int i = 0; i < singleConts.Length; i++)
            {
                x = singleConts[i].center.X;
                y = singleConts[i].center.Y;
                singleConts[i].realCenter.Y = (abcd[0,0]*x + abcd[1,0]*y + abcd[2,0]); //-1
                singleConts[i].realCenter.X = abcd[1,0]*x - abcd[0,0]*y + abcd[3,0];

                Cv2.Circle(wejthresh, singleConts[i].center, 2, new Scalar(0, 0, 255));
                string text = "x: " + Convert.ToString(singleConts[i].realCenter.X) + "y: " + Convert.ToString(singleConts[i].realCenter.Y);
                Cv2.PutText(wejthresh, text, singleConts[i].center, HersheyFonts.HersheySimplex, 0.4, new Scalar(50, 50, 255));

            }


            //sprawdzenie obliczen
            x = 65;
            y = 52;

            double xreal, yreal;

            xreal = (abcd[0, 0] * x + abcd[1, 0] * y + abcd[2, 0])*-1; //-1
            yreal = abcd[1, 0] * x - abcd[0, 0] * y + abcd[3, 0];
            string textreal = "realne wspolrzedne odniesiania: x:" + Convert.ToString(xreal) + "y: " + Convert.ToString(yreal);
            richTextBox1.Text += "\n\n\n " + textreal;

            double xcheck, ycheck;

            xcheck = (abcd[0, 0] * xreal + abcd[1, 0] * yreal - abcd[1, 0] * abcd[3,0] - abcd[0, 0] * abcd[2,0]) / (Math.Pow(abcd[0,0],2) + Math.Pow(abcd[1,0],2));
            ycheck = (abcd[1,0] * xreal - abcd[0, 0] * yreal - abcd[1,0] * abcd[2,0] + abcd[0, 0] * abcd[3,0]) / (Math.Pow(abcd[0, 0],2) + Math.Pow(abcd[1,0],2));
            textreal += "obliczenie  x: " + Convert.ToString(xcheck) + "y: " + Convert.ToString(ycheck);
            richTextBox1.Text += "\n\n\n " + textreal;

            //wyswietlenie obrazu ze zmianami
            Bitmap bitimg = Vision.MatToBitmap(wejthresh);
            pictureBox1.Image = bitimg;


        }


        //add tool masks on the image !
        private void button20_Click(object sender, EventArgs e)
        {
            Mat imageClone = wej.Clone();


            //zmienic na globalne !!!!!!!! ------------------------------------------------------------------------------------------------------
            (toolUpSingleContours, toolDownSingleContours, imageClone)=Vision.ToolPoints(imageClone, singleConts, ppm);

            richTextBox1.Text = "info :";
            //PETLA ilosc konturow
            for (int i= 0; i < singleConts.Length; i++)
            {
                //wysiwetlenie masek 
                Cv2.DrawContours(imageClone, toolUpSingleContours, i, new Scalar(255, 0, 0), 1);
                Cv2.PutText(imageClone, Convert.ToString(i), toolUpSingleContours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255), 1);
                Cv2.DrawContours(imageClone, toolDownSingleContours, i, new Scalar(0, 255, 0), 1);
                Cv2.PutText(imageClone, Convert.ToString(i), toolDownSingleContours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255), 1);

                //PETLA ILOSC punktow konturu
                for (int j = 0; j < singleConts[i].contour.Length; j++)
                {
                    //punkty konturu
                    OpenCvSharp.Point2f temp;
                    temp.X = singleConts[i].contour[j].X;
                    temp.Y = singleConts[i].contour[j].Y;

                    Cv2.Circle(imageClone, singleConts[i].contour[j], 1, new Scalar(0, 0, 255 - 10 * i), 1);
                }

                richTextBox1.Text += "\n";
            }
            Bitmap bitimg = Vision.MatToBitmap(imageClone);
            pictureBox1.Image = bitimg;
        }


        //check for collison 
        private void button18_Click(object sender, EventArgs e)
        {
            Mat imageClone = wej.Clone();

            //check if point is inside of the contour
            //Cv2.PointPolygonTest(,)

            int[][] collisions =  Vision.GetSingleCollisions(singleConts, toolUpSingleContours, toolDownSingleContours);

            richTextBox1.Text = "info : ";
            for (int i = 0; i < collisions.Length; i++)
            {

                for (int j = 0; j < collisions[i].Length; j++)
                {
                    richTextBox1.Text += "\n kolizja z konturem: " + Convert.ToString(i) + " konturu :" + Convert.ToString(collisions[i][j]);
                    singleConts[i].collisions = new int[collisions[i].Length];
                    singleConts[i].collisions[j] = collisions[i][j];
                    richTextBox1.Text += "\n kolizja z konturem: " + Convert.ToString(i) + " konturu :" + Convert.ToString(singleConts[i].collisions[j]);

                }

            }

            Bitmap bitimg = Vision.MatToBitmap(imageClone);
            pictureBox1.Image = bitimg;

        }



        //clear single conts
        private void button21_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "\n zabranie pojedynczych konturow: ";


            for (int i =0;i < singleConts.Length; i ++)
            {
                
                if(singleConts[i].collisions == null)
                {

                    richTextBox1.Text += "\n --" + Convert.ToString(i) + " --" + Convert.ToString(singleConts[i].center) + " ---  " + Convert.ToString(singleConts[i].angle) + " ---  ";


                }

            }

        }


        //wyslanie pozycji robienia zdjecia 

        // +398.22,0,+628.68,0,+176.32,+947.41,+0.00,R,A

        private void button26_Click(object sender, EventArgs e)
        {
            string message = "10 SP 5";
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            richTextBox1.Text += "\n" + String.Join(" ", bytes) + "\n" + str + "\n";
            port.Write(bytes, 0, bytes.Length);

            //wyslanie programu 
            // old   +416.91,+0.00,+595.31,+0.00,+177.50,+932.89,+0.00,R,A
            message = "20 MP +398.22,0,+628.68,0,+176.32,+947.41,+0.00,R,A";
            message += '\r';
            bytes = Encoding.Default.GetBytes(message);
            str = Encoding.Default.GetString(bytes);
            richTextBox1.Text += "\n" + String.Join(" ", bytes) + "\n" + str + "\n";
            port.Write(bytes, 0, bytes.Length);


            message = "30 GC";
            message += '\r';
            bytes = Encoding.Default.GetBytes(message);
            str = Encoding.Default.GetString(bytes);
            richTextBox1.Text += "\n" + String.Join(" ", bytes) + "\n" + str + "\n";
            port.Write(bytes, 0, bytes.Length);

        }








        //Calibrate - kalibracja chessboard w celu wyznaczenia camera matrix
        private void button22_Click_1(object sender, EventArgs e)
        {

            //kalibracja
            
            Mat calibrationImg = new Mat();
            Mat gray = new Mat();

            OpenCvSharp.Point2f[] cor;

            int PAT_ROW = 9, PAT_COL = 6;

            int PAT_SIZE = PAT_ROW * PAT_COL;



            List<List<Point3f>> objectPoints = new List<List<Point3f>>();
            List<List<Point2f>> imgPoints = new List<List<Point2f>>();



            TermCriteria criteria = new TermCriteria(CriteriaType.MaxIter | CriteriaType.Eps, 30, 0.001);

            //przygotowanie macierzy
            var objp = np.zeros((PAT_SIZE, 3), np.float32);
            (var v1, var v2) = np.mgrid(np.arange(0, PAT_ROW), np.arange(0, PAT_COL));
            var v3 = np.dstack(v1, v2);
            objp[$":,:2"] = v3.reshape(-1, 2);

            var objpl = NDArrayToList(objp);

            var size = new OpenCvSharp.Size(1280, 720);



            for (int i=1;i < 20;i++)
            {
                calibrationImg = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\kalibracja\c" + Convert.ToString(i) + ".jpg");
                Cv2.CvtColor(calibrationImg, gray, ColorConversionCodes.BGR2GRAY);

                bool flag = Cv2.FindChessboardCorners(gray, new OpenCvSharp.Size(9,6),out cor);

                if(flag == true)
                {
                    objectPoints.Add(objpl);
                    var cor2 = Cv2.CornerSubPix(gray, cor, new OpenCvSharp.Size(6,6), new OpenCvSharp.Size(-1, -1),criteria);
                    Cv2.DrawChessboardCorners(calibrationImg, new OpenCvSharp.Size(9,6), cor2, flag);

                    if (cor2?.Length > 0)
                    {
                        imgPoints.Add(cor2.ToList());
                    }
                    else
                    {
                        imgPoints.Add(cor.ToList());
                    }
                }

            }


            double[,] cameraMatrix = { 
                {1218.120 ,0,668.99},
                {0 , 1283.9 , 444.09},
                {0,0,1}
            };


            double[] distCoff = {-0.094 , 0.087 , 0 , 0 ,0};

            OpenCvSharp.Vec3d[] tvecs = new Vec3d[19];
            tvecs[0] = new Vec3d(-10.0824417572178, 117.358766750977, 372.901609363185);
            tvecs[1] = new Vec3d(10.7486524587841, 121.650000478951, 384.761304756076);
            tvecs[2] = new Vec3d(120.875461318018, 65.5494231716320,338.240465902101);
            tvecs[3] = new Vec3d(112.967892698472,   73.8838527648858,    342.631847636279);
            tvecs[4] = new Vec3d(97.1534566751570,   86.5839899680321,    337.016831983972);
            tvecs[5] = new Vec3d(111.642957950928,   77.3986496368319,    323.203143567271);
            tvecs[6] = new Vec3d(76.1615958249237, - 78.4929922313541,   449.736718278977);
            tvecs[7] = new Vec3d(-87.3181762752908, - 41.7669669908886,   452.397585336596);
            tvecs[8] = new Vec3d(-112.671876913968, - 56.2080873262029,   320.610874964630);
            tvecs[9] = new Vec3d(-119.915789078284, - 61.8911369611310,   332.307057088681);
            tvecs[10] = new Vec3d(-78.7867350470808, - 55.4778531341788,   269.232106744094);
            tvecs[11] = new Vec3d(-75.9115694650109, - 54.4167215463782,   269.442251157451);
            tvecs[12] = new Vec3d(-79.3390158450569, - 57.2190421904105,   264.649355742233);
            tvecs[13] = new Vec3d(-82.2688041305708, - 61.4898039587820,   255.810026647890);
            tvecs[14] = new Vec3d(-75.1673612297411, - 57.7203522304048,   255.170097094128);
            tvecs[15] = new Vec3d(0.465188598174304, - 83.7588640989477,   450.848004286819);
            tvecs[16] = new Vec3d(36.0647582068929, - 78.4179567757368,   449.749533798997);
            tvecs[17] = new Vec3d(118.051344067450,  4.47091005496856,    445.398412920116);
            tvecs[18] = new Vec3d(123.760100231838, 2.61515202789507, 344.459799198286);

            OpenCvSharp.Vec3d[] rvecs = new Vec3d[19];
            rvecs[0] = new Vec3d(0.228043063865584, - 0.000882190495175474, - 1.56095811120915);
            rvecs[1] = new Vec3d(0.0576285321290564, 0.287549896591034, - 1.59428527112673);
            rvecs[2] = new Vec3d(-0.0974185891566731, - 0.382158001637208,  3.06282510805512);
            rvecs[3] = new Vec3d(-0.0482220514451975, - 0.110972405915712,  3.02425445633030);
            rvecs[4] = new Vec3d(-0.0414948410784172, - 0.140989941809161,  3.06838280161545);
            rvecs[5] = new Vec3d(-0.0439213047724225, - 0.128202471971795,  3.03545534236444);
            rvecs[6] = new Vec3d(-0.00587154057067819,   0.0427134413631573,  1.56738068559567);
            rvecs[7] = new Vec3d(-0.0329550367567077,    0.0250233991559446,  0.0295424575597349);
            rvecs[8] = new Vec3d(0.0530804253294322, 0.231716852891827, - 0.0263112913508322);
            rvecs[9] = new Vec3d(0.0876988989144098, 0.219169010582311, - 0.0101030527285377);
            rvecs[10] = new Vec3d(0.00617282257856472, - 0.239878510432961,  0.00739261372475464);
            rvecs[11] = new Vec3d(0.0109837502059964, - 0.236741908358476,  0.00685501910750658);
            rvecs[12] = new Vec3d(0.0155703368348845, - 0.251455594867367,  0.00222431978869344);
            rvecs[13] = new Vec3d(0.00951279968617727, - 0.239370912462555, - 0.000722250606603262);
            rvecs[14] = new Vec3d(0.0182930654222079, - 0.255635563206102, - 0.0124148103764345);
            rvecs[15] = new Vec3d(-0.0102701329295813,   0.0343581598641856,  0.998473878607217);
            rvecs[16] = new Vec3d(-0.00311513429420818,  0.0366299221661300,  1.31811397959244);
            rvecs[17] = new Vec3d(0.0199682151829022,    0.0481538472395983,  2.37426897311496);
            rvecs[18] = new Vec3d(0.179112868353219, - 0.494030357439673,  2.23756935447818);

            richTextBox1.Text = "camera matrix, " + Convert.ToString(cameraMatrix) + "\n\n imgp : " + Convert.ToString(imgPoints);
            Mat imgOut = new Mat();
            Cv2.Undistort(wej,imgOut,InputArray.Create(cameraMatrix),InputArray.Create(distCoff),null);

            wej = imgOut;

            Bitmap bitimg = Vision.MatToBitmap(wej);
            pictureBox1.Image = bitimg;

        }

        List<Point3f> NDArrayToList(NDArray array)
        {
            if (array == null) return null;

            var length = array.shape[0];

            List<Point3f> target = new List<Point3f>();
            for(var i =0; i< length;i++)
            {
                var item1 = (float)array[i][0];
                var item2 = (float)array[i][0];
                var item3 = (float)array[i][0];

                var point3f = new Point3f() { X = item1, Y = item2, Z = item3 };
                target.Add(point3f);
            }
            return target;
        }






        //wyslanie pozycji do robota
        private void button25_Click(object sender, EventArgs e)
        {

            //Metody na wysylanie polecen i tworzenie pozycji
            int nr = 4;
            double x = RoundUp(singleConts[nr].realCenter.X,2);
            double y = RoundUp(singleConts[nr].realCenter.Y,2);
            double angle = RoundUp(singleConts[nr].angle, 2);

            string message = "20 MP " + Convert.ToString(y) + ",0,220," + "0" +  ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            richTextBox1.Text = "\n" + String.Join(" ", bytes) + "\n" + str + "\n" + "\n\n" + "angle : " + Convert.ToString(angle);
            port.Write(bytes, 0, bytes.Length);

            richTextBox1.Text = " single conts nr:" + Convert.ToString(nr) + " x: " + Convert.ToString(singleConts[nr].realCenter.X) + " y: " + Convert.ToString(singleConts[nr].realCenter.Y) +
                "\n\n po round up x: " + Convert.ToString(x) + " y: " + Convert.ToString(y);

            message = "40 GO";
            message += '\r';
            bytes = Encoding.Default.GetBytes(message);
            str = Encoding.Default.GetString(bytes);
            richTextBox1.Text = "\n" + String.Join(" ", bytes) + "\n" + str + "\n" + "\n\n" + "angle : " + Convert.ToString(angle);
            port.Write(bytes, 0, bytes.Length);


            message = "60 MP " + Convert.ToString(y) + ",0,220," + Convert.ToString(angle) + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
            message += '\r';
            bytes = Encoding.Default.GetBytes(message);
            str = Encoding.Default.GetString(bytes);
            richTextBox1.Text = "\n" + String.Join(" ", bytes) + "\n" + str + "\n" + "\n\n" + "angle : " + Convert.ToString(angle);
            port.Write(bytes, 0, bytes.Length);


            message = "70 MP " + Convert.ToString(y) + ",0,209," + Convert.ToString(angle) + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
            message += '\r';
            bytes = Encoding.Default.GetBytes(message);
            str = Encoding.Default.GetString(bytes);
            richTextBox1.Text = "\n" + String.Join(" ", bytes) + "\n" + str + "\n" + "\n\n" + "angle : " + Convert.ToString(angle);
            port.Write(bytes, 0, bytes.Length);


            message = "80 GC";
            message += '\r';
            bytes = Encoding.Default.GetBytes(message);
            str = Encoding.Default.GetString(bytes);
            richTextBox1.Text = "\n" + String.Join(" ", bytes) + "\n" + str + "\n" + "\n\n" + "angle : " + Convert.ToString(angle);
            port.Write(bytes, 0, bytes.Length);

            /*

            message = "80 MP " + Convert.ToString(y) + ",-19.38,230," + Convert.ToString(angle) + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
            message += '\r';
            bytes = Encoding.Default.GetBytes(message);
            str = Encoding.Default.GetString(bytes);
            richTextBox1.Text = "\n" + String.Join(" ", bytes) + "\n" + str + "\n" + "\n\n" + "angle : " + Convert.ToString(angle);
            port.Write(bytes, 0, bytes.Length);

            

            
            message = "90 MP + 434.17,-19.38,+595.31,+0.00,+177.50,+1004.72,+0.00,R,A";
            message += '\r';
            bytes = Encoding.Default.GetBytes(message);
            str = Encoding.Default.GetString(bytes);
            richTextBox1.Text = "\n" + String.Join(" ", bytes) + "\n" + str + "\n" + "\n\n" + "angle : " + Convert.ToString(angle);
            port.Write(bytes, 0, bytes.Length);
            */


        }

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }


        //------------------------------------------------------------------------OKNO program --------------------------------------------------------------------------------
        
        





        //timer event // timer deklarowany przy polaczeniu z com portem
        private void TimerEventProcessor2(Object sender, EventArgs myEventArgs)
        {
            //timer 1 odpowiada za ciagle odczytywanie wiadomosci wysylanych z robota
            //timer 2 sending i reading duzych ilosci kodu, WAZNE eksperymenty z czasem


            //rozwiazanie na case !

            //case 1,2,3 na globalnych zmiennych typo timer2read , timer2send itp !
            
            //sending, reading commands, two way timer !

            
                //jak flaga 1 to definicja pozycji, wysylanie interwalowe, co 500 ms czyli pol sekundy
                string message = "";
                string str = "";
                if (counterTwo < msgTab.Length)
                {
                    message = msgTab[counterTwo];
                    str = Connect.SendData(port, message);
                    richTextBox6.Text += "\n" + str;
                    counterTwo++;
                }
                else
                {
                    counterTwo = 0;
                    timerTwo.Stop();
                }

        }

        //create checking program for all contours          MAX 10 KONTUROW
        private void button3_Click(object sender, EventArgs e)
        {
            //flaga na wykorzystanie timera !!

            try
            {
                string message = "";
                int counter = 10;


                //petla wysylajaca definicje pozycji do programu, musi zaczynac sie od 1 linii i 1 pozycji
                for(int nr = 0; nr < singleConts.Length;nr++)
                {

                    double x = RoundUp(singleConts[nr].realCenter.X, 2);
                    double y = RoundUp(singleConts[nr].realCenter.Y, 2);
                    double angle = RoundUp(singleConts[nr].angle, 2);

                    //pozcyja 20 mm nad klockiem bez kata
                    message = Convert.ToString(nr+1) +  " PD " + Convert.ToString(nr+1)+ "," + Convert.ToString(y) + ",0,220," + "0" + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                    msgTab[nr] = message;
                    richTextBox2.Text += "\n" + msgTab[nr];

                    //pozycja 20 mm nad klockiem z katem obrotu chwytaka
                    message = Convert.ToString(nr + 21) + " PD " + Convert.ToString(nr + 1) + "," + Convert.ToString(y) + ",0,220," + Convert.ToString(angle) + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                    msgTab[counter] = message;
                    richTextBox2.Text += "\n" + msgTab[counter];
                    counter++;

                    //pozcyja podniesienia klocka z katem obrotu
                    message = Convert.ToString(nr + 41) + " PD " + Convert.ToString(nr + 1) + "," + Convert.ToString(y) + ",0,218," + Convert.ToString(angle) + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                    msgTab[counter] = message;
                    richTextBox2.Text += "\n" + msgTab[counter];
                    counter++;
                }
                timerTwo.Start();
            }
            catch(Exception ex)
            {
                richTextBox2.Text = Convert.ToString(ex);
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {

        }





        // ----------------------------------------------------------------------- okno komunikacja -------------------------------------------------------------------
        //enter position number 
        private void button33_Click(object sender, EventArgs e)
        {
            try
            {
                string text = "N ";
                string tbox = textBox7.Text;
                if (Convert.ToInt32(tbox) >= 0 && Convert.ToInt32(tbox) <= 10)
                {
                    text += tbox;
                }
                string str =Connect.N(port,text);
                richTextBox6.Text += "\n" + str;
            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + Convert.ToString(ex);
            }
        }


        //enter command
        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                string message = textBox8.Text;
                message += '\r';
                byte[] bytes = Encoding.Default.GetBytes(message);
                string str = Encoding.Default.GetString(bytes);
                //port.Write(bytes, 0, bytes.Length);
                richTextBox6.Text += "\n" + str;
            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + Convert.ToString(ex);
            }
        }





        //program read button ---------------------------------------------------------NIE DZIALA
        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";
                storeFlag = false;

                for(int i =0;i < 50;i++)
                {
                    message = "LR " + Convert.ToString(i);
                    msgTab[i] = message;
                    richTextBox6.Text += "\n" + msgTab[i];
                }
                timerOne.Start();



            }
            catch(Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }





        //clear program data and positions
        private void button29_Click(object sender, EventArgs e)
        {
            try
            {
                string str = Connect.NW(port);
                richTextBox6.Text += "\n" + str;
            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }


        //stop button
        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                string str = Connect.HLT(port);
                richTextBox6.Text += "\n" + str;
            }
            catch(Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }


        //read actual position
        private void button31_Click(object sender, EventArgs e)
        {
            try
            {
                string str = Connect.WH(port);
                richTextBox6.Text += "\n" + str;
            }
            catch(Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }



        //clear console window
        private void button34_Click(object sender, EventArgs e)
        {
            richTextBox6.Text = "";
        }









        //controls
        //enter position number textbox
        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }
        //consola texbox 
        private void richTextBox6_TextChanged(object sender, EventArgs e)
        {

        }
        //command window textbox
        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
























        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Tab_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }


        //podlad programu
        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }


    }
}
