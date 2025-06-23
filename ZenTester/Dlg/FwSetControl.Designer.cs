
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
            this.SuspendLayout();
            // 
            // label_FwSetTest_Title
            // 
            this.label_FwSetTest_Title.BackColor = System.Drawing.Color.Black;
            this.label_FwSetTest_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_FwSetTest_Title.ForeColor = System.Drawing.Color.White;
            this.label_FwSetTest_Title.Location = new System.Drawing.Point(21, 21);
            this.label_FwSetTest_Title.Name = "label_FwSetTest_Title";
            this.label_FwSetTest_Title.Size = new System.Drawing.Size(666, 23);
            this.label_FwSetTest_Title.TabIndex = 3;
            this.label_FwSetTest_Title.Text = "Firmware down load Set";
            this.label_FwSetTest_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_FdSet_ConfRead
            // 
            this.button_FdSet_ConfRead.BackColor = System.Drawing.Color.Tan;
            this.button_FdSet_ConfRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_FdSet_ConfRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_FdSet_ConfRead.Location = new System.Drawing.Point(507, 105);
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
            this.button_FdSet_Test.Location = new System.Drawing.Point(310, 105);
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
            this.button_FdSet_JsonRead.Location = new System.Drawing.Point(507, 191);
            this.button_FdSet_JsonRead.Name = "button_FdSet_JsonRead";
            this.button_FdSet_JsonRead.Size = new System.Drawing.Size(180, 57);
            this.button_FdSet_JsonRead.TabIndex = 110;
            this.button_FdSet_JsonRead.Text = "Json Read";
            this.button_FdSet_JsonRead.UseVisualStyleBackColor = false;
            this.button_FdSet_JsonRead.Click += new System.EventHandler(this.button_FdSet_JsonRead_Click);
            // 
            // FwSetControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_FdSet_JsonRead);
            this.Controls.Add(this.button_FdSet_Test);
            this.Controls.Add(this.button_FdSet_ConfRead);
            this.Controls.Add(this.label_FwSetTest_Title);
            this.Name = "FwSetControl";
            this.Size = new System.Drawing.Size(807, 568);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_FwSetTest_Title;
        private System.Windows.Forms.Button button_FdSet_ConfRead;
        private System.Windows.Forms.Button button_FdSet_Test;
        private System.Windows.Forms.Button button_FdSet_JsonRead;
    }
}
