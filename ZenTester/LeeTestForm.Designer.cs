
namespace ZenHandler
{
    partial class LeeTestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_Con1_Test = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Con1_Test
            // 
            this.button_Con1_Test.BackColor = System.Drawing.Color.IndianRed;
            this.button_Con1_Test.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.button_Con1_Test.ForeColor = System.Drawing.Color.White;
            this.button_Con1_Test.Location = new System.Drawing.Point(12, 12);
            this.button_Con1_Test.Name = "button_Con1_Test";
            this.button_Con1_Test.Size = new System.Drawing.Size(67, 34);
            this.button_Con1_Test.TabIndex = 0;
            this.button_Con1_Test.Text = "Con1";
            this.button_Con1_Test.UseVisualStyleBackColor = false;
            this.button_Con1_Test.Click += new System.EventHandler(this.button_Con1_Test_Click);
            // 
            // LeeTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(183, 293);
            this.Controls.Add(this.button_Con1_Test);
            this.Name = "LeeTestForm";
            this.Text = "LeeTestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Con1_Test;
    }
}