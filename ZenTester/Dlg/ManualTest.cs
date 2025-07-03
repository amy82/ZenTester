using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;

namespace ZenTester.Dlg
{
    public partial class ManualTest : UserControl
    {
        private SetTestControl parentDlg;
        public int MarkIndex = 0;
        public int MaxMarkCount = 0;
        //public int CamIndex = 0;
        ///private OpenCvSharp.Point[] centerPos = new OpenCvSharp.Point[2];
        ///
        private OpenCvSharp.Point[] TopCenterPos = new OpenCvSharp.Point[2];

        private System.Drawing.Point roiStart;
        private System.Drawing.Point roiEnd;

        private object AoiTaskLock = new object();
        private Task AoiTask = null;
        private CancellationTokenSource CancelToken = new CancellationTokenSource();

        public ManualTest(SetTestControl _parent)
        {
            InitializeComponent();
            parentDlg = _parent;
            MaxMarkCount = Globalo.yamlManager.aoiRoiConfig.markData.Count;
        }

        public void SetSmallMark()
        {
            VisionClass.eMarkList mark = (VisionClass.eMarkList)MarkIndex;
            string markName = mark.ToString();
            
            label_Set_Mark_Model.Text = markName;// Globalo.yamlManager.aoiRoiConfig.markData[MarkIndex].name;
            string model = Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.Ppid;

            double sizeX = Globalo.visionManager.markUtil.zoomDispSize.X;
            double sizeY = Globalo.visionManager.markUtil.zoomDispSize.Y;

            Globalo.visionManager.markUtil.DisplaySmallMarkView(model, MarkIndex, sizeX, sizeY);    //Prev Click
        }

        private void button_Set_Mark_Prev_Click(object sender, EventArgs e)
        {
            //Prev
            if (MarkIndex > 0)
            {
                MarkIndex--;
                SetSmallMark();
            }
            else
            {
                MarkIndex = 0;
            }

        }
        private void button_Set_Mark_Next_Click(object sender, EventArgs e)
        {
            //Next
            //public enum eMarkList{SIDE_CONTACT = 0, SIDE_CONE, SIDE_ORING, TOP_KEY, MAX_MARK_LIST}
            int kk = (int)VisionClass.eMarkList.MAX_MARK_LIST;

            if (MarkIndex < MaxMarkCount - 1)
            {
                MarkIndex++;
                SetSmallMark();
            }
            else
            {
                MarkIndex = MaxMarkCount - 1;
            }


        }
        private void button_Pogo_Find_Test_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex];
            int dataSize = sizeX * sizeY;

            byte[] ImageBuffer = new byte[dataSize];
            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[parentDlg.CamIndex], ImageBuffer);

            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);


            //Globalo.visionManager.aoiTopTester.FindPogoPinCenter(CamIndex, src);    //포고핀 중심찾기

            bool rtn = Globalo.visionManager.aoiTopTester.FindCircleCenter(parentDlg.CamIndex, src, ref TopCenterPos[parentDlg.CamIndex]);     //가장 작은 원의 중심 찾기
        }
        #region [TOP CAMERA MANUAL TEST]
        private void button_Set_Key_Test_Click(object sender, EventArgs e)
        {
            bool rtn = true;
            parentDlg.manualConfig.checkBox_AllRelease();
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex];
            int dataSize = sizeX * sizeY;


            //A ~ D: 2개씩
            //E 타입만 1개
            string keyType = Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["KEYTYPE"].value;


            int key1Rtn = 0;
            int key2Rtn = 0;
            double offsetx = 0.0;
            double offsety = 0.0;
            double dKeyScore = 0.0;

            byte[] ImageBuffer = new byte[dataSize];
            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[parentDlg.CamIndex], ImageBuffer);
            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);

            //rtn = Globalo.visionManager.aoiTopTester.FindCircleCenter(parentDlg.CamIndex, src, ref TopCenterPos[parentDlg.CamIndex], true);     //가장 작은 원의 중심 찾기
            //if (rtn)
            //{
            //    offsetx = TopCenterPos[parentDlg.CamIndex].X - parentDlg.centerPos[parentDlg.CamIndex].X;
            //    offsety = TopCenterPos[parentDlg.CamIndex].Y - parentDlg.centerPos[parentDlg.CamIndex].Y;
            //}


            OpenCvSharp.Point markPos = new OpenCvSharp.Point();
            bool bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(parentDlg.CamIndex, VisionClass.eMarkList.TOP_KEY, ref markPos, ref dKeyScore);

            //key1Rtn = Globalo.visionManager.aoiTopTester.MilEdgeKeytest(parentDlg.CamIndex, 0, keyType, offsetx, offsety);        //키검사
            //if (keyType != "E")
            //{
            //    key2Rtn = Globalo.visionManager.aoiTopTester.MilEdgeKeytest(parentDlg.CamIndex, 1, keyType, offsetx, offsety);        //키검사
            //}



            string str = string.Empty;
            System.Drawing.Point clPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex] - 300);
            //str = $"Key {keyType} - {key1Rtn} / {key2Rtn} ";
            if (bRtn)//key1Rtn == 1 && key2Rtn == 1)
            {
                //성공

                Globalo.visionManager.milLibrary.m_clMilDrawText[parentDlg.CamIndex].AddList(clPoint, str, "나눔고딕", Color.GreenYellow, 13);
            }
            else
            {
                Globalo.visionManager.milLibrary.m_clMilDrawText[parentDlg.CamIndex].AddList(clPoint, str, "나눔고딕", Color.Red, 13);
            }


            




            Globalo.visionManager.milLibrary.DrawOverlayAll(parentDlg.CamIndex, 0);

        }

        private void button_Set_Housing_Test_Click(object sender, EventArgs e)
        {
            bool rtn = true;
            parentDlg.manualConfig.checkBox_AllRelease();
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex];
            int dataSize = sizeX * sizeY;


            byte[] ImageBuffer = new byte[dataSize];

            //
            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[parentDlg.CamIndex], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);

            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);



            List<OpenCvSharp.Point> FakraCenter = new List<OpenCvSharp.Point>();
            List<OpenCvSharp.Point> HousingCenter = new List<OpenCvSharp.Point>();

            //----------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            //Center Find
            //
            //----------------------------------------------------------------------------------------------------------------------------------------------
            OpenCvSharp.Point markPos = new OpenCvSharp.Point(0, 0);
            double score = 0.0;
            bool bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(parentDlg.CamIndex, VisionClass.eMarkList.TOP_CENTER, ref markPos, ref score);

            
            //----------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            //
            //
            //----------------------------------------------------------------------------------------------------------------------------------------------
            FakraCenter = Globalo.visionManager.aoiTopTester.Housing_Fakra_Test(parentDlg.CamIndex, src, markPos);     //Fakra 안쪽 원 찾기
            HousingCenter = Globalo.visionManager.aoiTopTester.Housing_Dent_Test(parentDlg.CamIndex, src, markPos);    //Con1,2(동심도)  / Dent (찌그러짐) 검사 

            if (FakraCenter.Count < 2)
            {
                Console.WriteLine($"In Fakra Find Fail:{FakraCenter.Count}");
                return;
            }
            if (HousingCenter.Count < 2)
            {
                Console.WriteLine($"Out Fakra Find Fail:{HousingCenter.Count}");
                return;
            }
            double CamResolX = 0.0;
            double CamResolY = 0.0;

            double con1Result = 0.0;
            double con2Result = 0.0;
            CamResolX = Globalo.yamlManager.configData.CamSettings.TopResolution.X;   // 0.0186f;
            CamResolY = Globalo.yamlManager.configData.CamSettings.TopResolution.Y;   // 0.0186f;


            OpenCvSharp.Point c1 = FakraCenter[1];
            OpenCvSharp.Point c2 = HousingCenter[0];
            OpenCvSharp.Point c3 = HousingCenter[1];
            string str = "";

            float dx = c1.X - c2.X;
            float dy = c1.Y - c2.Y;
            float dist = (float)Math.Sqrt(dx * dx + dy * dy);

            System.Drawing.Point ConePoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex] - 590);
            con1Result = dist * CamResolX;

            str = $"Con1:{con1Result.ToString("0.00#")}";
            //Globalo.visionManager.milLibrary.DrawOverlayText(parentDlg.CamIndex, ConePoint, str, Color.GreenYellow, 13);
            Globalo.visionManager.milLibrary.m_clMilDrawText[parentDlg.CamIndex].AddList(ConePoint, str, "나눔고딕", Color.GreenYellow, 13);
            Console.WriteLine($"Con1:{dist * CamResolX}");

            dx = c1.X - c3.X;
            dy = c1.Y - c3.Y;

            dist = (float)Math.Sqrt(dx * dx + dy * dy);

            con2Result = dist * CamResolX;
            Console.WriteLine($"Con2:{con2Result}");

            ConePoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex] - 520);
            str = $"Con2:{con2Result.ToString("0.00#")}";
            //Globalo.visionManager.milLibrary.DrawOverlayText(parentDlg.CamIndex, ConePoint, str, Color.GreenYellow, 13);
            Globalo.visionManager.milLibrary.m_clMilDrawText[parentDlg.CamIndex].AddList(ConePoint, str, "나눔고딕", Color.GreenYellow, 13);


            Globalo.visionManager.milLibrary.DrawOverlayAll(parentDlg.CamIndex);



            string str22 = "";
            string csvLine = $"{con1Result}, {con2Result}";

            string filePath = "CONdata.csv";
            // 파일이 없으면 헤더 추가
            if (!File.Exists(filePath))
            {
                File.AppendAllText(filePath, "CON1, CON2" + Environment.NewLine);
            }
            try
            {
                File.AppendAllText(filePath, csvLine + Environment.NewLine);
            }
            catch (IOException)
            {

            }
        }

        private void button_Set_Gasket_Test_Click(object sender, EventArgs e)
        {
            parentDlg.manualConfig.checkBox_AllRelease();
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex];
            int dataSize = sizeX * sizeY;


            byte[] ImageBuffer = new byte[dataSize];

            //MIL.MbufGet(Globalo.visionManager.milLibrary.MilCamGrabImageChild[CamIndex], ImageBuffer);
            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[parentDlg.CamIndex], ImageBuffer);

            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);



            double dScore = 0.0;
            //bool rtn = Globalo.visionManager.aoiTopTester.FindCircleCenter(parentDlg.CamIndex, src, ref TopCenterPos[parentDlg.CamIndex]);     //가장 작은 원의 중심 찾기
            bool rtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(parentDlg.CamIndex, VisionClass.eMarkList.TOP_CENTER, ref TopCenterPos[parentDlg.CamIndex], ref dScore);
            if (rtn)
            {
                Globalo.visionManager.aoiTopTester.GasketTest(parentDlg.CamIndex, src, TopCenterPos[parentDlg.CamIndex]);     //가스켓 검사
            }


            Globalo.visionManager.milLibrary.DrawOverlayAll(parentDlg.CamIndex);
        }

        private void button_Set_Dent_Test_Click(object sender, EventArgs e)
        {
            bool rtn = true;
            parentDlg.manualConfig.checkBox_AllRelease();
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex];
            int dataSize = sizeX * sizeY;



            //
            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);

            byte[] ImageBuffer = new byte[dataSize];
            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[parentDlg.CamIndex], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);

            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);

            //----------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            //Center Find
            //
            //----------------------------------------------------------------------------------------------------------------------------------------------
            OpenCvSharp.Point markPos = new OpenCvSharp.Point(0, 0);
            double score = 0.0;
            bool bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(parentDlg.CamIndex, VisionClass.eMarkList.TOP_CENTER, ref markPos, ref score, true);



            List<OpenCvSharp.Point> HousingCenter = new List<OpenCvSharp.Point>();
            HousingCenter = Globalo.visionManager.aoiTopTester.Housing_Dent_Test(parentDlg.CamIndex, src, markPos,  true);  //Con1,2(동심도)  / Dent (찌그러짐) 검사 

            Globalo.visionManager.milLibrary.DrawOverlayAll(parentDlg.CamIndex, 0);

        }
        #endregion

        #region [SIDE CAMERA MANUAL TEST]
        private void button_Set_Oring_Test_Click(object sender, EventArgs e)
        {
            if (parentDlg.CamIndex == 0)
            {
                return;
            }
            parentDlg.manualConfig.checkBox_AllRelease();
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex];
            int dataSize = sizeX * sizeY;

            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);

            OpenCvSharp.Point markPos = new OpenCvSharp.Point();
            double score = 0.0;
            //bool bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(parentDlg.CamIndex, VisionClass.eMarkList.SIDE_ORING, ref markPos, ref score);
            bool bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(parentDlg.CamIndex, VisionClass.eMarkList.SIDE_ORING, ref markPos, ref score, true);

            System.Drawing.Point OffsetPos = new System.Drawing.Point(0, 0);
            //if (bRtn)
            //{
            //    OffsetPos.X = markPos.X - (Globalo.yamlManager.aoiRoiConfig.ORING_ROI[0].X + (Globalo.yamlManager.aoiRoiConfig.ORING_ROI[0].Width / 2));
            //    OffsetPos.Y = markPos.Y - (Globalo.yamlManager.aoiRoiConfig.ORING_ROI[0].Y + (Globalo.yamlManager.aoiRoiConfig.ORING_ROI[0].Height / 2));

            //}
            //Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            //Globalo.visionManager.aoiSideTester.MilEdgeOringTest(parentDlg.CamIndex, 0, OffsetPos);

            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);

            //Globalo.visionManager.milLibrary.DrawOverlayAll(parentDlg.CamIndex);
        }

        private void button_Set_Cone_Test_Click(object sender, EventArgs e)
        {
            if (parentDlg.CamIndex == 0)
            {
                return;
            }
            parentDlg.manualConfig.checkBox_AllRelease();
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex];
            int dataSize = sizeX * sizeY;

            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);

            OpenCvSharp.Point markPos = new OpenCvSharp.Point();
            double score = 0.0;
            ///bool bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(parentDlg.CamIndex, VisionClass.eMarkList.SIDE_CONE, ref markPos, ref score);
            bool bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(parentDlg.CamIndex, VisionClass.eMarkList.SIDE_CONE, ref markPos, ref score, true);

            //System.Drawing.Point OffsetPos = new System.Drawing.Point(0, 0);
            //if (bRtn)
            //{
            //    OffsetPos.X = markPos.X - (Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].X + (Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].Width / 2));
            //    OffsetPos.Y = markPos.Y - (Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].Y + (Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].Height / 2));
            //}
            //Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            //Globalo.visionManager.aoiSideTester.MilEdgeConeTest(parentDlg.CamIndex, 0, OffsetPos);//, src);


            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);
            //Globalo.visionManager.milLibrary.DrawOverlayAll(parentDlg.CamIndex);
        }

        private void button_Set_Height_Test_Click(object sender, EventArgs e)
        {
            if (parentDlg.CamIndex == 0)
            {
                return;
            }
            parentDlg.manualConfig.checkBox_AllRelease();

            Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex];
            int dataSize = sizeX * sizeY;


            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);

           
            Globalo.visionManager.aoiSideTester.HeightTest(parentDlg.CamIndex); 
            //
            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);
        }
        #endregion
        public void showMark()
        {
            VisionClass.eMarkList mark = (VisionClass.eMarkList)MarkIndex;
            string markName = mark.ToString();

            label_Set_Mark_Model.Text = markName;// Globalo.yamlManager.aoiRoiConfig.markData[MarkIndex].name;
        }
        
        private void label_SetTest_Manual_Mark_Regist_Click(object sender, EventArgs e)
        {
            //double dSizeX, double dSizeY, double dCenterX, double dCenterY
            Rectangle DrawRoiBox = parentDlg.GetRoiRect();

            double dSizeX = DrawRoiBox.Width;
            double dSizeY = DrawRoiBox.Height;
            double dCenterX = DrawRoiBox.X + (DrawRoiBox.Width / 2);
            double dCenterY = DrawRoiBox.Y + (DrawRoiBox.Height / 2);

            Console.WriteLine($"Mark Roi Pos = Center X:{dCenterX}, Center Y:{dCenterY}");
            Console.WriteLine($"Mark Roi Size = Width:{dSizeX}, Height:{dSizeY}");

            if (dSizeX > 1024 || dSizeY > 1024)
            {
                Console.WriteLine($"Mark Roi Size Over = Width:{dSizeX}, Height:{dSizeY}");
                return;
            }
            if (dSizeX < 10 || dSizeY < 10)
            {
                Console.WriteLine($"Mark Roi Size Less = Width:{dSizeX}, Height:{dSizeY}");
                return;
            }

            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);        //Cam Grab

            Globalo.visionManager.markUtil.RegisterMark(parentDlg.CamIndex, MarkIndex, DrawRoiBox.X, DrawRoiBox.Y, dSizeX, dSizeY);

            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);
        }

        private void label_SetTest_Manual_Mark_View_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.markUtil.ViewMarkMask(parentDlg.CamIndex, MarkIndex);
        }

        private void label_SetTest_Manual_Mark_Find_Click(object sender, EventArgs e)
        {
            double dScore = 0.0;
            VisionClass.CDMotor dAlign = new VisionClass.CDMotor();

            Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);

            //


            Globalo.visionManager.markUtil.CalcSingleMarkAlign(parentDlg.CamIndex, MarkIndex, ref dAlign, ref dScore);

            Console.WriteLine($"X:{dAlign.X},Y: {dAlign.Y}, T:{dAlign.T}");

            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);
        }



        private void label_SetTest_Manual_Mark_Roi_Save_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.None;
            result = Globalo.MessageAskPopup("MARK ROI영역 등록하시겠습니까?");

            if (result == DialogResult.Yes)
            {
                Rectangle DrawRoiBox = parentDlg.GetRoiRect();

                Globalo.yamlManager.aoiRoiConfig.markData[MarkIndex].X = (int)(DrawRoiBox.X * Globalo.visionManager.milLibrary.xExpand[parentDlg.CamIndex] + 0.5);
                Globalo.yamlManager.aoiRoiConfig.markData[MarkIndex].Y = (int)(DrawRoiBox.Y * Globalo.visionManager.milLibrary.yExpand[parentDlg.CamIndex] + 0.5);
                Globalo.yamlManager.aoiRoiConfig.markData[MarkIndex].Width = (int)(DrawRoiBox.Width * Globalo.visionManager.milLibrary.xExpand[parentDlg.CamIndex] + 0.5);
                Globalo.yamlManager.aoiRoiConfig.markData[MarkIndex].Height = (int)(DrawRoiBox.Height * Globalo.visionManager.milLibrary.yExpand[parentDlg.CamIndex] + 0.5);

                Data.TaskDataYaml.Save_AoiConfig();
            }
        }

        private void label_SetTest_Manual_Image_Save_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Bitmap Image (*.bmp)|*.bmp";
                saveFileDialog.Title = "이미지 저장";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = saveFileDialog.FileName;
                    //grabbedImage.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    Globalo.visionManager.milLibrary.GrabImageSave(parentDlg.CamIndex, selectedFilePath);


                    Console.WriteLine("선택한 이미지 경로:\n" + selectedFilePath);
                }
            }
        }

        private void label_SetTest_Manual_Image_Load_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "이미지 파일 선택";
                //openFileDialog.Filter = "이미지 파일 (*.png;*.jpg;*.jpeg;*.bmp;*.gif)|*.png;*.jpg;*.jpeg;*.bmp;*.gif|모든 파일 (*.*)|*.*";
                openFileDialog.Filter = "이미지 파일 (*.bmp;)|*.bmp;|모든 파일 (*.*)|*.*";
                openFileDialog.InitialDirectory = "D:\\Work\\Pro_Ject\\Mexico\\Aoi\\_temp\\newCam"; ;// Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);
                    string selectedFilePath = openFileDialog.FileName;
                    Globalo.visionManager.SetTestLoadBmp(parentDlg.CamIndex, selectedFilePath);

                    Console.WriteLine("선택한 이미지 경로:\n" + selectedFilePath);
                    ///MessageBox.Show("선택한 이미지 경로:\n" + selectedFilePath);
                    ///
                    Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);
                }
                else
                {
                    Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);
                }
            }
        }
        public void RefreshTest()
        {
            showMark();
        }
        private void ManualTest_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                RefreshTest();
                
            }
            else
            {

            }
        }
        public void hidePanel()
        {
            
        }

        private void button_Mark_Top_Center_Find_Click(object sender, EventArgs e)
        {
            parentDlg.manualConfig.checkBox_AllRelease();
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex];
            int dataSize = sizeX * sizeY;

            Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, false);
            Globalo.visionManager.milLibrary.GetSnapImage(parentDlg.CamIndex);

            OpenCvSharp.Point markPos = new OpenCvSharp.Point(0,0);
            double score = 0.0;
            bool bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(parentDlg.CamIndex, VisionClass.eMarkList.TOP_CENTER, ref markPos, ref score, true);

        }

        private void button_Top_Manual_Auto_Click(object sender, EventArgs e)
        {
            //--------------------------------------------------------------------------------------------------------------
            //
            //
            // TOP MANUAL AUTO
            //
            //
            //
            //--------------------------------------------------------------------------------------------------------------
            if (parentDlg.CamIndex == 1)
            {
                return;
            }
            lock (AoiTaskLock)
            {
                // 이미 실행 중이면 무시
                if (AoiTask != null && !AoiTask.IsCompleted)
                {
                    Console.WriteLine("이미 검사 중입니다.");
                    return;
                }

                AoiTask = Task.Run(() =>
                {
                    Console.WriteLine("TOP MANUAL AUTO START");
                    int waitverify = 1;

                    try
                    {
                        Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);
                        Globalo.threadControl.testAutoThread.aoiTestFlow.TopCamFlow(false);
                        Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("예외 발생: " + ex.Message);
                    }

                    return waitverify;
                }, CancelToken.Token);
            }
        }

        private void button_Side_Manual_Auto_Click(object sender, EventArgs e)
        {
            if (parentDlg.CamIndex == 0)
            {
                return;
            }
            //--------------------------------------------------------------------------------------------------------------
            //
            //
            // SIDE MANUAL AUTO
            //
            //
            //
            //--------------------------------------------------------------------------------------------------------------

            lock (AoiTaskLock)
            {
                // 이미 실행 중이면 무시
                if (AoiTask != null && !AoiTask.IsCompleted)
                {
                    Console.WriteLine("이미 검사 중입니다.");
                    return;
                }

                AoiTask = Task.Run(() =>
                {
                    Console.WriteLine("SIDE MANUAL AUTO START");
                    int waitverify = 1;

                    try
                    {
                        Globalo.visionManager.milLibrary.ClearOverlay_Manual(parentDlg.CamIndex);
                        Globalo.threadControl.testAutoThread.aoiTestFlow.SideCamFlow(false);
                        Globalo.visionManager.milLibrary.SetGrabOn(parentDlg.CamIndex, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("예외 발생: " + ex.Message);
                    }

                    return waitverify;
                }, CancelToken.Token);
            }
        }
    }
}
