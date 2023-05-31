using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionSystem.Models;
using VisionSystem.Views;
using VisionSystemLibrary.ImageProcessing;

namespace VisionSystem.UIComponents
{
    public partial class CalibrationMethodsBox : UserControl
    {
        //child form
        public LivePreviewForm LivePreview;

        //UserControl
        public CalibrationConsoles ConsolesControl;

        //Controller
        public VisionSystemController VisionController;

        public CalibrationMethodsBox()
        {
            InitializeComponent();
        }

        public void DefaultValues()
        {
            //Thresholding
            lowerThresholdTextBox.Text = "55";
            upperThresholdTextBox.Text = "255";

            //Contour volumes
            lowerContourTextBox.Text = "1800";
            upperContourTextbox.Text = "4400";
            bigContoursTextbox.Text = "12000";

            //Colors

            //red
            redFromTextBox.Text = "0";          //ll
            redToTextBox.Text = "15";           //lh
            redFromTwoTextBox.Text = "170";     //hl
            redToTwoTextbox.Text = "250";       //hh

            //green
            greenFromTextBox.Text = "40";
            greenToTextbox.Text = "115";

            //blue
            blueFromTextBox.Text = "115";
            blueToTextbox.Text = "170";
            ConsolesControl.ConsoleWriteLines("Default values applied");
        }

        public void ResetCalibration()
        {
            lowerThresholdTextBox.Text = "";
            upperThresholdTextBox.Text = "";
            lowerContourTextBox.Text = "";
            upperContourTextbox.Text = "";
            bigContoursTextbox.Text = "";
            redFromTextBox.Text = "";          
            redToTextBox.Text = "";           
            redFromTwoTextBox.Text = "";     
            redToTwoTextbox.Text = "";       
            greenFromTextBox.Text = "";
            greenToTextbox.Text = "";
            blueFromTextBox.Text = "";
            blueToTextbox.Text = "";
            ConsolesControl.ConsoleWriteLines("Calibration restarted");
        }

        private void checkTresholdButton_Click(object sender, EventArgs e)
        {
            if (ValidateThresholdTextBoxes() && VisionController.BaseImage != null)
            {
                VisionController.SetThreshold(lowerThresholdTextBox.Text, upperThresholdTextBox.Text);
                Image newImage = ImageOperations.ApplyGreyScale(VisionController);
                LivePreview.Photo = newImage;
                ConsolesControl.ConsoleWriteLines("Threshold applied");
            }
            else
            {
                MessageBox.Show("Values are incorrect, try again");
            }
        }

        private bool ValidateThresholdTextBoxes()
        {
            double result = 0;
            List<bool> states = new List<bool>();
            states.Add(double.TryParse(lowerThresholdTextBox.Text, out result));
            states.Add(double.TryParse(upperThresholdTextBox.Text, out result));
            if (states.Contains(false))
            {
                return false;
            }
            return true;
        }

        private void findObjectsButton_Click(object sender, EventArgs e)
        {
            //TODO Validate Thresholding
            if (VisionController.ThresholdValuesStored)
            {
                Image newImage = ImageOperations.FindContours(VisionController);
                LivePreview.Photo = newImage;
                ConsolesControl.ConsoleWriteLines("Threshold applied");
            }
            else
            {
                MessageBox.Show("No greyscale Image, check threshold first");
            }
        }

        private void sortobjectsButton_Click(object sender, EventArgs e)
        {
            if (VisionController.contours != null)
            {
                Image newImage = ImageOperations.SortContoursOnVolume(VisionController);
                LivePreview.Photo = newImage;
                ConsolesControl.ConsoleWriteLines("Objects sorted");
            }
            else
            {
                MessageBox.Show("No contours stored, find contours first");
            }
        }
    }
}
