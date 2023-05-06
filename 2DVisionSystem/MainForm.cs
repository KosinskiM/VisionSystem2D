using DirectShowLib;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionSystemLibrary.ImageProcessing;
using VisionSystemLibrary.DataConnection;

namespace _2DVisionSystem
{
    public partial class MainForm : Form
    {
        //Main fields

        //Image aqusition
        List<DsDevice> Cameras = InitializingDevices.GetAvailableCaptureDevices();
        VideoCapture capture = new VideoCapture();

        string[] ComPorts = InitializingDevices.GetAvailableComPorts();
        double[,] cameraMatrix = {
                {1218.120 ,0,668.99},
                {0 , 1283.9 , 444.09},
                {0,0,1}
        };



        public MainForm()
        {
            InitializeComponent();
            InitializePictureBoxes();
            InitializeDropDowns();
            InitializeCameraMatrixGroupbox();
        }

        private void InitializePictureBoxes()
        {
            homePictureBox.Size = new System.Drawing.Size(690, 730);
            homePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            homePictureBox.Image = Conversion.MatToBitmap(Cv2.ImRead(@"C:\Projects\2DVisionSystem\2DVisionSystem\UIComponents\HomePagePicture.jpg"));
        }

        private void InitializeDropDowns()
        {
            availableCamerasDropDown.DataSource = Cameras;
            availableCamerasDropDown.DisplayMember = "Name";

            availableComPortsDropDown.DataSource = ComPorts;

        }
        


        //Camera Matrix Group Box
        private void InitializeCameraMatrixGroupbox()
        {
            //Default matrix for camera model DFK42BUC03

            cameraMatrix11.Text = "1218.120";
            cameraMatrix12.Text = "0";
            cameraMatrix13.Text = "668.99";

            cameraMatrix21.Text = "0";
            cameraMatrix22.Text = "1283.9";
            cameraMatrix23.Text = "444.09";

            cameraMatrix31.Text = "0";
            cameraMatrix32.Text = "0";
            cameraMatrix33.Text = "1";
        }

        private void changeCameraMatrixButton_Click(object sender, EventArgs e)
        {
            //Camera Matrix entered text Validation
            if (ValidateCameraMatrix())
            {
                cameraMatrix[0, 0] = double.Parse(cameraMatrix11.Text);
                cameraMatrix[0, 1] = double.Parse(cameraMatrix12.Text);
                cameraMatrix[0, 2] = double.Parse(cameraMatrix12.Text);
                cameraMatrix[1, 0] = double.Parse(cameraMatrix21.Text);
                cameraMatrix[1, 1] = double.Parse(cameraMatrix22.Text);
                cameraMatrix[1, 2] = double.Parse(cameraMatrix23.Text);
                cameraMatrix[2, 0] = double.Parse(cameraMatrix31.Text);
                cameraMatrix[2, 1] = double.Parse(cameraMatrix32.Text);
                cameraMatrix[2, 2] = double.Parse(cameraMatrix33.Text);
            }
            else
            {
                MessageBox.Show("Something went wrong, check all camera matrix entries and try again");
            }
        }
        private void restoreDefaultMatrixButton_Click(object sender, EventArgs e)
        {
            cameraMatrix11.Text = "1218.120";
            cameraMatrix12.Text = "0";
            cameraMatrix13.Text = "668.99";

            cameraMatrix21.Text = "0";
            cameraMatrix22.Text = "1283.9";
            cameraMatrix23.Text = "444.09";

            cameraMatrix31.Text = "0";
            cameraMatrix32.Text = "0";
            cameraMatrix33.Text = "1";

            cameraMatrix[0, 0] = double.Parse(cameraMatrix11.Text);
            cameraMatrix[0, 1] = double.Parse(cameraMatrix12.Text);
            cameraMatrix[0, 2] = double.Parse(cameraMatrix12.Text);
            cameraMatrix[1, 0] = double.Parse(cameraMatrix21.Text);
            cameraMatrix[1, 1] = double.Parse(cameraMatrix22.Text);
            cameraMatrix[1, 2] = double.Parse(cameraMatrix23.Text);
            cameraMatrix[2, 0] = double.Parse(cameraMatrix31.Text);
            cameraMatrix[2, 1] = double.Parse(cameraMatrix32.Text);
            cameraMatrix[2, 2] = double.Parse(cameraMatrix33.Text);

        }

        private bool ValidateCameraMatrix()
        {
            double result = 0;
            List<bool> states = new List<bool>();
            states.Add(double.TryParse(cameraMatrix11.Text, out result));
            states.Add(double.TryParse(cameraMatrix12.Text, out result));
            states.Add(double.TryParse(cameraMatrix13.Text, out result));
            states.Add(double.TryParse(cameraMatrix21.Text, out result));
            states.Add(double.TryParse(cameraMatrix22.Text, out result));
            states.Add(double.TryParse(cameraMatrix23.Text, out result));
            states.Add(double.TryParse(cameraMatrix31.Text, out result));
            states.Add(double.TryParse(cameraMatrix32.Text, out result));
            states.Add(double.TryParse(cameraMatrix33.Text, out result));

            if(states.Contains(false))
            {
                return false;
            }
            return true;
        }
        

        /// ToolStripMenu
        private void introductionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Introduction = new IntroductionForm();
            Introduction.Show();
        }

        private void connectCameraButton_Click(object sender, EventArgs e)
        {
            if (availableCamerasDropDown.DataSource != null)
            {
                capture.Open(0, availableCamerasDropDown.SelectedIndex);
                capture.Set(CaptureProperty.FrameWidth, 1280);
                capture.Set(CaptureProperty.FrameHeight, 720);
            }
            else
            {
                MessageBox.Show("Select Camera and try again");
            }
        }

        private void disconnectCameraButton_Click(object sender, EventArgs e)
        {
            if (capture != null)
            {
                capture.Release();
                capture.Dispose();
            }
            else
            {
                MessageBox.Show("Select Camera and try again");
            }
        }
    }
}
