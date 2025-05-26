using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;


namespace ZenHandler.VisionClass
{
    public class MilLibraryUtil
    {
        public MIL_ID MilApplication = MIL.M_NULL;         // Application identifier.
        public MIL_ID[] MilDigitizerList;
        public MIL_ID MilSystem = MIL.M_NULL;              // System identifier.
        public int TotalCamCount = 0;
        public int CamFixCount = 2;
        private bool AutoRunMode = true;
        public bool[] bGrabOnFlag = new bool[2];

        //GRAB IMAGE
        public MIL_ID[] MilCamGrabImage;
        public MIL_ID[] MilCamGrabImageChild;
        public MIL_ID[] MilProcImage;
        public MIL_ID[] MilProcImageChild;
        //AUTO CAMERA
        public MIL_ID[] MilCamDisplay;                        // Display identifier.
        public MIL_ID[] MilCamSmallImage;
        public MIL_ID[] MilCamSmallImageChild;
        public MIL_ID[] MilCamOverlay;
        public MIL_INT[] MilCamTransparent;


        //MANUAL SET CAMERA     Set Panel Buffer
        public MIL_ID MilSetCamDisplay;                        // Display identifier.
        public MIL_ID[] MilSetCamSmallImage;
        public MIL_ID[] MilSetCamSmallImageChild;

        public MIL_ID MilSetCamOverlay;
        public MIL_INT MilSetCamTransparent;

        public CMilDrawBox[] m_clMilDrawBox = new CMilDrawBox[2];
        public CMilDrawCircle[] m_clMilDrawCircle = new CMilDrawCircle[2];
        public CMilDrawText[] m_clMilDrawText = new CMilDrawText[2];
        public CMilDrawCross[] m_clMilDrawCross = new CMilDrawCross[2];
        //Overlay
        public MIL_ID[] MilCamOverlayImage;

        public int[] CAM_SIZE_X = new int[2];
        public int[] CAM_SIZE_Y = new int[2];
        private MIL_INT[] m_nMilSizeX = new MIL_INT[2];
        private MIL_INT[] m_nMilSizeY = new MIL_INT[2];

        private int CamControlWidth = 100;          //픽처 컨트롤 가로 사이즈 , 보여지는 사이즈
        private int CamControlHeight = 100;         //픽처 컨트롤 세로 사이즈 , 보여지는 사이즈
        private int SetCamControlWidth = 100;       //세팅용 픽처 컨트롤 가로 사이즈 , 보여지는 사이즈
        private int SetCamControlHeight = 100;      //세팅용 픽처 컨트롤 세로 사이즈 , 보여지는 사이즈

        public double[] xReduce = new double[2];
        public double[] yReduce = new double[2];
        public double[] xExpand = new double[2];
        public double[] yExpand = new double[2];



        public MilLibraryUtil()
        {
            int i = 0;
            MilDigitizerList = new MIL_ID[CamFixCount];
            MilDigitizerList[0] = MIL.M_NULL;
            MilDigitizerList[1] = MIL.M_NULL;

            for (i = 0; i < 2; i++)
            {
                m_clMilDrawBox[i] = new CMilDrawBox();
                m_clMilDrawCircle[i] = new CMilDrawCircle();
                m_clMilDrawText[i] = new CMilDrawText();
                m_clMilDrawCross[i] = new CMilDrawCross();

                CAM_SIZE_X[i] = 1024;
                CAM_SIZE_Y[i] = 768;

                xReduce[i] = 0.0;
                yReduce[i] = 0.0;
                xExpand[i] = 0.0;
                yExpand[i] = 0.0;

            }
            
        }

        public void RunModeChange(bool flag)
        {
            AutoRunMode = flag;
        }
        public void setCamSize(int index, int PanelW , int PanelH)
        {
            xReduce[index] = ((double)PanelW / (double)CAM_SIZE_X[index]);
            yReduce[index] = ((double)PanelH / (double)CAM_SIZE_Y[index]);

            xExpand[index] = ((double)CAM_SIZE_X[index] / (double)PanelW);
            yExpand[index] = ((double)CAM_SIZE_Y[index] / (double)PanelH);
        }

        //자동운전시 Cam1 , Cam2
        //세팅 화면 - Cam 1 - 2
        public void SelectDisplay(int index, IntPtr handle, int _w, int _h)
        {
            setCamSize(index, _w, _h);
            MIL.MdispSelectWindow(MilCamDisplay[index], MilCamSmallImageChild[index], handle);
        }

        public void SelectSetDisplay(int index, IntPtr handle, int _w, int _h)
        {
            setCamSize(index, _w, _h);
            MIL.MdispSelectWindow(MilSetCamDisplay, MilSetCamSmallImageChild[index], handle);
            //MIL.MdispSelect(MilSetCamDisplay, MilSetCamSmallImageChild[index]);        //여기에 하면 mil display로 나온다.
        }

        public void GrabImageSave(int index, string path)
        {
            MIL.MbufExport(path, MIL.M_BMP, Globalo.visionManager.milLibrary.MilCamGrabImageChild[index]);
        }

        public void SetGrabOn(int index, bool bGrab)
        {
            bGrabOnFlag[index] = bGrab;
        }

        public void GetSnapImage(int index)
        {
            MIL.MbufCopy(Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], Globalo.visionManager.milLibrary.MilProcImageChild[index]);
        }

        public void setCamImage(int index, string filePath)
        {
            MIL.MbufLoad(filePath, MilCamGrabImage[index]);
            MIL.MimResize(MilCamGrabImageChild[index], MilCamSmallImageChild[index], xReduce[index], yReduce[index], MIL.M_DEFAULT);
        }
        public void ClearOverlay(int index)
        {
            m_clMilDrawBox[index].RemoveAll();
            m_clMilDrawCircle[index].RemoveAll();
            m_clMilDrawText[index].RemoveAll();
            m_clMilDrawCross[index].RemoveAll();
            if (AutoRunMode)
            {
                MIL.MbufClear(MilCamOverlay[index], MilCamTransparent[index]);
            }
            else
            {
                MIL.MbufClear(MilSetCamOverlay, MilSetCamTransparent);
            }
                
        }
        public void DrawRgbValue(int index, Point clickP)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(index);
            int width = (int)MIL.MbufInquire(MilCamGrabImageChild[index], MIL.M_PITCH, MIL.M_NULL);
            int pos = clickP.Y * width + clickP.X;
            int pixelValue = 0;
            byte[] pixelRGB = new byte[3];
            MIL.MbufGet2d(MilCamGrabImageChild[index], (int)(clickP.X * xExpand[index]), (int)(clickP.Y * yExpand[index]), 1, 1, pixelRGB);

            int cx = (int)(clickP.X * xExpand[index] + 0.5);
            int cy = (int)(clickP.Y * yExpand[index] + 0.5);

            string str = $"[X={cx}, Y={cy}] Gray Value: {pixelRGB[0]}";
            Console.WriteLine($"[X,Y={cx},{cy}] Gray Value: {pixelRGB[0]}");

            System.Drawing.Point textPoint = new System.Drawing.Point(10, 10);
            Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, str, Color.Green, 15);
            
            Globalo.visionManager.milLibrary.DrawOverlayCross(0, cx, cy, 300, Color.Blue, 1, System.Drawing.Drawing2D.DashStyle.Solid);

            /*
                     p.x = (int)(m_ClickP.x * CAM_EXPAND_FACTOR_X + 0.5);
			        p.y = (int)(m_ClickP.y * CAM_EXPAND_FACTOR_Y + 0.5);

			        vision.crosslist[m_iCurCamNo].addList(p, 30, M_COLOR_RED);
			        MbufCopy(vision.MilGrabImageChild[m_iCurCamNo], vision.MilProcImageChild[m_iCurCamNo]);
			        width = MbufInquire(vision.MilProcImageChild[m_iCurCamNo], M_PITCH, M_NULL);
			        pos = p.y * width + p.x;
			        val = vision.MilImageBuffer[m_iCurCamNo][pos];

			        sprintf_s(szTmp, "(%d, %d) %d", p.x, p.y, val);
			        vision.textlist[m_iCurCamNo].addList(50, 700, szTmp, M_COLOR_RED, 17, 7, "Arial"); 
                     */
        }
        public void MilGrabRun(int index)
        {
            if (bGrabOnFlag[index] == true && MilDigitizerList[index] != MIL.M_NULL)
            {
                MIL.MdigGrab(MilDigitizerList[index], MilCamGrabImage[index]);
                MIL.MdigGrabWait(MilDigitizerList[index], MIL.M_GRAB_END);
                if (AutoRunMode)
                {
                    MIL.MimResize(MilCamGrabImageChild[index], MilCamSmallImageChild[index], xReduce[index], yReduce[index], MIL.M_DEFAULT);
                }
                else
                {
                    MIL.MimResize(MilCamGrabImageChild[index], MilSetCamSmallImageChild[index], xReduce[index], yReduce[index], MIL.M_DEFAULT);
                }
                //MIL.MbufExport($"d:\\grab{index}.BMP", MIL.M_BMP, MilCamGrabImage[index]);

            }

        }

        public void MilNewGrabRun(int index)
        {
            if (bGrabOnFlag[1] == true && MilDigitizerList[1] != MIL.M_NULL)
            {
                MIL.MdigGrab(MilDigitizerList[1], MilCamGrabImage[1]);

                MIL.MdigGrabWait(MilDigitizerList[1], MIL.M_GRAB_END);

                MIL.MimResize(MilCamGrabImageChild[1], MilCamSmallImageChild[1], xReduce[1], yReduce[1], MIL.M_DEFAULT);
                Thread.Sleep(1);
            }

        }
        public void EnableCamOverlay(int index)
        {
            MIL_INT DisplayType = MIL.MdispInquire(MilCamDisplay[index], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

            if (DisplayType == (MIL.M_WINDOWED | MIL.M_USER_WINDOW))
            {
                MIL.MdispControl(MilCamDisplay[index], MIL.M_OVERLAY, MIL.M_ENABLE);
                MIL.MdispControl(MilCamDisplay[index], MIL.M_OVERLAY_SHOW, MIL.M_ENABLE);
                MIL.MdispInquire(MilCamDisplay[index], MIL.M_OVERLAY_ID, ref MilCamOverlay[index]);
                MIL.MdispControl(MilCamDisplay[index], MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);

                MilCamTransparent[index] = MIL.MdispInquire(MilCamDisplay[index], MIL.M_TRANSPARENT_COLOR, MIL.M_NULL);
                //MIL.MdispInquire(MilCamDisplay[index], MIL.M_TRANSPARENT_COLOR, ref MilCamTransparent[index]);
                MIL.MbufClear(MilCamOverlay[index], MilCamTransparent[index]);
                MIL.MgraControl(MIL.M_DEFAULT, MIL.M_BACKGROUND_MODE, MIL.M_TRANSPARENT);
            }
        }

        public void EnableSetCamOverlay()
        {
            MIL_INT DisplayType = MIL.MdispInquire(MilSetCamDisplay, MIL.M_DISPLAY_TYPE, MIL.M_NULL);

            if (DisplayType == (MIL.M_WINDOWED | MIL.M_USER_WINDOW))
            {
                MIL.MdispControl(MilSetCamDisplay, MIL.M_OVERLAY, MIL.M_ENABLE);
                MIL.MdispControl(MilSetCamDisplay, MIL.M_OVERLAY_SHOW, MIL.M_ENABLE);
                MIL.MdispInquire(MilSetCamDisplay, MIL.M_OVERLAY_ID, ref MilSetCamOverlay);
                MIL.MdispControl(MilSetCamDisplay, MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);

                MilSetCamTransparent = MIL.MdispInquire(MilSetCamDisplay, MIL.M_TRANSPARENT_COLOR, MIL.M_NULL);
                MIL.MbufClear(MilSetCamOverlay, MilSetCamTransparent);
                MIL.MgraControl(MIL.M_DEFAULT, MIL.M_BACKGROUND_MODE, MIL.M_TRANSPARENT);
            }
        }
        
        public void drawTest(MIL_ID display, int index)
        {
            //display
            MIL.MmodControl(display, MIL.M_DEFAULT, 3203L, xReduce[index]);//M_DRAW_SCALE_X
            MIL.MmodControl(display, MIL.M_DEFAULT, 3204L, yReduce[index]);//M_DRAW_SCALE_Y
            MIL.MmodDraw(MIL.M_DEFAULT, display, Globalo.visionManager.milLibrary.MilCamOverlay[index], MIL.M_DRAW_BOX, MIL.M_DEFAULT, MIL.M_DEFAULT);
        }

        #region [OVERLAY DRAW]
        public void DrawOverlayAll(int index, int DisplayMode)
        {
            MIL_ID tempOverlay = MIL.M_NULL;
            if (AutoRunMode)
            {
                tempOverlay = MilCamOverlay[index];
            }
            else
            {
                tempOverlay = MilSetCamOverlay;
            }
            IntPtr hOverlayDC = IntPtr.Zero;

            MIL.MbufControl(tempOverlay, MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(tempOverlay, MIL.M_DC_HANDLE, MIL.M_NULL);


            m_clMilDrawBox[index].Draw(hOverlayDC, xReduce[index], yReduce[index]);
            m_clMilDrawCircle[index].Draw(hOverlayDC, xReduce[index], yReduce[index]);
            m_clMilDrawText[index].Draw(hOverlayDC, xReduce[index], yReduce[index]);
            m_clMilDrawCross[index].Draw(hOverlayDC, xReduce[index], yReduce[index]);

            MIL.MbufControl(tempOverlay, MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(tempOverlay, MIL.M_MODIFIED, MIL.M_DEFAULT);
        }
        public void DrawOverlayCross(int index, int x, int y,  int lineWid , Color color, int nWid, DashStyle nStyles)
        {
            IntPtr hOverlayDC = IntPtr.Zero;
            MIL_ID tempOverlay = MIL.M_NULL;
            if (AutoRunMode)
            {
                tempOverlay = MilCamOverlay[index];
            }
            else
            {
                tempOverlay = MilSetCamOverlay;
            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(tempOverlay, MIL.M_DC_HANDLE, MIL.M_NULL);

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    using (Pen arrowPen = new Pen(color, nWid))
                    {
                        double xResoul = xReduce[index];
                        double yResoul = yReduce[index];

                        int _wid = (int)(lineWid * xResoul + 0.5);
                        int _hei = (int)(lineWid * yResoul + 0.5);

                        int startX = (int)(x * xResoul + 0.5);
                        int startY = (int)(y * yResoul + 0.5);

                        DrawingGraphics.DrawLine(arrowPen, startX - _wid, startY, startX + _wid, startY);
                        DrawingGraphics.DrawLine(arrowPen, startX, startY - _wid, startX, startY + _wid);
                    }
                }

            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(tempOverlay, MIL.M_MODIFIED, MIL.M_DEFAULT);
        }
        public void DrawOverlayLine(int index, int x1, int y1, int x2, int y2, Color color, int nWid, DashStyle nStyles = System.Drawing.Drawing2D.DashStyle.Solid)
        {
            IntPtr hOverlayDC = IntPtr.Zero;
            MIL_ID tempOverlay = MIL.M_NULL;
            if (AutoRunMode)
            {
                tempOverlay = MilCamOverlay[index];
            }
            else
            {
                tempOverlay = MilSetCamOverlay;
            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(tempOverlay, MIL.M_DC_HANDLE, MIL.M_NULL);

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    using (Pen arrowPen = new Pen(color, nWid))
                    {
                        int startX = (int)(x1 * xReduce[index] + 0.5);
                        int startY = (int)(y1 * yReduce[index] + 0.5);
                        int endX = (int)(x2 * xReduce[index] + 0.5);
                        int endY = (int)(y2 * yReduce[index] + 0.5);

                        DrawingGraphics.DrawLine(arrowPen, startX, startY, endX, endY);
                    }
                }

            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(tempOverlay, MIL.M_MODIFIED, MIL.M_DEFAULT);
        }

        public void DrawOverlayArrow(int index, int x1 , int y1 , int x2 , int y2, Color color, int nWid, DashStyle nStyles = System.Drawing.Drawing2D.DashStyle.Solid)
        {
            IntPtr hOverlayDC = IntPtr.Zero;
            MIL_ID tempOverlay = MIL.M_NULL;
            if (AutoRunMode)
            {
                tempOverlay = MilCamOverlay[index];
            }
            else
            {
                tempOverlay = MilSetCamOverlay;
            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(tempOverlay, MIL.M_DC_HANDLE, MIL.M_NULL);

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    using (Pen arrowPen = new Pen(color, nWid))
                    {
                        arrowPen.DashStyle = nStyles;

                        // 양쪽 화살촉을 설정
                        AdjustableArrowCap arrowCap = new AdjustableArrowCap(6, 6, true);
                        arrowPen.StartCap = LineCap.Custom;
                        arrowPen.EndCap = LineCap.Custom;
                        arrowPen.CustomStartCap = arrowCap;
                        arrowPen.CustomEndCap = arrowCap;

                        //arrowPen.CustomEndCap = new AdjustableArrowCap(6, 6, true);  // 크기 조절 가능

                        int startX = (int)(x1 * xReduce[index] + 0.5);
                        int startY = (int)(y1 * yReduce[index] + 0.5);
                        int endX = (int)(x2 * xReduce[index] + 0.5);
                        int endY = (int)(y2 * yReduce[index] + 0.5);

                        DrawingGraphics.DrawLine(arrowPen, startX, startY, endX, endY);
                    }
                }
                
            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(tempOverlay, MIL.M_MODIFIED, MIL.M_DEFAULT);
        }
        public void DrawOverlayCircle(int index, System.Drawing.Point clPoint, int radius , Color color, int nWid, DashStyle nStyles)
        {
            IntPtr hOverlayDC = IntPtr.Zero;
            MIL_ID tempOverlay = MIL.M_NULL;
            if (AutoRunMode)
            {
                tempOverlay = MilCamOverlay[index];
            }
            else
            {
                tempOverlay = MilSetCamOverlay;
            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(tempOverlay, MIL.M_DC_HANDLE, MIL.M_NULL);

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    // Draw a blue cross.
                    using (Pen DrawingPen = new Pen(color))
                    {
                        DrawingPen.DashStyle = nStyles;
                        DrawingPen.Width = nWid; 

                        int x1 = (int)((clPoint.X * xReduce[index]) + 0.5);
                        int y1 = (int)((clPoint.Y * yReduce[index]) + 0.5);

                        int x2 = (int)((radius * xReduce[index]) + 0.5);
                        int y2 = (int)((radius * yReduce[index]) + 0.5);

                        DrawingGraphics.DrawEllipse(DrawingPen, x1, y1, x2, y2);

                    }
                }
            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(tempOverlay, MIL.M_MODIFIED, MIL.M_DEFAULT);
        }
        public void DrawOverlayPolygon(int index, List<System.Drawing.Point> points, Color color, int nWid, DashStyle nStyles)
        {
            MIL_ID tempOverlay = MIL.M_NULL;
            if (AutoRunMode)
            {
                tempOverlay = MilCamOverlay[index];
            }
            else
            {
                tempOverlay = MilSetCamOverlay;
            }
            IntPtr hOverlayDC = IntPtr.Zero;

            MIL.MbufControl(tempOverlay, MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(tempOverlay, MIL.M_DC_HANDLE, MIL.M_NULL);

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    // Draw a blue cross.
                    using (Pen DrawingPen = new Pen(color))
                    {
                        DrawingPen.DashStyle = nStyles;
                        DrawingPen.Width = nWid;
  
                        DrawingGraphics.DrawPolygon(DrawingPen, points.ToArray());

                        using (SolidBrush LeftBrush = new SolidBrush(Color.Red))
                        {
                            using (SolidBrush RightBrush = new SolidBrush(Color.Yellow))
                            {
                                using (Font OverlayFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold))
                                {
                                    // Write text in the overlay image
                                    //SizeF GDITextSize = DrawingGraphics.MeasureString("GDI Overlay Text", OverlayFont);
                                    //DrawingGraphics.DrawString("GDI Overlay Text", OverlayFont, LeftBrush, System.Convert.ToInt32(CAM_SIZE_X / 4 - GDITextSize.Width / 2), System.Convert.ToInt32(CAM_SIZE_Y * 3 / 4 - GDITextSize.Height / 2));
                                    //DrawingGraphics.DrawString("GDI Overlay Text", OverlayFont, RightBrush, System.Convert.ToInt32(CAM_SIZE_X * 3 / 4 - GDITextSize.Width / 2), System.Convert.ToInt32(CAM_SIZE_Y * 3 / 4 - GDITextSize.Height / 2));
                                }
                            }
                        }
                    }
                }
            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(tempOverlay, MIL.M_MODIFIED, MIL.M_DEFAULT);
        }

        public void DrawOverlayBox(int index, Rectangle clRect, Color color, int nWid, DashStyle nStyles = DashStyle.Solid)
        {
            IntPtr hOverlayDC = IntPtr.Zero;
            MIL_ID tempOverlay = MIL.M_NULL;
            if (AutoRunMode)
            {
                tempOverlay = MilCamOverlay[index];
            }
            else
            {
                tempOverlay = MilSetCamOverlay;
            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(tempOverlay, MIL.M_DC_HANDLE, MIL.M_NULL);

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    // Draw a blue cross.
                    using (Pen DrawingPen = new Pen(color))
                    {
                        DrawingPen.DashStyle = nStyles;
                        DrawingPen.Width = nWid;

                        int x1 = (int)((clRect.X * xReduce[index]) + 0.5);
                        int x2 = (int)((clRect.Width * xReduce[index]) + 0.5);
                        int y1 = (int)((clRect.Y * yReduce[index]) + 0.5);
                        int y2 = (int)((clRect.Height * yReduce[index]) + 0.5);

                        DrawingGraphics.DrawRectangle(DrawingPen, new Rectangle(x1, y1, x2, y2));

                        //DrawingGraphics.DrawLine(DrawingPen, 0, (int)(CAM_SIZE_Y / 2), CAM_SIZE_X, (int)(CAM_SIZE_Y / 2));
                        //DrawingGraphics.DrawLine(DrawingPen, (int)(CAM_SIZE_X / 2), 0, (int)(CAM_SIZE_X / 2), CAM_SIZE_Y);
                        // Prepare transparent text annotations.
                        // Define the Brushes and fonts used to draw text
                        using (SolidBrush LeftBrush = new SolidBrush(Color.Red))
                        {
                            using (SolidBrush RightBrush = new SolidBrush(Color.Yellow))
                            {
                                using (Font OverlayFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold))
                                {
                                    // Write text in the overlay image
                                    //SizeF GDITextSize = DrawingGraphics.MeasureString("GDI Overlay Text", OverlayFont);
                                    //DrawingGraphics.DrawString("GDI Overlay Text", OverlayFont, LeftBrush, System.Convert.ToInt32(CAM_SIZE_X / 4 - GDITextSize.Width / 2), System.Convert.ToInt32(CAM_SIZE_Y * 3 / 4 - GDITextSize.Height / 2));
                                    //DrawingGraphics.DrawString("GDI Overlay Text", OverlayFont, RightBrush, System.Convert.ToInt32(CAM_SIZE_X * 3 / 4 - GDITextSize.Width / 2), System.Convert.ToInt32(CAM_SIZE_Y * 3 / 4 - GDITextSize.Height / 2));
                                }
                            }
                        }
                    }
                }
            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(tempOverlay, MIL.M_MODIFIED, MIL.M_DEFAULT);
        }
        public void DrawOverlayText(int index, System.Drawing.Point clPoint, string szText, Color color, int nSize)
        {
            IntPtr hOverlayDC = IntPtr.Zero;
            int x = (int)((clPoint.X * xReduce[index]) + 0.5);
            int y = (int)((clPoint.Y * yReduce[index]) + 0.5);
            MIL_ID tempOverlay = MIL.M_NULL;
            if (AutoRunMode)
            {
                tempOverlay = MilCamOverlay[index];
            }
            else
            {
                tempOverlay = MilSetCamOverlay;
            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(tempOverlay, MIL.M_DC_HANDLE, MIL.M_NULL);

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    // Draw a blue cross.
                    using (Pen DrawingPen = new Pen(Color.Blue))
                    {
                        // Prepare transparent text annotations.
                        // Define the Brushes and fonts used to draw text
                        using (SolidBrush LeftBrush = new SolidBrush(color))
                        {
                            using (SolidBrush RightBrush = new SolidBrush(Color.Yellow))
                            {
                                using (Font OverlayFont = new Font("Arial", nSize, FontStyle.Bold))
                                {
                                    // Write text in the overlay image
                                    DrawingGraphics.DrawString(szText, OverlayFont, LeftBrush, x, y);
                                }
                            }
                        }
                    }
                }
            }
            MIL.MbufControl(tempOverlay, MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(tempOverlay, MIL.M_MODIFIED, MIL.M_DEFAULT);

        }
        #endregion
        public void AllocMilCamDisplay(IntPtr myPicHandler, int index)
        {
            //MIL.MdispAlloc(MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_DEFAULT, ref MilCamDisplay[index]);
            MIL.MdispAlloc(MilSystem, MIL.M_DEV0, "M_DEFAULT", MIL.M_DEFAULT, ref MilCamDisplay[index]);
            if (MilCamDisplay[index] != MIL.M_NULL)
            {
                MIL_INT DisplayType = MIL.MdispInquire(MilCamDisplay[index], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType != MIL.M_WINDOWED)
                {
                    MIL.MdispFree(MilCamDisplay[index]);
                    MilCamDisplay[index] = MIL.M_NULL;

                    return;
                }

                MIL.MdispSelectWindow(MilCamDisplay[index], MilCamSmallImageChild[index], myPicHandler);
            }
        }

        public void AllocMilSetCamDisplay(IntPtr myPicHandler)
        {
            MIL.MdispAlloc(MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_DEFAULT, ref MilSetCamDisplay);
            if (MilSetCamDisplay != MIL.M_NULL)
            {
                MIL_INT DisplayType = MIL.MdispInquire(MilSetCamDisplay, MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType != MIL.M_WINDOWED)
                {
                    MIL.MdispFree(MilSetCamDisplay);
                    MilSetCamDisplay = MIL.M_NULL;

                    return;
                }

                MIL.MdispSelectWindow(MilSetCamDisplay, MilSetCamSmallImageChild[0], myPicHandler);
            }
        }
        public void AllocMilCamBuffer(int index, int _CamW, int _CamH)
        {
            CamControlWidth = _CamW;
            CamControlHeight = _CamH;
            int i = 0;
            long lBufferAttributes = 0;
            if (index < TotalCamCount)
            {
                lBufferAttributes = MIL.M_IMAGE + MIL.M_GRAB + MIL.M_PROC + MIL.M_DISP;
            }
            else
            {
                lBufferAttributes = MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP;
            }
            MIL.MbufAlloc2d(MilSystem, CAM_SIZE_X[index], CAM_SIZE_Y[index], (8 + MIL.M_UNSIGNED), lBufferAttributes, ref MilCamGrabImage[index]);
            MIL.MbufClear(MilCamGrabImage[index], MIL.M_COLOR_BLACK);
            MIL.MbufChild2d(MilCamGrabImage[index], 0, 0, CAM_SIZE_X[index], CAM_SIZE_Y[index], ref MilCamGrabImageChild[index]);
            MIL.MbufClear(MilCamGrabImageChild[index], MIL.M_COLOR_BLACK);
            //
            //
            MIL.MbufAlloc2d(MilSystem, CAM_SIZE_X[index], CAM_SIZE_Y[index], (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilProcImage[index]);
            MIL.MbufClear(MilProcImage[index], MIL.M_COLOR_BLACK);
            MIL.MbufChild2d(MilProcImage[index], 0, 0, CAM_SIZE_X[index], CAM_SIZE_Y[index], ref MilProcImageChild[index]);
            MIL.MbufClear(MilProcImageChild[index], MIL.M_COLOR_BLACK);
            //
            MIL.MbufAllocColor(MilSystem, 3, CamControlWidth, CamControlHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilCamSmallImage[index]);
            MIL.MbufClear(MilCamSmallImage[index], MIL.M_COLOR_BLACK);
            //
            MIL.MbufChild2d(MilCamSmallImage[index], 0, 0, CamControlWidth, CamControlHeight, ref MilCamSmallImageChild[index]);
            MIL.MbufClear(MilCamSmallImageChild[index], MIL.M_COLOR_BLACK);

        }
        public void AllocMilSetCamBuffer(int index, int _CamW, int _CamH)
        {
            SetCamControlWidth = _CamW;
            SetCamControlHeight = _CamH;
            //
            //세팅 카메라 화면
            //
            MIL.MbufAllocColor(MilSystem, 3, SetCamControlWidth, SetCamControlHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilSetCamSmallImage[index]);
            MIL.MbufClear(MilSetCamSmallImage[index], MIL.M_COLOR_BLACK);
            MIL.MbufChild2d(MilSetCamSmallImage[index], 0, 0, SetCamControlWidth, SetCamControlHeight, ref MilSetCamSmallImageChild[index]);
            MIL.MbufClear(MilSetCamSmallImageChild[index], MIL.M_COLOR_BLACK);
        }
#region [MIL INIT]
        
        public void AllocMilApplication()
        {
            int i = 0;
            MIL.MappAlloc(MIL.M_NULL, MIL.M_DEFAULT, ref MilApplication);       //MIL.MappAlloc(MIL.M_DEFAULT, ref MilApplication);
            // Allocate defaults.
            MIL.MappControl(MIL.M_ERROR, MIL.M_PRINT_DISABLE);

            MIL.MsysAlloc(MIL.M_SYSTEM_GIGE_VISION, MIL.M_DEV0, MIL.M_COMPLETE, ref MilSystem);
            TotalCamCount = (int)MIL.MsysInquire(MilSystem, MIL.M_DIGITIZER_NUM, MIL.M_NULL);

            if (TotalCamCount > 0)
            {
                for (i = 0; i < TotalCamCount; i++)
                {
                    MIL.MdigAlloc(MilSystem, MIL.M_DEV0+i, ("aoiCam.dcf"), MIL.M_DEFAULT, ref MilDigitizerList[i]);


                    bGrabOnFlag[i] = true;
                }

                for (i = 0; i < TotalCamCount; i++)
                {
                    MIL.MdigControl(MilDigitizerList[i], MIL.M_GRAB_MODE, MIL.M_SYNCHRONOUS); //M_SYNCHRONOUS); M_SYNCHRONOUS  M_ASYNCHRONOUS
                    MIL.MdigControl(MilDigitizerList[i], MIL.M_GRAB_TIMEOUT, 3000);

                    MIL.MdigInquire(MilDigitizerList[i], MIL.M_SIZE_X, ref m_nMilSizeX[i]);
                    MIL.MdigInquire(MilDigitizerList[i], MIL.M_SIZE_Y, ref m_nMilSizeY[i]);

                    Console.WriteLine("Cam Width: " + m_nMilSizeX[i]);
                    Console.WriteLine("Cam Height: " + m_nMilSizeY[i]);

                    CAM_SIZE_X[i] = (int)m_nMilSizeX[i];
                    CAM_SIZE_Y[i] = (int)m_nMilSizeY[i];
                }
            }
            else
            {
                for (i = 0; i < 2; i++)
                {
                    m_nMilSizeX[i] = CAM_SIZE_X[i];
                    m_nMilSizeY[i] = CAM_SIZE_Y[i];
                }
                    
            }



            //카메라 2개라서 고정 : 2

            MilCamGrabImage = new MIL_ID[CamFixCount];
            MilCamGrabImageChild = new MIL_ID[CamFixCount];
            MilProcImage = new MIL_ID[CamFixCount];
            MilProcImageChild = new MIL_ID[CamFixCount];
            //
            MilCamDisplay = new MIL_ID[CamFixCount];
            MilCamSmallImage = new MIL_ID[CamFixCount];
            MilCamSmallImageChild = new MIL_ID[CamFixCount];
            //
            MilSetCamDisplay = new MIL_ID();
            MilSetCamSmallImage = new MIL_ID[CamFixCount];
            MilSetCamSmallImageChild = new MIL_ID[CamFixCount];
            MilCamOverlay = new MIL_ID[CamFixCount];
            MilSetCamOverlay = new MIL_ID();
            MilCamTransparent = new MIL_INT[CamFixCount];
            MilSetCamTransparent = new MIL_ID();
            MilCamOverlayImage = new MIL_ID[CamFixCount];
            

            for (i = 0; i < CamFixCount; i++)
            {
                MilCamGrabImage[i] = MIL.M_NULL;
                MilCamGrabImageChild[i] = MIL.M_NULL;
                MilProcImage[i] = MIL.M_NULL;
                MilProcImageChild[i] = MIL.M_NULL;
                //
                MilCamDisplay[i] = MIL.M_NULL;
                MilCamSmallImage[i] = MIL.M_NULL;
                MilCamSmallImageChild[i] = MIL.M_NULL;
                //
                
                MilSetCamSmallImage[i] = MIL.M_NULL;
                MilSetCamSmallImageChild[i] = MIL.M_NULL;
                MilCamOverlay[i] = MIL.M_NULL;
                MilCamTransparent[i] = MIL.M_NULL;
                MilCamOverlayImage[i] = MIL.M_NULL;
            }


            MilSetCamDisplay = MIL.M_NULL;
            MilSetCamOverlay = MIL.M_NULL;
            MilSetCamTransparent = MIL.M_NULL;



        }

        public void MilClose()
        {
            int i = 0;

            // Stop continuous grab.

            for (i = 0; i < CamFixCount; i++)
            {
                MIL.MdigHalt(MilDigitizerList[i]);

                MIL.MbufFree(MilCamGrabImage[i]);
                MIL.MbufFree(MilCamGrabImageChild[i]);
                MIL.MbufFree(MilProcImage[i]);
                MIL.MbufFree(MilProcImageChild[i]);
                //
                MIL.MdispSelect(MilCamDisplay[i], MIL.M_NULL);
                MIL.MbufFree(MilCamSmallImage[i]);
                MIL.MbufFree(MilCamSmallImageChild[i]);
                //
                
                MIL.MbufFree(MilSetCamSmallImage[i]);
                MIL.MbufFree(MilSetCamSmallImageChild[i]);
                MIL.MbufFree(MilCamOverlay[i]);
                
                MIL.MbufFree(MilCamOverlayImage[i]);

                MIL.MdigFree(MilDigitizerList[i]);

            }
            MIL.MbufFree(MilSetCamOverlay);
            MIL.MdispSelect(MilSetCamDisplay, MIL.M_NULL);

            MIL.MdispFree(MilCamDisplay[0]);
            MIL.MdispFree(MilCamDisplay[1]);
            MIL.MdispFree(MilSetCamDisplay);

            MIL.MsysFree(MilSystem);

            if (MilApplication != MIL.M_NULL)
            {
                MIL.MappFree(MilApplication);
            }
        }
        #endregion
    }
}
