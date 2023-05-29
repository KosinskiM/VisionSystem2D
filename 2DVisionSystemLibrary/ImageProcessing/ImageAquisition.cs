using DirectShowLib;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionSystemLibrary.DataConnection;

namespace VisionSystemLibrary.ImageProcessing
{

    public class ImageAquisition
    {

        static VideoCapture Capture = new VideoCapture();
        static double[,] CameraMatrix = {
                {1218.120 ,0,668.99},
                {0 , 1283.9 , 444.09},
                {0,0,1}
            };
        static double[] DistCoff = { -0.094, 0.087, 0, 0, 0 };



        public static void OpenCapture(int selectedCameraIndex, double[,] cameraMatrix)
        {
            CameraMatrix = cameraMatrix;
            if (!Capture.IsOpened())
            {
                Capture.Open(0, selectedCameraIndex);
                Capture.Set(CaptureProperty.FrameWidth, 1280);
                Capture.Set(CaptureProperty.FrameHeight, 720);
            }
        }

        public static Bitmap CaptureImage()
        {
            Mat input = new Mat();
            Mat undistorted = new Mat();
            Capture.Read(input);
            Cv2.Undistort(input, undistorted, InputArray.Create(CameraMatrix), InputArray.Create(DistCoff), null);
            Bitmap output = Conversion.MatToBitmap(undistorted);
            input.Dispose();
            undistorted.Dispose();

            return output;
        }
        
        public static void ReleseResources()
        {
            if (!Capture.IsDisposed)
            {
                Capture.Release();
                Capture.Dispose();
            }
        }

        public static Image GetSampleImage(string samplePath)
        {
            Mat sampleImage = Cv2.ImRead(@samplePath, ImreadModes.Unchanged);
            Mat undistorted = new Mat();
            Cv2.Undistort(sampleImage, undistorted, InputArray.Create(CameraMatrix), InputArray.Create(DistCoff), null);
            Image output = Conversion.MatToBitmap(undistorted);
            sampleImage.Dispose();
            undistorted.Dispose();
            return output;
        }    
    }
}
