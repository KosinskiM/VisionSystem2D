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
            this.thresholdingPage = new System.Windows.Forms.TabPage();
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
            this.objectApperancePage = new System.Windows.Forms.TabPage();
            this.measureRealDimensionsButton = new System.Windows.Forms.Button();
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
            this.storeInfo = new System.Windows.Forms.TabPage();
            this.button15 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.saveObjectsButton = new System.Windows.Forms.Button();
            calibrationtabControl = new System.Windows.Forms.TabControl();
            calibrationtabControl.SuspendLayout();
            this.thresholdingPage.SuspendLayout();
            this.objectApperancePage.SuspendLayout();
            this.storeInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // calibrationtabControl
            // 
            calibrationtabControl.Controls.Add(this.thresholdingPage);
            calibrationtabControl.Controls.Add(this.objectApperancePage);
            calibrationtabControl.Controls.Add(this.storeInfo);
            calibrationtabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            calibrationtabControl.Location = new System.Drawing.Point(0, 0);
            calibrationtabControl.Name = "calibrationtabControl";
            calibrationtabControl.SelectedIndex = 0;
            calibrationtabControl.Size = new System.Drawing.Size(427, 548);
            calibrationtabControl.TabIndex = 41;
            // 
            // thresholdingPage
            // 
            this.thresholdingPage.Controls.Add(this.findObjectsButton);
            this.thresholdingPage.Controls.Add(this.upperContourTextbox);
            this.thresholdingPage.Controls.Add(this.sortObjectsButton);
            this.thresholdingPage.Controls.Add(this.bigContoursTextbox);
            this.thresholdingPage.Controls.Add(this.lowerContourLabel);
            this.thresholdingPage.Controls.Add(this.bigContourLabel);
            this.thresholdingPage.Controls.Add(this.lowerContourTextBox);
            this.thresholdingPage.Controls.Add(this.upperContourLabel);
            this.thresholdingPage.Controls.Add(this.checkTresholdButton);
            this.thresholdingPage.Controls.Add(this.lowerThresholdTextBox);
            this.thresholdingPage.Controls.Add(this.lowerTreshholdLabel);
            this.thresholdingPage.Controls.Add(this.upperThresholdTextBox);
            this.thresholdingPage.Controls.Add(this.upperTresholdLabel);
            this.thresholdingPage.Location = new System.Drawing.Point(4, 22);
            this.thresholdingPage.Name = "thresholdingPage";
            this.thresholdingPage.Padding = new System.Windows.Forms.Padding(3);
            this.thresholdingPage.Size = new System.Drawing.Size(419, 522);
            this.thresholdingPage.TabIndex = 0;
            this.thresholdingPage.Text = "Treshold";
            this.thresholdingPage.UseVisualStyleBackColor = true;
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
            // objectApperancePage
            // 
            this.objectApperancePage.Controls.Add(this.measureRealDimensionsButton);
            this.objectApperancePage.Controls.Add(this.shapeRecoqnitionButton);
            this.objectApperancePage.Controls.Add(this.angleRecognitionButton);
            this.objectApperancePage.Controls.Add(this.blueColorLabel);
            this.objectApperancePage.Controls.Add(this.redColorRangeLabel);
            this.objectApperancePage.Controls.Add(this.greenColorLabel);
            this.objectApperancePage.Controls.Add(this.checkcolorDetectionButton);
            this.objectApperancePage.Controls.Add(this.blueToLabel);
            this.objectApperancePage.Controls.Add(this.redFromTextBox);
            this.objectApperancePage.Controls.Add(this.blueFromLabel);
            this.objectApperancePage.Controls.Add(this.redFromTwoTextBox);
            this.objectApperancePage.Controls.Add(this.blueToTextbox);
            this.objectApperancePage.Controls.Add(this.redToTextBox);
            this.objectApperancePage.Controls.Add(this.blueFromTextBox);
            this.objectApperancePage.Controls.Add(this.redToTwoTextbox);
            this.objectApperancePage.Controls.Add(this.greenToLabel);
            this.objectApperancePage.Controls.Add(this.redFromLabel);
            this.objectApperancePage.Controls.Add(this.greenFromLabel);
            this.objectApperancePage.Controls.Add(this.redFromTwoLabel);
            this.objectApperancePage.Controls.Add(this.greenToTextbox);
            this.objectApperancePage.Controls.Add(this.redToLabel);
            this.objectApperancePage.Controls.Add(this.greenFromTextBox);
            this.objectApperancePage.Controls.Add(this.redToTwoLabel);
            this.objectApperancePage.Location = new System.Drawing.Point(4, 22);
            this.objectApperancePage.Name = "objectApperancePage";
            this.objectApperancePage.Size = new System.Drawing.Size(419, 522);
            this.objectApperancePage.TabIndex = 2;
            this.objectApperancePage.Text = "Objects Apperance";
            this.objectApperancePage.UseVisualStyleBackColor = true;
            // 
            // measureRealDimensionsButton
            // 
            this.measureRealDimensionsButton.Location = new System.Drawing.Point(62, 464);
            this.measureRealDimensionsButton.Margin = new System.Windows.Forms.Padding(2);
            this.measureRealDimensionsButton.Name = "measureRealDimensionsButton";
            this.measureRealDimensionsButton.Size = new System.Drawing.Size(274, 28);
            this.measureRealDimensionsButton.TabIndex = 38;
            this.measureRealDimensionsButton.Text = "Measure Real Dimensions";
            this.measureRealDimensionsButton.UseVisualStyleBackColor = true;
            // 
            // shapeRecoqnitionButton
            // 
            this.shapeRecoqnitionButton.Location = new System.Drawing.Point(62, 359);
            this.shapeRecoqnitionButton.Margin = new System.Windows.Forms.Padding(2);
            this.shapeRecoqnitionButton.Name = "shapeRecoqnitionButton";
            this.shapeRecoqnitionButton.Size = new System.Drawing.Size(274, 25);
            this.shapeRecoqnitionButton.TabIndex = 37;
            this.shapeRecoqnitionButton.Text = "Shape Recognition";
            this.shapeRecoqnitionButton.UseVisualStyleBackColor = true;
            // 
            // angleRecognitionButton
            // 
            this.angleRecognitionButton.Location = new System.Drawing.Point(62, 409);
            this.angleRecognitionButton.Margin = new System.Windows.Forms.Padding(2);
            this.angleRecognitionButton.Name = "angleRecognitionButton";
            this.angleRecognitionButton.Size = new System.Drawing.Size(274, 27);
            this.angleRecognitionButton.TabIndex = 36;
            this.angleRecognitionButton.Text = "Angle Recognition";
            this.angleRecognitionButton.UseVisualStyleBackColor = true;
            // 
            // blueColorLabel
            // 
            this.blueColorLabel.AutoSize = true;
            this.blueColorLabel.Location = new System.Drawing.Point(35, 162);
            this.blueColorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.blueColorLabel.Name = "blueColorLabel";
            this.blueColorLabel.Size = new System.Drawing.Size(86, 13);
            this.blueColorLabel.TabIndex = 35;
            this.blueColorLabel.Text = "blue color range:";
            // 
            // redColorRangeLabel
            // 
            this.redColorRangeLabel.AutoSize = true;
            this.redColorRangeLabel.Location = new System.Drawing.Point(35, 48);
            this.redColorRangeLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.redColorRangeLabel.Name = "redColorRangeLabel";
            this.redColorRangeLabel.Size = new System.Drawing.Size(81, 13);
            this.redColorRangeLabel.TabIndex = 1;
            this.redColorRangeLabel.Text = "red color range:";
            // 
            // greenColorLabel
            // 
            this.greenColorLabel.AutoSize = true;
            this.greenColorLabel.Location = new System.Drawing.Point(35, 105);
            this.greenColorLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.greenColorLabel.Name = "greenColorLabel";
            this.greenColorLabel.Size = new System.Drawing.Size(93, 13);
            this.greenColorLabel.TabIndex = 34;
            this.greenColorLabel.Text = "green color range:";
            // 
            // checkcolorDetectionButton
            // 
            this.checkcolorDetectionButton.Location = new System.Drawing.Point(62, 305);
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
            this.blueToLabel.Location = new System.Drawing.Point(194, 147);
            this.blueToLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.blueToLabel.Name = "blueToLabel";
            this.blueToLabel.Size = new System.Drawing.Size(43, 13);
            this.blueToLabel.TabIndex = 33;
            this.blueToLabel.Text = "to (130)";
            // 
            // redFromTextBox
            // 
            this.redFromTextBox.Location = new System.Drawing.Point(134, 48);
            this.redFromTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.redFromTextBox.Name = "redFromTextBox";
            this.redFromTextBox.Size = new System.Drawing.Size(56, 20);
            this.redFromTextBox.TabIndex = 18;
            this.redFromTextBox.TextChanged += new System.EventHandler(this.redFromTextBox_TextChanged);
            // 
            // blueFromLabel
            // 
            this.blueFromLabel.AutoSize = true;
            this.blueFromLabel.Location = new System.Drawing.Point(132, 147);
            this.blueFromLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.blueFromLabel.Name = "blueFromLabel";
            this.blueFromLabel.Size = new System.Drawing.Size(54, 13);
            this.blueFromLabel.TabIndex = 32;
            this.blueFromLabel.Text = "from (100)";
            // 
            // redFromTwoTextBox
            // 
            this.redFromTwoTextBox.Location = new System.Drawing.Point(255, 48);
            this.redFromTwoTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.redFromTwoTextBox.Name = "redFromTwoTextBox";
            this.redFromTwoTextBox.Size = new System.Drawing.Size(58, 20);
            this.redFromTwoTextBox.TabIndex = 22;
            this.redFromTwoTextBox.TextChanged += new System.EventHandler(this.redFromTwoTextBox_TextChanged);
            // 
            // blueToTextbox
            // 
            this.blueToTextbox.Location = new System.Drawing.Point(194, 162);
            this.blueToTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.blueToTextbox.Name = "blueToTextbox";
            this.blueToTextbox.Size = new System.Drawing.Size(57, 20);
            this.blueToTextbox.TabIndex = 31;
            this.blueToTextbox.TextChanged += new System.EventHandler(this.blueToTextbox_TextChanged);
            // 
            // redToTextBox
            // 
            this.redToTextBox.Location = new System.Drawing.Point(194, 48);
            this.redToTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.redToTextBox.Name = "redToTextBox";
            this.redToTextBox.Size = new System.Drawing.Size(57, 20);
            this.redToTextBox.TabIndex = 19;
            this.redToTextBox.TextChanged += new System.EventHandler(this.redToTextBox_TextChanged);
            // 
            // blueFromTextBox
            // 
            this.blueFromTextBox.Location = new System.Drawing.Point(134, 162);
            this.blueFromTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.blueFromTextBox.Name = "blueFromTextBox";
            this.blueFromTextBox.Size = new System.Drawing.Size(56, 20);
            this.blueFromTextBox.TabIndex = 30;
            this.blueFromTextBox.TextChanged += new System.EventHandler(this.blueFromTextBox_TextChanged);
            // 
            // redToTwoTextbox
            // 
            this.redToTwoTextbox.Location = new System.Drawing.Point(317, 48);
            this.redToTwoTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.redToTwoTextbox.Name = "redToTwoTextbox";
            this.redToTwoTextbox.Size = new System.Drawing.Size(58, 20);
            this.redToTwoTextbox.TabIndex = 23;
            this.redToTwoTextbox.TextChanged += new System.EventHandler(this.redToTwoTextbox_TextChanged);
            // 
            // greenToLabel
            // 
            this.greenToLabel.AutoSize = true;
            this.greenToLabel.Location = new System.Drawing.Point(194, 90);
            this.greenToLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.greenToLabel.Name = "greenToLabel";
            this.greenToLabel.Size = new System.Drawing.Size(37, 13);
            this.greenToLabel.TabIndex = 29;
            this.greenToLabel.Text = "to (85)";
            // 
            // redFromLabel
            // 
            this.redFromLabel.AutoSize = true;
            this.redFromLabel.Location = new System.Drawing.Point(131, 33);
            this.redFromLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.redFromLabel.Name = "redFromLabel";
            this.redFromLabel.Size = new System.Drawing.Size(42, 13);
            this.redFromLabel.TabIndex = 20;
            this.redFromLabel.Text = "from (0)";
            // 
            // greenFromLabel
            // 
            this.greenFromLabel.AutoSize = true;
            this.greenFromLabel.Location = new System.Drawing.Point(132, 90);
            this.greenFromLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.greenFromLabel.Name = "greenFromLabel";
            this.greenFromLabel.Size = new System.Drawing.Size(48, 13);
            this.greenFromLabel.TabIndex = 28;
            this.greenFromLabel.Text = "from (35)";
            // 
            // redFromTwoLabel
            // 
            this.redFromTwoLabel.AutoSize = true;
            this.redFromTwoLabel.Location = new System.Drawing.Point(252, 33);
            this.redFromTwoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.redFromTwoLabel.Name = "redFromTwoLabel";
            this.redFromTwoLabel.Size = new System.Drawing.Size(54, 13);
            this.redFromTwoLabel.TabIndex = 24;
            this.redFromTwoLabel.Text = "from (160)";
            // 
            // greenToTextbox
            // 
            this.greenToTextbox.Location = new System.Drawing.Point(194, 105);
            this.greenToTextbox.Margin = new System.Windows.Forms.Padding(2);
            this.greenToTextbox.Name = "greenToTextbox";
            this.greenToTextbox.Size = new System.Drawing.Size(57, 20);
            this.greenToTextbox.TabIndex = 27;
            this.greenToTextbox.TextChanged += new System.EventHandler(this.greenToTextbox_TextChanged);
            // 
            // redToLabel
            // 
            this.redToLabel.AutoSize = true;
            this.redToLabel.Location = new System.Drawing.Point(191, 33);
            this.redToLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.redToLabel.Name = "redToLabel";
            this.redToLabel.Size = new System.Drawing.Size(37, 13);
            this.redToLabel.TabIndex = 21;
            this.redToLabel.Text = "to (15)";
            // 
            // greenFromTextBox
            // 
            this.greenFromTextBox.Location = new System.Drawing.Point(134, 105);
            this.greenFromTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.greenFromTextBox.Name = "greenFromTextBox";
            this.greenFromTextBox.Size = new System.Drawing.Size(56, 20);
            this.greenFromTextBox.TabIndex = 26;
            this.greenFromTextBox.TextChanged += new System.EventHandler(this.greenFromTextBox_TextChanged);
            // 
            // redToTwoLabel
            // 
            this.redToTwoLabel.AutoSize = true;
            this.redToTwoLabel.Location = new System.Drawing.Point(317, 33);
            this.redToTwoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.redToTwoLabel.Name = "redToTwoLabel";
            this.redToTwoLabel.Size = new System.Drawing.Size(43, 13);
            this.redToTwoLabel.TabIndex = 25;
            this.redToTwoLabel.Text = "to (180)";
            // 
            // storeInfo
            // 
            this.storeInfo.Controls.Add(this.button15);
            this.storeInfo.Controls.Add(this.button14);
            this.storeInfo.Controls.Add(this.saveObjectsButton);
            this.storeInfo.Location = new System.Drawing.Point(4, 22);
            this.storeInfo.Name = "storeInfo";
            this.storeInfo.Size = new System.Drawing.Size(419, 522);
            this.storeInfo.TabIndex = 3;
            this.storeInfo.Text = "Store Info";
            this.storeInfo.UseVisualStyleBackColor = true;
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
            // 
            // CalibrationMethodsBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(calibrationtabControl);
            this.Name = "CalibrationMethodsBox";
            this.Size = new System.Drawing.Size(427, 548);
            calibrationtabControl.ResumeLayout(false);
            this.thresholdingPage.ResumeLayout(false);
            this.thresholdingPage.PerformLayout();
            this.objectApperancePage.ResumeLayout(false);
            this.objectApperancePage.PerformLayout();
            this.storeInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage thresholdingPage;
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
        private System.Windows.Forms.TabPage objectApperancePage;
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
        private System.Windows.Forms.Button measureRealDimensionsButton;
        private System.Windows.Forms.TabPage storeInfo;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button saveObjectsButton;
    }
}
