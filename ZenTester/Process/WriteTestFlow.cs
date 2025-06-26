using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenTester.Process
{
    public class WriteTestFlow
    {
        public CancellationTokenSource CancelToken;
        public ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);  // true면 동작 가능

        public Task<int> writeTask;
        private int waitverify = 1;

        private readonly SynchronizationContext _syncContext;

        public int nTimeTick = 0;

        public TcpSocket.WriteApdData writetestData = new TcpSocket.WriteApdData();

        private TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
        private TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();

        public List<TcpSocket.EquipmentParameterInfo> CommandParameter { get; set; } = new List<TcpSocket.EquipmentParameterInfo>();
        private int m_nTestFinalResult;
        public WriteTestFlow()
        {
            writeTask = Task.FromResult(1);

        }
        public int WriteAutoProcess(int nStep)
        {
            int nRetStep = nStep;

            switch (nRetStep)
            {
                case 100:
                    m_nTestFinalResult = 1;
                    waitverify = -1;
                    writeTask = null;
                    CancelToken?.Dispose();
                    CancelToken = new CancellationTokenSource();    //
                    nRetStep = 110;

                    break;
                case 110:

                    nRetStep = 120;
                    break;
                case 120:
                    //Globalo.taskWork.CommandParameter <-------Special Data
                    nRetStep = 130;
                    break;
                case 130:
                    writeTask = Task.Run(() =>
                    {
                        waitverify = 1;
                        waitverify = WriteFlow();      //0 or -1 Return
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
                    sendEqipData.LotID = writetestData.Barcode;
                    sendEqipData.Judge = m_nTestFinalResult;

                    //1.Socket_Num
                    //2.Result
                    //3.Barcode
                    //4.SensorID

                    string[] apdList = { "Checksum0", "Checksum1", "Checksum2", "Checksum3", "Checksum4", "Socket_Num", "Result", "Barcode", "SensorID", "Time"};
                    string[] apdResult = { writetestData.Checksum0, writetestData.Checksum1, writetestData.Checksum2, writetestData.Checksum3, writetestData.Checksum4,
                            writetestData.Socket_Num, m_nTestFinalResult.ToString(), writetestData.Barcode, writetestData.SensorID, writetestData.Time };

                    for (int i = 0; i < apdList.Length; i++)
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
                    LotstartData.LotID = writetestData.Barcode;
                    LotstartData.Judge = Globalo.tcpManager.nRecv_Ack;

                    objectData.Data = LotstartData;
                    //TODO: 여기서 Special Data 여기서 보내야된다.
                    //
                    Globalo.tcpManager.SendMessage_To_Handler(objectData);        //T ->Handelr로 보내는 부분
                    break;
            }
            return nRetStep;
        }
        private int WriteFlow()
        {
            int nRtn = -1;
            bool bRtn = false;
            string szLog = "";
            int nRetStep = 10;
            while (true)
            {
                if (CancelToken.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Write Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                switch (nRetStep)
                {
                    case 10:
                        nRetStep = 20;
                        break;
                    case 20:
                        //스페셜 data로 txt만들기 CommandParameter

                        string txtFilePath = "";

                        Fxa.CrcClass.crcToTxtSave(CommandParameter, txtFilePath);

                        //Globalo.FxaBoardManager.fxaEEpromWrite.sa

                        //1.Special Data로 Crc 계산
                        //2.Special data + crc = txt 파일 생성
                        //3.txt파일명과 TeslaTrinity~~~Tool.exe 호출
                        //4.끝나면 Dat 파일이 생성된다.
                        //5.fxa보드로 cmd 명령어 호출 (dat 파일명 포함, 풀경로는 ini에 미리 설정)
                        //6.write 끝나면 checksum 값 보고
                        //7.end


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
                    Console.WriteLine("Write Flow - fail");
                    break;
                }

                if (nRetStep == 1000)
                {
                    Console.WriteLine("Write Flow - end");
                    break;
                }
                Thread.Sleep(10);       //TODO: while문안에서는 최소 10ms 꼭 필요
            }
            if (nRetStep == 1000)
            {
                nRtn = 0;
                Console.WriteLine("Write Flow - ok");
            }
            else
            {
                nRtn = -1;
                Console.WriteLine("Write Flow - ng");
            }
            return nRtn;
        }
    }
}
