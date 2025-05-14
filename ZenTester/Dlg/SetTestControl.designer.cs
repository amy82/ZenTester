
namespace ZenHandler.Dlg
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
            this.label_SetTest_Manual_Top_Test = new System.Windows.Forms.Label();
            this.label_SetTest_Manual_Side_Test = new System.Windows.Forms.Label();
            this.button_Set_Key_Test = new System.Windows.Forms.Button();
            this.button_Set_Housing_Test = new System.Windows.Forms.Button();
            this.button_Set_Gasket_Test = new System.Windows.Forms.Button();
            this.button_Set_Dent_Test = new System.Windows.Forms.Button();
            this.button_Set_Oring_Test = new System.Windows.Forms.Button();
            this.button_Set_Cone_Test = new System.Windows.Forms.Button();
            this.button_Set_Height_Test = new System.Windows.Forms.Button();
            this.button_Pogo_Find_Test = new System.Windows.Forms.Button();
            this.checkBox_Measure = new System.Windows.Forms.CheckBox();
            this.checkBox_Roi_Height = new System.Windows.Forms.CheckBox();
            this.checkBox_Roi_Cone = new System.Windows.Forms.CheckBox();
            this.checkBox_Roi_ORing = new System.Windows.Forms.CheckBox();
            this.checkBox_Roi_Key = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label_SetTest_Title
            // 
            this.label_SetTest_Title.BackColor = System.Drawing.SystemColors.WindowFrame;
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
            this.button_SetTest_TopCam.Location = new System.Drawing.Point(1033, 12);
            this.button_SetTest_TopCam.Name = "button_SetTest_TopCam";
            this.button_SetTest_TopCam.Size = new System.Drawing.Size(75, 23);
            this.button_SetTest_TopCam.TabIndex = 5;
            this.button_SetTest_TopCam.Text = "Top Cam";
            this.button_SetTest_TopCam.UseVisualStyleBackColor = true;
            this.button_SetTest_TopCam.Click += new System.EventHandler(this.button_SetTest_TopCam_Click);
            // 
            // button_SetTest_SideCam
            // 
            this.button_SetTest_SideCam.Location = new System.Drawing.Point(1114, 12);
            this.button_SetTest_SideCam.Name = "button_SetTest_SideCam";
            this.button_SetTest_SideCam.Size = new System.Drawing.Size(75, 23);
            this.button_SetTest_SideCam.TabIndex = 6;
            this.button_SetTest_SideCam.Text = "Side Cam";
            this.button_SetTest_SideCam.UseVisualStyleBackColor = true;
            this.button_SetTest_SideCam.Click += new System.EventHandler(this.button_SetTest_SideCam_Click);
            // 
            // label_SetTest_Manual_Top_Test
            // 
            this.label_SetTest_Manual_Top_Test.BackColor = System.Drawing.SystemColors.Window;
            this.label_SetTest_Manual_Top_Test.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_SetTest_Manual_Top_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_SetTest_Manual_Top_Test.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Manual_Top_Test.ForeColor = System.Drawing.Color.Black;
            this.label_SetTest_Manual_Top_Test.Location = new System.Drawing.Point(1049, 92);
            this.label_SetTest_Manual_Top_Test.Name = "label_SetTest_Manual_Top_Test";
            this.label_SetTest_Manual_Top_Test.Size = new System.Drawing.Size(302, 29);
            this.label_SetTest_Manual_Top_Test.TabIndex = 52;
            this.label_SetTest_Manual_Top_Test.Text = "Top Cam Manual Test";
            this.label_SetTest_Manual_Top_Test.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_SetTest_Manual_Side_Test
            // 
            this.label_SetTest_Manual_Side_Test.BackColor = System.Drawing.SystemColors.Window;
            this.label_SetTest_Manual_Side_Test.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_SetTest_Manual_Side_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_SetTest_Manual_Side_Test.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Manual_Side_Test.ForeColor = System.Drawing.Color.Black;
            this.label_SetTest_Manual_Side_Test.Location = new System.Drawing.Point(1049, 371);
            this.label_SetTest_Manual_Side_Test.Name = "label_SetTest_Manual_Side_Test";
            this.label_SetTest_Manual_Side_Test.Size = new System.Drawing.Size(302, 29);
            this.label_SetTest_Manual_Side_Test.TabIndex = 53;
            this.label_SetTest_Manual_Side_Test.Text = "Side Cam Manual Test";
            this.label_SetTest_Manual_Side_Test.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Set_Key_Test
            // 
            this.button_Set_Key_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Key_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Key_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Key_Test.Location = new System.Drawing.Point(1049, 123);
            this.button_Set_Key_Test.Name = "button_Set_Key_Test";
            this.button_Set_Key_Test.Size = new System.Drawing.Size(140, 56);
            this.button_Set_Key_Test.TabIndex = 54;
            this.button_Set_Key_Test.Text = "KEY TEST";
            this.button_Set_Key_Test.UseVisualStyleBackColor = false;
            this.button_Set_Key_Test.Click += new System.EventHandler(this.button_Set_Key_Test_Click);
            // 
            // button_Set_Housing_Test
            // 
            this.button_Set_Housing_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Housing_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Housing_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Housing_Test.Location = new System.Drawing.Point(1049, 178);
            this.button_Set_Housing_Test.Name = "button_Set_Housing_Test";
            this.button_Set_Housing_Test.Size = new System.Drawing.Size(140, 56);
            this.button_Set_Housing_Test.TabIndex = 55;
            this.button_Set_Housing_Test.Text = "HOUSING TEST";
            this.button_Set_Housing_Test.UseVisualStyleBackColor = false;
            this.button_Set_Housing_Test.Click += new System.EventHandler(this.button_Set_Housing_Test_Click);
            // 
            // button_Set_Gasket_Test
            // 
            this.button_Set_Gasket_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Gasket_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Gasket_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Gasket_Test.Location = new System.Drawing.Point(1049, 233);
            this.button_Set_Gasket_Test.Name = "button_Set_Gasket_Test";
            this.button_Set_Gasket_Test.Size = new System.Drawing.Size(140, 56);
            this.button_Set_Gasket_Test.TabIndex = 56;
            this.button_Set_Gasket_Test.Text = "GASKET TEST";
            this.button_Set_Gasket_Test.UseVisualStyleBackColor = false;
            this.button_Set_Gasket_Test.Click += new System.EventHandler(this.button_Set_Gasket_Test_Click);
            // 
            // button_Set_Dent_Test
            // 
            this.button_Set_Dent_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Dent_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Dent_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Dent_Test.Location = new System.Drawing.Point(1049, 288);
            this.button_Set_Dent_Test.Name = "button_Set_Dent_Test";
            this.button_Set_Dent_Test.Size = new System.Drawing.Size(140, 56);
            this.button_Set_Dent_Test.TabIndex = 57;
            this.button_Set_Dent_Test.Text = "DENT TEST";
            this.button_Set_Dent_Test.UseVisualStyleBackColor = false;
            this.button_Set_Dent_Test.Click += new System.EventHandler(this.button_Set_Dent_Test_Click);
            // 
            // button_Set_Oring_Test
            // 
            this.button_Set_Oring_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Oring_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Oring_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Oring_Test.Location = new System.Drawing.Point(1049, 403);
            this.button_Set_Oring_Test.Name = "button_Set_Oring_Test";
            this.button_Set_Oring_Test.Size = new System.Drawing.Size(140, 56);
            this.button_Set_Oring_Test.TabIndex = 58;
            this.button_Set_Oring_Test.Text = "ORING TEST";
            this.button_Set_Oring_Test.UseVisualStyleBackColor = false;
            this.button_Set_Oring_Test.Click += new System.EventHandler(this.button_Set_Oring_Test_Click);
            // 
            // button_Set_Cone_Test
            // 
            this.button_Set_Cone_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Cone_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Cone_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Cone_Test.Location = new System.Drawing.Point(1049, 458);
            this.button_Set_Cone_Test.Name = "button_Set_Cone_Test";
            this.button_Set_Cone_Test.Size = new System.Drawing.Size(140, 56);
            this.button_Set_Cone_Test.TabIndex = 59;
            this.button_Set_Cone_Test.Text = "CONE TEST";
            this.button_Set_Cone_Test.UseVisualStyleBackColor = false;
            this.button_Set_Cone_Test.Click += new System.EventHandler(this.button_Set_Cone_Test_Click);
            // 
            // button_Set_Height_Test
            // 
            this.button_Set_Height_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Height_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Height_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Height_Test.Location = new System.Drawing.Point(1049, 513);
            this.button_Set_Height_Test.Name = "button_Set_Height_Test";
            this.button_Set_Height_Test.Size = new System.Drawing.Size(140, 56);
            this.button_Set_Height_Test.TabIndex = 60;
            this.button_Set_Height_Test.Text = "HEIGHT TEST";
            this.button_Set_Height_Test.UseVisualStyleBackColor = false;
            this.button_Set_Height_Test.Click += new System.EventHandler(this.button_Set_Height_Test_Click);
            // 
            // button_Pogo_Find_Test
            // 
            this.button_Pogo_Find_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Pogo_Find_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Pogo_Find_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Pogo_Find_Test.Location = new System.Drawing.Point(1304, 33);
            this.button_Pogo_Find_Test.Name = "button_Pogo_Find_Test";
            this.button_Pogo_Find_Test.Size = new System.Drawing.Size(140, 56);
            this.button_Pogo_Find_Test.TabIndex = 61;
            this.button_Pogo_Find_Test.Text = "POGO FIND TEST";
            this.button_Pogo_Find_Test.UseVisualStyleBackColor = false;
            this.button_Pogo_Find_Test.Click += new System.EventHandler(this.button_Pogo_Find_Test_Click);
            // 
            // checkBox_Measure
            // 
            this.checkBox_Measure.AutoSize = true;
            this.checkBox_Measure.BackColor = System.Drawing.Color.Cornsilk;
            this.checkBox_Measure.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_Measure.ForeColor = System.Drawing.Color.Black;
            this.checkBox_Measure.Location = new System.Drawing.Point(1049, 50);
            this.checkBox_Measure.Name = "checkBox_Measure";
            this.checkBox_Measure.Size = new System.Drawing.Size(200, 28);
            this.checkBox_Measure.TabIndex = 62;
            this.checkBox_Measure.Text = "Measure Distance";
            this.checkBox_Measure.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Measure.UseVisualStyleBackColor = false;
            this.checkBox_Measure.CheckedChanged += new System.EventHandler(this.checkBox_Measure_CheckedChanged);
            // 
            // checkBox_Roi_Height
            // 
            this.checkBox_Roi_Height.AutoSize = true;
            this.checkBox_Roi_Height.BackColor = System.Drawing.Color.Cornsilk;
            this.checkBox_Roi_Height.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_Roi_Height.ForeColor = System.Drawing.Color.Black;
            this.checkBox_Roi_Height.Location = new System.Drawing.Point(1195, 527);
            this.checkBox_Roi_Height.Name = "checkBox_Roi_Height";
            this.checkBox_Roi_Height.Size = new System.Drawing.Size(145, 28);
            this.checkBox_Roi_Height.TabIndex = 63;
            this.checkBox_Roi_Height.Text = "ROI HEIGHT";
            this.checkBox_Roi_Height.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Roi_Height.UseVisualStyleBackColor = false;
            // 
            // checkBox_Roi_Cone
            // 
            this.checkBox_Roi_Cone.AutoSize = true;
            this.checkBox_Roi_Cone.BackColor = System.Drawing.Color.Cornsilk;
            this.checkBox_Roi_Cone.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_Roi_Cone.ForeColor = System.Drawing.Color.Black;
            this.checkBox_Roi_Cone.Location = new System.Drawing.Point(1195, 472);
            this.checkBox_Roi_Cone.Name = "checkBox_Roi_Cone";
            this.checkBox_Roi_Cone.Size = new System.Drawing.Size(128, 28);
            this.checkBox_Roi_Cone.TabIndex = 64;
            this.checkBox_Roi_Cone.Text = "ROI CONE";
            this.checkBox_Roi_Cone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Roi_Cone.UseVisualStyleBackColor = false;
            // 
            // checkBox_Roi_ORing
            // 
            this.checkBox_Roi_ORing.AutoSize = true;
            this.checkBox_Roi_ORing.BackColor = System.Drawing.Color.Cornsilk;
            this.checkBox_Roi_ORing.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_Roi_ORing.ForeColor = System.Drawing.Color.Black;
            this.checkBox_Roi_ORing.Location = new System.Drawing.Point(1195, 417);
            this.checkBox_Roi_ORing.Name = "checkBox_Roi_ORing";
            this.checkBox_Roi_ORing.Size = new System.Drawing.Size(137, 28);
            this.checkBox_Roi_ORing.TabIndex = 65;
            this.checkBox_Roi_ORing.Text = "ROI ORING";
            this.checkBox_Roi_ORing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Roi_ORing.UseVisualStyleBackColor = false;
            // 
            // checkBox_Roi_Key
            // 
            this.checkBox_Roi_Key.AutoSize = true;
            this.checkBox_Roi_Key.BackColor = System.Drawing.Color.Cornsilk;
            this.checkBox_Roi_Key.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_Roi_Key.ForeColor = System.Drawing.Color.Black;
            this.checkBox_Roi_Key.Location = new System.Drawing.Point(1195, 137);
            this.checkBox_Roi_Key.Name = "checkBox_Roi_Key";
            this.checkBox_Roi_Key.Size = new System.Drawing.Size(109, 28);
            this.checkBox_Roi_Key.TabIndex = 69;
            this.checkBox_Roi_Key.Text = "ROI KEY";
            this.checkBox_Roi_Key.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_Roi_Key.UseVisualStyleBackColor = false;
            // 
            // SetTestControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
            this.Controls.Add(this.checkBox_Roi_Key);
            this.Controls.Add(this.checkBox_Roi_ORing);
            this.Controls.Add(this.checkBox_Roi_Cone);
            this.Controls.Add(this.checkBox_Roi_Height);
            this.Controls.Add(this.checkBox_Measure);
            this.Controls.Add(this.button_Pogo_Find_Test);
            this.Controls.Add(this.button_Set_Height_Test);
            this.Controls.Add(this.button_Set_Cone_Test);
            this.Controls.Add(this.button_Set_Oring_Test);
            this.Controls.Add(this.button_Set_Dent_Test);
            this.Controls.Add(this.button_Set_Gasket_Test);
            this.Controls.Add(this.button_Set_Housing_Test);
            this.Controls.Add(this.button_Set_Key_Test);
            this.Controls.Add(this.label_SetTest_Manual_Side_Test);
            this.Controls.Add(this.label_SetTest_Manual_Top_Test);
            this.Controls.Add(this.button_SetTest_SideCam);
            this.Controls.Add(this.button_SetTest_TopCam);
            this.Controls.Add(this.Set_panelCam);
            this.Controls.Add(this.label_SetTest_Title);
            this.Name = "SetTestControl";
            this.Size = new System.Drawing.Size(1770, 800);
            this.VisibleChanged += new System.EventHandler(this.SetTestControl_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label_SetTest_Title;
        private System.Windows.Forms.Button button_SetTest_TopCam;
        private System.Windows.Forms.Button button_SetTest_SideCam;
        public System.Windows.Forms.Panel Set_panelCam;
        public System.Windows.Forms.Label label_SetTest_Manual_Top_Test;
        public System.Windows.Forms.Label label_SetTest_Manual_Side_Test;
        private System.Windows.Forms.Button button_Set_Key_Test;
        private System.Windows.Forms.Button button_Set_Housing_Test;
        private System.Windows.Forms.Button button_Set_Gasket_Test;
        private System.Windows.Forms.Button button_Set_Dent_Test;
        private System.Windows.Forms.Button button_Set_Oring_Test;
        private System.Windows.Forms.Button button_Set_Cone_Test;
        private System.Windows.Forms.Button button_Set_Height_Test;
        private System.Windows.Forms.Button button_Pogo_Find_Test;
        private System.Windows.Forms.CheckBox checkBox_Measure;
        private System.Windows.Forms.CheckBox checkBox_Roi_Height;
        private System.Windows.Forms.CheckBox checkBox_Roi_Cone;
        private System.Windows.Forms.CheckBox checkBox_Roi_ORing;
        private System.Windows.Forms.CheckBox checkBox_Roi_Key;
    }
}
