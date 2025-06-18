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

        public CameraControl()
        {
            InitializeComponent();

            //initNewCameraSet();
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
        public void initNewCameraSet()
        {
            //Globalo.visionManager = new VisionClass.VisionManager(getWidth(), getHeight() , Globalo.setTestControl.Set_panelCam.Width, Globalo.setTestControl.Set_panelCam.Height);

           



            
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
                openFileDialog.InitialDirectory = "D:\\Work\\Pro_Ject\\Mexico\\Aoi\\_temp\\Image"; ;// Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
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

        private void CameraControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                
            }
        }
    }
}
