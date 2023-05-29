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
using VisionSystemLibrary.ImageProcessing;

namespace VisionSystem.UIComponents
{
    public partial class CalibrationMethodsBox : UserControl
    {
        public LivePreviewForm LivePreview;

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
        }

        private void checkTresholdButton_Click(object sender, EventArgs e)
        {
            //TODO DataValidation
            Image livePreviewImage = LivePreview.Photo;
            Image newImage = ImageOperations.ConvertToGreyScale(livePreviewImage,lowerThresholdTextBox.Text,upperThresholdTextBox.Text);
            LivePreview.Photo = newImage;
        }
    }
}
