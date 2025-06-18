using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            sendEqipData.LotID = "testLot";// Globalo.dataManage.TaskWork.m_szChipID;
            EqipData.Data = sendEqipData;
            Globalo.tcpManager.SendMessage_To_SecsGem(EqipData);        //test
        }

        private void button23_Click(object sender, EventArgs e)
        {
            //verify apd
            TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
            EqipData.Type = "EquipmentData";

            TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
            sendEqipData.Command = "LOT_APD_REPORT";
            sendEqipData.LotID = "testLot";//Globalo.dataManage.TaskWork.m_szChipID;
            sendEqipData.Judge = 1;/// Globalo.taskWork.m_nTestFinalResult;

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
    }
}
