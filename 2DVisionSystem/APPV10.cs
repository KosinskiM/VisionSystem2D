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
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Clear_version_robotApp
{
    public partial class APPV10 : Form
    {
        //selected videocapture divice
        VideoCapture capture = new VideoCapture();
        VideoCapture capture2 = new VideoCapture();

        //Camera parameters
        //Camera parameters
        double[,] cameraMatrix = {
                {1218.120 ,0,668.99},
                {0 , 1283.9 , 444.09},
                {0,0,1}
            };

        double[] distCoff = { -0.094, 0.087, 0, 0, 0 };

        double ppm; //pixel per metrix parameter 

        //obiekty mat przechowujące zdjecia
        Mat wej = new Mat();
        Mat wejCopy = new Mat();


        //zbior obiektów
        ContourOb[] singleConts;
        ContourOb[] connectedConts;

        //----TIMERY

        //timer 1 aplikacji
        //-live preview, lapanie klatek
        static System.Timers.Timer timerOne = new System.Timers.Timer();

        //timer 2 portu rs232 odbieranie
        static System.Timers.Timer timerTwo = new System.Timers.Timer();
        static bool storeFlag;
        static string[] storedMsg;

        //nowe zmienne globalne
        int flag = 0;
        string ansWH = "";

        //timer 3 portu rs232 wysylanie
        static System.Timers.Timer timerThree = new System.Timers.Timer();
        int counterThree;

        //asynchroniczne timery
        /*
        //timer 4 portu rs232 odbieranie programu;
        static System.Timers.Timer timerFour = new System.Timers.Timer();

        //timer 5 portu rs232 odbieranie programu;
        static System.Timers.Timer timer5 = new System.Timers.Timer();
        //timer 6 portu rs232 odbieranie programu;
        static System.Timers.Timer timer6 = new System.Timers.Timer();
        */

        //synchroniczne
        //timer 4 portu rs232 odbieranie programu;
        static System.Timers.Timer timerFour = new System.Timers.Timer();
        //timer 5 portu rs232 odbieranie programu;
        static System.Timers.Timer timer5 = new System.Timers.Timer();
        //timer 6 portu rs232 odbieranie programu;
        static System.Timers.Timer timer6 = new System.Timers.Timer();


        //Zapisane kontury globalnie
        OpenCvSharp.Point[][] contours;     //kontury znalezione
        OpenCvSharp.Point[][] smallContours;    //posortowane obiekty
        OpenCvSharp.Point[][] connectedContours;    //posortowane zlaczone obiekty 
        OpenCvSharp.Point[][] toolUpSingleContours;     //maska narzedzia gorna
        OpenCvSharp.Point[][] toolDownSingleContours;       //maska narzedzia dolna

        //aruco markers
        OpenCvSharp.Point2f[][] MarkerOut = new OpenCvSharp.Point2f[10][];
        int[] MarkerId;




        //console writer counter
        int console7counter;


        //RS232 connection
        static SerialPort port;

        //odczyt 50 linii programu
        static string[] programTab = new string[50];
        static string[] msgTab = new string[50];

        public APPV10()
        {
            InitializeComponent();
        }

        //zdarzenie pierwszego wlaczenia programu
        private void APPV10_Load(object sender, EventArgs e)
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
                comboBox1.Items.Add(cameraNames[i]);
            }


            //zaladowanie dostepnych portow
            if (ports.Length != 0)
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    comboBox2.Items.Add(ports[i]);
                }
            }
            else
            {
                comboBox2.Items.Add("none");
            }


            Mat wejscie = new Mat();
            wejscie = Cv2.ImRead(@"C:\Users\MKPC\Desktop\2D vision system\sample\start.png", ImreadModes.Unchanged);
            pictureBox1.Image = VISION.MatToBitmap(wejscie);



            //timer 5 live preview
            timer5.Elapsed += TimerEventProcessor5;
            timer5.Interval = 50;
        }

        //------------------------------------------------------------------------------tabHOME------------------------------------------

        //default options button home screen
        private void DEFAULT_button_Click(object sender, EventArgs e)
        {
            textBox1.Text = "55";
            textBox2.Text = "255";

            textBox3.Text = "1900";
            textBox4.Text = "4400";
            textBox5.Text = "12000";

            //red
            textBox9.Text = "0";        //ll
            textBox10.Text = "15";      //lh
            textBox12.Text = "170";     //hl
            textBox11.Text = "250";     //hh

            //green
            textBox14.Text = "40";
            textBox13.Text = "115";

            //blue
            textBox16.Text = "115";
            textBox15.Text = "170";
        }


        //textbox opis całej aplikacji - brak interakcji !!!
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //reset button, zwolnienie wszystkich zasobow aplikacji !!!!!------------------------------------------- PUSTO
        private void button1_Click(object sender, EventArgs e)
        {

        }

        //zdjecie wejsciowe
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        //combobox camery!
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            capture.Open(0, comboBox1.SelectedIndex);
            capture.Set(CaptureProperty.FrameWidth, 1280);
            capture.Set(CaptureProperty.FrameHeight, 720);

            //Setup picturebox size
            //pictureBox2.Height = 720;
            //pictureBox2.Width = 1280;

        }

        //disconnect camera
        private void button26_Click(object sender, EventArgs e)
        {
            capture.Release();
        }

        //combobox comporty
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //connect to com port
        private void button36_Click(object sender, EventArgs e)
        {
            try
            {
                // Instantiate the communications
                // port with some basic settings
                port = new SerialPort("COM5", 9600, Parity.Even, 8, StopBits.Two);
                port.Handshake = Handshake.None;
                port.ReadTimeout = 500;
                port.WriteTimeout = 500;
                // Open the port for communications
                port.Open();

                //po otwarciu portu odpalenie timera 2
                timerTwo.Elapsed += TimerEventProcessor2;
                timerTwo.Interval = 500;
                storeFlag = false;
                flag = 0;
                timerTwo.Start();

                //po otwarciu portu odpalenie timera 3
                timerThree.Elapsed += TimerEventProcessor3;
                timerThree.Interval = 500;

                bool state = port.IsOpen;
                richTextBox6.Text += "\n port communication open ? : " + Convert.ToString(state);


                richTextBox4.Text = "NO PROGRAM SELECTED";
            }
            catch (Exception ex)
            {
                richTextBox6.Text += Convert.ToString(ex);
            }
        }


        //GO TO HOME POSITION
        private async void button37_Click(object sender, EventArgs e)
        {
            //zatrzymanie odczytu do okna rs232 connection
            timerTwo.Stop();

            await goImgPosAsync();
            await Task.WhenAll();
            timerTwo.Stop();
        }

        private async Task goImgPosAsync()
        {

            //wybranie programu 9 sterownika
            CONNECT.N(port, "9");
            await Task.Delay(500);
            richTextBox3.BeginInvoke((MethodInvoker)delegate ()
            {
                richTextBox3.Text += "\n" + "wybranie programu: N 9";
            });

            //wyczyszczenie programu
            CONNECT.NW(port);
            await Task.Delay(500);
            richTextBox3.BeginInvoke((MethodInvoker)delegate ()
            {
                richTextBox3.Text += "\n" + "wyczyszczenie programu: NW 9";
            });

            //ustawienie predkosci
            CONNECT.SendData(port, "10 SP 6");
            await Task.Delay(500);
            richTextBox3.BeginInvoke((MethodInvoker)delegate ()
            {
                richTextBox3.Text += "\n" + "program line [10]: 10 SP 6";
            });

            //sprawdzenie czy chwytak jest na bezpiecznej wysokosci
            bool safeHeight = await GripperSafeHeightAsync();
            bool zeroRotation = await GripperInZeroAsync();
            Task.WaitAll();

            //zamkniecie chwytaka na koniec ruchu
            CONNECT.SendData(port, "100 GC");
            await Task.Delay(500);
            richTextBox3.BeginInvoke((MethodInvoker)delegate ()
            {
                richTextBox3.Text += "\n" + "program line [100]: 100 GC";
            });

            //zkonczenie programu
            CONNECT.SendData(port, "110 ED");
            await Task.Delay(500);
            richTextBox3.BeginInvoke((MethodInvoker)delegate ()
            {
                richTextBox3.Text += "\n" + "program line [110]: ED";
            });

            //wystartowanie programu
            CONNECT.SendData(port, "RN");
            await Task.Delay(500);
            richTextBox3.BeginInvoke((MethodInvoker)delegate ()
            {
                richTextBox3.Text += "\n" + "komenda: RUN";
            });
            //-----------------------------------------------------------------------------do metody
            await Task.Delay(2000);

            //dopiero ruch do pozycji zdjecia wczesniej to bylo zabezpieczenie

            //koniec przeorientowania do pozycji bezpiecznej


            bool wynik = await checkSafePositionAsync();
            await Task.WhenAll();
            //w zaleznosci od wyniku pozycyjonowania rozgalezienie programu
            if (wynik == true)
            {
                //wyczyszczenie programu
                CONNECT.NW(port);
                await Task.Delay(200);
                richTextBox3.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox3.Text = "\n" + "wyczyszczenie programu 9: NW";
                });

                //ustawienie predkosci
                CONNECT.SendData(port, "10 SP 6");
                await Task.Delay(200);
                richTextBox3.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox3.Text = "\n" + "program line [10]: 10 SP 6";
                });

                //pozycja zdjecia : +380.11,+0.00,+613.27,+0.00,+176.32,+938.94,+0.00,R,A,C
                //nowa : +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A,C

                //przeorientowanie do pozycji robienia zdjecia
                CONNECT.SendData(port, "20 MP +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");
                await Task.Delay(200);
                richTextBox3.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox3.Text = "\n" + "program line [20]: 20 MP +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A";
                });

                //zamkniecie chwytaka
                CONNECT.SendData(port, "21 TI 5");
                await Task.Delay(200);
                richTextBox3.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox3.Text = "\n" + "program line [21]: TI 5";
                });

                //zamkniecie chwytaka
                CONNECT.SendData(port, "GC");
                await Task.Delay(200);
                richTextBox3.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox3.Text = "\n" + "komenda: GC";
                });

                //odpalenie programu
                CONNECT.SendData(port, "RN");
                await Task.Delay(200);
                richTextBox3.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox3.Text = "\n" + "komenda: RUN";
                });

            }
            else
            {
                AsyncConsoleWriter("BLAD POZYCJONOWANIA");
                return;
            }



        }






        //-------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------tabSETUP----------------------------------------------------


        //reset preview
        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            contours = null;
            smallContours = null;
            connectedContours = null;
            wej.Dispose();
        }

        //default options setup window
        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = "55";
            textBox2.Text = "255";

            textBox3.Text = "1800";
            textBox4.Text = "4400";
            textBox5.Text = "12000";

            //red
            textBox9.Text = "0";        //ll
            textBox10.Text = "15";      //lh
            textBox12.Text = "170";     //hl
            textBox11.Text = "250";     //hh

            //green
            textBox14.Text = "40";
            textBox13.Text = "115";

            //blue
            textBox16.Text = "115";
            textBox15.Text = "170";
        }


        //combobox sample preview
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Mat undistorted = new Mat();
                string photo = comboBox3.Items[comboBox3.SelectedIndex].ToString();

                switch (photo)
                {
                    case "sample photo 1":
                        {
                            wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\test1.jpg", ImreadModes.Unchanged);
                        }
                        break;

                    case "sample photo 2":
                        {
                            wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\sample2.jpg", ImreadModes.Unchanged);
                        }
                        break;
                    case "sample photo 3":
                        {
                            wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\sample3.jpg", ImreadModes.Unchanged);
                        }
                        break;
                    case "sample photo 4":
                        {
                            wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\sample4.jpg", ImreadModes.Unchanged);
                        }
                        break;
                }
                Cv2.Undistort(wej, undistorted, InputArray.Create(cameraMatrix), InputArray.Create(distCoff), null);
                wej = undistorted;
                //show
                pictureBox2.Image = VISION.MatToBitmap(wej);
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n" + ex;
            }
        }


        //-----------------LIVE-------------
        private void TimerEventProcessor1(Object sender, EventArgs myEventArgs)
        {
            try
            {
                
                Mat klon = new Mat();
                Mat undistorted = new Mat();

                if (capture.IsOpened() == false)
                {
                    richTextBox6.BeginInvoke((MethodInvoker)delegate ()
                    {
                        richTextBox3.Text = "ERROR nie znaleziono kamery";
                        richTextBox3.ScrollToCaret();
                    });
                    timerOne.Stop();
                }
                else
                {
                    capture.Read(klon);
                }
                Cv2.Undistort(klon, undistorted, InputArray.Create(cameraMatrix), InputArray.Create(distCoff), null);
                Image oldImg = pictureBox2.Image;
                pictureBox2.Image = VISION.MatToBitmap(undistorted);
                if (oldImg != null)
                {
                    oldImg.Dispose();
                }
                klon.Dispose();
                undistorted.Dispose();

            }
            catch (Exception ex)
            {

                richTextBox6.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox3.Text += "\n" + ex;
                    richTextBox3.ScrollToCaret();
                });
            }
        }


        //live start button
        private void LIVEbutton_Click(object sender, EventArgs e)
        {
            try
            {
                timerOne.Elapsed += TimerEventProcessor1;
                timerOne.Interval = 100;
                timerOne.Start();
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n" + ex;
            }
        }

        //live stop button
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                timerOne.Stop();
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n" + ex;
            }
        }
        //---------------LIVE-END------------

        //capture the frame from camera
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                Mat klon = new Mat();
                Mat undistorted = new Mat();
                
                if (capture.IsOpened() == false)
                {
                    richTextBox3.Text = "ERROR nie znaleziono kamery";
                }
                else
                {
                    capture.Read(klon);
                }
              
                Cv2.Undistort(klon, undistorted, InputArray.Create(cameraMatrix), InputArray.Create(distCoff), null);

                //Rect rec = new Rect(20,5,undistorted.Width-20,undistorted.Height-5);
                //OpenCvSharp.OpenCVException: 0 <= roi.x && 0 <= roi.width && roi.x + roi.width <= m.cols && 0 <= roi.y && 0 <= roi.height && roi.y + roi.height <= m.rows
                //Mat cropped = new Mat(undistorted,rec);

                wej = undistorted;

                Image oldImg = pictureBox2.Image;
                pictureBox2.Image = VISION.MatToBitmap(undistorted);


                if (oldImg != null)
                {
                    oldImg.Dispose();
                }
                klon.Dispose();
                //undistorted.Dispose();

            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }
        }


        //undistort img from camera
        private void button22_Click(object sender, EventArgs e)
        {
            Mat undistorted = new Mat();
            Cv2.Undistort(wej, undistorted, InputArray.Create(cameraMatrix), InputArray.Create(distCoff), null);
            wej = undistorted;
            pictureBox2.Image = VISION.MatToBitmap(wej);
        }

        //przetwarzanie obrazu

        //enter treshold to app
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (timerOne.Enabled != true)
                {
                    //clone wejscia i maty
                    Mat original = wej.Clone();
                    Mat refGray = new Mat();
                    Mat gaus = new Mat();
                    Mat thresh = new Mat();
                    //konwersja na odcienie szarosci
                    Cv2.CvtColor(original, refGray, ColorConversionCodes.BGR2GRAY);
                    //blur 
                    OpenCvSharp.Size kernel = new OpenCvSharp.Size(3, 3);
                    //Cv2.GaussianBlur(refGray, gaus, kernel,0,0);
                    Cv2.MorphologyEx(refGray, gaus, MorphTypes.Close, null);
                    //tresh 127 - 255
                    Cv2.Threshold(gaus, thresh, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text), ThresholdTypes.BinaryInv);
                    pictureBox2.Image = VISION.MatToBitmap(thresh);
                }
                else
                {
                    richTextBox3.Text += "\nLIVE PREVIEW is ON, stop it to see changes";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }
        }

        //find objects -- znalezienie wszystkich konturów !
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (timerOne.Enabled != true)
                {
                    OpenCvSharp.Point[][] kontury = null;
                    Mat original = wej.Clone();

                    kontury = VISION.GetContours(wej, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
                    Cv2.DrawContours(original, kontury, -1, new Scalar(0, 0, 255), thickness: 3);

                    double a;
                    int area;
                    for (int i = 0; i < kontury.Length; i++)
                    {
                        Cv2.Circle(original, kontury[i][0], 1, new Scalar(0, 255, 0), 1);
                        a = Cv2.ContourArea(kontury[i]);
                        area = Convert.ToInt32(a);
                        Cv2.PutText(original, Convert.ToString(area), kontury[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);
                    }
                    //zapis konturów globalnie
                    contours = kontury;

                    pictureBox2.Image = VISION.MatToBitmap(original);
                }
                else
                {
                    richTextBox3.Text += "\nLIVE PREVIEW is ON, stop it to see changes";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }
        }

        //sorting object on small and connected
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (timerOne.Enabled != true)
                {
                    Mat imageClone = wej.Clone();
                    int small, connected;

                    OpenCvSharp.Point punkt;
                    OpenCvSharp.Point[][] smallC;
                    OpenCvSharp.Point[][] connectedC;

                    //wyswietlenie konturow zaleznie od objetosci
                    (imageClone, small, connected) = VISION.showContours(imageClone, contours, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text));

                    pictureBox2.Image = VISION.MatToBitmap(imageClone);
                    richTextBox3.Text = "liczba konturów osobnych" + Convert.ToString(small) + "\n" + "liczba konturów polaczonych: " + Convert.ToString(connected) + "\n";

                    //indeksowanie kazdego obiektu o odpowiednim rozmiarze
                    (smallC, connectedC) = VISION.SmallBigContours(contours, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text), small, connected);

                    Cv2.DrawContours(imageClone, smallC, -1, new Scalar(0, 255, 0), thickness: 3);
                    Cv2.DrawContours(imageClone, connectedC, -1, new Scalar(255, 0, 0), thickness: 3);

                    (smallContours, connectedContours) = VISION.LeftSortContours(smallC, connectedC, imageClone);
                    //(sortedSmallContours, sortedConnectedContours) = Vision.LSortContours(smallContours, connectedContours);

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

                    //zaznaczenie konturów
                    Cv2.DrawContours(imageClone, smallContours, -1, new Scalar(0, 255, 0), thickness: 3);
                    Cv2.DrawContours(imageClone, connectedContours, -1, new Scalar(255, 0, 0), thickness: 3);

                    pictureBox2.Image = VISION.MatToBitmap(imageClone);
                }
                else
                {
                    richTextBox3.Text += "\nLIVE PREVIEW is ON, stop it to see changes";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }
        }

        //color recogintion button, checking color parameters
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (timerOne.Enabled != true)
                {
                    Mat obliczenia = wej.Clone();
                    Mat imageClone = wej.Clone();
                    Mat hsv = new Mat();
                    OpenCvSharp.Point center;
                    Vec3b vector;

                    Cv2.CvtColor(imageClone, hsv, ColorConversionCodes.BGR2HSV_FULL);


                    for (int i = 0; i < smallContours.Length; i++)
                    {
                        string color = VISION.ContourColor(obliczenia, smallContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox13.Text), Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                        //momenty
                        var M = Cv2.Moments(smallContours[i], false);
                        center.X = Convert.ToInt32(M.M10 / M.M00);
                        center.Y = Convert.ToInt32(M.M01 / M.M00);

                        vector = hsv.At<Vec3b>(center.Y, center.X);
                        Scalar col = new Scalar(vector.Item0, vector.Item1, vector.Item2);

                        Cv2.Circle(imageClone, center, 3, new Scalar(0, 255, 0), 1);
                        Cv2.PutText(imageClone, color + " " + Convert.ToString(col.Val1) + " " + Convert.ToString(col.Val2) + " " + Convert.ToString(col.Val3) + " ", smallContours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);


                    }
                    pictureBox2.Image = VISION.MatToBitmap(imageClone);
                }
                else
                {
                    richTextBox3.Text += "\nLIVE PREVIEW is ON, stop it to see changes";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }
        }

        //shape recognition
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (timerOne.Enabled != true)
                {
                    string shape;

                    Mat imageClone = wej.Clone();

                    for (int i = 0; i < smallContours.Length; i++)
                    {
                        shape = VISION.GetShape(smallContours[i]);

                        if (shape == "triangle")
                        {
                            Cv2.PutText(imageClone, "triangle", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                        }
                        else if (shape == "square")
                        {
                            Cv2.PutText(imageClone, "square", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                        }
                        else if (shape == "rectangle")
                        {
                            Cv2.PutText(imageClone, "rectangle", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                        }
                        else if (shape == "pentagon")
                        {
                            Cv2.PutText(imageClone, "pentagon", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                        }
                        else if (shape == "circle")
                        {
                            Cv2.PutText(imageClone, "circle", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                        }
                    }

                    pictureBox2.Image = VISION.MatToBitmap(imageClone);
                }
                else
                {
                    richTextBox3.Text += "\nLIVE PREVIEW is ON, stop it to see changes";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }
        }

        //angle recognition
        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                if (timerOne.Enabled != true)
                {
                    int angle = 0;
                    int[] peaks;
                    OpenCvSharp.Point center, mid, temp;
                    int[] corners;

                    //nowa metoda
                    Rect r;
                    float ang;

                    Mat imageClone = wej.Clone();

                    for (int i = 0; i < smallContours.Length; i++)
                    {
                        angle = VISION.GetAngle(imageClone, smallContours[i]);

                        Cv2.PutText(imageClone, Convert.ToString(angle), smallContours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 255), 1);

                    }

                    Bitmap bitimg = VISION.MatToBitmap(imageClone);
                    pictureBox2.Image = bitimg;
                }
                else
                {
                    richTextBox3.Text += "\nLIVE PREVIEW is ON, stop it to see changes";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }
        }

        //Create virtual objects
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                if (timerOne.Enabled != true)
                {
                    //przygotowanie zmiennych
                    Mat imageClone = wej.Clone();

                    string color, shape;
                    int angle;
                    OpenCvSharp.Point c = new OpenCvSharp.Point();
                    OpenCvSharp.Point[] cor;

                    singleConts = new ContourOb[smallContours.Length];
                    connectedConts = new ContourOb[connectedContours.Length];



                    for (int i = 0; i < smallContours.Length; i++)
                    {
                        //center z momentow 
                        c = VISION.GetCenter(smallContours[i]);

                        //kolor
                        color = VISION.ContourColor(imageClone, smallContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text),
                            Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox13.Text),
                            Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                        //shape 
                        shape = VISION.GetShape(smallContours[i]);

                        //angle
                        angle = VISION.GetAngle(imageClone, smallContours[i]);

                        //corners
                        cor = VISION.GetCorners(smallContours[i]);

                        //tworzeni kazdego obiektu
                        singleConts[i] = new ContourOb()
                        {
                            indx = i,        //index 0,1,2,3,4...       
                            center = c,      //srodek konturu
                            angle = angle,   //obrot konturu od osi x
                            color = color,   //kolor konturu
                            shape = shape,   //ksztalt konturu
                            corners = cor,
                            contour = smallContours[i],   //wszystkie punkty konturu
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

                    for (int i = 0; i < connectedContours.Length; i++)
                    {
                        //center z momentow 
                        c = VISION.GetCenter(connectedContours[i]);

                        //kolor
                        color = VISION.ContourColor(imageClone, connectedContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text),
                            Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text),
                            Convert.ToInt32(textBox13.Text), Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                        //shape 
                        shape = VISION.GetShape(connectedContours[i]);

                        //angle
                        angle = VISION.GetAngle(imageClone, connectedContours[i]);

                        //corners
                        cor = VISION.GetCorners(connectedContours[i]);

                        //tworzeni kazdego obiektu
                        connectedConts[i] = new ContourOb()
                        {
                            indx = i,        //index 0,1,2,3,4...       
                            center = c,      //srodek konturu
                            angle = angle,   //obrot konturu od osi x
                            color = color,   //kolor konturu
                            shape = shape,   //ksztalt konturu
                            corners = cor,
                            contour = connectedContours[i],   //wszystkie punkty konturu
                        };

                        //organizacja naroznikow
                        OpenCvSharp.Point temp0, temp1, temp2, temp3;
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
                else
                {
                    richTextBox3.Text += "\nLIVE PREVIEW is ON, stop it to see changes";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }

        }

        //show singleconts
        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                if (timerOne.Enabled != true)
                {
                    Mat imageClone = wej.Clone();

                    richTextBox11.Text += "\n\nSTORED SINGLE OBJECTS";

                    for (int i = 0; i < singleConts.Length; i++)
                    {
                        //indekswoanie na obrazie
                        Cv2.Circle(imageClone, singleConts[i].contour[0], 1, new Scalar(255, 0, 0), 1);

                        //puttext
                        Cv2.PutText(imageClone, Convert.ToString(singleConts[i].indx), singleConts[i].contour[0], HersheyFonts.HersheySimplex, 1, new Scalar(0, 255, 0), 1);

                        //opisy
                        richTextBox11.Text += "\n" + Convert.ToString(singleConts[i].indx) + "\n";
                        richTextBox11.Text += "center:  " + Convert.ToString(singleConts[i].center) + "\n";
                        richTextBox11.Text += "angle:   " + Convert.ToString(singleConts[i].angle) + "\n";
                        richTextBox11.Text += "color:   " + Convert.ToString(singleConts[i].color) + "\n";
                        richTextBox11.Text += "shape:   " + Convert.ToString(singleConts[i].shape) + "\n";
                    }
                    //zaznaczenie konturów
                    Cv2.DrawContours(imageClone, smallContours, -1, new Scalar(0, 255, 0), thickness: 3);
                    pictureBox2.Image = VISION.MatToBitmap(imageClone);
                }
                else
                {
                    richTextBox3.Text += "\nLIVE PREVIEW is ON, stop it to see changes";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }

        }

        //show connected objects
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                if (timerOne.Enabled != true)
                {
                    Mat imageClone = wej.Clone();

                    richTextBox10.Text += "\n\nSTORED CONNECTED OBJECTS";

                    for (int i = 0; i < connectedConts.Length; i++)
                    {
                        //indekswoanie na obrazie
                        Cv2.Circle(imageClone, connectedConts[i].contour[0], 1, new Scalar(255, 0, 0), 1);

                        //puttext
                        Cv2.PutText(imageClone, Convert.ToString(connectedConts[i].indx), connectedConts[i].contour[0], HersheyFonts.HersheySimplex, 1, new Scalar(0, 255, 0), 1);

                        //opisy
                        richTextBox3.Text += "\n" + Convert.ToString(connectedConts[i].indx) + "\n";
                        richTextBox3.Text += "center:  " + Convert.ToString(connectedConts[i].center) + "\n";
                        richTextBox3.Text += "angle:   " + Convert.ToString(connectedConts[i].angle) + "\n";
                        richTextBox3.Text += "color:   " + Convert.ToString(connectedConts[i].color) + "\n";
                        richTextBox3.Text += "shape:   " + Convert.ToString(connectedConts[i].shape) + "\n";
                    }

                    Cv2.DrawContours(imageClone, connectedContours, -1, new Scalar(255, 0, 0), thickness: 3);
                    pictureBox2.Image = VISION.MatToBitmap(imageClone);
                }
                else
                {
                    richTextBox3.Text += "\nLIVE PREVIEW is ON, stop it to see changes";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }
        }

        //pomiar
        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                if (timerOne.Enabled != true)
                {

                    //nowa pozycja zdjecia
                    //+380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A,C


                    //ppm = 2.00919589996338;


                    //1. pixel permetric
                    //reset
                    Mat wejthresh = wej.Clone();
                    RotatedRect rot;
                    Matrix<double> abcd;

                    //find markers on image
                    (MarkerOut, MarkerId) = VISION.findArucoMarkers(wejthresh, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));

                    //to zostaje
                    if (MarkerId.Length == 2)
                    {
                        OpenCvSharp.Aruco.CvAruco.DrawDetectedMarkers(wejthresh, MarkerOut, MarkerId, new Scalar(255, 0, 0));

                        richTextBox3.Text += "\n>??\n\n " + "X1: " + Convert.ToString(MarkerOut[0][0].X) + "\nY1 = " + Convert.ToString(MarkerOut[0][0].Y) + "\n";
                        richTextBox3.Text += "\n>??\n\n " + "X2: " + Convert.ToString(MarkerOut[1][0].X) + "\nY2 = " + Convert.ToString(MarkerOut[1][0].Y) + "\n";
                    }
                    else
                    {
                        richTextBox3.Text += "\n\n Na zdjeciu nie wykryto dwóch markerów ktore są niezbędne do obliczeń\n\n liczba wykrytych markerów to: "
                            + Convert.ToString(MarkerId);
                    }

                    //Macierz transforamcji ukladow wsplorzednych
                    (abcd, ppm) = VISION.coordinatesABCD(MarkerOut, MarkerId);

                    richTextBox3.Text += "\n\nppm : " + Convert.ToString(ppm);

                    //wspolrzedne srodka obietu we wspolrzednych robota
                    (singleConts, connectedConts) = VISION.mapCoordinates(singleConts, connectedConts, abcd);

                    for (int i = 0; i < singleConts.Length; i++)
                    {
                        Cv2.Circle(wejthresh, singleConts[i].center, 2, new Scalar(0, 255, 0));
                        Cv2.PutText(wejthresh, Convert.ToString(singleConts[i].realCenter), singleConts[i].center, HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255));
                    }
                    for (int i = 0; i < connectedConts.Length; i++)
                    {
                        Cv2.Circle(wejthresh, connectedConts[i].center, 2, new Scalar(0, 255, 0));
                        Cv2.PutText(wejthresh, Convert.ToString(connectedConts[i].realCenter), connectedConts[i].center, HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255));
                    }

                    //wyswietlenie obrazu ze zmianami        
                    pictureBox2.Image = VISION.MatToBitmap(wejthresh);
                    
                }
                else
                {
                    richTextBox3.Text += "\nLIVE PREVIEW is ON, stop it to see changes";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }
        }

        //tool masks
        private void button20_Click(object sender, EventArgs e)
        {
            try
            {
                if (timerOne.Enabled != true)
                {
                    Mat imageClone = wej.Clone();


                    (toolUpSingleContours, toolDownSingleContours, imageClone) = VISION.ToolPoints(imageClone, singleConts, ppm);

                    //PETLA ilosc konturow
                    for (int i = 0; i < singleConts.Length; i++)
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
                    }
                    pictureBox2.Image = VISION.MatToBitmap(imageClone);

                }
                else
                {
                    richTextBox3.Text += "\nLIVE PREVIEW is ON, stop it to see changes";
                }
            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }
        }

        //check for collisions
        private void button18_Click(object sender, EventArgs e)
        {
            int[][] collisions = VISION.GetSingleCollisions(singleConts, toolUpSingleContours, toolDownSingleContours);

            richTextBox10.Text = "info : ";
            for (int i = 0; i < collisions.Length; i++)
            {

                for (int j = 0; j < collisions[i].Length; j++)
                {
                    if (collisions[i][j] != 1000)
                    {
                        singleConts[i].collisions = new int[collisions[i].Length];
                        singleConts[i].collisions[j] = collisions[i][j];
                        richTextBox10.Text += "\n kolizja przy podnoszeniu obiektu: " + Convert.ToString(i) + " z obiektem:  " + Convert.ToString(singleConts[i].collisions[j]);
                    }
                }

            }
        }

        //test choosing algorithm czyli test poprawnego sortowania obiektow
        private void button30_Click(object sender, EventArgs e)
        {



            /*
            Mat imageClone = wej.Clone();
            (toolUpSingleContours, toolDownSingleContours, imageClone) = VISION.ToolPoints(imageClone, connectedConts, ppm);

            //PETLA ilosc konturow
            for (int i = 0; i < singleConts.Length; i++)
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
            }
            pictureBox2.Image = VISION.MatToBitmap(imageClone);

            */
        }








        //klikniecie na tab1
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


        //lower treshold texbox input
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        //upper trshold texbox input
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        //lower object volume
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        //upper object volume
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        //connected bojects volume
        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        //pictureboxclick on it
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        //---------------------------------------------------------------------------------------------------------------------------------







        //---------------------------------------------------RS232 COONNECTION------------------------------------------------------------


        //connect button
        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                // Instantiate the communications
                // port with some basic settings
                port = new SerialPort("COM5", 9600, Parity.Even, 8, StopBits.Two);
                port.Handshake = Handshake.None;
                port.ReadTimeout = 500;
                port.WriteTimeout = 500;
                // Open the port for communications
                port.Open();

                //po otwarciu portu odpalenie timera 2
                timerTwo.Elapsed += TimerEventProcessor2;
                timerTwo.Interval = 500;
                storeFlag = false;
                flag = 0;
                timerTwo.Start();

                //po otwarciu portu odpalenie timera 3
                timerThree.Elapsed += TimerEventProcessor3;
                timerThree.Interval = 500;

                bool state = port.IsOpen;
                richTextBox6.Text += "\n port communication open ? : " + Convert.ToString(state);


                richTextBox4.Text = "NO PROGRAM SELECTED";
            }
            catch (Exception ex)
            {
                richTextBox6.Text += Convert.ToString(ex);
            }
        }



        //timer ciaglego odczytu z portu komunikacyjnego
        private void TimerEventProcessor2(Object sender, EventArgs myEventArgs)
        {
            //String RecievedData = "dzialam w tle" + '\r';
            String RecievedData = port.ReadExisting();
            //wypisanie odbiernaych informacji do console window

            if (RecievedData != "")
            {

                //wybor flagi
                switch (flag)
                {

                    //odbieranie ciagłe przy pracy aplikacji
                    case 0:

                        richTextBox6.BeginInvoke((MethodInvoker)delegate ()
                        {
                            richTextBox6.Text += "\n" + RecievedData;
                            richTextBox6.ScrollToCaret();
                        });

                        break;


                    //odczyt programu do okna program window
                    case 1:

                        richTextBox5.BeginInvoke((MethodInvoker)delegate ()
                        {
                            richTextBox5.Text += "\n" + Convert.ToString(counterThree) + RecievedData;
                            richTextBox5.ScrollToCaret();
                        });

                        break;




                }
            }
        }


        //timer 3 wysylania do portu komunikacyjnego
        private void TimerEventProcessor3(Object sender, EventArgs myEventArgs)
        {
            string message = "";
            string str = "";
            if (counterThree < programTab.Length)
            {
                message = programTab[counterThree];
                str = CONNECT.SendData(port, message);

                richTextBox6.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox6.Text += "\nwyslano: " + str;
                    richTextBox6.ScrollToCaret();
                });
                counterThree++;
            }
            else
            {
                counterThree = 0;
                flag = 0;
                programTab = CONNECT.programTabClear(programTab);
                timerThree.Stop();
            }
        }

        //program read from controller - send and recive
        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "";

                for (int i = 1; i < 50; i++)
                {
                    message = "LR " + Convert.ToString(i);
                    programTab[i] = message;
                }

                //wyslanie wiadomosci
                counterThree = 0;
                flag = 1;
                timerThree.Start();


            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }



        //disconect button
        private void button24_Click(object sender, EventArgs e)
        {
            try
            {
                // Close the port and dispose
                port.Close();
                port.Dispose();

                timerTwo.Stop();
                timerThree.Stop();
                if (port.IsOpen == false)
                {
                    richTextBox6.Text += "DISCONNECTED";
                }
            }
            catch (Exception ex)
            {
                richTextBox6.Text += Convert.ToString(ex);
            }
        }


        //enter command
        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                string message = textBox8.Text;

                string str = CONNECT.SendData(port, message);
                richTextBox6.Text += "wyslana komenda: " + str;
            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + Convert.ToString(ex);
            }
        }


        //delete program and positions
        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                string str = CONNECT.NW(port);
                richTextBox6.Text += "wyslana komenda: " + str;
            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }

        //RUN button
        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                string str = CONNECT.RN(port, textBox6.Text, textBox17.Text, textBox18.Text);
                richTextBox6.Text += "wyslana komenda: " + str;
            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }


        //read selected program
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                string str = CONNECT.QN(port);
                richTextBox6.Text += "\nwyslana komenda: " + str;
            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }


        //select program
        private void button33_Click(object sender, EventArgs e)
        {
            try
            {
                string str = CONNECT.N(port, textBox7.Text);
                richTextBox6.Text += "wyslana komenda: " + str;
                richTextBox4.Text = "Selected program:\n\n" + str;
            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }


        //stop robot movement button
        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                string str = CONNECT.HLT(port);
                richTextBox6.Text += "wyslana komenda: " + str;
            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }


        //read actual position
        private void button31_Click(object sender, EventArgs e)
        {
            try
            {
                string str = CONNECT.WH(port);
                richTextBox6.Text += "wyslana komenda: " + str;
            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }


        //clear console
        private void button34_Click(object sender, EventArgs e)
        {
            richTextBox6.Text = "";
        }

        //clear program window
        private void button29_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBox5.Text = "";
            }
            catch (Exception ex)
            {
                richTextBox6.Text += "\n" + ex;
            }
        }


        //com ports to choose from - combobox
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //command text input
        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        //program name texbox
        private void textBox18_TextChanged(object sender, EventArgs e)
        {

        }



        //--------------------------------------------------------------------------------------
















        //---------------------------------------------------------------------------------------MAIN PROGRAM------------------------------------------------

        static bool goBackFlag = false;

        //move robot to starting position
        private async void button22_Click_1(object sender, EventArgs e)
        {
            //zatrzymanie odczytu do okna rs232 connection
            timerTwo.Stop();

            //rozpoczecie programu
            console7counter = 0;
            richTextBox7.Text += "0  -----------Rozpoczeto program-----------";
            console7counter++;

            richTextBox8.Text += "-----------Wyslane dane do sterownika-----------";


            //start live preview
            //timer5.Start();

            await Task.Run(() => goImgPosProgAsync());

            //oczekiwanie na zakonczenie programu
            await Task.WhenAll();

            consoleWriter("-----------Zakonczono program-----------");

            //timer5.Stop();
            timerTwo.Start();

        }

        private async Task goImgPosProgAsync()
        {
            await Task.Delay(100);
            AsyncConsoleWriter("wejscie do pod programu");

            //------step 1 - wybranie programu wyczyszczenie i ustawienie predkosci-------------------do metody 

            //wybranie programu 9 sterownika
            CONNECT.N(port, "9");
            await Task.Delay(500);
            AsyncConsoleWriter("wybranie programu: N 9");
            AsyncConsoleWriter8("N 9");

            //wyczyszczenie programu
            CONNECT.NW(port);
            await Task.Delay(500);
            AsyncConsoleWriter("wyczyszczenie programu: NW 9");
            AsyncConsoleWriter8("NW");

            //ustawienie predkosci
            CONNECT.SendData(port, "10 SP 6");
            await Task.Delay(500);
            AsyncConsoleWriter("program line [10]: 10 SP 6");
            AsyncConsoleWriter8("10 SP 6");

            //sprawdzenie czy chwytak jest na bezpiecznej wysokosci
            bool safeHeight = await GripperSafeHeightAsync();
            bool zeroRotation = await GripperInZeroAsync();
            Task.WaitAll();

            //zamkniecie chwytaka na koniec ruchu
            CONNECT.SendData(port, "100 GC");
            await Task.Delay(500);
            AsyncConsoleWriter("program line [100]: 100 GC");
            AsyncConsoleWriter8("100 GC");

            //zkonczenie programu
            CONNECT.SendData(port, "110 ED");
            await Task.Delay(500);
            AsyncConsoleWriter("program line [110]: ED");
            AsyncConsoleWriter8("110 ED");

            //wystartowanie programu
            CONNECT.SendData(port, "RN");
            await Task.Delay(500);
            AsyncConsoleWriter("komenda: RUN");
            AsyncConsoleWriter8("RN");

            //-----------------------------------------------------------------------------do metody
            await Task.Delay(2000);

            //dopiero ruch do pozycji zdjecia wczesniej to bylo zabezpieczenie

            //koniec przeorientowania do pozycji bezpiecznej

            //wyczyszczenie okna programu sterownika
            richTextBox8.BeginInvoke((MethodInvoker)delegate ()
            {
                richTextBox8.Text = "-----------Wyslane dane do sterownika-----------";
            });


            bool wynik = await checkSafePositionAsync();
            await Task.WhenAll();
            //w zaleznosci od wyniku pozycyjonowania rozgalezienie programu
            if (wynik == true)
            {
                //wyczyszczenie programu
                CONNECT.NW(port);
                await Task.Delay(200);
                AsyncConsoleWriter("wyczyszczenie programu 9: NW");
                AsyncConsoleWriter8("NW");

                //ustawienie predkosci
                CONNECT.SendData(port, "10 SP 6");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [10]: 10 SP 6");
                AsyncConsoleWriter8("10 SP 6");

                //pozycja zdjecia : +380.11,+0.00,+613.27,+0.00,+176.32,+938.94,+0.00,R,A,C
                //nowa : +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A,C

                //przeorientowanie do pozycji robienia zdjecia
                CONNECT.SendData(port, "20 MP +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [20]: 20 MP +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");
                AsyncConsoleWriter8("20 MP +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");

                //zamkniecie chwytaka
                CONNECT.SendData(port, "21 TI 5");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [21]: TI 5");
                AsyncConsoleWriter8("21 TI 5");

                //zamkniecie chwytaka
                CONNECT.SendData(port, "GC");
                await Task.Delay(200);
                AsyncConsoleWriter("komenda: GC");
                AsyncConsoleWriter8("GC");

                //odpalenie programu
                CONNECT.SendData(port, "RN");
                await Task.Delay(200);
                AsyncConsoleWriter("komenda: RUN");
                AsyncConsoleWriter8("RN");
            }
            else
            {
                AsyncConsoleWriter("BLAD POZYCJONOWANIA");
                return;
            }



        }


        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }






        //zmienne globalne to tego wracaniaaaaa
        int msgIndex = 0;
       

        //to good
        private bool GripperInZero()
        {
            bool zeroRotation = false;
            try
            {
                string message = "", str = "";
                int counter = 0;



                consoleWriter("odczyt aktualnej pozycji...");

                //odczyt aktualnej pozycji
                string aktuPos = askForPosition(port);

                //tu raport
                consoleWriter("odczyt pozycji : " + aktuPos);

                //sprawdzenie czy obrot chwytaka to 0 inaczej mozna zlamac uchwyt na kamere
                (bool rotated, string pozycja) = CONNECT.checkGripperRotation(aktuPos);
                consoleWriter("chwytak obrocony ? : " + Convert.ToString(rotated));




                //jezeli chwytak obrocony to obrot do 0 stopni 
                if (rotated == true)
                {

                    //obrocenie samego chwytaka
                    msgTab[msgIndex] = "30 MP " + pozycja;
                    consoleWriter("program line [20]: " + msgTab[msgIndex]);
                    msgIndex++;
                }
                else
                {
                    consoleWriter("chwytak ma odpowiedni kat obrotu do poruszania");
                }


                /*

                //odczyt aktualnej pozycji
                aktuPos = askForPosition(port);
                consoleWriter("odczyt pozycji: " + aktuPos);

                //sprawdzenie czy obrot chwytaka to 0 inaczej mozna zlamac uchwyt na kamere
                (rotated, pozycja) = CONNECT.checkGripperRotation(aktuPos);

                //sprawdzenie czy aktualna pozycja jest rowna zadanej
                if (rotated == false)
                {
                    consoleWriter("[double check]chwytak ma odpowiedni kat obrotu do poruszania");
                    zeroRotation = true;
                }
                else
                {
                    consoleWriter("ERROR pozycja nie zostala osiagnieta");
                    zeroRotation = false;
                }
                */

            }
            catch (Exception ex)
            {
                richTextBox7.Text += Convert.ToString(ex);
            }
            return zeroRotation;
        }

        //to good
        private bool GripperSafeHeight()
        {
            bool safeHeight = false;
            try
            {

                //odczyt aktualnej pozycji
                string aktuPos = askForPosition(port);
                consoleWriter("odczyt pozycji: " + aktuPos);

                //sprawdzenie czy gripper jest na wysokosci >= 230 
                (bool safe, string pozycja, double number) = CONNECT.checkGripperHeight(aktuPos);


                consoleWriter("chwytak na bezpiecznej  wysokosci : " + Convert.ToString(safe) + "   aktualna wysokosc: " + Convert.ToString(number));


                //warunki na wysokosc
                if (safe == false)
                {
                    //przeorientowanie na bezpieczna wysokosc
                    msgTab[msgIndex] = "20 MP " + pozycja;
                    consoleWriter("program line [30]: " + msgTab[msgIndex]);
                    msgIndex++;

                    

                }
                else
                {
                    consoleWriter("Gripper jest na bezpiecznej wysokosci");
                }

                /*
                //odczyt aktualnej pozycji
                aktuPos = askForPosition(port);
                consoleWriter("odczyt pozycji: " + aktuPos);

                //sprawdzenie czy obrot chwytaka to 0 inaczej mozna zlamac uchwyt na kamere
                (safe, pozycja, number) = CONNECT.checkGripperHeight(aktuPos);

                //sprawdzenie czy aktualna pozycja jest rowna zadanej
                if (safe == false)
                {
                    consoleWriter("ERROR gripper nie jest na bezpiecznej wysokosci");
                    safeHeight = false;
                }
                else
                {
                    consoleWriter("[double check] Gripper jest na bezpiecznej wysokosci");
                    safeHeight = true;
                }
                */


            }
            catch (Exception ex)
            {
                richTextBox7.Text += Convert.ToString(ex);
            }
            return safeHeight;
        }


        bool imgPosition = false;

        int timer4counter=0;
        //timer 4 wait na pozycje wyslana odczekanie kilku tickow
        private void TimerEventProcessor4(Object sender, EventArgs myEventArgs)
        {
            try
            {
                string pozycja;
                Mat klon1 = new Mat();
                Mat klon2 = new Mat();
                Mat undistorted = new Mat();
                string message = "WH";
                string str = CONNECT.SendData(port, message);
                Task.Delay(3000);

                //string pozycja = port.ReadExisting();
                //string pozycja = "+398.22,0,+628.68,0.00,+176.32,+947.41,+0.00,R,A,C";

                if (timer4counter > 5)
                {
                    pozycja = "+398.22,0,+628.68,0.00,+176.32,+947.41,+0.00,R,A,C";
                    AsyncConsoleWriter(pozycja);
                    timer4counter = 0;
                }
                else
                {
                    pozycja = "+416.13,+9.33,+496.21,+1.62,+175.58,+1102.50,+0.00,R,A,O";
                    AsyncConsoleWriter(pozycja);
                }


                //imgPosition = CONNECT.imgPosition(pozycja);
                if (imgPosition == true)
                {
                    timer5.Stop();
                    Task.Delay(500);
                    capture.Read(klon1);
                    Task.Delay(500);
                    richTextBox7.BeginInvoke((MethodInvoker)delegate ()
                    {
                        richTextBox7.Text += "\n" + "kamera na pozycji zdjecia";
                        richTextBox7.ScrollToCaret();
                    });

                    //clear programTab
                    richTextBox8.BeginInvoke((MethodInvoker)delegate ()
                    {
                        richTextBox8.Text = "";
                    });


                    programTab = CONNECT.programTabClear(programTab);

                    capture.Read(klon2);
                    Cv2.Undistort(klon2, undistorted, InputArray.Create(cameraMatrix), InputArray.Create(distCoff), null);


                    Image oldImg = pictureBox3.Image;

                    wej = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\sample3.jpg", ImreadModes.Unchanged);
                    //wej = undistorted;
                    pictureBox3.Image = VISION.MatToBitmap(wej);
                    if (oldImg != null)
                    {
                        oldImg.Dispose();
                    }
                    klon1.Dispose();
                    klon2.Dispose();
                    undistorted.Dispose();
                    timerFour.Stop();
                }
                else
                {

                }


                timer4counter++;

            }
            catch (Exception ex)
            {
                richTextBox7.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox7.Text += "\n" + ex;
                    richTextBox7.ScrollToCaret();
                });
            }
        }


      



        private void CheckImgPosition()
        {
            //AsyncConsoleWriter("\n rozpoczeto sprawdzanie pozycji ..");
            timerFour.Start();
        }

        private void GoToImgPosition()
        {
            //AsyncConsoleWriter("\n rozpoczeto sprawdzanie pozycji ..");
            timer6.Start();
        }



        int counter6 = 0;
        //timer6 wysylanie do controllera
        private void TimerEventProcessor6(Object sender, EventArgs myEventArgs)
        {

            //jak flaga 1 to definicja pozycji, wysylanie interwalowe, co 500 ms czyli pol sekundy
            string message = "";
            string str = "";
            if (counter6 < msgTab.Length && msgTab[counter6] != null)
            {
                message = msgTab[counter6];
                str = CONNECT.SendData(port, message);

                richTextBox8.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox8.Text += str;
                    richTextBox8.ScrollToCaret();
                });
                counter6++;
            }
            else
            {
                counter6 = 0;
                timerTwo.Start();
                CheckImgPosition();
                timer6.Stop();
            }

        }

        //DO ZMIANY !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        private void ImgPositioning()
        {
            bool safeHeight, zeroRotation, imgPosition;
            string position = "";
            try
            {

                //wylaczenie timera odbierajacego informacje z portu rs232!
                timerTwo.Stop();

                richTextBox7.Text = "1  -----------Rozpoczeto program-----------";
                //wlaczenie live preview
                timer5.Start();
                console7counter = 2;

                //wybranie programu 9
                msgTab[0] = "N 9";
                consoleWriter("wybranie programu: " + msgTab[msgIndex]);
                msgIndex++;

                //wyczyszczenie programu
                msgTab[1] = "NW";
                consoleWriter("wyczyszczenie programu N 9 " + msgTab[msgIndex]);
                msgIndex++;

                //ustawienie predkosci
                msgTab[2] = "10 SP 6";
                consoleWriter("program line [10]: " + msgTab[msgIndex]);
                msgIndex++;

                //sprawdzenie czy chwytak jest na bezpiecznej wysokosci
                safeHeight = GripperSafeHeight();
                zeroRotation = GripperInZero();

                //zamkniecie chwytaka na koniec ruchu
                msgTab[msgIndex] = "100 GC";
                consoleWriter("program line [100]: " + msgTab[msgIndex]);
                msgIndex++;

                //zkonczenie programu
                msgTab[msgIndex] = "ED";
                consoleWriter("program line [110]: " + msgTab[msgIndex]);
                msgIndex++;

                //wlaczenie
                msgTab[msgIndex] = "RN";
                consoleWriter("komenda: " + msgTab[msgIndex]);
                msgIndex++;


                //send and run the program and check the img position
                GoToImgPosition();







                //po spelnieniu warunkow przejscie do pozycji robienia zdjecia

                //w pozycji zrobienie zdjecia o odpowiedniej ostrosci
                //kamera musi byc wlaczona wczesniej przed zrobieniem zdjecia !! inaczej ostrosc do dupy !!

                //jak jest zdjecie to caly algorytm wizyjny krok po kroku w jednej metodzie !!!

                /*
                //teraz pytanie jak rozwiazac mozliwe zmiany w ustawieniu !
                */

            }
            catch (Exception ex)
            {
                richTextBox7.Text += Convert.ToString(ex);
            }
        }

        private void VisionSystem()
        {
            try
            {
                consoleWriter("\n rozpoczeto przetwrzanie obrazu");
                //----------------------------------------------------------FIND OBJECTS
                
                //clone wejscia i maty
                Mat original = wej.Clone();
                OpenCvSharp.Point[][] kontury = null;
                kontury = VISION.GetContours(wej, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
                Cv2.DrawContours(original, kontury, -1, new Scalar(0, 0, 255), thickness: 3);

                double a;
                int area;
                for (int i = 0; i < kontury.Length; i++)
                {
                    Cv2.Circle(original, kontury[i][0], 1, new Scalar(0, 255, 0), 1);
                    a = Cv2.ContourArea(kontury[i]);
                    area = Convert.ToInt32(a);
                    Cv2.PutText(original, Convert.ToString(area), kontury[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);
                }
                //zapis konturów globalnie
                contours = kontury;

                pictureBox3.Image = VISION.MatToBitmap(original);


                //----------------------------------------------------------SORT OBJECTS
                Mat imageClone = wej.Clone();
                int small, connected;

                OpenCvSharp.Point punkt;
                OpenCvSharp.Point[][] smallC;
                OpenCvSharp.Point[][] connectedC;

                //wyswietlenie konturow zaleznie od objetosci
                (imageClone, small, connected) = VISION.showContours(imageClone, contours, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text));

                pictureBox3.Image = VISION.MatToBitmap(imageClone);
                richTextBox7.Text += "\nliczba konturów osobnych" + Convert.ToString(small) + "\n" + "liczba konturów polaczonych: " + Convert.ToString(connected) + "\n";

                //indeksowanie kazdego obiektu o odpowiednim rozmiarze
                (smallC, connectedC) = VISION.SmallBigContours(contours, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text), small, connected);

                Cv2.DrawContours(imageClone, smallC, -1, new Scalar(0, 255, 0), thickness: 3);
                Cv2.DrawContours(imageClone, connectedC, -1, new Scalar(255, 0, 0), thickness: 3);

                (smallContours, connectedContours) = VISION.LeftSortContours(smallC, connectedC, imageClone);
                //(sortedSmallContours, sortedConnectedContours) = Vision.LSortContours(smallContours, connectedContours);

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

                //zaznaczenie konturów
                Cv2.DrawContours(imageClone, smallContours, -1, new Scalar(0, 255, 0), thickness: 3);
                Cv2.DrawContours(imageClone, connectedContours, -1, new Scalar(255, 0, 0), thickness: 3);

                pictureBox3.Image = VISION.MatToBitmap(imageClone);



                //----------------------------------------------------------GET COLORS
                imageClone = wej.Clone();
                Mat hsv = new Mat();
                OpenCvSharp.Point center;
                Vec3b vector;
                string color;

                Cv2.CvtColor(imageClone, hsv, ColorConversionCodes.BGR2HSV_FULL);


                for (int i = 0; i < smallContours.Length; i++)
                {
                    color = VISION.ContourColor(imageClone, smallContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox13.Text), Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                    //momenty
                    var M = Cv2.Moments(smallContours[i], false);
                    center.X = Convert.ToInt32(M.M10 / M.M00);
                    center.Y = Convert.ToInt32(M.M01 / M.M00);

                    vector = hsv.At<Vec3b>(center.Y, center.X);
                    Scalar col = new Scalar(vector.Item0, vector.Item1, vector.Item2);

                    Cv2.Circle(imageClone, center, 3, new Scalar(0, 255, 0), 1);
                    Cv2.PutText(imageClone, color + " " + Convert.ToString(col.Val0) + " " + Convert.ToString(col.Val1) + " " + Convert.ToString(col.Val2) + " ", smallContours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);


                }
                pictureBox3.Image = VISION.MatToBitmap(imageClone);



                //----------------------------------------------------------GET SHAPE


                imageClone = wej.Clone();
                string shape;

                for (int i = 0; i < smallContours.Length; i++)
                {
                    shape = VISION.GetShape(smallContours[i]);

                    if (shape == "triangle")
                    {
                        Cv2.PutText(imageClone, "triangle", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                    else if (shape == "square")
                    {
                        Cv2.PutText(imageClone, "square", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                    else if (shape == "rectangle")
                    {
                        Cv2.PutText(imageClone, "rectangle", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                    else if (shape == "pentagon")
                    {
                        Cv2.PutText(imageClone, "pentagon", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                    else if (shape == "circle")
                    {
                        Cv2.PutText(imageClone, "circle", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                    }
                }

                pictureBox3.Image = VISION.MatToBitmap(imageClone);



                //----------------------------------------------------------GET ANGLE


                int angle = 0;

                imageClone = wej.Clone();

                for (int i = 0; i < smallContours.Length; i++)
                {
                    angle = VISION.GetAngle(imageClone, smallContours[i]);

                    Cv2.PutText(imageClone, Convert.ToString(angle), smallContours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 255), 1);

                }

                Bitmap bitimg = VISION.MatToBitmap(imageClone);
                pictureBox3.Image = bitimg;




                //----------------------------------------------------------CREATE OBJECTS

                //przygotowanie zmiennych
                imageClone = wej.Clone();
                OpenCvSharp.Point c = new OpenCvSharp.Point();
                OpenCvSharp.Point[] cor;

                singleConts = new ContourOb[smallContours.Length];
                connectedConts = new ContourOb[connectedContours.Length];



                for (int i = 0; i < smallContours.Length; i++)
                {
                    //center z momentow 
                    c = VISION.GetCenter(smallContours[i]);

                    //kolor
                    color = VISION.ContourColor(imageClone, smallContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text),
                        Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox13.Text),
                        Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                    //shape 
                    shape = VISION.GetShape(smallContours[i]);

                    //angle
                    angle = VISION.GetAngle(imageClone, smallContours[i]);

                    //corners
                    cor = VISION.GetCorners(smallContours[i]);

                    //tworzeni kazdego obiektu
                    singleConts[i] = new ContourOb()
                    {
                        indx = i,        //index 0,1,2,3,4...       
                        center = c,      //srodek konturu
                        angle = angle,   //obrot konturu od osi x
                        color = color,   //kolor konturu
                        shape = shape,   //ksztalt konturu
                        corners = cor,
                        contour = smallContours[i],   //wszystkie punkty konturu
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

                for (int i = 0; i < connectedContours.Length; i++)
                {
                    //center z momentow 
                    c = VISION.GetCenter(connectedContours[i]);

                    //kolor
                    color = VISION.ContourColor(imageClone, connectedContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text),
                        Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text),
                        Convert.ToInt32(textBox13.Text), Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                    //shape 
                    shape = VISION.GetShape(connectedContours[i]);

                    //angle
                    angle = VISION.GetAngle(imageClone, connectedContours[i]);

                    //corners
                    cor = VISION.GetCorners(connectedContours[i]);

                    //tworzeni kazdego obiektu
                    connectedConts[i] = new ContourOb()
                    {
                        indx = i,        //index 0,1,2,3,4...       
                        center = c,      //srodek konturu
                        angle = angle,   //obrot konturu od osi x
                        color = color,   //kolor konturu
                        shape = shape,   //ksztalt konturu
                        corners = cor,
                        contour = connectedContours[i],   //wszystkie punkty konturu
                    };

                    //organizacja naroznikow
                    OpenCvSharp.Point temp0, temp1, temp2, temp3;
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





                //----------------------------------------------------------SHOW single CONTS


                imageClone = wej.Clone();
                richTextBox9.Text = "";

                for (int i = 0; i < singleConts.Length; i++)
                {
                    //indekswoanie na obrazie
                    Cv2.Circle(imageClone, singleConts[i].contour[0], 1, new Scalar(255, 0, 0), 1);

                    //puttext
                    Cv2.PutText(imageClone, Convert.ToString(singleConts[i].indx), singleConts[i].contour[0], HersheyFonts.HersheySimplex, 1, new Scalar(0, 255, 0), 1);

                    //opisy
                    richTextBox9.Text += "\n" + Convert.ToString(singleConts[i].indx) + "\n";
                    richTextBox9.Text += "center:  " + Convert.ToString(singleConts[i].center) + "\n";
                    richTextBox9.Text += "angle:   " + Convert.ToString(singleConts[i].angle) + "\n";
                    richTextBox9.Text += "color:   " + Convert.ToString(singleConts[i].color) + "\n";
                    richTextBox9.Text += "shape:   " + Convert.ToString(singleConts[i].shape) + "\n";
                }
                //zaznaczenie konturów
                Cv2.DrawContours(imageClone, smallContours, -1, new Scalar(0, 255, 0), thickness: 3);
                pictureBox3.Image = VISION.MatToBitmap(imageClone);


            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }

        }





















        //--------------------------------------------------------------- proba asynchroniczna---------------------------------------------------------------------------------------------------------------------------

        //console displayer
        void consoleWriter(string message)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(richTextBox7.Lines[richTextBox7.GetLineFromCharIndex(richTextBox7.SelectionStart)]))
                {
                    richTextBox7.AppendText(Environment.NewLine + Convert.ToString(console7counter) + "   " + message);
                }
                else
                {
                    richTextBox7.AppendText(Convert.ToString(console7counter) + "   " + message);
                }

                console7counter++;
            }
            catch (Exception ex)
            {

                richTextBox7.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox7.Text += "\n" + ex;
                    richTextBox7.ScrollToCaret();
                });
            }
        }

        //async displayer
        void AsyncConsoleWriter(string message)
        {
            try
            {
                richTextBox7.BeginInvoke((MethodInvoker)delegate ()
                {
                    if (!string.IsNullOrWhiteSpace(richTextBox7.Lines[richTextBox7.GetLineFromCharIndex(richTextBox7.SelectionStart)]))
                    {
                        richTextBox7.AppendText(Environment.NewLine + Convert.ToString(console7counter) + "   " + message);
                    }
                    else
                    {
                        richTextBox7.AppendText(Convert.ToString(console7counter) + "   " + message);
                    }
                    richTextBox7.ScrollToCaret();
                });


                console7counter++;

            }
            catch (Exception ex)
            {

                richTextBox7.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox7.Text += "\n" + ex;
                    richTextBox7.ScrollToCaret();
                });
            }
        }

        void AsyncConsoleWriter8(string message)
        {
            try
            {
                richTextBox8.BeginInvoke((MethodInvoker)delegate ()
                {
                    if (!string.IsNullOrWhiteSpace(richTextBox8.Lines[richTextBox8.GetLineFromCharIndex(richTextBox8.SelectionStart)]))
                    {
                        richTextBox8.AppendText(Environment.NewLine + message);
                    }
                    else
                    {
                        richTextBox8.AppendText(message);
                    }
                    richTextBox8.ScrollToCaret();
                });

            }
            catch (Exception ex)
            {

                richTextBox8.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox8.Text += "\n" + ex;
                    richTextBox8.ScrollToCaret();
                });
            }
        }





        int counterPosition;

        bool free = false;

        //timer 5wyswietlanie video
        private void TimerEventProcessor5(Object sender, EventArgs myEventArgs)
        {
            try
            {
                Mat klon = new Mat();
                Mat undistorted = new Mat();

                if (capture.IsOpened() == false)
                {
                    richTextBox7.BeginInvoke((MethodInvoker)delegate ()
                    {
                        richTextBox7.Text = "ERROR nie znaleziono kamery";
                        richTextBox7.ScrollToCaret();
                    });
                    timer5.Stop();
                }
                else
                {
                    capture.Read(klon);
                }
                Cv2.Undistort(klon, undistorted, InputArray.Create(cameraMatrix), InputArray.Create(distCoff), null);


                Image oldImg = pictureBox3.Image;
                pictureBox3.Image = VISION.MatToBitmap(undistorted);
                if (oldImg != null)
                {
                    oldImg.Dispose();
                }
                klon.Dispose();
                undistorted.Dispose();

            }
            catch (Exception ex)
            {

                richTextBox7.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox7.Text += "\n" + ex;
                    richTextBox7.ScrollToCaret();
                });
            }
        }


        //metody synchroniczne systemu wizyjnego ---------------
        private Mat visionMeasure()
        {
            Mat wejthresh = wej.Clone();

            //find markers on image
            (MarkerOut, MarkerId) = VISION.findArucoMarkers(wejthresh, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
            OpenCvSharp.Aruco.CvAruco.DrawDetectedMarkers(wejthresh, MarkerOut, MarkerId, new Scalar(255, 0, 0));


            return wejthresh;
        }

        private Mat visionContours()
        {
            //clone wejscia i maty
            Mat imgClone = wej.Clone();
            OpenCvSharp.Point[][] kontury = null;
            kontury = VISION.GetContours(imgClone, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
            Cv2.DrawContours(imgClone, kontury, -1, new Scalar(0, 0, 255), thickness: 3);

            double a;
            int area;
            for (int i = 0; i < kontury.Length; i++)
            {
                Cv2.Circle(imgClone, kontury[i][0], 1, new Scalar(0, 255, 0), 1);
                a = Cv2.ContourArea(kontury[i]);
                area = Convert.ToInt32(a);
                Cv2.PutText(imgClone, Convert.ToString(area), kontury[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);
            }
            //zapis konturów globalnie
            contours = kontury;

            return imgClone;
        }

        private Mat visionSortConts()
        {
            Mat imgClone = wej.Clone();
            int small, connected;

            OpenCvSharp.Point punkt;
            OpenCvSharp.Point[][] smallC;
            OpenCvSharp.Point[][] connectedC;

            //wyswietlenie konturow zaleznie od objetosci
            (imgClone, small, connected) = VISION.showContours(imgClone, contours, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text));

            //indeksowanie kazdego obiektu o odpowiednim rozmiarze
            (smallC, connectedC) = VISION.SmallBigContours(contours, Convert.ToInt32(textBox3.Text), Convert.ToInt32(textBox4.Text), Convert.ToInt32(textBox5.Text), small, connected);

            Cv2.DrawContours(imgClone, smallC, -1, new Scalar(0, 255, 0), thickness: 3);
            Cv2.DrawContours(imgClone, connectedC, -1, new Scalar(255, 0, 0), thickness: 3);

            (smallContours, connectedContours) = VISION.LeftSortContours(smallC, connectedC, imgClone);
            //(sortedSmallContours, sortedConnectedContours) = Vision.LSortContours(smallContours, connectedContours);

            for (int i = 0; i < smallContours.Length; i++)
            {
                punkt = smallContours[i][0];
                //punkt.X = punkt.X - 100;
                //punkt.Y = punkt.Y - 100;
                Cv2.PutText(imgClone, Convert.ToString(i), punkt, HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 255, 0), 1);
            }

            for (int i = 0; i < connectedContours.Length; i++)
            {
                punkt = connectedContours[i][0];
                //punkt.X = punkt.X - 100;
                //punkt.Y = punkt.Y - 100;
                Cv2.PutText(imgClone, Convert.ToString(i), punkt, HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);
            }

            //zaznaczenie konturów
            Cv2.DrawContours(imgClone, smallContours, -1, new Scalar(0, 255, 0), thickness: 3);
            Cv2.DrawContours(imgClone, connectedContours, -1, new Scalar(255, 0, 0), thickness: 3);


            return imgClone;
        }

        private Mat visionColorRec()
        {
            Mat imgClone = wej.Clone();
            Mat hsv = new Mat();
            OpenCvSharp.Point center;
            Vec3b vector;
            string color;

            Cv2.CvtColor(imgClone, hsv, ColorConversionCodes.BGR2HSV_FULL);


            for (int i = 0; i < smallContours.Length; i++)
            {
                color = VISION.ContourColor(imgClone, smallContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text), Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox13.Text), Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                //momenty
                var M = Cv2.Moments(smallContours[i], false);
                center.X = Convert.ToInt32(M.M10 / M.M00);
                center.Y = Convert.ToInt32(M.M01 / M.M00);

                vector = hsv.At<Vec3b>(center.Y, center.X);
                Scalar col = new Scalar(vector.Item0, vector.Item1, vector.Item2);

                Cv2.Circle(imgClone, center, 3, new Scalar(0, 255, 0), 1);
                Cv2.PutText(imgClone, color + " " + Convert.ToString(col.Val0) + " " + Convert.ToString(col.Val1) + " " + Convert.ToString(col.Val2) + " ", smallContours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 0), 1);


            }

            return imgClone;
        }

        private Mat visionShapeRec()
        {
            Mat imgClone = wej.Clone();
            string shape;

            for (int i = 0; i < smallContours.Length; i++)
            {
                shape = VISION.GetShape(smallContours[i]);

                if (shape == "triangle")
                {
                    Cv2.PutText(imgClone, "triangle", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                }
                else if (shape == "square")
                {
                    Cv2.PutText(imgClone, "square", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                }
                else if (shape == "rectangle")
                {
                    Cv2.PutText(imgClone, "rectangle", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                }
                else if (shape == "pentagon")
                {
                    Cv2.PutText(imgClone, "pentagon", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                }
                else if (shape == "circle")
                {
                    Cv2.PutText(imgClone, "circle", smallContours[i][0], HersheyFonts.HersheySimplex, 1, new Scalar(255, 255, 255), 1);
                }
            }

            return imgClone;
        }

        private Mat visionAngleRec()
        {
            int angle = 0;
            Mat imgClone = wej.Clone();

            for (int i = 0; i < smallContours.Length; i++)
            {
                angle = VISION.GetAngle(imgClone, smallContours[i]);

                Cv2.PutText(imgClone, Convert.ToString(angle), smallContours[i][0], HersheyFonts.HersheySimplex, 0.8, new Scalar(255, 0, 255), 1);
            }
            return imgClone;
        }

        private void visionCreateOb()
        {
            //przygotowanie zmiennych
            Mat imgClone = wej.Clone();

            string color = "";
            string shape = "";
            int angle = 0;
            int robotAngle = 0;

            OpenCvSharp.Point c = new OpenCvSharp.Point();
            OpenCvSharp.Point[] cor;

            singleConts = new ContourOb[smallContours.Length];
            connectedConts = new ContourOb[connectedContours.Length];


            for (int i = 0; i < smallContours.Length; i++)
            {
                //center z momentow 
                c = VISION.GetCenter(smallContours[i]);
                //kolor
                color = VISION.ContourColor(imgClone, smallContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text),
                    Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox13.Text),
                    Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));
                //shape 
                shape = VISION.GetShape(smallContours[i]);
                //angle
                angle = VISION.GetAngle(imgClone, smallContours[i]);

                //dostosowanie kata obrotu do obrotu chwytaka
                if(angle > 90)
                {
                    robotAngle = angle - 180;    
                }
                else
                {
                    robotAngle = angle;
                }

                //corners
                cor = VISION.GetCorners(smallContours[i]);

                //tworzeni kazdego obiektu
                singleConts[i] = new ContourOb()
                {
                    indx = i,        //index 0,1,2,3,4...       
                    center = c,      //srodek konturu
                    angle = angle,   //obrot konturu od osi x
                    realAngle = robotAngle,
                    color = color,   //kolor konturu
                    shape = shape,   //ksztalt konturu
                    corners = cor,
                    contour = smallContours[i],   //wszystkie punkty konturu
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

            for (int i = 0; i < connectedContours.Length; i++)
            {
                //center z momentow 
                c = VISION.GetCenter(connectedContours[i]);

                //kolor
                color = VISION.ContourColor(imgClone, connectedContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text),
                    Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text),
                    Convert.ToInt32(textBox13.Text), Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

                //shape 
                shape = VISION.GetShape(connectedContours[i]);

                //angle
                angle = VISION.GetAngle(imgClone, connectedContours[i]);

                //corners
                cor = VISION.GetCorners(connectedContours[i]);

                //tworzeni kazdego obiektu
                connectedConts[i] = new ContourOb()
                {
                    indx = i,        //index 0,1,2,3,4...       
                    center = c,      //srodek konturu
                    angle = angle,   //obrot konturu od osi x
                    color = color,   //kolor konturu
                    shape = shape,   //ksztalt konturu
                    corners = cor,
                    contour = connectedContours[i],   //wszystkie punkty konturu
                };

                //organizacja naroznikow
                OpenCvSharp.Point temp0, temp1, temp2, temp3;
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

        private Mat visionMapping()
        {
            Mat wejthresh = wej.Clone();
            Matrix<double> abcd;

            //zdefiniowanie rzeczywistych wymiarow srodka konturu

            //Macierz transforamcji ukladow wsplorzednych
            (abcd, ppm) = VISION.coordinatesABCD(MarkerOut, MarkerId);

            //wspolrzedne srodka obietu we wspolrzednych robota
            (singleConts, connectedConts) = VISION.mapCoordinates(singleConts, connectedConts, abcd);

            for (int i = 0; i < singleConts.Length; i++)
            {
                Cv2.Circle(wejthresh, singleConts[i].center, 2, new Scalar(0, 255, 0));
                Cv2.PutText(wejthresh, Convert.ToString(singleConts[i].realCenter), singleConts[i].center, HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255));
            }
            for (int i = 0; i < connectedConts.Length; i++)
            {
                Cv2.Circle(wejthresh, connectedConts[i].center, 2, new Scalar(0, 255, 0));
                Cv2.PutText(wejthresh, Convert.ToString(connectedConts[i].realCenter), connectedConts[i].center, HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 0, 255));
            }

            return wejthresh;
        }

        private (Mat,string) visionShowOb()
        {
            Mat imgClone = wej.Clone();
            string message = "Pojedyncze obiekty: \n";
            for (int i = 0; i < singleConts.Length; i++)
            {
                //indekswoanie na obrazie
                Cv2.Circle(imgClone, singleConts[i].contour[0], 1, new Scalar(255, 0, 0), 1);

                //puttext
                Cv2.PutText(imgClone, Convert.ToString(singleConts[i].indx), singleConts[i].contour[0], HersheyFonts.HersheySimplex, 1, new Scalar(0, 255, 0), 1);

                //opisy
                message += "\n" + Convert.ToString(singleConts[i].indx) + "\n";
                message += "center:         " + Convert.ToString(singleConts[i].center) + "\n";
                message += "real center:    " + Convert.ToString(singleConts[i].realCenter) + "\n";
                message += "angle:          " + Convert.ToString(singleConts[i].angle) + "\n";
                message += "color:          " + Convert.ToString(singleConts[i].color) + "\n";
                message += "shape:          " + Convert.ToString(singleConts[i].shape) + "\n";
            }

            //zaznaczenie konturów
            Cv2.DrawContours(imgClone, smallContours, -1, new Scalar(0, 255, 0), thickness: 3);

            message += "\n\nGrupy obiektów:";

            if (connectedConts.Length > 0)
            {
                for (int i = 0; i < connectedConts.Length; i++)
                {
                    //indekswoanie na obrazie
                    Cv2.Circle(imgClone, connectedConts[i].contour[0], 1, new Scalar(255, 0, 0), 1);

                    //puttext
                    Cv2.PutText(imgClone, Convert.ToString(connectedConts[i].indx), connectedConts[i].contour[0], HersheyFonts.HersheySimplex, 1, new Scalar(0, 255, 0), 1);

                    //opisy
                    message += "\n" + Convert.ToString(connectedConts[i].indx) + "\n";
                    message += "center:  " + Convert.ToString(connectedConts[i].center) + "\n";
                    message += "angle:   " + Convert.ToString(connectedConts[i].angle) + "\n";
                    message += "color:   " + Convert.ToString(connectedConts[i].color) + "\n";
                    message += "shape:   " + Convert.ToString(connectedConts[i].shape) + "\n";
                }
            }
            else
            {
                message += "\n\nBrak grup obiektów";
            }

            Cv2.DrawContours(imgClone, connectedContours, -1, new Scalar(255, 0, 0), thickness: 3);
            pictureBox2.Image = VISION.MatToBitmap(imgClone);



            return (imgClone,message);
        }

        private Mat toolMasks()
        {
            Mat imageClone = wej.Clone();


            (toolUpSingleContours, toolDownSingleContours, imageClone) = VISION.ToolPoints(imageClone, singleConts, ppm);

            //PETLA ilosc konturow
            for (int i = 0; i < singleConts.Length; i++)
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
            }
            return imageClone;
        }

        private string detectCollisions()
        {
            int[][] collisions = VISION.GetSingleCollisions(singleConts, toolUpSingleContours, toolDownSingleContours);

            string message = "";
            for (int i = 0; i < collisions.Length; i++)
            {
                for (int j = 0; j < collisions[i].Length; j++)
                {
                    if (collisions[i][j] != 1000)
                    {
                        singleConts[i].collisions = new int[collisions[i].Length];
                        singleConts[i].collisions[j] = collisions[i][j];
                        message += "\n kolizja przy podnoszeniu obiektu: " + Convert.ToString(i) + " z obiektem:  " + Convert.ToString(singleConts[i].collisions[j]);
                    }
                }
            }
            
            return message;
        }

        private string[] predefineSinglePositions()
        {
            string[] message = new string[singleConts.Length * 4];
            string[] messageAfter = new string[singleConts.Length * 16];
            int counter = 0;
            int counter02 = 0;

            //tutaj wybor tych bez kolizji



            //petla wysylajaca definicje pozycji do programu, musi zaczynac sie od 1 linii i 1 pozycji
            for (int nr = 0; nr < singleConts.Length; nr++)
            {
                free = false;
                if(singleConts[nr].collisions == null)
                {
                    double x = RoundUp(singleConts[nr].realCenter.X, 2);
                    double y = RoundUp(singleConts[nr].realCenter.Y, 2);
                    double angle = RoundUp(singleConts[nr].realAngle, 2);

                    //numer obiektu zapisany w tej samej tabeli string co pozycje dla latwego znalezienia
                    message[counter] = Convert.ToString(nr);
                    counter++;
                    //pozcyja 20 mm nad klockiem bez kata
                    message[counter] = Convert.ToString(y) + ",0,305," + "0" + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                    counter++;

                    //pozycja 20 mm nad klockiem z katem obrotu chwytaka
                    message[counter] = Convert.ToString(y) + ",0,305," + Convert.ToString(angle) + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                    counter++;

                    //pozcyja podniesienia klocka z katem obrotu
                    message[counter] = Convert.ToString(y) + ",0,217," + Convert.ToString(angle) + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                    counter++;
                }
            }

            string msg = "";
            for (int i = 0; i < message.Length; i++)
            {
                msg += "\n" + message[i];
            }

            richTextBox12.BeginInvoke((MethodInvoker)delegate ()
            {
                richTextBox12.Text = "\n Predefined single Positions: \n\n";
                richTextBox12.Text += msg;
                richTextBox12.ScrollToCaret();
            });



            //AsyncConsoleWriter("first counter2 : " + counter02);
            //petla po kazdym konturze
            for (int nr = 0; nr < singleConts.Length; nr++)
            {
                //jezeli kontur posiada kolizje
                if (singleConts[nr].collisions != null)
                {
                    //petla po kolizjach danego konturu
                    for (int j = 0; j < singleConts[nr].collisions.Length; j++)
                    {
                        //petla po pozycjach predefiniowanych zapisanych
                        for (int i = 0; i < message.Length; i += 4)
                        {
                            if (message[i] != null || message[i] != "")
                            {
                                //AsyncConsoleWriter("numer konturu: " + nr);
                                //AsyncConsoleWriter("if counter2 : " + counter02 + "   message: " + message[i] + "    kolizja z: " + singleConts[nr].collisions[j]);
                                //jezeli obiekt z którym jest kolizja został wczesniej wpisany na liste to wpisz ten 
                                if (singleConts[nr].collisions[j] == Convert.ToInt64(message[i]) && singleConts[nr].collisions[j] != 0)
                                {

                                    double x = RoundUp(singleConts[nr].realCenter.X, 2);
                                    double y = RoundUp(singleConts[nr].realCenter.Y, 2);
                                    double angle = RoundUp(singleConts[nr].realAngle, 2);

                                    //numer obiektu zapisany w tej samej tabeli string co pozycje dla latwego znalezienia
                                    messageAfter[counter02] = Convert.ToString(nr);
                                    counter02++;
                                    //pozcyja 20 mm nad klockiem bez kata
                                    messageAfter[counter02] = Convert.ToString(y) + ",0,305," + "0" + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                                    counter02++;

                                    //pozycja 20 mm nad klockiem z katem obrotu chwytaka
                                    messageAfter[counter02] = Convert.ToString(y) + ",0,305," + Convert.ToString(angle) + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                                    counter02++;

                                    //pozycja podniesienia klocka z katem obrotu
                                    messageAfter[counter02] = Convert.ToString(y) + ",0,217," + Convert.ToString(angle) + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                                    counter02++;
                                }
                                else
                                {
                                    //AsyncConsoleWriter("else counter2 : " + counter02);
                                }


                            }
                        }


                    }
                }  
            }

            if(counter02 > 0 )
            {
                //złączenie dwoch wiadomosci
                for (int i = 0; i < counter02; i++)
                {
                    message[counter] = messageAfter[i];
                    counter++;
                }
            }
            



            counter = 0;
            counter02 = 0;


            if(singleConts.Length == 0)
            {
                free = true;
            }

            return message;
        }

        private string[] predefineConnectedPositions()
        {
            string[] message = new string[connectedConts.Length * 5];
            int counter = 0;



            if(sortCounter % 2 == 0)
            {
                //petla wysylajaca definicje pozycji do programu, musi zaczynac sie od 1 linii i 1 pozycji
                for (int nr = 0; nr < connectedConts.Length; nr++)
                {
                    free = false;
                    if (connectedConts[nr].collisions == null)
                    {
                        double x = RoundUp(connectedConts[nr].realCenter.X, 2);
                        double y = RoundUp(connectedConts[nr].realCenter.Y, 2);
                        double angle = RoundUp(connectedConts[nr].realAngle, 2);

                        //numer obiektu zapisany w tej samej tabeli string co pozycje dla latwego znalezienia " MP +365.50,+0.00,+304.65,+0.00,+177.50,+625.38,+0.00,R,A"
                        message[counter] = Convert.ToString(nr);
                        counter++;

                        //podjechanie obok obiektu
                        message[counter] = Convert.ToString(y + 80) + ",0,240," + "0" + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                        counter++;

                        //obnizenie
                        message[counter] = Convert.ToString(y + 80) + ",0,217," + "0" + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                        counter++;

                        //wjechanie w skupisko klockow
                        message[counter] = Convert.ToString(y - 40) + ",0,217," + "0" + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                        counter++;

                        //podniesienie chwytka
                        message[counter] = Convert.ToString(y - 40) + ",0,305," + "0" + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                        counter++;

                    }
                }
            }
            else
            {
                //petla wysylajaca definicje pozycji do programu, musi zaczynac sie od 1 linii i 1 pozycji
                for (int nr = 0; nr < connectedConts.Length; nr++)
                {
                    free = false;
                    if (connectedConts[nr].collisions == null)
                    {
                        double x = RoundUp(connectedConts[nr].realCenter.X, 2);
                        double y = RoundUp(connectedConts[nr].realCenter.Y, 2);
                        double angle = RoundUp(connectedConts[nr].realAngle, 2);

                        //numer obiektu zapisany w tej samej tabeli string co pozycje dla latwego znalezienia " MP +365.50,+0.00,+304.65,+0.00,+177.50,+625.38,+0.00,R,A"
                        message[counter] = Convert.ToString(nr);
                        counter++;

                        //podjechanie obok obiektu
                        message[counter] = Convert.ToString(y - 80) + ",0,240," + "0" + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                        counter++;

                        //obnizenie
                        message[counter] = Convert.ToString(y - 80) + ",0,217," + "0" + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                        counter++;

                        //wjechanie w skupisko klockow
                        message[counter] = Convert.ToString(y + 40) + ",0,217," + "0" + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                        counter++;

                        //podniesienie chwytka
                        message[counter] = Convert.ToString(y + 40) + ",0,305," + "0" + ",+177.50, " + Convert.ToString(x) + ",+0.00,R,A";
                        counter++;

                    }
                }
            }





            return message;
        }


        //-------------------------------------------------------




        //run test program button
        private async void button23_Click(object sender, EventArgs e)
        {
            try
            {
                //zatrzymanie odczytu do okna rs232 connection
                timerTwo.Stop();
                
                //rozpoczecie programu
                console7counter = 0;
                richTextBox7.Text+= "0  -----------Rozpoczeto program-----------";
                console7counter++;

                richTextBox8.Text += "-----------Wyslane dane do sterownika-----------";


                //start live preview
                timer5.Start();
                
                consoleWriter("-----------video started-----------");
                //async task program !
                await Task.Run(() => runTestProgramAsync());

                //oczekiwanie na zakonczenie programu
                await Task.WhenAll();

                consoleWriter("-----------Zakonczono program-----------");


                timerTwo.Start();

            }
            catch (Exception ex)
            {
                richTextBox7.Text += Convert.ToString(ex);
            }
        }

 
        private async Task runTestProgramAsync()
        {

            await Task.Delay(100);
            AsyncConsoleWriter("wejscie do pod programu");

            //------step 1 - wybranie programu wyczyszczenie i ustawienie predkosci-------------------do metody 
            
            //wybranie programu 9 sterownika
            CONNECT.N(port, "9");
            await Task.Delay(500);
            AsyncConsoleWriter("wybranie programu: N 9");
            AsyncConsoleWriter8("N 9");

            //wyczyszczenie programu
            CONNECT.NW(port);
            await Task .Delay(500);
            AsyncConsoleWriter("wyczyszczenie programu: NW 9");
            AsyncConsoleWriter8("NW");

            //ustawienie predkosci
            CONNECT.SendData(port, "10 SP 10");
            await Task.Delay(500);
            AsyncConsoleWriter("program line [10]: 10 SP 10");
            AsyncConsoleWriter8("10 SP 10");

            //sprawdzenie czy chwytak jest na bezpiecznej wysokosci
            bool safeHeight = await GripperSafeHeightAsync();
            bool zeroRotation = await GripperInZeroAsync();
            Task.WaitAll();

            //zamkniecie chwytaka na koniec ruchu
            CONNECT.SendData(port, "100 GC");
            await Task.Delay(500);
            AsyncConsoleWriter("program line [100]: 100 GC");
            AsyncConsoleWriter8("100 GC");

            //zkonczenie programu
            CONNECT.SendData(port, "110 ED");
            await Task.Delay(500);
            AsyncConsoleWriter("program line [110]: ED");
            AsyncConsoleWriter8("110 ED");

            //wystartowanie programu
            CONNECT.SendData(port, "RN");
            await Task.Delay(500);
            AsyncConsoleWriter("komenda: RUN");
            AsyncConsoleWriter8("RN");

            //-----------------------------------------------------------------------------do metody
            await Task.Delay(2000);

            //dopiero ruch do pozycji zdjecia wczesniej to bylo zabezpieczenie

            //koniec przeorientowania do pozycji bezpiecznej

            //wyczyszczenie okna programu sterownika
            richTextBox8.BeginInvoke((MethodInvoker)delegate ()
            {
                richTextBox8.Text = "-----------Wyslane dane do sterownika-----------";
            });


            bool wynik = await checkSafePositionAsync();
            await Task.WhenAll();
            //w zaleznosci od wyniku pozycyjonowania rozgalezienie programu
            if (wynik == true)
            {
                //wyczyszczenie programu
                CONNECT.NW(port);
                await Task.Delay(200);
                AsyncConsoleWriter("wyczyszczenie programu 9: NW");
                AsyncConsoleWriter8("NW");

                //ustawienie predkosci
                CONNECT.SendData(port, "10 SP 10");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [10]: 10 SP 10");
                AsyncConsoleWriter8("10 SP 10");

                //pozycja zdjecia : +380.11,+0.00,+613.27,+0.00,+176.32,+938.94,+0.00,R,A,C
                //nowa : +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A,C

                //przeorientowanie do pozycji robienia zdjecia
                CONNECT.SendData(port, "20 MP +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [20]: 20 MP +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");
                AsyncConsoleWriter8("20 MP +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");

                //zamkniecie chwytaka
                CONNECT.SendData(port, "21 TI 5");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [21]: TI 5");
                AsyncConsoleWriter8("21 TI 5");

                //zamkniecie chwytaka
                CONNECT.SendData(port, "GC");
                await Task.Delay(200);
                AsyncConsoleWriter("komenda: GC");
                AsyncConsoleWriter8("GC");

                //odpalenie programu
                CONNECT.SendData(port, "RN");
                await Task.Delay(200);
                AsyncConsoleWriter("komenda: RUN");
                AsyncConsoleWriter8("RN");
            }
            else
            {
                AsyncConsoleWriter("BLAD POZYCJONOWANIA");
                return;
            }


            sortCounter = 0;

            while(free == false)
            {
                await Task.Delay(200);
                imgPosition = false;
                wynik = await checkImgPositionAsync();
                AsyncConsoleWriter("wynik pozycjonowania:   " + wynik);
                await Task.WhenAll();
                await Task.Delay(200);
                //w zaleznosci od wyniku pozycyjonowania rozgalezienie programu
                bool visionStatus = false;
                if (wynik == true)
                {
                    visionStatus = await Task.Run(() => startVisionSystemAsync());
                }
                else
                {
                    AsyncConsoleWriter("BLAD POZYCJONOWANIA");
                    return;
                }
                await Task.WhenAll();

                bool singleSorted = false;

                if (visionStatus == true)
                {
                    singleSorted = await startSortingSingle();
                }
                else
                {
                    AsyncConsoleWriter("BLAD SYSTEMU WIZYJNEGO");
                    return;
                }

                
                await Task.WhenAll();
                await Task.Delay(200);
                imgPosition = false;
                wynik = await checkImgPositionAsync();
                AsyncConsoleWriter("wynik pozycjonowania:   " + wynik);
                await Task.WhenAll();


                if (wynik == true && singleSorted == true)
                {
                    await startSortingConnected();
                    //timer 5
                    timer5.Start();
                }
                else
                {
                    AsyncConsoleWriter("BLAD sortowania pojedynczych");
                    return;
                }

                await Task.WhenAll();


            }



            return;
        }



        private async Task<bool> startSortingSingle()
        {
            

            if (predefinedSinglePositions != null && predefinedSinglePositions.Length > 0)
            {
                AsyncConsoleWriter("ROZPOCZETO SORTOWANIE OBIEKTOW SKUPIONYCH");
                
                richTextBox8.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox8.Text += "-----------Wyslane dane do sterownika-----------";
                    richTextBox8.ScrollToCaret();
                });

                //wyczyszczenie programu
                CONNECT.NW(port);
                await Task.Delay(200);
                AsyncConsoleWriter("wyczyszczenie programu: NW 9");
                AsyncConsoleWriter8("NW");

                //ustawienie predkosci
                CONNECT.SendData(port, "10 SP 12");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [10]: 10 SP 12");
                AsyncConsoleWriter8("10 SP 12");

                //timer
                CONNECT.SendData(port, "11 TI 5");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [11]: TI 5");
                AsyncConsoleWriter8("11 TI 5");

                //otwarcie chwytaka
                CONNECT.SendData(port, "12 GO");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [12]: 11 GO");
                AsyncConsoleWriter8("12 GO");



                int temp = 0;
                int counter = 0;
                int obiekt = 0;
                //sortowanie klockow pojedynczych
                for (int i = 1; i < predefinedSinglePositions.Length; i++)
                {
                    if(predefinedSinglePositions[i] == null)
                    {
                        break;
                    }

                    //przeorientowanie do danego obiektu na bezpieczna wysokosc
                    temp = 100 + i + counter;
                    string lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " MP " + predefinedSinglePositions[i]);
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP " + predefinedSinglePositions[i]);
                    AsyncConsoleWriter8(lineNumber + " MP " + predefinedSinglePositions[i]);
                    i++;

                    //przeorientowanie do danego obiektu o odpowiedni kat chwytaka
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " MP " + predefinedSinglePositions[i]);
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP " + predefinedSinglePositions[i]);
                    AsyncConsoleWriter8(lineNumber + " MP " + predefinedSinglePositions[i]);
                    i++;

                    //przeorientowanie do pozycji podniesienia obiektu
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " MP " + predefinedSinglePositions[i]);
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP " + predefinedSinglePositions[i]);
                    AsyncConsoleWriter8(lineNumber + " MP " + predefinedSinglePositions[i]);
                    counter++;

                    //timer
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + "TI 5");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " TI 5");
                    AsyncConsoleWriter8(lineNumber + " TI 5");
                    counter++;

                    //next zamkniecie chwytaka
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " GC");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " GO");
                    AsyncConsoleWriter8(lineNumber + " GC");
                    counter++;

                    //timer
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + "TI 5");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " TI 5");
                    AsyncConsoleWriter8(lineNumber + " TI 5");
                    counter++;


                    //next podniesienie klocka
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " MP " + predefinedSinglePositions[i-1]);
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP " + predefinedSinglePositions[i]);
                    AsyncConsoleWriter8(lineNumber + " MP " + predefinedSinglePositions[i-1]);
                    counter++;

                    //next obrot do 0 stopni chwytaka
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " MP " + predefinedSinglePositions[i-2]);
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP " + predefinedSinglePositions[i]);
                    AsyncConsoleWriter8(lineNumber + " MP " + predefinedSinglePositions[i]);
                    counter++;

                    //przejechanie do pozycji srodkowej miedzy miejscem podniesienia a zrzutem bez kolizji
                    //pozcyja srodkowa: +365.50,+0.00,+304.65,+0.00,+177.50,+625.38,+0.00,R,A,C
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " MP +365.50,+0.00,+305,+0.00,+177.50,+625.38,+0.00,R,A");
                    await Task.Delay(100);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP +365.50,+0.00,+304.65,+0.00,+177.50,+625.38,+0.00,R,A");
                    AsyncConsoleWriter8(lineNumber + " MP +365.50,+0.00,+304.65,+0.00,+177.50,+625.38,+0.00,R,A");
                    counter++;

                    
                    //zrzut zaleznie od koloru
                    if(singleConts[Convert.ToInt32(predefinedSinglePositions[obiekt])].color == "red")
                    {
                        //pozcyja pjemnika red: 
                        temp = 100 + i + counter;
                        lineNumber = Convert.ToString(temp);
                        CONNECT.SendData(port, lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+373.22,+0.00,R,A");
                        await Task.Delay(200);
                        AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+373.22,+0.00,R,A");
                        AsyncConsoleWriter8(lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+373.22,+0.00,R,A");
                        counter++;
                    }
                    else if(singleConts[Convert.ToInt32(predefinedSinglePositions[obiekt])].color == "green")
                    {
                        //pozcyja pjemnika green: 
                        temp = 100 + i + counter;
                        lineNumber = Convert.ToString(temp);
                        CONNECT.SendData(port, lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+485.81,+0.00,R,A");
                        await Task.Delay(200);
                        AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+485.81,+0.00,R,A");
                        AsyncConsoleWriter8(lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+485.81,+0.00,R,A");
                        counter++;
                    }
                    else if(singleConts[Convert.ToInt32(predefinedSinglePositions[obiekt])].color == "blue")
                    {
                        //pozcyja pjemnika blue: 
                        temp = 100 + i + counter;
                        lineNumber = Convert.ToString(temp);
                        CONNECT.SendData(port, lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+259.12,+0.00,R,A");
                        await Task.Delay(200);
                        AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+259.12,+0.00,R,A");
                        AsyncConsoleWriter8(lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+259.12,+0.00,R,A");
                        counter++;
                    }
                    else
                    {
                        //pozcyja pjemnika mix: 
                        temp = 100 + i + counter;
                        lineNumber = Convert.ToString(temp);
                        CONNECT.SendData(port, lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+259.12,+0.00,R,A");
                        await Task.Delay(200);
                        AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+259.12,+0.00,R,A");
                        AsyncConsoleWriter8(lineNumber + " MP +317.66,+0.00,+304.65,+0.00,+177.50,+259.12,+0.00,R,A");
                        counter++;
                    }
                    //timer
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + "TI 5");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " TI 5");
                    AsyncConsoleWriter8(lineNumber + " TI 5");
                    counter++;

                    //otwarcie chwytaka i zrzut
                    temp = 100 + i + counter;
                    CONNECT.SendData(port, lineNumber + " GO");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " GO");
                    AsyncConsoleWriter8(lineNumber + " GO");
                    counter++;

                    //timer
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + "TI 5");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " TI 5");
                    AsyncConsoleWriter8(lineNumber + " TI 5");
                    counter++;

                    //przeorientowanie do pozycji srodkowej  do podnoszenia kolejnych klockow!
                    //pozcyja srodkowa: +365.50,+0.00,+304.65,+0.00,+177.50,+625.38,+0.00,R,A,C
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " MP +365.50,+0.00,+305,+0.00,+177.50,+625.38,+0.00,R,A");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP +365.50,+0.00,+304.65,+0.00,+177.50,+625.38,+0.00,R,A");
                    AsyncConsoleWriter8(lineNumber + " MP +365.50,+0.00,+304.65,+0.00,+177.50,+625.38,+0.00,R,A");
                    counter++;


                    //kolejny obiekt 
                    obiekt+=4;
                    i++;

                }

                //przejscie do pozycji robienia zdjecia
                CONNECT.SendData(port, "900 MP +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [900]: 20 MP  +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");
                AsyncConsoleWriter8("900 MP  +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");

                //zamkniecie chwytaka
                CONNECT.SendData(port, "901 GC");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [901]: GC");
                AsyncConsoleWriter8("901 GC");

                //zaznaczenie konca programu
                CONNECT.SendData(port, "902 ED");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [902]: ED");
                AsyncConsoleWriter8("902 ED");


                //otwarcie chwytaka
                CONNECT.SendData(port, "RN");
                await Task.Delay(200);
                AsyncConsoleWriter("komenda : RUN");
                AsyncConsoleWriter8("RN");

                //start live preview
                timer5.Start();

                return true;
            }
            return true;
        }


        int sortCounter = 0;
        private async Task startSortingConnected()
        {

            if (predefinedConnectedPositions != null)
            {
                AsyncConsoleWriter("ROZPOCZETO SORTOWANIE OBIEKTOW ROZPROSZONYCH");

                richTextBox8.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox8.Text += "-----------Wyslane dane do sterownika-----------";
                    richTextBox8.ScrollToCaret();
                });

                //wyczyszczenie programu
                CONNECT.NW(port);
                await Task.Delay(200);
                AsyncConsoleWriter("wyczyszczenie programu: NW 9");
                AsyncConsoleWriter8("NW");

                //ustawienie predkosci
                CONNECT.SendData(port, "10 SP 12");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [10]: 10 SP 12");
                AsyncConsoleWriter8("10 SP 12");

                //timer
                CONNECT.SendData(port, "11 TI 5");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [11]: TI 5");
                AsyncConsoleWriter8("11 TI 5");

                //otwarcie chwytaka
                CONNECT.SendData(port, "12 GC");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [12]: 11 GC");
                AsyncConsoleWriter8("12 GC");




                int temp = 0;
                int counter = 0;
                int obiekt = 0;
                //sortowanie klockow pojedynczych
                for (int i = 1; i < predefinedConnectedPositions.Length; i++)
                {
                    if (predefinedConnectedPositions[i] == null)
                    {
                        break;
                    }

                    //przeorientowanie do obiektu na bezpieczna wysokosc i odsuniecie
                    temp = 100 + i + counter;
                    string lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " MP " + predefinedConnectedPositions[i]);
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP " + predefinedConnectedPositions[i]);
                    AsyncConsoleWriter8(lineNumber + " MP " + predefinedConnectedPositions[i]);
                    i++;

                    //obnizenie
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " MP " + predefinedConnectedPositions[i]);
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP " + predefinedConnectedPositions[i]);
                    AsyncConsoleWriter8(lineNumber + " MP " + predefinedConnectedPositions[i]);
                    i++;

                    //wjechanie w skupisko
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " MP " + predefinedConnectedPositions[i]);
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP " + predefinedConnectedPositions[i]);
                    AsyncConsoleWriter8(lineNumber + " MP " + predefinedConnectedPositions[i]);
                    i++;


                    //timer
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + "TI 5");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " TI 5");
                    AsyncConsoleWriter8(lineNumber + " TI 5");
                    counter++;

                    //next zamkniecie chwytaka
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " GO");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " GO");
                    AsyncConsoleWriter8(lineNumber + " GO");
                    counter++;

                    //timer
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + "TI 5");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " TI 5");
                    AsyncConsoleWriter8(lineNumber + " TI 5");
                    counter++;


                    //odjechanie do gory
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " MP " + predefinedConnectedPositions[i]);
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " MP " + predefinedConnectedPositions[i]);
                    AsyncConsoleWriter8(lineNumber + " MP " + predefinedConnectedPositions[i]);
                    counter++;

                    //timer
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + "TI 5");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " TI 5");
                    AsyncConsoleWriter8(lineNumber + " TI 5");
                    counter++;

                    //next zamkniecie chwytaka
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + " GC");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " GC");
                    AsyncConsoleWriter8(lineNumber + " GC");
                    counter++;

                    //timer
                    temp = 100 + i + counter;
                    lineNumber = Convert.ToString(temp);
                    CONNECT.SendData(port, lineNumber + "TI 5");
                    await Task.Delay(200);
                    AsyncConsoleWriter("program line [" + lineNumber + "]:" + lineNumber + " TI 5");
                    AsyncConsoleWriter8(lineNumber + " TI 5");
                    counter++;




                    //next obiekt najpierw liczba wskazujac na numer obiektu
                    i++;

                }
                //przejscie do pozycji robienia zdjecia
                CONNECT.SendData(port, "900 MP +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [900]: 20 MP  +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");
                AsyncConsoleWriter8("900 MP  +380.93,-2.66,+596.28,+0.40,+176.32,+954.80,+0.00,R,A");

                //zamkniecie chwytaka
                CONNECT.SendData(port, "901 GC");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [901]: GC");
                AsyncConsoleWriter8("901 GC");

                //zaznaczenie konca programu
                CONNECT.SendData(port, "902 ED");
                await Task.Delay(200);
                AsyncConsoleWriter("program line [902]: ED");
                AsyncConsoleWriter8("902 ED");


                //otwarcie chwytaka
                CONNECT.SendData(port, "RN");
                await Task.Delay(200);
                AsyncConsoleWriter("komenda : RUN");
                AsyncConsoleWriter8("RN");


                sortCounter++;
            }
        }

        //zmienna przechowujca pozycje 
        string[] predefinedSinglePositions;
        string[] predefinedConnectedPositions;

        private async Task<bool> startVisionSystemAsync()
        {
            try
            {
                int visionDelay = 200;

                AsyncConsoleWriter("rozpoczeto przetwarzanie obrazu");

                //------------------------------------------mesurement

                Mat img = visionMeasure();

                Image oldImg = pictureBox3.Image;
                pictureBox3.Image = VISION.MatToBitmap(img);
                if (oldImg != null)
                {
                    oldImg.Dispose();
                }
                img.Dispose();
                AsyncConsoleWriter("dokonano pomiarow");
                await Task.Delay(visionDelay);


                //------------------------------------Step 1 kontury
                img =  visionContours();

                oldImg = pictureBox3.Image;
                pictureBox3.Image = VISION.MatToBitmap(img);
                if (oldImg != null)
                {
                    oldImg.Dispose();
                }
                img.Dispose();
                AsyncConsoleWriter("zakonczono szukanie konturow");
                await Task.Delay(visionDelay);


                //----------------------------------------------------------SORT OBJECTS

                img = visionSortConts();

                oldImg = pictureBox3.Image;
                pictureBox3.Image = VISION.MatToBitmap(img);
                if (oldImg != null)
                {
                    oldImg.Dispose();
                }
                img.Dispose();
                AsyncConsoleWriter("zakonczono sortowanie");
                await Task.Delay(visionDelay);


                //----------------------------------------------------------GET COLORS

                img = visionColorRec();

                oldImg = pictureBox3.Image;
                pictureBox3.Image = VISION.MatToBitmap(img);
                if (oldImg != null)
                {
                    oldImg.Dispose();
                }
                img.Dispose();
                AsyncConsoleWriter("zakonczono rozpoznawanie kolorow");
                await Task.Delay(visionDelay);


                //----------------------------------------------------------GET SHAPE

                img = visionShapeRec();

                oldImg = pictureBox3.Image;
                pictureBox3.Image = VISION.MatToBitmap(img);
                if (oldImg != null)
                {
                    oldImg.Dispose();
                }
                img.Dispose();
                AsyncConsoleWriter("zakonczono rozpoznawanie ksztaltu");
                await Task.Delay(visionDelay);

                //----------------------------------------------------------GET ANGLE

                img = visionAngleRec();

                oldImg = pictureBox3.Image;
                pictureBox3.Image = VISION.MatToBitmap(img);
                if (oldImg != null)
                {
                    oldImg.Dispose();
                }
                img.Dispose();
                AsyncConsoleWriter("zakonczono estymacje orientacji obiketow");
                await Task.Delay(visionDelay);


                //----------------------------------------------------------CREATE OBJECTS

                visionCreateOb();
                await Task.Delay(visionDelay);
                AsyncConsoleWriter("stworzono obiekty");

                //----------------------------------------------------------MAPP OBJECTS
                visionMapping();
                await Task.Delay(visionDelay);
                AsyncConsoleWriter("zmapowano obiekty");

                //----------------------------------------------------------SHOW Objects
                string message = "";
                (img,message) = visionShowOb();
                richTextBox9.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox9.Text = "\n Wykryte i zapisane obiekty: \n\n";
                    richTextBox9.Text += message;
                    richTextBox9.ScrollToCaret();
                });
                oldImg = pictureBox3.Image;
                pictureBox3.Image = VISION.MatToBitmap(img);
                AsyncConsoleWriter("zapisano obiekty");
                await Task.Delay(visionDelay);


                //odpowiednio segregowac bez kolizji


                //-----------------------------------------------------------TOOL MASKS

                img = toolMasks();

                oldImg = pictureBox3.Image;
                pictureBox3.Image = VISION.MatToBitmap(img);
                if (oldImg != null)
                {
                    oldImg.Dispose();
                }
                img.Dispose();
                AsyncConsoleWriter("dodano maski narzędzia");
                await Task.Delay(visionDelay);




                //-----------------------------------------------------------COLLISIONS ALG
                message = "";
                message = detectCollisions();

                richTextBox13.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox13.Text = "\n Detected collisions: \n\n";
                    richTextBox13.Text += message;
                    richTextBox13.ScrollToCaret();
                });
                AsyncConsoleWriter("wykryto kolizje");
                await Task.Delay(visionDelay);

                //----------------------------------------------------------PREDEFINE SINGLE POSIITONS

                message = "";
                predefinedSinglePositions = predefineSinglePositions();

                for(int i =0; i < predefinedSinglePositions.Length;i++)
                {
                    message += "\n" + predefinedSinglePositions[i];
                }

                richTextBox12.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox12.Text = "\n Predefined single Positions: \n\n";
                    richTextBox12.Text += message;
                    richTextBox12.ScrollToCaret();
                });
                await Task.Delay(visionDelay);

                //----------------------------------------------------------PREDEFINE CONNECTED POSITIONS

                message = "";
                predefinedConnectedPositions = predefineConnectedPositions();
                for (int i = 0; i < predefinedConnectedPositions.Length; i++)
                {
                    message += "\n" + predefinedConnectedPositions[i];
                }

                richTextBox12.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox12.Text += "\n Predefined Connected Positions: \n\n";
                    richTextBox12.Text += message;
                    richTextBox12.ScrollToCaret();
                });













                return true;
            }
            catch (Exception ex)
            {
                richTextBox7.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox7.Text += "\n BŁAD ---- " + ex;
                    richTextBox7.ScrollToCaret();
                });
                return false;
            }
        }

        //asking for position async
        private async Task<string> askForPositionAsync(SerialPort port)
        {
            string pozycja = "";
            string junk = port.ReadExisting();

            await Task.Delay(1000);
            string message = "WH";
            string str = CONNECT.SendData(port, message);

            await Task.Delay(1000);

            //wlasciwy odczyt pozycji
            pozycja = port.ReadExisting();
            

            //string pozycja = "+416.13,+9.33,-200.43,+1.62,+175.58,+1102.50,+0.00,R,A,O";
            //string pozycja = "+398.22,0,+628.68,0.00,+176.32,+947.41,+0.00,R,A,C";

            return pozycja;
        }
        //ask for position
        private string askForPosition(SerialPort port)
        {
            string message = "WH";
            string str = CONNECT.SendData(port, message);


            Task.Delay(500).GetAwaiter().GetResult();
            //string pozycja = port.ReadExisting();
            string pozycja = "+416.13,+9.33,-200.43,+1.62,+175.58,+1102.50,+0.00,R,A,O";
            //string pozycja = "+398.22,0,+628.68,0.00,+176.32,+947.41,+0.00,R,A,C";

            return pozycja;
        }


        private async Task<bool> GripperSafeHeightAsync()
        {
            bool safeHeight = false;

            await Task.Delay(100);
            //odczyt aktualnej pozycji
            string aktuPos = await askForPositionAsync(port);
            Task.WaitAll();


            AsyncConsoleWriter("odczyt pozycji: " + aktuPos);


            //sprawdzenie czy gripper jest na wysokosci >= 230 
            (bool safe, string pozycja, double number) = CONNECT.checkGripperHeight(aktuPos);

            await Task.Delay(10);
            AsyncConsoleWriter("chwytak na bezpiecznej  wysokosci : " + Convert.ToString(safe) + "   aktualna wysokosc: " + Convert.ToString(number));

            //warunki na wysokosc
            if (safe == false)
            {
                //przeorientowanie na bezpieczna wysokosc
                CONNECT.SendData(port, "20 MP " + pozycja);
                await Task.Delay(100);
                AsyncConsoleWriter("program line [20]: 20 MP " + pozycja);
                richTextBox8.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox8.Text += "\n20 MP " + pozycja;
                    richTextBox8.ScrollToCaret();
                });
                safeHeight = false;

            }
            else
            {
                safeHeight = true;
                AsyncConsoleWriter("Gripper jest na bezpiecznej wysokosci");
            }


            return safeHeight;
        }


        private async Task<bool> GripperInZeroAsync()
        {
            bool zeroRotation = false;

            AsyncConsoleWriter("odczyt aktualnej pozycji...");
            await Task.Delay(100);
            //odczyt aktualnej pozycji
            string aktuPos = await askForPositionAsync(port);
            Task.WaitAll();

            //tu raport
            await Task.Delay(10);
            AsyncConsoleWriter("odczyt pozycji : " + aktuPos);

            //sprawdzenie czy obrot chwytaka to 0 inaczej mozna zlamac uchwyt na kamere
            (bool safe, string pozycja) = CONNECT.checkGripperRotation(aktuPos);
            await Task.Delay(10);
            AsyncConsoleWriter("chwytak w pozycji bezpiecznej ? : " + Convert.ToString(safe));

            //jezeli chwytak obrocony to obrot do 0 stopni 
            if (safe == false)
            {

                //obrocenie samego chwytaka
                CONNECT.SendData(port, "30 MP " + pozycja);
                await Task.Delay(100);
                AsyncConsoleWriter("program line [30]: 30 MP " + pozycja);
                richTextBox8.BeginInvoke((MethodInvoker)delegate ()
                {
                    richTextBox8.Text += "\n30 MP " + pozycja;
                    richTextBox8.ScrollToCaret();
                });
            }

            return zeroRotation;
        }

        private async Task<bool> checkImgPositionAsync()
        {
            counterPosition = 0;

            while(counterPosition < 100 && imgPosition == false)
            {

                string pozycja;
                Mat klon1 = new Mat();
                Mat klon2 = new Mat();
                Mat copy = new Mat();
                Mat undistorted = new Mat();

                //pobranie pozycji ze sterownika
                pozycja = await askForPositionAsync(port);
                Task.WaitAll();
                //wlasciwa pozycja robienia zdjecia
                //"+398.22,0,+628.68,0.00,+176.32,+947.41,+0.00,R,A,C"
                /*
                 * 
                 * 
                 * 
                 * 
                if (counterPosition > 2)
                {
                    pozycja = "+398.22,0.00,+628.68,0.00,+176.32,+947.41,+0.00,R,A,C";
                    AsyncConsoleWriter(pozycja);
                }
                else
                {
                    pozycja = "+416.13,+9.33,+496.21,+1.62,+175.58,+1102.50,+0.00,R,A,O";
                    AsyncConsoleWriter(pozycja);
                }
                */


                AsyncConsoleWriter(pozycja);
                //sprawdzenie pozycji
                imgPosition = CONNECT.imgPosition(pozycja);


                if (imgPosition == true)
                {
                    timer5.Stop();
                    await Task.Delay(500);
                    capture.Read(klon1);
                    await Task.Delay(500);
                    richTextBox7.BeginInvoke((MethodInvoker)delegate ()
                    {
                        richTextBox7.Text += "\n" + "kamera na pozycji zdjecia";
                        richTextBox7.ScrollToCaret();
                    });

                    //clear programTab
                    richTextBox8.BeginInvoke((MethodInvoker)delegate ()
                    {
                        richTextBox8.Text = "";
                    });


                    //wlasciwy odczyt
                    capture.Read(klon2);
                    Cv2.Undistort(klon2, wej, InputArray.Create(cameraMatrix), InputArray.Create(distCoff), null);

                   

                    //symulowany odczyt 
                    //copy = Cv2.ImRead(@"C:\Users\MKPC\Desktop\PINZ\sample\wej.jpg", ImreadModes.Unchanged);
                    //Cv2.Undistort(copy, wej, InputArray.Create(cameraMatrix), InputArray.Create(distCoff), null);
                    
                    
                    //zarzadzanie zasobami
                    Image oldImg = pictureBox3.Image;
                    pictureBox3.Image = VISION.MatToBitmap(wej);
                    if (oldImg != null)
                    {
                        oldImg.Dispose();
                    }
                    klon1.Dispose();
                    klon2.Dispose();
                    undistorted.Dispose();

                    return true;
                }
                else if (counterPosition >= 50)
                {
                    AsyncConsoleWriter("BŁĄD POZYCJA NIEPOPRAWNA");
                }
                counterPosition++;
                AsyncConsoleWriter("Pozycja zdjecia: " + imgPosition);
                await Task.Delay(1000);
            }
            AsyncConsoleWriter("koniec pętli");
            return false;
        }


        private async Task<bool> checkSafePositionAsync()
        {
            counterPosition = 0;
            while (counterPosition < 100 && imgPosition == false)
            {
                string pozycja;
                string message = "WH";
                string str = CONNECT.SendData(port, message);
                await Task.Delay(500);

                pozycja = await askForPositionAsync(port);

                //wlasciwa pozycja robienia zdjecia
                //"+398.22,0,+628.68,0.00,+176.32,+947.41,+0.00,R,A,C"

                /*
                if (counterPosition > 2)
                {
                    pozycja = "+398.22,0.00,+240,0.00,+176.32,+947.41,+0.00,R,A,C";
                    AsyncConsoleWriter(pozycja);
                }
                else
                {
                    pozycja = "+416.13,+9.33,+496.21,+1.62,+175.58,+1102.50,+0.00,R,A,O";
                    AsyncConsoleWriter(pozycja);
                }
                */

                //AsyncConsoleWriter(pozycja);
                //------------------------------------------------------------------------------------------------------------------------------

                //sprawdzenie pozycji
                bool w = CONNECT.safePosition(pozycja);

                if (w == true)
                {

                    return true;
                }
                else if (counterPosition >= 50)
                {
                    AsyncConsoleWriter("BŁĄD POZYCJA NIEPOPRAWNA");
                }
                counterPosition++;

                await Task.Delay(500);
            }
            AsyncConsoleWriter("koniec pętli");
            return false;
        }


        //STOP BUTTON EMERGENCY
        private void button35_Click(object sender, EventArgs e)
        {
            //stop
            CONNECT.HLT(port);
            richTextBox7.Text += "\n\n";
            richTextBox7.Text += "AWARYJNIE ZATRZYMANO PRACE ROBOTA !!!!!";
        }




        //zabezpiecznie obszaru
        private async void button25_Click(object sender, EventArgs e)
        {
            try
            {

                capture2.Open(0, 0);
                capture2.Set(CaptureProperty.FrameWidth, 1280);
                capture2.Set(CaptureProperty.FrameHeight, 720);

                //rozpoczecie programu
                console7counter = 0;
                richTextBox7.Text += "\n" + "-----------SAFETY RUNNING-----------";
                console7counter++;

                await Task.Delay(500);
                await Task.Run(() => safetyAsync());
                await Task.WhenAll();

            }
            catch (Exception ex)
            {
                richTextBox3.Text += "\n AN ERROR OCCOURED, reset and try from the begining" + "\n\n" + Convert.ToString(ex);
            }


        }


        private async Task safetyAsync()
        {
            try
            {
                Mat junk = new Mat();

                capture2.Read(junk);
                await Task.Delay(200);
                capture2.Read(junk);

                Mat grab = new Mat();
                Mat grab_grey = new Mat();

                Mat frame = new Mat();
               
                Mat prev = new Mat();
                Mat prev_grey = new Mat();
                
                

                OpenCvSharp.Size mask = new OpenCvSharp.Size(3, 3);

                OpenCvSharp.Point[][] contours;
                HierarchyIndex[] hIndx;
                var kernel = new OpenCvSharp.Size(3, 3);
                safetystop = false;


                //new frame
                capture2.Read(grab);
                //gray frame
                Cv2.CvtColor(grab, grab_grey, ColorConversionCodes.BGR2GRAY);
                Cv2.GaussianBlur(grab_grey, grab_grey, mask, 0);
                await Task.Delay(10);
                prev_grey = grab_grey.Clone();

                while (safetystop == false)
                {
                    //new frame
                    capture2.Read(frame);
                    //gray frame
                    Mat frame_grey = new Mat();
                    Cv2.CvtColor(frame, frame_grey, ColorConversionCodes.BGR2GRAY);
                    Cv2.GaussianBlur(frame_grey, frame_grey, mask, 0);

                    await Task.Delay(1);

                    Mat diff = new Mat();
                    Cv2.Absdiff(frame_grey,prev_grey,diff);

                    //zmiana klatki poprzedniej
                    prev_grey = frame_grey.Clone();

                    Cv2.Dilate(diff, diff, new Mat(), default, 1);

                    Mat threshed = new Mat();
                    //threshold pokazujacy zmiany
                    Cv2.Threshold(diff, threshed, 20, 255, ThresholdTypes.Binary);

                    //test
                    Cv2.FindContours(threshed, out contours, out hIndx, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

                    Cv2.DrawContours(frame, contours, -1, new Scalar(0, 0, 255), 2, LineTypes.AntiAlias);




                    await Task.Delay(1);
                    Image oldImg = pictureBox3.Image;
                    pictureBox3.Image = VISION.MatToBitmap(frame);

                    if (hIndx.Length > 0)
                    {
                        richTextBox7.BeginInvoke((MethodInvoker)delegate ()
                        {

                            richTextBox7.Text += "\n" + "UWAGA";
                            richTextBox7.Text += "\n" + "WARNING";
                            richTextBox7.Text += "\n" + "WYKRYTO RUCH W STREFIE NIEBEZPIECZNEJ";
                            richTextBox7.Text += "\n" + "ZATRZYMANIE PRACY";

                            richTextBox7.ScrollToCaret();
                        });

                        //stop
                        CONNECT.HLT(port);
                        //richTextBox7.Text += "\n\n";
                        //richTextBox7.Text += "AWARYJNIE ZATRZYMANO PRACE ROBOTA !!!!!";

                        safetystop = true;
                    }



                    if (oldImg != null && grab != null && frame_grey != null && threshed != null && diff != null)
                    {
                        oldImg.Dispose();
                        grab.Dispose();
                        frame_grey.Dispose();
                        threshed.Dispose();
                        diff.Dispose();
                    }

                }



                if (frame != null && grab != null && prev != null && prev_grey != null)
                {
                    frame.Dispose();
                    grab.Dispose();
                    prev.Dispose();
                    prev_grey.Dispose();
                    junk.Dispose();
                }

            }
            catch (Exception ex)
            {
                AsyncConsoleWriter("blad !!: " + ex);
            }

        }


        bool safetystop = false;

        private void button38_Click(object sender, EventArgs e)
        {
            safetystop = true;
        }
















        //console window
        private void richTextBox7_TextChanged(object sender, EventArgs e)
        {
               
        }

        //stop sendind and reiveing
        private void button8_Click(object sender, EventArgs e)
        {
            flag = 0;
            timerThree.Stop();
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}




/*




private void checkImgPosition()
{
    try
    {
        timerFour.Start();

    }
    catch (Exception ex)
    {
        richTextBox7.Text += Convert.ToString(ex);
    }
}

int counterFour = 0;
bool imgPosition;
//timer 4 wait na pozycje wyslana odczekanie kilku tickow
private void TimerEventProcessor4(Object sender, EventArgs myEventArgs)
{
    //co sekunde odczyt
    consoleWriter("odczyt aktualnej pozycji...");
    string aktuPos = askForPosition(port);
    consoleWriter("odczyt pozycji : " + aktuPos);

    imgPosition = CONNECT.imgPosition(aktuPos);
    consoleWriter("image position : " + Convert.ToString(imgPosition));

    if (imgPosition == true) timerFour.Stop();

}

//timer 5 kamera
private void TimerEventProcessor5(Object sender, EventArgs myEventArgs)
{
    try
    {
        Mat undistorted = new Mat();
        if (capture.IsOpened() == false)
        {
            richTextBox7.Text = "ERROR nie znaleziono kamery";
            timer5.Stop();
        }
        else
        {
            capture.Read(wej);
        }
        Cv2.Undistort(wej, undistorted, InputArray.Create(cameraMatrix), InputArray.Create(distCoff), null);
        wej = undistorted;
        pictureBox3.Image = VISION.MatToBitmap(wej);
    }
    catch (Exception ex)
    {
        richTextBox7.Text += "\n" + ex;
    }

}



//DO ZMIANY !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
private void goToImgPosition()
{
    bool safeHeight, zeroRotation, imgPosition;
    string position = "";
    try
    {
        //wylaczenie timera odbierajacego informacje z portu rs232!
        timerTwo.Stop();


        richTextBox7.Text = "1  -----------Rozpoczeto program-----------";
        //wlaczenie kamery
        timer5.Start();

        //wybranie programu 9
        msgTab[0] = "N 9";
        consoleWriter("wybranie programu: " + msgTab[msgIndex]);
        msgIndex++;
        //wyczyszczenie programu
        msgTab[1] = "NW";
        consoleWriter("wyczyszczenie programu N 9 " + msgTab[msgIndex]);
        msgIndex++;
        //ustawienie predkosci
        msgTab[2] = "10 SP 6";
        consoleWriter("program line [10]: " + msgTab[msgIndex]);
        msgIndex++;

        //sprawdzenie czy chwytak jest na bezpiecznej wysokosci
        safeHeight = GripperSafeHeight();
        zeroRotation = GripperInZero();

        //zamkniecie chwytaka na koniec ruchu
        msgTab[msgIndex] = "100 GC";
        consoleWriter("program line [100]: " + msgTab[msgIndex]);
        msgIndex++;

        msgTab[msgIndex] = "RN";
        consoleWriter("RUN SUBPROGRAM: " + msgTab[msgIndex]);
        msgIndex++;



        //odczekaj na pozycje sprawdzanie pozycji interwalowe co 1,5 sekundy
        timerFour.Tick += new EventHandler(TimerEventProcessor4);
        timerFour.Interval = 1500;   //co sekunde sprawdza pozycje (MAYBE UPGRADE ?)
        checkImgPosition();











        //po spelnieniu warunkow przejscie do pozycji robienia zdjecia


        //w pozycji zrobienie zdjecia o odpowiedniej ostrosci
        //kamera musi byc wlaczona wczesniej przed zrobieniem zdjecia !! inaczej ostrosc do dupy !!


        //jak jest zdjecie to caly algorytm wizyjny krok po kroku w jednej metodzie !!!








        /*
        //teraz pytanie jak rozwiazac mozliwe zmiany w ustawieniu !





        string message = "", str = "";
        int counter = 0;

        //wylaczenie timera odbierajacego informacje z portu rs232!
        timerTwo.Stop();

        richTextBox7.Text = "1  -----------Rozpoczeto program-----------";
        consoleWriter("odczyt aktualnej pozycji...");

        //odczyt aktualnej pozycji
        string aktuPos = askForPosition(port);
        consoleWriter("otrzymana pozycja: " + aktuPos);

        //sprawdzenie czy obrot chwytaka to 0 inaczej mozna zlamac uchwyt na kamere
        (bool odpowiedz, string separated) = CONNECT.checkGripperRotation(aktuPos);
        consoleWriter("chwytak obrocony ? : " + Convert.ToString(odpowiedz));

        //wybranie programu 9
        msgTab[0] = "N 9";
        consoleWriter("komenda: " + msgTab[0]);

        //wyczyszczenie programu
        msgTab[1] = "NW";
        consoleWriter("komenda: " + msgTab[1]);

        //ustawienie predkosci
        msgTab[2] = "10 SP 6";
        consoleWriter("komenda: " + msgTab[2]);




        //jezeli chwytak obrocony cofnac do pozycji 0      jak nie to jazda na pozycje kamery 
        if (odpowiedz == true)
        {

            //obrocenie samego chwytaka
            msgTab[3] = "20 MP " + separated;
            consoleWriter("komenda: " + msgTab[3]);

            //powrot do pozcyji robienia zdjecia
            msgTab[4] = "30 MP +398.22,0,+628.68,0,+176.32,+947.41,+0.00,R,A";
            consoleWriter("komenda: " + msgTab[4]);

            //upewnienie sie czy chywtak jest zamkniety
            msgTab[5] = "40 GC";
            consoleWriter("komenda: " + msgTab[5]);

        }
        else
        {

            //powrot do pozycji robienia zdjecia
            msgTab[3] = "20 MP +398.22,0,+628.68,0.00,+176.32,+947.41,+0.00,R,A" + '\r';
            consoleWriter("komenda: " + msgTab[3]);

            //upewnienie sie ze chwytak jest zamkniety
            msgTab[4] = "30 GC";
            consoleWriter("komenda: " + msgTab[4]);

        }



        //wait for the program to end 

        //odczyt aktualnej pozycji
        aktuPos = askForPosition(port);
        consoleWriter("otrzymana pozycja: " + aktuPos);

        //sprawdzenie czy aktualna pozycja jest rowna zadanej
        if (aktuPos == "+398.22,0,+628.68,0.00,+176.32,+947.41,+0.00,R,A,C")
        {
            consoleWriter("aktualna pozycja jest zgodna z pozycja robienia zdjecia");
            inPosition = true;
        }
        else
        {
            consoleWriter("ERROR pozycja nie zostala osiagnieta");
            inPosition = false;
        }
        */




