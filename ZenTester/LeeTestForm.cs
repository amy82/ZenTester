using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;

namespace ZenTester
{
    public partial class LeeTestForm : Form
    {
        public LeeTestForm()
        {
            InitializeComponent();
            this.TopMost = true;
            this.CenterToScreen();
        }

        private void button_Con1_Test_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTester.Con1Test(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTester.Con2Test(1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTester.SimpleCircleSearchExample(0);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTester.SimpleCircleSearchExample(1);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTester.MmetTest(0);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTester.MmetTest(1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTester.ChangeBinary1(0);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTester.ChangeBinary1(1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTester.RunMeasCase(0);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTester.RunMeasCase(1);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.opencvTester.houghCircleFine(0);

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.opencvTester.houghCircleFine(1);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.opencvTester.ContoursCircleFine(0);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.opencvTester.KeyFine(0);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.ClearOverlay(0);

            
        }

        private void button15_Click(object sender, EventArgs e)
        {
            
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.opencvTester.MilPopup(0);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.opencvTester.OpencvKeyCheck(0);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTopTester.Run(0);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.aoiTester.ComplexCircleSearchExample1(0);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Rectangle clRect = new Rectangle();
            clRect.X = 100;
            clRect.Y = 100;
            clRect.Width = 300;
            clRect.Height = 300;
            Globalo.visionManager.milLibrary.m_clMilDrawBox[0].AddList(clRect, 2, Color.Blue, System.Drawing.Drawing2D.DashStyle.Dot);
            Globalo.visionManager.milLibrary.m_clMilDrawCircle[0].AddList(500, 500, 100, 1, DashStyle.Solid, Color.Red);
            Globalo.visionManager.milLibrary.DrawOverlayAll(0,0);
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Globalo.tcpManager.ReqRecipeToSecsgem();
            Globalo.tcpManager.ReqModelToSecsgem();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            //verify object
            TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
            EqipData.Type = "EquipmentData";

            TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
            sendEqipData.Command = "OBJECT_ID_REPORT";
            sendEqipData.BcrId = "testLot-1";
            sendEqipData.DataID = "1";
            EqipData.Data = sendEqipData;
            Globalo.tcpManager.SendMessage_To_SecsGem(EqipData);        //test 

            Thread.Sleep(100);

            //sendEqipData.BcrId = "testLot-222";
            //sendEqipData.DataID = "1";
            //EqipData.Data = sendEqipData;
            //Globalo.tcpManager.SendMessage_To_SecsGem(EqipData);        //test 
        }

        private void button23_Click(object sender, EventArgs e)
        {
            //verify apd
            TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
            EqipData.Type = "EquipmentData";

            TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
            sendEqipData.Command = "LOT_APD_REPORT";
            sendEqipData.BcrId = "testLot"; //Globalo.dataManage.TaskWork.m_szChipID;
            sendEqipData.Judge = 1;         /// Globalo.taskWork.m_nTestFinalResult;

            //1.Socket_Num
            //2.Result
            //3.Barcode
            //4.SensorID
            int tCount = 4;
            string[] apdList = { "Socket_Num", "Result", "Barcode", "SensorID" };
            string[] apdResult = { "11", "22", "33", "44" };

            for (int i = 0; i < tCount; i++)
            {
                TcpSocket.EquipmentParameterInfo pInfo = new TcpSocket.EquipmentParameterInfo();

                pInfo.Name = apdList[i];
                pInfo.Value = apdResult[i];

                sendEqipData.CommandParameter.Add(pInfo);
            }

            EqipData.Data = sendEqipData;
            Globalo.tcpManager.SendMessage_To_SecsGem(EqipData);        //test
        }

        private void button24_Click(object sender, EventArgs e)
        {
            
            TcpSocket.TesterData data = new TcpSocket.TesterData();
            data.init();
            //data.BcrId = new string[1];
            data.BcrId[0] = textBoxBcr.Text;
            data.socketNum[0] = 1;
            Globalo.taskManager.Aoi_TestRun(data);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            OpenCvSharp.Point ConePos = new OpenCvSharp.Point();
            Globalo.visionManager.milLibrary.ClearOverlay(1);

            Globalo.visionManager.milLibrary.SetGrabOn(1, false);
            Globalo.visionManager.milLibrary.GetSnapImage(1);
            //
            //
            double score = 0.0;
            Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(1, VisionClass.eMarkList.SIDE_HEIGHT, ref ConePos, ref score);

            Console.WriteLine($"x:{ConePos.X},y:{ConePos.Y}");
        }

        private void button26_Click(object sender, EventArgs e)
        {
            int camnum = 0;
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(camnum);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[camnum];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[camnum];
            int dataSize = sizeX * sizeY;

            Globalo.visionManager.milLibrary.SetGrabOn(camnum, false);
            Globalo.visionManager.milLibrary.GetSnapImage(camnum);

            byte[] ImageBuffer = new byte[dataSize];
            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[camnum], ImageBuffer);

            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);


            MIL_ID roiMilImage = MIL.M_NULL;

            double offsetx = 0.0;
            double offsety = 0.0;
            int startX = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[0].X + (int)offsetx;
            int startY = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[0].Y + (int)offsety;

            int OffsetWidth = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[0].Width;
            int OffsetHeight = Globalo.yamlManager.aoiRoiConfig.KEY_ROI[0].Height;

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, OffsetWidth, OffsetHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref roiMilImage);

            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilProcImageChild[0], startX, startY, OffsetWidth, OffsetHeight, ref roiMilImage);

            Globalo.visionManager.aoiTopTester.OpencvKeytest(roiMilImage);

        }

        private void button27_Click(object sender, EventArgs e)
        {
            bool rtn = true;

            int index = 0;
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(index);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[index];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[index];
            int dataSize = sizeX * sizeY;


            byte[] ImageBuffer = new byte[dataSize];

            //
            Globalo.visionManager.milLibrary.SetGrabOn(index, false);
            Globalo.visionManager.milLibrary.GetSnapImage(index);

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[index], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);

            Globalo.visionManager.milLibrary.SetGrabOn(index, true);



            List<OpenCvSharp.Point> FakraCenter = new List<OpenCvSharp.Point>();
            List<OpenCvSharp.Point> HousingCenter = new List<OpenCvSharp.Point>();


            Globalo.visionManager.aoiTopTester.Housing_EdgeFind_Test(index, src);     //Fakra 안쪽 원 찾기
        }

        private void button28_Click(object sender, EventArgs e)
        {
            TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
            EqipData.Type = "EquipmentData";

            TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
            sendEqipData.Command = "LOT_APD_REPORT";
            sendEqipData.BcrId = "testLot"; //Globalo.dataManage.TaskWork.m_szChipID;
            sendEqipData.Judge = 1;         /// Globalo.taskWork.m_nTestFinalResult;
            sendEqipData.DataID = "1";
            //1.Socket_Num
            //2.Result
            //3.Barcode
            //4.SensorID
            int tCount = 4;
            //string[] apdList = { "Socket_Num", "Result", "Barcode", "SensorID" };
            //string[] apdResult = { "11", "22", "33", "44" };
            string[] apdList = {"Checksum0", "Checksum1", "Checksum2", "Checksum3", "Checksum4", "Socket_Num", "Result", "Barcode", "SensorID", "Time" };
            string[] apdResult = { "01"};

            for (int i = 0; i < tCount; i++)
            {
                TcpSocket.EquipmentParameterInfo pInfo = new TcpSocket.EquipmentParameterInfo();

                pInfo.Name = apdList[i];
                pInfo.Value = "01";

                sendEqipData.CommandParameter.Add(pInfo);
            }

            EqipData.Data = sendEqipData;
            Globalo.tcpManager.SendMessage_To_SecsGem(EqipData);        //test
        }

        private void button29_Click(object sender, EventArgs e)
        {
            TcpSocket.AoiApdData _testData = new TcpSocket.AoiApdData();
            _testData.Socket_Num = "1";
            _testData.Barcode = "lot123";



            TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
            TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
            EqipData.Type = "EquipmentData";
            sendEqipData.Command = "LOT_APD_REPORT";

            sendEqipData.DataID = _testData.Socket_Num;
            sendEqipData.BcrId = _testData.Barcode;
            sendEqipData.Judge = 1;
            sendEqipData.CommandParameter.Clear();
            string[] apdList = {
                        "LH", "RH", "MH",  "Gasket", "KeyType", "CircleDented" , "Concentrycity_A", "Concentrycity_D", "Cone", "ORing"
                        , "Result" , "Barcode", "Socket_Num" };

            string[] apdResult = { _testData.LH, _testData.RH, _testData.MH,
                        _testData.Gasket, _testData.KeyType,_testData.CircleDented, _testData.Concentrycity_A, _testData.Concentrycity_D,
                        _testData.Cone, _testData.ORing, _testData.Result ,_testData.Barcode, _testData.Socket_Num};

            for (int i = 0; i < apdResult.Length; i++)
            {
                TcpSocket.EquipmentParameterInfo pInfo = new TcpSocket.EquipmentParameterInfo();

                pInfo.Name = apdList[i];
                pInfo.Value = apdResult[i];

                sendEqipData.CommandParameter.Add(pInfo);
            }
            EqipData.Data = sendEqipData;
            Globalo.tcpManager.nRecv_Ack = -1;
            Globalo.tcpManager.SendMessage_To_SecsGem(EqipData);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            //작은원 동심도
            bool rtn = true;
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(VisionClass.AoiTester.TOP_INDEX);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[VisionClass.AoiTester.TOP_INDEX];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[VisionClass.AoiTester.TOP_INDEX];
            int dataSize = sizeX * sizeY;


            byte[] ImageBuffer = new byte[dataSize];

            //
            Globalo.visionManager.milLibrary.SetGrabOn(VisionClass.AoiTester.TOP_INDEX, false);
            Globalo.visionManager.milLibrary.GetSnapImage(VisionClass.AoiTester.TOP_INDEX);

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[VisionClass.AoiTester.TOP_INDEX], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);
            // 3채널로 변환
            Cv2.CvtColor(src, src, ColorConversionCodes.GRAY2BGR);
            int sizeX2 = Globalo.visionManager.milLibrary.CAM_SIZE_X[VisionClass.AoiTester.TOP_INDEX];
            int sizeY2 = Globalo.visionManager.milLibrary.CAM_SIZE_Y[VisionClass.AoiTester.TOP_INDEX];
            int dataSize2 = sizeX2 * sizeY2;
            byte[] ImageBuffer2 = new byte[dataSize2];
            //
            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[VisionClass.AoiTester.TOP_INDEX], ImageBuffer2);
            //Mat src2 = new Mat(sizeY2, sizeX2, MatType.CV_8UC1);
            //Marshal.Copy(ImageBuffer2, 0, src2.Data, dataSize2);
            //string sidepath = $"d:\\srcImage_{topcount}.jpg";
            //Cv2.ImWrite(sidepath, src2);

            Globalo.visionManager.milLibrary.SetGrabOn(VisionClass.AoiTester.TOP_INDEX, true);



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
            bool bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(VisionClass.AoiTester.TOP_INDEX, VisionClass.eMarkList.TOP_CENTER, ref markPos, ref score);


            //----------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            //
            //
            //----------------------------------------------------------------------------------------------------------------------------------------------
            FakraCenter = Globalo.visionManager.aoiTopTester.Housing_Fakra_Test(VisionClass.AoiTester.TOP_INDEX, src, markPos, false);     //Fakra 안쪽 원 찾기
        }

        private void button31_Click(object sender, EventArgs e)
        {
            //큰원 동심도
            bool rtn = true;
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(VisionClass.AoiTester.TOP_INDEX);

            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[VisionClass.AoiTester.TOP_INDEX];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[VisionClass.AoiTester.TOP_INDEX];
            int dataSize = sizeX * sizeY;


            byte[] ImageBuffer = new byte[dataSize];

            //
            Globalo.visionManager.milLibrary.SetGrabOn(VisionClass.AoiTester.TOP_INDEX, false);
            Globalo.visionManager.milLibrary.GetSnapImage(VisionClass.AoiTester.TOP_INDEX);

            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[VisionClass.AoiTester.TOP_INDEX], ImageBuffer);
            Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
            Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);
            Cv2.CvtColor(src, src, ColorConversionCodes.GRAY2BGR);
            int sizeX2 = Globalo.visionManager.milLibrary.CAM_SIZE_X[VisionClass.AoiTester.TOP_INDEX];
            int sizeY2 = Globalo.visionManager.milLibrary.CAM_SIZE_Y[VisionClass.AoiTester.TOP_INDEX];
            int dataSize2 = sizeX2 * sizeY2;
            byte[] ImageBuffer2 = new byte[dataSize2];
            //
            MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[VisionClass.AoiTester.TOP_INDEX], ImageBuffer2);
            //Mat src2 = new Mat(sizeY2, sizeX2, MatType.CV_8UC1);
            //Marshal.Copy(ImageBuffer2, 0, src2.Data, dataSize2);
            //string sidepath = $"d:\\srcImage_{topcount}.jpg";
            //Cv2.ImWrite(sidepath, src2);

            Globalo.visionManager.milLibrary.SetGrabOn(VisionClass.AoiTester.TOP_INDEX, true);



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
            bool bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(VisionClass.AoiTester.TOP_INDEX, VisionClass.eMarkList.TOP_CENTER, ref markPos, ref score);


            //----------------------------------------------------------------------------------------------------------------------------------------------
            //
            //
            //
            //
            //----------------------------------------------------------------------------------------------------------------------------------------------
            HousingCenter = Globalo.visionManager.aoiTopTester.Housing_Dent_Test(VisionClass.AoiTester.TOP_INDEX, src, markPos, false, false);    //Con1,2(동심도)  / Dent (찌그러짐) 검사 
        }

        private void button32_Click(object sender, EventArgs e)
        {
            TcpSocket.MessageWrapper objectData = new TcpSocket.MessageWrapper();
            objectData.Type = "TesterData";

            //TcpSocket.EquipmentData LotstartData = new TcpSocket.EquipmentData();
            TcpSocket.TesterData resultData = new TcpSocket.TesterData();
            resultData.init();
            resultData.BcrId[0] = "testLog1";
            resultData.BcrId[1] = "testLog2";
            resultData.BcrId[2] = "testLog3";
            resultData.BcrId[3] = "testLog4";
            resultData.Cmd = "CMD_RESULT";
            resultData.socketNum[0] = 1;
            resultData.socketNum[1] = 2;
            resultData.socketNum[2] = 3;
            resultData.socketNum[3] = 4;
            resultData.DefectCode[0] = "0";
            resultData.DefectCode[1] = "0";
            resultData.DefectCode[2] = "1";
            resultData.DefectCode[3] = "0";
            resultData.States[0] = 1;//Globalo.tcpManager.nRecv_Ack;
            resultData.States[1] = 0;
            resultData.States[2] = 1;
            resultData.States[3] = 1;
            //LotstartData.CommandParameter = Globalo.dataManage.TaskWork.SpecialDataParameter.Select(item => item.DeepCopy()).ToList();

            objectData.Data = resultData;
            Globalo.tcpManager.SendMessage_To_Handler(objectData);
        }

        private void button33_Click(object sender, EventArgs e)
        {
            TcpSocket.MessageWrapper objectData = new TcpSocket.MessageWrapper();
            objectData.Type = "TesterData";

            //TcpSocket.EquipmentData LotstartData = new TcpSocket.EquipmentData();
            TcpSocket.TesterData resultData = new TcpSocket.TesterData();
            resultData.init();
            resultData.BcrId[0] = "1111";// aoiApdData.Barcode;
            resultData.Cmd = "CMD_RESULT";
            resultData.socketNum[0] = 1;// int.Parse(aoiApdData.Socket_Num);
            resultData.States[0] = Globalo.tcpManager.nRecv_Ack;
            //LotstartData.CommandParameter = Globalo.dataManage.TaskWork.SpecialDataParameter.Select(item => item.DeepCopy()).ToList();

            objectData.Data = resultData;
            Globalo.tcpManager.SendMessage_To_Handler(objectData);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            //=C12/D12*E12
            //Globalo.yamlManager.configData.CamSettings.TopResolution.X
            //Globalo.yamlManager.configData.CamSettings.TopResolution.Y

            //Globalo.yamlManager.configData.CamSettings.SideResolution.X
            //Globalo.yamlManager.configData.CamSettings.SideResolution.Y

            //string cal = 40 / 현재길이 * 현재 설정값

            double target = 40.0;    //Target 값



            double curx = 39.618852;    //현재값 길이
            double cury = 39.618852;    //현재값 길이



            double resulx = 0.021462;// Globalo.yamlManager.configData.CamSettings.TopResolution.X;    //resoultion
            double resuly = 0.021462;// Globalo.yamlManager.configData.CamSettings.TopResolution.X;    //resoultion

            double calDatax = target / curx * resulx;
            double calDatay = target / cury * resuly;

            string cal = "";

            textBox_calx.Text = calDatax.ToString();
            textBox_caly.Text = calDatay.ToString();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            Globalo.visionManager.milLibrary.FindPattern(VisionClass.AoiTester.TOP_INDEX);
            return;

            MIL_ID MilImage = MIL.M_NULL;               // Image buffer identifier.
            MIL_ID GraphicList = MIL.M_NULL;
            //MIL_ID ContextId = MIL.M_NULL;              // ContextId identifier.
            MIL_ID MilDisplay = MIL.M_NULL;         // Display identifier.
            MIL_ID Result = MIL.M_NULL;                 // Result identifier.
            MIL_INT NumResults = 0;                     // Number of results found.

            double XOrg = 0.0;                          // Original model position.
            double YOrg = 0.0;
            double x = 0.0;                             // Model position.
            double y = 0.0;
            double ErrX = 0.0;                          // Model error position.
            double ErrY = 0.0;
            double Score = 0.0;                         // Model correlation score.
            double Time = 0.0;                          // Model search time.

            int FIND_MODEL_X_POS = 2454;
            int FIND_MODEL_Y_POS = 1759;
            int FIND_MODEL_WIDTH = 848;
            int FIND_MODEL_HEIGHT = 803;

            double FIND_MODEL_X_CENTER = (FIND_MODEL_X_POS + (FIND_MODEL_WIDTH - 1) / 2.0);
            double FIND_MODEL_Y_CENTER = (FIND_MODEL_Y_POS + (FIND_MODEL_HEIGHT - 1) / 2.0);

            double FIND_SHIFT_X = 24.5;
            double FIND_SHIFT_Y = 27.5;

            Globalo.visionManager.milLibrary.ClearOverlay_Manual(VisionClass.AoiTester.TOP_INDEX);
            Globalo.visionManager.milLibrary.GetSnapImage(VisionClass.AoiTester.TOP_INDEX);
            Globalo.visionManager.milLibrary.SetGrabOn(VisionClass.AoiTester.TOP_INDEX, false);


            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[VisionClass.AoiTester.TOP_INDEX];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[VisionClass.AoiTester.TOP_INDEX];
            int dataSize = sizeX * sizeY;

            MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay);

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, 0, 0, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilImage);

            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilProcImageChild[VisionClass.AoiTester.TOP_INDEX], 0, 0, sizeX, sizeY, ref MilImage);


            ///Globalo.visionManager.milLibrary.Load_pat(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.Ppid);

            //MIL.MpatRestore("d:\\patpat.pat", Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref ContextId); 
            //MIL_ID MpatRestore(MIL_INT FileName, MIL_ID SysId, long ControlFlag, ref MIL_ID ContextPatIdPtr);

            // Display the image buffer.
            MIL.MdispSelect(MilDisplay, MilImage);

            // Allocate a graphic list to hold the subpixel annotations to draw.
            MIL.MgraAllocList(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref GraphicList);

            // Associate the graphic list to the display for annotations.
            MIL.MdispControl(MilDisplay, MIL.M_ASSOCIATED_GRAPHIC_LIST_ID, GraphicList);

            

            // Define a regular model.
            //MIL.MpatDefine(ContextId, MIL.M_REGULAR_MODEL, MilImage, FIND_MODEL_X_POS, FIND_MODEL_Y_POS, FIND_MODEL_WIDTH, FIND_MODEL_HEIGHT, MIL.M_DEFAULT);

            // Set the search accuracy to high.
           MIL.MpatControl(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_ACCURACY, MIL.M_HIGH);

            // Set the search model speed to high.
            MIL.MpatControl(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_SPEED, MIL.M_MEDIUM);

            // Activate the search model angle mode.
            MIL.MpatControl(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_SEARCH_ANGLE_MODE, MIL.M_ENABLE);

            // Set the search model range angle.
            MIL.MpatControl(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_SEARCH_ANGLE_DELTA_NEG, 20);
            MIL.MpatControl(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_SEARCH_ANGLE_DELTA_POS, 20);

            // Set the search model angle accuracy.
            MIL.MpatControl(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_SEARCH_ANGLE_ACCURACY, 0.5);

            // Set the search model angle interpolation mode to bilinear.
            //MIL.MpatControl(ContextId, MIL.M_DEFAULT, MIL.M_SEARCH_ANGLE_INTERPOLATION_MODE, MIL.M_BILINEAR);

            // Preprocess the model.
            MIL.MpatPreprocess(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MilImage);

            // Draw a box around the model in the model image.
            MIL.MgraControl(MIL.M_DEFAULT, MIL.M_COLOR, MIL.M_COLOR_GREEN);
            MIL.MpatDraw(MIL.M_DEFAULT, Globalo.visionManager.milLibrary.m_MilPatModel[0], GraphicList, MIL.M_DRAW_BOX + MIL.M_DRAW_POSITION, MIL.M_DEFAULT, MIL.M_ORIGINAL);

            // Clear annotations.
            MIL.MgraClear(MIL.M_DEFAULT, GraphicList);

            // Allocate result buffer.
            MIL.MpatAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref Result);

            // Dummy first call for bench measure purpose only (bench stabilization, cache effect, etc...). This first call is NOT required by the application.
            MIL.MpatFind(Globalo.visionManager.milLibrary.m_MilPatModel[0], MilImage, Result);
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_RESET + MIL.M_SYNCHRONOUS, MIL.M_NULL);

            // Find the model in the target buffer.
            MIL.MpatFind(Globalo.visionManager.milLibrary.m_MilPatModel[0], MilImage, Result);

            // Read the time spent in MpatFindModel.
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_READ + MIL.M_SYNCHRONOUS, ref Time);

            // If one model was found above the acceptance threshold.
            MIL.MpatGetResult(Result, MIL.M_GENERAL, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref NumResults);

            if (NumResults == 1)
            {
                // Read results and draw a box around the model occurrence.
                MIL.MpatGetResult(Result, MIL.M_DEFAULT, MIL.M_POSITION_X, ref x);
                MIL.MpatGetResult(Result, MIL.M_DEFAULT, MIL.M_POSITION_Y, ref y);
                MIL.MpatGetResult(Result, MIL.M_DEFAULT, MIL.M_SCORE, ref Score);
                MIL.MgraControl(MIL.M_DEFAULT, MIL.M_COLOR, MIL.M_COLOR_BLUE);
                MIL.MpatDraw(MIL.M_DEFAULT, Result, GraphicList, MIL.M_DRAW_BOX + MIL.M_DRAW_POSITION, MIL.M_DEFAULT, MIL.M_DEFAULT);

                // Calculate the position errors in X and Y and inquire original model position.
                ErrX = Math.Abs((FIND_MODEL_X_CENTER + FIND_SHIFT_X) - x);
                ErrY = Math.Abs((FIND_MODEL_Y_CENTER + FIND_SHIFT_Y) - y);
                MIL.MpatInquire(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_ORIGINAL_X, ref XOrg);
                MIL.MpatInquire(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_ORIGINAL_Y, ref YOrg);

                // Print out the search result of the model in the original image.
                Console.Write("Search results:\n");
                Console.Write("---------------------------------------------------\n");
                Console.Write("The model is found to be shifted by \tX:{0:0.00}, Y:{1:0.00}.\n", x - XOrg, y - YOrg);
                Console.Write("The model position error is \t\tX:{0:0.00}, Y:{1:0.00}\n", ErrX, ErrY);
                Console.Write("The model match score is \t\t{0:0.0}\n", Score);
                //Console.Write("The search time is \t\t\t{0:0.000} ms\n\n", Time * 1000.0);

            }

        }

        private void button36_Click(object sender, EventArgs e) //패턴 등록하기
        {

            Globalo.visionManager.milLibrary.AddPattern(VisionClass.AoiTester.TOP_INDEX);

            return;
            MIL_INT NumResults = 0;                     // Number of results found.
            double Time = 0.0;                          // Model search time.
            MIL_ID Result = MIL.M_NULL;                 // Result identifier.
            int FIND_MODEL_X_POS =  2454;
            int FIND_MODEL_Y_POS = 1759;
            int FIND_MODEL_WIDTH = 848;
            int FIND_MODEL_HEIGHT = 803;


            Rectangle DrawRoiBox = Globalo.setTestControl.GetRoiRect();
            FIND_MODEL_X_POS = DrawRoiBox.X;
            FIND_MODEL_Y_POS = DrawRoiBox.Y;

            FIND_MODEL_WIDTH = DrawRoiBox.Width;
            FIND_MODEL_HEIGHT = DrawRoiBox.Height;

            FIND_MODEL_X_POS = (int)(DrawRoiBox.X * Globalo.visionManager.milLibrary.xExpand[VisionClass.AoiTester.TOP_INDEX]);
            FIND_MODEL_Y_POS = (int)(DrawRoiBox.Y * Globalo.visionManager.milLibrary.yExpand[VisionClass.AoiTester.TOP_INDEX]);

            FIND_MODEL_WIDTH = (int)(DrawRoiBox.Width * Globalo.visionManager.milLibrary.xExpand[VisionClass.AoiTester.TOP_INDEX]);
            FIND_MODEL_HEIGHT = (int)(DrawRoiBox.Height * Globalo.visionManager.milLibrary.yExpand[VisionClass.AoiTester.TOP_INDEX]);


            double FIND_MODEL_X_CENTER = (FIND_MODEL_X_POS + (FIND_MODEL_WIDTH - 1) / 2.0);
            double FIND_MODEL_Y_CENTER = (FIND_MODEL_Y_POS + (FIND_MODEL_HEIGHT - 1) / 2.0);
            // Target image shifting values.
            double FIND_SHIFT_X = 24.5;
            double FIND_SHIFT_Y = 27.5;
            double XOrg = 0.0;                          // Original model position.
            double YOrg = 0.0;
            double x = 0.0;                             // Model position.
            double y = 0.0;
            double ErrX = 0.0;                          // Model error position.
            double ErrY = 0.0;
            double Score = 0.0;                         // Model correlation score.
            double AnnotationColor = MIL.M_COLOR_GREEN; // Drawing color.
            

            //패턴 찾기
            MIL_ID MilImage = MIL.M_NULL;               // Image buffer identifier.
            MIL_ID GraphicList = MIL.M_NULL;
            MIL_ID ContextId = MIL.M_NULL;              // ContextId identifier.
            MIL_ID MilDisplay = MIL.M_NULL;         // Display identifier.
            bool rtn = true;
            Globalo.visionManager.milLibrary.ClearOverlay_Manual(VisionClass.AoiTester.TOP_INDEX);
            Globalo.visionManager.milLibrary.GetSnapImage(VisionClass.AoiTester.TOP_INDEX);
            Globalo.visionManager.milLibrary.SetGrabOn(VisionClass.AoiTester.TOP_INDEX, false);


            int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[VisionClass.AoiTester.TOP_INDEX];
            int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[VisionClass.AoiTester.TOP_INDEX];
            int dataSize = sizeX * sizeY;

            MIL.MdispAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_WINDOWED, ref MilDisplay);

            MIL.MbufAlloc2d(Globalo.visionManager.milLibrary.MilSystem, 0, 0, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilImage);

            MIL.MbufChild2d(Globalo.visionManager.milLibrary.MilProcImageChild[VisionClass.AoiTester.TOP_INDEX], 0, 0, sizeX, sizeY, ref MilImage);
            
            

            // Display the image buffer.
            MIL.MdispSelect(MilDisplay, MilImage);

            // Allocate a graphic list to hold the subpixel annotations to draw.
            MIL.MgraAllocList(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref GraphicList);

            // Associate the graphic list to the display for annotations.
            MIL.MdispControl(MilDisplay, MIL.M_ASSOCIATED_GRAPHIC_LIST_ID, GraphicList);

            // Allocate a normalized pattern matching context.
            MIL.MpatAlloc(Globalo.visionManager.milLibrary.MilSystem, MIL.M_NORMALIZED, MIL.M_DEFAULT, ref Globalo.visionManager.milLibrary.m_MilPatModel[0]);

            // Define a regular model.
            MIL.MpatDefine(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_REGULAR_MODEL, MilImage, FIND_MODEL_X_POS,FIND_MODEL_Y_POS, FIND_MODEL_WIDTH, FIND_MODEL_HEIGHT, MIL.M_DEFAULT);

            // Set the search accuracy to high.
            MIL.MpatControl(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_ACCURACY, MIL.M_HIGH);

            // Set the search model speed to high.
            MIL.MpatControl(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_SPEED, MIL.M_MEDIUM);

            // Activate the search model angle mode.
            MIL.MpatControl(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_SEARCH_ANGLE_MODE, MIL.M_ENABLE);

            // Set the search model range angle.
            //MIL.MpatControl(ContextId, MIL.M_DEFAULT, MIL.M_SEARCH_ANGLE_DELTA_NEG, 10);
            //MIL.MpatControl(ContextId, MIL.M_DEFAULT, MIL.M_SEARCH_ANGLE_DELTA_POS, 10);

            // Set the search model angle accuracy.
            //MIL.MpatControl(ContextId, MIL.M_DEFAULT, MIL.M_SEARCH_ANGLE_ACCURACY, 0.25);

            // Set the search model angle interpolation mode to bilinear.
            //MIL.MpatControl(ContextId, MIL.M_DEFAULT, MIL.M_SEARCH_ANGLE_INTERPOLATION_MODE, MIL.M_BILINEAR);

            // Preprocess the model.
            MIL.MpatPreprocess(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MilImage);

            // Draw a box around the model in the model image.
            MIL.MgraControl(MIL.M_DEFAULT, MIL.M_COLOR, MIL.M_COLOR_GREEN);
            MIL.MpatDraw(MIL.M_DEFAULT, Globalo.visionManager.milLibrary.m_MilPatModel[0], GraphicList, MIL.M_DRAW_BOX + MIL.M_DRAW_POSITION, MIL.M_DEFAULT, MIL.M_ORIGINAL);

            //MIL.MpatSave("d:\\patpat.pat", ContextId, MIL.M_DEFAULT);
            Globalo.visionManager.milLibrary.Save_pat(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.Ppid);

            MIL.MbufExport("d:\\patpat.BMP", MIL.M_BMP, MilImage);
            // Clear annotations.
            MIL.MgraClear(MIL.M_DEFAULT, GraphicList);

            // Translate the image on a subpixel level.
            MIL.MimTranslate(MilImage, MilImage, FIND_SHIFT_X, FIND_SHIFT_Y, MIL.M_DEFAULT);

            // Allocate result buffer.
            MIL.MpatAllocResult(Globalo.visionManager.milLibrary.MilSystem, MIL.M_DEFAULT, ref Result);

            // Dummy first call for bench measure purpose only (bench stabilization, cache effect, etc...). This first call is NOT required by the application.
            MIL.MpatFind(Globalo.visionManager.milLibrary.m_MilPatModel[0], MilImage, Result);
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_RESET + MIL.M_SYNCHRONOUS, MIL.M_NULL);

            // Find the model in the target buffer.
            MIL.MpatFind(Globalo.visionManager.milLibrary.m_MilPatModel[0], MilImage, Result);

            // Read the time spent in MpatFindModel.
            MIL.MappTimer(MIL.M_DEFAULT, MIL.M_TIMER_READ + MIL.M_SYNCHRONOUS, ref Time);

            // If one model was found above the acceptance threshold.
            MIL.MpatGetResult(Result, MIL.M_GENERAL, MIL.M_NUMBER + MIL.M_TYPE_MIL_INT, ref NumResults);

            if (NumResults == 1)
            {
                // Read results and draw a box around the model occurrence.
                MIL.MpatGetResult(Result, MIL.M_DEFAULT, MIL.M_POSITION_X, ref x);
                MIL.MpatGetResult(Result, MIL.M_DEFAULT, MIL.M_POSITION_Y, ref y);
                MIL.MpatGetResult(Result, MIL.M_DEFAULT, MIL.M_SCORE, ref Score);
                MIL.MgraControl(MIL.M_DEFAULT, MIL.M_COLOR, MIL.M_COLOR_BLUE);
                MIL.MpatDraw(MIL.M_DEFAULT, Result, GraphicList, MIL.M_DRAW_BOX + MIL.M_DRAW_POSITION, MIL.M_DEFAULT, MIL.M_DEFAULT);

                // Calculate the position errors in X and Y and inquire original model position.
                ErrX = Math.Abs((FIND_MODEL_X_CENTER + FIND_SHIFT_X) - x);
                ErrY = Math.Abs((FIND_MODEL_Y_CENTER + FIND_SHIFT_Y) - y);
                MIL.MpatInquire(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_ORIGINAL_X, ref XOrg);
                MIL.MpatInquire(Globalo.visionManager.milLibrary.m_MilPatModel[0], MIL.M_DEFAULT, MIL.M_ORIGINAL_Y, ref YOrg);

                // Print out the search result of the model in the original image.
                Console.Write("Search results:\n");
                Console.Write("---------------------------------------------------\n");
                Console.Write("The model is found to be shifted by \tX:{0:0.00}, Y:{1:0.00}.\n", x - XOrg, y - YOrg);
                Console.Write("The model position error is \t\tX:{0:0.00}, Y:{1:0.00}\n", ErrX, ErrY);
                Console.Write("The model match score is \t\t{0:0.0}\n", Score);
                Console.Write("The search time is \t\t\t{0:0.000} ms\n\n", Time * 1000.0);

            }
        }
    }
}
