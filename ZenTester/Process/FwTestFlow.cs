using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenTester.Process
{
    public class FwTestFlow
    {
        public CancellationTokenSource CancelToken;
        public ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);  // true면 동작 가능

        public Task<int> fwTask;
        private int waitFw = 1;

        private readonly SynchronizationContext _syncContext;

        public int nTimeTick = 0;
        public string serverfwFileName = string.Empty;
        public TcpSocket.FwapdData fwtestData = new TcpSocket.FwapdData();
        private TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
        private TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();

        private int m_nTestFinalResult;
        private object openFileDlg;
        private int nRetryCount = 0;
        private int nRetryMax = 3;

        public FwTestFlow()
        {
            fwTask = Task.FromResult(1);
        }

        public int FwAutoProcess(int nStep)
        {
            int nRetStep = nStep;
            bool bRtn = false;
            switch (nRetStep)
            {
                case 100:
                    nRetryCount = 0;
                    m_nTestFinalResult = 1;
                    waitFw = -1;
                    fwTask = null;
                    CancelToken?.Dispose();
                    CancelToken = new CancellationTokenSource();    //
                    nRetStep = 110;

                    break;
                case 110:
                    //Handler로 부터 펌웨어 파일명 받아서 , Conf.ini파일에서 파일명 비교
                    //다를 경우 펌웨어 파일 요청 FTP server


                    string fwFileName = Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName();
                    if(fwFileName == serverfwFileName)
                    {
                        nRetStep = 120; //JUMP
                    }
                    else
                    {
                        //1.다를경우 펌웨어 버전 업데이트 필요
                        //2. 펌웨어 파일 다운로드 FTP Server

                        //3. SFTP를 통해  FXA보드에 펌웨어 파일 전달

                        nRetStep = 111;
                    }
                    //
                    
                    break;
                case 111:
                    nRetStep = 112;
                    break;
                case 112:
                    nRetStep = 113;
                    break;
                case 113:
                    // SFTP를 통해 FXA보드에 펌웨어 파일 복사

                    bRtn = Globalo.FxaBoardManager.fxaFirmwardDw.sFtpUploadFile(serverfwFileName);
                    if(bRtn == false)
                    {
                        if(nRetryCount < nRetryMax)
                        {
                            nRetryCount++;
                            nRetStep = 113;
                            break;
                        }
                        else
                        {
                            //Fail
                            m_nTestFinalResult = -3;
                            Console.WriteLine($"Firmware down Fail");
                            nRetStep = 220;
                            break;
                        }
                    }
                    nRetStep = 120;
                    break;

                case 120:
                    //Globalo.taskWork.CommandParameter <-------Special Data
                    nRetStep = 130;
                    break;
                case 130:
                    fwTask = Task.Run(() =>
                    {
                        waitFw = 1;
                        waitFw = FwFlow();      //0 or -1 Return
                        Console.WriteLine($"-------------- Fw Task - end {waitFw}");
                        return waitFw;
                    }, CancelToken.Token);

                    nTimeTick = Environment.TickCount;
                    nRetStep = 131;
                    break;
                case 131:
                    if (waitFw == 1)
                    {
                        if (Environment.TickCount - nTimeTick > 50000)
                        {
                            Console.WriteLine("Timeout - {waitFw}");
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
                    sendEqipData.LotID = fwtestData.Barcode;
                    sendEqipData.Judge = m_nTestFinalResult;
                    

                    string[] apdList = { "Result_Code", "Socket_Num", "Version", "Result", "Barcode", "Heater_Current" };
                    string[] apdResult = { fwtestData.Result_Code, fwtestData.Socket_Num, fwtestData.Version,  m_nTestFinalResult.ToString(), fwtestData.Barcode, fwtestData.Heater_Current };

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
                    //완공다되면 Handler로도 보내줘야된다.

                    TcpSocket.MessageWrapper objectData = new TcpSocket.MessageWrapper();
                    objectData.Type = "EquipmentData";

                    TcpSocket.EquipmentData LotstartData = new TcpSocket.EquipmentData();
                    LotstartData.Command = "APS_LOT_FINISH";
                    LotstartData.LotID = fwtestData.Barcode;
                    LotstartData.Judge = Globalo.tcpManager.nRecv_Ack;

                    objectData.Data = LotstartData;
                    //TODO: 여기서 Special Data 여기서 보내야된다.
                    //
                    Globalo.tcpManager.SendMessage_To_Handler(objectData);        //T ->Handelr로 보내는 부분
                    break;
            }
            return nRetStep;
        }

        private int FwFlow()
        {
            int nRtn = -1;
            bool bRtn = false;
            string szLog = "";
            int nRetStep = 10;
            while (true)
            {
                if (CancelToken.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Fw Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                switch (nRetStep)
                {
                    case 10:
                        nRetStep = 20;
                        break;
                    case 20:
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
                    Console.WriteLine("Fw Flow - fail");
                    break;
                }

                if (nRetStep == 1000)
                {
                    Console.WriteLine("Fw Flow - end");
                    break;
                }
                Thread.Sleep(10);       //TODO: while문안에서는 최소 10ms 꼭 필요
            }
            if (nRetStep == 1000)
            {
                nRtn = 0;
                Console.WriteLine("Fw Flow - ok");
            }
            else
            {
                nRtn = -1;
                Console.WriteLine("Fw Flow - ng");
            }
            return nRtn;
        }
    }
}
