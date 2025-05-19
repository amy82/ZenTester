
namespace ZenHandler.VisionClass
{
    partial class MarkViewerForm
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
            this.panel_MarkZoomImage = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel_MarkZoomImage
            // 
            this.panel_MarkZoomImage.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel_MarkZoomImage.Location = new System.Drawing.Point(12, 12);
            this.panel_MarkZoomImage.Name = "panel_MarkZoomImage";
            this.panel_MarkZoomImage.Size = new System.Drawing.Size(574, 426);
            this.panel_MarkZoomImage.TabIndex = 0;
            // 
            // MarkViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel_MarkZoomImage);
            this.Name = "MarkViewerForm";
            this.Text = "MarkViewerForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_MarkZoomImage;
    }
}