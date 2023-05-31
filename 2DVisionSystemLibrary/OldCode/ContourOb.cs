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
using Point = OpenCvSharp.Point;


namespace Clear_version_robotApp
{
    class ContourOb
    {
        //pola klasy contur obiekt
        public int indx;            //index 0,1,2,3,4...       
        public Point center;        //srodek konturu
        public OpenCvSharp.Point2d realCenter;    //rzeczywisty srodek konturu
        public float angle;         //obrot konturu od osi x
        public float realAngle;
        public string color;        //kolor konturu
        public string shape;        //ksztalt konturu
        public OpenCvSharp.Point[] corners;  //cornery
        public OpenCvSharp.Point[] contour;  //wszystkie punkty konturu
        public int[] collisions;
    }
}
