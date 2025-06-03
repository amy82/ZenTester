using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ZenTester.Dlg
{
    public partial class ModelControl : UserControl
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용

        private eManualBtn manualBtnTab;

        //private ManualTransfer manualTransfer;

        private Controls.DefaultGridView ModelGridView1;

        public enum eManualBtn : int
        {
            TransferTab = 0, MagazineTab, LiftTab, pcbTab, lensTab
        };

        private int GridCol = 2;                                //picker , bcr , state
        private int GridRow = 10;                                //picker , bcr , state
        private int[] StartPos = new int[] { 1450, 10 };          //Grid Pos
        private int[] inGridWid = new int[] { 70, 500 };    //Grid Width
        public ModelControl(int _w , int _h)
        {
            InitializeComponent();

            //manualTransfer = new ManualTransfer();

            //teachingLens = new TeachingLens();
            this.Paint += new PaintEventHandler(Form_Paint);


            this.Width = _w;
            this.Height = _h;


            //manualTransfer.Visible = false;
            //teachingLens.Visible = false;
            //this.Controls.Add(manualTransfer);
            //TeachingPanel.Controls.Add(teachingLens);

            //
            //manualTransfer.Location = new System.Drawing.Point(0, 89);
            setInterface();


            ModelGridView1 = new Controls.DefaultGridView(GridCol, GridRow, inGridWid, 40);
            ModelGridView1.Location = new Point(10, TeachingTitleLabel.Location.Y+ TeachingTitleLabel.Height + 30);
            string[] title = new string[] { "No", "Model" };         //Grid Width
            for (int i = 0; i < ModelGridView1.ColumnCount; i++)
            {
                ModelGridView1.Columns[i].Name = title[i];
            }
            this.Controls.Add(ModelGridView1);

            //manualBtnTab = eManualBtn.TransferTab;
            //TeachingBtnChange(manualBtnTab);
        }
        public void showModelList()
        {
            int i = 0;
            string posName = "";
            for (i = 0; i < GridRow; i++)
            {
                posName = Globalo.yamlManager.modelLIstData.ModelData.Modellist[i];
                //ModelGridView1.Rows[i].SetValues(posName);
                
                if (i < Globalo.yamlManager.modelLIstData.ModelData.Modellist.Count)
                {
                    ModelGridView1[1, i].Value = posName;
                }
                else
                {
                    ModelGridView1.Rows.Add("", posName);
                }
                //dataGridView_EEpromData.Rows.Add((i + 1).ToString(), "0x" + (startAddr + i).ToString("X2"), "0x" + Globalo.mCCdPanel.CcdEEpromReadData[i].ToString("X2"), displayChar);// (char)Globalo.mCCdPanel.CcdEEpromReadData[i]);

            }
        }
        public void ManualDlgStop()
        {
            //manualTransfer.bManualStopKey = true;       //수동 모터 이동 중 빠져나오게
            //manualSocket.bManualStopKey = true;       //수동 모터 이동 중 빠져나오게
            //manualLift.bManualStopKey = true;       //수동 모터 이동 중 빠져나오게
            //manualMagazine.bManualStopKey = true;       //수동 모터 이동 중 빠져나오게
        }
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            int lineStartY = TeachingTitleLabel.Location.Y + Globalo.TabLineY;
            // Graphics 객체 가져오기
            Graphics g = e.Graphics;

            // Pen 객체 생성 (색상과 두께 설정)
            Color color = Color.FromArgb(175, 175, 175);//Color.FromArgb(151, 149, 145);
            Pen pen = new Pen(color, 1);

            // 라인 그리기 (시작점과 끝점 설정)
            g.DrawLine(pen, 0, lineStartY, this.Width, lineStartY);

            // 리소스 해제
            pen.Dispose();
            
        }
        public void setInterface()
        {

            TeachingTitleLabel.ForeColor = ColorTranslator.FromHtml("#6F6F6F");

            //BTN_TEACH_PCB.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            //BTN_TEACH_LENS.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");

        }
        private void TeachingBtnChange(eManualBtn index)
        {
            //BTN_TEACH_PCB.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            //BTN_TEACH_LENS.BackColor = ColorTranslator.FromHtml("#E1E0DF");

            manualBtnTab = index;

            if (manualBtnTab == eManualBtn.TransferTab)
            {
                //BTN_TEACH_PCB.BackColor = ColorTranslator.FromHtml("#FFB230");
                //manualTransfer.Visible = true;
                //teachingLens.Visible = false;

                //teachingLens.hidePanel();
                //manualTransfer.showPanel();
            }
            else
            {
                //BTN_TEACH_LENS.BackColor = ColorTranslator.FromHtml("#FFB230");
                //teachingLens.Visible = true;
                //manualTransfer.Visible = false;

                //manualTransfer.hidePanel();
                //teachingLens.showPanel();
            }
        }
        private void BTN_TEACH_PCB_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eManualBtn.TransferTab);
        }

        private void BTN_TEACH_LENS_Click(object sender, EventArgs e)
        {
            TeachingBtnChange(eManualBtn.lensTab);
        }

        private void ManualControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                //TeachingBtnChange(manualBtnTab);
                showModelList();
            }
            else
            {
                //manualTransfer.Visible = false;
                //manualTransfer.hidePanel();
            }
        }
    }
}
