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

        //private Controls.DefaultGridView ModelGridView1;

        public enum eManualBtn : int
        {
            TransferTab = 0, MagazineTab, LiftTab, pcbTab, lensTab
        };

        private string PointFormat = "0.0###";
        private int dRowHeight = 40;
        //
        private string ColorSelecttGrid = "#E1E0DF";       //FFB230
        //
        //
        private int nGridRowCount = 1;              //Grid 총 Row / 세로 칸 수
        private int nGridColCount = 2;              //Grid 총 Col / 가로 칸 수

        public int SelectColIndex = -1;
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

            nGridRowCount = Globalo.yamlManager.modelLIstData.ModelData.Modellist.Count;

            //ModelGridView1 = new Controls.DefaultGridView(GridCol, modelCnt, inGridWid, 40);
            //ModelGridView1.Location = new Point(10, TeachingTitleLabel.Location.Y+ TeachingTitleLabel.Height + 30);

            string[] title = new string[] { "No", "Model" };         //Grid Width


            for (int i = 0; i < dataGridViewModel.ColumnCount; i++)
            {
                dataGridViewModel.Columns[i].Name = title[i];
            }
            //this.Controls.Add(ModelGridView1);

            InitializeGrid();
        }
        public void showModelList()
        {
            int i = 0;
            string posName = "";
            int modelCnt = Globalo.yamlManager.modelLIstData.ModelData.Modellist.Count;
            for (i = 0; i < modelCnt; i++)
            {
                posName = Globalo.yamlManager.modelLIstData.ModelData.Modellist[i];
                //ModelGridView1.Rows[i].SetValues(posName);
                dataGridViewModel[1, i].Value = posName;

                //if (i < GridRow)//Globalo.yamlManager.modelLIstData.ModelData.Modellist.Count)
                //{
                //    dataGridViewModel[1, i].Value = posName;
                //}
                //else
                //{
                //    dataGridViewModel.Rows.Add("", posName);
                //}
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
        }
        private void InitializeGrid()
        {
            //GRID
            int i = 0;
            int RowCount = nGridRowCount;

            int dGridHeight = RowCount * dRowHeight;
            int scrollWidth = 3;// 20;


            int dGridWidth = 0;
            for (i = 0; i < inGridWid.Length; i++)
            {
                dGridWidth += inGridWid[i];
            }

            dataGridViewModel.ColumnCount = nGridColCount;
            dataGridViewModel.EnableHeadersVisualStyles = false;
            dataGridViewModel.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing; //사이즈 조절 막기
            dataGridViewModel.RowCount = nGridRowCount;
            dataGridViewModel.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dataGridViewModel.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Yellow;
            // 헤더 배경색: 파란색, 글자색: 흰색, 폰트: 굵은 고딕체
            dataGridViewModel.EnableHeadersVisualStyles = false; // 반드시 꺼야 커스텀 색상이 적용됨

            dataGridViewModel.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue;
            dataGridViewModel.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewModel.ColumnHeadersDefaultCellStyle.Font = new Font("나눔고딕",17, FontStyle.Bold);     //HEADER
            dataGridViewModel.DefaultCellStyle.Font = new Font("나눔고딕", 15, FontStyle.Bold);     //목록
            //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;    //마우스 사이즈 조절 막기 Height
            //this.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridViewModel.AllowUserToResizeRows = false;
            dataGridViewModel.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridViewModel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dataGridViewModel.Name = "Grid";
            dataGridViewModel.Size = new Size(dGridWidth + scrollWidth, dGridHeight + dRowHeight + 2);
            dataGridViewModel.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewModel.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridViewModel.GridColor = Color.Black;
            dataGridViewModel.RowHeadersVisible = false;
            //this.CellClick += TeachGrid_CellClick;
            //this.CellDoubleClick += TeachGrid_CellDoubleClick;

            string[] titleArr = { "No", "RECIPE LIST" };
            for (i = 0; i < dataGridViewModel.ColumnCount; i++)
            {
                dataGridViewModel.Columns[i].Name = titleArr[i];
                dataGridViewModel.Columns[i].Resizable = DataGridViewTriState.False;
                dataGridViewModel.Columns[i].Width = inGridWid[i];
                dataGridViewModel.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridViewModel.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }


            dataGridViewModel.ColumnHeadersHeight = dRowHeight;

            for (i = 0; i < nGridRowCount; i++)
            {
                dataGridViewModel.Rows[i].Height = dRowHeight;
            }

            string posName = "";

            dataGridViewModel.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewModel.ReadOnly = true;
            dataGridViewModel.CurrentCell = null;
            dataGridViewModel.MultiSelect = false;
        }
        private void TeachingBtnChange(eManualBtn index)
        {

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
