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

namespace kalibracja
{
    public partial class kalibracja : Form
    {
        //szachownice
        Mat[] camAngles = new Mat[10];

        //timer
        static Timer oneTimer = new Timer();
        static int alarmCounter = 1;

        //zrodlo zdjec
        private FrameSource frameSource;


        public kalibracja()
        {
            InitializeComponent();
        }

        


        
        private void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            Mat calib = new Mat();

            bool found;
            
            
            if(alarmCounter > 11)
            {
                oneTimer.Stop();
            }
            else
            {
                frameSource.NextFrame(camAngles[alarmCounter]);

               


                alarmCounter += 1;
            }

            //eukildean distance
            //Cv2.Norm
        }

        private void calib()
        {




        }





        private void button1_Click(object sender, EventArgs e)
        {

            frameSource = Cv2.CreateFrameSource_Camera(0);

            //zdefiniowanie parametrow timera
            oneTimer.Tick += new EventHandler(TimerEventProcessor);
            oneTimer.Interval = 10000;
            oneTimer.Start();

        }
        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void kalibracja_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
