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
        public MarkViewerForm()
        {
            InitializeComponent();

            this.CenterToScreen();
            //panel_MarkZoomImage
        }
        private void InitMarkViewDlg()
        {
            DispSize.X = panel_MarkZoomImage.Width;
            DispSize.Y = panel_MarkZoomImage.Height;

            MIL.MbufAllocColor(Globalo.visionManager.milLibrary.MilSystem, 1L, DispSize.X, DispSize.Y, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref Globalo.visionManager.markUtil.m_MilMarkImage);
            if (Globalo.visionManager.markUtil.m_MilMarkImage != MIL.M_NULL)
            {
                Globalo.visionManager.markUtil.m_MilMarkDisplay = MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEV0, "M_DEFAULT", MIL.M_DEFAULT, MIL.M_NULL);

                MIL_INT DisplayType = MIL.MdispInquire(Globalo.visionManager.markUtil.m_MilMarkDisplay, MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType != MIL.M_WINDOWED)
                {
                    MIL.MdispFree(Globalo.visionManager.markUtil.m_MilMarkDisplay);
                    Globalo.visionManager.markUtil.m_MilMarkDisplay = MIL.M_NULL;

                    return;
                }
            }

            if (Globalo.visionManager.markUtil.m_MilMarkDisplay != MIL.M_NULL)
            {
                MIL.MdispSelectWindow(Globalo.visionManager.markUtil.m_MilMarkDisplay, Globalo.visionManager.markUtil.m_MilMarkImage, panel_MarkZoomImage.Handle);
                Globalo.visionManager.markUtil.MarkEnableOverlay();
            }

            //this->DisplayMarkView(m_nUnit, m_nMarkNo, m_clMaskViewDlg.m_iMarkSetSizeX, m_clMaskViewDlg.m_iMarkSetSizeY);
            //this->ShowMarkNo();
        }
        public void DisplayMarkView(int nMarkNo, double dZoomMarkWidth, double dZoomMarkHeight)
        {
            //_stprintf_s(szPath, SIZE_OF_1K, _T("%s\\%s\\MARK-%d.bmp"), BASE_MODEL_PATH, g_clSysData.m_szModelName, nMarkNo + 1);

            MIL.MbufClear(Globalo.visionManager.markUtil.m_MilMarkOverlay, Globalo.visionManager.markUtil.m_lTransparentColor);
            MIL.MbufClear(Globalo.visionManager.markUtil.m_MilMarkImage, 192);
        }
        public void InitMaskViewDlg(int nMarkNo, int m_iSizeX , int m_iSizeY)
        {
            //panel_MarkZoomImage
            
            

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, DispSize.X, DispSize.Y, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref Globalo.visionManager.markUtil.m_MilMarkImage);

            if (Globalo.visionManager.markUtil.m_MilMarkImage != MIL.M_NULL)
            {
                if (Globalo.visionManager.markUtil.m_MilMarkDisplay != MIL.M_NULL)
                {
                    MIL.MbufClear(Globalo.visionManager.markUtil.m_MilMarkDisplay, 0);
                    Globalo.visionManager.markUtil.m_MilMarkDisplay = MIL.M_NULL;
                    m_MilMaskOverlay = MIL.M_NULL;
                    Globalo.visionManager.markUtil.m_MilMarkDisplay = MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, MIL.M_NULL);

                    MIL.MdispSelectWindow(Globalo.visionManager.markUtil.m_MilMarkDisplay, Globalo.visionManager.markUtil.m_MilMarkImage, panel_MarkZoomImage.Handle);
                }
            }
        }
    }
}
