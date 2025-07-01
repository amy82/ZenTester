namespace ZenTester.Dlg
{
    partial class ConfigControl
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
            this.ManualTitleLabel = new System.Windows.Forms.Label();
            this.ManualPanel = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label_Config_Tray_GapY_Val = new System.Windows.Forms.Label();
            this.label_Config_Tray_GapX_Val = new System.Windows.Forms.Label();
            this.label_Config_Tray_GapY = new System.Windows.Forms.Label();
            this.label_Config_Tray_GapX = new System.Windows.Forms.Label();
            this.ComboBox_Language = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label_CsvScanMax = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_PinCountMax = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BTN_CONFIG_SAVE = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_Secsgem_Ip4 = new System.Windows.Forms.Label();
            this.label_Secsgem_Ip1 = new System.Windows.Forms.Label();
            this.label_Secsgem_Port = new System.Windows.Forms.Label();
            this.label_Config_Secsgem_Port = new System.Windows.Forms.Label();
            this.label_Config_Secsgem_Ip = new System.Windows.Forms.Label();
            this.label_Secsgem_Ip3 = new System.Windows.Forms.Label();
            this.label_Secsgem_Ip2 = new System.Windows.Forms.Label();
            this.label_Handler_Ip4 = new System.Windows.Forms.Label();
            this.label_Handler_Ip1 = new System.Windows.Forms.Label();
            this.label_Handler_Port = new System.Windows.Forms.Label();
            this.label_Config_Handler_Port = new System.Windows.Forms.Label();
            this.label_Config_Handler_Ip = new System.Windows.Forms.Label();
            this.label_Handler_Ip3 = new System.Windows.Forms.Label();
            this.label_Handler_Ip2 = new System.Windows.Forms.Label();
            this.poisonComboBox_Light_Port = new System.Windows.Forms.ComboBox();
            this.button_Bcr_DisConnect = new System.Windows.Forms.Button();
            this.button_Bcr_Connect = new System.Windows.Forms.Button();
            this.label_Config_Light = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.hopeCheckBox_ImageGrabUse = new ReaLTaiizor.Controls.HopeCheckBox();
            this.hopeCheckBox_PinCountUse = new ReaLTaiizor.Controls.HopeCheckBox();
            this.checkBox_BcrGo = new System.Windows.Forms.CheckBox();
            this.checkBox_IdleReportPass = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ManualPanel.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ManualTitleLabel
            // 
            this.ManualTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ManualTitleLabel.Location = new System.Drawing.Point(3, 0);
            this.ManualTitleLabel.Name = "ManualTitleLabel";
            this.ManualTitleLabel.Size = new System.Drawing.Size(250, 50);
            this.ManualTitleLabel.TabIndex = 2;
            this.ManualTitleLabel.Text = "| CONFIG";
            this.ManualTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ManualPanel
            // 
            this.ManualPanel.Controls.Add(this.groupBox3);
            this.ManualPanel.Controls.Add(this.BTN_CONFIG_SAVE);
            this.ManualPanel.Controls.Add(this.groupBox1);
            this.ManualPanel.Controls.Add(this.groupBox2);
            this.ManualPanel.Location = new System.Drawing.Point(21, 97);
            this.ManualPanel.Name = "ManualPanel";
            this.ManualPanel.Size = new System.Drawing.Size(855, 708);
            this.ManualPanel.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.White;
            this.groupBox3.Controls.Add(this.label_Config_Tray_GapY_Val);
            this.groupBox3.Controls.Add(this.label_Config_Tray_GapX_Val);
            this.groupBox3.Controls.Add(this.label_Config_Tray_GapY);
            this.groupBox3.Controls.Add(this.label_Config_Tray_GapX);
            this.groupBox3.Controls.Add(this.ComboBox_Language);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label_CsvScanMax);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label_PinCountMax);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(22, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(298, 587);
            this.groupBox3.TabIndex = 46;
            this.groupBox3.TabStop = false;
            // 
            // label_Config_Tray_GapY_Val
            // 
            this.label_Config_Tray_GapY_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_GapY_Val.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_GapY_Val.Location = new System.Drawing.Point(184, 315);
            this.label_Config_Tray_GapY_Val.Name = "label_Config_Tray_GapY_Val";
            this.label_Config_Tray_GapY_Val.Size = new System.Drawing.Size(100, 29);
            this.label_Config_Tray_GapY_Val.TabIndex = 54;
            this.label_Config_Tray_GapY_Val.Text = "0.0";
            this.label_Config_Tray_GapY_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Config_Tray_GapY_Val.Click += new System.EventHandler(this.label_Config_Tray_GapY_Val_Click);
            // 
            // label_Config_Tray_GapX_Val
            // 
            this.label_Config_Tray_GapX_Val.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_GapX_Val.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_GapX_Val.Location = new System.Drawing.Point(184, 285);
            this.label_Config_Tray_GapX_Val.Name = "label_Config_Tray_GapX_Val";
            this.label_Config_Tray_GapX_Val.Size = new System.Drawing.Size(100, 29);
            this.label_Config_Tray_GapX_Val.TabIndex = 53;
            this.label_Config_Tray_GapX_Val.Text = "0.0";
            this.label_Config_Tray_GapX_Val.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Config_Tray_GapX_Val.Click += new System.EventHandler(this.label_Config_Tray_GapX_Val_Click);
            // 
            // label_Config_Tray_GapY
            // 
            this.label_Config_Tray_GapY.BackColor = System.Drawing.SystemColors.Window;
            this.label_Config_Tray_GapY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_GapY.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Config_Tray_GapY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_GapY.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Tray_GapY.Location = new System.Drawing.Point(17, 316);
            this.label_Config_Tray_GapY.Name = "label_Config_Tray_GapY";
            this.label_Config_Tray_GapY.Size = new System.Drawing.Size(161, 29);
            this.label_Config_Tray_GapY.TabIndex = 52;
            this.label_Config_Tray_GapY.Text = "Tray Gap Y";
            this.label_Config_Tray_GapY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Tray_GapX
            // 
            this.label_Config_Tray_GapX.BackColor = System.Drawing.SystemColors.Window;
            this.label_Config_Tray_GapX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Tray_GapX.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Config_Tray_GapX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Tray_GapX.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Tray_GapX.Location = new System.Drawing.Point(17, 285);
            this.label_Config_Tray_GapX.Name = "label_Config_Tray_GapX";
            this.label_Config_Tray_GapX.Size = new System.Drawing.Size(161, 29);
            this.label_Config_Tray_GapX.TabIndex = 51;
            this.label_Config_Tray_GapX.Text = "Tray Gap X";
            this.label_Config_Tray_GapX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ComboBox_Language
            // 
            this.ComboBox_Language.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ComboBox_Language.DropDownHeight = 120;
            this.ComboBox_Language.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ComboBox_Language.FormattingEnabled = true;
            this.ComboBox_Language.IntegralHeight = false;
            this.ComboBox_Language.ItemHeight = 20;
            this.ComboBox_Language.Location = new System.Drawing.Point(132, 111);
            this.ComboBox_Language.Name = "ComboBox_Language";
            this.ComboBox_Language.Size = new System.Drawing.Size(142, 28);
            this.ComboBox_Language.TabIndex = 50;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Window;
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(18, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 29);
            this.label5.TabIndex = 35;
            this.label5.Text = "Language";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_CsvScanMax
            // 
            this.label_CsvScanMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CsvScanMax.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_CsvScanMax.Location = new System.Drawing.Point(174, 391);
            this.label_CsvScanMax.Name = "label_CsvScanMax";
            this.label_CsvScanMax.Size = new System.Drawing.Size(100, 29);
            this.label_CsvScanMax.TabIndex = 34;
            this.label_CsvScanMax.Text = "0";
            this.label_CsvScanMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_CsvScanMax.Visible = false;
            this.label_CsvScanMax.Click += new System.EventHandler(this.label_CsvScanMax_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.Window;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(7, 391);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 29);
            this.label4.TabIndex = 33;
            this.label4.Text = "CSV 검색 기간 (month)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.Visible = false;
            // 
            // label_PinCountMax
            // 
            this.label_PinCountMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_PinCountMax.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_PinCountMax.Location = new System.Drawing.Point(184, 28);
            this.label_PinCountMax.Name = "label_PinCountMax";
            this.label_PinCountMax.Size = new System.Drawing.Size(100, 29);
            this.label_PinCountMax.TabIndex = 32;
            this.label_PinCountMax.Text = "0";
            this.label_PinCountMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_PinCountMax.Click += new System.EventHandler(this.label_PinCountMax_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(17, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 29);
            this.label2.TabIndex = 31;
            this.label2.Text = "PIN COUNT MAX :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BTN_CONFIG_SAVE
            // 
            this.BTN_CONFIG_SAVE.BackColor = System.Drawing.Color.Tan;
            this.BTN_CONFIG_SAVE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_CONFIG_SAVE.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_CONFIG_SAVE.ForeColor = System.Drawing.Color.White;
            this.BTN_CONFIG_SAVE.Location = new System.Drawing.Point(551, 614);
            this.BTN_CONFIG_SAVE.Name = "BTN_CONFIG_SAVE";
            this.BTN_CONFIG_SAVE.Size = new System.Drawing.Size(122, 53);
            this.BTN_CONFIG_SAVE.TabIndex = 28;
            this.BTN_CONFIG_SAVE.Text = "SAVE";
            this.BTN_CONFIG_SAVE.UseVisualStyleBackColor = false;
            this.BTN_CONFIG_SAVE.Click += new System.EventHandler(this.BTN_CONFIG_SAVE_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.label_Secsgem_Ip4);
            this.groupBox1.Controls.Add(this.label_Secsgem_Ip1);
            this.groupBox1.Controls.Add(this.label_Secsgem_Port);
            this.groupBox1.Controls.Add(this.label_Config_Secsgem_Port);
            this.groupBox1.Controls.Add(this.label_Config_Secsgem_Ip);
            this.groupBox1.Controls.Add(this.label_Secsgem_Ip3);
            this.groupBox1.Controls.Add(this.label_Secsgem_Ip2);
            this.groupBox1.Controls.Add(this.label_Handler_Ip4);
            this.groupBox1.Controls.Add(this.label_Handler_Ip1);
            this.groupBox1.Controls.Add(this.label_Handler_Port);
            this.groupBox1.Controls.Add(this.label_Config_Handler_Port);
            this.groupBox1.Controls.Add(this.label_Config_Handler_Ip);
            this.groupBox1.Controls.Add(this.label_Handler_Ip3);
            this.groupBox1.Controls.Add(this.label_Handler_Ip2);
            this.groupBox1.Controls.Add(this.poisonComboBox_Light_Port);
            this.groupBox1.Controls.Add(this.button_Bcr_DisConnect);
            this.groupBox1.Controls.Add(this.button_Bcr_Connect);
            this.groupBox1.Controls.Add(this.label_Config_Light);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(375, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(394, 297);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            // 
            // label_Secsgem_Ip4
            // 
            this.label_Secsgem_Ip4.BackColor = System.Drawing.Color.White;
            this.label_Secsgem_Ip4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Secsgem_Ip4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Secsgem_Ip4.ForeColor = System.Drawing.Color.Black;
            this.label_Secsgem_Ip4.Location = new System.Drawing.Point(175, 138);
            this.label_Secsgem_Ip4.Name = "label_Secsgem_Ip4";
            this.label_Secsgem_Ip4.Size = new System.Drawing.Size(50, 40);
            this.label_Secsgem_Ip4.TabIndex = 136;
            this.label_Secsgem_Ip4.Text = "100";
            this.label_Secsgem_Ip4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Secsgem_Ip4.Click += new System.EventHandler(this.label_Secsgem_Ip4_Click);
            // 
            // label_Secsgem_Ip1
            // 
            this.label_Secsgem_Ip1.BackColor = System.Drawing.Color.White;
            this.label_Secsgem_Ip1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Secsgem_Ip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Secsgem_Ip1.ForeColor = System.Drawing.Color.Black;
            this.label_Secsgem_Ip1.Location = new System.Drawing.Point(22, 138);
            this.label_Secsgem_Ip1.Name = "label_Secsgem_Ip1";
            this.label_Secsgem_Ip1.Size = new System.Drawing.Size(50, 40);
            this.label_Secsgem_Ip1.TabIndex = 135;
            this.label_Secsgem_Ip1.Text = "100";
            this.label_Secsgem_Ip1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Secsgem_Ip1.Click += new System.EventHandler(this.label_Secsgem_Ip1_Click);
            // 
            // label_Secsgem_Port
            // 
            this.label_Secsgem_Port.BackColor = System.Drawing.Color.White;
            this.label_Secsgem_Port.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Secsgem_Port.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Secsgem_Port.ForeColor = System.Drawing.Color.Black;
            this.label_Secsgem_Port.Location = new System.Drawing.Point(230, 138);
            this.label_Secsgem_Port.Name = "label_Secsgem_Port";
            this.label_Secsgem_Port.Size = new System.Drawing.Size(77, 40);
            this.label_Secsgem_Port.TabIndex = 134;
            this.label_Secsgem_Port.Text = "2001";
            this.label_Secsgem_Port.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Secsgem_Port.Click += new System.EventHandler(this.label_Secsgem_Port_Click);
            // 
            // label_Config_Secsgem_Port
            // 
            this.label_Config_Secsgem_Port.BackColor = System.Drawing.Color.LightGray;
            this.label_Config_Secsgem_Port.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Secsgem_Port.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Config_Secsgem_Port.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Secsgem_Port.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Secsgem_Port.Location = new System.Drawing.Point(230, 110);
            this.label_Config_Secsgem_Port.Name = "label_Config_Secsgem_Port";
            this.label_Config_Secsgem_Port.Size = new System.Drawing.Size(77, 28);
            this.label_Config_Secsgem_Port.TabIndex = 133;
            this.label_Config_Secsgem_Port.Text = "PORT";
            this.label_Config_Secsgem_Port.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Secsgem_Ip
            // 
            this.label_Config_Secsgem_Ip.BackColor = System.Drawing.Color.LightGray;
            this.label_Config_Secsgem_Ip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Secsgem_Ip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Config_Secsgem_Ip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Secsgem_Ip.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Secsgem_Ip.Location = new System.Drawing.Point(22, 110);
            this.label_Config_Secsgem_Ip.Name = "label_Config_Secsgem_Ip";
            this.label_Config_Secsgem_Ip.Size = new System.Drawing.Size(203, 28);
            this.label_Config_Secsgem_Ip.TabIndex = 132;
            this.label_Config_Secsgem_Ip.Text = "SECSGEM IP ADDRESS";
            this.label_Config_Secsgem_Ip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Secsgem_Ip3
            // 
            this.label_Secsgem_Ip3.BackColor = System.Drawing.Color.White;
            this.label_Secsgem_Ip3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Secsgem_Ip3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Secsgem_Ip3.ForeColor = System.Drawing.Color.Black;
            this.label_Secsgem_Ip3.Location = new System.Drawing.Point(124, 138);
            this.label_Secsgem_Ip3.Name = "label_Secsgem_Ip3";
            this.label_Secsgem_Ip3.Size = new System.Drawing.Size(50, 40);
            this.label_Secsgem_Ip3.TabIndex = 131;
            this.label_Secsgem_Ip3.Text = "100";
            this.label_Secsgem_Ip3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Secsgem_Ip3.Click += new System.EventHandler(this.label_Secsgem_Ip3_Click);
            // 
            // label_Secsgem_Ip2
            // 
            this.label_Secsgem_Ip2.BackColor = System.Drawing.Color.White;
            this.label_Secsgem_Ip2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Secsgem_Ip2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Secsgem_Ip2.ForeColor = System.Drawing.Color.Black;
            this.label_Secsgem_Ip2.Location = new System.Drawing.Point(73, 138);
            this.label_Secsgem_Ip2.Name = "label_Secsgem_Ip2";
            this.label_Secsgem_Ip2.Size = new System.Drawing.Size(50, 40);
            this.label_Secsgem_Ip2.TabIndex = 130;
            this.label_Secsgem_Ip2.Text = "100";
            this.label_Secsgem_Ip2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Secsgem_Ip2.Click += new System.EventHandler(this.label_Secsgem_Ip2_Click);
            // 
            // label_Handler_Ip4
            // 
            this.label_Handler_Ip4.BackColor = System.Drawing.Color.White;
            this.label_Handler_Ip4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Handler_Ip4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Handler_Ip4.ForeColor = System.Drawing.Color.Black;
            this.label_Handler_Ip4.Location = new System.Drawing.Point(175, 67);
            this.label_Handler_Ip4.Name = "label_Handler_Ip4";
            this.label_Handler_Ip4.Size = new System.Drawing.Size(50, 40);
            this.label_Handler_Ip4.TabIndex = 129;
            this.label_Handler_Ip4.Text = "100";
            this.label_Handler_Ip4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Handler_Ip4.Click += new System.EventHandler(this.label_Handler_Ip4_Click);
            // 
            // label_Handler_Ip1
            // 
            this.label_Handler_Ip1.BackColor = System.Drawing.Color.White;
            this.label_Handler_Ip1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Handler_Ip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Handler_Ip1.ForeColor = System.Drawing.Color.Black;
            this.label_Handler_Ip1.Location = new System.Drawing.Point(22, 67);
            this.label_Handler_Ip1.Name = "label_Handler_Ip1";
            this.label_Handler_Ip1.Size = new System.Drawing.Size(50, 40);
            this.label_Handler_Ip1.TabIndex = 128;
            this.label_Handler_Ip1.Text = "100";
            this.label_Handler_Ip1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Handler_Ip1.Click += new System.EventHandler(this.label_Handler_Ip1_Click);
            // 
            // label_Handler_Port
            // 
            this.label_Handler_Port.BackColor = System.Drawing.Color.White;
            this.label_Handler_Port.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Handler_Port.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Handler_Port.ForeColor = System.Drawing.Color.Black;
            this.label_Handler_Port.Location = new System.Drawing.Point(230, 67);
            this.label_Handler_Port.Name = "label_Handler_Port";
            this.label_Handler_Port.Size = new System.Drawing.Size(77, 40);
            this.label_Handler_Port.TabIndex = 127;
            this.label_Handler_Port.Text = "2001";
            this.label_Handler_Port.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Handler_Port.Click += new System.EventHandler(this.label_Handler_Port_Click);
            // 
            // label_Config_Handler_Port
            // 
            this.label_Config_Handler_Port.BackColor = System.Drawing.Color.LightGray;
            this.label_Config_Handler_Port.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Handler_Port.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Config_Handler_Port.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Handler_Port.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Handler_Port.Location = new System.Drawing.Point(230, 39);
            this.label_Config_Handler_Port.Name = "label_Config_Handler_Port";
            this.label_Config_Handler_Port.Size = new System.Drawing.Size(77, 28);
            this.label_Config_Handler_Port.TabIndex = 126;
            this.label_Config_Handler_Port.Text = "PORT";
            this.label_Config_Handler_Port.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Config_Handler_Ip
            // 
            this.label_Config_Handler_Ip.BackColor = System.Drawing.Color.LightGray;
            this.label_Config_Handler_Ip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Handler_Ip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Config_Handler_Ip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Handler_Ip.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Handler_Ip.Location = new System.Drawing.Point(22, 39);
            this.label_Config_Handler_Ip.Name = "label_Config_Handler_Ip";
            this.label_Config_Handler_Ip.Size = new System.Drawing.Size(203, 28);
            this.label_Config_Handler_Ip.TabIndex = 125;
            this.label_Config_Handler_Ip.Text = "HANDLER IP ADDRESS";
            this.label_Config_Handler_Ip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_Handler_Ip3
            // 
            this.label_Handler_Ip3.BackColor = System.Drawing.Color.White;
            this.label_Handler_Ip3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Handler_Ip3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Handler_Ip3.ForeColor = System.Drawing.Color.Black;
            this.label_Handler_Ip3.Location = new System.Drawing.Point(124, 67);
            this.label_Handler_Ip3.Name = "label_Handler_Ip3";
            this.label_Handler_Ip3.Size = new System.Drawing.Size(50, 40);
            this.label_Handler_Ip3.TabIndex = 124;
            this.label_Handler_Ip3.Text = "100";
            this.label_Handler_Ip3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Handler_Ip3.Click += new System.EventHandler(this.label_Handler_Ip3_Click);
            // 
            // label_Handler_Ip2
            // 
            this.label_Handler_Ip2.BackColor = System.Drawing.Color.White;
            this.label_Handler_Ip2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Handler_Ip2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Handler_Ip2.ForeColor = System.Drawing.Color.Black;
            this.label_Handler_Ip2.Location = new System.Drawing.Point(73, 67);
            this.label_Handler_Ip2.Name = "label_Handler_Ip2";
            this.label_Handler_Ip2.Size = new System.Drawing.Size(50, 40);
            this.label_Handler_Ip2.TabIndex = 123;
            this.label_Handler_Ip2.Text = "100";
            this.label_Handler_Ip2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_Handler_Ip2.Click += new System.EventHandler(this.label_Handler_Ip2_Click);
            // 
            // poisonComboBox_Light_Port
            // 
            this.poisonComboBox_Light_Port.Cursor = System.Windows.Forms.Cursors.Hand;
            this.poisonComboBox_Light_Port.DropDownHeight = 120;
            this.poisonComboBox_Light_Port.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.poisonComboBox_Light_Port.FormattingEnabled = true;
            this.poisonComboBox_Light_Port.IntegralHeight = false;
            this.poisonComboBox_Light_Port.ItemHeight = 20;
            this.poisonComboBox_Light_Port.Location = new System.Drawing.Point(137, 197);
            this.poisonComboBox_Light_Port.Name = "poisonComboBox_Light_Port";
            this.poisonComboBox_Light_Port.Size = new System.Drawing.Size(170, 28);
            this.poisonComboBox_Light_Port.TabIndex = 51;
            // 
            // button_Bcr_DisConnect
            // 
            this.button_Bcr_DisConnect.BackColor = System.Drawing.Color.Tan;
            this.button_Bcr_DisConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Bcr_DisConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Bcr_DisConnect.ForeColor = System.Drawing.Color.White;
            this.button_Bcr_DisConnect.Location = new System.Drawing.Point(195, 233);
            this.button_Bcr_DisConnect.Name = "button_Bcr_DisConnect";
            this.button_Bcr_DisConnect.Size = new System.Drawing.Size(95, 40);
            this.button_Bcr_DisConnect.TabIndex = 48;
            this.button_Bcr_DisConnect.Text = "DISCONNECT";
            this.button_Bcr_DisConnect.UseVisualStyleBackColor = false;
            this.button_Bcr_DisConnect.Click += new System.EventHandler(this.button_Bcr_DisConnect_Click);
            // 
            // button_Bcr_Connect
            // 
            this.button_Bcr_Connect.BackColor = System.Drawing.Color.Tan;
            this.button_Bcr_Connect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Bcr_Connect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_Bcr_Connect.ForeColor = System.Drawing.Color.White;
            this.button_Bcr_Connect.Location = new System.Drawing.Point(94, 231);
            this.button_Bcr_Connect.Name = "button_Bcr_Connect";
            this.button_Bcr_Connect.Size = new System.Drawing.Size(95, 44);
            this.button_Bcr_Connect.TabIndex = 47;
            this.button_Bcr_Connect.Text = "CONNECT";
            this.button_Bcr_Connect.UseVisualStyleBackColor = false;
            this.button_Bcr_Connect.Click += new System.EventHandler(this.button_Bcr_Connect_Click);
            // 
            // label_Config_Light
            // 
            this.label_Config_Light.BackColor = System.Drawing.SystemColors.Window;
            this.label_Config_Light.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Config_Light.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_Config_Light.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Config_Light.ForeColor = System.Drawing.Color.Black;
            this.label_Config_Light.Location = new System.Drawing.Point(22, 197);
            this.label_Config_Light.Name = "label_Config_Light";
            this.label_Config_Light.Size = new System.Drawing.Size(109, 29);
            this.label_Config_Light.TabIndex = 30;
            this.label_Config_Light.Text = "LIGHT";
            this.label_Config_Light.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(42, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(204, 23);
            this.label1.TabIndex = 26;
            this.label1.Text = "COMMUNICATION";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.hopeCheckBox_ImageGrabUse);
            this.groupBox2.Controls.Add(this.hopeCheckBox_PinCountUse);
            this.groupBox2.Controls.Add(this.checkBox_BcrGo);
            this.groupBox2.Controls.Add(this.checkBox_IdleReportPass);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(375, 340);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 268);
            this.groupBox2.TabIndex = 45;
            this.groupBox2.TabStop = false;
            // 
            // hopeCheckBox_ImageGrabUse
            // 
            this.hopeCheckBox_ImageGrabUse.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_ImageGrabUse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.hopeCheckBox_ImageGrabUse.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.hopeCheckBox_ImageGrabUse.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.hopeCheckBox_ImageGrabUse.Enable = true;
            this.hopeCheckBox_ImageGrabUse.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_ImageGrabUse.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.hopeCheckBox_ImageGrabUse.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.hopeCheckBox_ImageGrabUse.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hopeCheckBox_ImageGrabUse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.hopeCheckBox_ImageGrabUse.Location = new System.Drawing.Point(83, 159);
            this.hopeCheckBox_ImageGrabUse.Name = "hopeCheckBox_ImageGrabUse";
            this.hopeCheckBox_ImageGrabUse.Size = new System.Drawing.Size(159, 20);
            this.hopeCheckBox_ImageGrabUse.TabIndex = 30;
            this.hopeCheckBox_ImageGrabUse.Text = "CCD GRAB USE";
            this.hopeCheckBox_ImageGrabUse.UseVisualStyleBackColor = true;
            this.hopeCheckBox_ImageGrabUse.Visible = false;
            // 
            // hopeCheckBox_PinCountUse
            // 
            this.hopeCheckBox_PinCountUse.CheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_PinCountUse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.hopeCheckBox_PinCountUse.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(198)))), ((int)(((byte)(202)))));
            this.hopeCheckBox_PinCountUse.DisabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(186)))), ((int)(((byte)(187)))), ((int)(((byte)(189)))));
            this.hopeCheckBox_PinCountUse.Enable = true;
            this.hopeCheckBox_PinCountUse.EnabledCheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.hopeCheckBox_PinCountUse.EnabledStringColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.hopeCheckBox_PinCountUse.EnabledUncheckedColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(158)))), ((int)(((byte)(161)))));
            this.hopeCheckBox_PinCountUse.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hopeCheckBox_PinCountUse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(49)))), ((int)(((byte)(51)))));
            this.hopeCheckBox_PinCountUse.Location = new System.Drawing.Point(83, 133);
            this.hopeCheckBox_PinCountUse.Name = "hopeCheckBox_PinCountUse";
            this.hopeCheckBox_PinCountUse.Size = new System.Drawing.Size(163, 20);
            this.hopeCheckBox_PinCountUse.TabIndex = 29;
            this.hopeCheckBox_PinCountUse.Text = "PIN COUNT USE";
            this.hopeCheckBox_PinCountUse.UseVisualStyleBackColor = true;
            this.hopeCheckBox_PinCountUse.Visible = false;
            // 
            // checkBox_BcrGo
            // 
            this.checkBox_BcrGo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.checkBox_BcrGo.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.checkBox_BcrGo.FlatAppearance.BorderSize = 2;
            this.checkBox_BcrGo.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray;
            this.checkBox_BcrGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_BcrGo.Location = new System.Drawing.Point(29, 212);
            this.checkBox_BcrGo.Name = "checkBox_BcrGo";
            this.checkBox_BcrGo.Size = new System.Drawing.Size(194, 21);
            this.checkBox_BcrGo.TabIndex = 28;
            this.checkBox_BcrGo.Text = "Start Automation on Barcode";
            this.checkBox_BcrGo.UseVisualStyleBackColor = false;
            this.checkBox_BcrGo.Visible = false;
            // 
            // checkBox_IdleReportPass
            // 
            this.checkBox_IdleReportPass.BackColor = System.Drawing.Color.WhiteSmoke;
            this.checkBox_IdleReportPass.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.checkBox_IdleReportPass.FlatAppearance.BorderSize = 2;
            this.checkBox_IdleReportPass.FlatAppearance.CheckedBackColor = System.Drawing.Color.DarkGray;
            this.checkBox_IdleReportPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_IdleReportPass.Location = new System.Drawing.Point(29, 185);
            this.checkBox_IdleReportPass.Name = "checkBox_IdleReportPass";
            this.checkBox_IdleReportPass.Size = new System.Drawing.Size(194, 21);
            this.checkBox_IdleReportPass.TabIndex = 27;
            this.checkBox_IdleReportPass.Text = "IDLE REASON REPORT PASS";
            this.checkBox_IdleReportPass.UseVisualStyleBackColor = false;
            this.checkBox_IdleReportPass.Visible = false;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(42, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(204, 23);
            this.label3.TabIndex = 26;
            this.label3.Text = "운전 설정";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ConfigControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.Controls.Add(this.ManualPanel);
            this.Controls.Add(this.ManualTitleLabel);
            this.Name = "ConfigControl";
            this.Size = new System.Drawing.Size(965, 850);
            this.VisibleChanged += new System.EventHandler(this.ConfigControl_VisibleChanged);
            this.ManualPanel.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ManualTitleLabel;
        private System.Windows.Forms.Panel ManualPanel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button BTN_CONFIG_SAVE;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label_Config_Light;
        private System.Windows.Forms.CheckBox checkBox_IdleReportPass;
        private System.Windows.Forms.CheckBox checkBox_BcrGo;
        private ReaLTaiizor.Controls.HopeCheckBox hopeCheckBox_PinCountUse;
        private System.Windows.Forms.Button button_Bcr_Connect;
        private System.Windows.Forms.Button button_Bcr_DisConnect;
        private ReaLTaiizor.Controls.HopeCheckBox hopeCheckBox_ImageGrabUse;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label_PinCountMax;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox ComboBox_Language;
        private System.Windows.Forms.ComboBox poisonComboBox_Light_Port;
        public System.Windows.Forms.Label label_Config_Tray_GapY;
        public System.Windows.Forms.Label label_Config_Tray_GapX;
        private System.Windows.Forms.Label label_Config_Tray_GapY_Val;
        private System.Windows.Forms.Label label_Config_Tray_GapX_Val;
        public System.Windows.Forms.Label label_Config_Handler_Ip;
        private System.Windows.Forms.Label label_Handler_Ip3;
        private System.Windows.Forms.Label label_Handler_Ip2;
        public System.Windows.Forms.Label label_Config_Handler_Port;
        private System.Windows.Forms.Label label_Handler_Port;
        private System.Windows.Forms.Label label_CsvScanMax;
        private System.Windows.Forms.Label label_Handler_Ip4;
        private System.Windows.Forms.Label label_Handler_Ip1;
        private System.Windows.Forms.Label label_Secsgem_Ip4;
        private System.Windows.Forms.Label label_Secsgem_Ip1;
        private System.Windows.Forms.Label label_Secsgem_Port;
        public System.Windows.Forms.Label label_Config_Secsgem_Port;
        public System.Windows.Forms.Label label_Config_Secsgem_Ip;
        private System.Windows.Forms.Label label_Secsgem_Ip3;
        private System.Windows.Forms.Label label_Secsgem_Ip2;
    }
}
