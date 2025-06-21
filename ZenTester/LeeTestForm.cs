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
            for (int i = 0; i < 3; i++)
            {
                TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
                EqipData.Type = "EquipmentData";

                TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
                sendEqipData.Command = "VERIFY_OBJECT_REPORT";
                sendEqipData.LotID = "testLot-" + i.ToString();// Globalo.dataManage.TaskWork.m_szChipID;
                sendEqipData.DataID = "1";
                EqipData.Data = sendEqipData;
                Globalo.tcpManager.SendMessage_To_SecsGem(EqipData);        //test

                Thread.Sleep(500);
            }
            

        }

        private void button23_Click(object sender, EventArgs e)
        {
            //verify apd
            TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
            EqipData.Type = "EquipmentData";

            TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
            sendEqipData.Command = "LOT_APD_REPORT";
            sendEqipData.LotID = "testLot"; //Globalo.dataManage.TaskWork.m_szChipID;
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
            data.LotId = new string[1];
            data.LotId[0] = "manual lot";
            data.socketNum = 1;
            Globalo.taskManager.Aoi_TestRun(data);
        }

        private void button25_Click(object sender, EventArgs e)
        {
            System.Drawing.Point ConePos = new System.Drawing.Point();
            Globalo.visionManager.milLibrary.ClearOverlay(1);

            Globalo.visionManager.milLibrary.SetGrabOn(1, false);
            Globalo.visionManager.milLibrary.GetSnapImage(1);
            //
            //
            Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(1, VisionClass.eMarkList.SIDE_HEIGHT, ref ConePos);

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
    }
}
