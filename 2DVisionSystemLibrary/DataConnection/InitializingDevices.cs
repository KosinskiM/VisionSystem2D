using DirectShowLib;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionSystemLibrary.DataConnection
{
    public class InitializingDevices
    {
        public static string[] GetAvailableComPorts()
        {
            return SerialPort.GetPortNames();
        }

        public static List<DsDevice> GetAvailableCaptureDevices()
        {
            return new List<DsDevice>(DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice));
        }

    }
}
