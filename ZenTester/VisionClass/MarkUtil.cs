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
        public MarkViewerForm markViewer;
        private MIL_ID m_MilModModel;
        private MIL_ID m_MilModResult;
        public MIL_ID m_MilMarkImage;
        public MIL_ID m_MilMarkOverlay;
        public MIL_ID m_MilMarkDisplay;
        public MIL_INT m_lTransparentColor;
        public MarkUtil()
        {
            m_MilModModel = MIL.M_NULL;
            m_MilModResult = MIL.M_NULL;
            m_MilMarkImage = MIL.M_NULL;
            m_MilMarkOverlay = MIL.M_NULL;
            m_lTransparentColor = MIL.M_NULL;

            markViewer = new MarkViewerForm();
        }

        public bool RegisterMark(int index, double dStartX, double dStartY, double dSizeX, double dSizeY)
        {
            MIL_ID tempMilImage = MIL.M_NULL;
            if (m_MilModModel != MIL.M_NULL)
            {
                MIL.MmodFree(m_MilModModel);
                m_MilModModel = MIL.M_NULL;
            }

            System.Drawing.Point m_clPtMarkSize = new System.Drawing.Point();
            System.Drawing.Point m_clPtMarkStartPos = new System.Drawing.Point();
            m_clPtMarkStartPos.X = (int)dStartX;
            m_clPtMarkStartPos.Y = (int)dStartY;

            m_clPtMarkSize.X = (int)dSizeX;
            m_clPtMarkSize.Y = (int)dSizeX;

            MIL.MbufClear(tempMilImage, 0);

            MIL.MmodAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_GEOMETRIC, MIL.M_DEFAULT, ref m_MilModModel);
            MIL.MmodDefine(m_MilModModel, MIL.M_IMAGE, Globalo.visionManager.milLibrary.MilProcImageChild[index], m_clPtMarkStartPos.X, m_clPtMarkStartPos.Y, m_clPtMarkSize.X, m_clPtMarkSize.Y);
            MIL.MmodDraw(MIL.M_DEFAULT, m_MilModModel, tempMilImage, MIL.M_DRAW_IMAGE, MIL.M_DEFAULT, MIL.M_DEFAULT);

            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_X, m_clPtMarkSize.X / 2);
            MIL.MmodControl(m_MilModModel, MIL.M_DEFAULT, MIL.M_REFERENCE_Y, m_clPtMarkSize.Y / 2);

            markViewer.InitMaskViewDlg(0, m_clPtMarkSize.X, m_clPtMarkSize.Y);
            markViewer.ShowDialog();
            return false;
        }

        public void ViewMarkMask()
        {
            
        }

        public void MarkEnableOverlay()
        {
            if (m_MilMarkDisplay != MIL.M_NULL)
            {
                MIL_INT DisplayType = MIL.MdispInquire(m_MilMarkDisplay, MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType == (MIL.M_WINDOWED | MIL.M_USER_WINDOW))
                {
                    MIL.MdispControl(m_MilMarkDisplay, MIL.M_WINDOW_SHOW, MIL.M_ENABLE);
                    MIL.MdispControl(m_MilMarkDisplay, MIL.M_OVERLAY, MIL.M_ENABLE);
                    MIL.MdispInquire(m_MilMarkDisplay, MIL.M_OVERLAY_ID, ref m_MilMarkOverlay);
                    MIL.MdispControl(m_MilMarkDisplay, MIL.M_OVERLAY_SHOW, MIL.M_DISABLE);

                    m_lTransparentColor = MIL.MdispInquire(m_MilMarkDisplay, MIL.M_TRANSPARENT_COLOR, MIL.M_NULL);

                    MIL.MbufClear(m_MilMarkOverlay, m_lTransparentColor);

                    MIL.MdispControl(m_MilMarkDisplay, MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);
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
