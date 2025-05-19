using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;


namespace ZenHandler.VisionClass
{
    public class MarkUtil
    {
        
        public MIL_ID m_MilModModel;
        public MIL_ID m_MilModResult;
        public MIL_ID m_MilMarkOverlay;

        public MIL_ID[] m_MilMarkImage;
        public MIL_ID[] m_MilMarkDisplay;
        public MIL_INT m_lTransparentColor;


        public System.Drawing.Point m_clPtMarkSize = new System.Drawing.Point();
        public System.Drawing.Point m_clPtMarkStartPos = new System.Drawing.Point();
        public System.Drawing.Point m_clPtSmallMarkDispSize = new System.Drawing.Point();
        private System.Drawing.Point smallDispSize = new System.Drawing.Point();

        public int m_nSmooth = 10;
        public MarkUtil()
        {
            int i = 0;
            m_MilMarkImage = new MIL_ID[2];
            m_MilMarkDisplay = new MIL_ID[2];
            for (i = 0; i < 2; i++)
            {
                m_MilMarkImage[i] = MIL.M_NULL;
                m_MilMarkDisplay[i] = MIL.M_NULL;
            }
            m_MilModModel = MIL.M_NULL;
            m_MilModResult = MIL.M_NULL;
            
            m_MilMarkOverlay = MIL.M_NULL;
            m_lTransparentColor = MIL.M_NULL;
            

            smallDispSize.X = Globalo.setTestControl.panel_Mark.Width;
            smallDispSize.Y = Globalo.setTestControl.panel_Mark.Height;
            InitMarkViewDlg();

        }
        private void InitMarkViewDlg()
        {
            MIL.MbufAllocColor(Globalo.visionManager.milLibrary.MilSystem, 1L, smallDispSize.X, smallDispSize.Y, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref m_MilMarkImage[0]);
            if (m_MilMarkImage[0] != MIL.M_NULL)
            {
                m_MilMarkDisplay[0] = MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEV0, "M_DEFAULT", MIL.M_DEFAULT, MIL.M_NULL);

                MIL_INT DisplayType = MIL.MdispInquire(m_MilMarkDisplay[0], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType != MIL.M_WINDOWED)
                {
                    MIL.MdispFree(m_MilMarkDisplay[0]);
                    m_MilMarkDisplay[0] = MIL.M_NULL;

                    return;
                }
            }

            if (m_MilMarkDisplay[0] != MIL.M_NULL)
            {
                MIL.MdispSelectWindow(m_MilMarkDisplay[0], m_MilMarkImage[0], Globalo.setTestControl.panel_Mark.Handle);
                MarkEnableOverlay();
            }

            //DisplayMarkView(m_nUnit, m_nMarkNo, m_clMaskViewDlg.m_iMarkSetSizeX, m_clMaskViewDlg.m_iMarkSetSizeY);
            //this->ShowMarkNo();
        }
        public void DisplayMarkView(int nMarkNo, int dZoomMarkWidth, int dZoomMarkHeight)
        {
            double dZoomX = 0.0;
            double dZoomY = 0.0;

            double dSizeX = 0.0;
            double dSizeY = 0.0;

            double dCenterX = 0.0;
            double dCenterY = 0.0;

            string szPath = "";
            //_stprintf_s(szPath, SIZE_OF_1K, _T("%s\\%s\\MARK-%d.bmp"), BASE_MODEL_PATH, g_clSysData.m_szModelName, nMarkNo + 1);

            MIL.MbufClear(m_MilMarkOverlay, m_lTransparentColor);
            MIL.MbufClear(m_MilMarkImage[0], 192);

            if (true)       //szPath 파일이 있으면
            {
                MIL.MbufImport(szPath, MIL.M_BMP, MIL.M_LOAD, MIL.M_NULL, ref m_MilMarkImage[1]);

                MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_X, ref dSizeX);       //<-----여기서왜 마크 사이즈를 바꾸지?
                MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_Y, ref dSizeY);

                m_clPtMarkSize.X = (int)dSizeX;
                m_clPtMarkSize.Y = (int)dSizeY;

                MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_X, ref dCenterX);
                MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_Y, ref dCenterY);

                //m_clPtMarkCenterPos.X = (int)dCenterX;
                //m_clPtMarkCenterPos.Y = (int)dCenterY;

                dZoomX = smallDispSize.X / dZoomMarkWidth;
                dZoomY = smallDispSize.Y / dZoomMarkHeight;

                MIL.MimResize(m_MilMarkImage[1], m_MilMarkImage[0], dZoomX, dZoomY, MIL.M_DEFAULT);

                DrawMarkOverlay(nMarkNo);	//<----여기서 작은 마크에 Line Draw
            }

            //m_nMarkNo = nMarkNo;
            //g_clTaskWork[m_nUnit].m_ManualMarkIndex = m_nMarkNo;
        }
        public void DrawMarkOverlay(int nNo)
        {
            double dSizeX = 0.0;
            double dSizeY = 0.0;

            System.Drawing.Point clDptCenter = new System.Drawing.Point();

            double m_dZoomX = 0.0;
            double m_dZoomY = 0.0;


            MIL.MbufClear(m_MilMarkOverlay, m_lTransparentColor);

            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_X, ref dSizeX);
            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_ALLOC_SIZE_Y, ref dSizeY);

            m_dZoomX = (double)m_clPtSmallMarkDispSize.X / (double)dSizeX;      //마크 이미지 축소 OR 확대
            m_dZoomY = (double)m_clPtSmallMarkDispSize.Y / (double)dSizeY;

            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, 3203L, m_dZoomX); //M_DRAW_SCALE_X
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, 3204L, m_dZoomY); //M_DRAW_SCALE_Y

            MIL_INT m_clCdCenterX = MIL.M_NULL;
            MIL_INT m_clCdCenterY = MIL.M_NULL;

            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_X, ref m_clCdCenterX);
            MIL.MmodInquire(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_Y, ref m_clCdCenterY);

            clDptCenter.X = (int)m_clCdCenterX;
            clDptCenter.Y = (int)m_clCdCenterY;

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MmodDraw(MIL.M_DEFAULT, m_MilModModel, m_MilMarkOverlay, MIL.M_DRAW_DONT_CARE, MIL.M_DEFAULT, MIL.M_DEFAULT);//<---노란 마스크 영역


            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_MAGENTA);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, 3203L, m_dZoomX);   //M_DRAW_SCALE_X
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, 3204L, m_dZoomY);   //M_DRAW_SCALE_Y

            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SMOOTHNESS, m_nSmooth);
            MIL.MmodDraw(MIL.M_DEFAULT, m_MilModModel, m_MilMarkOverlay, MIL.M_DRAW_EDGES, MIL.M_DEFAULT, MIL.M_DEFAULT);//<-----EDGE 영역

            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_RED);


            //	m_dZoomX = (double)g_clModelFinder.m_clPtSmallMarkDispSize.x / (double)g_clModelFinder.m_clPtZoomMarkDispSize.x;		//마크 이미지 축소 OR 확대
            //m_dZoomY = (double)g_clModelFinder.m_clPtSmallMarkDispSize.y / (double)g_clModelFinder.m_clPtZoomMarkDispSize.y;

            int dCenterX = (int)(clDptCenter.X * m_dZoomX);
            int dCenterY = (int)(clDptCenter.Y * m_dZoomY);

            MIL.MgraLine(MIL.M_DEFAULT, m_MilMarkOverlay, dCenterX, 0, dCenterX, dSizeY);    //<-----Center Cross x축
            MIL.MgraLine(MIL.M_DEFAULT, m_MilMarkOverlay, 0, dCenterY, dSizeX, dCenterY);    //<-----Center Cross y축
        }
        public bool RegisterMark(int index, double dStartX, double dStartY, double dSizeX, double dSizeY)
        {
            MIL_ID MilTempImage = MIL.M_NULL;
            if (m_MilModModel != MIL.M_NULL)
            {
                MIL.MmodFree(m_MilModModel);
                m_MilModModel = MIL.M_NULL;
            }

            

            m_clPtMarkStartPos.X = (int)(dStartX * Globalo.visionManager.milLibrary.xExpand[index]);
            m_clPtMarkStartPos.Y = (int)(dStartY * Globalo.visionManager.milLibrary.yExpand[index]);

            m_clPtMarkSize.X = (int)(dSizeX * Globalo.visionManager.milLibrary.xExpand[index]);
            m_clPtMarkSize.Y = (int)(dSizeX * Globalo.visionManager.milLibrary.yExpand[index]);

            MIL.MbufClear(MilTempImage, 0);

            MIL.MbufAllocColor(Globalo.visionManager.milLibrary.MilSystem, 1L, Globalo.visionManager.milLibrary.CAM_SIZE_X[0], Globalo.visionManager.milLibrary.CAM_SIZE_Y[0], 
                (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_DISP + MIL.M_PROC, ref MilTempImage);

            MIL.MmodAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_GEOMETRIC, MIL.M_DEFAULT, ref m_MilModModel);
            MIL.MmodDefine(m_MilModModel, MIL.M_IMAGE, Globalo.visionManager.milLibrary.MilCamGrabImageChild[index], m_clPtMarkStartPos.X, m_clPtMarkStartPos.Y, m_clPtMarkSize.X, m_clPtMarkSize.Y);
            MIL.MmodDraw(MIL.M_DEFAULT, m_MilModModel, MilTempImage, MIL.M_DRAW_IMAGE, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_X, m_clPtMarkSize.X / 2);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_Y, m_clPtMarkSize.Y / 2);


            Globalo.visionManager.markViewer.InitMaskViewDlg(0, m_clPtMarkSize.X, m_clPtMarkSize.Y);
            Globalo.visionManager.markViewer.ShowDialog();

            return false;
        }

        //-----------------------------------------------------------------------------
        //
        //	마크 속성 세팅
        //
        //-----------------------------------------------------------------------------
        public void SettingFindMark(int nNo = 0)
        {

            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SEARCH_POSITION_RANGE, MIL.M_ENABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SEARCH_ANGLE_RANGE, MIL.M_ENABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SEARCH_SCALE_RANGE, MIL.M_ENABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_NUMBER, 3/*1*/);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SPEED, MIL.M_MEDIUM/*M_HIGH*/);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_ACCURACY, MIL.M_HIGH);// M_MEDIUM);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SHARED_EDGES, MIL.M_DISABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_ASPECT_RATIO, MIL.M_DEFAULT);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SAVE_TARGET_EDGES, MIL.M_DISABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_TARGET_CACHING, MIL.M_DISABLE);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_SMOOTHNESS, m_nSmooth);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_DETAIL_LEVEL, MIL.M_MEDIUM);
            MIL.MmodControl(m_MilModModel, MIL.M_CONTEXT, MIL.M_FILTER_MODE, MIL.M_RECURSIVE);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_ANGLE, 0);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_ANGLE_DELTA_NEG, 10);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_ANGLE_DELTA_POS, 10);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_SCALE, 1.0);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_SCALE_MIN_FACTOR, 0.99);
            /////MIL.MmodControl(m_MilModModel[nUnit][nNo], M_DEFAULT, M_SCALE_MAX_FACTOR, 1.01);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_SCALE_MAX_FACTOR, 1.2);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_ACCEPTANCE, 90);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_ACCEPTANCE_TARGET, 80);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_CERTAINTY, 90);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_CERTAINTY_TARGET, 80);
        }

        public void ViewMarkMask()
        {
            
        }

        public void MarkEnableOverlay()
        {
            if (m_MilMarkDisplay[0] != MIL.M_NULL)
            {
                MIL_INT DisplayType = MIL.MdispInquire(m_MilMarkDisplay[0], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType == (MIL.M_WINDOWED | MIL.M_USER_WINDOW))
                {
                    MIL.MdispControl(m_MilMarkDisplay[0], MIL.M_WINDOW_SHOW, MIL.M_ENABLE);
                    MIL.MdispControl(m_MilMarkDisplay[0], MIL.M_OVERLAY, MIL.M_ENABLE);
                    MIL.MdispInquire(m_MilMarkDisplay[0], MIL.M_OVERLAY_ID, ref m_MilMarkOverlay);
                    MIL.MdispControl(m_MilMarkDisplay[0], MIL.M_OVERLAY_SHOW, MIL.M_DISABLE);

                    m_lTransparentColor = MIL.MdispInquire(m_MilMarkDisplay[0], MIL.M_TRANSPARENT_COLOR, MIL.M_NULL);

                    MIL.MbufClear(m_MilMarkOverlay, m_lTransparentColor);

                    MIL.MdispControl(m_MilMarkDisplay[0], MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);
                    MIL.MgraControl(MIL.M_DEFAULT, MIL.M_BACKGROUND_MODE, MIL.M_TRANSPARENT);
                }

                //if (m_bInitMarkOverlay)
                //{
                //    if (m_bEnableMarkOverlay == false)
                //    {
                //        MdispControl(m_MilMarkDisplay[0], M_OVERLAY_SHOW, M_ENABLE);
                //    }
                //    else
                //    {
                //        MdispControl(m_MilMarkDisplay[0], M_OVERLAY_SHOW, M_DISABLE);
                //        m_bEnableMarkOverlay = FALSE;
                //    }

                //    m_bInitMarkOverlay = FALSE;
                //}
            }
        }
    }
}
