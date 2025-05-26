namespace ZenHandler.Dlg
{
    partial class ModelControl
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
            this.TeachingTitleLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TeachingTitleLabel
            // 
            this.TeachingTitleLabel.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 19F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TeachingTitleLabel.Location = new System.Drawing.Point(16, 16);
            this.TeachingTitleLabel.Name = "TeachingTitleLabel";
            this.TeachingTitleLabel.Size = new System.Drawing.Size(250, 42);
            this.TeachingTitleLabel.TabIndex = 31;
            this.TeachingTitleLabel.Text = "| MODEL";
            this.TeachingTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ModelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.Controls.Add(this.TeachingTitleLabel);
            this.Name = "ModelControl";
            this.Size = new System.Drawing.Size(770, 800);
            this.VisibleChanged += new System.EventHandler(this.ManualControl_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label TeachingTitleLabel;
    }
}
