
namespace ZenTester.Dlg
{
    partial class WriteSetControl
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
            this.label_WriteSetTest_Title = new System.Windows.Forms.Label();
            this.button_WSet_ConfRead = new System.Windows.Forms.Button();
            this.button_WSet_Crc_Cal = new System.Windows.Forms.Button();
            this.button_WSet_Run = new System.Windows.Forms.Button();
            this.button_WSet_Dat_Create = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button_WSet_Exe_Val2 = new System.Windows.Forms.Label();
            this.button_WSet_Exe_Title2 = new System.Windows.Forms.Label();
            this.button_WSet_Exe_Val1 = new System.Windows.Forms.Label();
            this.button_WSet_Exe_Title1 = new System.Windows.Forms.Label();
            this.button_WSet_Path_Val = new System.Windows.Forms.Label();
            this.button_WSet_Path_Set = new System.Windows.Forms.Button();
            this.button_WSet_Path_Title = new System.Windows.Forms.Label();
            this.button_WSet_Exe_Val3 = new System.Windows.Forms.Label();
            this.button_WSet_Exe_Title3 = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_WriteSetTest_Title
            // 
            this.label_WriteSetTest_Title.BackColor = System.Drawing.Color.Black;
            this.label_WriteSetTest_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_WriteSetTest_Title.ForeColor = System.Drawing.Color.White;
            this.label_WriteSetTest_Title.Location = new System.Drawing.Point(21, 21);
            this.label_WriteSetTest_Title.Name = "label_WriteSetTest_Title";
            this.label_WriteSetTest_Title.Size = new System.Drawing.Size(813, 23);
            this.label_WriteSetTest_Title.TabIndex = 3;
            this.label_WriteSetTest_Title.Text = "Write load Set";
            this.label_WriteSetTest_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_WSet_ConfRead
            // 
            this.button_WSet_ConfRead.BackColor = System.Drawing.Color.Tan;
            this.button_WSet_ConfRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_WSet_ConfRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_ConfRead.Location = new System.Drawing.Point(25, 474);
            this.button_WSet_ConfRead.Name = "button_WSet_ConfRead";
            this.button_WSet_ConfRead.Size = new System.Drawing.Size(180, 57);
            this.button_WSet_ConfRead.TabIndex = 108;
            this.button_WSet_ConfRead.Text = "Conf Read";
            this.button_WSet_ConfRead.UseVisualStyleBackColor = false;
            this.button_WSet_ConfRead.Click += new System.EventHandler(this.button_FdSet_ConfRead_Click);
            // 
            // button_WSet_Crc_Cal
            // 
            this.button_WSet_Crc_Cal.BackColor = System.Drawing.Color.Tan;
            this.button_WSet_Crc_Cal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_WSet_Crc_Cal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Crc_Cal.Location = new System.Drawing.Point(25, 411);
            this.button_WSet_Crc_Cal.Name = "button_WSet_Crc_Cal";
            this.button_WSet_Crc_Cal.Size = new System.Drawing.Size(180, 57);
            this.button_WSet_Crc_Cal.TabIndex = 109;
            this.button_WSet_Crc_Cal.Text = "W Crc Cal";
            this.button_WSet_Crc_Cal.UseVisualStyleBackColor = false;
            this.button_WSet_Crc_Cal.Click += new System.EventHandler(this.button_WSet_Crc_Cal_Click);
            // 
            // button_WSet_Run
            // 
            this.button_WSet_Run.BackColor = System.Drawing.Color.Tan;
            this.button_WSet_Run.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_WSet_Run.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Run.Location = new System.Drawing.Point(25, 348);
            this.button_WSet_Run.Name = "button_WSet_Run";
            this.button_WSet_Run.Size = new System.Drawing.Size(180, 57);
            this.button_WSet_Run.TabIndex = 110;
            this.button_WSet_Run.Text = "Manual Write";
            this.button_WSet_Run.UseVisualStyleBackColor = false;
            this.button_WSet_Run.Click += new System.EventHandler(this.button_WSet_Run_Click);
            // 
            // button_WSet_Dat_Create
            // 
            this.button_WSet_Dat_Create.BackColor = System.Drawing.Color.Tan;
            this.button_WSet_Dat_Create.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_WSet_Dat_Create.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Dat_Create.Location = new System.Drawing.Point(25, 285);
            this.button_WSet_Dat_Create.Name = "button_WSet_Dat_Create";
            this.button_WSet_Dat_Create.Size = new System.Drawing.Size(180, 57);
            this.button_WSet_Dat_Create.TabIndex = 111;
            this.button_WSet_Dat_Create.Text = "Dat Create";
            this.button_WSet_Dat_Create.UseVisualStyleBackColor = false;
            this.button_WSet_Dat_Create.Click += new System.EventHandler(this.button_WSet_Dat_Create_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.Color.White;
            this.groupBox5.Controls.Add(this.button_WSet_Exe_Val3);
            this.groupBox5.Controls.Add(this.button_WSet_Exe_Title3);
            this.groupBox5.Controls.Add(this.button_WSet_Exe_Val2);
            this.groupBox5.Controls.Add(this.button_WSet_Exe_Title2);
            this.groupBox5.Controls.Add(this.button_WSet_Exe_Val1);
            this.groupBox5.Controls.Add(this.button_WSet_Exe_Title1);
            this.groupBox5.Controls.Add(this.button_WSet_Path_Val);
            this.groupBox5.Controls.Add(this.button_WSet_Path_Set);
            this.groupBox5.Controls.Add(this.button_WSet_Path_Title);
            this.groupBox5.Location = new System.Drawing.Point(25, 66);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(809, 189);
            this.groupBox5.TabIndex = 114;
            this.groupBox5.TabStop = false;
            // 
            // button_WSet_Exe_Val2
            // 
            this.button_WSet_Exe_Val2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.button_WSet_Exe_Val2.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Exe_Val2.Location = new System.Drawing.Point(111, 111);
            this.button_WSet_Exe_Val2.Name = "button_WSet_Exe_Val2";
            this.button_WSet_Exe_Val2.Size = new System.Drawing.Size(322, 25);
            this.button_WSet_Exe_Val2.TabIndex = 39;
            this.button_WSet_Exe_Val2.Text = "cam_eeprom_flasher.exe";
            this.button_WSet_Exe_Val2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_WSet_Exe_Title2
            // 
            this.button_WSet_Exe_Title2.BackColor = System.Drawing.Color.Transparent;
            this.button_WSet_Exe_Title2.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Exe_Title2.ForeColor = System.Drawing.Color.Black;
            this.button_WSet_Exe_Title2.Location = new System.Drawing.Point(11, 111);
            this.button_WSet_Exe_Title2.Name = "button_WSet_Exe_Title2";
            this.button_WSet_Exe_Title2.Size = new System.Drawing.Size(94, 23);
            this.button_WSet_Exe_Title2.TabIndex = 38;
            this.button_WSet_Exe_Title2.Text = "Trinity exe:";
            this.button_WSet_Exe_Title2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_WSet_Exe_Val1
            // 
            this.button_WSet_Exe_Val1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.button_WSet_Exe_Val1.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Exe_Val1.Location = new System.Drawing.Point(111, 84);
            this.button_WSet_Exe_Val1.Name = "button_WSet_Exe_Val1";
            this.button_WSet_Exe_Val1.Size = new System.Drawing.Size(322, 25);
            this.button_WSet_Exe_Val1.TabIndex = 37;
            this.button_WSet_Exe_Val1.Text = "ThunderEEPROMVerificationTool.exe";
            this.button_WSet_Exe_Val1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_WSet_Exe_Title1
            // 
            this.button_WSet_Exe_Title1.BackColor = System.Drawing.Color.Transparent;
            this.button_WSet_Exe_Title1.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Exe_Title1.ForeColor = System.Drawing.Color.Black;
            this.button_WSet_Exe_Title1.Location = new System.Drawing.Point(11, 84);
            this.button_WSet_Exe_Title1.Name = "button_WSet_Exe_Title1";
            this.button_WSet_Exe_Title1.Size = new System.Drawing.Size(94, 23);
            this.button_WSet_Exe_Title1.TabIndex = 36;
            this.button_WSet_Exe_Title1.Text = "Tesla exe:";
            this.button_WSet_Exe_Title1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_WSet_Path_Val
            // 
            this.button_WSet_Path_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.button_WSet_Path_Val.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Path_Val.Location = new System.Drawing.Point(11, 42);
            this.button_WSet_Path_Val.Name = "button_WSet_Path_Val";
            this.button_WSet_Path_Val.Size = new System.Drawing.Size(792, 29);
            this.button_WSet_Path_Val.TabIndex = 35;
            this.button_WSet_Path_Val.Text = "D:\\EVMS\\TP\\ENV\\fwexe\\ThunderEEPROMCreationTool_250526_1111";
            this.button_WSet_Path_Val.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_WSet_Path_Set
            // 
            this.button_WSet_Path_Set.BackColor = System.Drawing.Color.Tan;
            this.button_WSet_Path_Set.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_WSet_Path_Set.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Path_Set.ForeColor = System.Drawing.Color.White;
            this.button_WSet_Path_Set.Location = new System.Drawing.Point(719, 139);
            this.button_WSet_Path_Set.Name = "button_WSet_Path_Set";
            this.button_WSet_Path_Set.Size = new System.Drawing.Size(84, 36);
            this.button_WSet_Path_Set.TabIndex = 31;
            this.button_WSet_Path_Set.Text = "Select";
            this.button_WSet_Path_Set.UseVisualStyleBackColor = false;
            this.button_WSet_Path_Set.Click += new System.EventHandler(this.button_WSet_Path_Set_Click);
            // 
            // button_WSet_Path_Title
            // 
            this.button_WSet_Path_Title.BackColor = System.Drawing.Color.Transparent;
            this.button_WSet_Path_Title.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Path_Title.ForeColor = System.Drawing.Color.Black;
            this.button_WSet_Path_Title.Location = new System.Drawing.Point(11, 17);
            this.button_WSet_Path_Title.Name = "button_WSet_Path_Title";
            this.button_WSet_Path_Title.Size = new System.Drawing.Size(128, 23);
            this.button_WSet_Path_Title.TabIndex = 26;
            this.button_WSet_Path_Title.Text = "Tesla exe Path";
            this.button_WSet_Path_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_WSet_Exe_Val3
            // 
            this.button_WSet_Exe_Val3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.button_WSet_Exe_Val3.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Exe_Val3.Location = new System.Drawing.Point(111, 139);
            this.button_WSet_Exe_Val3.Name = "button_WSet_Exe_Val3";
            this.button_WSet_Exe_Val3.Size = new System.Drawing.Size(322, 25);
            this.button_WSet_Exe_Val3.TabIndex = 41;
            this.button_WSet_Exe_Val3.Text = "ti_cam_eeprom_flasher.exe";
            this.button_WSet_Exe_Val3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_WSet_Exe_Title3
            // 
            this.button_WSet_Exe_Title3.BackColor = System.Drawing.Color.Transparent;
            this.button_WSet_Exe_Title3.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_Exe_Title3.ForeColor = System.Drawing.Color.Black;
            this.button_WSet_Exe_Title3.Location = new System.Drawing.Point(11, 139);
            this.button_WSet_Exe_Title3.Name = "button_WSet_Exe_Title3";
            this.button_WSet_Exe_Title3.Size = new System.Drawing.Size(94, 23);
            this.button_WSet_Exe_Title3.TabIndex = 40;
            this.button_WSet_Exe_Title3.Text = "Opal exe:";
            this.button_WSet_Exe_Title3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WriteSetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.button_WSet_Dat_Create);
            this.Controls.Add(this.button_WSet_Run);
            this.Controls.Add(this.button_WSet_Crc_Cal);
            this.Controls.Add(this.button_WSet_ConfRead);
            this.Controls.Add(this.label_WriteSetTest_Title);
            this.Name = "WriteSetControl";
            this.Size = new System.Drawing.Size(1100, 568);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_WriteSetTest_Title;
        private System.Windows.Forms.Button button_WSet_ConfRead;
        private System.Windows.Forms.Button button_WSet_Crc_Cal;
        private System.Windows.Forms.Button button_WSet_Run;
        private System.Windows.Forms.Button button_WSet_Dat_Create;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label button_WSet_Exe_Val1;
        private System.Windows.Forms.Label button_WSet_Exe_Title1;
        private System.Windows.Forms.Label button_WSet_Path_Val;
        private System.Windows.Forms.Button button_WSet_Path_Set;
        private System.Windows.Forms.Label button_WSet_Path_Title;
        private System.Windows.Forms.Label button_WSet_Exe_Val2;
        private System.Windows.Forms.Label button_WSet_Exe_Title2;
        private System.Windows.Forms.Label button_WSet_Exe_Val3;
        private System.Windows.Forms.Label button_WSet_Exe_Title3;
    }
}
