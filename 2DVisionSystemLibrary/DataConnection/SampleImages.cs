using System;
using System.Collections.Generic;
using OpenCvSharp;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using VisionSystemLibrary.ImageProcessing;
using System.Configuration;

namespace VisionSystemLibrary.DataConnection
{
    public class SampleImages
    {
        private static string[] sampleImagePaths = Directory.GetFiles(ConfigurationManager.AppSettings["sampleImagesPath"]);

        public static List<string> GetAllSampleImageNames()
        {
            List<string> sampleNames = new List<string>();

            foreach (string sampleName in sampleImagePaths)
            {
                sampleNames.Add(Path.GetFileName(sampleName));
            }
            return sampleNames;
        }

        public static Image GetSampleImage(int sampleImageIndex)
        {
            string selectedPath = sampleImagePaths[sampleImageIndex];

            Image sampleImage = ImageAquisition.GetSampleImage(selectedPath);

            return sampleImage;
        }
    }
}


//List<string> sampleImagePaths = new List<string>();
//foreach (string sampleImagePath in junkSampleImagePaths)
//{
//    StringBuilder word = new StringBuilder();
//    StringBuilder path = new StringBuilder();

//    for (int i = 0; i < sampleImagePath.Length;)
//    {
//        char letter = sampleImagePath[i];
//        if (i + 1 < sampleImagePath.Length)
//        {
//            if (letter == '\\')
//            {
//                word.Append(letter);
//                path.Append(word);
//                word.Clear();
//                i++;
//            }
//        }
//        if (letter != '\\')
//        {
//            word.Append(letter);
//            i++;
//        }
//    }
//    path.Append(word);
//    sampleImagePaths.Add("@" + path.ToString());
//}

//string selectedPath = "@" + sampleImagePaths[sampleImageIndex + 1].ToString();