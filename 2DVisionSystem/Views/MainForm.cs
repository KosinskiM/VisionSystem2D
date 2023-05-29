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
using VisionSystem;
using _2DVisionSystemLibrary.DataConnection;

namespace _2DVisionSystem
{
    public partial class MainForm : Form
    {
        double[,] CameraMatrix = new double[3,3];

        //Child Forms
        private LivePreviewForm LivePreview;


        //Constructor
        public MainForm()
        {
            InitializeComponent();
            InitializePictureBoxes();
            InitializeDropDowns();
            InitializeCameraMatrixGroupbox();
        }
        //Main Form events
        public void MainForm_Closing(object sender, CancelEventArgs e)
        {
            ImageAquisition.ReleseResources();
            Connections.CloseComPortConnection();
        }

        //Main Form Initialization
        private void InitializePictureBoxes()
        {
            homePictureBox.Size = new System.Drawing.Size(690, 730);
            homePictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            homePictureBox.Image = Conversion.MatToBitmap(Cv2.ImRead(@"C:\Projects\2DVisionSystem\2DVisionSystem\UIComponents\HomePagePicture.jpg"));
        }
        private void InitializeDropDowns()
        {
            availableCamerasDropDown.DataSource = InitializingDevices.GetAvailableCaptureDevices();
            availableCamerasDropDown.DisplayMember = "Name";

            availableComPortsDropDown.DataSource = InitializingDevices.GetAvailableComPorts();

            sampleImageDropDown.DataSource = SampleImages.GetAllSampleImageNames();
        }
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

            ReadCameraMatrix();
        }

        //Camera Matrix Group Box
        private void changeCameraMatrixButton_Click(object sender, EventArgs e)
        {
            //Camera Matrix entered text Validation
            if (ValidateCameraMatrix())
            {
                ReadCameraMatrix();
            }
            else
            {
                MessageBox.Show("Something went wrong, check all camera matrix entries and try again");
            }
        }
        private void ReadCameraMatrix()
        {
            CameraMatrix[0, 0] = double.Parse(cameraMatrix11.Text);
            CameraMatrix[0, 1] = double.Parse(cameraMatrix12.Text);
            CameraMatrix[0, 2] = double.Parse(cameraMatrix12.Text);
            CameraMatrix[1, 0] = double.Parse(cameraMatrix21.Text);
            CameraMatrix[1, 1] = double.Parse(cameraMatrix22.Text);
            CameraMatrix[1, 2] = double.Parse(cameraMatrix23.Text);
            CameraMatrix[2, 0] = double.Parse(cameraMatrix31.Text);
            CameraMatrix[2, 1] = double.Parse(cameraMatrix32.Text);
            CameraMatrix[2, 2] = double.Parse(cameraMatrix33.Text);
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

            ReadCameraMatrix();

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

            if (states.Contains(false))
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


        //Camera Connection
        private void connectCameraButton_Click(object sender, EventArgs e)
        {
            if (availableCamerasDropDown.DataSource != null)
            {
                int selectedCamera = availableCamerasDropDown.SelectedIndex;
                ImageAquisition.OpenCapture(selectedCamera,CameraMatrix);
            }
            else
            {
                MessageBox.Show("Select Camera and try again");
            }
        }
        private void disconnectCameraButton_Click(object sender, EventArgs e)
        {
            try
            {
                ImageAquisition.ReleseResources();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Select Camera and try again \n\n Error message: " + ex);
            }
        }

        //Comport Connection
        private void connectComPortButton_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedComPort  = availableComPortsDropDown.SelectedItem.ToString();
                Connections.OpenComPortConnection(selectedComPort);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong, please check connection and try again\nError message:\n"+ex);
            }
        }
        private void disconnectComPortButton_Click(object sender, EventArgs e)
        {
            try
            {
                Connections.CloseComPortConnection();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong, please try again\n Error message:\n" + ex);
            }
        }






        //LivePreview Controls
        private void livePreviewButton_Click(object sender, EventArgs e)
        {
            LivePreview = new LivePreviewForm();
            LivePreview.Show();
            calibrationMethodsBox.LivePreview = LivePreview;
            calibrationConsoles.ConsoleWriteLines("Live Preview Window opened");
        }

        private void startLivePreviewButton_Click(object sender, EventArgs e)
        {
            LivePreview.StartPreview();
            calibrationConsoles.ConsoleWriteLines("Live Preview Started ");
        }

        private void stopLivePreviewButton_Click(object sender, EventArgs e)
        {
            LivePreview.StopPreview();
            calibrationConsoles.ConsoleWriteLines("Live Preview stopped");
        }

        private void captureImageButton_Click(object sender, EventArgs e)
        {
            if (LivePreview != null)
            {
                Bitmap img = ImageAquisition.CaptureImage();
                LivePreview.Photo = img;
            }
            calibrationConsoles.ConsoleWriteLines("Image captured");

        }

        private void loadSampleImg_Click(object sender, EventArgs e)
        {
            int selectedIndex = sampleImageDropDown.SelectedIndex;
            LivePreview.LoadSampleImage(selectedIndex);
        }


        private void defaultValuesButton_Click_1(object sender, EventArgs e)
        {
            calibrationMethodsBox.DefaultValues();
            calibrationConsoles.ConsoleWriteLines("Default values applied");
        }

        private void resetCalibrationButton_Click(object sender, EventArgs e)
        {
            calibrationMethodsBox.ResetCalibration();
            calibrationConsoles.ConsoleWriteLines("Calibration restarted");
        }

        private void calibrationMethodsBox_Load(object sender, EventArgs e)
        {

        }
    }
}
