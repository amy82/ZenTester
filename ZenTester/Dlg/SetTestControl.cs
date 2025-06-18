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

namespace ZenTester.Dlg
{
    public partial class SetTestControl : UserControl
    {
        public enum eManualBtn : int
        {
            TestTab = 0, ConfigTab
        };
        public enum ResizeDirection
        {
            None,
            TopLeft, Top, TopRight, Right, BottomRight, Bottom, BottomLeft, Left,
            Move
        }

        private eManualBtn manualBtnTab;

        public Dlg.ManualTest manualTest;
        public Dlg.ManualConfig manualConfig;
        public int CamIndex = 0;
        const int HANDLE_SIZE = 10;
        const int LINE_THICKNESS = 2;
        const int LINE_HIT_MARGIN = 10;
        private System.Drawing.Point roiStart;
        private System.Drawing.Point roiEnd;
        public Bitmap CurrentImage { get; set; }

        
        public System.Drawing.Point m_clClickPoint = new System.Drawing.Point();
        public Rectangle m_rSetCamBox = new Rectangle();

        
        private bool isDragging = false;
        private bool isMovingRoi= false;
        private ResizeDirection resizeDir = ResizeDirection.None;
        private bool isResizing = false;
        public int isRoiChecked = -1;
        public int isRoiNo = -1;

        private System.Drawing.Point moveStartMousePos;     // 마우스가 눌린 위치
        private System.Drawing.Point moveStartRoiPos;       // ROI 원래 위치

        public System.Drawing.Point[,] DistLineX = new System.Drawing.Point[2, 2];

        private bool m_bDrawFlag = false;
        private bool m_bBoxMoveFlag = false;
        public bool m_bDrawMeasureLine = false;

        private Rectangle DrawRoiBox;

        public List<Data.Roi> tempRoi = new List<Data.Roi>();


        public int[] CamW = new int[2];
        public int[] CamH = new int[2];

        public OpenCvSharp.Point[] centerPos = new OpenCvSharp.Point[2];
        private OpenCvSharp.Point[] TopCenterPos = new OpenCvSharp.Point[2];
        
        
        

        
        public SetTestControl()
        {
            InitializeComponent();

            manualBtnTab = eManualBtn.TestTab;

            manualTest = new ManualTest(this);
            manualConfig = new ManualConfig(this);

            manualTest.Visible = true;
            manualConfig.Visible = false;

            this.Controls.Add(manualTest);
            this.Controls.Add(manualConfig);

            manualTest.Location = new System.Drawing.Point(1050, 50);
            manualConfig.Location = new System.Drawing.Point(1050, 50);


            button_SetTest_SideCam.BackColor = Color.DarkGray;
            tempRoi.Clear();

            
        }
        public void setCamCenter()
        {
            int i = 0;
            for (i = 0; i < 2; i++)
            {
                CamW[i] = Globalo.visionManager.milLibrary.CAM_SIZE_X[i];
                CamH[i] = Globalo.visionManager.milLibrary.CAM_SIZE_Y[i];

                centerPos[i].X = (int)(CamW[i] / 2);
                centerPos[i].Y = (int)(CamH[i] / 2);

                DistLineX[i,0] = new System.Drawing.Point(500, 500);
                DistLineX[i,1] = new System.Drawing.Point(CamW[i] - 500, CamH[i] - 500);
            }

            manualConfig.showCamResol();
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
            //Globalo.visionManager.milLibrary.ClearOverlay(0);
            //Globalo.visionManager.milLibrary.DrawOverlayText(0, new System.Drawing.Point(100,100), "overlay test", Color.Yellow, 30);

            //Globalo.visionManager.milLibrary.DrawOverlayArrow(0, 500, 500 , 500, 1500, Color.Yellow, 2, System.Drawing.Drawing2D.DashStyle.Solid);

            //Globalo.visionManager.aoiTester.CirCleFind(0);

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
        }

        private void btn_SideCam_Image_Save_Click(object sender, EventArgs e)
        {
            //Globalo.visionManager.milLibrary.ClearOverlay(1);  
            //Globalo.visionManager.milLibrary.DrawOverlayText(1, new System.Drawing.Point(1500, 200), "overlay test1", Color.Yellow, 30); 

            //Rectangle m_clRect = new Rectangle((int)(100), (int)(100), 1000, 1000);
            //Globalo.visionManager.milLibrary.DrawOverlayBox(1, m_clRect, Color.Blue, 1, System.Drawing.Drawing2D.DashStyle.Dot);
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

            button_SetTest_TopCam.BackColor = Color.Black;
            button_SetTest_SideCam.BackColor = Color.DarkGray;
            CamIndex = 0;
            Globalo.visionManager.ChangeSettingDisplayHandle(CamIndex, Set_panelCam);
            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, true);
            manualConfig.checkBox_AllRelease();
        }

        private void button_SetTest_SideCam_Click(object sender, EventArgs e)
        {
            button_SetTest_SideCam.BackColor = Color.Black;
            button_SetTest_TopCam.BackColor = Color.DarkGray;
            CamIndex = 1;
            Globalo.visionManager.ChangeSettingDisplayHandle(CamIndex, Set_panelCam);

            Globalo.visionManager.milLibrary.SetGrabOn(CamIndex, true);
            manualConfig.checkBox_AllRelease();

            //roi 다시 그리기
        }

        

        private void SetTestControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Globalo.visionManager.milLibrary.RunModeChange(false);
                Globalo.visionManager.ChangeSettingDisplayHandle(CamIndex, Set_panelCam);
            }
            else
            {
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
                        DistLineX[CamIndex, 0].Y += dy;
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
                manualConfig.DrawDistnace();
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

                
                manualConfig.drawTestRoi(isRoiChecked);
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
                Rectangle roi = GetRoiRect();//roiStart, roiEnd);
                Rectangle m_clRect = new Rectangle((int)(roi.X * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5), (int)(roi.Y * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5),
                    (int)(roi.Width * Globalo.visionManager.milLibrary.xExpand[CamIndex] + 0.5), (int)(roi.Height * Globalo.visionManager.milLibrary.yExpand[CamIndex] + 0.5));

                Globalo.visionManager.milLibrary.ClearOverlay(CamIndex);
                Globalo.visionManager.milLibrary.DrawOverlayBox(CamIndex, m_clRect, Color.Blue, 1, System.Drawing.Drawing2D.DashStyle.Solid);

                //Set_panelCam.Invalidate();
                // 마우스 왼쪽 버튼이 눌린 상태에서 이동 중
                //Console.WriteLine($"드래그 중: X={e.X}, Y={e.Y}");
                // ROI 박스 그리기 등 처리
            }
            else if (isMovingRoi)
            {
                // 마우스 이동량 계산
                int dx = e.X - moveStartMousePos.X;
                int dy = e.Y - moveStartMousePos.Y;

                //Console.WriteLine($"드래그 중: dx={dx}, dy={dy}");
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
                DrawRoiBox = GetRoiRect();//roiStart, roiEnd);

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

        
        

        public Rectangle GetRoiRect()//System.Drawing.Point p1, System.Drawing.Point p2)
        {
            return new Rectangle(
                Math.Min(roiStart.X, roiEnd.X),
                Math.Min(roiStart.Y, roiEnd.Y),
                Math.Abs(roiEnd.X - roiStart.X),
                Math.Abs(roiEnd.Y - roiStart.Y)
            );
        }
        private ResizeDirection GetDistDirection(System.Drawing.Point mousePos)
        {
            //거리측정
            //(int)(DrawRoiBox.X * Globalo.visionManager.milLibrary.xExpand + 0.5)
            Rectangle x1Line = new Rectangle((int)(DistLineX[CamIndex, 0].X * Globalo.visionManager.milLibrary.xReduce[CamIndex] + 0.5) - LINE_HIT_MARGIN / 2, 0, LINE_HIT_MARGIN, (int)(CamH[CamIndex] * Globalo.visionManager.milLibrary.yReduce[CamIndex] + 0.5));
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
        
        private void SetBtnChange(eManualBtn index)
        {
            btn_Set_Test_Control.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            btn_Set_Config_Control.BackColor = ColorTranslator.FromHtml("#E1E0DF");
            manualBtnTab = index;

            if (manualBtnTab == eManualBtn.TestTab)
            {
                btn_Set_Test_Control.BackColor = ColorTranslator.FromHtml("#FFB230");
                manualTest.Visible = true;
                manualConfig.Visible = false;

            }
            else
            {
                btn_Set_Config_Control.BackColor = ColorTranslator.FromHtml("#FFB230");
                manualConfig.Visible = true;
                manualTest.Visible = false;
            }
        }

        private void btn_Set_Test_Control_Click(object sender, EventArgs e)
        {
            SetBtnChange(eManualBtn.TestTab);
        }

        private void btn_Set_Config_Control_Click(object sender, EventArgs e)
        {
            SetBtnChange(eManualBtn.ConfigTab);
        }
    }
}
