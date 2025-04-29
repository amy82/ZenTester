using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;

namespace ZenHandler.VisionClass
{
    public class AoiTester
    {
        public AoiTester()
        {

        }
        public void CirCleFind(int index)
        {
            Console.WriteLine("CirCleFind");
            Rectangle m_clRectCalRoi = new Rectangle((int)(100), (int)(100), 1000, 1000);

            MIL_ID MilChildLow = MIL.M_NULL;

            //MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilCamGrabImage[index],m_clRectCalRoi.X, m_clRectCalRoi.Y, m_clRectCalRoi.Width , m_clRectCalRoi.Height, ref MilChildLow);
            MIL_ID MilSubImage01 = MIL.M_NULL;
            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilCamGrabImage[index], 0L, 0L, Globalo.visionManager.milLibrary.CAM_SIZE_X, Globalo.visionManager.milLibrary.CAM_SIZE_Y, ref MilSubImage01);
            MIL.MimBinarize(Globalo.visionManager.milLibrary.MilCamGrabImage[index], MilSubImage01, MIL.M_BIMODAL + MIL.M_GREATER, MIL.M_NULL, MIL.M_NULL);

            MIL.MimRank(MilSubImage01, MilSubImage01, MIL.M_3X3_RECT, MIL.M_MEDIAN, MIL.M_BINARY);
            MIL.MbufExport("D:\\TEST.BMP", MIL.M_BMP, MilSubImage01);
        }
    }
}
