
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
            this.SuspendLayout();
            // 
            // label_WriteSetTest_Title
            // 
            this.label_WriteSetTest_Title.BackColor = System.Drawing.Color.Black;
            this.label_WriteSetTest_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_WriteSetTest_Title.ForeColor = System.Drawing.Color.White;
            this.label_WriteSetTest_Title.Location = new System.Drawing.Point(21, 21);
            this.label_WriteSetTest_Title.Name = "label_WriteSetTest_Title";
            this.label_WriteSetTest_Title.Size = new System.Drawing.Size(666, 23);
            this.label_WriteSetTest_Title.TabIndex = 3;
            this.label_WriteSetTest_Title.Text = "Write load Set";
            this.label_WriteSetTest_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_WSet_ConfRead
            // 
            this.button_WSet_ConfRead.BackColor = System.Drawing.Color.Tan;
            this.button_WSet_ConfRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_WSet_ConfRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_WSet_ConfRead.Location = new System.Drawing.Point(507, 316);
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
            this.button_WSet_Crc_Cal.Location = new System.Drawing.Point(507, 123);
            this.button_WSet_Crc_Cal.Name = "button_WSet_Crc_Cal";
            this.button_WSet_Crc_Cal.Size = new System.Drawing.Size(180, 57);
            this.button_WSet_Crc_Cal.TabIndex = 109;
            this.button_WSet_Crc_Cal.Text = "W Crc Cal";
            this.button_WSet_Crc_Cal.UseVisualStyleBackColor = false;
            // 
            // WriteSetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_WSet_Crc_Cal);
            this.Controls.Add(this.button_WSet_ConfRead);
            this.Controls.Add(this.label_WriteSetTest_Title);
            this.Name = "WriteSetControl";
            this.Size = new System.Drawing.Size(807, 568);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_WriteSetTest_Title;
        private System.Windows.Forms.Button button_WSet_ConfRead;
        private System.Windows.Forms.Button button_WSet_Crc_Cal;
    }
}
