
namespace ZenTester.Dlg
{
    partial class FwSetControl
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
            this.label_FwSetTest_Title = new System.Windows.Forms.Label();
            this.button_FdSet_ConfRead = new System.Windows.Forms.Button();
            this.button_FdSet_Test = new System.Windows.Forms.Button();
            this.button_FdSet_JsonRead = new System.Windows.Forms.Button();
            this.button_FdSet_Read = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_FdSet_Path_Set = new System.Windows.Forms.Button();
            this.button_FdSet_Path_Title = new System.Windows.Forms.Label();
            this.button_FdSet_Path_Val = new System.Windows.Forms.Label();
            this.button_FdSet_Exe_Title = new System.Windows.Forms.Label();
            this.button_FdSet_Exe_Val = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_FwSetTest_Title
            // 
            this.label_FwSetTest_Title.BackColor = System.Drawing.Color.Black;
            this.label_FwSetTest_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_FwSetTest_Title.ForeColor = System.Drawing.Color.White;
            this.label_FwSetTest_Title.Location = new System.Drawing.Point(21, 21);
            this.label_FwSetTest_Title.Name = "label_FwSetTest_Title";
            this.label_FwSetTest_Title.Size = new System.Drawing.Size(813, 23);
            this.label_FwSetTest_Title.TabIndex = 3;
            this.label_FwSetTest_Title.Text = "Firmware down load Set";
            this.label_FwSetTest_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_FdSet_ConfRead
            // 
            this.button_FdSet_ConfRead.BackColor = System.Drawing.Color.Tan;
            this.button_FdSet_ConfRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_FdSet_ConfRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FdSet_ConfRead.Location = new System.Drawing.Point(25, 312);
            this.button_FdSet_ConfRead.Name = "button_FdSet_ConfRead";
            this.button_FdSet_ConfRead.Size = new System.Drawing.Size(180, 57);
            this.button_FdSet_ConfRead.TabIndex = 108;
            this.button_FdSet_ConfRead.Text = "Conf Read";
            this.button_FdSet_ConfRead.UseVisualStyleBackColor = false;
            this.button_FdSet_ConfRead.Click += new System.EventHandler(this.button_FdSet_ConfRead_Click);
            // 
            // button_FdSet_Test
            // 
            this.button_FdSet_Test.BackColor = System.Drawing.Color.Tan;
            this.button_FdSet_Test.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_FdSet_Test.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FdSet_Test.Location = new System.Drawing.Point(25, 186);
            this.button_FdSet_Test.Name = "button_FdSet_Test";
            this.button_FdSet_Test.Size = new System.Drawing.Size(180, 57);
            this.button_FdSet_Test.TabIndex = 109;
            this.button_FdSet_Test.Text = "fw Test";
            this.button_FdSet_Test.UseVisualStyleBackColor = false;
            this.button_FdSet_Test.Click += new System.EventHandler(this.button_FdSet_Test_Click);
            // 
            // button_FdSet_JsonRead
            // 
            this.button_FdSet_JsonRead.BackColor = System.Drawing.Color.Tan;
            this.button_FdSet_JsonRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_FdSet_JsonRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FdSet_JsonRead.Location = new System.Drawing.Point(25, 375);
            this.button_FdSet_JsonRead.Name = "button_FdSet_JsonRead";
            this.button_FdSet_JsonRead.Size = new System.Drawing.Size(180, 57);
            this.button_FdSet_JsonRead.TabIndex = 110;
            this.button_FdSet_JsonRead.Text = "Json Read";
            this.button_FdSet_JsonRead.UseVisualStyleBackColor = false;
            this.button_FdSet_JsonRead.Click += new System.EventHandler(this.button_FdSet_JsonRead_Click);
            // 
            // button_FdSet_Read
            // 
            this.button_FdSet_Read.BackColor = System.Drawing.Color.Tan;
            this.button_FdSet_Read.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_FdSet_Read.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FdSet_Read.Location = new System.Drawing.Point(25, 249);
            this.button_FdSet_Read.Name = "button_FdSet_Read";
            this.button_FdSet_Read.Size = new System.Drawing.Size(180, 57);
            this.button_FdSet_Read.TabIndex = 111;
            this.button_FdSet_Read.Text = "Read version/sensorid";
            this.button_FdSet_Read.UseVisualStyleBackColor = false;
            this.button_FdSet_Read.Click += new System.EventHandler(this.button_FdSet_Read_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.White;
            this.groupBox5.Controls.Add(this.button_FdSet_Exe_Val);
            this.groupBox5.Controls.Add(this.button_FdSet_Exe_Title);
            this.groupBox5.Controls.Add(this.button_FdSet_Path_Val);
            this.groupBox5.Controls.Add(this.button_FdSet_Path_Set);
            this.groupBox5.Controls.Add(this.button_FdSet_Path_Title);
            this.groupBox5.Location = new System.Drawing.Point(25, 52);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(809, 128);
            this.groupBox5.TabIndex = 112;
            this.groupBox5.TabStop = false;
            // 
            // button_FdSet_Path_Set
            // 
            this.button_FdSet_Path_Set.BackColor = System.Drawing.Color.Tan;
            this.button_FdSet_Path_Set.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_FdSet_Path_Set.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FdSet_Path_Set.ForeColor = System.Drawing.Color.White;
            this.button_FdSet_Path_Set.Location = new System.Drawing.Point(719, 84);
            this.button_FdSet_Path_Set.Name = "button_FdSet_Path_Set";
            this.button_FdSet_Path_Set.Size = new System.Drawing.Size(84, 36);
            this.button_FdSet_Path_Set.TabIndex = 31;
            this.button_FdSet_Path_Set.Text = "Select";
            this.button_FdSet_Path_Set.UseVisualStyleBackColor = false;
            this.button_FdSet_Path_Set.Click += new System.EventHandler(this.button_FdSet_Path_Set_Click);
            // 
            // button_FdSet_Path_Title
            // 
            this.button_FdSet_Path_Title.BackColor = System.Drawing.Color.Transparent;
            this.button_FdSet_Path_Title.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FdSet_Path_Title.ForeColor = System.Drawing.Color.Black;
            this.button_FdSet_Path_Title.Location = new System.Drawing.Point(11, 17);
            this.button_FdSet_Path_Title.Name = "button_FdSet_Path_Title";
            this.button_FdSet_Path_Title.Size = new System.Drawing.Size(128, 23);
            this.button_FdSet_Path_Title.TabIndex = 26;
            this.button_FdSet_Path_Title.Text = "Tesla exe Path";
            this.button_FdSet_Path_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_FdSet_Path_Val
            // 
            this.button_FdSet_Path_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.button_FdSet_Path_Val.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FdSet_Path_Val.Location = new System.Drawing.Point(11, 42);
            this.button_FdSet_Path_Val.Name = "button_FdSet_Path_Val";
            this.button_FdSet_Path_Val.Size = new System.Drawing.Size(792, 29);
            this.button_FdSet_Path_Val.TabIndex = 35;
            this.button_FdSet_Path_Val.Text = "D:\\EVMS\\TP\\ENV\\fwexe\\TeslaEXE\\Tesla_FW_exe\\Trinity_FW_Download_20250421_1111";
            this.button_FdSet_Path_Val.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_FdSet_Exe_Title
            // 
            this.button_FdSet_Exe_Title.BackColor = System.Drawing.Color.Transparent;
            this.button_FdSet_Exe_Title.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FdSet_Exe_Title.ForeColor = System.Drawing.Color.Black;
            this.button_FdSet_Exe_Title.Location = new System.Drawing.Point(11, 84);
            this.button_FdSet_Exe_Title.Name = "button_FdSet_Exe_Title";
            this.button_FdSet_Exe_Title.Size = new System.Drawing.Size(94, 23);
            this.button_FdSet_Exe_Title.TabIndex = 36;
            this.button_FdSet_Exe_Title.Text = "Tesla exe:";
            this.button_FdSet_Exe_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_FdSet_Exe_Val
            // 
            this.button_FdSet_Exe_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.button_FdSet_Exe_Val.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FdSet_Exe_Val.Location = new System.Drawing.Point(111, 84);
            this.button_FdSet_Exe_Val.Name = "button_FdSet_Exe_Val";
            this.button_FdSet_Exe_Val.Size = new System.Drawing.Size(286, 25);
            this.button_FdSet_Exe_Val.TabIndex = 37;
            this.button_FdSet_Exe_Val.Text = "cypress_cam_flashing.exe";
            this.button_FdSet_Exe_Val.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FwSetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.button_FdSet_Read);
            this.Controls.Add(this.button_FdSet_JsonRead);
            this.Controls.Add(this.button_FdSet_Test);
            this.Controls.Add(this.button_FdSet_ConfRead);
            this.Controls.Add(this.label_FwSetTest_Title);
            this.Name = "FwSetControl";
            this.Size = new System.Drawing.Size(1260, 568);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_FwSetTest_Title;
        private System.Windows.Forms.Button button_FdSet_ConfRead;
        private System.Windows.Forms.Button button_FdSet_Test;
        private System.Windows.Forms.Button button_FdSet_JsonRead;
        private System.Windows.Forms.Button button_FdSet_Read;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button_FdSet_Path_Set;
        private System.Windows.Forms.Label button_FdSet_Path_Title;
        private System.Windows.Forms.Label button_FdSet_Path_Val;
        private System.Windows.Forms.Label button_FdSet_Exe_Title;
        private System.Windows.Forms.Label button_FdSet_Exe_Val;
    }
}
