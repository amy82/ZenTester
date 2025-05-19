using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Matrox.MatroxImagingLibrary;

namespace ZenHandler.VisionClass
{
    public partial class MarkViewerForm : Form
    {
        private System.Drawing.Point DispSize = new System.Drawing.Point();
        private MIL_ID m_MilMaskOverlay;
        private MIL_ID m_MilMask;
        private MIL_INT m_MilTransparentColor;

        private bool m_bInitOverlay = false;
        byte[] m_pMaskBuff = null;

        private double m_dZoomX;
        private double m_dZoomY;
        

        public MarkViewerForm()
        {
            InitializeComponent();

            this.CenterToScreen();

            m_MilMask = MIL.M_NULL;
            m_MilMaskOverlay = MIL.M_NULL;
            m_MilTransparentColor = MIL.M_NULL;

            DispSize.X = panel_MarkZoomImage.Width;
            DispSize.Y = panel_MarkZoomImage.Height;

            //MbufAllocColor(g_clVision.m_MilSystem[0], 1L, CCD1_CAM_SIZE_X, CCD1_CAM_SIZE_Y, (8 + M_UNSIGNED), M_IMAGE + M_DISP + M_PROC, &g_clModelFinder.m_MilMarkImage[1]);
            MIL.MbufAllocColor(Globalo.visionManager.milLibrary.MilSystem, 1, Globalo.visionManager.milLibrary.CAM_SIZE_X[0], Globalo.visionManager.milLibrary.CAM_SIZE_Y[0], (8 + MIL.M_UNSIGNED), 
                MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref Globalo.visionManager.markUtil.m_MilMarkImage[1]);


            if (Globalo.visionManager.markUtil.m_MilMarkImage[1] != MIL.M_NULL)
            {
                Globalo.visionManager.markUtil.m_MilMarkDisplay[1] = MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEV0, "M_DEFAULT", MIL.M_DEFAULT, MIL.M_NULL);
                MIL_INT DisplayType = MIL.MdispInquire(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType != MIL.M_WINDOWED)
                {
                    MIL.MdispFree(Globalo.visionManager.markUtil.m_MilMarkDisplay[1]);
                    Globalo.visionManager.markUtil.m_MilMarkDisplay[1] = MIL.M_NULL;

                    return;
                }
            }
            //panel_MarkZoomImage
        }

        public void DisplayMarkView(int nMarkNo, double dZoomMarkWidth, double dZoomMarkHeight)
        {
            //_stprintf_s(szPath, SIZE_OF_1K, _T("%s\\%s\\MARK-%d.bmp"), BASE_MODEL_PATH, g_clSysData.m_szModelName, nMarkNo + 1);

            MIL.MbufClear(Globalo.visionManager.markUtil.m_MilMarkOverlay, Globalo.visionManager.markUtil.m_lTransparentColor);
            MIL.MbufClear(Globalo.visionManager.markUtil.m_MilMarkImage[1], 192);
        }
        public void InitMaskViewDlg(int nMarkNo, int m_iSizeX , int m_iSizeY)
        {
            //panel_MarkZoomImage
            
            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, DispSize.X, DispSize.Y, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref Globalo.visionManager.markUtil.m_MilMarkImage[1]);

            if (Globalo.visionManager.markUtil.m_MilMarkImage[1] != MIL.M_NULL)
            {
                if (Globalo.visionManager.markUtil.m_MilMarkDisplay[1] != MIL.M_NULL)
                {
                    MIL.MbufClear(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], 0);
                    Globalo.visionManager.markUtil.m_MilMarkDisplay[1] = MIL.M_NULL;
                    m_MilMaskOverlay = MIL.M_NULL;
                    Globalo.visionManager.markUtil.m_MilMarkDisplay[1] = MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, MIL.M_NULL);

                    MIL.MdispSelectWindow(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], Globalo.visionManager.markUtil.m_MilMarkImage[1], panel_MarkZoomImage.Handle);

                    
                }
            }


            MaskEnableOverlay();


            //m_nUnit = nUnit;
            //m_nMarkNo = nMarkNo;

            //m_bMaskDrag = false;
            //m_bInitOverlay = false;
            //m_bEnableOverlay = false;
            m_pMaskBuff = null;
            //m_nEdgeSmooth = g_clMarkData[nUnit].m_nSmooth[nMarkNo];

            m_dZoomX = (double)DispSize.X / (double)m_iSizeX;      //마크 이미지 축소 OR 확대
            m_dZoomY = (double)DispSize.Y / (double)m_iSizeY;

            MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, 3203L, m_dZoomX);//M_DRAW_SCALE_X
            MIL.MmodControl(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, 3204L, m_dZoomY);//M_DRAW_SCALE_Y


            MIL.MmodDraw(MIL.M_DEFAULT, Globalo.visionManager.markUtil.m_MilModModel, Globalo.visionManager.markUtil.m_MilMarkImage[1], MIL.M_DRAW_IMAGE, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL_INT m_clCdCenterX = MIL.M_NULL;
            MIL_INT m_clCdCenterY = MIL.M_NULL;

            MIL.MmodInquire(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_X + MIL.M_TYPE_DOUBLE, ref m_clCdCenterX);  //드래그된 영역에서 중심 X
            MIL.MmodInquire(Globalo.visionManager.markUtil.m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_Y + MIL.M_TYPE_DOUBLE, ref m_clCdCenterX); //드래그된 영역에서 중심 Y


            //m_clPtMarkSize.x = (int)(m_iSizeX + 0.5);
            //m_clPtMarkSize.y = (int)(m_iSizeY + 0.5);
            // 마스크 이미지 초기화
            MIL.MbufAllocColor(Globalo.visionManager.milLibrary.MilSystem, 1, m_iSizeX, m_iSizeY, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref m_MilMask);

            if (m_MilMask != MIL.M_NULL)
            {
                m_pMaskBuff = new byte[m_iSizeX * m_iSizeY];
                // m_pMaskBuff = (unsigned char*)malloc(m_iSizeX * m_iSizeY * sizeof(unsigned char));
                //memset(m_pMaskBuff, 0, (m_iSizeX * m_iSizeY * sizeof(unsigned char)));
            }
            // 센터라인 그리기
            //this->DrawCenterLine(m_clCdCenter);
        }
        private void MaskEnableOverlay()
        {
            if (m_bInitOverlay == false)
            {
                m_bInitOverlay = true;

                if (Globalo.visionManager.markUtil.m_MilMarkDisplay[1] != MIL.M_NULL)
                {
                    MIL_INT DisplayType = MIL.MdispInquire(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                    if (DisplayType == (MIL.M_WINDOWED | MIL.M_USER_WINDOW))
                    {
                        MIL.MdispControl(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_OVERLAY, MIL.M_ENABLE);
                        MIL.MdispInquire(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_OVERLAY_ID, ref m_MilMaskOverlay);

                        MIL.MdispControl(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_OVERLAY_SHOW, MIL.M_ENABLE);
                        MIL.MdispControl(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);

                        m_MilTransparentColor = MIL.MdispInquire(Globalo.visionManager.markUtil.m_MilMarkDisplay[1], MIL.M_TRANSPARENT_COLOR, MIL.M_NULL);



                        MIL.MbufClear(m_MilMaskOverlay, m_MilTransparentColor);
                        MIL.MgraControl(MIL.M_DEFAULT, MIL.M_BACKGROUND_MODE, MIL.M_TRANSPARENT);
                    }
                }
            }
        }


    }
}
