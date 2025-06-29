
namespace ZenTester.Dlg
{
    partial class VerifySetControl
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
            this.label_VerifySetTest_Title = new System.Windows.Forms.Label();
            this.button_VSet_ConfRead = new System.Windows.Forms.Button();
            this.button_VSet_Crc_Cal = new System.Windows.Forms.Button();
            this.button_VSet_Run = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_VSet_Exe_Val = new System.Windows.Forms.Label();
            this.button_VSet_Exe_Title = new System.Windows.Forms.Label();
            this.button_VSet_Path_Val = new System.Windows.Forms.Label();
            this.button_VSet_Path_Set = new System.Windows.Forms.Button();
            this.button_VSet_Path_Title = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_VerifySetTest_Title
            // 
            this.label_VerifySetTest_Title.BackColor = System.Drawing.Color.Black;
            this.label_VerifySetTest_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_VerifySetTest_Title.ForeColor = System.Drawing.Color.White;
            this.label_VerifySetTest_Title.Location = new System.Drawing.Point(21, 21);
            this.label_VerifySetTest_Title.Name = "label_VerifySetTest_Title";
            this.label_VerifySetTest_Title.Size = new System.Drawing.Size(813, 23);
            this.label_VerifySetTest_Title.TabIndex = 3;
            this.label_VerifySetTest_Title.Text = "Verify load Set";
            this.label_VerifySetTest_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_VSet_ConfRead
            // 
            this.button_VSet_ConfRead.BackColor = System.Drawing.Color.Tan;
            this.button_VSet_ConfRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_VSet_ConfRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_VSet_ConfRead.Location = new System.Drawing.Point(41, 362);
            this.button_VSet_ConfRead.Name = "button_VSet_ConfRead";
            this.button_VSet_ConfRead.Size = new System.Drawing.Size(180, 57);
            this.button_VSet_ConfRead.TabIndex = 108;
            this.button_VSet_ConfRead.Text = "Conf Read";
            this.button_VSet_ConfRead.UseVisualStyleBackColor = false;
            this.button_VSet_ConfRead.Click += new System.EventHandler(this.button_FdSet_ConfRead_Click);
            // 
            // button_VSet_Crc_Cal
            // 
            this.button_VSet_Crc_Cal.BackColor = System.Drawing.Color.Tan;
            this.button_VSet_Crc_Cal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_VSet_Crc_Cal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_VSet_Crc_Cal.Location = new System.Drawing.Point(41, 299);
            this.button_VSet_Crc_Cal.Name = "button_VSet_Crc_Cal";
            this.button_VSet_Crc_Cal.Size = new System.Drawing.Size(180, 57);
            this.button_VSet_Crc_Cal.TabIndex = 109;
            this.button_VSet_Crc_Cal.Text = "V Crc Cal";
            this.button_VSet_Crc_Cal.UseVisualStyleBackColor = false;
            this.button_VSet_Crc_Cal.Click += new System.EventHandler(this.button_VSet_Crc_Cal_Click);
            // 
            // button_VSet_Run
            // 
            this.button_VSet_Run.BackColor = System.Drawing.Color.Tan;
            this.button_VSet_Run.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_VSet_Run.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_VSet_Run.Location = new System.Drawing.Point(41, 236);
            this.button_VSet_Run.Name = "button_VSet_Run";
            this.button_VSet_Run.Size = new System.Drawing.Size(180, 57);
            this.button_VSet_Run.TabIndex = 110;
            this.button_VSet_Run.Text = "Manual Verify";
            this.button_VSet_Run.UseVisualStyleBackColor = false;
            this.button_VSet_Run.Click += new System.EventHandler(this.button_VSet_Run_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.White;
            this.groupBox5.Controls.Add(this.button_VSet_Exe_Val);
            this.groupBox5.Controls.Add(this.button_VSet_Exe_Title);
            this.groupBox5.Controls.Add(this.button_VSet_Path_Val);
            this.groupBox5.Controls.Add(this.button_VSet_Path_Set);
            this.groupBox5.Controls.Add(this.button_VSet_Path_Title);
            this.groupBox5.Location = new System.Drawing.Point(25, 68);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(809, 128);
            this.groupBox5.TabIndex = 113;
            this.groupBox5.TabStop = false;
            // 
            // button_VSet_Exe_Val
            // 
            this.button_VSet_Exe_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.button_VSet_Exe_Val.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_VSet_Exe_Val.Location = new System.Drawing.Point(111, 84);
            this.button_VSet_Exe_Val.Name = "button_VSet_Exe_Val";
            this.button_VSet_Exe_Val.Size = new System.Drawing.Size(322, 25);
            this.button_VSet_Exe_Val.TabIndex = 37;
            this.button_VSet_Exe_Val.Text = "ThunderEEPROMVerificationTool.exe";
            this.button_VSet_Exe_Val.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_VSet_Exe_Title
            // 
            this.button_VSet_Exe_Title.BackColor = System.Drawing.Color.Transparent;
            this.button_VSet_Exe_Title.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_VSet_Exe_Title.ForeColor = System.Drawing.Color.Black;
            this.button_VSet_Exe_Title.Location = new System.Drawing.Point(11, 84);
            this.button_VSet_Exe_Title.Name = "button_VSet_Exe_Title";
            this.button_VSet_Exe_Title.Size = new System.Drawing.Size(94, 23);
            this.button_VSet_Exe_Title.TabIndex = 36;
            this.button_VSet_Exe_Title.Text = "Tesla exe:";
            this.button_VSet_Exe_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_VSet_Path_Val
            // 
            this.button_VSet_Path_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.button_VSet_Path_Val.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_VSet_Path_Val.Location = new System.Drawing.Point(11, 42);
            this.button_VSet_Path_Val.Name = "button_VSet_Path_Val";
            this.button_VSet_Path_Val.Size = new System.Drawing.Size(792, 29);
            this.button_VSet_Path_Val.TabIndex = 35;
            this.button_VSet_Path_Val.Text = "D:\\EVMS\\TP\\ENV\\fwexe\\ThunderEEPROMVerificationTool_250526_1111";
            this.button_VSet_Path_Val.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_VSet_Path_Set
            // 
            this.button_VSet_Path_Set.BackColor = System.Drawing.Color.Tan;
            this.button_VSet_Path_Set.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_VSet_Path_Set.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_VSet_Path_Set.ForeColor = System.Drawing.Color.White;
            this.button_VSet_Path_Set.Location = new System.Drawing.Point(719, 84);
            this.button_VSet_Path_Set.Name = "button_VSet_Path_Set";
            this.button_VSet_Path_Set.Size = new System.Drawing.Size(84, 36);
            this.button_VSet_Path_Set.TabIndex = 31;
            this.button_VSet_Path_Set.Text = "Select";
            this.button_VSet_Path_Set.UseVisualStyleBackColor = false;
            this.button_VSet_Path_Set.Click += new System.EventHandler(this.button_VSet_Path_Set_Click);
            // 
            // button_VSet_Path_Title
            // 
            this.button_VSet_Path_Title.BackColor = System.Drawing.Color.Transparent;
            this.button_VSet_Path_Title.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_VSet_Path_Title.ForeColor = System.Drawing.Color.Black;
            this.button_VSet_Path_Title.Location = new System.Drawing.Point(11, 17);
            this.button_VSet_Path_Title.Name = "button_VSet_Path_Title";
            this.button_VSet_Path_Title.Size = new System.Drawing.Size(128, 23);
            this.button_VSet_Path_Title.TabIndex = 26;
            this.button_VSet_Path_Title.Text = "Tesla exe Path";
            this.button_VSet_Path_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VerifySetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.button_VSet_Run);
            this.Controls.Add(this.button_VSet_Crc_Cal);
            this.Controls.Add(this.button_VSet_ConfRead);
            this.Controls.Add(this.label_VerifySetTest_Title);
            this.Name = "VerifySetControl";
            this.Size = new System.Drawing.Size(1100, 568);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_VerifySetTest_Title;
        private System.Windows.Forms.Button button_VSet_ConfRead;
        private System.Windows.Forms.Button button_VSet_Crc_Cal;
        private System.Windows.Forms.Button button_VSet_Run;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label button_VSet_Exe_Val;
        private System.Windows.Forms.Label button_VSet_Exe_Title;
        private System.Windows.Forms.Label button_VSet_Path_Val;
        private System.Windows.Forms.Button button_VSet_Path_Set;
        private System.Windows.Forms.Label button_VSet_Path_Title;
    }
}
