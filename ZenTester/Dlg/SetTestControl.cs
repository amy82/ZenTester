﻿using Matrox.MatroxImagingLibrary;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Dlg
{
    public partial class SetTestControl : UserControl
    {
        public enum ResizeDirection
        {
            None,
            TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left,
            Move
        }
        const int HANDLE_SIZE = 10;
        const int LINE_THICKNESS = 2;
        const int LINE_HIT_MARGIN = 10;

        public Bitmap CurrentImage { get; set; }

        private OpenCvSharp.Point centerPos;
        public System.Drawing.Point m_clClickPoint = new System.Drawing.Point();
        public Rectangle m_rSetCamBox = new Rectangle();

        private System.Drawing.Point roiStart;
        private System.Drawing.Point roiEnd;
        private bool isDragging = false;
        private bool isMovingRoi= false;
        private ResizeDirection resizeDir = ResizeDirection.None;
        private bool isResizing = false;
        private int isRoiChecked = -1;
        private int isRoiNo = -1;

        private System.Drawing.Point moveStartMousePos;     // 마우스가 눌린 위치
        private System.Drawing.Point moveStartRoiPos;       // ROI 원래 위치

        //private System.Drawing.Point[] DistLineX = new System.Drawing.Point[2];
        private System.Drawing.Point[,] DistLineX = new System.Drawing.Point[2, 2];

        private bool m_bDrawFlag = false;
        private bool m_bBoxMoveFlag = false;
        private bool m_bDrawMeasureLine = false;

        private Rectangle DrawRoiBox;

        public List<Data.Roi> tempRoi = new List<Data.Roi>();


        private int[] CamW = new int[2];
        private int[] CamH = new int[2];
        int CamIndex = 0;
        public SetTestControl()
        {
            InitializeComponent();

            tempRoi.Clear();

            //this.Set_panelCam.
            checkBox_Roi_Key.CheckedChanged += checkBox_CheckedChanged;
            checkBox_Roi_ORing.CheckedChanged += checkBox_CheckedChanged;
            checkBox_Roi_Cone.CheckedChanged += checkBox_CheckedChanged;
            checkBox_Roi_Height.CheckedChanged += checkBox_CheckedChanged;
        }
        public void setCamCenter()
        {
            int i = 0;
            for (i = 0; i < 2; i++)
            {
                CamW[i] = Globalo.visionManager.milLibrary.CAM_SIZE_X[i];
                CamH[i] = Globalo.visionManager.milLibrary.CAM_SIZE_Y[i];

                centerPos.X = (int)(CamW[i] / 2);
                centerPos.Y = (int)(CamH[i] / 2);

                DistLineX[i,0] = new System.Drawing.Point(500, 500);
                DistLineX[i,1] = new System.Drawing.Point(CamW[i] - 500, CamH[i] - 500);
            }

            showCamResol();
            
        }
        private void showCamResol()
        {
            label_Set_TopCam_ResolX_Val.Text = Globalo.yamlManager.aoiRoiConfig.TopResolution.X.ToString();
            label_Set_TopCam_ResolY_Val.Text = Globalo.yamlManager.aoiRoiConfig.TopResolution.Y.ToString();

            label_Set_SideCam_ResolX_Val.Text = Globalo.yamlManager.aoiRoiConfig.SideResolution.X.ToString();
            label_Set_SideCam_ResolY_Val.Text = Globalo.yamlManager.aoiRoiConfig.SideResolution.Y.ToString();
        }
        public void ClearCheckbox()
        {
            checkBox_Roi_Key.Checked = false;
            checkBox_Roi_ORing.Checked = false;
            checkBox_Roi_Cone.Checked = false;
            checkBox_Roi_Height.Checked = false;

        }

        public void UpdateImage(Bitmap image)
        {
            if (CurrentImage != null) CurrentImage.Dispose();

            CurrentImage = (Bitmap)image.Clone();

            //pictureBoxCam1.Image = CurrentImage;
        }

        public int getWidth()
        {
            return this.Set_panelCam.Width;
        }
        public int getHeight()
        {
            return this.Set_panelCam.Height;
        }
        
        private void btn_TopCam_Image_Save_Click(object sender, EventArgs e)
        {
            //TOP CAMERA SAVE
            //
            Globalo.visionManager.milLibrary.ClearOverlay(0);
            Globalo.visionManager.milLibrary.DrawOverlayText(0, new System.Drawing.Point(100,100), "overlay test", Color.Yellow, 30);

            Globalo.visionManager.milLibrary.DrawOverlayArrow(0, 500, 500 , 500, 1500, Color.Yellow, 2, System.Drawing.Drawing2D.DashStyle.Solid);
            Globalo.visionManager.milLibrary.DrawOverlayArrow(0, 2500, 500 , 2500, 2000, Color.Yellow, 2, System.Drawing.Drawing2D.DashStyle.Solid);
            Globalo.visionManager.milLibrary.DrawOverlayArrow(0, 3500, 500 , 3500, 1500, Color.Yellow, 2, System.Drawing.Drawing2D.DashStyle.Solid);

            Globalo.visionManager.aoiTester.CirCleFind(0);

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
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new System.Drawing.Point(1500, 200), "overlay test1", Color.Yellow, 30); 
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new System.Drawing.Point(1500, 400), "overlay test2", Color.Yellow, 30); 
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new System.Drawing.Point(1500, 600), "overlay test3", Color.Yellow, 30); 
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new System.Drawing.Point(1500, 800), "overlay test4", Color.Yellow, 30); 
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new System.Drawing.Point(1500, 1000), "overlay test5", Color.Yellow, 30); 
            Globalo.visionManager.milLibrary.DrawOverlayText(1, new System.Drawing.Point(1500, 1200), "overlay test6", Color.Yellow, 30);

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

        private void button_SetTest_TopCam_Click(object sender, EventArgs e)
        {
            //Set_panelCam.Handle
            CamIndex = 0;
            Globalo.visionManager.ChangeSettingDisplayHandle(CamIndex, Set_panelCam);
        }

        private void button_SetTest_SideCam_Click(object sender, EventArgs e)
        {
            CamIndex = 1;
            Globalo.visionManager.ChangeSettingDisplayHandle(CamIndex, Set_panelCam);
        }
        private void drawCenterCross()
        {
            Globalo.visionManager.milLibrary.ClearOverlay(0);
            int cx = Globalo.visionManager.milLibrary.CAM_SIZE_X[CamIndex];
            int cy = Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex];
            Globalo.visionManager.milLibrary.DrawOverlayCross(0, cx / 2, cy / 2, 1000, Color.Yellow, 1, System.Drawing.Drawing2D.DashStyle.Solid);
        }
        private void SetTestControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Globalo.visionManager.milLibrary.RunModeChange(false);
                Globalo.visionManager.ChangeSettingDisplayHandle(0, Set_panelCam);

                drawCenterCross();
                showCamResol();
            }
            else
            {
                checkBox_Measure.Checked = false;
                m_bDrawMeasureLine = false;
                isRoiChecked = -1;
                isRoiNo = -1;
                ClearCheckbox();
            }
        }

        #region [TOP CAMERA MANUAL TEST]
        private void button_Set_Key_Test_Click(object sender, EventArgs e)
        {
            bool rtn = true;

            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex];
            int dataSize = sizeX * sizeY;


            //byte[] ImageBuffer = new byte[dataSize];

            //MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[CamIndex], ImageBuffer);
            //Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            //Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);


            //Globalo.visionManager.aoiTopTester.Keytest(CamIndex, src, centerPos, 0);        //키검사

            //A ~ D: 2개씩
            //E 타입만 1개
            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(CamIndex);
            Globalo.visionManager.aoiTopTester.MilEdgeKeytest(CamIndex, 0, "C");        //키검사
            Globalo.visionManager.aoiTopTester.MilEdgeKeytest(CamIndex, 1, "C");        //키검사
            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, true);
            
        }

        private void button_Set_Housing_Test_Click(object sender, EventArgs e)
        {
            bool rtn = true;

            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex];
            int dataSize = sizeX * sizeY;


            byte[] ImageBuffer = new byte[dataSize];

            



            //
            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(CamIndex);

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[CamIndex], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);

            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, true);

            List<OpenCvSharp.Point> FakraCenter;
            List<OpenCvSharp.Point> HousingCenter;
            FakraCenter = Globalo.visionManager.aoiTopTester.Housing_Fakra_Test(CamIndex, src); //Fakra 안쪽 원 찾기
            HousingCenter = Globalo.visionManager.aoiTopTester.Housing_Dent_Test(CamIndex, src); //Con1,2(동심도)  / Dent (찌그러짐) 검사 


            double CamResolX = 0.0;
            double CamResolY = 0.0;
            CamResolX = Globalo.yamlManager.aoiRoiConfig.TopResolution.X;   // 0.0186f;
            CamResolY = Globalo.yamlManager.aoiRoiConfig.TopResolution.Y;   //0.0186f;


            OpenCvSharp.Point c1 = FakraCenter[1];
            OpenCvSharp.Point c2 = HousingCenter[0];
            OpenCvSharp.Point c3 = HousingCenter[1];

            float dx = c1.X - c2.X;
            float dy = c1.Y - c2.Y;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);

            System.Drawing.Point ConePoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex] - 520);
            string str = $"Con1:{(dist * CamResolX).ToString("0.00#")}";
            Globalo.visionManager.milLibrary.DrawOverlayText(CamIndex, ConePoint, str, Color.GreenYellow, 13);
            Console.WriteLine($"Con1:{dist* CamResolX}");

            dx = c1.X - c3.X;
            dy = c1.Y - c3.Y;
            dist = (float)Math.Sqrt(dx * dx + dy * dy);

            Console.WriteLine($"Con2:{dist* CamResolX}");


            

            ConePoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex] - 590);
            str = $"Con2:{(dist * CamResolX).ToString("0.00#")}";
            Globalo.visionManager.milLibrary.DrawOverlayText(CamIndex, ConePoint, str, Color.GreenYellow, 13);

        }

        private void button_Set_Gasket_Test_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex];
            int dataSize = sizeX * sizeY;


            byte[] ImageBuffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[CamIndex], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);

            Globalo.visionManager.aoiTopTester.GasketTest(CamIndex, src, centerPos);     //가스켓 검사
        }

        private void button_Set_Dent_Test_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region [SIDE CAMERA MANUAL TEST]
        private void button_Set_Oring_Test_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex];
            int dataSize = sizeX * sizeY;


            //byte[] ImageBuffer = new byte[dataSize];
            //MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[CamIndex], ImageBuffer);
            //Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            //Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);



            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(CamIndex);
            Globalo.visionManager.aoiSideTester.MilEdgeOringTest(CamIndex, 0);
            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, true);
        }

        private void button_Set_Cone_Test_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex];
            int dataSize = sizeX * sizeY;


            //byte[] ImageBuffer = new byte[dataSize];
            //MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[CamIndex], ImageBuffer);
            //Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            //Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);


            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(CamIndex);
            Globalo.visionManager.aoiSideTester.MilEdgeConeTest(CamIndex, 0);//, src);
            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, true);
        }

        private void button_Set_Height_Test_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex];
            int dataSize = sizeX * sizeY;

            //byte[] ImageBuffer = new byte[dataSize];

            //MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[CamIndex], ImageBuffer);
            //Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            //Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);

            //Globalo.visionManager.aoiSideTester.HeightTest(CamIndex, src);
            //Globalo.visionManager.aoiSideTester.MilHeightTest(CamIndex, src);

            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(CamIndex);
            //
            Globalo.visionManager.aoiSideTester.HeightTest(CamIndex);
            //
            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, true);
        }
        #endregion

        private void button_Pogo_Find_Test_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex];
            int dataSize = sizeX * sizeY;

            byte[] ImageBuffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[CamIndex], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);


            Globalo.visionManager.aoiTopTester.FindPogoPinCenter(CamIndex, src);     //가스켓 검사
        }
        private void drawTestRoi(int index)
        {
            Data.Roi targetRoi;
            Rectangle m_clRect;
            System.Drawing.Point textPoint;
            Globalo.visionManager.milLibrary.ClearOverlay(0);

            int boxLine = 1;
            if (index == 0)
            {
                targetRoi = tempRoi[0];// Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[0];
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(0, textPoint, "LH ROI", Color.BlueViolet, 15);

                targetRoi = tempRoi[1];//Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[1];
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(0, textPoint, "CH ROI", Color.BlueViolet, 15);

                targetRoi = tempRoi[2];//Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[2];
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(0, textPoint, "RH ROI", Color.BlueViolet, 15);
            }
            if (index == 1)
            {
                targetRoi = tempRoi[0];//Globalo.yamlManager.aoiRoiConfig.CONE_ROI.FirstOrDefault(r => r.name == Data.NO_ROI.CONE.ToString());
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(0, textPoint, "CONE ROI", Color.BlueViolet, 15);
            }
            if (index == 2)
            {
                targetRoi = tempRoi[0];//Globalo.yamlManager.aoiRoiConfig.ORING_ROI.FirstOrDefault(r => r.name == Data.NO_ROI.ORING.ToString());
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(0, textPoint, "ORING ROI", Color.BlueViolet, 15);
            }
            if (index == 3)
            {
                targetRoi = tempRoi[0];//Globalo.yamlManager.aoiRoiConfig.KEY_ROI.FirstOrDefault(r => r.name == Data.NO_ROI.KEY1.ToString());

                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(0, textPoint, "KEY1 ROI", Color.BlueViolet, 15);

                targetRoi = tempRoi[1];//Globalo.yamlManager.aoiRoiConfig.KEY_ROI.FirstOrDefault(r => r.name == Data.NO_ROI.KEY2.ToString());
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(0, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(0, textPoint, "KEY2 ROI", Color.BlueViolet, 15);
            }
        }
        private int checkNoRoi(int index, System.Drawing.Point mousePos)
        {
            int i = 0;
            Rectangle RoiBox = new Rectangle();
            mousePos.X = (int)(mousePos.X * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5);
            mousePos.Y = (int)(mousePos.Y * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5);
            //
            //
            for (i = 0; i < tempRoi.Count; i++)
            {
                RoiBox.X = tempRoi[i].X - HANDLE_SIZE / 2;
                RoiBox.Y = tempRoi[i].Y - HANDLE_SIZE / 2;
                RoiBox.Width = tempRoi[i].Width + HANDLE_SIZE;
                RoiBox.Height = tempRoi[i].Height + HANDLE_SIZE;
                if (RoiBox.Contains(mousePos))
                {
                    return i;
                }
            }
            return -1;
        }
        #region [MOUSE DRAW]

        
        private void Set_panelCam_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isRoiChecked >= 0)   //roi 영역 클릭했는지 판단
                {
                    moveStartMousePos = e.Location;
                    Console.WriteLine($"{e.Location.X} , {e.Location.Y}");
                    
                    isRoiNo = checkNoRoi(isRoiChecked, e.Location);

                    if (isRoiNo >= 0)      //Height//if (isRoiChecked == 0 && isRoiNo >= 0)      //Height
                    {
                        DrawRoiBox.X = tempRoi[isRoiNo].X;
                        DrawRoiBox.Y = tempRoi[isRoiNo].Y;
                        DrawRoiBox.Width = tempRoi[isRoiNo].Width;
                        DrawRoiBox.Height = tempRoi[isRoiNo].Height;
                    }

                    Console.WriteLine($"[{isRoiChecked}] Click : {isRoiNo}");

                    System.Drawing.Point roiMousePos = new System.Drawing.Point();
                    roiMousePos.X = (int)(e.Location.X * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5);
                    roiMousePos.Y = (int)(e.Location.Y * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5);
                    
                    resizeDir = GetResizeDirection(roiMousePos, DrawRoiBox);

                    Console.WriteLine($"resizeDir : {resizeDir} {DrawRoiBox.X},{DrawRoiBox.Y}");
                    if (resizeDir == ResizeDirection.Move)
                    {
                        moveStartRoiPos = DrawRoiBox.Location;
                        roiStart = e.Location;
                        roiEnd = e.Location;
                    }
                    return;
                }
                if (m_bDrawMeasureLine == true)
                {
                    moveStartMousePos = e.Location;
                    resizeDir = GetDistDirection(e.Location);
                    return;
                }
                if (DrawRoiBox != null)
                {
                    resizeDir = GetResizeDirection(e.Location, DrawRoiBox);
                }
                
                if (resizeDir != ResizeDirection.None && resizeDir != ResizeDirection.Move)
                {
                    if (DrawRoiBox.Width > 10 || DrawRoiBox.Height > 10)
                    {
                        isResizing = true;
                        moveStartMousePos = e.Location;
                        moveStartRoiPos = DrawRoiBox.Location;
                    }
                    return;
                }
                else if (DrawRoiBox != null && DrawRoiBox.Contains(e.Location))
                {
                    isMovingRoi = true;
                    moveStartMousePos = e.Location;
                    moveStartRoiPos = DrawRoiBox.Location;
                }
                else
                {

                    isDragging = true;
                    roiStart = e.Location;
                    roiEnd = e.Location;

                    m_clClickPoint = new System.Drawing.Point(e.X, e.Y);

                    Globalo.visionManager.milLibrary.DrawRgbValue(CamIndex, m_clClickPoint);
                }
            }
        }
        private void Set_panelCam_MouseMove(object sender, MouseEventArgs e)
        {
            if (DrawRoiBox != null)// && isResizing == false)
            {
                if (m_bDrawMeasureLine == true)
                {
                    ResizeDirection hoverDir = GetDistDirection(e.Location);
                    Cursor.Current = GetCursorByResizeDirection(hoverDir);
                }
                else if (isRoiChecked >= 0 && isRoiNo >= 0)
                {
                    System.Drawing.Point roiMousePos = new System.Drawing.Point();
                    roiMousePos.X = (int)(e.Location.X * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5);
                    roiMousePos.Y = (int)(e.Location.Y * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5);
                    ResizeDirection hoverDir = GetResizeDirection(roiMousePos, DrawRoiBox);
                    Cursor.Current = GetCursorByResizeDirection(hoverDir);
                }
                else if (isDragging == false)
                {
                    ResizeDirection hoverDir = GetResizeDirection(e.Location, DrawRoiBox);
                    Cursor.Current = GetCursorByResizeDirection(hoverDir);
                }
            }
            if (m_bDrawMeasureLine && e.Button == MouseButtons.Left)
            {
                int dx = (int)((e.X - moveStartMousePos.X) * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5);
                int dy = (int)((e.Y - moveStartMousePos.Y) * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5);

                switch (resizeDir)
                {
                    case ResizeDirection.Top:
                        DistLineX[CamIndex,0].Y += dy;
                        break;
                    case ResizeDirection.Left:
                        DistLineX[CamIndex, 0].X += dx;
                        break;
                    case ResizeDirection.Right:
                        DistLineX[CamIndex, 1].X += dx;
                        break;
                    case ResizeDirection.Bottom:
                        DistLineX[CamIndex, 1].Y += dy;
                        break;
                }

                moveStartMousePos = e.Location;
                DrawDistnace();
            }
            else if (isRoiChecked >= 0 && isRoiNo >= 0 && resizeDir != ResizeDirection.None)
            {
                roiEnd = e.Location;
                int dx = (int)((e.X - moveStartMousePos.X) * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5); 
                int dy = (int)((e.Y - moveStartMousePos.Y) * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5);

                Rectangle r = DrawRoiBox;

                switch (resizeDir)
                {
                    case ResizeDirection.TopLeft:
                        r.X += dx;
                        r.Y += dy;
                        r.Width -= dx;
                        r.Height -= dy;
                        break;

                    case ResizeDirection.Top:
                        r.Y += dy;
                        r.Height -= dy;
                        break;

                    case ResizeDirection.TopRight:
                        r.Y += dy;
                        r.Width += dx;
                        r.Height -= dy;
                        break;

                    case ResizeDirection.Right:
                        r.Width += dx;
                        break;

                    case ResizeDirection.BottomRight:
                        r.Width += dx;
                        r.Height += dy;
                        break;

                    case ResizeDirection.Bottom:
                        r.Height += dy;
                        break;

                    case ResizeDirection.BottomLeft:
                        r.X += dx;
                        r.Width -= dx;
                        r.Height += dy;
                        break;

                    case ResizeDirection.Left:
                        r.X += dx;
                        r.Width -= dx;
                        break;

                    case ResizeDirection.Move: 

                        r.Location = new System.Drawing.Point(moveStartRoiPos.X + dx, moveStartRoiPos.Y + dy);
                        DrawRoiBox.X = r.X;
                        DrawRoiBox.Y = r.Y;
                        break;
                }
                if(resizeDir != ResizeDirection.None && resizeDir != ResizeDirection.Move)
                {
                    DrawRoiBox = r;
                    moveStartMousePos = e.Location;

                }
                
                // 최소 크기 보정
                if (r.Width < 5) r.Width = 5;
                if (r.Height < 5) r.Height = 5;

                tempRoi[isRoiNo].X = DrawRoiBox.X;
                tempRoi[isRoiNo].Y = DrawRoiBox.Y;
                tempRoi[isRoiNo].Width = DrawRoiBox.Width;
                tempRoi[isRoiNo].Height = DrawRoiBox.Height;

                if (isRoiChecked == 0)      //Height
                {
                    //Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[isRoiNo].x = DrawRoiBox.X;
                    //Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[isRoiNo].y = DrawRoiBox.Y;
                    //Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[isRoiNo].width = DrawRoiBox.Width;
                    //Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[isRoiNo].height = DrawRoiBox.Height;
                }
                else if (isRoiChecked == 1)      //Cone
                {

                }
                else if (isRoiChecked == 2)      //Oring
                {

                }
                else if (isRoiChecked == 3)      //Key
                {

                }

                drawTestRoi(isRoiChecked);
            }
            else if (isResizing)
            {
                int dx = e.X - moveStartMousePos.X;
                int dy = e.Y - moveStartMousePos.Y;

                Rectangle r = DrawRoiBox;

                switch (resizeDir)
                {
                    case ResizeDirection.TopLeft:
                        r.X += dx;
                        r.Y += dy;
                        r.Width -= dx;
                        r.Height -= dy;
                        break;

                    case ResizeDirection.Top:
                        r.Y += dy;
                        r.Height -= dy;
                        break;

                    case ResizeDirection.TopRight:
                        r.Y += dy;
                        r.Width += dx;
                        r.Height -= dy;
                        break;

                    case ResizeDirection.Right:
                        r.Width += dx;
                        break;

                    case ResizeDirection.BottomRight:
                        r.Width += dx;
                        r.Height += dy;
                        break;

                    case ResizeDirection.Bottom:
                        r.Height += dy;
                        break;

                    case ResizeDirection.BottomLeft:
                        r.X += dx;
                        r.Width -= dx;
                        r.Height += dy;
                        break;

                    case ResizeDirection.Left:
                        r.X += dx;
                        r.Width -= dx;
                        break;
                }

                // 최소 크기 보정
                if (r.Width < 5) r.Width = 5;
                if (r.Height < 5) r.Height = 5;

                DrawRoiBox = r;
                moveStartMousePos = e.Location;

                Rectangle m_clRect = new Rectangle(
                    (int)(DrawRoiBox.X * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5), (int)(DrawRoiBox.Y * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5),
                    (int)(DrawRoiBox.Width * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5), (int)(DrawRoiBox.Height * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5));
                Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);
                Globalo.visionManager.milLibrary.DrawOverlayBox(CamIndex, m_clRect, Color.Blue, 1, System.Drawing.Drawing2D.DashStyle.Solid);
            }
            else if (isDragging)
            {
                roiEnd = e.Location;
                Rectangle roi = GetRoiRect(roiStart, roiEnd);
                Rectangle m_clRect = new Rectangle((int)(roi.X * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5), (int)(roi.Y * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5),
                    (int)(roi.Width * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5), (int)(roi.Height * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5));

                Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);
                Globalo.visionManager.milLibrary.DrawOverlayBox(CamIndex, m_clRect, Color.Blue, 1, System.Drawing.Drawing2D.DashStyle.Solid);

                //Set_panelCam.Invalidate();
                // 마우스 왼쪽 버튼이 눌린 상태에서 이동 중
                Console.WriteLine($"드래그 중: X={e.X}, Y={e.Y}");
                // ROI 박스 그리기 등 처리
            }
            else if (isMovingRoi)
            {
                // 마우스 이동량 계산
                int dx = e.X - moveStartMousePos.X;
                int dy = e.Y - moveStartMousePos.Y;

                Console.WriteLine($"드래그 중: dx={dx}, dy={dy}");
                DrawRoiBox.Location = new System.Drawing.Point(moveStartRoiPos.X + dx, moveStartRoiPos.Y + dy);


                Rectangle m_clRect = new Rectangle(
                    (int)(DrawRoiBox.X * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5), (int)(DrawRoiBox.Y * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5),
                    (int)(DrawRoiBox.Width * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5), (int)(DrawRoiBox.Height * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5));

                Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);
                Globalo.visionManager.milLibrary.DrawOverlayBox(CamIndex, m_clRect, Color.Blue, 1, System.Drawing.Drawing2D.DashStyle.Solid);
            }
        }
        private void Set_panelCam_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
            resizeDir = ResizeDirection.None;
            isMovingRoi = false;
            if (isDragging)
            {
                isDragging = false;
                
                roiEnd = e.Location;
                DrawRoiBox = GetRoiRect(roiStart, roiEnd);

                int dragw = (int)(DrawRoiBox.Width * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5);
                int dragh = (int)(DrawRoiBox.Height * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5);
                Console.WriteLine($"Drag Roi = w:{dragw},h:{dragh}");
            }
            
            
        }
        //--------------------------------------------------------------------------------------
        //
        //ALIGN CAM
        //
        //--------------------------------------------------------------------------------------
        
        #endregion

        private void checkBox_Measure_CheckedChanged(object sender, EventArgs e)
        {
            m_bDrawMeasureLine = false;
            if (checkBox_Measure.Checked)
            {
                isRoiChecked = -1;
                isRoiNo = -1;
                m_bDrawMeasureLine = true;
                DrawDistnace();
            }
            else
            {
                drawCenterCross();
            }
        }
        private void DrawDistnace()
        {

            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            //DistLineX[0] = new System.Drawing.Point(500, 500);
            //DistLineX[1] = new System.Drawing.Point(sizeX - 500, sizeY - 500);

            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);
            Globalo.visionManager.milLibrary.DrawOverlayLine(0, (int)(DistLineX[CamIndex,0].X), 0, (int)(DistLineX[CamIndex, 0].X), (int)Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex], Color.Red, 1);
            Globalo.visionManager.milLibrary.DrawOverlayLine(0, (int)(DistLineX[CamIndex, 1].X), 0, (int)(DistLineX[CamIndex, 1].X), (int)Globalo.visionManager.milLibrary.CAM_SIZE_Y[CamIndex], Color.Red, 1);

            Globalo.visionManager.milLibrary.DrawOverlayLine(0, 0, (int)(DistLineX[CamIndex, 0].Y), (int)Globalo.visionManager.milLibrary.CAM_SIZE_X[CamIndex], (int)(DistLineX[CamIndex, 0].Y), Color.Blue, 1);
            Globalo.visionManager.milLibrary.DrawOverlayLine(0, 0, (int)(DistLineX[CamIndex, 1].Y), (int)Globalo.visionManager.milLibrary.CAM_SIZE_X[CamIndex], (int)(DistLineX[CamIndex, 1].Y), Color.Blue, 1);

            double CamResolX = 0.0;
            double CamResolY = 0.0;
            //CamResolX = Globalo.yamlManager.aoiRoiConfig.SideResolution.X;   // 0.02026f;
            //CamResolY = Globalo.yamlManager.aoiRoiConfig.SideResolution.Y;   //0.02026f;//0.0288f;
            CamResolX = Globalo.yamlManager.aoiRoiConfig.TopResolution.X;   // 0.02026f;
            CamResolY = Globalo.yamlManager.aoiRoiConfig.TopResolution.Y;   //0.02026f;//0.0288f;

            Console.WriteLine($"CamResolX:{CamResolX}");
            Console.WriteLine($"CamResolY:{CamResolY}");
            //
            System.Drawing.Point textPoint;

            string str = $"[Distance x:{Math.Abs(DistLineX[CamIndex, 0].X - DistLineX[CamIndex, 1].X) * CamResolX}";
            textPoint = new System.Drawing.Point(10, CamH[CamIndex] - 250);
            Globalo.visionManager.milLibrary.DrawOverlayText(CamIndex, textPoint, str, Color.Blue, 15);

            str = $"[Distance y:{Math.Abs(DistLineX[CamIndex, 0].Y - DistLineX[CamIndex, 1].Y) * CamResolY}";
            textPoint = new System.Drawing.Point(10, CamH[CamIndex] - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(CamIndex, textPoint, str, Color.Blue, 15);

        }

        private Rectangle GetRoiRect(System.Drawing.Point p1, System.Drawing.Point p2)
        {
            return new Rectangle(
                Math.Min(p1.X, p2.X),
                Math.Min(p1.Y, p2.Y),
                Math.Abs(p2.X - p1.X),
                Math.Abs(p2.Y - p1.Y)
            );
        }
        private ResizeDirection GetDistDirection(System.Drawing.Point mousePos)
        {
            //거리측정
            //(int)(DrawRoiBox.X * Globalo.visionManager.milLibrary.xExpand + 0.5)
            Rectangle x1Line = new Rectangle((int)(DistLineX[CamIndex,0].X * Globalo.visionManager.milLibrary.xReduce[CamIndex] + 0.5) - LINE_HIT_MARGIN / 2, 0, LINE_HIT_MARGIN, (int)(CamH[CamIndex] * Globalo.visionManager.milLibrary.yReduce[CamIndex] + 0.5));
            Rectangle x2Line = new Rectangle((int)(DistLineX[CamIndex, 1].X * Globalo.visionManager.milLibrary.xReduce[CamIndex] + 0.5) - LINE_HIT_MARGIN / 2, 0, LINE_HIT_MARGIN, (int)(CamH[CamIndex] * Globalo.visionManager.milLibrary.yReduce[CamIndex] + 0.5));

            Rectangle y1Line = new Rectangle(0, (int)(DistLineX[CamIndex, 0].Y * Globalo.visionManager.milLibrary.yReduce[CamIndex] + 0.5) - LINE_HIT_MARGIN / 2, (int)(CamW[CamIndex] * Globalo.visionManager.milLibrary.xReduce[CamIndex] + 0.5), LINE_HIT_MARGIN);
            Rectangle y2Line = new Rectangle(0, (int)(DistLineX[CamIndex, 1].Y * Globalo.visionManager.milLibrary.yReduce[CamIndex] + 0.5) - LINE_HIT_MARGIN / 2, (int)(CamW[CamIndex] * Globalo.visionManager.milLibrary.xReduce[CamIndex] + 0.5), LINE_HIT_MARGIN);

            if (y1Line.Contains(mousePos)) return ResizeDirection.Top;
            if (y2Line.Contains(mousePos)) return ResizeDirection.Bottom;

            if (x1Line.Contains(mousePos)) return ResizeDirection.Left;
            if (x2Line.Contains(mousePos)) return ResizeDirection.Right;

            return ResizeDirection.None;

        }
        private ResizeDirection GetResizeDirection(System.Drawing.Point mousePos, Rectangle roibox)
        {
            
            // 9개 위치
            Rectangle left = new Rectangle(roibox.Left - HANDLE_SIZE / 2, roibox.Top + HANDLE_SIZE, HANDLE_SIZE, roibox.Height - HANDLE_SIZE / 2);
            Rectangle right = new Rectangle(roibox.Right - HANDLE_SIZE / 2, roibox.Top + HANDLE_SIZE, HANDLE_SIZE, roibox.Height - HANDLE_SIZE / 2);
            Rectangle top = new Rectangle(roibox.Left + HANDLE_SIZE / 2, roibox.Top - HANDLE_SIZE / 2, roibox.Width - HANDLE_SIZE / 2, HANDLE_SIZE);
            Rectangle bottom = new Rectangle(roibox.Left + HANDLE_SIZE / 2, roibox.Bottom - HANDLE_SIZE / 2, roibox.Width - HANDLE_SIZE / 2, HANDLE_SIZE);
            //
            Rectangle topLeft = new Rectangle(roibox.Left - HANDLE_SIZE / 2, roibox.Top - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE);
            Rectangle topRight = new Rectangle(roibox.Right - HANDLE_SIZE / 2, roibox.Top - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE);
            Rectangle bottomRight = new Rectangle(roibox.Right - HANDLE_SIZE / 2, roibox.Bottom - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE);
            Rectangle bottomLeft = new Rectangle(roibox.Left - HANDLE_SIZE / 2, roibox.Bottom - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE);

            Rectangle MoveRect = new Rectangle(roibox.Left + HANDLE_SIZE / 2, roibox.Top + HANDLE_SIZE / 2, roibox.Width - HANDLE_SIZE, roibox.Height - HANDLE_SIZE);

            if (topLeft.Contains(mousePos)) return ResizeDirection.TopLeft;
            if (topRight.Contains(mousePos)) return ResizeDirection.TopRight;
            if (bottomRight.Contains(mousePos)) return ResizeDirection.BottomRight;
            if (bottomLeft.Contains(mousePos)) return ResizeDirection.BottomLeft;
            if (top.Contains(mousePos)) return ResizeDirection.Top;
            if (right.Contains(mousePos)) return ResizeDirection.Right;
            if (bottom.Contains(mousePos)) return ResizeDirection.Bottom;
            if (left.Contains(mousePos)) return ResizeDirection.Left;
            if (MoveRect.Contains(mousePos)) return ResizeDirection.Move;


            return ResizeDirection.None;
        }
        private Cursor GetCursorByResizeDirection(ResizeDirection dir)
        {
            switch (dir)
            {
                case ResizeDirection.TopLeft:
                case ResizeDirection.BottomRight:
                    return Cursors.SizeNWSE;

                case ResizeDirection.TopRight:
                case ResizeDirection.BottomLeft:
                    return Cursors.SizeNESW;

                case ResizeDirection.Top:
                case ResizeDirection.Bottom:
                    return Cursors.SizeNS;

                case ResizeDirection.Left:
                case ResizeDirection.Right:
                    return Cursors.SizeWE;
                case ResizeDirection.Move:
                    return Cursors.SizeAll;
                default:
                    return Cursors.Default;
            }
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox changed = sender as CheckBox;
            isRoiChecked = -1;
            if (changed.Checked)
            {
                tempRoi.Clear();
                // 모든 체크박스를 순회하면서
                foreach (var ctrl in this.Controls)
                {
                    if (ctrl is CheckBox cb && cb != changed)
                    {
                        cb.Checked = false;
                    }
                }

                Console.WriteLine($"{changed.Name} Checked");

                if (changed.Name == "checkBox_Roi_Height") 
                {
                    isRoiChecked = 0;

                    tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[0].Clone());
                    tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[1].Clone());
                    tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[2].Clone());
                    drawTestRoi(isRoiChecked);
                }
                if (changed.Name == "checkBox_Roi_Cone") 
                {
                    isRoiChecked = 1;

                    tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].Clone());
                    drawTestRoi(isRoiChecked);
                }
                if (changed.Name == "checkBox_Roi_ORing")
                {
                    isRoiChecked = 2;
                    tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.ORING_ROI[0].Clone());
                    drawTestRoi(isRoiChecked);
                }
                if (changed.Name == "checkBox_Roi_Key")
                {
                    isRoiChecked = 3;
                    tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.KEY_ROI[0].Clone());
                    tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.KEY_ROI[1].Clone());
                    drawTestRoi(isRoiChecked);
                }

            }
            if(isRoiChecked  < 0) 
            {
                Globalo.visionManager.milLibrary.ClearOverlay(0); 
            }
        }

        private void button_Set_Roi_Save_Click(object sender, EventArgs e)
        {
            int i = 0;
            if (isRoiChecked == 0)      //Height
            {
                for (i = 0; i < Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI.Count; i++)
                {
                    Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[i] = tempRoi[i].Clone();
                }
            }
            else if (isRoiChecked == 1)      //Cone
            {
                for (i = 0; i < Globalo.yamlManager.aoiRoiConfig.CONE_ROI.Count; i++)
                {
                    Globalo.yamlManager.aoiRoiConfig.CONE_ROI[i] = tempRoi[i].Clone();
                }
            }
            else if (isRoiChecked == 2)      //Oring
            {
                for (i = 0; i < Globalo.yamlManager.aoiRoiConfig.ORING_ROI.Count; i++)
                {
                    Globalo.yamlManager.aoiRoiConfig.ORING_ROI[i] = tempRoi[i].Clone();
                }
            }
            else if (isRoiChecked == 3)      //Key
            {
                for (i = 0; i < Globalo.yamlManager.aoiRoiConfig.KEY_ROI.Count; i++)
                {
                    Globalo.yamlManager.aoiRoiConfig.KEY_ROI[i] = tempRoi[i].Clone();
                }
            }
            Data.TaskDataYaml.Save_AoiConfig("AoiConfig.yaml");
        }

        private void CamResolutionInput(Label OffsetLabel)
        {
            string labelValue = OffsetLabel.Text;
            decimal decimalValue = 0;


            if (decimal.TryParse(labelValue, out decimalValue))
            {
                // 소수점 형식으로 변환
                string formattedValue = decimalValue.ToString("0.000###");
                NumPadForm popupForm = new NumPadForm(formattedValue, false);

                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    double dNumData = Double.Parse(popupForm.NumPadResult);

                    OffsetLabel.Text = dNumData.ToString("0.000###");
                }
            }
        }

        private void label_Set_TopCam_ResolX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            CamResolutionInput(clickedLabel);
        }

        private void label_Set_TopCam_ResolY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            CamResolutionInput(clickedLabel);
        }

        private void label_Set_SideCam_ResolX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            CamResolutionInput(clickedLabel);
        }

        private void label_Set_SideCam_ResolY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            CamResolutionInput(clickedLabel);
        }

        private void button_Set_Top_Resol_Save_Click(object sender, EventArgs e)
        {
            Globalo.yamlManager.aoiRoiConfig.TopResolution.X = double.Parse(label_Set_TopCam_ResolX_Val.Text);
            Globalo.yamlManager.aoiRoiConfig.TopResolution.Y = double.Parse(label_Set_TopCam_ResolY_Val.Text);
            Data.TaskDataYaml.Save_AoiConfig("AoiConfig.yaml");

            if (checkBox_Measure.Checked)
            {
                DrawDistnace();
            }
        }

        private void button_Set_Side_Resol_Save_Click(object sender, EventArgs e)
        {
            Globalo.yamlManager.aoiRoiConfig.SideResolution.X = double.Parse(label_Set_SideCam_ResolX_Val.Text);
            Globalo.yamlManager.aoiRoiConfig.SideResolution.Y = double.Parse(label_Set_SideCam_ResolY_Val.Text);

            Data.TaskDataYaml.Save_AoiConfig("AoiConfig.yaml");
            if (checkBox_Measure.Checked)
            {
                DrawDistnace();
            }
        }

        private void label_SetTest_Manual_Mark_Regist_Click(object sender, EventArgs e)
        {

            //double dSizeX, double dSizeY, double dCenterX, double dCenterY
            Rectangle DrawRoiBox = GetRoiRect(roiStart, roiEnd);

            double dSizeX = DrawRoiBox.Width;
            double dSizeY = DrawRoiBox.Height;
            double dCenterX = DrawRoiBox.X + (DrawRoiBox.Width / 2);
            double dCenterY = DrawRoiBox.Y + (DrawRoiBox.Height / 2);

            Console.WriteLine($"Mark Roi = Center X:{dCenterX},Center Y:{dCenterY},w:{dSizeX},h:{dSizeY}");

            if (dSizeX > 1024 || dSizeY > 1024)
            {
                return;
            }
            if (dSizeX < 10 || dSizeY < 10)
            {
                return;
            }
            Globalo.visionManager.markUtil.RegisterMark(CamIndex, DrawRoiBox.X, DrawRoiBox.Y, dSizeX, dSizeY);
        }

        private void label_SetTest_Manual_Mark_View_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.markUtil.ViewMarkMask();
        }

        private void label_SetTest_Manual_Mark_Find_Click(object sender, EventArgs e)
        {
            VisionClass.CDMotor dAlign = new VisionClass.CDMotor();


            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(CamIndex);
            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, true);

            Globalo.visionManager.markUtil.CalcSingleMarkAlign(CamIndex, 0, ref dAlign);

            Console.WriteLine($"X:{dAlign.X},Y: {dAlign.Y}, T:{dAlign.T}");
        }
    }
}
