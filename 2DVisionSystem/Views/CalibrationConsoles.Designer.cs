namespace VisionSystem.Views
{
    partial class CalibrationConsoles
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.consoleWindowLabel = new System.Windows.Forms.Label();
            this.consoleTextBox = new System.Windows.Forms.RichTextBox();
            this.objectsInformationTextBox = new System.Windows.Forms.RichTextBox();
            this.objectsInformationLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // consoleWindowLabel
            // 
            this.consoleWindowLabel.AutoSize = true;
            this.consoleWindowLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.consoleWindowLabel.Location = new System.Drawing.Point(123, 10);
            this.consoleWindowLabel.Name = "consoleWindowLabel";
            this.consoleWindowLabel.Size = new System.Drawing.Size(173, 25);
            this.consoleWindowLabel.TabIndex = 44;
            this.consoleWindowLabel.Text = "Console Window";
            // 
            // consoleTextBox
            // 
            this.consoleTextBox.Location = new System.Drawing.Point(13, 38);
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.Size = new System.Drawing.Size(439, 699);
            this.consoleTextBox.TabIndex = 43;
            this.consoleTextBox.Text = "";
            // 
            // objectsInformationTextBox
            // 
            this.objectsInformationTextBox.Location = new System.Drawing.Point(458, 38);
            this.objectsInformationTextBox.Name = "objectsInformationTextBox";
            this.objectsInformationTextBox.Size = new System.Drawing.Size(439, 699);
            this.objectsInformationTextBox.TabIndex = 45;
            this.objectsInformationTextBox.Text = "";
            // 
            // objectsInformationLabel
            // 
            this.objectsInformationLabel.AutoSize = true;
            this.objectsInformationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.objectsInformationLabel.Location = new System.Drawing.Point(589, 10);
            this.objectsInformationLabel.Name = "objectsInformationLabel";
            this.objectsInformationLabel.Size = new System.Drawing.Size(197, 25);
            this.objectsInformationLabel.TabIndex = 46;
            this.objectsInformationLabel.Text = "Objects Information";
            // 
            // CalibrationConsoles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.objectsInformationLabel);
            this.Controls.Add(this.objectsInformationTextBox);
            this.Controls.Add(this.consoleWindowLabel);
            this.Controls.Add(this.consoleTextBox);
            this.Name = "CalibrationConsoles";
            this.Size = new System.Drawing.Size(911, 752);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label consoleWindowLabel;
        private System.Windows.Forms.RichTextBox consoleTextBox;
        private System.Windows.Forms.RichTextBox objectsInformationTextBox;
        private System.Windows.Forms.Label objectsInformationLabel;
    }
}
