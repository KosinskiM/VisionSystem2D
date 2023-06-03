using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionSystem.Models;
using VisionSystemLibrary.ImageProcessing;

namespace VisionSystemLibrary.VisionSystemLogic
{
    public class StoredElementOperations
    {
        public static void CreateStoredElements(VisionSystemController VisionController)
        {
            ////przygotowanie zmiennych
            //Mat imageClone = wej.Clone();

            //string color, shape;
            //int angle;
            //OpenCvSharp.Point c = new OpenCvSharp.Point();
            //OpenCvSharp.Point[] cor;

            //OpenCvSharp.Point[][] smallContoursSorted = VisionController.SmallContoursSorted;
            //OpenCvSharp.Point[][] connectedContoursSorted = VisionController.ConnectedContoursSorted;
            
            ////Store Small contours
            //for (int i = 0; i < smallContoursSorted.Length; i++)
            //{
            //    StoredElement element = new StoredElement();

               
            //    //center z momentow 
            //    element.Center = ImageOperations.GetCenter(smallContoursSorted[i])

            //    //kolor
            //    element.Color = ImageOperations.ContourColor(imageClone, smallContoursSorted[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text),
            //        Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text), Convert.ToInt32(textBox13.Text),
            //        Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

            //    //shape 
            //    shape = VISION.GetShape(smallContours[i]);

            //    //angle
            //    angle = VISION.GetAngle(imageClone, smallContours[i]);

            //    //corners
            //    cor = VISION.GetCorners(smallContours[i]);

            //    //tworzeni kazdego obiektu
            //    singleConts[i] = new ContourOb()
            //    {
            //        indx = i,        //index 0,1,2,3,4...       
            //        center = c,      //srodek konturu
            //        angle = angle,   //obrot konturu od osi x
            //        color = color,   //kolor konturu
            //        shape = shape,   //ksztalt konturu
            //        corners = cor,
            //        contour = smallContours[i],   //wszystkie punkty konturu
            //    };


            //    //organizacja naroznikow
            //    OpenCvSharp.Point temp0, temp1, temp2, temp3;
            //    if (singleConts[i].angle > 90)
            //    {

            //        temp0 = singleConts[i].corners[0];
            //        temp1 = singleConts[i].corners[1];
            //        temp2 = singleConts[i].corners[2];
            //        temp3 = singleConts[i].corners[3];
            //        singleConts[i].corners[0] = temp1;
            //        singleConts[i].corners[1] = temp2;
            //        singleConts[i].corners[2] = temp3;
            //        singleConts[i].corners[3] = temp0;

            //    }


            //}

            //for (int i = 0; i < connectedContours.Length; i++)
            //{
            //    //center z momentow 
            //    c = VISION.GetCenter(connectedContours[i]);

            //    //kolor
            //    color = VISION.ContourColor(imageClone, connectedContours[i], Convert.ToInt32(textBox9.Text), Convert.ToInt32(textBox10.Text),
            //        Convert.ToInt32(textBox12.Text), Convert.ToInt32(textBox11.Text), Convert.ToInt32(textBox14.Text),
            //        Convert.ToInt32(textBox13.Text), Convert.ToInt32(textBox16.Text), Convert.ToInt32(textBox15.Text));

            //    //shape 
            //    shape = VISION.GetShape(connectedContours[i]);

            //    //angle
            //    angle = VISION.GetAngle(imageClone, connectedContours[i]);

            //    //corners
            //    cor = VISION.GetCorners(connectedContours[i]);

            //    //tworzeni kazdego obiektu
            //    connectedConts[i] = new ContourOb()
            //    {
            //        indx = i,        //index 0,1,2,3,4...       
            //        center = c,      //srodek konturu
            //        angle = angle,   //obrot konturu od osi x
            //        color = color,   //kolor konturu
            //        shape = shape,   //ksztalt konturu
            //        corners = cor,
            //        contour = connectedContours[i],   //wszystkie punkty konturu
            //    };

            //    //organizacja naroznikow
            //    OpenCvSharp.Point temp0, temp1, temp2, temp3;
            //    if (connectedConts[i].angle > 90)
            //    {

            //        temp0 = connectedConts[i].corners[0];
            //        temp1 = connectedConts[i].corners[1];
            //        temp2 = connectedConts[i].corners[2];
            //        temp3 = connectedConts[i].corners[3];

            //        connectedConts[i].corners[0] = temp1;
            //        connectedConts[i].corners[1] = temp2;
            //        connectedConts[i].corners[2] = temp3;
            //        connectedConts[i].corners[3] = temp0;
            //    }




            //}


            //VisionController.StoredSmallElement.Add(element);
        }


    }
}
