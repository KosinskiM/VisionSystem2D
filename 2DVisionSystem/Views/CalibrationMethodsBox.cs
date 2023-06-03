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
using VisionSystemLibrary.VisionSystemLogic;

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

        //input textbox data validation

        private bool ValidateInputNumber(string input)
        {
            double result = 0;
            if (double.TryParse(input, out result))
            {
                return true;
            }
            return false;
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
        private void lowerThresholdTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(lowerThresholdTextBox.Text))
            {
                VisionController.ChangeLowerThreshold(lowerThresholdTextBox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void upperThresholdTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(upperThresholdTextBox.Text))
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
        private void lowerContourTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(lowerContourTextBox.Text))
            {
                VisionController.ChangeLowerVolume(lowerContourTextBox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");

        }
        private void upperContourTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(upperContourTextbox.Text))
            {
                VisionController.ChangeUpperVolume(upperContourTextbox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void bigContoursTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(bigContoursTextbox.Text))
            {
                VisionController.ChangeBigVolume(bigContoursTextbox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void sortobjectsButton_Click(object sender, EventArgs e)
        {
            if (VisionController.Contours != null)
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

        private void saveObjectsButton_Click(object sender, EventArgs e)
        {
            if (VisionController.SmallContoursSorted != null || VisionController.ConnectedContoursSorted != null)
            {
                StoredElementOperations.CreateStoredElements(VisionController);
            }
            else
            {
                MessageBox.Show("No contours stored, find contours first");
            }
        }


        //colors 
        private void redFromTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(redFromTextBox.Text))
            {
                VisionController.ChangeFromRedRange(redFromTextBox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void redToTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(redToTextBox.Text))
            {
                VisionController.ChangeFromRedRange(redToTextBox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void redFromTwoTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(redFromTwoTextBox.Text))
            {
                VisionController.ChangeFromRedRange(redFromTwoTextBox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void redToTwoTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(redToTwoTextbox.Text))
            {
                VisionController.ChangeFromRedRange(redToTwoTextbox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void greenFromTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(greenFromTextBox.Text))
            {
                VisionController.ChangeFromRedRange(greenFromTextBox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void greenToTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(greenToTextbox.Text))
            {
                VisionController.ChangeFromRedRange(greenToTextbox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void blueFromTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(blueFromTextBox.Text))
            {
                VisionController.ChangeFromRedRange(blueFromTextBox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }
        private void blueToTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ValidateInputNumber(blueToTextbox.Text))
            {
                VisionController.ChangeFromRedRange(blueToTextbox.Text);
            }
            else
                MessageBox.Show("Please insert a valid number and try again !");
        }

        private void checkcolorDetectionButton_Click(object sender, EventArgs e)
        {
            //validate values if good
            //pass range values
        }

    }
}
