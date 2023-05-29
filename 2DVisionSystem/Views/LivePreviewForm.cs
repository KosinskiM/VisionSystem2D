using _2DVisionSystem;
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
using VisionSystemLibrary.DataConnection;
using VisionSystemLibrary.ImageProcessing;

namespace VisionSystem
{
    public partial class LivePreviewForm : Form
    {
        public Image Photo
        {
            get 
            {
                return LivePreviewPictureBox.Image;
            }
            set 
            { 
                LivePreviewPictureBox.Image = value;
            }
        }


        System.Timers.Timer LivePreviewTimer = new System.Timers.Timer();

        public LivePreviewForm()
        {
            InitializeComponent();
        }



        public void CaptureImage()
        {
            using (Image img = ImageAquisition.CaptureImage())
            {
                LivePreviewPictureBox.Image = img;
            }
        }

        public void StartPreview()
        {

            LivePreviewTimer.Elapsed += LivePreviewTimerEventProcessor;
            LivePreviewTimer.Interval = 100;
            LivePreviewTimer.Start();
        }

        public void StopPreview()
        {
            LivePreviewTimer.Stop();
        }


        private void LivePreviewTimerEventProcessor(Object sender, EventArgs myEventArgs)
        {
            try
            {
                Image capturedImage = ImageAquisition.CaptureImage();
                Image oldImg = LivePreviewPictureBox.Image;
                if (oldImg != null)
                {
                    oldImg.Dispose();
                }
                LivePreviewPictureBox.Image = capturedImage;
            }
            catch (Exception ex)
            {
                LivePreviewTimer.Stop();
                MessageBox.Show("Something went wrong with image aquisition, please try again\n\n Error message:\n\n" + ex.Message);
                this.Invoke((MethodInvoker)delegate
                {
                    this.Close();
                });
            }
        }

        public void LoadSampleImage(int sampleImageIndex)
        {
            Image sampleImage = SampleImages.GetSampleImage(sampleImageIndex);
            LivePreviewPictureBox.Image = sampleImage;
        }

        public void CheckThreshold()
        {

        }

    }
}
