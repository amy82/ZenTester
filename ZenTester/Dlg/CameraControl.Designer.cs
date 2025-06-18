
namespace ZenTester.Dlg
{
    partial class CameraControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelCam1 = new System.Windows.Forms.Panel();
            this.label_Socket_Result1 = new System.Windows.Forms.Label();
            this.label_Socket_Result2 = new System.Windows.Forms.Label();
            this.btn_TopCam_Image_Load = new System.Windows.Forms.Button();
            this.btn_TopCam_Image_Save = new System.Windows.Forms.Button();
            this.btn_SideCam_Image_Save = new System.Windows.Forms.Button();
            this.btn_SideCam_Image_Load = new System.Windows.Forms.Button();
            this.panelCam2 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(673, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "TOP CAM";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(684, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(673, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "SIDE CAM";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelCam1
            // 
            this.panelCam1.Location = new System.Drawing.Point(3, 34);
            this.panelCam1.Name = "panelCam1";
            this.panelCam1.Size = new System.Drawing.Size(673, 482);
            this.panelCam1.TabIndex = 4;
            // 
            // label_Socket_Result1
            // 
            this.label_Socket_Result1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_Socket_Result1.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Socket_Result1.ForeColor = System.Drawing.Color.White;
            this.label_Socket_Result1.Location = new System.Drawing.Point(1469, 10);
            this.label_Socket_Result1.Name = "label_Socket_Result1";
            this.label_Socket_Result1.Size = new System.Drawing.Size(241, 23);
            this.label_Socket_Result1.TabIndex = 5;
            this.label_Socket_Result1.Text = "#1 RESULT";
            this.label_Socket_Result1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Socket_Result2
            // 
            this.label_Socket_Result2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_Socket_Result2.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Socket_Result2.ForeColor = System.Drawing.Color.White;
            this.label_Socket_Result2.Location = new System.Drawing.Point(1469, 382);
            this.label_Socket_Result2.Name = "label_Socket_Result2";
            this.label_Socket_Result2.Size = new System.Drawing.Size(241, 23);
            this.label_Socket_Result2.TabIndex = 6;
            this.label_Socket_Result2.Text = "#2 RESULT";
            this.label_Socket_Result2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_TopCam_Image_Load
            // 
            this.btn_TopCam_Image_Load.Location = new System.Drawing.Point(601, 522);
            this.btn_TopCam_Image_Load.Name = "btn_TopCam_Image_Load";
            this.btn_TopCam_Image_Load.Size = new System.Drawing.Size(75, 29);
            this.btn_TopCam_Image_Load.TabIndex = 7;
            this.btn_TopCam_Image_Load.Text = "Load";
            this.btn_TopCam_Image_Load.UseVisualStyleBackColor = true;
            this.btn_TopCam_Image_Load.Click += new System.EventHandler(this.btn_TopCam_Image_Load_Click);
            // 
            // btn_TopCam_Image_Save
            // 
            this.btn_TopCam_Image_Save.Location = new System.Drawing.Point(520, 522);
            this.btn_TopCam_Image_Save.Name = "btn_TopCam_Image_Save";
            this.btn_TopCam_Image_Save.Size = new System.Drawing.Size(75, 29);
            this.btn_TopCam_Image_Save.TabIndex = 8;
            this.btn_TopCam_Image_Save.Text = "Save";
            this.btn_TopCam_Image_Save.UseVisualStyleBackColor = true;
            this.btn_TopCam_Image_Save.Click += new System.EventHandler(this.btn_TopCam_Image_Save_Click);
            // 
            // btn_SideCam_Image_Save
            // 
            this.btn_SideCam_Image_Save.Location = new System.Drawing.Point(1201, 522);
            this.btn_SideCam_Image_Save.Name = "btn_SideCam_Image_Save";
            this.btn_SideCam_Image_Save.Size = new System.Drawing.Size(75, 29);
            this.btn_SideCam_Image_Save.TabIndex = 10;
            this.btn_SideCam_Image_Save.Text = "Save";
            this.btn_SideCam_Image_Save.UseVisualStyleBackColor = true;
            this.btn_SideCam_Image_Save.Click += new System.EventHandler(this.btn_SideCam_Image_Save_Click);
            // 
            // btn_SideCam_Image_Load
            // 
            this.btn_SideCam_Image_Load.Location = new System.Drawing.Point(1282, 522);
            this.btn_SideCam_Image_Load.Name = "btn_SideCam_Image_Load";
            this.btn_SideCam_Image_Load.Size = new System.Drawing.Size(75, 29);
            this.btn_SideCam_Image_Load.TabIndex = 9;
            this.btn_SideCam_Image_Load.Text = "Load";
            this.btn_SideCam_Image_Load.UseVisualStyleBackColor = true;
            this.btn_SideCam_Image_Load.Click += new System.EventHandler(this.btn_SideCam_Image_Load_Click);
            // 
            // panelCam2
            // 
            this.panelCam2.Location = new System.Drawing.Point(684, 36);
            this.panelCam2.Name = "panelCam2";
            this.panelCam2.Size = new System.Drawing.Size(673, 482);
            this.panelCam2.TabIndex = 5;
            // 
            // CameraControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelCam2);
            this.Controls.Add(this.btn_SideCam_Image_Save);
            this.Controls.Add(this.btn_SideCam_Image_Load);
            this.Controls.Add(this.btn_TopCam_Image_Save);
            this.Controls.Add(this.btn_TopCam_Image_Load);
            this.Controls.Add(this.label_Socket_Result2);
            this.Controls.Add(this.label_Socket_Result1);
            this.Controls.Add(this.panelCam1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CameraControl";
            this.Size = new System.Drawing.Size(1770, 800);
            this.VisibleChanged += new System.EventHandler(this.CameraControl_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_Socket_Result1;
        private System.Windows.Forms.Label label_Socket_Result2;
        private System.Windows.Forms.Button btn_TopCam_Image_Load;
        private System.Windows.Forms.Button btn_TopCam_Image_Save;
        private System.Windows.Forms.Button btn_SideCam_Image_Save;
        private System.Windows.Forms.Button btn_SideCam_Image_Load;
        public System.Windows.Forms.Panel panelCam1;
        public System.Windows.Forms.Panel panelCam2;
    }
}
