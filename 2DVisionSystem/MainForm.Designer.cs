namespace _2DVisionSystem
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.homePictureBox = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.homeTab = new System.Windows.Forms.TabPage();
            this.connectionGroupBox = new System.Windows.Forms.GroupBox();
            this.disconnectComPortButton = new System.Windows.Forms.Button();
            this.availableComPortsLabel = new System.Windows.Forms.Label();
            this.connectComPortButton = new System.Windows.Forms.Button();
            this.availableComPortsDropDown = new System.Windows.Forms.ComboBox();
            this.imageGroupBox = new System.Windows.Forms.GroupBox();
            this.availableCamerasLabel = new System.Windows.Forms.Label();
            this.connectCameraButton = new System.Windows.Forms.Button();
            this.changeCameraMatrixButton = new System.Windows.Forms.Button();
            this.disconnectCameraButton = new System.Windows.Forms.Button();
            this.EnterCameraMatrixGroupBox = new System.Windows.Forms.GroupBox();
            this.cameraMatrix33 = new System.Windows.Forms.TextBox();
            this.cameraMatrix32 = new System.Windows.Forms.TextBox();
            this.cameraMatrix31 = new System.Windows.Forms.TextBox();
            this.cameraMatrix23 = new System.Windows.Forms.TextBox();
            this.cameraMatrix22 = new System.Windows.Forms.TextBox();
            this.cameraMatrix21 = new System.Windows.Forms.TextBox();
            this.cameraMatrix13 = new System.Windows.Forms.TextBox();
            this.cameraMatrix12 = new System.Windows.Forms.TextBox();
            this.cameraMatrix11 = new System.Windows.Forms.TextBox();
            this.availableCamerasDropDown = new System.Windows.Forms.ComboBox();
            this.calibrationTab = new System.Windows.Forms.TabPage();
            this.manualConnectionTab = new System.Windows.Forms.TabPage();
            this.mainProgramTab = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.introductionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.robotManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreDefaultMatrixButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.homePictureBox)).BeginInit();
            this.tabControl.SuspendLayout();
            this.homeTab.SuspendLayout();
            this.connectionGroupBox.SuspendLayout();
            this.imageGroupBox.SuspendLayout();
            this.EnterCameraMatrixGroupBox.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // homePictureBox
            // 
            this.homePictureBox.Location = new System.Drawing.Point(27, 76);
            this.homePictureBox.Margin = new System.Windows.Forms.Padding(6);
            this.homePictureBox.Name = "homePictureBox";
            this.homePictureBox.Size = new System.Drawing.Size(722, 757);
            this.homePictureBox.TabIndex = 0;
            this.homePictureBox.TabStop = false;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(22, 22);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(223, 29);
            this.titleLabel.TabIndex = 2;
            this.titleLabel.Text = "2D Vision System ";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.homeTab);
            this.tabControl.Controls.Add(this.calibrationTab);
            this.tabControl.Controls.Add(this.manualConnectionTab);
            this.tabControl.Controls.Add(this.mainProgramTab);
            this.tabControl.Location = new System.Drawing.Point(0, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1687, 911);
            this.tabControl.TabIndex = 4;
            // 
            // homeTab
            // 
            this.homeTab.Controls.Add(this.connectionGroupBox);
            this.homeTab.Controls.Add(this.imageGroupBox);
            this.homeTab.Controls.Add(this.titleLabel);
            this.homeTab.Controls.Add(this.homePictureBox);
            this.homeTab.Location = new System.Drawing.Point(4, 34);
            this.homeTab.Name = "homeTab";
            this.homeTab.Padding = new System.Windows.Forms.Padding(3);
            this.homeTab.Size = new System.Drawing.Size(1679, 873);
            this.homeTab.TabIndex = 0;
            this.homeTab.Text = "Home";
            this.homeTab.UseVisualStyleBackColor = true;
            // 
            // connectionGroupBox
            // 
            this.connectionGroupBox.Controls.Add(this.disconnectComPortButton);
            this.connectionGroupBox.Controls.Add(this.availableComPortsLabel);
            this.connectionGroupBox.Controls.Add(this.connectComPortButton);
            this.connectionGroupBox.Controls.Add(this.availableComPortsDropDown);
            this.connectionGroupBox.Location = new System.Drawing.Point(786, 491);
            this.connectionGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.connectionGroupBox.Name = "connectionGroupBox";
            this.connectionGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.connectionGroupBox.Size = new System.Drawing.Size(853, 342);
            this.connectionGroupBox.TabIndex = 6;
            this.connectionGroupBox.TabStop = false;
            this.connectionGroupBox.Text = "Connection with robot via Com Port";
            // 
            // disconnectComPortButton
            // 
            this.disconnectComPortButton.Location = new System.Drawing.Point(247, 230);
            this.disconnectComPortButton.Margin = new System.Windows.Forms.Padding(2);
            this.disconnectComPortButton.Name = "disconnectComPortButton";
            this.disconnectComPortButton.Size = new System.Drawing.Size(314, 45);
            this.disconnectComPortButton.TabIndex = 36;
            this.disconnectComPortButton.Text = "Disconnect";
            this.disconnectComPortButton.UseVisualStyleBackColor = true;
            // 
            // availableComPortsLabel
            // 
            this.availableComPortsLabel.AutoSize = true;
            this.availableComPortsLabel.Location = new System.Drawing.Point(171, 67);
            this.availableComPortsLabel.Name = "availableComPortsLabel";
            this.availableComPortsLabel.Size = new System.Drawing.Size(206, 25);
            this.availableComPortsLabel.TabIndex = 35;
            this.availableComPortsLabel.Text = "Available Com Ports";
            // 
            // connectComPortButton
            // 
            this.connectComPortButton.Location = new System.Drawing.Point(247, 160);
            this.connectComPortButton.Margin = new System.Windows.Forms.Padding(2);
            this.connectComPortButton.Name = "connectComPortButton";
            this.connectComPortButton.Size = new System.Drawing.Size(314, 45);
            this.connectComPortButton.TabIndex = 7;
            this.connectComPortButton.Text = "Connect";
            this.connectComPortButton.UseVisualStyleBackColor = true;
            // 
            // availableComPortsDropDown
            // 
            this.availableComPortsDropDown.FormattingEnabled = true;
            this.availableComPortsDropDown.Location = new System.Drawing.Point(173, 94);
            this.availableComPortsDropDown.Margin = new System.Windows.Forms.Padding(2);
            this.availableComPortsDropDown.Name = "availableComPortsDropDown";
            this.availableComPortsDropDown.Size = new System.Drawing.Size(451, 33);
            this.availableComPortsDropDown.TabIndex = 0;
            // 
            // imageGroupBox
            // 
            this.imageGroupBox.Controls.Add(this.restoreDefaultMatrixButton);
            this.imageGroupBox.Controls.Add(this.availableCamerasLabel);
            this.imageGroupBox.Controls.Add(this.connectCameraButton);
            this.imageGroupBox.Controls.Add(this.changeCameraMatrixButton);
            this.imageGroupBox.Controls.Add(this.disconnectCameraButton);
            this.imageGroupBox.Controls.Add(this.EnterCameraMatrixGroupBox);
            this.imageGroupBox.Controls.Add(this.availableCamerasDropDown);
            this.imageGroupBox.Location = new System.Drawing.Point(786, 76);
            this.imageGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.imageGroupBox.Name = "imageGroupBox";
            this.imageGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.imageGroupBox.Size = new System.Drawing.Size(853, 411);
            this.imageGroupBox.TabIndex = 5;
            this.imageGroupBox.TabStop = false;
            this.imageGroupBox.Text = "Image aquisition - camera";
            // 
            // availableCamerasLabel
            // 
            this.availableCamerasLabel.AutoSize = true;
            this.availableCamerasLabel.Location = new System.Drawing.Point(39, 97);
            this.availableCamerasLabel.Name = "availableCamerasLabel";
            this.availableCamerasLabel.Size = new System.Drawing.Size(192, 25);
            this.availableCamerasLabel.TabIndex = 34;
            this.availableCamerasLabel.Text = "Available Cameras";
            // 
            // connectCameraButton
            // 
            this.connectCameraButton.Location = new System.Drawing.Point(44, 183);
            this.connectCameraButton.Name = "connectCameraButton";
            this.connectCameraButton.Size = new System.Drawing.Size(319, 46);
            this.connectCameraButton.TabIndex = 33;
            this.connectCameraButton.Text = "Connect Camera";
            this.connectCameraButton.UseVisualStyleBackColor = true;
            this.connectCameraButton.Click += new System.EventHandler(this.connectCameraButton_Click);
            // 
            // changeCameraMatrixButton
            // 
            this.changeCameraMatrixButton.Location = new System.Drawing.Point(479, 253);
            this.changeCameraMatrixButton.Name = "changeCameraMatrixButton";
            this.changeCameraMatrixButton.Size = new System.Drawing.Size(312, 46);
            this.changeCameraMatrixButton.TabIndex = 32;
            this.changeCameraMatrixButton.Text = "Change Camera Matrix";
            this.changeCameraMatrixButton.UseVisualStyleBackColor = true;
            this.changeCameraMatrixButton.Click += new System.EventHandler(this.changeCameraMatrixButton_Click);
            // 
            // disconnectCameraButton
            // 
            this.disconnectCameraButton.Location = new System.Drawing.Point(44, 235);
            this.disconnectCameraButton.Name = "disconnectCameraButton";
            this.disconnectCameraButton.Size = new System.Drawing.Size(319, 46);
            this.disconnectCameraButton.TabIndex = 31;
            this.disconnectCameraButton.Text = "Disconnect Camera";
            this.disconnectCameraButton.UseVisualStyleBackColor = true;
            this.disconnectCameraButton.Click += new System.EventHandler(this.disconnectCameraButton_Click);
            // 
            // EnterCameraMatrixGroupBox
            // 
            this.EnterCameraMatrixGroupBox.Controls.Add(this.cameraMatrix33);
            this.EnterCameraMatrixGroupBox.Controls.Add(this.cameraMatrix32);
            this.EnterCameraMatrixGroupBox.Controls.Add(this.cameraMatrix31);
            this.EnterCameraMatrixGroupBox.Controls.Add(this.cameraMatrix23);
            this.EnterCameraMatrixGroupBox.Controls.Add(this.cameraMatrix22);
            this.EnterCameraMatrixGroupBox.Controls.Add(this.cameraMatrix21);
            this.EnterCameraMatrixGroupBox.Controls.Add(this.cameraMatrix13);
            this.EnterCameraMatrixGroupBox.Controls.Add(this.cameraMatrix12);
            this.EnterCameraMatrixGroupBox.Controls.Add(this.cameraMatrix11);
            this.EnterCameraMatrixGroupBox.Location = new System.Drawing.Point(451, 60);
            this.EnterCameraMatrixGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.EnterCameraMatrixGroupBox.Name = "EnterCameraMatrixGroupBox";
            this.EnterCameraMatrixGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.EnterCameraMatrixGroupBox.Size = new System.Drawing.Size(371, 175);
            this.EnterCameraMatrixGroupBox.TabIndex = 30;
            this.EnterCameraMatrixGroupBox.TabStop = false;
            this.EnterCameraMatrixGroupBox.Text = "Enter Camera Matrix";
            // 
            // cameraMatrix33
            // 
            this.cameraMatrix33.Location = new System.Drawing.Point(240, 120);
            this.cameraMatrix33.Name = "cameraMatrix33";
            this.cameraMatrix33.Size = new System.Drawing.Size(100, 31);
            this.cameraMatrix33.TabIndex = 8;
            this.cameraMatrix33.Text = "0";
            // 
            // cameraMatrix32
            // 
            this.cameraMatrix32.Location = new System.Drawing.Point(134, 120);
            this.cameraMatrix32.Name = "cameraMatrix32";
            this.cameraMatrix32.Size = new System.Drawing.Size(100, 31);
            this.cameraMatrix32.TabIndex = 7;
            this.cameraMatrix32.Text = "0";
            // 
            // cameraMatrix31
            // 
            this.cameraMatrix31.Location = new System.Drawing.Point(28, 120);
            this.cameraMatrix31.Name = "cameraMatrix31";
            this.cameraMatrix31.Size = new System.Drawing.Size(100, 31);
            this.cameraMatrix31.TabIndex = 6;
            this.cameraMatrix31.Text = "0";
            // 
            // cameraMatrix23
            // 
            this.cameraMatrix23.Location = new System.Drawing.Point(240, 83);
            this.cameraMatrix23.Name = "cameraMatrix23";
            this.cameraMatrix23.Size = new System.Drawing.Size(100, 31);
            this.cameraMatrix23.TabIndex = 5;
            this.cameraMatrix23.Text = "0";
            // 
            // cameraMatrix22
            // 
            this.cameraMatrix22.Location = new System.Drawing.Point(134, 83);
            this.cameraMatrix22.Name = "cameraMatrix22";
            this.cameraMatrix22.Size = new System.Drawing.Size(100, 31);
            this.cameraMatrix22.TabIndex = 4;
            this.cameraMatrix22.Text = "0";
            // 
            // cameraMatrix21
            // 
            this.cameraMatrix21.Location = new System.Drawing.Point(28, 83);
            this.cameraMatrix21.Name = "cameraMatrix21";
            this.cameraMatrix21.Size = new System.Drawing.Size(100, 31);
            this.cameraMatrix21.TabIndex = 3;
            this.cameraMatrix21.Text = "0";
            // 
            // cameraMatrix13
            // 
            this.cameraMatrix13.Location = new System.Drawing.Point(240, 46);
            this.cameraMatrix13.Name = "cameraMatrix13";
            this.cameraMatrix13.Size = new System.Drawing.Size(100, 31);
            this.cameraMatrix13.TabIndex = 2;
            this.cameraMatrix13.Text = "0";
            // 
            // cameraMatrix12
            // 
            this.cameraMatrix12.Location = new System.Drawing.Point(134, 46);
            this.cameraMatrix12.Name = "cameraMatrix12";
            this.cameraMatrix12.Size = new System.Drawing.Size(100, 31);
            this.cameraMatrix12.TabIndex = 1;
            this.cameraMatrix12.Text = "0";
            // 
            // cameraMatrix11
            // 
            this.cameraMatrix11.Location = new System.Drawing.Point(28, 46);
            this.cameraMatrix11.Name = "cameraMatrix11";
            this.cameraMatrix11.Size = new System.Drawing.Size(100, 31);
            this.cameraMatrix11.TabIndex = 0;
            this.cameraMatrix11.Text = "0";
            // 
            // availableCamerasDropDown
            // 
            this.availableCamerasDropDown.FormattingEnabled = true;
            this.availableCamerasDropDown.Location = new System.Drawing.Point(44, 124);
            this.availableCamerasDropDown.Margin = new System.Windows.Forms.Padding(2);
            this.availableCamerasDropDown.Name = "availableCamerasDropDown";
            this.availableCamerasDropDown.Size = new System.Drawing.Size(319, 33);
            this.availableCamerasDropDown.TabIndex = 0;
            // 
            // calibrationTab
            // 
            this.calibrationTab.Location = new System.Drawing.Point(4, 34);
            this.calibrationTab.Name = "calibrationTab";
            this.calibrationTab.Padding = new System.Windows.Forms.Padding(3);
            this.calibrationTab.Size = new System.Drawing.Size(1679, 873);
            this.calibrationTab.TabIndex = 1;
            this.calibrationTab.Text = "Calibration";
            this.calibrationTab.UseVisualStyleBackColor = true;
            // 
            // manualConnectionTab
            // 
            this.manualConnectionTab.Location = new System.Drawing.Point(4, 34);
            this.manualConnectionTab.Name = "manualConnectionTab";
            this.manualConnectionTab.Size = new System.Drawing.Size(1679, 873);
            this.manualConnectionTab.TabIndex = 2;
            this.manualConnectionTab.Text = "Manual Connection";
            this.manualConnectionTab.UseVisualStyleBackColor = true;
            // 
            // mainProgramTab
            // 
            this.mainProgramTab.Location = new System.Drawing.Point(4, 34);
            this.mainProgramTab.Name = "mainProgramTab";
            this.mainProgramTab.Size = new System.Drawing.Size(1679, 873);
            this.mainProgramTab.TabIndex = 3;
            this.mainProgramTab.Text = "Main Program";
            this.mainProgramTab.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1687, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem,
            this.documentationToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.introductionToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // introductionToolStripMenuItem
            // 
            this.introductionToolStripMenuItem.Name = "introductionToolStripMenuItem";
            this.introductionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.introductionToolStripMenuItem.Text = "Introduction";
            this.introductionToolStripMenuItem.Click += new System.EventHandler(this.introductionToolStripMenuItem_Click);
            // 
            // documentationToolStripMenuItem
            // 
            this.documentationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.robotManualToolStripMenuItem});
            this.documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
            this.documentationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.documentationToolStripMenuItem.Text = "Documentation";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 941);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1687, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(95, 17);
            this.statusLabel.Text = "Operation Status";
            // 
            // statusProgressBar
            // 
            this.statusProgressBar.Name = "statusProgressBar";
            this.statusProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // robotManualToolStripMenuItem
            // 
            this.robotManualToolStripMenuItem.Name = "robotManualToolStripMenuItem";
            this.robotManualToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.robotManualToolStripMenuItem.Text = "RV-E3J Robot Manual";
            // 
            // restoreDefaultMatrixButton
            // 
            this.restoreDefaultMatrixButton.Location = new System.Drawing.Point(479, 305);
            this.restoreDefaultMatrixButton.Name = "restoreDefaultMatrixButton";
            this.restoreDefaultMatrixButton.Size = new System.Drawing.Size(312, 46);
            this.restoreDefaultMatrixButton.TabIndex = 35;
            this.restoreDefaultMatrixButton.Text = "Restore Default Matrix";
            this.restoreDefaultMatrixButton.UseVisualStyleBackColor = true;
            this.restoreDefaultMatrixButton.Click += new System.EventHandler(this.restoreDefaultMatrixButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(1687, 963);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "MainForm";
            this.Text = "2D Vision System";
            ((System.ComponentModel.ISupportInitialize)(this.homePictureBox)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.homeTab.ResumeLayout(false);
            this.homeTab.PerformLayout();
            this.connectionGroupBox.ResumeLayout(false);
            this.connectionGroupBox.PerformLayout();
            this.imageGroupBox.ResumeLayout(false);
            this.imageGroupBox.PerformLayout();
            this.EnterCameraMatrixGroupBox.ResumeLayout(false);
            this.EnterCameraMatrixGroupBox.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox homePictureBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage homeTab;
        private System.Windows.Forms.TabPage calibrationTab;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem introductionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar statusProgressBar;
        private System.Windows.Forms.TabPage manualConnectionTab;
        private System.Windows.Forms.TabPage mainProgramTab;
        private System.Windows.Forms.GroupBox imageGroupBox;
        private System.Windows.Forms.Button disconnectCameraButton;
        private System.Windows.Forms.GroupBox EnterCameraMatrixGroupBox;
        private System.Windows.Forms.ComboBox availableCamerasDropDown;
        private System.Windows.Forms.GroupBox connectionGroupBox;
        private System.Windows.Forms.Button disconnectComPortButton;
        private System.Windows.Forms.Label availableComPortsLabel;
        private System.Windows.Forms.Button connectComPortButton;
        private System.Windows.Forms.ComboBox availableComPortsDropDown;
        private System.Windows.Forms.Label availableCamerasLabel;
        private System.Windows.Forms.Button connectCameraButton;
        private System.Windows.Forms.Button changeCameraMatrixButton;
        private System.Windows.Forms.TextBox cameraMatrix33;
        private System.Windows.Forms.TextBox cameraMatrix32;
        private System.Windows.Forms.TextBox cameraMatrix31;
        private System.Windows.Forms.TextBox cameraMatrix23;
        private System.Windows.Forms.TextBox cameraMatrix22;
        private System.Windows.Forms.TextBox cameraMatrix21;
        private System.Windows.Forms.TextBox cameraMatrix13;
        private System.Windows.Forms.TextBox cameraMatrix12;
        private System.Windows.Forms.TextBox cameraMatrix11;
        private System.Windows.Forms.ToolStripMenuItem robotManualToolStripMenuItem;
        private System.Windows.Forms.Button restoreDefaultMatrixButton;
    }
}

