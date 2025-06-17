using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenTester.Process
{
    public class VerifyTestFlow
    {
        public CancellationTokenSource CancelToken;
        public ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);  // true면 동작 가능

        public Task<int> verifyTask;
        private int waitverify = 1;

        private readonly SynchronizationContext _syncContext;

        public int nTimeTick = 0;

        public TcpSocket.VerifyApdData verifytestData = new TcpSocket.VerifyApdData();

        public VerifyTestFlow()
        {
            verifyTask = Task.FromResult(1);
        }

        public int VerifyAutoProcess(int nStep)
        {
            int nRetStep = nStep;

            switch (nRetStep)
            {
                case 100:
                    waitverify = -1;
                    verifyTask = null;
                    CancelToken?.Dispose();
                    CancelToken = new CancellationTokenSource();    //

                    verifytestData.init();
                    verifytestData.Socket_Num = socketNumber.ToString();
                    break;
                case 110:
                    //착공걸기
                    TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
                    EqipData.Type = "EquipmentData";

                    TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
                    sendEqipData.Command = "OBJECT_ID_REPORT";
                    sendEqipData.LotID = "testLot";// Globalo.dataManage.TaskWork.m_szChipID;
                    EqipData.Data = sendEqipData;
                    Globalo.tcpManager.SendMessageToServer(EqipData);
                    break;
                case 111:
                    //착공 대기 or verify 진행
                    break;

                case 120:

                    break;
                case 130:
                    verifyTask = Task.Run(() =>
                    {
                        waitverify = 1;
                        waitverify = VerifyFlow();      //0 or -1 Return
                        Console.WriteLine($"-------------- Verify Task - end {waitverify}");
                        return waitverify;
                    }, CancelToken.Token);
                    break;
                case 200:

                    break;
            }
            return nRetStep;
        }
        private int VerifyFlow()
        {
            int nRtn = -1;
            bool bRtn = false;
            string szLog = "";
            int nRetStep = 10;
            while (true)
            {
                if (CancelToken.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Verify Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                switch (nRetStep)
                {
                    case 10:

                        break;
                    default:
                        break;
                }

                if (nRetStep < 0)
                {
                    Console.WriteLine("Verify Flow - fail");
                    break;
                }

                if (nRetStep == 1000)
                {
                    Console.WriteLine("Verify Flow - end");
                    break;
                }
                Thread.Sleep(10);       //TODO: while문안에서는 최소 10ms 꼭 필요
            }
            if (nRetStep == 1000)
            {
                nRtn = 0;
                Console.WriteLine("Verify Flow - ok");
            }
            else
            {
                nRtn = -1;
                Console.WriteLine("Verify Flow - ng");
            }
            return nRtn;
        }
    }
}
