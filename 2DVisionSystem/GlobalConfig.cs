using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionSystem
{
    public class GlobalConfig
    {
        public static string getSampleImagesPath()
        {
            string sampleImagesPath = Properties.Settings.Default.sampleImagesPath;
            return sampleImagesPath;
        }

    }
}
