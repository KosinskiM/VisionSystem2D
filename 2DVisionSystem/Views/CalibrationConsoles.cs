﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionSystem.Views
{
    public partial class CalibrationConsoles : UserControl
    {
        public CalibrationConsoles()
        {
            InitializeComponent();
        }


        //TODO take some code to class
        public void ConsoleWriteLines(string text)
        {
            StringBuilder content = new StringBuilder();
            content.Append(consoleTextBox.Text);
            //messge = 1#   Live Preview Window opened
            int lineCounter = 0;
            for(int i = 0; i < content.Length; i++)
            {
                if(content[i] == '\n')
                {
                    lineCounter++;
                }
            }
            content.AppendLine($"{ lineCounter }#   "+ text);

            consoleTextBox.Text = content.ToString();
        }

        public void ObjectsInfoWriteLines(string text)
        {
            StringBuilder content = new StringBuilder();
            content.Append(objectsInformationTextBox.Text);
            //messge = 1#   Live Preview Window opened
            int lineCounter = 0;
            for (int i = 0; i < content.Length; i++)
            {
                if (content[i] == '\n')
                {
                    lineCounter++;
                }
            }
            content.AppendLine($"{ lineCounter }#   " + text);

            objectsInformationTextBox.Text = content.ToString();
        }

    }
}
