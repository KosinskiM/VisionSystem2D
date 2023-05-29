using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionSystem.Models
{
    public class StoredObject
    {
        //Fields

        private int id;            //index 0,1,2,3,4...       
        private Point center;        //srodek konturu
        private OpenCvSharp.Point2d realCenter;    //rzeczywisty srodek konturu
        private float angle;         //obrot konturu od osi x
        private float realAngle;
        private string color;        //kolor konturu
        private string shape;        //ksztalt konturu
        private OpenCvSharp.Point[] corners;  //cornery
        private OpenCvSharp.Point[] contour;  //wszystkie punkty konturu
        private int[] collisions;


        //Props
        public int Id 
        {
            get { return id; } 
            set { id = value; }
        }
        public Point Center
        {
            get { return center; }
            set { center = value; }
        }
        public OpenCvSharp.Point2d RealCenter
        {
            get { return realCenter; }
            set { realCenter = value; }
        }
        public float Angle
        {
            get { return angle; }
            set { Angle = value; }
        }
        public float RealAngle
        {
            get { return realAngle; }
            set { realAngle = value; }
        }
        public string Color
        {
            get { return color; }
            set { color = value; }
        }
        public string Shape
        {
            get { return shape; }
            set { shape = value; }
        }
        public OpenCvSharp.Point[] Corners
        {
            get { return corners; }
            set { corners = value; }
        }
        public OpenCvSharp.Point[] Contour
        {
            get { return contour; }
            set { contour = value; }
        }
        public int[] Collisons
        {
            get { return collisions; }
            set { collisions = value; }
        }
    }
}
