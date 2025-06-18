
namespace ZenTester.Dlg
{
    partial class SetTestControl
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
            this.label_SetTest_Title = new System.Windows.Forms.Label();
            this.Set_panelCam = new System.Windows.Forms.Panel();
            this.button_SetTest_TopCam = new System.Windows.Forms.Button();
            this.button_SetTest_SideCam = new System.Windows.Forms.Button();
            this.btn_Set_Config_Control = new System.Windows.Forms.Button();
            this.btn_Set_Test_Control = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_SetTest_Title
            // 
            this.label_SetTest_Title.BackColor = System.Drawing.Color.Black;
            this.label_SetTest_Title.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Title.ForeColor = System.Drawing.Color.White;
            this.label_SetTest_Title.Location = new System.Drawing.Point(3, 10);
            this.label_SetTest_Title.Name = "label_SetTest_Title";
            this.label_SetTest_Title.Size = new System.Drawing.Size(1020, 23);
            this.label_SetTest_Title.TabIndex = 2;
            this.label_SetTest_Title.Text = "Setting Camera";
            this.label_SetTest_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Set_panelCam
            // 
            this.Set_panelCam.BackColor = System.Drawing.Color.DarkKhaki;
            this.Set_panelCam.Location = new System.Drawing.Point(3, 35);
            this.Set_panelCam.Name = "Set_panelCam";
            this.Set_panelCam.Size = new System.Drawing.Size(1020, 730);
            this.Set_panelCam.TabIndex = 4;
            this.Set_panelCam.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Set_panelCam_MouseDown);
            this.Set_panelCam.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Set_panelCam_MouseMove);
            this.Set_panelCam.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Set_panelCam_MouseUp);
            // 
            // button_SetTest_TopCam
            // 
            this.button_SetTest_TopCam.BackColor = System.Drawing.Color.Black;
            this.button_SetTest_TopCam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_SetTest_TopCam.ForeColor = System.Drawing.Color.White;
            this.button_SetTest_TopCam.Location = new System.Drawing.Point(1049, 8);
            this.button_SetTest_TopCam.Name = "button_SetTest_TopCam";
            this.button_SetTest_TopCam.Size = new System.Drawing.Size(73, 30);
            this.button_SetTest_TopCam.TabIndex = 5;
            this.button_SetTest_TopCam.Text = "Top Cam";
            this.button_SetTest_TopCam.UseVisualStyleBackColor = false;
            this.button_SetTest_TopCam.Click += new System.EventHandler(this.button_SetTest_TopCam_Click);
            // 
            // button_SetTest_SideCam
            // 
            this.button_SetTest_SideCam.BackColor = System.Drawing.Color.DarkGray;
            this.button_SetTest_SideCam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_SetTest_SideCam.ForeColor = System.Drawing.Color.White;
            this.button_SetTest_SideCam.Location = new System.Drawing.Point(1129, 8);
            this.button_SetTest_SideCam.Name = "button_SetTest_SideCam";
            this.button_SetTest_SideCam.Size = new System.Drawing.Size(73, 30);
            this.button_SetTest_SideCam.TabIndex = 6;
            this.button_SetTest_SideCam.Text = "Side Cam";
            this.button_SetTest_SideCam.UseVisualStyleBackColor = false;
            this.button_SetTest_SideCam.Click += new System.EventHandler(this.button_SetTest_SideCam_Click);
            // 
            // btn_Set_Config_Control
            // 
            this.btn_Set_Config_Control.BackColor = System.Drawing.Color.Tan;
            this.btn_Set_Config_Control.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Set_Config_Control.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Set_Config_Control.ForeColor = System.Drawing.Color.White;
            this.btn_Set_Config_Control.Location = new System.Drawing.Point(1643, 10);
            this.btn_Set_Config_Control.Name = "btn_Set_Config_Control";
            this.btn_Set_Config_Control.Size = new System.Drawing.Size(87, 44);
            this.btn_Set_Config_Control.TabIndex = 38;
            this.btn_Set_Config_Control.Text = "Config";
            this.btn_Set_Config_Control.UseVisualStyleBackColor = false;
            this.btn_Set_Config_Control.Click += new System.EventHandler(this.btn_Set_Config_Control_Click);
            // 
            // btn_Set_Test_Control
            // 
            this.btn_Set_Test_Control.BackColor = System.Drawing.Color.Tan;
            this.btn_Set_Test_Control.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Set_Test_Control.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Set_Test_Control.ForeColor = System.Drawing.Color.White;
            this.btn_Set_Test_Control.Location = new System.Drawing.Point(1554, 10);
            this.btn_Set_Test_Control.Name = "btn_Set_Test_Control";
            this.btn_Set_Test_Control.Size = new System.Drawing.Size(87, 44);
            this.btn_Set_Test_Control.TabIndex = 37;
            this.btn_Set_Test_Control.Text = "Test";
            this.btn_Set_Test_Control.UseVisualStyleBackColor = false;
            this.btn_Set_Test_Control.Click += new System.EventHandler(this.btn_Set_Test_Control_Click);
            // 
            // SetTestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.Controls.Add(this.btn_Set_Config_Control);
            this.Controls.Add(this.btn_Set_Test_Control);
            this.Controls.Add(this.button_SetTest_SideCam);
            this.Controls.Add(this.button_SetTest_TopCam);
            this.Controls.Add(this.Set_panelCam);
            this.Controls.Add(this.label_SetTest_Title);
            this.Name = "SetTestControl";
            this.Size = new System.Drawing.Size(1770, 800);
            this.VisibleChanged += new System.EventHandler(this.SetTestControl_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label_SetTest_Title;
        private System.Windows.Forms.Button button_SetTest_TopCam;
        private System.Windows.Forms.Button button_SetTest_SideCam;
        public System.Windows.Forms.Panel Set_panelCam;
        private System.Windows.Forms.Button btn_Set_Config_Control;
        private System.Windows.Forms.Button btn_Set_Test_Control;
    }
}
