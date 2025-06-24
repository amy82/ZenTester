namespace ZenTester.Dlg
{
    partial class LogControl
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
            this.listBox_Log = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // ManualTitleLabel
            // 
            this.ManualTitleLabel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ManualTitleLabel.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ManualTitleLabel.ForeColor = System.Drawing.Color.White;
            this.ManualTitleLabel.Location = new System.Drawing.Point(0, 0);
            this.ManualTitleLabel.Name = "ManualTitleLabel";
            this.ManualTitleLabel.Size = new System.Drawing.Size(514, 26);
            this.ManualTitleLabel.TabIndex = 2;
            this.ManualTitleLabel.Text = "| LOG";
            this.ManualTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // listBox_Log
            // 
            this.listBox_Log.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.listBox_Log.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.listBox_Log.FormattingEnabled = true;
            this.listBox_Log.HorizontalScrollbar = true;
            this.listBox_Log.ItemHeight = 15;
            this.listBox_Log.Location = new System.Drawing.Point(0, 26);
            this.listBox_Log.Margin = new System.Windows.Forms.Padding(0);
            this.listBox_Log.Name = "listBox_Log";
            this.listBox_Log.Size = new System.Drawing.Size(514, 664);
            this.listBox_Log.TabIndex = 76;
            // 
            // LogControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.listBox_Log);
            this.Controls.Add(this.ManualTitleLabel);
            this.Name = "LogControl";
            this.Size = new System.Drawing.Size(514, 690);
            this.VisibleChanged += new System.EventHandler(this.AlarmControl_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ManualTitleLabel;
        public System.Windows.Forms.ListBox listBox_Log;
    }
}
