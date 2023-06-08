namespace VisionSystem.UIComponents
{
    partial class CalibrationMethodsBox
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
            System.Windows.Forms.TabControl calibrationtabControl;
            this.thresholdingTab = new System.Windows.Forms.TabPage();
            this.findObjectsButton = new System.Windows.Forms.Button();
            this.upperContourTextbox = new System.Windows.Forms.TextBox();
            this.sortObjectsButton = new System.Windows.Forms.Button();
            this.bigContoursTextbox = new System.Windows.Forms.TextBox();
            this.lowerContourLabel = new System.Windows.Forms.Label();
            this.bigContourLabel = new System.Windows.Forms.Label();
            this.lowerContourTextBox = new System.Windows.Forms.TextBox();
            this.upperContourLabel = new System.Windows.Forms.Label();
            this.checkTresholdButton = new System.Windows.Forms.Button();
            this.lowerThresholdTextBox = new System.Windows.Forms.TextBox();
            this.lowerTreshholdLabel = new System.Windows.Forms.Label();
            this.upperThresholdTextBox = new System.Windows.Forms.TextBox();
            this.upperTresholdLabel = new System.Windows.Forms.Label();
            this.objectApperanceTab = new System.Windows.Forms.TabPage();
            this.shapeRecoqnitionButton = new System.Windows.Forms.Button();
            this.angleRecognitionButton = new System.Windows.Forms.Button();
            this.blueColorLabel = new System.Windows.Forms.Label();
            this.redColorRangeLabel = new System.Windows.Forms.Label();
            this.greenColorLabel = new System.Windows.Forms.Label();
            this.checkcolorDetectionButton = new System.Windows.Forms.Button();
            this.blueToLabel = new System.Windows.Forms.Label();
            this.redFromTextBox = new System.Windows.Forms.TextBox();
            this.blueFromLabel = new System.Windows.Forms.Label();
            this.redFromTwoTextBox = new System.Windows.Forms.TextBox();
            this.blueToTextbox = new System.Windows.Forms.TextBox();
            this.redToTextBox = new System.Windows.Forms.TextBox();
            this.blueFromTextBox = new System.Windows.Forms.TextBox();
            this.redToTwoTextbox = new System.Windows.Forms.TextBox();
            this.greenToLabel = new System.Windows.Forms.Label();
            this.redFromLabel = new System.Windows.Forms.Label();
            this.greenFromLabel = new System.Windows.Forms.Label();
            this.redFromTwoLabel = new System.Windows.Forms.Label();
            this.greenToTextbox = new System.Windows.Forms.TextBox();
            this.redToLabel = new System.Windows.Forms.Label();
            this.greenFromTextBox = new System.Windows.Forms.TextBox();
            this.redToTwoLabel = new System.Windows.Forms.Label();
            this.realDimensionsTab = new System.Windows.Forms.TabPage();
            this.showToolMasksButton = new System.Windows.Forms.Button();
            this.measureReferenceObjectsButton = new System.Windows.Forms.Button();
            this.storeInfoTab = new System.Windows.Forms.TabPage();
            this.button15 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.saveObjectsButton = new System.Windows.Forms.Button();
            this.showRealCoordinatesButton = new System.Windows.Forms.Button();
            calibrationtabControl = new System.Windows.Forms.TabControl();
            calibrationtabControl.SuspendLayout();
            this.thresholdingTab.SuspendLayout();
            this.objectApperanceTab.SuspendLayout();
            this.realDimensionsTab.SuspendLayout();
            this.storeInfoTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // calibrationtabControl
            // 
            calibrationtabControl.Controls.Add(this.thresholdingTab);
            calibrationtabControl.Controls.Add(this.objectApperanceTab);
            calibrationtabControl.Controls.Add(this.realDimensionsTab);
            calibrationtabControl.Controls.Add(this.storeInfoTab);
            calibrationtabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            calibrationtabControl.Location = new System.Drawing.Point(0, 0);
            calibrationtabControl.Name = "calibrationtabControl";
            calibrationtabControl.SelectedIndex = 0;
            calibrationtabControl.Size = new System.Drawing.Size(427, 548);
            calibrationtabControl.TabIndex = 41;
            // 
            // thresholdingTab
            // 
            this.thresholdingTab.Controls.Add(this.findObjectsButton);
            this.thresholdingTab.Controls.Add(this.upperContourTextbox);
            this.thresholdingTab.Controls.Add(this.sortObjectsButton);
            this.thresholdingTab.Controls.Add(this.bigContoursTextbox);
            this.thresholdingTab.Controls.Add(this.lowerContourLabel);
            this.thresholdingTab.Controls.Add(this.bigContourLabel);
            this.thresholdingTab.Controls.Add(this.lowerContourTextBox);
            this.thresholdingTab.Controls.Add(this.upperContourLabel);
            this.thresholdingTab.Controls.Add(this.checkTresholdButton);
            this.thresholdingTab.Controls.Add(this.lowerThresholdTextBox);
            this.thresholdingTab.Controls.Add(this.lowerTreshholdLabel);
            this.thresholdingTab.Controls.Add(this.upperThresholdTextBox);
            this.thresholdingTab.Controls.Add(this.upperTresholdLabel);
            this.thresholdingTab.Location = new System.Drawing.Point(4, 22);
            this.thresholdingTab.Name = "thresholdingTab";
            this.thresholdingTab.Padding = new System.Windows.Forms.Padding(3);
            this.thresholdingTab.Size = new System.Drawing.Size(419, 522);
            this.thresholdingTab.TabIndex = 0;
            this.thresholdingTab.Text = "Treshold";
            this.thresholdingTab.UseVisualStyleBackColor = true;
            // 
            // findObjectsButton
            // 
            this.findObjectsButton.Location = new System.Drawing.Point(90, 194);
            this.findObjectsButton.Margin = new System.Windows.Forms.Padding(2);
            this.findObjectsButton.Name = "findObjectsButton";
            this.findObjectsButton.Size = new System.Drawing.Size(225, 28);
            this.findObjectsButton.TabIndex = 31;
            this.findObjectsButton.Text = "Find Objects";
            this.findObjectsButton.UseVisualStyleBackColor = true;
            this.findObjectsButton.Click += new System.EventHandler(this.findObjectsButton_Click);
            // 
            // upperContourTextbox
            // 
            this.upperContourTextbox.Location = new System.Drawing.Point(90, 304);
            this.upperContourTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.upperContourTextbox.Name = "upperContourTextbox";
            this.upperContourTextbox.Size = new System.Drawing.Size(225, 20);
            this.upperContourTextbox.TabIndex = 35;
            this.upperContourTextbox.TextChanged += new System.EventHandler(this.upperContourTextbox_TextChanged);
            // 
            // sortObjectsButton
            // 
            this.sortObjectsButton.Location = new System.Drawing.Point(90, 381);
            this.sortObjectsButton.Margin = new System.Windows.Forms.Padding(2);
            this.sortObjectsButton.Name = "sortObjectsButton";
            this.sortObjectsButton.Size = new System.Drawing.Size(225, 28);
            this.sortObjectsButton.TabIndex = 32;
            this.sortObjectsButton.Text = "Sort Objects";
            this.sortObjectsButton.UseVisualStyleBackColor = true;
            this.sortObjectsButton.Click += new System.EventHandler(this.sortobjectsButton_Click);
            // 
            // bigContoursTextbox
            // 
            this.bigContoursTextbox.Location = new System.Drawing.Point(90, 346);
            this.bigContoursTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.bigContoursTextbox.Name = "bigContoursTextbox";
            this.bigContoursTextbox.Size = new System.Drawing.Size(225, 20);
            this.bigContoursTextbox.TabIndex = 37;
            this.bigContoursTextbox.TextChanged += new System.EventHandler(this.bigContoursTextbox_TextChanged);
            // 
            // lowerContourLabel
            // 
            this.lowerContourLabel.AutoSize = true;
            this.lowerContourLabel.Location = new System.Drawing.Point(89, 248);
            this.lowerContourLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lowerContourLabel.Name = "lowerContourLabel";
            this.lowerContourLabel.Size = new System.Drawing.Size(114, 13);
            this.lowerContourLabel.TabIndex = 34;
            this.lowerContourLabel.Text = "Lower Contour Volume";
            // 
            // bigContourLabel
            // 
            this.bigContourLabel.AutoSize = true;
            this.bigContourLabel.Location = new System.Drawing.Point(89, 331);
            this.bigContourLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.bigContourLabel.Name = "bigContourLabel";
            this.bigContourLabel.Size = new System.Drawing.Size(115, 13);
            this.bigContourLabel.TabIndex = 38;
            this.bigContourLabel.Text = "Big Contours Start Size";
            // 
            // lowerContourTextBox
            // 
            this.lowerContourTextBox.Location = new System.Drawing.Point(90, 263);
            this.lowerContourTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.lowerContourTextBox.Name = "lowerContourTextBox";
            this.lowerContourTextBox.Size = new System.Drawing.Size(225, 20);
            this.lowerContourTextBox.TabIndex = 33;
            this.lowerContourTextBox.TextChanged += new System.EventHandler(this.lowerContourTextBox_TextChanged);
            // 
            // upperContourLabel
            // 
            this.upperContourLabel.AutoSize = true;
            this.upperContourLabel.Location = new System.Drawing.Point(89, 289);
            this.upperContourLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.upperContourLabel.Name = "upperContourLabel";
            this.upperContourLabel.Size = new System.Drawing.Size(114, 13);
            this.upperContourLabel.TabIndex = 36;
            this.upperContourLabel.Text = "Upper Contour Volume";
            // 
            // checkTresholdButton
            // 
            this.checkTresholdButton.Location = new System.Drawing.Point(90, 162);
            this.checkTresholdButton.Margin = new System.Windows.Forms.Padding(2);
            this.checkTresholdButton.Name = "checkTresholdButton";
            this.checkTresholdButton.Size = new System.Drawing.Size(225, 28);
            this.checkTresholdButton.TabIndex = 2;
            this.checkTresholdButton.Text = "Check";
            this.checkTresholdButton.UseVisualStyleBackColor = true;
            this.checkTresholdButton.Click += new System.EventHandler(this.checkTresholdButton_Click);
            // 
            // lowerThresholdTextBox
            // 
            this.lowerThresholdTextBox.Location = new System.Drawing.Point(90, 78);
            this.lowerThresholdTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.lowerThresholdTextBox.Name = "lowerThresholdTextBox";
            this.lowerThresholdTextBox.Size = new System.Drawing.Size(225, 20);
            this.lowerThresholdTextBox.TabIndex = 13;
            this.lowerThresholdTextBox.TextChanged += new System.EventHandler(this.lowerThresholdTextBox_TextChanged);
            // 
            // lowerTreshholdLabel
            // 
            this.lowerTreshholdLabel.AutoSize = true;
            this.lowerTreshholdLabel.Location = new System.Drawing.Point(89, 63);
            this.lowerTreshholdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lowerTreshholdLabel.Name = "lowerTreshholdLabel";
            this.lowerTreshholdLabel.Size = new System.Drawing.Size(72, 13);
            this.lowerTreshholdLabel.TabIndex = 14;
            this.lowerTreshholdLabel.Text = "Lower Thresh";
            // 
            // upperThresholdTextBox
            // 
            this.upperThresholdTextBox.Location = new System.Drawing.Point(90, 127);
            this.upperThresholdTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.upperThresholdTextBox.Name = "upperThresholdTextBox";
            this.upperThresholdTextBox.Size = new System.Drawing.Size(225, 20);
            this.upperThresholdTextBox.TabIndex = 15;
            this.upperThresholdTextBox.TextChanged += new System.EventHandler(this.upperThresholdTextBox_TextChanged);
            // 
            // upperTresholdLabel
            // 
            this.upperTresholdLabel.AutoSize = true;
            this.upperTresholdLabel.Location = new System.Drawing.Point(91, 112);
            this.upperTresholdLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.upperTresholdLabel.Name = "upperTresholdLabel";
            this.upperTresholdLabel.Size = new System.Drawing.Size(72, 13);
            this.upperTresholdLabel.TabIndex = 16;
            this.upperTresholdLabel.Text = "Upper Thresh";
            // 
            // objectApperanceTab
            // 
            this.objectApperanceTab.Controls.Add(this.shapeRecoqnitionButton);
            this.objectApperanceTab.Controls.Add(this.angleRecognitionButton);
            this.objectApperanceTab.Controls.Add(this.blueColorLabel);
            this.objectApperanceTab.Controls.Add(this.redColorRangeLabel);
            this.objectApperanceTab.Controls.Add(this.greenColorLabel);
            this.objectApperanceTab.Controls.Add(this.checkcolorDetectionButton);
            this.objectApperanceTab.Controls.Add(this.blueToLabel);
            this.objectApperanceTab.Controls.Add(this.redFromTextBox);
            this.objectApperanceTab.Controls.Add(this.blueFromLabel);
            this.objectApperanceTab.Controls.Add(this.redFromTwoTextBox);
            this.objectApperanceTab.Controls.Add(this.blueToTextbox);
            this.objectApperanceTab.Controls.Add(this.redToTextBox);
            this.objectApperanceTab.Controls.Add(this.blueFromTextBox);
            this.objectApperanceTab.Controls.Add(this.redToTwoTextbox);
            this.objectApperanceTab.Controls.Add(this.greenToLabel);
            this.objectApperanceTab.Controls.Add(this.redFromLabel);
            this.objectApperanceTab.Controls.Add(this.greenFromLabel);
            this.objectApperanceTab.Controls.Add(this.redFromTwoLabel);
            this.objectApperanceTab.Controls.Add(this.greenToTextbox);
            this.objectApperanceTab.Controls.Add(this.redToLabel);
            this.objectApperanceTab.Controls.Add(this.greenFromTextBox);
            this.objectApperanceTab.Controls.Add(this.redToTwoLabel);
            this.objectApperanceTab.Location = new System.Drawing.Point(4, 22);
            this.objectApperanceTab.Name = "objectApperanceTab";
            this.objectApperanceTab.Size = new System.Drawing.Size(419, 522);
            this.objectApperanceTab.TabIndex = 2;
            this.objectApperanceTab.Text = "Objects Apperance";
            this.objectApperanceTab.UseVisualStyleBackColor = true;
            // 
            // shapeRecoqnitionButton
            // 
            this.shapeRecoqnitionButton.Location = new System.Drawing.Point(62, 337);
            this.shapeRecoqnitionButton.Margin = new System.Windows.Forms.Padding(2);
            this.shapeRecoqnitionButton.Name = "shapeRecoqnitionButton";
            this.shapeRecoqnitionButton.Size = new System.Drawing.Size(274, 25);
            this.shapeRecoqnitionButton.TabIndex = 37;
            this.shapeRecoqnitionButton.Text = "Shape Recognition";
            this.shapeRecoqnitionButton.UseVisualStyleBackColor = true;
            this.shapeRecoqnitionButton.Click += new System.EventHandler(this.shapeRecoqnitionButton_Click);
            // 
            // angleRecognitionButton
            // 
            this.angleRecognitionButton.Location = new System.Drawing.Point(62, 387);
            this.angleRecognitionButton.Margin = new System.Windows.Forms.Padding(2);
            this.angleRecognitionButton.Name = "angleRecognitionButton";
            this.angleRecognitionButton.Size = new System.Drawing.Size(274, 27);
            this.angleRecognitionButton.TabIndex = 36;
            this.angleRecognitionButton.Text = "Angle Recognition";
            this.angleRecognitionButton.UseVisualStyleBackColor = true;
            this.angleRecognitionButton.Click += new System.EventHandler(this.angleRecognitionButton_Click);
            // 
            // blueColorLabel
            // 
            this.blueColorLabel.AutoSize = true;
            this.blueColorLabel.Location = new System.Drawing.Point(30, 212);
            this.blueColorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.blueColorLabel.Name = "blueColorLabel";
            this.blueColorLabel.Size = new System.Drawing.Size(86, 13);
            this.blueColorLabel.TabIndex = 35;
            this.blueColorLabel.Text = "blue color range:";
            // 
            // redColorRangeLabel
            // 
            this.redColorRangeLabel.AutoSize = true;
            this.redColorRangeLabel.Location = new System.Drawing.Point(30, 98);
            this.redColorRangeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.redColorRangeLabel.Name = "redColorRangeLabel";
            this.redColorRangeLabel.Size = new System.Drawing.Size(81, 13);
            this.redColorRangeLabel.TabIndex = 1;
            this.redColorRangeLabel.Text = "red color range:";
            // 
            // greenColorLabel
            // 
            this.greenColorLabel.AutoSize = true;
            this.greenColorLabel.Location = new System.Drawing.Point(30, 155);
            this.greenColorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.greenColorLabel.Name = "greenColorLabel";
            this.greenColorLabel.Size = new System.Drawing.Size(93, 13);
            this.greenColorLabel.TabIndex = 34;
            this.greenColorLabel.Text = "green color range:";
            // 
            // checkcolorDetectionButton
            // 
            this.checkcolorDetectionButton.Location = new System.Drawing.Point(62, 283);
            this.checkcolorDetectionButton.Margin = new System.Windows.Forms.Padding(2);
            this.checkcolorDetectionButton.Name = "checkcolorDetectionButton";
            this.checkcolorDetectionButton.Size = new System.Drawing.Size(274, 24);
            this.checkcolorDetectionButton.TabIndex = 0;
            this.checkcolorDetectionButton.Text = "Check Color Detection";
            this.checkcolorDetectionButton.UseVisualStyleBackColor = true;
            this.checkcolorDetectionButton.Click += new System.EventHandler(this.checkcolorDetectionButton_Click);
            // 
            // blueToLabel
            // 
            this.blueToLabel.AutoSize = true;
            this.blueToLabel.Location = new System.Drawing.Point(189, 197);
            this.blueToLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.blueToLabel.Name = "blueToLabel";
            this.blueToLabel.Size = new System.Drawing.Size(43, 13);
            this.blueToLabel.TabIndex = 33;
            this.blueToLabel.Text = "to (130)";
            // 
            // redFromTextBox
            // 
            this.redFromTextBox.Location = new System.Drawing.Point(129, 98);
            this.redFromTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.redFromTextBox.Name = "redFromTextBox";
            this.redFromTextBox.Size = new System.Drawing.Size(56, 20);
            this.redFromTextBox.TabIndex = 18;
            this.redFromTextBox.TextChanged += new System.EventHandler(this.redFromTextBox_TextChanged);
            // 
            // blueFromLabel
            // 
            this.blueFromLabel.AutoSize = true;
            this.blueFromLabel.Location = new System.Drawing.Point(127, 197);
            this.blueFromLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.blueFromLabel.Name = "blueFromLabel";
            this.blueFromLabel.Size = new System.Drawing.Size(54, 13);
            this.blueFromLabel.TabIndex = 32;
            this.blueFromLabel.Text = "from (100)";
            // 
            // redFromTwoTextBox
            // 
            this.redFromTwoTextBox.Location = new System.Drawing.Point(250, 98);
            this.redFromTwoTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.redFromTwoTextBox.Name = "redFromTwoTextBox";
            this.redFromTwoTextBox.Size = new System.Drawing.Size(58, 20);
            this.redFromTwoTextBox.TabIndex = 22;
            this.redFromTwoTextBox.TextChanged += new System.EventHandler(this.redFromTwoTextBox_TextChanged);
            // 
            // blueToTextbox
            // 
            this.blueToTextbox.Location = new System.Drawing.Point(189, 212);
            this.blueToTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.blueToTextbox.Name = "blueToTextbox";
            this.blueToTextbox.Size = new System.Drawing.Size(57, 20);
            this.blueToTextbox.TabIndex = 31;
            this.blueToTextbox.TextChanged += new System.EventHandler(this.blueToTextbox_TextChanged);
            // 
            // redToTextBox
            // 
            this.redToTextBox.Location = new System.Drawing.Point(189, 98);
            this.redToTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.redToTextBox.Name = "redToTextBox";
            this.redToTextBox.Size = new System.Drawing.Size(57, 20);
            this.redToTextBox.TabIndex = 19;
            this.redToTextBox.TextChanged += new System.EventHandler(this.redToTextBox_TextChanged);
            // 
            // blueFromTextBox
            // 
            this.blueFromTextBox.Location = new System.Drawing.Point(129, 212);
            this.blueFromTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.blueFromTextBox.Name = "blueFromTextBox";
            this.blueFromTextBox.Size = new System.Drawing.Size(56, 20);
            this.blueFromTextBox.TabIndex = 30;
            this.blueFromTextBox.TextChanged += new System.EventHandler(this.blueFromTextBox_TextChanged);
            // 
            // redToTwoTextbox
            // 
            this.redToTwoTextbox.Location = new System.Drawing.Point(312, 98);
            this.redToTwoTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.redToTwoTextbox.Name = "redToTwoTextbox";
            this.redToTwoTextbox.Size = new System.Drawing.Size(58, 20);
            this.redToTwoTextbox.TabIndex = 23;
            this.redToTwoTextbox.TextChanged += new System.EventHandler(this.redToTwoTextbox_TextChanged);
            // 
            // greenToLabel
            // 
            this.greenToLabel.AutoSize = true;
            this.greenToLabel.Location = new System.Drawing.Point(189, 140);
            this.greenToLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.greenToLabel.Name = "greenToLabel";
            this.greenToLabel.Size = new System.Drawing.Size(37, 13);
            this.greenToLabel.TabIndex = 29;
            this.greenToLabel.Text = "to (85)";
            // 
            // redFromLabel
            // 
            this.redFromLabel.AutoSize = true;
            this.redFromLabel.Location = new System.Drawing.Point(126, 83);
            this.redFromLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.redFromLabel.Name = "redFromLabel";
            this.redFromLabel.Size = new System.Drawing.Size(42, 13);
            this.redFromLabel.TabIndex = 20;
            this.redFromLabel.Text = "from (0)";
            // 
            // greenFromLabel
            // 
            this.greenFromLabel.AutoSize = true;
            this.greenFromLabel.Location = new System.Drawing.Point(127, 140);
            this.greenFromLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.greenFromLabel.Name = "greenFromLabel";
            this.greenFromLabel.Size = new System.Drawing.Size(48, 13);
            this.greenFromLabel.TabIndex = 28;
            this.greenFromLabel.Text = "from (35)";
            // 
            // redFromTwoLabel
            // 
            this.redFromTwoLabel.AutoSize = true;
            this.redFromTwoLabel.Location = new System.Drawing.Point(247, 83);
            this.redFromTwoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.redFromTwoLabel.Name = "redFromTwoLabel";
            this.redFromTwoLabel.Size = new System.Drawing.Size(54, 13);
            this.redFromTwoLabel.TabIndex = 24;
            this.redFromTwoLabel.Text = "from (160)";
            // 
            // greenToTextbox
            // 
            this.greenToTextbox.Location = new System.Drawing.Point(189, 155);
            this.greenToTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.greenToTextbox.Name = "greenToTextbox";
            this.greenToTextbox.Size = new System.Drawing.Size(57, 20);
            this.greenToTextbox.TabIndex = 27;
            this.greenToTextbox.TextChanged += new System.EventHandler(this.greenToTextbox_TextChanged);
            // 
            // redToLabel
            // 
            this.redToLabel.AutoSize = true;
            this.redToLabel.Location = new System.Drawing.Point(186, 83);
            this.redToLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.redToLabel.Name = "redToLabel";
            this.redToLabel.Size = new System.Drawing.Size(37, 13);
            this.redToLabel.TabIndex = 21;
            this.redToLabel.Text = "to (15)";
            // 
            // greenFromTextBox
            // 
            this.greenFromTextBox.Location = new System.Drawing.Point(129, 155);
            this.greenFromTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.greenFromTextBox.Name = "greenFromTextBox";
            this.greenFromTextBox.Size = new System.Drawing.Size(56, 20);
            this.greenFromTextBox.TabIndex = 26;
            this.greenFromTextBox.TextChanged += new System.EventHandler(this.greenFromTextBox_TextChanged);
            // 
            // redToTwoLabel
            // 
            this.redToTwoLabel.AutoSize = true;
            this.redToTwoLabel.Location = new System.Drawing.Point(312, 83);
            this.redToTwoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.redToTwoLabel.Name = "redToTwoLabel";
            this.redToTwoLabel.Size = new System.Drawing.Size(43, 13);
            this.redToTwoLabel.TabIndex = 25;
            this.redToTwoLabel.Text = "to (180)";
            // 
            // realDimensionsTab
            // 
            this.realDimensionsTab.Controls.Add(this.showRealCoordinatesButton);
            this.realDimensionsTab.Controls.Add(this.showToolMasksButton);
            this.realDimensionsTab.Controls.Add(this.measureReferenceObjectsButton);
            this.realDimensionsTab.Location = new System.Drawing.Point(4, 22);
            this.realDimensionsTab.Name = "realDimensionsTab";
            this.realDimensionsTab.Size = new System.Drawing.Size(419, 522);
            this.realDimensionsTab.TabIndex = 4;
            this.realDimensionsTab.Text = "Real Dimensions";
            this.realDimensionsTab.UseVisualStyleBackColor = true;
            // 
            // showToolMasksButton
            // 
            this.showToolMasksButton.Location = new System.Drawing.Point(66, 115);
            this.showToolMasksButton.Margin = new System.Windows.Forms.Padding(2);
            this.showToolMasksButton.Name = "showToolMasksButton";
            this.showToolMasksButton.Size = new System.Drawing.Size(274, 28);
            this.showToolMasksButton.TabIndex = 47;
            this.showToolMasksButton.Text = "Show Tool Masks";
            this.showToolMasksButton.UseVisualStyleBackColor = true;
            this.showToolMasksButton.Click += new System.EventHandler(this.showToolMasksButton_Click);
            // 
            // measureReferenceObjectsButton
            // 
            this.measureReferenceObjectsButton.Location = new System.Drawing.Point(66, 28);
            this.measureReferenceObjectsButton.Margin = new System.Windows.Forms.Padding(2);
            this.measureReferenceObjectsButton.Name = "measureReferenceObjectsButton";
            this.measureReferenceObjectsButton.Size = new System.Drawing.Size(274, 28);
            this.measureReferenceObjectsButton.TabIndex = 46;
            this.measureReferenceObjectsButton.Text = "Measure Reference Objects";
            this.measureReferenceObjectsButton.UseVisualStyleBackColor = true;
            this.measureReferenceObjectsButton.Click += new System.EventHandler(this.measureReferenceObjectsButton_Click);
            // 
            // storeInfoTab
            // 
            this.storeInfoTab.Controls.Add(this.button15);
            this.storeInfoTab.Controls.Add(this.button14);
            this.storeInfoTab.Controls.Add(this.saveObjectsButton);
            this.storeInfoTab.Location = new System.Drawing.Point(4, 22);
            this.storeInfoTab.Name = "storeInfoTab";
            this.storeInfoTab.Size = new System.Drawing.Size(419, 522);
            this.storeInfoTab.TabIndex = 3;
            this.storeInfoTab.Text = "Store Info";
            this.storeInfoTab.UseVisualStyleBackColor = true;
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(83, 105);
            this.button15.Margin = new System.Windows.Forms.Padding(2);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(225, 31);
            this.button15.TabIndex = 44;
            this.button15.Text = "Show Objects In Groups";
            this.button15.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(83, 70);
            this.button14.Margin = new System.Windows.Forms.Padding(2);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(225, 31);
            this.button14.TabIndex = 43;
            this.button14.Text = "Show Single Objects";
            this.button14.UseVisualStyleBackColor = true;
            // 
            // saveObjectsButton
            // 
            this.saveObjectsButton.Location = new System.Drawing.Point(83, 35);
            this.saveObjectsButton.Margin = new System.Windows.Forms.Padding(2);
            this.saveObjectsButton.Name = "saveObjectsButton";
            this.saveObjectsButton.Size = new System.Drawing.Size(225, 31);
            this.saveObjectsButton.TabIndex = 42;
            this.saveObjectsButton.Text = "Save Objects";
            this.saveObjectsButton.UseVisualStyleBackColor = true;
            this.saveObjectsButton.Click += new System.EventHandler(this.saveObjectsButton_Click_1);
            // 
            // showRealCoordinatesButton
            // 
            this.showRealCoordinatesButton.Location = new System.Drawing.Point(66, 72);
            this.showRealCoordinatesButton.Margin = new System.Windows.Forms.Padding(2);
            this.showRealCoordinatesButton.Name = "showRealCoordinatesButton";
            this.showRealCoordinatesButton.Size = new System.Drawing.Size(274, 28);
            this.showRealCoordinatesButton.TabIndex = 48;
            this.showRealCoordinatesButton.Text = "Show Real Coordinates";
            this.showRealCoordinatesButton.UseVisualStyleBackColor = true;
            this.showRealCoordinatesButton.Click += new System.EventHandler(this.showRealCoordinatesButton_Click);
            // 
            // CalibrationMethodsBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(calibrationtabControl);
            this.Name = "CalibrationMethodsBox";
            this.Size = new System.Drawing.Size(427, 548);
            calibrationtabControl.ResumeLayout(false);
            this.thresholdingTab.ResumeLayout(false);
            this.thresholdingTab.PerformLayout();
            this.objectApperanceTab.ResumeLayout(false);
            this.objectApperanceTab.PerformLayout();
            this.realDimensionsTab.ResumeLayout(false);
            this.storeInfoTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage thresholdingTab;
        private System.Windows.Forms.Button findObjectsButton;
        private System.Windows.Forms.TextBox upperContourTextbox;
        private System.Windows.Forms.Button sortObjectsButton;
        private System.Windows.Forms.TextBox bigContoursTextbox;
        private System.Windows.Forms.Label lowerContourLabel;
        private System.Windows.Forms.Label bigContourLabel;
        private System.Windows.Forms.TextBox lowerContourTextBox;
        private System.Windows.Forms.Label upperContourLabel;
        private System.Windows.Forms.Button checkTresholdButton;
        private System.Windows.Forms.TextBox lowerThresholdTextBox;
        private System.Windows.Forms.Label lowerTreshholdLabel;
        private System.Windows.Forms.TextBox upperThresholdTextBox;
        private System.Windows.Forms.Label upperTresholdLabel;
        private System.Windows.Forms.TabPage objectApperanceTab;
        private System.Windows.Forms.Label blueColorLabel;
        private System.Windows.Forms.Label redColorRangeLabel;
        private System.Windows.Forms.Label greenColorLabel;
        private System.Windows.Forms.Button checkcolorDetectionButton;
        private System.Windows.Forms.Label blueToLabel;
        private System.Windows.Forms.TextBox redFromTextBox;
        private System.Windows.Forms.Label blueFromLabel;
        private System.Windows.Forms.TextBox redFromTwoTextBox;
        private System.Windows.Forms.TextBox blueToTextbox;
        private System.Windows.Forms.TextBox redToTextBox;
        private System.Windows.Forms.TextBox blueFromTextBox;
        private System.Windows.Forms.TextBox redToTwoTextbox;
        private System.Windows.Forms.Label greenToLabel;
        private System.Windows.Forms.Label redFromLabel;
        private System.Windows.Forms.Label greenFromLabel;
        private System.Windows.Forms.Label redFromTwoLabel;
        private System.Windows.Forms.TextBox greenToTextbox;
        private System.Windows.Forms.Label redToLabel;
        private System.Windows.Forms.TextBox greenFromTextBox;
        private System.Windows.Forms.Label redToTwoLabel;
        private System.Windows.Forms.Button shapeRecoqnitionButton;
        private System.Windows.Forms.Button angleRecognitionButton;
        private System.Windows.Forms.TabPage storeInfoTab;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button saveObjectsButton;
        private System.Windows.Forms.TabPage realDimensionsTab;
        private System.Windows.Forms.Button showToolMasksButton;
        private System.Windows.Forms.Button measureReferenceObjectsButton;
        private System.Windows.Forms.Button showRealCoordinatesButton;
    }
}
