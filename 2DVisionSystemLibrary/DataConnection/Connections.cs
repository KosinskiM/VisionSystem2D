using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionSystemLibrary.DataConnection
{
    public class Connections
    {
        static SerialPort port;


        public static void OpenComPortConnection(string selectedComPort)
        {
            port = new SerialPort(selectedComPort, 9600, Parity.Even, 8, StopBits.Two);
            port.Handshake = Handshake.None;
            port.ReadTimeout = 500;
            port.WriteTimeout = 500;
            port.Open();
        }

        public static void CloseComPortConnection()
        {
            if (port.IsOpen)
            {
                port.Close();
                port.Dispose();
            }
        }
    }
}
