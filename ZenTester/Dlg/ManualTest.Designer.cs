
namespace ZenTester.Dlg
{
    partial class ManualTest
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
            this.label_SetTest_Manual_Mark_Roi_Save = new System.Windows.Forms.Button();
            this.label_Set_Mark_Model = new System.Windows.Forms.Label();
            this.button_Set_Mark_Next = new System.Windows.Forms.Button();
            this.button_Set_Mark_Prev = new System.Windows.Forms.Button();
            this.label_SetTest_Manual_Image_Load = new System.Windows.Forms.Button();
            this.label_SetTest_Manual_Image_Save = new System.Windows.Forms.Button();
            this.label_SetTest_Manual_Mark_Find = new System.Windows.Forms.Button();
            this.label_SetTest_Manual_Mark_View = new System.Windows.Forms.Button();
            this.label_SetTest_Manual_Mark_Regist = new System.Windows.Forms.Button();
            this.panel_Mark = new System.Windows.Forms.Panel();
            this.label_SetTest_Manual_Mark_Image = new System.Windows.Forms.Label();
            this.button_Set_Height_Test = new System.Windows.Forms.Button();
            this.button_Set_Cone_Test = new System.Windows.Forms.Button();
            this.button_Set_Oring_Test = new System.Windows.Forms.Button();
            this.button_Set_Dent_Test = new System.Windows.Forms.Button();
            this.button_Set_Gasket_Test = new System.Windows.Forms.Button();
            this.button_Set_Housing_Test = new System.Windows.Forms.Button();
            this.button_Set_Key_Test = new System.Windows.Forms.Button();
            this.label_SetTest_Manual_Side_Test = new System.Windows.Forms.Label();
            this.label_SetTest_Manual_Top_Test = new System.Windows.Forms.Label();
            this.button_Pogo_Find_Test = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_SetTest_Manual_Mark_Roi_Save
            // 
            this.label_SetTest_Manual_Mark_Roi_Save.BackColor = System.Drawing.Color.Tan;
            this.label_SetTest_Manual_Mark_Roi_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_SetTest_Manual_Mark_Roi_Save.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Manual_Mark_Roi_Save.Location = new System.Drawing.Point(565, 579);
            this.label_SetTest_Manual_Mark_Roi_Save.Name = "label_SetTest_Manual_Mark_Roi_Save";
            this.label_SetTest_Manual_Mark_Roi_Save.Size = new System.Drawing.Size(98, 38);
            this.label_SetTest_Manual_Mark_Roi_Save.TabIndex = 103;
            this.label_SetTest_Manual_Mark_Roi_Save.Text = "ROI SAVE";
            this.label_SetTest_Manual_Mark_Roi_Save.UseVisualStyleBackColor = false;
            this.label_SetTest_Manual_Mark_Roi_Save.Click += new System.EventHandler(this.label_SetTest_Manual_Mark_Roi_Save_Click);
            // 
            // label_Set_Mark_Model
            // 
            this.label_Set_Mark_Model.BackColor = System.Drawing.Color.White;
            this.label_Set_Mark_Model.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Set_Mark_Model.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Set_Mark_Model.Location = new System.Drawing.Point(309, 656);
            this.label_Set_Mark_Model.Name = "label_Set_Mark_Model";
            this.label_Set_Mark_Model.Size = new System.Drawing.Size(195, 34);
            this.label_Set_Mark_Model.TabIndex = 102;
            this.label_Set_Mark_Model.Text = "A Model";
            this.label_Set_Mark_Model.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Set_Mark_Next
            // 
            this.button_Set_Mark_Next.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Mark_Next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Mark_Next.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Set_Mark_Next.ForeColor = System.Drawing.Color.White;
            this.button_Set_Mark_Next.Location = new System.Drawing.Point(507, 656);
            this.button_Set_Mark_Next.Name = "button_Set_Mark_Next";
            this.button_Set_Mark_Next.Size = new System.Drawing.Size(56, 34);
            this.button_Set_Mark_Next.TabIndex = 101;
            this.button_Set_Mark_Next.Text = "▶";
            this.button_Set_Mark_Next.UseVisualStyleBackColor = false;
            this.button_Set_Mark_Next.Click += new System.EventHandler(this.button_Set_Mark_Next_Click);
            // 
            // button_Set_Mark_Prev
            // 
            this.button_Set_Mark_Prev.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Mark_Prev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Mark_Prev.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Set_Mark_Prev.ForeColor = System.Drawing.Color.White;
            this.button_Set_Mark_Prev.Location = new System.Drawing.Point(250, 656);
            this.button_Set_Mark_Prev.Name = "button_Set_Mark_Prev";
            this.button_Set_Mark_Prev.Size = new System.Drawing.Size(56, 34);
            this.button_Set_Mark_Prev.TabIndex = 100;
            this.button_Set_Mark_Prev.Text = "◀";
            this.button_Set_Mark_Prev.UseVisualStyleBackColor = false;
            this.button_Set_Mark_Prev.Click += new System.EventHandler(this.button_Set_Mark_Prev_Click);
            // 
            // label_SetTest_Manual_Image_Load
            // 
            this.label_SetTest_Manual_Image_Load.BackColor = System.Drawing.Color.Tan;
            this.label_SetTest_Manual_Image_Load.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_SetTest_Manual_Image_Load.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Manual_Image_Load.Location = new System.Drawing.Point(565, 653);
            this.label_SetTest_Manual_Image_Load.Name = "label_SetTest_Manual_Image_Load";
            this.label_SetTest_Manual_Image_Load.Size = new System.Drawing.Size(98, 38);
            this.label_SetTest_Manual_Image_Load.TabIndex = 99;
            this.label_SetTest_Manual_Image_Load.Text = "IMAGE LOAD";
            this.label_SetTest_Manual_Image_Load.UseVisualStyleBackColor = false;
            this.label_SetTest_Manual_Image_Load.Click += new System.EventHandler(this.label_SetTest_Manual_Image_Load_Click);
            // 
            // label_SetTest_Manual_Image_Save
            // 
            this.label_SetTest_Manual_Image_Save.BackColor = System.Drawing.Color.Tan;
            this.label_SetTest_Manual_Image_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_SetTest_Manual_Image_Save.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Manual_Image_Save.Location = new System.Drawing.Point(565, 616);
            this.label_SetTest_Manual_Image_Save.Name = "label_SetTest_Manual_Image_Save";
            this.label_SetTest_Manual_Image_Save.Size = new System.Drawing.Size(98, 38);
            this.label_SetTest_Manual_Image_Save.TabIndex = 98;
            this.label_SetTest_Manual_Image_Save.Text = "IMAGE SAVE";
            this.label_SetTest_Manual_Image_Save.UseVisualStyleBackColor = false;
            this.label_SetTest_Manual_Image_Save.Click += new System.EventHandler(this.label_SetTest_Manual_Image_Save_Click);
            // 
            // label_SetTest_Manual_Mark_Find
            // 
            this.label_SetTest_Manual_Mark_Find.BackColor = System.Drawing.Color.Tan;
            this.label_SetTest_Manual_Mark_Find.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_SetTest_Manual_Mark_Find.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Manual_Mark_Find.Location = new System.Drawing.Point(565, 423);
            this.label_SetTest_Manual_Mark_Find.Name = "label_SetTest_Manual_Mark_Find";
            this.label_SetTest_Manual_Mark_Find.Size = new System.Drawing.Size(98, 75);
            this.label_SetTest_Manual_Mark_Find.TabIndex = 97;
            this.label_SetTest_Manual_Mark_Find.Text = "MARK FIND";
            this.label_SetTest_Manual_Mark_Find.UseVisualStyleBackColor = false;
            this.label_SetTest_Manual_Mark_Find.Click += new System.EventHandler(this.label_SetTest_Manual_Mark_Find_Click);
            // 
            // label_SetTest_Manual_Mark_View
            // 
            this.label_SetTest_Manual_Mark_View.BackColor = System.Drawing.Color.Tan;
            this.label_SetTest_Manual_Mark_View.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_SetTest_Manual_Mark_View.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Manual_Mark_View.Location = new System.Drawing.Point(565, 541);
            this.label_SetTest_Manual_Mark_View.Name = "label_SetTest_Manual_Mark_View";
            this.label_SetTest_Manual_Mark_View.Size = new System.Drawing.Size(98, 39);
            this.label_SetTest_Manual_Mark_View.TabIndex = 96;
            this.label_SetTest_Manual_Mark_View.Text = "VIEW";
            this.label_SetTest_Manual_Mark_View.UseVisualStyleBackColor = false;
            this.label_SetTest_Manual_Mark_View.Click += new System.EventHandler(this.label_SetTest_Manual_Mark_View_Click);
            // 
            // label_SetTest_Manual_Mark_Regist
            // 
            this.label_SetTest_Manual_Mark_Regist.BackColor = System.Drawing.Color.Tan;
            this.label_SetTest_Manual_Mark_Regist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_SetTest_Manual_Mark_Regist.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Manual_Mark_Regist.Location = new System.Drawing.Point(565, 497);
            this.label_SetTest_Manual_Mark_Regist.Name = "label_SetTest_Manual_Mark_Regist";
            this.label_SetTest_Manual_Mark_Regist.Size = new System.Drawing.Size(98, 45);
            this.label_SetTest_Manual_Mark_Regist.TabIndex = 95;
            this.label_SetTest_Manual_Mark_Regist.Text = "REGIST";
            this.label_SetTest_Manual_Mark_Regist.UseVisualStyleBackColor = false;
            this.label_SetTest_Manual_Mark_Regist.Click += new System.EventHandler(this.label_SetTest_Manual_Mark_Regist_Click);
            // 
            // panel_Mark
            // 
            this.panel_Mark.BackColor = System.Drawing.Color.Gray;
            this.panel_Mark.Location = new System.Drawing.Point(248, 423);
            this.panel_Mark.Name = "panel_Mark";
            this.panel_Mark.Size = new System.Drawing.Size(316, 231);
            this.panel_Mark.TabIndex = 94;
            // 
            // label_SetTest_Manual_Mark_Image
            // 
            this.label_SetTest_Manual_Mark_Image.BackColor = System.Drawing.SystemColors.Window;
            this.label_SetTest_Manual_Mark_Image.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_SetTest_Manual_Mark_Image.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_SetTest_Manual_Mark_Image.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Manual_Mark_Image.ForeColor = System.Drawing.Color.Black;
            this.label_SetTest_Manual_Mark_Image.Location = new System.Drawing.Point(248, 393);
            this.label_SetTest_Manual_Mark_Image.Name = "label_SetTest_Manual_Mark_Image";
            this.label_SetTest_Manual_Mark_Image.Size = new System.Drawing.Size(415, 29);
            this.label_SetTest_Manual_Mark_Image.TabIndex = 93;
            this.label_SetTest_Manual_Mark_Image.Text = "Mark Image";
            this.label_SetTest_Manual_Mark_Image.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Set_Height_Test
            // 
            this.button_Set_Height_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Height_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Height_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Height_Test.Location = new System.Drawing.Point(349, 246);
            this.button_Set_Height_Test.Name = "button_Set_Height_Test";
            this.button_Set_Height_Test.Size = new System.Drawing.Size(180, 70);
            this.button_Set_Height_Test.TabIndex = 112;
            this.button_Set_Height_Test.Text = "HEIGHT TEST";
            this.button_Set_Height_Test.UseVisualStyleBackColor = false;
            this.button_Set_Height_Test.Click += new System.EventHandler(this.button_Set_Height_Test_Click);
            // 
            // button_Set_Cone_Test
            // 
            this.button_Set_Cone_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Cone_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Cone_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Cone_Test.Location = new System.Drawing.Point(349, 176);
            this.button_Set_Cone_Test.Name = "button_Set_Cone_Test";
            this.button_Set_Cone_Test.Size = new System.Drawing.Size(180, 70);
            this.button_Set_Cone_Test.TabIndex = 111;
            this.button_Set_Cone_Test.Text = "CONE TEST";
            this.button_Set_Cone_Test.UseVisualStyleBackColor = false;
            this.button_Set_Cone_Test.Click += new System.EventHandler(this.button_Set_Cone_Test_Click);
            // 
            // button_Set_Oring_Test
            // 
            this.button_Set_Oring_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Oring_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Oring_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Oring_Test.Location = new System.Drawing.Point(349, 106);
            this.button_Set_Oring_Test.Name = "button_Set_Oring_Test";
            this.button_Set_Oring_Test.Size = new System.Drawing.Size(180, 70);
            this.button_Set_Oring_Test.TabIndex = 110;
            this.button_Set_Oring_Test.Text = "ORING TEST";
            this.button_Set_Oring_Test.UseVisualStyleBackColor = false;
            this.button_Set_Oring_Test.Click += new System.EventHandler(this.button_Set_Oring_Test_Click);
            // 
            // button_Set_Dent_Test
            // 
            this.button_Set_Dent_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Dent_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Dent_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Dent_Test.Location = new System.Drawing.Point(29, 246);
            this.button_Set_Dent_Test.Name = "button_Set_Dent_Test";
            this.button_Set_Dent_Test.Size = new System.Drawing.Size(180, 70);
            this.button_Set_Dent_Test.TabIndex = 109;
            this.button_Set_Dent_Test.Text = "DENT TEST";
            this.button_Set_Dent_Test.UseVisualStyleBackColor = false;
            this.button_Set_Dent_Test.Click += new System.EventHandler(this.button_Set_Dent_Test_Click);
            // 
            // button_Set_Gasket_Test
            // 
            this.button_Set_Gasket_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Gasket_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Gasket_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Gasket_Test.Location = new System.Drawing.Point(29, 176);
            this.button_Set_Gasket_Test.Name = "button_Set_Gasket_Test";
            this.button_Set_Gasket_Test.Size = new System.Drawing.Size(180, 70);
            this.button_Set_Gasket_Test.TabIndex = 108;
            this.button_Set_Gasket_Test.Text = "GASKET TEST";
            this.button_Set_Gasket_Test.UseVisualStyleBackColor = false;
            this.button_Set_Gasket_Test.Click += new System.EventHandler(this.button_Set_Gasket_Test_Click);
            // 
            // button_Set_Housing_Test
            // 
            this.button_Set_Housing_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Housing_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Housing_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Housing_Test.Location = new System.Drawing.Point(29, 106);
            this.button_Set_Housing_Test.Name = "button_Set_Housing_Test";
            this.button_Set_Housing_Test.Size = new System.Drawing.Size(180, 70);
            this.button_Set_Housing_Test.TabIndex = 107;
            this.button_Set_Housing_Test.Text = "HOUSING TEST";
            this.button_Set_Housing_Test.UseVisualStyleBackColor = false;
            this.button_Set_Housing_Test.Click += new System.EventHandler(this.button_Set_Housing_Test_Click);
            // 
            // button_Set_Key_Test
            // 
            this.button_Set_Key_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Set_Key_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Set_Key_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Set_Key_Test.Location = new System.Drawing.Point(29, 316);
            this.button_Set_Key_Test.Name = "button_Set_Key_Test";
            this.button_Set_Key_Test.Size = new System.Drawing.Size(180, 70);
            this.button_Set_Key_Test.TabIndex = 106;
            this.button_Set_Key_Test.Text = "KEY TEST";
            this.button_Set_Key_Test.UseVisualStyleBackColor = false;
            this.button_Set_Key_Test.Click += new System.EventHandler(this.button_Set_Key_Test_Click);
            // 
            // label_SetTest_Manual_Side_Test
            // 
            this.label_SetTest_Manual_Side_Test.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label_SetTest_Manual_Side_Test.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_SetTest_Manual_Side_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_SetTest_Manual_Side_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Manual_Side_Test.ForeColor = System.Drawing.Color.Black;
            this.label_SetTest_Manual_Side_Test.Location = new System.Drawing.Point(349, 68);
            this.label_SetTest_Manual_Side_Test.Name = "label_SetTest_Manual_Side_Test";
            this.label_SetTest_Manual_Side_Test.Size = new System.Drawing.Size(180, 38);
            this.label_SetTest_Manual_Side_Test.TabIndex = 105;
            this.label_SetTest_Manual_Side_Test.Text = "Side Cam Manual Test";
            this.label_SetTest_Manual_Side_Test.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_SetTest_Manual_Top_Test
            // 
            this.label_SetTest_Manual_Top_Test.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label_SetTest_Manual_Top_Test.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_SetTest_Manual_Top_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_SetTest_Manual_Top_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_SetTest_Manual_Top_Test.ForeColor = System.Drawing.Color.Black;
            this.label_SetTest_Manual_Top_Test.Location = new System.Drawing.Point(29, 68);
            this.label_SetTest_Manual_Top_Test.Name = "label_SetTest_Manual_Top_Test";
            this.label_SetTest_Manual_Top_Test.Size = new System.Drawing.Size(180, 38);
            this.label_SetTest_Manual_Top_Test.TabIndex = 104;
            this.label_SetTest_Manual_Top_Test.Text = "Top Cam Manual Test";
            this.label_SetTest_Manual_Top_Test.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Pogo_Find_Test
            // 
            this.button_Pogo_Find_Test.BackColor = System.Drawing.Color.Tan;
            this.button_Pogo_Find_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Pogo_Find_Test.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Pogo_Find_Test.Location = new System.Drawing.Point(29, 22);
            this.button_Pogo_Find_Test.Name = "button_Pogo_Find_Test";
            this.button_Pogo_Find_Test.Size = new System.Drawing.Size(180, 39);
            this.button_Pogo_Find_Test.TabIndex = 114;
            this.button_Pogo_Find_Test.Text = "FIND CENTER TEST";
            this.button_Pogo_Find_Test.UseVisualStyleBackColor = false;
            this.button_Pogo_Find_Test.Click += new System.EventHandler(this.button_Pogo_Find_Test_Click);
            // 
            // ManualTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGreen;
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
            this.Controls.Add(this.label_SetTest_Manual_Mark_Roi_Save);
            this.Controls.Add(this.label_Set_Mark_Model);
            this.Controls.Add(this.button_Set_Mark_Next);
            this.Controls.Add(this.button_Set_Mark_Prev);
            this.Controls.Add(this.label_SetTest_Manual_Image_Load);
            this.Controls.Add(this.label_SetTest_Manual_Image_Save);
            this.Controls.Add(this.label_SetTest_Manual_Mark_Find);
            this.Controls.Add(this.label_SetTest_Manual_Mark_View);
            this.Controls.Add(this.label_SetTest_Manual_Mark_Regist);
            this.Controls.Add(this.panel_Mark);
            this.Controls.Add(this.label_SetTest_Manual_Mark_Image);
            this.Name = "ManualTest";
            this.Size = new System.Drawing.Size(700, 720);
            this.VisibleChanged += new System.EventHandler(this.ManualTest_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button label_SetTest_Manual_Mark_Roi_Save;
        private System.Windows.Forms.Label label_Set_Mark_Model;
        private System.Windows.Forms.Button button_Set_Mark_Next;
        private System.Windows.Forms.Button button_Set_Mark_Prev;
        private System.Windows.Forms.Button label_SetTest_Manual_Image_Load;
        private System.Windows.Forms.Button label_SetTest_Manual_Image_Save;
        private System.Windows.Forms.Button label_SetTest_Manual_Mark_Find;
        private System.Windows.Forms.Button label_SetTest_Manual_Mark_View;
        private System.Windows.Forms.Button label_SetTest_Manual_Mark_Regist;
        public System.Windows.Forms.Panel panel_Mark;
        public System.Windows.Forms.Label label_SetTest_Manual_Mark_Image;
        private System.Windows.Forms.Button button_Set_Height_Test;
        private System.Windows.Forms.Button button_Set_Cone_Test;
        private System.Windows.Forms.Button button_Set_Oring_Test;
        private System.Windows.Forms.Button button_Set_Dent_Test;
        private System.Windows.Forms.Button button_Set_Gasket_Test;
        private System.Windows.Forms.Button button_Set_Housing_Test;
        private System.Windows.Forms.Button button_Set_Key_Test;
        public System.Windows.Forms.Label label_SetTest_Manual_Side_Test;
        public System.Windows.Forms.Label label_SetTest_Manual_Top_Test;
        private System.Windows.Forms.Button button_Pogo_Find_Test;
    }
}
