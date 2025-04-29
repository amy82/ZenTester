using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Dlg
{
    public partial class CameraControl : UserControl
    {
        public Bitmap CurrentImage { get; set; }
        private Controls.DefaultGridView ResultGridView1;
        private Controls.DefaultGridView ResultGridView2;

        private int GridCol = 2;                                //picker , bcr , state
        private int GridRow = 10;                                //picker , bcr , state
        private int[] StartPos = new int[] { 1450, 10 };          //Grid Pos
        private int[] inGridWid = new int[] { 150, 100 };    //Grid Width

        public CameraControl()
        {
            InitializeComponent();
            initNewCameraSet();
            initResultGrid();
            
        }
        public void initResultGrid()
        {
            int i = 0;
            ResultGridView1 = new Controls.DefaultGridView(GridCol, GridRow, inGridWid);
            ResultGridView2 = new Controls.DefaultGridView(GridCol, GridRow, inGridWid);
            ResultGridView1.Location = new Point(label_Socket_Result1.Location.X, label_Socket_Result1.Location.Y + label_Socket_Result1.Height + 1);
            ResultGridView2.Location = new Point(label_Socket_Result2.Location.X, label_Socket_Result2.Location.Y + label_Socket_Result2.Height + 1);

            string[] title = new string[] { "Item", "Result" };         //Grid Width
            for (i = 0; i < ResultGridView1.ColumnCount; i++)
            {
                ResultGridView1.Columns[i].Name = title[i];
                ResultGridView2.Columns[i].Name = title[i];
            }

            string posName = "";
            string[] ItemList = new string[] { "LH ", "MH", "RH", "O Ring", "CONE", "CON1", "CON2", "GASKET", "DENT", "KEY" };         //Grid Width
            for (i = 0; i < GridRow; i++)
            {
                posName = ItemList[i];
                ResultGridView1.Rows[i].SetValues(posName);
                ResultGridView2.Rows[i].SetValues(posName);
            }

            label_Socket_Result1.Width = ResultGridView1.Width;
            label_Socket_Result2.Width = ResultGridView2.Width;
            this.Controls.Add(ResultGridView1);
            this.Controls.Add(ResultGridView2);
        }
        public void initNewCameraSet()
        {
            Globalo.visionManager = new VisionClass.VisionManager(getWidth(), getHeight());
            Globalo.visionManager.RegisterDisplayHandle(0, panelCam1.Handle);
            Globalo.visionManager.RegisterDisplayHandle(1, panelCam2.Handle);

            Globalo.visionManager.MilSet();

        }
        public void UpdateImage(Bitmap image)
        {
            if (CurrentImage != null) CurrentImage.Dispose();

            CurrentImage = (Bitmap)image.Clone();

            //pictureBoxCam1.Image = CurrentImage;
        }

        public int getWidth()
        {
            return this.panelCam1.Width;
        }
        public int getHeight()
        {
            return this.panelCam1.Height;
        }

        private void btn_TopCam_Image_Save_Click(object sender, EventArgs e)
        {
            //TOP CAMERA SAVE
            //MbufExport(sPath, M_BMP, g_clVision.m_MilGrabImage[0][0]);
            Globalo.visionManager.milLibrary.ClearOverlay(0);
            Globalo.visionManager.milLibrary.DrawOverlayText(0, new Point(100,100), "overlay test", Color.Yellow, 30);

            Globalo.visionManager.milLibrary.DrawOverlayArrow(0, 500, 500 , 500, 1500, Color.Yellow, 2, System.Drawing.Drawing2D.DashStyle.Solid);
            Globalo.visionManager.milLibrary.DrawOverlayArrow(0, 2500, 500 , 2500, 2000, Color.Yellow, 2, System.Drawing.Drawing2D.DashStyle.Solid);
            Globalo.visionManager.milLibrary.DrawOverlayArrow(0, 3500, 500 , 3500, 1500, Color.Yellow, 2, System.Drawing.Drawing2D.DashStyle.Solid);

            Globalo.visionManager.aoiTester

        }

        private void btn_TopCam_Image_Load_Click(object sender, EventArgs e)
        {
            //TOP CAMERA LOAD
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "이미지 파일 선택";
                //openFileDialog.Filter = "이미지 파일 (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|모든 파일 (*.*)|*.*";
                openFileDialog.Filter = "이미지 파일 (*.bmp;)|*.bmp;|모든 파일 (*.*)|*.*";
                openFileDialog.InitialDirectory = "D:\\Work\\Pro_Ject\\Mexico\\Aoi\\_temp\\Image"; ;// Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                Globalo.visionManager.milLibrary.SetGrabOn(0, false);
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Globalo.visionManager.milLibrary.ClearOverlay(0);
                    string selectedFilePath = openFileDialog.FileName;
                    Globalo.visionManager.SetLoadBmp(0, selectedFilePath);
                    Console.WriteLine("선택한 이미지 경로:\n" + selectedFilePath);
                    ///MessageBox.Show("선택한 이미지 경로:\n" + selectedFilePath);
                }
                else
                {
                    Globalo.visionManager.milLibrary.SetGrabOn(0, true);
                }
            }
            

            //g_clVision.m_nGrabMode[m_nUnit] = GRAB_LIVE;
            //MbufLoad(pDlg->GetPathName(), g_clVision.m_MilGrabImage[0][0]);
            //MimResize(g_clVision.m_MilGrabImageChild[0][0], g_clVision.MilCamSmallImageChild[0][0], CAM_REDUCE_FACTOR_X, CAM_REDUCE_FACTOR_Y, M_DEFAULT);
        }

        private void btn_SideCam_Image_Save_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(1);  
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new Point(1500, 200), "overlay test1", Color.Yellow, 30); 
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new Point(1500, 400), "overlay test2", Color.Yellow, 30); 
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new Point(1500, 600), "overlay test3", Color.Yellow, 30); 
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new Point(1500, 800), "overlay test4", Color.Yellow, 30); 
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new Point(1500, 1000), "overlay test5", Color.Yellow, 30); 
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new Point(1500, 1200), "overlay test6", Color.Yellow, 30);

            Rectangle m_clRect = new Rectangle((int)(100), (int)(100), 1000, 1000);
            Globalo.visionManager.milLibrary.DrawOverlayBox(1, m_clRect, Color.Blue, 1, System.Drawing.Drawing2D.DashStyle.Dot);
        }

        private void btn_SideCam_Image_Load_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "이미지 파일 선택";
                //openFileDialog.Filter = "이미지 파일 (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|모든 파일 (*.*)|*.*";
                openFileDialog.Filter = "이미지 파일 (*.bmp;)|*.bmp;|모든 파일 (*.*)|*.*";
                openFileDialog.InitialDirectory = "D:\\Work\\Pro_Ject\\Mexico\\Aoi\\_temp\\Image"; ;// Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                Globalo.visionManager.milLibrary.SetGrabOn(1, false);
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Globalo.visionManager.milLibrary.ClearOverlay(1);
                    string selectedFilePath = openFileDialog.FileName;
                    Globalo.visionManager.SetLoadBmp(1, selectedFilePath);
                    Console.WriteLine("선택한 이미지 경로:\n" + selectedFilePath);
                    ///MessageBox.Show("선택한 이미지 경로:\n" + selectedFilePath);
                }
                else
                {
                    Globalo.visionManager.milLibrary.SetGrabOn(1, true);
                }
            }

        }
    }
}
