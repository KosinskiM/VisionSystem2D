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


        //Thresholding
        private bool ValidateThresholdLowerTextBox()
        {
            double result = 0;
            if (double.TryParse(lowerThresholdTextBox.Text, out result))
            {
                return true;
            }
            return false;
        }
        private bool ValidateThresholdUpperTextBox()
        {
            double result = 0;
            if (double.TryParse(upperThresholdTextBox.Text, out result))
            {
                return true;
            }
            return false;
        }
        private void lowerThresholdTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateThresholdLowerTextBox())
            {
                VisionController.ChangeLowerThreshold(lowerThresholdTextBox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void upperThresholdTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateThresholdUpperTextBox())
            {
                VisionController.ChangeUpperThreshold(upperThresholdTextBox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void checkTresholdButton_Click(object sender, EventArgs e)
        {
            if (VisionController.BaseImage != null)
            {
                Image newImage = ImageOperations.ApplyGreyScale(VisionController);
                LivePreview.Photo = newImage;
                ConsolesControl.ConsoleWriteLines("Threshold applied");
            }
            else
            {
                MessageBox.Show("No Image found, take a photo or load sample image and try again !");
            }
        }


        //Finding objects
        private void findObjectsButton_Click(object sender, EventArgs e)
        {
            //TODO Validate Thresholding
            if (VisionController.ThresholdValuesStored)
            {
                Image newImage = ImageOperations.FindContours(VisionController);
                LivePreview.Photo = newImage;
                ConsolesControl.ConsoleWriteLines("Objects Found");
            }
            else
            {
                MessageBox.Show("No greyscale Image, check threshold first");
            }
        }

        //Volume textBoxes and Objects
        private bool ValidateLowerVolumeTextBox()
        {
            double result = 0;
            if (double.TryParse(lowerContourTextBox.Text, out result))
            {
                return true;
            }
            return false;
        }
        private bool ValidateUpperVolumeTextBox()
        {
            double result = 0;
            if (double.TryParse(upperContourTextbox.Text, out result))
            {
                return true;
            }
            return false;
        }
        private bool ValidateBigVolumeTextBox()
        {
            double result = 0;
            if (double.TryParse(bigContoursTextbox.Text, out result))
            {
                return true;
            }
            return false;
        }
        private void lowerContourTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateLowerVolumeTextBox())
            {
                VisionController.ChangeLowerVolume(lowerContourTextBox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");

        }
        private void upperContourTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateUpperVolumeTextBox())
            {
                VisionController.ChangeUpperVolume(upperContourTextbox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void bigContoursTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateBigVolumeTextBox())
            {
                VisionController.ChangeBigVolume(bigContoursTextbox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
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
