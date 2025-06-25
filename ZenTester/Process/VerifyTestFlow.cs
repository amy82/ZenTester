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
        private TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
        private TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();

        private int m_nTestFinalResult;
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
                    m_nTestFinalResult = 1;
                    waitverify = -1;
                    verifyTask = null;
                    CancelToken?.Dispose();
                    CancelToken = new CancellationTokenSource();
                    nRetStep = 110;

                    break;
                case 110:
                    //착공걸기 - 착공은 차례대로만  Verify 도
                    //Tester에서 Secsgem으로 착공 거는 공정은 Verify 공정만..
                    
                    EqipData.Type = "EquipmentData";
                    sendEqipData.Command = "VERIFY_OBJECT_REPORT";//"OBJECT_ID_REPORT";
                    sendEqipData.LotID = verifytestData.Barcode;
                    sendEqipData.DataID = verifytestData.Socket_Num;
                    EqipData.Data = sendEqipData;

                    Globalo.tcpManager.nRecv_Ack = -1;
                    Globalo.taskWork.bRecv_Client_LotStart = -1;
                    Globalo.tcpManager.SendMessage_To_SecsGem(EqipData);        //object
                    nTimeTick = Environment.TickCount;
                    nRetStep = 111;
                    break;
                case 111:
                    //착공 대기 or verify 진행
                    //if (Globalo.tcpManager.nRecv_Ack == 0)
                    if (Globalo.taskWork.bRecv_Client_LotStart == 0)
                    {
                        nRetStep = 120;
                    }
                    else if (Globalo.taskWork.bRecv_Client_LotStart > 0)
                    {
                        Console.WriteLine($"LOT START FAIL - {Globalo.taskWork.bRecv_Client_LotStart}");
                        nRetStep = -1;
                    }
                    else if (Environment.TickCount - nTimeTick > 15000)//6000)
                    {
                        Console.WriteLine($"Timeout {nRetStep}");
                        nRetStep = -1;
                    }
                    
                    break;

                case 120:
                    //Globalo.taskWork.CommandParameter <-------Special Data
                    nRetStep = 130;
                    break;
                case 130:
                    verifyTask = Task.Run(() =>
                    {
                        waitverify = 1;
                        waitverify = VerifyFlow();      //0 or -1 Return
                        Console.WriteLine($"-------------- Verify Task - end {waitverify}");
                        return waitverify;
                    }, CancelToken.Token);

                    nTimeTick = Environment.TickCount;
                    nRetStep = 131;
                    break;
                case 131:
                    if (waitverify == 1)
                    {
                        if (Environment.TickCount - nTimeTick > 50000)
                        {
                            Console.WriteLine("Timeout - {waitverify}");
                            nRetStep = -1;
                            break;
                        }
                        break;
                    }
                    nRetStep = 200;
                    break;
                case 200:
                    EqipData.Type = "EquipmentData";
                    sendEqipData.Command = "LOT_APD_REPORT";
                    sendEqipData.LotID = verifytestData.Barcode;
                    sendEqipData.Judge = m_nTestFinalResult;
                    sendEqipData.DataID = verifytestData.Socket_Num;

                    //1.Socket_Num
                    //2.Result
                    //3.Barcode
                    //4.SensorID
                    int tCount = 4;
                    string[] apdList = { "Socket_Num", "Result", "Barcode", "SensorID" };
                    string[] apdResult = { verifytestData.Socket_Num, m_nTestFinalResult.ToString(), verifytestData.Barcode, verifytestData.SensorID };

                    for (int i = 0; i < tCount; i++)
                    {
                        TcpSocket.EquipmentParameterInfo pInfo = new TcpSocket.EquipmentParameterInfo();

                        pInfo.Name = apdList[i];
                        pInfo.Value = apdResult[i];

                        sendEqipData.CommandParameter.Add(pInfo);
                    }

                    EqipData.Data = sendEqipData;
                    Globalo.tcpManager.nRecv_Ack = -1;
                    Globalo.taskWork.bRecv_Client_ApdReport = -1;
                    Globalo.tcpManager.SendMessage_To_SecsGem(EqipData);
                    nTimeTick = Environment.TickCount;

                    nRetStep = 210;
                    break;

                case 210:
                    //착공 확인 대기
                    if (Globalo.taskWork.bRecv_Client_ApdReport == 0)
                    {
                        nRetStep = 220;
                    }
                    else if (Globalo.taskWork.bRecv_Client_ApdReport == 1)
                    {
                        m_nTestFinalResult = -1;
                        Console.WriteLine($"APD REPORT FAIL - {Globalo.taskWork.bRecv_Client_ApdReport}");
                        nRetStep = 220;
                    }
                    else if (Environment.TickCount - nTimeTick > 5000)
                    {
                        m_nTestFinalResult = -2;
                        Console.WriteLine($"Timeout {nRetStep}");
                        nRetStep = 220;
                    }
                    break;
                case 220:
                    //Verify 공정은 Secsgem으로 apd보고해야된다 . 나머지는 Handler로
                    //완공다되면 Handler로도 보내줘야된다.


                    TcpSocket.MessageWrapper objectData = new TcpSocket.MessageWrapper();
                    objectData.Type = "EquipmentData";

                    TcpSocket.EquipmentData LotstartData = new TcpSocket.EquipmentData();
                    LotstartData.Command = "APS_LOT_FINISH";
                    LotstartData.LotID = verifytestData.Barcode;
                    LotstartData.Judge = Globalo.tcpManager.nRecv_Ack;

                    objectData.Data = LotstartData;
                    //TODO: 여기서 Special Data 여기서 보내야된다.
                    //
                    Globalo.tcpManager.SendMessage_To_Handler(objectData);        //T ->Handelr로 보내는 부분


                    nRetStep = 1000;
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
                        Globalo.FxaBoardManager.fxaEEpromVerify.mmdEEpromData.Clear();      //여기에 crc 계산값을 담자

                        Fxa.CrcClass.ChangeToHex(Globalo.taskWork.CommandParameter);

                        ushort crc16_ccitt_zero = Fxa.CrcClass.ComputeCRC16(Globalo.FxaBoardManager.fxaEEpromVerify.mmdEEpromData.ToArray(), 0x1021, 0x0000, 0x0000);   //0xFFFF

                        byte[] outbytes;
                        Fxa.CrcClass.CalculateCRC16(Globalo.FxaBoardManager.fxaEEpromVerify.mmdEEpromData.ToArray(), out outbytes);
                        //

                        nRetStep = 20;
                        break;
                    case 20:
                        //Special Data 로 Crc 계산
                        //Special Data + Crc  결과로 txt 파일 생성
                        //txt파일 명 과 같이 Verify~~.exe 호출
                        //verify 진행

                        //끝
                        //Globalo.FxaBoardManager.fxaEEpromVerify.mmdEEpromData

                        nRetStep = 30;
                        break;
                    case 30:
                        nRetStep = 900;
                        break;
                    case 900:
                        m_nTestFinalResult = 1;
                        nRetStep = 1000;
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
