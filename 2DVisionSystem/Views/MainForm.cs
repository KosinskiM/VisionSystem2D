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
using VisionSystem.Models;

namespace _2DVisionSystem
{
    public partial class MainForm : Form
    {
        VisionSystemController VisionSystemController = new VisionSystemController();

        double[,] CameraMatrix = new double[3, 3];

        //Child Forms
        private LivePreviewForm LivePreview;


        //Constructor
        public MainForm()
        {
            InitializeComponent();
            InitializePictureBoxes();
            InitializeDropDowns();
            InitializeCameraMatrixGroupbox();
            InitializeCalibrationConsoles();
            InitializeVisionSystemController();
            HomeTabConsoleWrite("Application Started");
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
            //TODO use IO to load image !
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
        private void InitializeCalibrationConsoles()
        {
            calibrationMethodsBox.ConsolesControl = calibrationConsoles;
        }
        private void InitializeVisionSystemController()
        {
            calibrationMethodsBox.VisionController = VisionSystemController;
        }


        //Main Form Home Tab

        //Home Tab console
        private void HomeTabConsoleWrite(string text)
        {
            StringBuilder content = new StringBuilder();
            content.Append(homeTabConsole.Text);
            //messge = 1#   Live Preview Window opened
            int lineCounter = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == '\n')
                {
                    lineCounter++;
                }
            }
            content.AppendLine($"{ lineCounter }#   " + text);
            homeTabConsole.Text = content.ToString();
        }

        //Camera Matrix Group Box
        private void changeCameraMatrixButton_Click(object sender, EventArgs e)
        {
            //Camera Matrix entered text Validation
            if (ValidateCameraMatrix())
            {
                ReadCameraMatrix();
                HomeTabConsoleWrite("Camera Matrix in Vision System changed");
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
            HomeTabConsoleWrite("Default Camera Matrix values restored");

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

        //Camera Connection
        private void connectCameraButton_Click(object sender, EventArgs e)
        {
            if (availableCamerasDropDown.DataSource != null)
            {
                int selectedCamera = availableCamerasDropDown.SelectedIndex;
                ImageAquisition.OpenCapture(selectedCamera,CameraMatrix);
                HomeTabConsoleWrite($"Connected with { availableCamerasDropDown.SelectedItem } Camera");
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
                HomeTabConsoleWrite($"Disconnected with { availableCamerasDropDown.SelectedItem } Camera");
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
                HomeTabConsoleWrite($"Connected with { availableComPortsDropDown.SelectedItem } Com Port");
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
                HomeTabConsoleWrite($"Connected with { availableComPortsDropDown.SelectedItem } Com Port");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong, please try again\n Error message:\n" + ex);
            }
        }


        /// ToolStripMenu
        private void introductionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form Introduction = new IntroductionForm();
            Introduction.Show();
        }


        //LivePreview Controls
        private void livePreviewButton_Click(object sender, EventArgs e)
        {
            if (LivePreview == null)
            {
                LivePreview = new LivePreviewForm();
                LivePreview.Show();
                calibrationMethodsBox.LivePreview = LivePreview;
                calibrationConsoles.ConsoleWriteLines("Live Preview Window opened");
            }
            else MessageBox.Show("Live Preview Window is already opened");
        }
        private void startLivePreviewButton_Click(object sender, EventArgs e)
        {
            if (LivePreview != null)
            {
                LivePreview.StartPreview();
                calibrationConsoles.ConsoleWriteLines("Live Preview Started "); 
            }
            else MessageBox.Show("Please open Live Preview Window first");
        }
        private void stopLivePreviewButton_Click(object sender, EventArgs e)
        {
            if (LivePreview != null)
            {
                LivePreview.StopPreview();
                calibrationConsoles.ConsoleWriteLines("Live Preview stopped");
            }
            else MessageBox.Show("Please open Live Preview Window first");
        }
        private void captureImageButton_Click(object sender, EventArgs e)
        {
            if (LivePreview != null)
            {
                Bitmap img = ImageAquisition.CaptureImage();
                LivePreview.Photo = img;
                VisionSystemController.SetBaseImage(img);
                calibrationConsoles.ConsoleWriteLines("Image captured");
            }
            else MessageBox.Show("Please open Live Preview Window first");
        }
        private void loadSampleImg_Click(object sender, EventArgs e)
        {
            if (LivePreview != null)
            {
                int selectedIndex = sampleImageDropDown.SelectedIndex;
                LivePreview.LoadSampleImage(selectedIndex);
                Image img = LivePreview.Photo;
                VisionSystemController.SetBaseImage(img);
                calibrationConsoles.ConsoleWriteLines($"Sample Image { sampleImageDropDown.SelectedItem } Loaded");
            }
            else MessageBox.Show("Please open Live Preview Window first");
        }
        private void defaultValuesButton_Click_1(object sender, EventArgs e)
        {
            calibrationMethodsBox.DefaultValues();
        }
        private void resetCalibrationButton_Click(object sender, EventArgs e)
        {
            calibrationMethodsBox.ResetCalibration();
        }
   

    }
}
