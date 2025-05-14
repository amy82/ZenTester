using Matrox.MatroxImagingLibrary;
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
        public enum MOUSE_CLICK_TYPE
        {
            MOUSE_DRAG = 0, TRACK_DRAG, DISP_MOVE, MEASURE, DIST_CHECK
        };
        public enum SQUARE_TYPE
        {
            SQUARE_RESET = 0, SQUARE_CREATE, SQUARE_RESIZE, SQUARE_MOVE
        };

        public enum SQUARE_DIR
        {
            STANDARD = -1, CENTER, LEFT, TOP, RIGHT, BOTTOM, LEFTTOP, LEFTBOTTOM, RIGHTTOP, RIGHTBOTTOM
        };

        public enum MOUSE_CURSOR
        {
            MOVE_ALL, MOVE_WIDTH_LEFT, MOVE_WIDTH_RIGHT, MOVE_HEIGHT_TOP, MOVE_HEIGHT_BOTTOM,
            MOVE_NW, MOVE_NE, MOVE_SW, MOVE_SE
        };
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
        private Controls.DefaultGridView ResultGridView1;
        private Controls.DefaultGridView ResultGridView2;

        private OpenCvSharp.Point centerPos;
        private int GridCol = 2;                                //picker , bcr , state
        private int GridRow = 10;                                //picker , bcr , state
        private int[] StartPos = new int[] { 1450, 10 };          //Grid Pos
        private int[] inGridWid = new int[] { 150, 100 };    //Grid Width

        private MOUSE_CLICK_TYPE m_nDragFlag = MOUSE_CLICK_TYPE.MEASURE;
        private SQUARE_TYPE m_nType = SQUARE_TYPE.SQUARE_RESET;
        public System.Drawing.Point m_clClickPoint = new System.Drawing.Point();
        public Rectangle m_rSetCamBox = new Rectangle();

        private System.Drawing.Point roiStart;
        private System.Drawing.Point roiEnd;
        private bool isDragging = false;
        private bool isMovingRoi= false;
        private ResizeDirection resizeDir = ResizeDirection.None;
        private bool isResizing = false;


        private System.Drawing.Point moveStartMousePos;     // 마우스가 눌린 위치
        private System.Drawing.Point moveStartRoiPos;       // ROI 원래 위치

        private System.Drawing.Point[] DistLineX = new System.Drawing.Point[2];


        private bool m_bDrawFlag = false;
        private bool m_bBoxMoveFlag = false;
        private bool m_bDrawMeasureLine = false;

        private Rectangle DrawRoiBox;
        public MOUSE_CURSOR m_iMoveType;

        private int CamW;
        private int CamH;
        int CamIndex = 0;
        public SetTestControl()
        {
            InitializeComponent();


            

            
        }
        public void setCamCenter()
        {
            CamW = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            CamH = Globalo.visionManager.milLibrary.CAM_SIZE_Y;

            DistLineX[0] = new System.Drawing.Point(500, 500);
            DistLineX[1] = new System.Drawing.Point(CamW - 500, CamH - 500);

            centerPos.X = (int)(CamW / 2);
            centerPos.Y = (int)(CamH / 2);
        }
        public void initResultGrid()
        {
            int i = 0;

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

        private void SetTestControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Globalo.visionManager.milLibrary.RunModeChange(false);
                Globalo.visionManager.ChangeSettingDisplayHandle(0, Set_panelCam);

                Globalo.visionManager.milLibrary.ClearOverlay(0);
                int cx = Globalo.visionManager.milLibrary.CAM_SIZE_X;
                int cy = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
                Globalo.visionManager.milLibrary.DrawOverlayCross(0, cx / 2, cy / 2, 1000, Color.Yellow, 1, System.Drawing.Drawing2D.DashStyle.Solid);
            }
        }
        #region [TOP CAMERA MANUAL TEST]
        private void button_Set_Key_Test_Click(object sender, EventArgs e)
        {
            bool rtn = true;

            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
            int dataSize = sizeX * sizeY;


            byte[] ImageBuffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[CamIndex], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);


            Globalo.visionManager.aoiTopTester.Keytest(CamIndex, src, centerPos);        //키검사
        }

        private void button_Set_Housing_Test_Click(object sender, EventArgs e)
        {
            bool rtn = true;

            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
            int dataSize = sizeX * sizeY;


            byte[] ImageBuffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[CamIndex], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);



            centerPos = Globalo.visionManager.aoiTopTester.Housing_Dent_Test(CamIndex, src); //Con1,2(동심도)  / Dent (찌그러짐) 검사 
        }

        private void button_Set_Gasket_Test_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
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

        }

        private void button_Set_Cone_Test_Click(object sender, EventArgs e)
        {

        }

        private void button_Set_Height_Test_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
            int dataSize = sizeX * sizeY;


            byte[] ImageBuffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[CamIndex], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);

            //Globalo.visionManager.aoiSideTester.HeightTest(CamIndex, src);     //
            //Globalo.visionManager.aoiSideTester.MilHeightTest(CamIndex, src);     //
            Globalo.visionManager.aoiSideTester.MilEdgeHeightTest(CamIndex, src);     //
        }
        #endregion

        private void button_Pogo_Find_Test_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X;
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y;
            int dataSize = sizeX * sizeY;

            byte[] ImageBuffer = new byte[dataSize];

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[CamIndex], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);


            Globalo.visionManager.aoiTopTester.FindPogoPinCenter(CamIndex, src);     //가스켓 검사
        }
        #region [MOUSE DRAW]

        
        private void Set_panelCam_MouseDown(object sender, MouseEventArgs e)
        {
            int iGap = 20;
            if (e.Button == MouseButtons.Left)
            {
                if (m_bDrawMeasureLine == true)
                {
                    moveStartMousePos = e.Location;
                    resizeDir = GetResizeDirection(1, e.Location);
                    return;
                }
                if (DrawRoiBox != null)
                {
                    resizeDir = GetResizeDirection(0, e.Location);
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
                    ResizeDirection hoverDir = GetResizeDirection(1, e.Location);
                    Cursor.Current = GetCursorByResizeDirection(hoverDir);
                }
                else if (isDragging == false)
                {
                    ResizeDirection hoverDir = GetResizeDirection(0, e.Location);
                    Cursor.Current = GetCursorByResizeDirection(hoverDir);
                }
            }
            if (m_bDrawMeasureLine && e.Button == MouseButtons.Left)
            {
                int dx = (int)((e.X - moveStartMousePos.X) * Globalo.visionManager.milLibrary.xExpand + 0.5);
                int dy = (int)((e.Y - moveStartMousePos.Y) * Globalo.visionManager.milLibrary.yExpand + 0.5);

                switch (resizeDir)
                {
                    case ResizeDirection.Top:
                        DistLineX[0].Y += dy;
                        break;
                    case ResizeDirection.Left:
                        DistLineX[0].X += dx;
                        break;
                    case ResizeDirection.Right:
                        DistLineX[1].X += dx;
                        break;

                    case ResizeDirection.Bottom:
                        DistLineX[1].Y += dy;
                        break;


                }
                moveStartMousePos = e.Location;
                DrawDistnace();
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
                    (int)(DrawRoiBox.X * Globalo.visionManager.milLibrary.xExpand + 0.5), (int)(DrawRoiBox.Y * Globalo.visionManager.milLibrary.yExpand + 0.5),
                    (int)(DrawRoiBox.Width * Globalo.visionManager.milLibrary.xExpand + 0.5), (int)(DrawRoiBox.Height * Globalo.visionManager.milLibrary.yExpand + 0.5));
                Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);
                Globalo.visionManager.milLibrary.DrawOverlayBox(CamIndex, m_clRect, Color.Blue, 1, System.Drawing.Drawing2D.DashStyle.Solid);
            }
            else if (isDragging)
            {
                roiEnd = e.Location;
                Rectangle roi = GetRoiRect(roiStart, roiEnd);
                Rectangle m_clRect = new Rectangle((int)(roi.X * Globalo.visionManager.milLibrary.xExpand + 0.5), (int)(roi.Y * Globalo.visionManager.milLibrary.yExpand + 0.5),
                    (int)(roi.Width * Globalo.visionManager.milLibrary.xExpand + 0.5), (int)(roi.Height * Globalo.visionManager.milLibrary.yExpand + 0.5));

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
                    (int)(DrawRoiBox.X * Globalo.visionManager.milLibrary.xExpand + 0.5), (int)(DrawRoiBox.Y * Globalo.visionManager.milLibrary.yExpand + 0.5),
                    (int)(DrawRoiBox.Width * Globalo.visionManager.milLibrary.xExpand + 0.5), (int)(DrawRoiBox.Height * Globalo.visionManager.milLibrary.yExpand + 0.5));
                Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);
                Globalo.visionManager.milLibrary.DrawOverlayBox(CamIndex, m_clRect, Color.Blue, 1, System.Drawing.Drawing2D.DashStyle.Solid);
            }
        }
        private void Set_panelCam_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
            if (isDragging)
            {
                isDragging = false;
                resizeDir = ResizeDirection.None;
                DrawRoiBox = GetRoiRect(roiStart, roiEnd);

                roiEnd = e.Location;

                int dragw = (int)(DrawRoiBox.Width * Globalo.visionManager.milLibrary.xExpand + 0.5);
                int dragh = (int)(DrawRoiBox.Height * Globalo.visionManager.milLibrary.yExpand + 0.5);
                Console.WriteLine($"Drag Roi = w:{dragw},h:{dragh}");
            }

            isMovingRoi = false;
        }
        //--------------------------------------------------------------------------------------
        //
        //ALIGN CAM
        //
        //--------------------------------------------------------------------------------------
        private MOUSE_CURSOR checkMousePos(System.Drawing.Point p, Rectangle rcTemp, int DisplayMode)
        {
            int iGap = 0;
            MOUSE_CURSOR iRtn = MOUSE_CURSOR.MOVE_ALL;

            double dExpandFactorX = 0.0;
            double dExpandFactorY = 0.0;
            dExpandFactorX = Globalo.visionManager.milLibrary.xExpand;
            dExpandFactorY = Globalo.visionManager.milLibrary.yExpand;

            iGap = 20;// (int)(dExpandFactorX * 3);

            System.Drawing.Point point = p;

            point.X = (int)(point.X * dExpandFactorX + 0.5);
            point.Y = (int)(point.Y * dExpandFactorY + 0.5);


            //박스 이동
            if (point.X > rcTemp.Left + iGap &&
                point.X < rcTemp.Right - iGap &&
                point.Y > rcTemp.Top + iGap &&
                point.Y < rcTemp.Bottom - iGap)
            {
                iRtn = MOUSE_CURSOR.MOVE_ALL;
            }
            // 좌 크기
            else if (point.Y > rcTemp.Top + iGap && point.Y < rcTemp.Bottom - iGap && point.X > rcTemp.Left - iGap && point.X < rcTemp.Left + iGap)
            {
                iRtn = MOUSE_CURSOR.MOVE_WIDTH_LEFT;
            }
            // 우 크기
            else if (point.Y > rcTemp.Top + iGap && point.Y < rcTemp.Bottom - iGap && point.X > rcTemp.Right - iGap && point.X < rcTemp.Right + iGap)
            {
                iRtn = MOUSE_CURSOR.MOVE_WIDTH_RIGHT;
            }
            // 상 크기
            else if (point.X > rcTemp.Left + iGap && point.X < rcTemp.Right - iGap && point.Y > rcTemp.Top - iGap && point.Y < rcTemp.Top + iGap)
            {
                iRtn = MOUSE_CURSOR.MOVE_HEIGHT_TOP;
            }
            // 하 크기
            else if (point.X > rcTemp.Left + iGap && point.X < rcTemp.Right - iGap && point.Y > rcTemp.Bottom - iGap && point.Y < rcTemp.Bottom + iGap)
            {
                iRtn = MOUSE_CURSOR.MOVE_HEIGHT_BOTTOM;
            }
            // 좌상 크기
            else if (point.X > rcTemp.Left - iGap && point.X < rcTemp.Left + iGap && point.Y > rcTemp.Top - iGap && point.Y < rcTemp.Top + iGap)
            {
                iRtn = MOUSE_CURSOR.MOVE_NW;
            }
            // 우상 크기
            else if (point.X > rcTemp.Right - iGap && point.X < rcTemp.Right + iGap && point.Y > rcTemp.Top - iGap && point.Y < rcTemp.Top + iGap)
            {
                iRtn = MOUSE_CURSOR.MOVE_NE;
            }
            // 좌하 크기
            else if (point.X > rcTemp.Left - iGap && point.X < rcTemp.Left + iGap && point.Y > rcTemp.Bottom - iGap && point.Y < rcTemp.Bottom + iGap)
            {
                iRtn = MOUSE_CURSOR.MOVE_SW;
            }
            // 우하 크기
            else if (point.X > rcTemp.Right - iGap && point.X < rcTemp.Right + iGap && point.Y > rcTemp.Bottom - iGap && point.Y < rcTemp.Bottom + iGap)
            {
                iRtn = MOUSE_CURSOR.MOVE_SE;
            }
            else
            {
            }

            return iRtn;
        }
        #endregion

        private void checkBox_Measure_CheckedChanged(object sender, EventArgs e)
        {
            m_bDrawMeasureLine = false;
            if (checkBox_Measure.Checked)
            {
                m_bDrawMeasureLine = true;
                DrawDistnace();
            }
            else
            {
                Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);
            }
        }
        private void DrawDistnace()
        {

            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);

            //DistLineX[0] = new System.Drawing.Point(500, 500);
            //DistLineX[1] = new System.Drawing.Point(sizeX - 500, sizeY - 500);

            Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);
            Globalo.visionManager.milLibrary.DrawOverlayLine(0, (int)(DistLineX[0].X), 0, (int)(DistLineX[0].X), (int)Globalo.visionManager.milLibrary.CAM_SIZE_Y, Color.Red, 1);
            Globalo.visionManager.milLibrary.DrawOverlayLine(0, (int)(DistLineX[1].X), 0, (int)(DistLineX[1].X), (int)Globalo.visionManager.milLibrary.CAM_SIZE_Y, Color.Red, 1);

            Globalo.visionManager.milLibrary.DrawOverlayLine(0, 0, (int)(DistLineX[0].Y), (int)Globalo.visionManager.milLibrary.CAM_SIZE_X, (int)(DistLineX[0].Y), Color.Blue, 1);
            Globalo.visionManager.milLibrary.DrawOverlayLine(0, 0, (int)(DistLineX[1].Y), (int)Globalo.visionManager.milLibrary.CAM_SIZE_X, (int)(DistLineX[1].Y), Color.Blue, 1);

            Globalo.visionManager.CamResol.X = 0.02026f;
            Globalo.visionManager.CamResol.Y = 0.02026f;//0.0288f;

            System.Drawing.Point textPoint;

            string str = $"[Distance x: = {Math.Abs(DistLineX[0].X - DistLineX[1].X) * Globalo.visionManager.CamResol.X}";
            textPoint = new System.Drawing.Point(10, CamH - 250);
            Globalo.visionManager.milLibrary.DrawOverlayText(CamIndex, textPoint, str, Color.Green, 15);

            str = $"[Distance y: = {Math.Abs(DistLineX[0].Y - DistLineX[1].Y) * Globalo.visionManager.CamResol.Y}";
            textPoint = new System.Drawing.Point(10, CamH - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(CamIndex, textPoint, str, Color.Green, 15);

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
        private ResizeDirection GetResizeDirection(int Mode, System.Drawing.Point mousePos)
        {
            Rectangle r;
            if (Mode == 0)
            {
                r = DrawRoiBox;
                // 9개 위치
                Rectangle left = new Rectangle(r.Left - HANDLE_SIZE / 2, r.Top + HANDLE_SIZE, HANDLE_SIZE, r.Height - HANDLE_SIZE / 2);
                Rectangle right = new Rectangle(r.Right - HANDLE_SIZE / 2, r.Top + HANDLE_SIZE, HANDLE_SIZE, r.Height - HANDLE_SIZE / 2);
                Rectangle top = new Rectangle(r.Left + HANDLE_SIZE / 2, r.Top - HANDLE_SIZE / 2, r.Width - HANDLE_SIZE / 2, HANDLE_SIZE);
                Rectangle bottom = new Rectangle(r.Left + HANDLE_SIZE / 2, r.Bottom - HANDLE_SIZE / 2, r.Width - HANDLE_SIZE / 2, HANDLE_SIZE);
                //
                Rectangle topLeft = new Rectangle(r.Left - HANDLE_SIZE / 2, r.Top - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE);
                Rectangle topRight = new Rectangle(r.Right - HANDLE_SIZE / 2, r.Top - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE);
                Rectangle bottomRight = new Rectangle(r.Right - HANDLE_SIZE / 2, r.Bottom - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE);
                Rectangle bottomLeft = new Rectangle(r.Left - HANDLE_SIZE / 2, r.Bottom - HANDLE_SIZE / 2, HANDLE_SIZE, HANDLE_SIZE);

                Rectangle MoveRect = new Rectangle(r.Left + HANDLE_SIZE / 2, r.Top + HANDLE_SIZE / 2, r.Width - HANDLE_SIZE, r.Height - HANDLE_SIZE);

                if (topLeft.Contains(mousePos)) return ResizeDirection.TopLeft;
                if (topRight.Contains(mousePos)) return ResizeDirection.TopRight;
                if (bottomRight.Contains(mousePos)) return ResizeDirection.BottomRight;
                if (bottomLeft.Contains(mousePos)) return ResizeDirection.BottomLeft;
                if (top.Contains(mousePos)) return ResizeDirection.Top;
                if (right.Contains(mousePos)) return ResizeDirection.Right;
                if (bottom.Contains(mousePos)) return ResizeDirection.Bottom;
                if (left.Contains(mousePos)) return ResizeDirection.Left;
                if (MoveRect.Contains(mousePos)) return ResizeDirection.Move;
            }
            else
            {
                //(int)(DrawRoiBox.X * Globalo.visionManager.milLibrary.xExpand + 0.5)
                Rectangle x1Line = new Rectangle((int)(DistLineX[0].X * Globalo.visionManager.milLibrary.xReduce + 0.5) - LINE_HIT_MARGIN /2, 0, LINE_HIT_MARGIN, (int)(CamH * Globalo.visionManager.milLibrary.yReduce +0.5));
                Rectangle x2Line = new Rectangle((int)(DistLineX[1].X * Globalo.visionManager.milLibrary.xReduce + 0.5) - LINE_HIT_MARGIN / 2, 0, LINE_HIT_MARGIN, (int)(CamH * Globalo.visionManager.milLibrary.yReduce + 0.5));

                Rectangle y1Line = new Rectangle(0, (int)(DistLineX[0].Y * Globalo.visionManager.milLibrary.yReduce + 0.5) - LINE_HIT_MARGIN / 2, (int)(CamW * Globalo.visionManager.milLibrary.xReduce + 0.5), LINE_HIT_MARGIN);
                Rectangle y2Line = new Rectangle(0, (int)(DistLineX[1].Y * Globalo.visionManager.milLibrary.yReduce + 0.5) - LINE_HIT_MARGIN / 2, (int)(CamW * Globalo.visionManager.milLibrary.xReduce + 0.5), LINE_HIT_MARGIN);

                if (y1Line.Contains(mousePos)) return ResizeDirection.Top;
                if (y2Line.Contains(mousePos)) return ResizeDirection.Bottom;

                if (x1Line.Contains(mousePos)) return ResizeDirection.Left;
                if (x2Line.Contains(mousePos)) return ResizeDirection.Right;

            }
            

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
    }
}
