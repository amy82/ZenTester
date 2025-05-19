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

        private System.Drawing.Point smallDispSize = new System.Drawing.Point();
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

            //this->DisplayMarkView(m_nUnit, m_nMarkNo, m_clMaskViewDlg.m_iMarkSetSizeX, m_clMaskViewDlg.m_iMarkSetSizeY);
            //this->ShowMarkNo();
        }
        public void DisplayMarkView()
        {

        }
        public bool RegisterMark(int index, double dStartX, double dStartY, double dSizeX, double dSizeY)
        {
            MIL_ID MilTempImage = MIL.M_NULL;
            if (m_MilModModel != MIL.M_NULL)
            {
                MIL.MmodFree(m_MilModModel);
                m_MilModModel = MIL.M_NULL;
            }

            System.Drawing.Point m_clPtMarkSize = new System.Drawing.Point();
            System.Drawing.Point m_clPtMarkStartPos = new System.Drawing.Point();

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
