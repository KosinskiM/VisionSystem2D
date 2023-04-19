using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;
using NumSharp;
using SystemWizyjny;
using System.IO.Ports;
using DirectShowLib;

namespace SystemWizyjny
{
    class Connect
    {
        //------------------------------------------------------- wszystkie konwersje -------------------------------------------------------
        
        //--------------------------------------------------- float -> hex / hex -> float ---------------------------------------------------
        public static string FloatToHexString(float f)
        {
            var bytes = BitConverter.GetBytes(f);
            var i = BitConverter.ToInt32(bytes, 0);

            return "0x" + i.ToString("X8");
        }
        public static float FromHexStringToFloat(string s)
        {
            var i = Convert.ToInt32(s, 16);
            var bytes = BitConverter.GetBytes(i);

            return BitConverter.ToSingle(bytes, 0);
        }

        //-------------------------------------------------------------- int to byte[] -------------------------------------------------------
        
        public static byte[] IntToByte(int x)
        {
            byte[] array = BitConverter.GetBytes(x);

            return array;
        }




        //------------------------------------------------- RS232 functionalities ------------------------------------------------------------

        //send data
        public static string SendData(SerialPort port, string message)
        {
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }

        //recive data
        public static string ReciveData(SerialPort port)
        {
            string read = port.ReadExisting();

            return read;

        }










        //------------------------------------------------------------ Move master commands for RV-E3J:) ---------------------------------------------------------------
        public static void MP(SerialPort port, int x, int y, int z, int a, int b,int axis7, int f1, int f2)
        {
            //wersja string
            string message = "MP" + " " + Convert.ToString(x) + "," + Convert.ToString(y) + "," + Convert.ToString(z) +
                            "," + Convert.ToString(a) + "," + Convert.ToString(b) + "," + Convert.ToString(axis7) + ",";
            if (f1 == 0)
            {
                message += "R" + ",";
            }
            else
            {
                message += "L" + ",";
            }

            if (f2 == 0)
            {
                message += "A" + ",";
            }
            else
            {
                message += "B" + ",";
            }

            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);
        }




        //change program
        public static string N(SerialPort port, string number)
        {
            string message = "N " + number;

            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            //port.Write(bytes, 0, bytes.Length);

            return str;
        }



        //read program
        public static string PR(SerialPort port,string lineNumber)
        {
            string message = "LR " + lineNumber;
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }


        //clear program data and positions
        public static string NW(SerialPort port)
        {
            string message = "NW";
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            //port.Write(bytes, 0, bytes.Length);

            return str;
        }


        //STOP ROBOT MOVEMENT
        public static string HLT(SerialPort port)
        {
            string message = "HLT";
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            //port.Write(bytes, 0, bytes.Length);

            return str;
        }


        public static string WH(SerialPort port)
        {
            string message = "WH";
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            //port.Write(bytes, 0, bytes.Length);

            return str;
        }



        public static string[] msgTabClear(string[] msgTab)
        {
            for(int i=0;i<msgTab.Length;i++)
            {
                msgTab[i] = "";
            }

           return msgTab;
        }




    }
}
