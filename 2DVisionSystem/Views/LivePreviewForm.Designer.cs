namespace VisionSystem
{
    partial class LivePreviewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LivePreviewForm));
            this.LivePreviewPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LivePreviewPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LivePreviewPictureBox
            // 
            this.LivePreviewPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LivePreviewPictureBox.Location = new System.Drawing.Point(0, 0);
            this.LivePreviewPictureBox.Name = "LivePreviewPictureBox";
            this.LivePreviewPictureBox.Size = new System.Drawing.Size(813, 696);
            this.LivePreviewPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LivePreviewPictureBox.TabIndex = 0;
            this.LivePreviewPictureBox.TabStop = false;
            // 
            // LivePreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 696);
            this.Controls.Add(this.LivePreviewPictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LivePreviewForm";
            this.Text = "LivePreviewForm";
            ((System.ComponentModel.ISupportInitialize)(this.LivePreviewPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox LivePreviewPictureBox;
    }
}