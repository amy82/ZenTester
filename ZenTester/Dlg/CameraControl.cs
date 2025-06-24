using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenTester.Dlg
{
    public partial class CameraControl : UserControl
    {
        public Bitmap CurrentImage { get; set; }
        private Controls.DefaultGridView ResultGridView1;
        private Controls.DefaultGridView ResultGridView2;

        private int GridCol = 2;                                //picker , bcr , state
        private int GridRow = 11;                                //picker , bcr , state
        private int[] StartPos = new int[] { 1450, 10 };          //Grid Pos
        private int[] inGridWid = new int[] { 120, 130 };    //Grid Width


        private Label[] LeftResultTitle;
        private Label[] RightResultTitle;

        private Label[] LeftResultVal;
        private Label[] RightResultVal;
        public CameraControl()
        {
            InitializeComponent();

            //initResultGrid();
            LeftResultTitle = new Label[] { label_Aoi_Result_Lh1, label_Aoi_Result_Mh1, label_Aoi_Result_Rh1,
                label_Aoi_Result_Cone1, label_Aoi_Result_ORing1,label_Aoi_Result_Gasket1, label_Aoi_Result_Key1,label_Aoi_Result_Dent1,
            label_Aoi_Result_ConA1, label_Aoi_Result_ConD1};

            RightResultTitle = new Label[] { label_Aoi_Result_Lh2, label_Aoi_Result_Mh2, label_Aoi_Result_Rh2,
                label_Aoi_Result_Cone2, label_Aoi_Result_ORing2,label_Aoi_Result_Gasket2, label_Aoi_Result_Key2,label_Aoi_Result_Dent2,
            label_Aoi_Result_ConA2, label_Aoi_Result_ConD2};


            LeftResultVal = new Label[] { label_Aoi_Result_Lh_Val1, label_Aoi_Result_Mh_Val1, label_Aoi_Result_Rh_Val1,
                label_Aoi_Result_Cone_Val1, label_Aoi_Result_ORing_Val1,label_Aoi_Result_Gasket_Val1, label_Aoi_Result_Key_Val1,label_Aoi_Result_Dent_Val1,
            label_Aoi_Result_ConA_Val1, label_Aoi_Result_ConD_Val1};

            RightResultVal = new Label[] { label_Aoi_Result_Lh_Val2, label_Aoi_Result_Mh_Val2, label_Aoi_Result_Rh_Val2,
                label_Aoi_Result_Cone_Val2, label_Aoi_Result_ORing_Val2, label_Aoi_Result_Gasket_Val2, label_Aoi_Result_Key_Val2,label_Aoi_Result_Dent_Val2,
            label_Aoi_Result_ConA_Val2, label_Aoi_Result_ConD_Val2};

            initResult();
        }
        public void setSideTestResult(int socketNum, string result)
        {
            string[] items = result.Split(',');
            if (socketNum == 1)
            {
                //0, 1, 2, 3, 4
                
            }
            else
            {

            }
        }

        public void setTopTestResult(int socketNum, string result)
        {
            string[] items = result.Split(',');
            if (socketNum == 1)
            {
                //5, 6, 7, 8, 9
                //LeftResultVal[i].Text = "0.0";
            }
            else
            {
                //RightResultVal[i].Text = "0.0";
            }
        }
        

        public void initResult()
        {
            int i = 0;
            for (i = 0; i < LeftResultVal.Length; i++)
            {
                LeftResultVal[i].Text = "0.0";
            }
            for (i = 0; i < RightResultVal.Length; i++)
            {
                RightResultVal[i].Text = "0.0";
            }
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
            string[] ItemList = new string[] { "LH ", "RH", "MH", "Gasket", "KeyType", 
                "CircleDented", "Concentrycity_A", "Concentrycity_D", "Cone", "ORing","Result" };         //Grid Width
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
            //
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Bitmap Image (*.bmp)|*.bmp";
                saveFileDialog.Title = "이미지 저장";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = saveFileDialog.FileName;
                    //grabbedImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    Globalo.visionManager.milLibrary.GrabImageSave(0, selectedFilePath);


                    Console.WriteLine("선택한 이미지 경로:\n" + selectedFilePath);
                }
            }
        }

        private void btn_TopCam_Image_Load_Click(object sender, EventArgs e)
        {
            //TOP CAMERA LOAD
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "이미지 파일 선택";
                //openFileDialog.Filter = "이미지 파일 (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|모든 파일 (*.*)|*.*";
                openFileDialog.Filter = "이미지 파일 (*.bmp;)|*.bmp;|모든 파일 (*.*)|*.*";
                //D:\Work\Pro_Ject\Mexico\Aoi\_temp\newCam
                openFileDialog.InitialDirectory = "D:\\Work\\Pro_Ject\\Mexico\\Aoi\\_temp\\newCam"; ;// Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                Globalo.visionManager.milLibrary.SetGrabOn(0, false);
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Globalo.visionManager.milLibrary.ClearOverlay(0);
                    string selectedFilePath = openFileDialog.FileName;
                    Globalo.visionManager.SetLoadBmp(0, selectedFilePath);
                    Console.WriteLine("선택한 이미지 경로:\n" + selectedFilePath);
                }
                else
                {
                    Globalo.visionManager.milLibrary.SetGrabOn(0, true);
                }
            }
        }

        private void btn_SideCam_Image_Save_Click(object sender, EventArgs e)
        {
            //SIDE CAMERA SAVE
            //
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Bitmap Image (*.bmp)|*.bmp";
                saveFileDialog.Title = "이미지 저장";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = saveFileDialog.FileName;
                    //grabbedImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    Globalo.visionManager.milLibrary.GrabImageSave(1, selectedFilePath);


                    Console.WriteLine("선택한 이미지 경로:\n" + selectedFilePath);
                }
            }
        }

        private void btn_SideCam_Image_Load_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "이미지 파일 선택";
                //openFileDialog.Filter = "이미지 파일 (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|모든 파일 (*.*)|*.*";
                openFileDialog.Filter = "이미지 파일 (*.bmp;)|*.bmp;|모든 파일 (*.*)|*.*";
                openFileDialog.InitialDirectory = "D:\\Work\\Pro_Ject\\Mexico\\Aoi\\_temp\\newCam"; ;// Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
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
        public void drawCenterCross()
        {
            int cx = 0;
            int cy = 0;
            Globalo.visionManager.milLibrary.ClearOverlay(0);
            Globalo.visionManager.milLibrary.ClearOverlay(1);

            cx = Globalo.visionManager.milLibrary.CAM_SIZE_X[0];
            cy = Globalo.visionManager.milLibrary.CAM_SIZE_Y[0];

            Globalo.visionManager.milLibrary.DrawOverlayCross(0, cx / 2, cy / 2, 1000, Color.Yellow, 1, System.Drawing.Drawing2D.DashStyle.Solid);

            cx = Globalo.visionManager.milLibrary.CAM_SIZE_X[1];
            cy = Globalo.visionManager.milLibrary.CAM_SIZE_Y[1];

            Globalo.visionManager.milLibrary.DrawOverlayCross(1, cx / 2, cy / 2, 1000, Color.Yellow, 1, System.Drawing.Drawing2D.DashStyle.Solid);
        }

        private void CameraControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                
            }
        }
    }
}
