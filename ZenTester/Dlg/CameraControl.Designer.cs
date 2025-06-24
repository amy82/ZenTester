
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
            this.panelCam2 = new System.Windows.Forms.Panel();
            this.label_Aoi_Result_Lh_Val1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Lh1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Mh_Val1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Mh1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Rh_Val1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Rh1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Cone_Val1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Cone1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ORing_Val1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ORing1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Gasket_Val1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Gasket1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Key_Val1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Key1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Dent_Val1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Dent1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ConA_Val1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ConA1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ConD_Val1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ConD1 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ConD_Val2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ConD2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ConA_Val2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ConA2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Dent_Val2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Dent2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Key_Val2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Key2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Gasket_Val2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Gasket2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ORing_Val2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_ORing2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Cone_Val2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Cone2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Rh_Val2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Rh2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Mh_Val2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Mh2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Lh_Val2 = new System.Windows.Forms.Label();
            this.label_Aoi_Result_Lh2 = new System.Windows.Forms.Label();
            this.label_LogTitle = new System.Windows.Forms.Label();
            this.listBox_Log = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(623, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "TOP CAM";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(628, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(623, 27);
            this.label2.TabIndex = 3;
            this.label2.Text = "SIDE CAM";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelCam1
            // 
            this.panelCam1.Location = new System.Drawing.Point(3, 34);
            this.panelCam1.Name = "panelCam1";
            this.panelCam1.Size = new System.Drawing.Size(623, 482);
            this.panelCam1.TabIndex = 4;
            // 
            // label_Socket_Result1
            // 
            this.label_Socket_Result1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label_Socket_Result1.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Socket_Result1.ForeColor = System.Drawing.Color.Black;
            this.label_Socket_Result1.Location = new System.Drawing.Point(20, 528);
            this.label_Socket_Result1.Name = "label_Socket_Result1";
            this.label_Socket_Result1.Size = new System.Drawing.Size(550, 23);
            this.label_Socket_Result1.TabIndex = 5;
            this.label_Socket_Result1.Text = "*LEFT SOCKET RESULT";
            this.label_Socket_Result1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Socket_Result2
            // 
            this.label_Socket_Result2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label_Socket_Result2.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Socket_Result2.ForeColor = System.Drawing.Color.Black;
            this.label_Socket_Result2.Location = new System.Drawing.Point(667, 528);
            this.label_Socket_Result2.Name = "label_Socket_Result2";
            this.label_Socket_Result2.Size = new System.Drawing.Size(550, 23);
            this.label_Socket_Result2.TabIndex = 6;
            this.label_Socket_Result2.Text = "*RIGHT SOCKET RESULT";
            this.label_Socket_Result2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_TopCam_Image_Load
            // 
            this.btn_TopCam_Image_Load.Location = new System.Drawing.Point(1521, 553);
            this.btn_TopCam_Image_Load.Name = "btn_TopCam_Image_Load";
            this.btn_TopCam_Image_Load.Size = new System.Drawing.Size(72, 29);
            this.btn_TopCam_Image_Load.TabIndex = 7;
            this.btn_TopCam_Image_Load.Text = "Load";
            this.btn_TopCam_Image_Load.UseVisualStyleBackColor = true;
            this.btn_TopCam_Image_Load.Visible = false;
            this.btn_TopCam_Image_Load.Click += new System.EventHandler(this.btn_TopCam_Image_Load_Click);
            // 
            // btn_TopCam_Image_Save
            // 
            this.btn_TopCam_Image_Save.Location = new System.Drawing.Point(1450, 553);
            this.btn_TopCam_Image_Save.Name = "btn_TopCam_Image_Save";
            this.btn_TopCam_Image_Save.Size = new System.Drawing.Size(72, 29);
            this.btn_TopCam_Image_Save.TabIndex = 8;
            this.btn_TopCam_Image_Save.Text = "Save";
            this.btn_TopCam_Image_Save.UseVisualStyleBackColor = true;
            this.btn_TopCam_Image_Save.Visible = false;
            this.btn_TopCam_Image_Save.Click += new System.EventHandler(this.btn_TopCam_Image_Save_Click);
            // 
            // panelCam2
            // 
            this.panelCam2.Location = new System.Drawing.Point(628, 34);
            this.panelCam2.Name = "panelCam2";
            this.panelCam2.Size = new System.Drawing.Size(623, 482);
            this.panelCam2.TabIndex = 5;
            // 
            // label_Aoi_Result_Lh_Val1
            // 
            this.label_Aoi_Result_Lh_Val1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Lh_Val1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Lh_Val1.Location = new System.Drawing.Point(192, 553);
            this.label_Aoi_Result_Lh_Val1.Name = "label_Aoi_Result_Lh_Val1";
            this.label_Aoi_Result_Lh_Val1.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Lh_Val1.TabIndex = 34;
            this.label_Aoi_Result_Lh_Val1.Text = "0";
            this.label_Aoi_Result_Lh_Val1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Lh1
            // 
            this.label_Aoi_Result_Lh1.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Lh1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Lh1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Lh1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Lh1.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Lh1.Location = new System.Drawing.Point(30, 553);
            this.label_Aoi_Result_Lh1.Name = "label_Aoi_Result_Lh1";
            this.label_Aoi_Result_Lh1.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Lh1.TabIndex = 33;
            this.label_Aoi_Result_Lh1.Text = "LH (12.390)";
            this.label_Aoi_Result_Lh1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Mh_Val1
            // 
            this.label_Aoi_Result_Mh_Val1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Mh_Val1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Mh_Val1.Location = new System.Drawing.Point(192, 582);
            this.label_Aoi_Result_Mh_Val1.Name = "label_Aoi_Result_Mh_Val1";
            this.label_Aoi_Result_Mh_Val1.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Mh_Val1.TabIndex = 36;
            this.label_Aoi_Result_Mh_Val1.Text = "0";
            this.label_Aoi_Result_Mh_Val1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Mh1
            // 
            this.label_Aoi_Result_Mh1.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Mh1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Mh1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Mh1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Mh1.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Mh1.Location = new System.Drawing.Point(30, 582);
            this.label_Aoi_Result_Mh1.Name = "label_Aoi_Result_Mh1";
            this.label_Aoi_Result_Mh1.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Mh1.TabIndex = 35;
            this.label_Aoi_Result_Mh1.Text = "MH (34.550)";
            this.label_Aoi_Result_Mh1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Rh_Val1
            // 
            this.label_Aoi_Result_Rh_Val1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Rh_Val1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Rh_Val1.Location = new System.Drawing.Point(192, 611);
            this.label_Aoi_Result_Rh_Val1.Name = "label_Aoi_Result_Rh_Val1";
            this.label_Aoi_Result_Rh_Val1.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Rh_Val1.TabIndex = 38;
            this.label_Aoi_Result_Rh_Val1.Text = "0";
            this.label_Aoi_Result_Rh_Val1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Rh1
            // 
            this.label_Aoi_Result_Rh1.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Rh1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Rh1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Rh1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Rh1.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Rh1.Location = new System.Drawing.Point(30, 611);
            this.label_Aoi_Result_Rh1.Name = "label_Aoi_Result_Rh1";
            this.label_Aoi_Result_Rh1.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Rh1.TabIndex = 37;
            this.label_Aoi_Result_Rh1.Text = "RH (12.390)";
            this.label_Aoi_Result_Rh1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Cone_Val1
            // 
            this.label_Aoi_Result_Cone_Val1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Cone_Val1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Cone_Val1.Location = new System.Drawing.Point(192, 640);
            this.label_Aoi_Result_Cone_Val1.Name = "label_Aoi_Result_Cone_Val1";
            this.label_Aoi_Result_Cone_Val1.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Cone_Val1.TabIndex = 40;
            this.label_Aoi_Result_Cone_Val1.Text = "0";
            this.label_Aoi_Result_Cone_Val1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Cone1
            // 
            this.label_Aoi_Result_Cone1.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Cone1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Cone1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Cone1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Cone1.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Cone1.Location = new System.Drawing.Point(30, 640);
            this.label_Aoi_Result_Cone1.Name = "label_Aoi_Result_Cone1";
            this.label_Aoi_Result_Cone1.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Cone1.TabIndex = 39;
            this.label_Aoi_Result_Cone1.Text = "Cone (O)";
            this.label_Aoi_Result_Cone1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ORing_Val1
            // 
            this.label_Aoi_Result_ORing_Val1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ORing_Val1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ORing_Val1.Location = new System.Drawing.Point(192, 669);
            this.label_Aoi_Result_ORing_Val1.Name = "label_Aoi_Result_ORing_Val1";
            this.label_Aoi_Result_ORing_Val1.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_ORing_Val1.TabIndex = 42;
            this.label_Aoi_Result_ORing_Val1.Text = "0";
            this.label_Aoi_Result_ORing_Val1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ORing1
            // 
            this.label_Aoi_Result_ORing1.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_ORing1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ORing1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_ORing1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ORing1.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_ORing1.Location = new System.Drawing.Point(30, 669);
            this.label_Aoi_Result_ORing1.Name = "label_Aoi_Result_ORing1";
            this.label_Aoi_Result_ORing1.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_ORing1.TabIndex = 41;
            this.label_Aoi_Result_ORing1.Text = "ORing (X)";
            this.label_Aoi_Result_ORing1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Gasket_Val1
            // 
            this.label_Aoi_Result_Gasket_Val1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Gasket_Val1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Gasket_Val1.Location = new System.Drawing.Point(456, 554);
            this.label_Aoi_Result_Gasket_Val1.Name = "label_Aoi_Result_Gasket_Val1";
            this.label_Aoi_Result_Gasket_Val1.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Gasket_Val1.TabIndex = 44;
            this.label_Aoi_Result_Gasket_Val1.Text = "0";
            this.label_Aoi_Result_Gasket_Val1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Gasket1
            // 
            this.label_Aoi_Result_Gasket1.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Gasket1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Gasket1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Gasket1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Gasket1.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Gasket1.Location = new System.Drawing.Point(294, 554);
            this.label_Aoi_Result_Gasket1.Name = "label_Aoi_Result_Gasket1";
            this.label_Aoi_Result_Gasket1.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Gasket1.TabIndex = 43;
            this.label_Aoi_Result_Gasket1.Text = "Gasket (73)";
            this.label_Aoi_Result_Gasket1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Key_Val1
            // 
            this.label_Aoi_Result_Key_Val1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Key_Val1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Key_Val1.Location = new System.Drawing.Point(456, 583);
            this.label_Aoi_Result_Key_Val1.Name = "label_Aoi_Result_Key_Val1";
            this.label_Aoi_Result_Key_Val1.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Key_Val1.TabIndex = 46;
            this.label_Aoi_Result_Key_Val1.Text = "0";
            this.label_Aoi_Result_Key_Val1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Key1
            // 
            this.label_Aoi_Result_Key1.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Key1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Key1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Key1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Key1.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Key1.Location = new System.Drawing.Point(294, 583);
            this.label_Aoi_Result_Key1.Name = "label_Aoi_Result_Key1";
            this.label_Aoi_Result_Key1.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Key1.TabIndex = 45;
            this.label_Aoi_Result_Key1.Text = "Key (A)";
            this.label_Aoi_Result_Key1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Dent_Val1
            // 
            this.label_Aoi_Result_Dent_Val1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Dent_Val1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Dent_Val1.Location = new System.Drawing.Point(456, 612);
            this.label_Aoi_Result_Dent_Val1.Name = "label_Aoi_Result_Dent_Val1";
            this.label_Aoi_Result_Dent_Val1.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Dent_Val1.TabIndex = 48;
            this.label_Aoi_Result_Dent_Val1.Text = "0";
            this.label_Aoi_Result_Dent_Val1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Dent1
            // 
            this.label_Aoi_Result_Dent1.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Dent1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Dent1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Dent1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Dent1.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Dent1.Location = new System.Drawing.Point(294, 612);
            this.label_Aoi_Result_Dent1.Name = "label_Aoi_Result_Dent1";
            this.label_Aoi_Result_Dent1.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Dent1.TabIndex = 47;
            this.label_Aoi_Result_Dent1.Text = "Dent (200)";
            this.label_Aoi_Result_Dent1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ConA_Val1
            // 
            this.label_Aoi_Result_ConA_Val1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ConA_Val1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ConA_Val1.Location = new System.Drawing.Point(456, 641);
            this.label_Aoi_Result_ConA_Val1.Name = "label_Aoi_Result_ConA_Val1";
            this.label_Aoi_Result_ConA_Val1.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_ConA_Val1.TabIndex = 50;
            this.label_Aoi_Result_ConA_Val1.Text = "0";
            this.label_Aoi_Result_ConA_Val1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ConA1
            // 
            this.label_Aoi_Result_ConA1.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_ConA1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ConA1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_ConA1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ConA1.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_ConA1.Location = new System.Drawing.Point(294, 641);
            this.label_Aoi_Result_ConA1.Name = "label_Aoi_Result_ConA1";
            this.label_Aoi_Result_ConA1.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_ConA1.TabIndex = 49;
            this.label_Aoi_Result_ConA1.Text = "Con1 (0.3)";
            this.label_Aoi_Result_ConA1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ConD_Val1
            // 
            this.label_Aoi_Result_ConD_Val1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ConD_Val1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ConD_Val1.Location = new System.Drawing.Point(456, 670);
            this.label_Aoi_Result_ConD_Val1.Name = "label_Aoi_Result_ConD_Val1";
            this.label_Aoi_Result_ConD_Val1.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_ConD_Val1.TabIndex = 52;
            this.label_Aoi_Result_ConD_Val1.Text = "0";
            this.label_Aoi_Result_ConD_Val1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ConD1
            // 
            this.label_Aoi_Result_ConD1.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_ConD1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ConD1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_ConD1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ConD1.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_ConD1.Location = new System.Drawing.Point(294, 670);
            this.label_Aoi_Result_ConD1.Name = "label_Aoi_Result_ConD1";
            this.label_Aoi_Result_ConD1.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_ConD1.TabIndex = 51;
            this.label_Aoi_Result_ConD1.Text = "Con2 (0.3)";
            this.label_Aoi_Result_ConD1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ConD_Val2
            // 
            this.label_Aoi_Result_ConD_Val2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ConD_Val2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ConD_Val2.Location = new System.Drawing.Point(1108, 670);
            this.label_Aoi_Result_ConD_Val2.Name = "label_Aoi_Result_ConD_Val2";
            this.label_Aoi_Result_ConD_Val2.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_ConD_Val2.TabIndex = 72;
            this.label_Aoi_Result_ConD_Val2.Text = "0";
            this.label_Aoi_Result_ConD_Val2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ConD2
            // 
            this.label_Aoi_Result_ConD2.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_ConD2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ConD2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_ConD2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ConD2.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_ConD2.Location = new System.Drawing.Point(946, 670);
            this.label_Aoi_Result_ConD2.Name = "label_Aoi_Result_ConD2";
            this.label_Aoi_Result_ConD2.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_ConD2.TabIndex = 71;
            this.label_Aoi_Result_ConD2.Text = "Con2 (0.3)";
            this.label_Aoi_Result_ConD2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ConA_Val2
            // 
            this.label_Aoi_Result_ConA_Val2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ConA_Val2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ConA_Val2.Location = new System.Drawing.Point(1108, 641);
            this.label_Aoi_Result_ConA_Val2.Name = "label_Aoi_Result_ConA_Val2";
            this.label_Aoi_Result_ConA_Val2.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_ConA_Val2.TabIndex = 70;
            this.label_Aoi_Result_ConA_Val2.Text = "0";
            this.label_Aoi_Result_ConA_Val2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ConA2
            // 
            this.label_Aoi_Result_ConA2.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_ConA2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ConA2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_ConA2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ConA2.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_ConA2.Location = new System.Drawing.Point(946, 641);
            this.label_Aoi_Result_ConA2.Name = "label_Aoi_Result_ConA2";
            this.label_Aoi_Result_ConA2.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_ConA2.TabIndex = 69;
            this.label_Aoi_Result_ConA2.Text = "Con1 (0.3)";
            this.label_Aoi_Result_ConA2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Dent_Val2
            // 
            this.label_Aoi_Result_Dent_Val2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Dent_Val2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Dent_Val2.Location = new System.Drawing.Point(1108, 612);
            this.label_Aoi_Result_Dent_Val2.Name = "label_Aoi_Result_Dent_Val2";
            this.label_Aoi_Result_Dent_Val2.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Dent_Val2.TabIndex = 68;
            this.label_Aoi_Result_Dent_Val2.Text = "0";
            this.label_Aoi_Result_Dent_Val2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Dent2
            // 
            this.label_Aoi_Result_Dent2.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Dent2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Dent2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Dent2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Dent2.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Dent2.Location = new System.Drawing.Point(946, 612);
            this.label_Aoi_Result_Dent2.Name = "label_Aoi_Result_Dent2";
            this.label_Aoi_Result_Dent2.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Dent2.TabIndex = 67;
            this.label_Aoi_Result_Dent2.Text = "Dent (200)";
            this.label_Aoi_Result_Dent2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Key_Val2
            // 
            this.label_Aoi_Result_Key_Val2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Key_Val2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Key_Val2.Location = new System.Drawing.Point(1108, 583);
            this.label_Aoi_Result_Key_Val2.Name = "label_Aoi_Result_Key_Val2";
            this.label_Aoi_Result_Key_Val2.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Key_Val2.TabIndex = 66;
            this.label_Aoi_Result_Key_Val2.Text = "0";
            this.label_Aoi_Result_Key_Val2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Key2
            // 
            this.label_Aoi_Result_Key2.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Key2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Key2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Key2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Key2.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Key2.Location = new System.Drawing.Point(946, 583);
            this.label_Aoi_Result_Key2.Name = "label_Aoi_Result_Key2";
            this.label_Aoi_Result_Key2.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Key2.TabIndex = 65;
            this.label_Aoi_Result_Key2.Text = "Key (A)";
            this.label_Aoi_Result_Key2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Gasket_Val2
            // 
            this.label_Aoi_Result_Gasket_Val2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Gasket_Val2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Gasket_Val2.Location = new System.Drawing.Point(1108, 554);
            this.label_Aoi_Result_Gasket_Val2.Name = "label_Aoi_Result_Gasket_Val2";
            this.label_Aoi_Result_Gasket_Val2.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Gasket_Val2.TabIndex = 64;
            this.label_Aoi_Result_Gasket_Val2.Text = "0";
            this.label_Aoi_Result_Gasket_Val2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Gasket2
            // 
            this.label_Aoi_Result_Gasket2.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Gasket2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Gasket2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Gasket2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Gasket2.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Gasket2.Location = new System.Drawing.Point(946, 554);
            this.label_Aoi_Result_Gasket2.Name = "label_Aoi_Result_Gasket2";
            this.label_Aoi_Result_Gasket2.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Gasket2.TabIndex = 63;
            this.label_Aoi_Result_Gasket2.Text = "Gasket (73)";
            this.label_Aoi_Result_Gasket2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ORing_Val2
            // 
            this.label_Aoi_Result_ORing_Val2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ORing_Val2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ORing_Val2.Location = new System.Drawing.Point(844, 669);
            this.label_Aoi_Result_ORing_Val2.Name = "label_Aoi_Result_ORing_Val2";
            this.label_Aoi_Result_ORing_Val2.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_ORing_Val2.TabIndex = 62;
            this.label_Aoi_Result_ORing_Val2.Text = "0";
            this.label_Aoi_Result_ORing_Val2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_ORing2
            // 
            this.label_Aoi_Result_ORing2.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_ORing2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_ORing2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_ORing2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_ORing2.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_ORing2.Location = new System.Drawing.Point(682, 669);
            this.label_Aoi_Result_ORing2.Name = "label_Aoi_Result_ORing2";
            this.label_Aoi_Result_ORing2.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_ORing2.TabIndex = 61;
            this.label_Aoi_Result_ORing2.Text = "ORing (X)";
            this.label_Aoi_Result_ORing2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Cone_Val2
            // 
            this.label_Aoi_Result_Cone_Val2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Cone_Val2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Cone_Val2.Location = new System.Drawing.Point(844, 640);
            this.label_Aoi_Result_Cone_Val2.Name = "label_Aoi_Result_Cone_Val2";
            this.label_Aoi_Result_Cone_Val2.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Cone_Val2.TabIndex = 60;
            this.label_Aoi_Result_Cone_Val2.Text = "0";
            this.label_Aoi_Result_Cone_Val2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Cone2
            // 
            this.label_Aoi_Result_Cone2.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Cone2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Cone2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Cone2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Cone2.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Cone2.Location = new System.Drawing.Point(682, 640);
            this.label_Aoi_Result_Cone2.Name = "label_Aoi_Result_Cone2";
            this.label_Aoi_Result_Cone2.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Cone2.TabIndex = 59;
            this.label_Aoi_Result_Cone2.Text = "Cone (O)";
            this.label_Aoi_Result_Cone2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Rh_Val2
            // 
            this.label_Aoi_Result_Rh_Val2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Rh_Val2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Rh_Val2.Location = new System.Drawing.Point(844, 611);
            this.label_Aoi_Result_Rh_Val2.Name = "label_Aoi_Result_Rh_Val2";
            this.label_Aoi_Result_Rh_Val2.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Rh_Val2.TabIndex = 58;
            this.label_Aoi_Result_Rh_Val2.Text = "0";
            this.label_Aoi_Result_Rh_Val2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Rh2
            // 
            this.label_Aoi_Result_Rh2.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Rh2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Rh2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Rh2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Rh2.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Rh2.Location = new System.Drawing.Point(682, 611);
            this.label_Aoi_Result_Rh2.Name = "label_Aoi_Result_Rh2";
            this.label_Aoi_Result_Rh2.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Rh2.TabIndex = 57;
            this.label_Aoi_Result_Rh2.Text = "RH (12.390)";
            this.label_Aoi_Result_Rh2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Mh_Val2
            // 
            this.label_Aoi_Result_Mh_Val2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Mh_Val2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Mh_Val2.Location = new System.Drawing.Point(844, 582);
            this.label_Aoi_Result_Mh_Val2.Name = "label_Aoi_Result_Mh_Val2";
            this.label_Aoi_Result_Mh_Val2.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Mh_Val2.TabIndex = 56;
            this.label_Aoi_Result_Mh_Val2.Text = "0";
            this.label_Aoi_Result_Mh_Val2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Mh2
            // 
            this.label_Aoi_Result_Mh2.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Mh2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Mh2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Mh2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Mh2.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Mh2.Location = new System.Drawing.Point(682, 582);
            this.label_Aoi_Result_Mh2.Name = "label_Aoi_Result_Mh2";
            this.label_Aoi_Result_Mh2.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Mh2.TabIndex = 55;
            this.label_Aoi_Result_Mh2.Text = "MH (34.550)";
            this.label_Aoi_Result_Mh2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Lh_Val2
            // 
            this.label_Aoi_Result_Lh_Val2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Lh_Val2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Lh_Val2.Location = new System.Drawing.Point(844, 553);
            this.label_Aoi_Result_Lh_Val2.Name = "label_Aoi_Result_Lh_Val2";
            this.label_Aoi_Result_Lh_Val2.Size = new System.Drawing.Size(100, 29);
            this.label_Aoi_Result_Lh_Val2.TabIndex = 54;
            this.label_Aoi_Result_Lh_Val2.Text = "0";
            this.label_Aoi_Result_Lh_Val2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Aoi_Result_Lh2
            // 
            this.label_Aoi_Result_Lh2.BackColor = System.Drawing.SystemColors.Window;
            this.label_Aoi_Result_Lh2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Aoi_Result_Lh2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Aoi_Result_Lh2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Aoi_Result_Lh2.ForeColor = System.Drawing.Color.Black;
            this.label_Aoi_Result_Lh2.Location = new System.Drawing.Point(682, 553);
            this.label_Aoi_Result_Lh2.Name = "label_Aoi_Result_Lh2";
            this.label_Aoi_Result_Lh2.Size = new System.Drawing.Size(161, 29);
            this.label_Aoi_Result_Lh2.TabIndex = 53;
            this.label_Aoi_Result_Lh2.Text = "LH (12.390)";
            this.label_Aoi_Result_Lh2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_LogTitle
            // 
            this.label_LogTitle.AutoSize = true;
            this.label_LogTitle.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_LogTitle.Location = new System.Drawing.Point(1257, 13);
            this.label_LogTitle.Name = "label_LogTitle";
            this.label_LogTitle.Size = new System.Drawing.Size(73, 15);
            this.label_LogTitle.TabIndex = 73;
            this.label_LogTitle.Text = " LOG VIEW";
            // 
            // listBox_Log
            // 
            this.listBox_Log.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listBox_Log.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.listBox_Log.FormattingEnabled = true;
            this.listBox_Log.HorizontalScrollbar = true;
            this.listBox_Log.ItemHeight = 15;
            this.listBox_Log.Location = new System.Drawing.Point(1255, 32);
            this.listBox_Log.Margin = new System.Windows.Forms.Padding(0);
            this.listBox_Log.Name = "listBox_Log";
            this.listBox_Log.Size = new System.Drawing.Size(504, 484);
            this.listBox_Log.TabIndex = 74;
            // 
            // CameraControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label_LogTitle);
            this.Controls.Add(this.listBox_Log);
            this.Controls.Add(this.label_Aoi_Result_ConD_Val2);
            this.Controls.Add(this.label_Aoi_Result_ConD2);
            this.Controls.Add(this.label_Aoi_Result_ConA_Val2);
            this.Controls.Add(this.label_Aoi_Result_ConA2);
            this.Controls.Add(this.label_Aoi_Result_Dent_Val2);
            this.Controls.Add(this.label_Aoi_Result_Dent2);
            this.Controls.Add(this.label_Aoi_Result_Key_Val2);
            this.Controls.Add(this.label_Aoi_Result_Key2);
            this.Controls.Add(this.label_Aoi_Result_Gasket_Val2);
            this.Controls.Add(this.label_Aoi_Result_Gasket2);
            this.Controls.Add(this.label_Aoi_Result_ORing_Val2);
            this.Controls.Add(this.label_Aoi_Result_ORing2);
            this.Controls.Add(this.label_Aoi_Result_Cone_Val2);
            this.Controls.Add(this.label_Aoi_Result_Cone2);
            this.Controls.Add(this.label_Aoi_Result_Rh_Val2);
            this.Controls.Add(this.label_Aoi_Result_Rh2);
            this.Controls.Add(this.label_Aoi_Result_Mh_Val2);
            this.Controls.Add(this.label_Aoi_Result_Mh2);
            this.Controls.Add(this.label_Aoi_Result_Lh_Val2);
            this.Controls.Add(this.label_Aoi_Result_Lh2);
            this.Controls.Add(this.label_Aoi_Result_ConD_Val1);
            this.Controls.Add(this.label_Aoi_Result_ConD1);
            this.Controls.Add(this.label_Aoi_Result_ConA_Val1);
            this.Controls.Add(this.label_Aoi_Result_ConA1);
            this.Controls.Add(this.label_Aoi_Result_Dent_Val1);
            this.Controls.Add(this.label_Aoi_Result_Dent1);
            this.Controls.Add(this.label_Aoi_Result_Key_Val1);
            this.Controls.Add(this.label_Aoi_Result_Key1);
            this.Controls.Add(this.label_Aoi_Result_Gasket_Val1);
            this.Controls.Add(this.label_Aoi_Result_Gasket1);
            this.Controls.Add(this.label_Aoi_Result_ORing_Val1);
            this.Controls.Add(this.label_Aoi_Result_ORing1);
            this.Controls.Add(this.label_Aoi_Result_Cone_Val1);
            this.Controls.Add(this.label_Aoi_Result_Cone1);
            this.Controls.Add(this.label_Aoi_Result_Rh_Val1);
            this.Controls.Add(this.label_Aoi_Result_Rh1);
            this.Controls.Add(this.label_Aoi_Result_Mh_Val1);
            this.Controls.Add(this.label_Aoi_Result_Mh1);
            this.Controls.Add(this.label_Aoi_Result_Lh_Val1);
            this.Controls.Add(this.label_Aoi_Result_Lh1);
            this.Controls.Add(this.panelCam2);
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
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_Socket_Result1;
        private System.Windows.Forms.Label label_Socket_Result2;
        private System.Windows.Forms.Button btn_TopCam_Image_Load;
        private System.Windows.Forms.Button btn_TopCam_Image_Save;
        public System.Windows.Forms.Panel panelCam1;
        public System.Windows.Forms.Panel panelCam2;
        private System.Windows.Forms.Label label_Aoi_Result_Lh_Val1;
        public System.Windows.Forms.Label label_Aoi_Result_Lh1;
        private System.Windows.Forms.Label label_Aoi_Result_Mh_Val1;
        public System.Windows.Forms.Label label_Aoi_Result_Mh1;
        private System.Windows.Forms.Label label_Aoi_Result_Rh_Val1;
        public System.Windows.Forms.Label label_Aoi_Result_Rh1;
        private System.Windows.Forms.Label label_Aoi_Result_Cone_Val1;
        public System.Windows.Forms.Label label_Aoi_Result_Cone1;
        private System.Windows.Forms.Label label_Aoi_Result_ORing_Val1;
        public System.Windows.Forms.Label label_Aoi_Result_ORing1;
        private System.Windows.Forms.Label label_Aoi_Result_Gasket_Val1;
        public System.Windows.Forms.Label label_Aoi_Result_Gasket1;
        private System.Windows.Forms.Label label_Aoi_Result_Key_Val1;
        public System.Windows.Forms.Label label_Aoi_Result_Key1;
        private System.Windows.Forms.Label label_Aoi_Result_Dent_Val1;
        public System.Windows.Forms.Label label_Aoi_Result_Dent1;
        private System.Windows.Forms.Label label_Aoi_Result_ConA_Val1;
        public System.Windows.Forms.Label label_Aoi_Result_ConA1;
        private System.Windows.Forms.Label label_Aoi_Result_ConD_Val1;
        public System.Windows.Forms.Label label_Aoi_Result_ConD1;
        private System.Windows.Forms.Label label_Aoi_Result_ConD_Val2;
        public System.Windows.Forms.Label label_Aoi_Result_ConD2;
        private System.Windows.Forms.Label label_Aoi_Result_ConA_Val2;
        public System.Windows.Forms.Label label_Aoi_Result_ConA2;
        private System.Windows.Forms.Label label_Aoi_Result_Dent_Val2;
        public System.Windows.Forms.Label label_Aoi_Result_Dent2;
        private System.Windows.Forms.Label label_Aoi_Result_Key_Val2;
        public System.Windows.Forms.Label label_Aoi_Result_Key2;
        private System.Windows.Forms.Label label_Aoi_Result_Gasket_Val2;
        public System.Windows.Forms.Label label_Aoi_Result_Gasket2;
        private System.Windows.Forms.Label label_Aoi_Result_ORing_Val2;
        public System.Windows.Forms.Label label_Aoi_Result_ORing2;
        private System.Windows.Forms.Label label_Aoi_Result_Cone_Val2;
        public System.Windows.Forms.Label label_Aoi_Result_Cone2;
        private System.Windows.Forms.Label label_Aoi_Result_Rh_Val2;
        public System.Windows.Forms.Label label_Aoi_Result_Rh2;
        private System.Windows.Forms.Label label_Aoi_Result_Mh_Val2;
        public System.Windows.Forms.Label label_Aoi_Result_Mh2;
        private System.Windows.Forms.Label label_Aoi_Result_Lh_Val2;
        public System.Windows.Forms.Label label_Aoi_Result_Lh2;
        private System.Windows.Forms.Label label_LogTitle;
        public System.Windows.Forms.ListBox listBox_Log;
    }
}
