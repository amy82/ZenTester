using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;


namespace ZenHandler.VisionClass
{
    public class MilLibraryUtil
    {
        public MIL_ID MilApplication = MIL.M_NULL;         // Application identifier.

        //public List<MIL_ID> MilDigitizerList { get; set; } = new List<MIL_ID>();
        public MIL_ID[] MilDigitizerList;
        public MIL_ID MilSystem = MIL.M_NULL;              // System identifier.
        public MIL_ID[] MilCamDisplay;                        // Display identifier.
        public int TotalCamCount = 0;
        public int CamFixCount = 2;


        public bool[] bGrabOnFlag = new bool[2];

        //CAMERA
        public MIL_ID[] MilCamGrabImage;
        public MIL_ID[] MilCamGrabImageChild;
        public MIL_ID[] MilCamSmallImage;
        public MIL_ID[] MilCamSmallImageChild;
        public MIL_ID[] MilCamOverlay;
        public MIL_INT[] MilCamTransparent;
        //Overlay
        public MIL_ID[] MilCamOverlayImage;


        ///public byte[][][] m_pImgBuff = new byte[1][][];



        public int CAM_SIZE_X = 2000;
        public int CAM_SIZE_Y = 1500;
        private MIL_INT m_nMilSizeX = 0;
        private MIL_INT m_nMilSizeY = 0;

        private int CamControlWidth = 100;      //픽처 컨트롤 가로 사이즈
        private int CamControlHeight = 100;     //픽처 컨트롤 세로 사이즈

        public double xReduce = 0.0;
        public double yReduce = 0.0;


        public MilLibraryUtil()
        {
            MilDigitizerList = new MIL_ID[CamFixCount];
            MilDigitizerList[0] = MIL.M_NULL;
            MilDigitizerList[1] = MIL.M_NULL;
        }

        public void MilClose()
        {
            int i = 0;

            // Stop continuous grab.

            for (i = 0; i < CamFixCount; i++)
            {
                MIL.MdigHalt(MilDigitizerList[i]);
                
                MIL.MdispSelect(MilCamDisplay[i], MIL.M_NULL);
                MIL.MbufFree(MilCamGrabImage[i]);
                MIL.MbufFree(MilCamGrabImageChild[i]);
                MIL.MbufFree(MilCamGrabImageChild[i]);
                MIL.MbufFree(MilCamSmallImage[i]);
                MIL.MbufFree(MilCamSmallImageChild[i]);
                MIL.MbufFree(MilCamOverlay[i]);
                MIL.MbufFree(MilCamOverlayImage[i]);

                MIL.MdigFree(MilDigitizerList[i]);

            }

            MIL.MdispFree(MilCamDisplay[0]);
            MIL.MdispFree(MilCamDisplay[1]);

            MIL.MsysFree(MilSystem);
            if (MilApplication != MIL.M_NULL)
            {
                MIL.MappFree(MilApplication);
            }
        }
        
        public void setCamSize(double _rx , double _ry)
        {
            
            

        }
        public void SetGrabOn(int index, bool bGrab)
        {
            bGrabOnFlag[index] = bGrab;
        }
        public void setCamImage(int index, string filePath)
        {
            MIL.MbufLoad(filePath, MilCamGrabImage[index]);
            MIL.MimResize(MilCamGrabImageChild[index], MilCamSmallImageChild[index], xReduce, yReduce, MIL.M_DEFAULT);
        }
        public void ClearOverlay(int index)
        {
            //m_clMilDrawBox[nUnit].RemoveAll();
            //m_clMilDrawText[nUnit].RemoveAll();
            //m_clMilDrawCross[nUnit].RemoveAll();

            MIL.MbufClear(MilCamOverlay[index], MilCamTransparent[index]);
        }
        public void MilGrabRun(int index)
        {
            if (bGrabOnFlag[index] == true && MilDigitizerList[index] != MIL.M_NULL)
            {
                MIL.MdigGrab(MilDigitizerList[index], MilCamGrabImage[index]);
                MIL.MdigGrabWait(MilDigitizerList[index], MIL.M_GRAB_END);
                MIL.MimResize(MilCamGrabImageChild[index], MilCamSmallImageChild[index], xReduce, yReduce, MIL.M_DEFAULT);
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
        public void drawTest(MIL_ID display, int index)
        {
            //display
            MIL.MmodControl(display, MIL.M_DEFAULT, 3203L, xReduce);//M_DRAW_SCALE_X
            MIL.MmodControl(display, MIL.M_DEFAULT, 3204L, yReduce);//M_DRAW_SCALE_Y
            MIL.MmodDraw(MIL.M_DEFAULT, display, Globalo.visionManager.milLibrary.MilCamOverlay[index], MIL.M_DRAW_BOX, MIL.M_DEFAULT, MIL.M_DEFAULT);
        }
        public void DrawOverlayArrow(int index, int x1 , int y1 , int x2 , int y2, Color color, int nWid, DashStyle nStyles)
        {
            IntPtr hOverlayDC = IntPtr.Zero;

            MIL.MbufControl(MilCamOverlay[index], MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(MilCamOverlay[index], MIL.M_DC_HANDLE, MIL.M_NULL);

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

                        int startX = (int)(x1 * xReduce + 0.5);
                        int startY = (int)(y1 * yReduce + 0.5);
                        int endX = (int)(x2 * xReduce + 0.5);
                        int endY = (int)(y2 * yReduce + 0.5);

                        DrawingGraphics.DrawLine(arrowPen, startX, startY, endX, endY);
                    }
                }
                
            }
            MIL.MbufControl(MilCamOverlay[index], MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(MilCamOverlay[index], MIL.M_MODIFIED, MIL.M_DEFAULT);
        }
        public void DrawOverlayCircle(int index, Rectangle clRect, Color color, int nWid, DashStyle nStyles)
        {
            IntPtr hOverlayDC = IntPtr.Zero;

            MIL.MbufControl(MilCamOverlay[index], MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(MilCamOverlay[index], MIL.M_DC_HANDLE, MIL.M_NULL);

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    // Draw a blue cross.
                    using (Pen DrawingPen = new Pen(color))
                    {
                        DrawingPen.DashStyle = nStyles;
                        DrawingPen.Width = nWid; 

                        int x1 = (int)((clRect.X * xReduce) + 0.5);
                        int y1 = (int)((clRect.Y * yReduce) + 0.5);

                        int x2 = (int)((clRect.Width * xReduce) + 0.5);
                        int y2 = (int)((clRect.Height * yReduce) + 0.5);

                        DrawingGraphics.DrawEllipse(DrawingPen, x1, y1, x2, y2);

                    }
                }
            }
            MIL.MbufControl(MilCamOverlay[index], MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(MilCamOverlay[index], MIL.M_MODIFIED, MIL.M_DEFAULT);
        }
        public void DrawOverlayBox(int index, Rectangle clRect, Color color, int nWid, DashStyle nStyles)
        {
            IntPtr hOverlayDC = IntPtr.Zero;

            MIL.MbufControl(MilCamOverlay[index], MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(MilCamOverlay[index], MIL.M_DC_HANDLE, MIL.M_NULL);

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    // Draw a blue cross.
                    using (Pen DrawingPen = new Pen(Color.Blue))
                    {
                        DrawingPen.DashStyle = nStyles;
                        DrawingPen.Width = nWid;
                        int x1 = (int)((clRect.X * xReduce) + 0.5);
                        int x2 = (int)((clRect.Width * xReduce) + 0.5);
                        int y1 = (int)((clRect.Y * yReduce) + 0.5);
                        int y2 = (int)((clRect.Height * yReduce) + 0.5);

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
            MIL.MbufControl(MilCamOverlay[index], MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(MilCamOverlay[index], MIL.M_MODIFIED, MIL.M_DEFAULT);
        }
        public void DrawOverlayText(int index, System.Drawing.Point clPoint, string szText, Color color, int nSize)
        {
            IntPtr hOverlayDC = IntPtr.Zero;
            int x = (int)((clPoint.X * xReduce) + 0.5);
            int y = (int)((clPoint.Y * yReduce) + 0.5);

            MIL.MbufControl(MilCamOverlay[index], MIL.M_DC_ALLOC, MIL.M_DEFAULT);
            hOverlayDC = (IntPtr)MIL.MbufInquire(MilCamOverlay[index], MIL.M_DC_HANDLE, MIL.M_NULL);

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    // Draw a blue cross.
                    using (Pen DrawingPen = new Pen(Color.Blue))
                    {
                        // Prepare transparent text annotations.
                        // Define the Brushes and fonts used to draw text
                        using (SolidBrush LeftBrush = new SolidBrush(Color.Red))
                        {
                            using (SolidBrush RightBrush = new SolidBrush(Color.Yellow))
                            {
                                using (Font OverlayFont = new Font(FontFamily.GenericSansSerif, nSize, FontStyle.Bold))
                                {
                                    // Write text in the overlay image
                                    DrawingGraphics.DrawString(szText, OverlayFont, LeftBrush, x, y);
                                }
                            }
                        }
                    }
                }
            }
            MIL.MbufControl(MilCamOverlay[index], MIL.M_DC_FREE, MIL.M_DEFAULT);
            MIL.MbufControl(MilCamOverlay[index], MIL.M_MODIFIED, MIL.M_DEFAULT);

        }
        public void AllocMilCamDisplay(IntPtr myPicHandler, int index)
        {
            MIL.MdispAlloc(MilSystem, MIL.M_DEV0, "M_DEFAULT", MIL.M_DEFAULT, ref MilCamDisplay[index]);
            //MIL.MdispAlloc(MilSystem, MIL.M_DEV0 + index, "M_DEFAULT", MIL.M_DEFAULT, ref MilCamDisplay[index]);
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
        public void AllocMilCamBuffer(int index)
        {
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
            MIL.MbufAlloc2d(MilSystem, CAM_SIZE_X, CAM_SIZE_Y, (8 + MIL.M_UNSIGNED), lBufferAttributes, ref MilCamGrabImage[index]);
            MIL.MbufClear(MilCamGrabImage[index], MIL.M_COLOR_BLACK);
            //
            MIL.MbufChild2d(MilCamGrabImage[index], 0, 0, CAM_SIZE_X, CAM_SIZE_Y, ref MilCamGrabImageChild[index]);
            MIL.MbufClear(MilCamGrabImageChild[index], MIL.M_COLOR_BLACK);
            //
            //MIL.MbufAlloc2d(MilSystem, CamControlWidth, CamControlHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilCamSmallImage[index]);
            //MbufAllocColor(m_MilSystem[nSysIndex], 3, SMALL_CAM_SIZE_X, SMALL_CAM_SIZE_Y, (8 + M_UNSIGNED), M_IMAGE + M_PROC + M_DISP, &m_MilSmallImage[nCamIndex]);

            MIL.MbufAllocColor(MilSystem, 3, CamControlWidth, CamControlHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilCamSmallImage[index]);
            MIL.MbufClear(MilCamSmallImage[index], MIL.M_COLOR_BLACK);
            //
            MIL.MbufChild2d(MilCamSmallImage[index], 0, 0, CamControlWidth, CamControlHeight, ref MilCamSmallImageChild[index]);
            MIL.MbufClear(MilCamSmallImageChild[index], MIL.M_COLOR_BLACK);

        }
        public void AllocMilApplication(int _CamW, int _CamH)
        {
            int i = 0;
            CamControlWidth = _CamW;
            CamControlHeight = _CamH;
            MIL.MappAlloc(MIL.M_NULL, MIL.M_DEFAULT, ref MilApplication);//MIL.MappAlloc(MIL.M_DEFAULT, ref MilApplication);
            // Allocate defaults.
            MIL.MappControl(MIL.M_ERROR, MIL.M_PRINT_DISABLE);

            MIL.MsysAlloc(MIL.M_SYSTEM_GIGE_VISION, MIL.M_DEV0, MIL.M_COMPLETE, ref MilSystem);
            TotalCamCount = (int)MIL.MsysInquire(MilSystem, MIL.M_DIGITIZER_NUM, MIL.M_NULL);

            if (TotalCamCount > 0)
            {
                for (i = 0; i < TotalCamCount; i++)
                {
                    MIL.MdigAlloc(MilSystem, MIL.M_DEV0 + i, ("aoiCam.dcf"), MIL.M_DEFAULT, ref MilDigitizerList[i]);
                    bGrabOnFlag[i] = true;
                }

                for (i = 0; i < TotalCamCount; i++)
                {
                    MIL.MdigControl(MilDigitizerList[i], MIL.M_GRAB_MODE, MIL.M_ASYNCHRONOUS); //M_SYNCHRONOUS); M_SYNCHRONOUS  M_ASYNCHRONOUS
                    MIL.MdigControl(MilDigitizerList[i], MIL.M_GRAB_TIMEOUT, 1000);

                    MIL.MdigInquire(MilDigitizerList[i], MIL.M_SIZE_X, ref m_nMilSizeX);
                    MIL.MdigInquire(MilDigitizerList[i], MIL.M_SIZE_Y, ref m_nMilSizeY);

                    Console.WriteLine("Cam Width: " + m_nMilSizeX);
                    Console.WriteLine("Cam Height: " + m_nMilSizeY);

                    CAM_SIZE_X = (int)m_nMilSizeX;
                    CAM_SIZE_Y = (int)m_nMilSizeY;

                    xReduce = ((double)CamControlWidth / (double)CAM_SIZE_X);
                    yReduce = ((double)CamControlHeight / (double)CAM_SIZE_Y);
                }
            }
            else
            {
                m_nMilSizeX = CAM_SIZE_X;
                m_nMilSizeY = CAM_SIZE_Y;
            }



            //카메라 2개라서 고정 : 2
            
            MilCamDisplay = new MIL_ID[CamFixCount];
            MilCamGrabImage = new MIL_ID[CamFixCount];
            MilCamGrabImageChild = new MIL_ID[CamFixCount];
            MilCamSmallImage = new MIL_ID[CamFixCount];
            MilCamSmallImageChild = new MIL_ID[CamFixCount];
            MilCamOverlay = new MIL_ID[CamFixCount];
            MilCamTransparent = new MIL_INT[CamFixCount];
            MilCamOverlayImage = new MIL_ID[CamFixCount];
            

            for (i = 0; i < CamFixCount; i++)
            {
                MilCamDisplay[i] = MIL.M_NULL;
                MilCamGrabImage[i] = MIL.M_NULL;
                MilCamGrabImageChild[i] = MIL.M_NULL;
                MilCamSmallImage[i] = MIL.M_NULL;
                MilCamSmallImageChild[i] = MIL.M_NULL;
                MilCamOverlay[i] = MIL.M_NULL;
                MilCamTransparent[i] = MIL.M_NULL;
                MilCamOverlayImage[i] = MIL.M_NULL;

                AllocMilCamBuffer(i);

            }


            

        }
    }
}
