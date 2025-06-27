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
            string szLog = "";
            switch (nRetStep)
            {
                case 100:
                    nRetryCount = 0;
                    m_nTestFinalResult = 1;
                    waitFw = -1;
                    fwTask = null;
                    CancelToken?.Dispose();
                    CancelToken = new CancellationTokenSource();
                    nRetStep = 110;

                    break;
                case 110:
                    //Handler로 부터 펌웨어 파일명 받아서 , Conf.ini파일에서 파일명 비교
                    //다를 경우 펌웨어 파일 요청 FTP server


                    string fwFileName = Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName("FIRMWARE_FILE");
                    

                    bRtn = Globalo.FxaBoardManager.fxaFirmwardDw.chkfwExeFileCheck(fwFileName);
                    
                    //250623 파일 유무만 확인 Ftp 없음 xx
                    //if (fwFileName == serverfwFileName)


                    if (bRtn)    //exe 파일 유무 확인
                    {
                        szLog = $"[FW] {fwFileName} FIRMWARE FILE CHECK : OK  [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 120; //JUMP
                        //같은 경우 업데이트 필요 없음
                        break;
                    }
                    else
                    {
                        szLog = $"[FW] {fwFileName} FIRMWARE FILE CHECK : FAIL  [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        //1.다를경우 펌웨어 버전 업데이트 필요
                        //2. 펌웨어 파일 다운로드 FTP Server
                        //3. SFTP를 통해  FXA보드에 펌웨어 파일 전달
                        nRetStep = -1;
                        break;
                    }
                    //
                    
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

                    string[] apdList = { "Result_Code", "Socket_Num", "Version", "Result", "Barcode", "Heater_Current" };
                    //string[] apdResult = { fwtestData.Result_Code[0], fwtestData.Socket_Num, fwtestData.Version[0], m_nTestFinalResult.ToString(), fwtestData.Barcode, fwtestData.Heater_Current[0] };

                    for (int i = 0; i < 4; i++)
                    {
                        TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
                        TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();

                        EqipData.Type = "EquipmentData";
                        sendEqipData.Command = "LOT_APD_REPORT";
                        sendEqipData.LotID = fwtestData.arrBcr[i];
                        sendEqipData.ErrCode = "";      //DefectCode
                        sendEqipData.Judge = int.Parse(fwtestData.Result[i]);

                        for (int j = 0; j < apdList.Length; j++)
                        {
                            TcpSocket.EquipmentParameterInfo pInfo = new TcpSocket.EquipmentParameterInfo();
                            pInfo.Name = apdList[j];
                            if(pInfo.Name == apdList[0])
                            {
                                pInfo.Value = fwtestData.Result_Code[j];
                            }
                            else if (pInfo.Name == apdList[1])
                            {
                                pInfo.Value = fwtestData.Socket_Num;
                            }
                            else if (pInfo.Name == apdList[2])
                            {
                                pInfo.Value = fwtestData.Version[j];
                            }
                            else if (pInfo.Name == apdList[3])
                            {
                                pInfo.Value = fwtestData.Result[j];
                            }
                            else if (pInfo.Name == apdList[4])
                            {
                                pInfo.Value = fwtestData.arrBcr[j];
                            }
                            else if (pInfo.Name == apdList[5])
                            {
                                pInfo.Value = fwtestData.Heater_Current[j];
                            }

                            sendEqipData.CommandParameter.Add(pInfo);
                            EqipData.Data = sendEqipData;
                            
                        }

                        Globalo.tcpManager.nRecv_Ack = -1;
                        Globalo.taskWork.bRecv_Client_ApdReport = -1;
                        Globalo.tcpManager.SendMessage_To_SecsGem(EqipData);

                        Thread.Sleep(300);
                    }
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
            int i = 0;
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
                        //fwtestData.Version = Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName("FIRMWARE_VERSION");
                        fwtestData.LogPath = Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName("LOG_PATH"); //<--완료후 json 파일 생성 위치

                        // 쉼표로 분리
                        
                        string fwRtn = Globalo.FxaBoardManager.fxaFirmwardDw.FirmwareDownLoadForCamAsync(fwtestData.arrBcr);
                        string[] receivedParse = fwRtn.Split(',');     //총 13개




                        szLog = $"[FW] Firmware Result:{fwRtn} [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        string[] items = fwRtn.Split(',');
                        //"$T,01,CAM1_P1637042-00-C-SLGM250434C00283,05,02,,00,03,,00,04,CAM3_P1637042-00-C-SLGM250434C00283\r,03";

                        //3번째 , 6번째, 9번째, 12번째

                        fwtestData.Result_Code[0] = items[3];
                        fwtestData.Result_Code[1] = items[6];
                        fwtestData.Result_Code[2] = items[9];
                        fwtestData.Result_Code[3] = items[12];

                        nTimeTick = Environment.TickCount;
                        nRetStep = 25;
                        break;
                    case 25:
                        if (Environment.TickCount - nTimeTick > 300)
                        {
                            nRetStep = 30;
                        }
                        break;
                    case 30:
                        for (i = 0; i < 4; i++)
                        {
                            if (int.Parse(fwtestData.Result_Code[i]) == 0)
                            {
                                fwtestData.Result[i] = "1";
                                szLog = $"Cam{i + 1} Firmware Download ok -{fwtestData.Result_Code[i]}";

                                int nResult = int.Parse(fwtestData.Result_Code[i]);
                                Globalo.FxaBoardManager.fxaFirmwardDw.getfwResultFromJson(fwtestData.arrBcr[i], nResult);
                            }
                            else
                            {
                                //ng
                                fwtestData.Result[i] = "0";
                                //fail
                                szLog = $"Cam{i + 1} Firmware Download fail -{fwtestData.Result_Code[i]}";
                            }


                            //
                            fwtestData.Heater_Current[i] = Globalo.FxaBoardManager.fxaFirmwardDw.getHeater_Current(fwtestData.arrBcr[i], fwtestData.Result[i]);
                        }

                        nRetStep = 40;
                        break;
                    case 40:

                        nRetStep = 50;
                        break;
                    case 50:
                        string[] _sensorId = new string[4];
                        string[] _version = new string[4];

                        //ReadFirmware
                        Globalo.FxaBoardManager.fxaFirmwardDw.ReadFirmware(ref _version, ref _sensorId);
                        for (i = 0; i < 4; i++)
                        {
                            fwtestData.Version[i] = _version[i];
                            fwtestData.Sensorid[i] = _sensorId[i];

                            szLog = $"[FW] Version: {fwtestData.Version[i]} SensorId:{fwtestData.Sensorid[i]} [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        nRetStep = 60;
                        break;
                    case 60:
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
