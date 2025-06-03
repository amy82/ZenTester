
namespace ZenTester.VisionClass
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
            this.button_Mask_Save = new System.Windows.Forms.Button();
            this.button_Mask_Close = new System.Windows.Forms.Button();
            this.label_Mask_Edge_Smooth_Val = new System.Windows.Forms.Label();
            this.label_Mask_Edge_Smooth = new System.Windows.Forms.Label();
            this.label_Mask_Clear = new System.Windows.Forms.Button();
            this.label_Mask_Erase = new System.Windows.Forms.Button();
            this.label_Mask_Bg = new System.Windows.Forms.Button();
            this.label_Mask_Brush_Size = new System.Windows.Forms.Label();
            this.trackBar_Mask_Brush_Size = new System.Windows.Forms.TrackBar();
            this.label_Mask_Brush_Size_Val = new System.Windows.Forms.Label();
            this.button_Mask_Center_Move_Left = new System.Windows.Forms.Button();
            this.button_Mask_Center_Move_Right = new System.Windows.Forms.Button();
            this.button_Mask_Center_Move_Up = new System.Windows.Forms.Button();
            this.button_Mask_Center_Move_Down = new System.Windows.Forms.Button();
            this.button_Mask_Center_Move_Value = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Mask_Brush_Size)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_MarkZoomImage
            // 
            this.panel_MarkZoomImage.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel_MarkZoomImage.Location = new System.Drawing.Point(225, 2);
            this.panel_MarkZoomImage.Name = "panel_MarkZoomImage";
            this.panel_MarkZoomImage.Size = new System.Drawing.Size(593, 532);
            this.panel_MarkZoomImage.TabIndex = 0;
            this.panel_MarkZoomImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_MarkZoomImage_MouseDown);
            this.panel_MarkZoomImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_MarkZoomImage_MouseMove);
            this.panel_MarkZoomImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel_MarkZoomImage_MouseUp);
            // 
            // button_Mask_Save
            // 
            this.button_Mask_Save.BackColor = System.Drawing.Color.YellowGreen;
            this.button_Mask_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Mask_Save.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Mask_Save.Location = new System.Drawing.Point(590, 540);
            this.button_Mask_Save.Name = "button_Mask_Save";
            this.button_Mask_Save.Size = new System.Drawing.Size(111, 43);
            this.button_Mask_Save.TabIndex = 81;
            this.button_Mask_Save.Text = "SAVE";
            this.button_Mask_Save.UseVisualStyleBackColor = false;
            this.button_Mask_Save.Click += new System.EventHandler(this.button_Mask_Save_Click);
            // 
            // button_Mask_Close
            // 
            this.button_Mask_Close.BackColor = System.Drawing.Color.YellowGreen;
            this.button_Mask_Close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Mask_Close.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Mask_Close.Location = new System.Drawing.Point(707, 540);
            this.button_Mask_Close.Name = "button_Mask_Close";
            this.button_Mask_Close.Size = new System.Drawing.Size(111, 43);
            this.button_Mask_Close.TabIndex = 82;
            this.button_Mask_Close.Text = "CLOSE";
            this.button_Mask_Close.UseVisualStyleBackColor = false;
            this.button_Mask_Close.Click += new System.EventHandler(this.button_Mask_Close_Click);
            // 
            // label_Mask_Edge_Smooth_Val
            // 
            this.label_Mask_Edge_Smooth_Val.BackColor = System.Drawing.SystemColors.Window;
            this.label_Mask_Edge_Smooth_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Mask_Edge_Smooth_Val.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_Mask_Edge_Smooth_Val.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Mask_Edge_Smooth_Val.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Mask_Edge_Smooth_Val.ForeColor = System.Drawing.Color.Black;
            this.label_Mask_Edge_Smooth_Val.Location = new System.Drawing.Point(157, 192);
            this.label_Mask_Edge_Smooth_Val.Name = "label_Mask_Edge_Smooth_Val";
            this.label_Mask_Edge_Smooth_Val.Size = new System.Drawing.Size(65, 43);
            this.label_Mask_Edge_Smooth_Val.TabIndex = 84;
            this.label_Mask_Edge_Smooth_Val.Text = "0";
            this.label_Mask_Edge_Smooth_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Mask_Edge_Smooth_Val.Click += new System.EventHandler(this.label_Mask_Edge_Smooth_Val_Click);
            // 
            // label_Mask_Edge_Smooth
            // 
            this.label_Mask_Edge_Smooth.BackColor = System.Drawing.Color.Gray;
            this.label_Mask_Edge_Smooth.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Mask_Edge_Smooth.ForeColor = System.Drawing.Color.Black;
            this.label_Mask_Edge_Smooth.Location = new System.Drawing.Point(12, 192);
            this.label_Mask_Edge_Smooth.Name = "label_Mask_Edge_Smooth";
            this.label_Mask_Edge_Smooth.Size = new System.Drawing.Size(142, 43);
            this.label_Mask_Edge_Smooth.TabIndex = 83;
            this.label_Mask_Edge_Smooth.Text = "EDGE SMOOTH";
            this.label_Mask_Edge_Smooth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Mask_Clear
            // 
            this.label_Mask_Clear.BackColor = System.Drawing.Color.Tan;
            this.label_Mask_Clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Mask_Clear.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Mask_Clear.Location = new System.Drawing.Point(15, 428);
            this.label_Mask_Clear.Name = "label_Mask_Clear";
            this.label_Mask_Clear.Size = new System.Drawing.Size(99, 51);
            this.label_Mask_Clear.TabIndex = 85;
            this.label_Mask_Clear.Text = "MASK CLEAR";
            this.label_Mask_Clear.UseVisualStyleBackColor = false;
            this.label_Mask_Clear.Click += new System.EventHandler(this.label_Mask_Clear_Click);
            // 
            // label_Mask_Erase
            // 
            this.label_Mask_Erase.BackColor = System.Drawing.Color.Tan;
            this.label_Mask_Erase.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Mask_Erase.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Mask_Erase.Location = new System.Drawing.Point(120, 428);
            this.label_Mask_Erase.Name = "label_Mask_Erase";
            this.label_Mask_Erase.Size = new System.Drawing.Size(99, 51);
            this.label_Mask_Erase.TabIndex = 86;
            this.label_Mask_Erase.Text = "MASK ERASE";
            this.label_Mask_Erase.UseVisualStyleBackColor = false;
            this.label_Mask_Erase.Click += new System.EventHandler(this.label_Mask_Erase_Click);
            // 
            // label_Mask_Bg
            // 
            this.label_Mask_Bg.BackColor = System.Drawing.Color.Tan;
            this.label_Mask_Bg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Mask_Bg.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Mask_Bg.Location = new System.Drawing.Point(120, 485);
            this.label_Mask_Bg.Name = "label_Mask_Bg";
            this.label_Mask_Bg.Size = new System.Drawing.Size(99, 51);
            this.label_Mask_Bg.TabIndex = 87;
            this.label_Mask_Bg.Text = "BG MASK";
            this.label_Mask_Bg.UseVisualStyleBackColor = false;
            this.label_Mask_Bg.Click += new System.EventHandler(this.label_Mask_Bg_Click);
            // 
            // label_Mask_Brush_Size
            // 
            this.label_Mask_Brush_Size.BackColor = System.Drawing.Color.Gray;
            this.label_Mask_Brush_Size.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Mask_Brush_Size.ForeColor = System.Drawing.Color.Black;
            this.label_Mask_Brush_Size.Location = new System.Drawing.Point(12, 256);
            this.label_Mask_Brush_Size.Name = "label_Mask_Brush_Size";
            this.label_Mask_Brush_Size.Size = new System.Drawing.Size(207, 43);
            this.label_Mask_Brush_Size.TabIndex = 88;
            this.label_Mask_Brush_Size.Text = "MASK BRUSH SIZE";
            this.label_Mask_Brush_Size.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBar_Mask_Brush_Size
            // 
            this.trackBar_Mask_Brush_Size.Location = new System.Drawing.Point(12, 302);
            this.trackBar_Mask_Brush_Size.Maximum = 100;
            this.trackBar_Mask_Brush_Size.Name = "trackBar_Mask_Brush_Size";
            this.trackBar_Mask_Brush_Size.Size = new System.Drawing.Size(142, 45);
            this.trackBar_Mask_Brush_Size.TabIndex = 89;
            this.trackBar_Mask_Brush_Size.Scroll += new System.EventHandler(this.trackBar_Mask_Brush_Size_Scroll);
            // 
            // label_Mask_Brush_Size_Val
            // 
            this.label_Mask_Brush_Size_Val.BackColor = System.Drawing.SystemColors.Window;
            this.label_Mask_Brush_Size_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Mask_Brush_Size_Val.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label_Mask_Brush_Size_Val.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Mask_Brush_Size_Val.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Mask_Brush_Size_Val.ForeColor = System.Drawing.Color.Black;
            this.label_Mask_Brush_Size_Val.Location = new System.Drawing.Point(154, 302);
            this.label_Mask_Brush_Size_Val.Name = "label_Mask_Brush_Size_Val";
            this.label_Mask_Brush_Size_Val.Size = new System.Drawing.Size(65, 43);
            this.label_Mask_Brush_Size_Val.TabIndex = 90;
            this.label_Mask_Brush_Size_Val.Text = "0";
            this.label_Mask_Brush_Size_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Mask_Center_Move_Left
            // 
            this.button_Mask_Center_Move_Left.BackColor = System.Drawing.Color.Tan;
            this.button_Mask_Center_Move_Left.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Mask_Center_Move_Left.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Mask_Center_Move_Left.ForeColor = System.Drawing.Color.White;
            this.button_Mask_Center_Move_Left.Location = new System.Drawing.Point(8, 66);
            this.button_Mask_Center_Move_Left.Name = "button_Mask_Center_Move_Left";
            this.button_Mask_Center_Move_Left.Size = new System.Drawing.Size(65, 50);
            this.button_Mask_Center_Move_Left.TabIndex = 91;
            this.button_Mask_Center_Move_Left.Text = "◀";
            this.button_Mask_Center_Move_Left.UseVisualStyleBackColor = false;
            this.button_Mask_Center_Move_Left.Click += new System.EventHandler(this.button_Mask_Center_Move_Left_Click);
            // 
            // button_Mask_Center_Move_Right
            // 
            this.button_Mask_Center_Move_Right.BackColor = System.Drawing.Color.Tan;
            this.button_Mask_Center_Move_Right.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Mask_Center_Move_Right.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Mask_Center_Move_Right.ForeColor = System.Drawing.Color.White;
            this.button_Mask_Center_Move_Right.Location = new System.Drawing.Point(139, 66);
            this.button_Mask_Center_Move_Right.Name = "button_Mask_Center_Move_Right";
            this.button_Mask_Center_Move_Right.Size = new System.Drawing.Size(65, 50);
            this.button_Mask_Center_Move_Right.TabIndex = 92;
            this.button_Mask_Center_Move_Right.Text = "▶";
            this.button_Mask_Center_Move_Right.UseVisualStyleBackColor = false;
            this.button_Mask_Center_Move_Right.Click += new System.EventHandler(this.button_Mask_Center_Move_Right_Click);
            // 
            // button_Mask_Center_Move_Up
            // 
            this.button_Mask_Center_Move_Up.BackColor = System.Drawing.Color.Tan;
            this.button_Mask_Center_Move_Up.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Mask_Center_Move_Up.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Mask_Center_Move_Up.ForeColor = System.Drawing.Color.White;
            this.button_Mask_Center_Move_Up.Location = new System.Drawing.Point(72, 19);
            this.button_Mask_Center_Move_Up.Name = "button_Mask_Center_Move_Up";
            this.button_Mask_Center_Move_Up.Size = new System.Drawing.Size(67, 48);
            this.button_Mask_Center_Move_Up.TabIndex = 93;
            this.button_Mask_Center_Move_Up.Text = "▲";
            this.button_Mask_Center_Move_Up.UseVisualStyleBackColor = false;
            this.button_Mask_Center_Move_Up.Click += new System.EventHandler(this.button_Mask_Center_Move_Up_Click);
            // 
            // button_Mask_Center_Move_Down
            // 
            this.button_Mask_Center_Move_Down.BackColor = System.Drawing.Color.Tan;
            this.button_Mask_Center_Move_Down.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Mask_Center_Move_Down.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Mask_Center_Move_Down.ForeColor = System.Drawing.Color.White;
            this.button_Mask_Center_Move_Down.Location = new System.Drawing.Point(72, 117);
            this.button_Mask_Center_Move_Down.Name = "button_Mask_Center_Move_Down";
            this.button_Mask_Center_Move_Down.Size = new System.Drawing.Size(67, 48);
            this.button_Mask_Center_Move_Down.TabIndex = 94;
            this.button_Mask_Center_Move_Down.Text = "▼";
            this.button_Mask_Center_Move_Down.UseVisualStyleBackColor = false;
            this.button_Mask_Center_Move_Down.Click += new System.EventHandler(this.button_Mask_Center_Move_Down_Click);
            // 
            // button_Mask_Center_Move_Value
            // 
            this.button_Mask_Center_Move_Value.BackColor = System.Drawing.SystemColors.Window;
            this.button_Mask_Center_Move_Value.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.button_Mask_Center_Move_Value.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button_Mask_Center_Move_Value.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Mask_Center_Move_Value.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Mask_Center_Move_Value.ForeColor = System.Drawing.Color.Black;
            this.button_Mask_Center_Move_Value.Location = new System.Drawing.Point(73, 67);
            this.button_Mask_Center_Move_Value.Name = "button_Mask_Center_Move_Value";
            this.button_Mask_Center_Move_Value.Size = new System.Drawing.Size(66, 50);
            this.button_Mask_Center_Move_Value.TabIndex = 95;
            this.button_Mask_Center_Move_Value.Text = "10";
            this.button_Mask_Center_Move_Value.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.button_Mask_Center_Move_Value.Click += new System.EventHandler(this.button_Mask_Center_Move_Value_Click);
            // 
            // MarkViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 594);
            this.Controls.Add(this.button_Mask_Center_Move_Value);
            this.Controls.Add(this.button_Mask_Center_Move_Down);
            this.Controls.Add(this.button_Mask_Center_Move_Up);
            this.Controls.Add(this.button_Mask_Center_Move_Right);
            this.Controls.Add(this.button_Mask_Center_Move_Left);
            this.Controls.Add(this.label_Mask_Brush_Size_Val);
            this.Controls.Add(this.trackBar_Mask_Brush_Size);
            this.Controls.Add(this.label_Mask_Brush_Size);
            this.Controls.Add(this.label_Mask_Bg);
            this.Controls.Add(this.label_Mask_Erase);
            this.Controls.Add(this.label_Mask_Clear);
            this.Controls.Add(this.label_Mask_Edge_Smooth_Val);
            this.Controls.Add(this.label_Mask_Edge_Smooth);
            this.Controls.Add(this.button_Mask_Close);
            this.Controls.Add(this.button_Mask_Save);
            this.Controls.Add(this.panel_MarkZoomImage);
            this.Name = "MarkViewerForm";
            this.Text = "MarkViewerForm";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Mask_Brush_Size)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_Mask_Save;
        private System.Windows.Forms.Button button_Mask_Close;
        public System.Windows.Forms.Label label_Mask_Edge_Smooth_Val;
        private System.Windows.Forms.Label label_Mask_Edge_Smooth;
        private System.Windows.Forms.Button label_Mask_Clear;
        private System.Windows.Forms.Button label_Mask_Erase;
        private System.Windows.Forms.Button label_Mask_Bg;
        private System.Windows.Forms.Label label_Mask_Brush_Size;
        private System.Windows.Forms.TrackBar trackBar_Mask_Brush_Size;
        public System.Windows.Forms.Label label_Mask_Brush_Size_Val;
        public System.Windows.Forms.Panel panel_MarkZoomImage;
        private System.Windows.Forms.Button button_Mask_Center_Move_Left;
        private System.Windows.Forms.Button button_Mask_Center_Move_Right;
        private System.Windows.Forms.Button button_Mask_Center_Move_Up;
        private System.Windows.Forms.Button button_Mask_Center_Move_Down;
        public System.Windows.Forms.Label button_Mask_Center_Move_Value;
    }
}