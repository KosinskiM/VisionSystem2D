using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using NumSharp;
using System.IO.Ports;
using DirectShowLib;


namespace _2DVisionSystemLibrary.DataConnection
{
    //------------------------------------------------------------ Move master commands for RV-E3J:) ---------------------------------------------------------------

    /// <summary>
    /// MoveMasterCommandWrapper for c#
    /// it contains all necessary commands to
    /// communicate mitsubishi robot with c# app
    /// </summary>
    internal class MoveMasterCommandWrapper
    {
        /// <summary>
        /// send data
        /// </summary>
        /// <param name="port"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SendData(SerialPort port, string message)
        {
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }

        /// <summary>
        /// Move Position command
        /// </summary>
        /// <param name="port"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="axis7"></param>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        public static void MP(SerialPort port, int x, int y, int z, int a, int b, int axis7, int f1, int f2)
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

        /// <summary>
        /// Move position
        /// </summary>
        /// <param name="port"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string N(SerialPort port, string number)
        {
            string message = "N " + number + '\r'; ;

            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }

        /// <summary>
        /// Read program
        /// </summary>
        /// <param name="port"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public static string PR(SerialPort port, string lineNumber)
        {
            string message = "LR " + lineNumber + '\r'; ;
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }

        /// <summary>
        /// Clear program data with postisions
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string NW(SerialPort port)
        {
            string message = "NW" + '\r'; ;
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }

        /// <summary>
        /// Stop robot movement
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string HLT(SerialPort port)
        {
            string message = "HLT" + '\r'; ;
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }

        /// <summary>
        /// Get actual postion
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string WH(SerialPort port)
        {
            string message = "WH" + '\r'; ;
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }

        /// <summary>
        /// Change program
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string N(SerialPort port)
        {
            string message = "N" + '\r'; ;
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }

        /// <summary>
        /// Deletes specified program with position data
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ProgramNumber"></param>
        /// <returns></returns>
        public static string NW(SerialPort port, string ProgramNumber)
        {
            string message = "NW " + ProgramNumber;
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }


        /// <summary>
        /// Read program number
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string QN(SerialPort port)
        {
            string message = "QN " + '\r'; ;
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }

        /// <summary>
        /// Executes the specified program
        /// </summary>
        /// <param name="port"></param>
        /// <param name="startLine"></param>
        /// <param name="endLine"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public static string RN(SerialPort port, string startLine, string endLine, string programName)
        {
            string message = "RN " + startLine + "," + endLine + "," + programName + '\r'; ;
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }

        /// <summary>
        /// Resets program nad errors
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string RS(SerialPort port)
        {
            string message = "RS 0" + '\r'; ;
            message += '\r';
            byte[] bytes = Encoding.Default.GetBytes(message);
            string str = Encoding.Default.GetString(bytes);
            port.Write(bytes, 0, bytes.Length);

            return str;
        }
    }
}
