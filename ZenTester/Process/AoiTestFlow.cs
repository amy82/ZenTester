using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;

namespace ZenTester.Process
{
    public class AoiTestFlow
    {
        public CancellationTokenSource CancelToken;
        public ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);  // true면 동작 가능
        public Task<int> TopCamTask;
        public Task<int> SideCamTask;
        private int waitTopCam = 1;
        private int waitSideCam = 1;
        private readonly SynchronizationContext _syncContext;
        public int nTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯
        public int nTopTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯
        public int nSideTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯
        public int nLoadTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯
        public int nUnloadTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯

        public TcpSocket.AoiApdData aoiApdData = new TcpSocket.AoiApdData();
        //
        public TcpSocket.ResultAoiData ResultAoiAPdData = new TcpSocket.ResultAoiData();

        private OpenCvSharp.Point[] aoiCenterPos = new OpenCvSharp.Point[2];
        private TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
        private TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
        private int m_nTestFinalResult;
        private int sidecount = 0;

        private int captureDelay = 1000;
        public AoiTestFlow()
        {
            _syncContext = SynchronizationContext.Current;

            TopCamTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
            SideCamTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환

            aoiApdData.init();
            ResultAoiAPdData.init();
        }

        public int AoiAutoProcess(int nStep)
        {
            int nRetStep = nStep;
            string szLog = "";
            switch (nRetStep)
            {
                case 100:
                    
                    m_nTestFinalResult = 1;
                    Globalo.visionManager.milLibrary.RunModeChange(true);

                    waitTopCam = -1;
                    waitSideCam = -1;
                    TopCamTask = null;
                    SideCamTask = null;
                    CancelToken?.Dispose();
                    CancelToken = new CancellationTokenSource();
                    nRetStep = 110;
                    break;
                case 110:
                    //조명

                    int sData = Globalo.yamlManager.aoiRoiConfig.sideLightData[0].data;
                    Globalo.serialPortManager.LightControl.recvCheck = -1;
                    Globalo.serialPortManager.LightControl.AllctrlLedVolume(0, sData);      //1,2 채널 동시 변경
                    nTimeTick = Environment.TickCount;
                    nRetStep = 112;
                    break;
                case 112:
                    //조명
                    if (Program.nRunState == RUN_STATE.MANUAL)
                    {
                        nRetStep = 116;
                        break;
                    }
                    if (Globalo.serialPortManager.LightControl.recvCheck == -1)
                    {
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 3000)
                    {
                        szLog = $"[LIGHT] LIGHT CONTROLLER RECV FAIL [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    if (Globalo.serialPortManager.LightControl.recvCheck == 0)
                    {
                        //조명 정상 변경 실패
                        szLog = $"[LIGHT] LIGHT DATA CHANGE FAIL [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    //조명 정상 변경 완료
                    szLog = $"[LIGHT] LIGHT DATA CHANGE OK [STEP : {nRetStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nRetStep = 114;
                    nTimeTick = Environment.TickCount;
                    break;
                case 114:
                    if (Environment.TickCount - nTimeTick > captureDelay)
                    {
                        nRetStep = 116;
                    }
                    break;
                case 116:
                    //SIDE 캡처 - 이때 Top꺼져야된다.
                    Globalo.visionManager.milLibrary.ClearOverlay(VisionClass.AoiTester.SIDE_INDEX);
                    Globalo.visionManager.milLibrary.SetGrabOn(VisionClass.AoiTester.SIDE_INDEX, false);
                    Globalo.visionManager.milLibrary.GetSnapImage(VisionClass.AoiTester.SIDE_INDEX);
                    Globalo.visionManager.aoiTester.FinalBmpImageSave("Side", aoiApdData.Barcode, Globalo.visionManager.milLibrary.MilProcImageChild[VisionClass.AoiTester.SIDE_INDEX]);
                    Globalo.visionManager.milLibrary.SetGrabOn(VisionClass.AoiTester.SIDE_INDEX, true);
                    nRetStep = 120;
                    break;
                case 120:
                    
                    nRetStep = 130;
                    break;

                case 130:
                    Globalo.serialPortManager.LightControl.recvCheck = -1;
                    int tData = Globalo.yamlManager.aoiRoiConfig.topLightData[0].data;
                    int sData2 = Globalo.yamlManager.aoiRoiConfig.sideLightData[0].data;
                    Globalo.serialPortManager.LightControl.AllctrlLedVolume(tData, sData2);      //1,2 채널 동시 변경
                    nTimeTick = Environment.TickCount;
                    nRetStep = 132;
                    break;
                case 132:
                    //조명
                    if (Program.nRunState == RUN_STATE.MANUAL)
                    {
                        nRetStep = 140;
                        break;
                    }
                    if (Globalo.serialPortManager.LightControl.recvCheck == -1)
                    {
                        break;
                    }
                    else if (Environment.TickCount - nTimeTick > 3000)
                    {
                        szLog = $"[LIGHT] LIGHT CONTROLLER RECV FAIL [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        nRetStep *= -1;
                        break;
                    }

                    if (Globalo.serialPortManager.LightControl.recvCheck == 0)
                    {
                        //조명 정상 변경 실패
                        szLog = $"[LIGHT] LIGHT DATA CHANGE FAIL [STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);
                        nRetStep *= -1;
                        break;
                    }

                    //조명 정상 변경 완료
                    szLog = $"[LIGHT] LIGHT DATA CHANGE OK [STEP : {nRetStep}]";
                    Globalo.LogPrint("ManualControl", szLog);

                    nRetStep = 134;
                    nTimeTick = Environment.TickCount;
                    break;

                case 134:
                    if (Environment.TickCount - nTimeTick > captureDelay)
                    {
                        nRetStep = 140;
                    }
                    break;
                case 140:
                    
                    //TOP 캡처
                    Globalo.visionManager.milLibrary.ClearOverlay(VisionClass.AoiTester.TOP_INDEX);
                    Globalo.visionManager.milLibrary.SetGrabOn(VisionClass.AoiTester.TOP_INDEX, false);
                    Globalo.visionManager.milLibrary.GetSnapImage(VisionClass.AoiTester.TOP_INDEX);
                    Globalo.visionManager.aoiTester.FinalBmpImageSave("Top", aoiApdData.Barcode, Globalo.visionManager.milLibrary.MilProcImageChild[VisionClass.AoiTester.TOP_INDEX]);
                    Globalo.visionManager.milLibrary.SetGrabOn(VisionClass.AoiTester.TOP_INDEX, true);
                    nRetStep = 150;
                    break;
                case 150:
                    nRetStep = 160;
                    break;
                case 160:
                    nRetStep = 190;
                    break;
                case 190:
                    //----------------------------------------------------------------------------------------------------------------------------------
                    //
                    //
                    //
                    //
                    TopCamTask = Task.Run(() =>
                    {
                        waitTopCam = 1;
                        if (Program.nRunState == RUN_STATE.MANUAL)
                        {
                            waitTopCam = TopCamFlow(false);      //0 or -1 Return
                        }
                        else
                        {
                            waitTopCam = TopCamFlow();      //0 or -1 Return
                        }
                        
                        Console.WriteLine($"-------------- TopCam Task - end {waitTopCam}");
                        return waitTopCam;
                    }, CancelToken.Token);
                    //
                    //
                    SideCamTask = Task.Run(() =>
                    {
                        waitSideCam = 1;
                        if (Program.nRunState == RUN_STATE.MANUAL)
                        {
                            waitSideCam = SideCamFlow(false);      //0 or -1 Return
                        }
                        else
                        {
                            waitSideCam = SideCamFlow();      //0 or -1 Return
                        }
                           
                        Console.WriteLine($"-------------- SideCam Task - end {waitSideCam}");
                        return waitSideCam;
                    }, CancelToken.Token);
                    //
                    //
                    //
                    //
                    //
                    //----------------------------------------------------------------------------------------------------------------------------------
                    nRetStep = 200;

                    nTimeTick = Environment.TickCount;
                    break;
                case 200:
                    //Top, Side 둘다 검사 대기
                    if (waitTopCam == 1 || waitSideCam == 1)
                    {
                        if (Environment.TickCount - nTimeTick > 50000)
                        {
                            Console.WriteLine("Timeout - {waitTopCam},{waitSideCam}");
                            nRetStep = -1;
                            break;
                        }
                        break;
                    }
                    //
                    //
                    //Apd 보고 -> SecsGem Clinet -> 결과는 Handler로 전송
                    //Http.HttpService.LotApdReport(aoiApdData);     //<----바꾸자 HANDLER로 보내는걸로

                    EqipData.Type = "EquipmentData";
                    sendEqipData.Command = "LOT_APD_REPORT";
                    sendEqipData.DataID = aoiApdData.Socket_Num;
                    sendEqipData.BcrId = aoiApdData.Barcode;
                    sendEqipData.Judge = m_nTestFinalResult;
                    sendEqipData.CommandParameter.Clear();

                    string[] apdList = { 
                        "LH", "RH", "MH",  "Gasket", "KeyType", "CircleDented" , "Concentrycity_A", "Concentrycity_D", "Cone", "ORing"
                        , "Result" , "Barcode", "Socket_Num" };

                    string[] apdResult = { aoiApdData.LH, aoiApdData.RH, aoiApdData.MH,
                        aoiApdData.Gasket, aoiApdData.KeyType,aoiApdData.CircleDented, aoiApdData.Concentrycity_A, aoiApdData.Concentrycity_D,
                        aoiApdData.Cone, aoiApdData.ORing, aoiApdData.Result ,aoiApdData.Barcode, aoiApdData.Socket_Num};

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
                    nTimeTick = Environment.TickCount;


                    
                    Globalo.visionManager.aoiTester.FinalLogSave(aoiApdData);

                    nRetStep = 220;    //1000이상이면 종료
                    break;
                case 220:
                    //Verify 공정은 Secsgem으로 apd보고해야된다 . 나머지는 Handler로
                    //완공다되면 Handler로도 보내줘야된다.

                    TcpSocket.MessageWrapper objectData = new TcpSocket.MessageWrapper();
                    objectData.Type = "EquipmentData";

                    //TcpSocket.EquipmentData LotstartData = new TcpSocket.EquipmentData();
                    TcpSocket.TesterData resultData = new TcpSocket.TesterData();
                    resultData.init();
                    resultData.BcrId[0] = aoiApdData.Barcode;
                    resultData.Cmd = "CMD_RESULT";

                    resultData.States[0] = Globalo.tcpManager.nRecv_Ack;
                    //LotstartData.CommandParameter = Globalo.dataManage.TaskWork.SpecialDataParameter.Select(item => item.DeepCopy()).ToList();

                    objectData.Data = resultData;
                    Globalo.tcpManager.SendMessage_To_Handler(objectData);
                    nRetStep = 1000;
                    break;
            }
            return nRetStep;

        }

        #region [TOP CAM TEST]
        public int TopCamFlow(bool bAutorun = true)
        {
            int nRtn = -1;
            bool bRtn = false;
            string szLog = "";
            int data1 = 0;
            int data2 = 0;
            int dentCount = 0;
            int dentMaxCount = 0;
            const int topCamIndex = 0;
            int nRetStep = 10;
            if (bAutorun == false)
            {
                aoiApdData.Socket_Num = "0";
                CancelToken?.Dispose();
                CancelToken = new CancellationTokenSource();    //
            }
            while (true)
            {
                if (CancelToken.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Top Cam Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                switch (nRetStep)
                {
                    case 10:
                        aoiApdData.Gasket = "1";           //없거나,못찾으면 0 , 있으면 1
                        aoiApdData.KeyType = "A";          //찾으면 타입기록 , 못 찾으면 null기록
                        aoiApdData.CircleDented = "1";     //찌그러진 개수
                        aoiApdData.Concentrycity_A = "0.1";
                        aoiApdData.Concentrycity_D = "0.1";
                        //조명 변경
                        Globalo.setTestControl.manualConfig.checkBox_AllRelease();
                        //Globalo.visionManager.milLibrary.SetGrabOn(topCamIndex, true);

                        //Top Light Set, Ch:1
                        //Val 0 : Housing 약간 어둡게
                        //Val 1 : Key/Gasket  밝게
                        //Val 2 : Dent 0번과 비슷하게?

                        //data1 = Globalo.yamlManager.aoiRoiConfig.topLightData[0].data;
                        //data2 = Globalo.yamlManager.aoiRoiConfig.sideLightData[0].data;
                        //Globalo.serialPortManager.LightControl.ctrlLedVolume(1, data1);
                        
                        //Globalo.serialPortManager.LightControl.AllctrlLedVolume(data1, data2);      //1,2 채널 동시 변경

                        //szLog = $"[LIGHT] LIGHT CH1,2 CHANGE COMMAND[STEP : {nRetStep}]";
                        //Globalo.LogPrint("ManualControl", szLog);

                        //Side Light Set, Ch:2
                        //Val 0: Side Common - 사용 안 할 수도
                        nTopTimeTick = Environment.TickCount;
                        nRetStep = 50;
                        break;

                    case 50:
                        //if (bAutorun == false)
                        //{
                        //    nRetStep = 100;
                        //    break;
                        //}
                        //if (Globalo.serialPortManager.LightControl.recvCheck == -1)
                        //{
                        //    break;
                        //}
                        //else if (Environment.TickCount - nTopTimeTick > 3000)
                        //{
                        //    szLog = $"[LIGHT] LIGHT CONTROLLER RECV FAIL [STEP : {nRetStep}]";
                        //    Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                        //    nRetStep *= -1;
                        //    break;
                        //}

                        //if (Globalo.serialPortManager.LightControl.recvCheck == 0)
                        //{
                        //    //조명 정상 변경 실패
                        //    szLog = $"[LIGHT] LIGHT DATA CHANGE FAIL [STEP : {nRetStep}]";
                        //    Globalo.LogPrint("ManualControl", szLog);
                        //    nRetStep *= -1;
                        //    break;
                        //}

                        ////조명 정상 변경 완료
                        //szLog = $"[LIGHT] LIGHT DATA CHANGE OK [STEP : {nRetStep}]";
                        //Globalo.LogPrint("ManualControl", szLog);
                        nRetStep = 100;
                        break;

                    case 100:
                        int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[topCamIndex];
                        int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex];
                        int dataSize = sizeX * sizeY;

                        //Globalo.visionManager.milLibrary.ClearOverlay(topCamIndex);
                        //Globalo.visionManager.milLibrary.SetGrabOn(topCamIndex, false);
                        //Globalo.visionManager.milLibrary.GetSnapImage(topCamIndex);

                        byte[] ImageBuffer = new byte[dataSize];
                        MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[topCamIndex], ImageBuffer);
                        Mat TopMatImage = new Mat(sizeY, sizeX, MatType.CV_8UC1);
                        Marshal.Copy(ImageBuffer, 0, TopMatImage.Data, dataSize);
                        // 3채널로 변환
                        Cv2.CvtColor(TopMatImage, TopMatImage, ColorConversionCodes.GRAY2BGR);


                        //Globalo.visionManager.milLibrary.SetGrabOn(topCamIndex, true);
                        //Gasket - 유무 검사
                        //Dent - 찌그러짐
                        //Key - 유무 검사
                        //Housing Out - Center xy 차이
                        //Housing In - Center xy 차이
                        List<OpenCvSharp.Point> FakraCenter = new List<OpenCvSharp.Point>();
                        List<OpenCvSharp.Point> HousingCenter = new List<OpenCvSharp.Point>();

                        
                        int IsGasket = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET"].value);

                        string specKey = Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["KEYTYPE"].value;

                        int specGasketMin = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET_MIN"].value);
                        int specGasketMax = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET_MAX"].value);
                        int specDentMin = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["DENT_MIN"].value);
                        int specDentMax = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["DENT_MAX"].value);
                        double con_InMin = double.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_IN_MIN"].value);
                        double con_InMax = double.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_IN_MAX"].value);
                        double con_OutMin = double.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_OUT_MIN"].value);
                        double con_OutMax = double.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_OUT_MAX"].value);
                        double dScore = 0.0;
                        //----------------------------------------------------------------------------------------------------------------------------------------------------
                        //
                        //
                        //
                        //중심찾기
                        //
                        //
                        //
                        //----------------------------------------------------------------------------------------------------------------------------------------------------

                        //bool rtn = Globalo.visionManager.aoiTopTester.FindCircleCenter(topCamIndex, src, ref aoiCenterPos[topCamIndex], true);
                        bool rtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(topCamIndex, VisionClass.eMarkList.TOP_CENTER, ref aoiCenterPos[topCamIndex], ref dScore);
                        if (rtn)
                        {
                            szLog = $"[TOP CAM] CENTER FIND OK ({aoiCenterPos[topCamIndex].X},{aoiCenterPos[topCamIndex].Y})";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        else
                        {
                            //중심 찾기 실패
                            aoiCenterPos[topCamIndex].X = sizeX / 2;
                            aoiCenterPos[topCamIndex].Y = sizeY / 2;

                            szLog = $"[TOP CAM] CENTER FIND FAIL ({aoiCenterPos[topCamIndex].X},{aoiCenterPos[topCamIndex].Y})";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        //----------------------------------------------------------------------------------------------------------------------------------------------------
                        //
                        //
                        //가스켓 검사
                        //
                        // 유무에 따라서 검사해야된다.  0일때 있으면 ng , 1일때 없으면 ng
                        //
                        //----------------------------------------------------------------------------------------------------------------------------------------------------
                        int gasketLight = Globalo.visionManager.aoiTopTester.GasketTest(topCamIndex, TopMatImage, aoiCenterPos[topCamIndex], true);

                        ResultAoiAPdData.Gasket = string.Empty;
                        if (gasketLight < specGasketMin)// || gasketLight > specGasketMax)
                        {
                            //검사 결과 : 없다. X
                            if (IsGasket == 1)
                            {
                                //ng
                                aoiApdData.Result = "NG";

                                ResultAoiAPdData.Gasket = "NG";
                                szLog = $"[TOP CAM] GASKET LIGHT FAIL: {gasketLight} ({specGasketMin})";//({specGasketMin} ~ {specGasketMax})";
                                Globalo.LogPrint("ManualControl", szLog);
                            }
                            else
                            {
                                szLog = $"[TOP CAM] GASKET LIGHT PASS: {gasketLight} ({specGasketMin})";//({specGasketMin} ~ {specGasketMax})";
                                Globalo.LogPrint("ManualControl", szLog);
                            }
                            
                        }
                        else
                        {
                            //검사 결과 : 있다. ㅇ
                            if (IsGasket == 0)
                            {
                                //ng
                                aoiApdData.Result = "NG";

                                ResultAoiAPdData.Gasket = "NG";
                                szLog = $"[TOP CAM] GASKET LIGHT FAIL: {gasketLight} ({specGasketMin})";//({specGasketMin} ~ {specGasketMax})";
                                Globalo.LogPrint("ManualControl", szLog);
                            }
                            else
                            {
                                szLog = $"[TOP CAM] GASKET LIGHT PASS: {gasketLight} ({specGasketMin})";//({specGasketMin} ~ {specGasketMax})";
                                Globalo.LogPrint("ManualControl", szLog);
                            }
                            
                        }

                        aoiApdData.Gasket = gasketLight.ToString();

                        //----------------------------------------------------------------------------------------------------------------------------------------------------
                        //
                        //
                        //
                        //Dent (찌그러짐) 검사 
                        //
                        //
                        //
                        //----------------------------------------------------------------------------------------------------------------------------------------------------

                        HousingCenter = Globalo.visionManager.aoiTopTester.Housing_Dent_Test(topCamIndex, TopMatImage, aoiCenterPos[topCamIndex], true, true);   //true 일때 Dent(찌그러짐)검사
                        dentCount = 0;
                        dentMaxCount = 0;
                        if (HousingCenter.Count > 0)
                        {
                            dentCount = HousingCenter[0].X;
                            dentMaxCount = HousingCenter[0].Y;
                            int denUnderCnt = HousingCenter[0].X;
                            if (denUnderCnt < specDentMin || denUnderCnt > specDentMax)
                            {
                                aoiApdData.CircleDented = "0";
                            }
                            else
                            {
                                aoiApdData.CircleDented = "1";
                            }
                        }
                        else
                        {
                            aoiApdData.CircleDented = "0";
                        }
                        //----------------------------------------------------------------------------------------------------------------------------------------------------
                        //
                        //
                        //
                        //Key 검사 
                        //
                        //
                        //
                        //----------------------------------------------------------------------------------------------------------------------------------------------------
                        int key1Rtn = 0;
                        int key2Rtn = 0;
                        string keyType = Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["KEYTYPE"].value;
                        int cx = Globalo.visionManager.milLibrary.CAM_SIZE_X[topCamIndex] / 2;
                        int cy = Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] / 2;

                        double offsetx = aoiCenterPos[topCamIndex].X - cx;
                        double offsety = aoiCenterPos[topCamIndex].Y - cy;

                        //key1Rtn = Globalo.visionManager.aoiTopTester.MilEdgeKeytest(topCamIndex, 0, keyType, offsetx, offsety, true);        //키검사

                        //if (keyType != "E")
                        //{
                        //    key2Rtn = Globalo.visionManager.aoiTopTester.MilEdgeKeytest(topCamIndex, 1, keyType, offsetx, offsety, true);        //키검사
                        //}

                        double dKeyScore = 0.0;
                        OpenCvSharp.Point markPos = new OpenCvSharp.Point();
                        bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(topCamIndex, VisionClass.eMarkList.TOP_KEY, ref markPos, ref dKeyScore);

                        //if (key1Rtn == 0 || key2Rtn == 0)
                        ResultAoiAPdData.KeyType = string.Empty;
                        if (dKeyScore < 60.0)
                        {
                            //ng

                            aoiApdData.Result = "NG";
                            ResultAoiAPdData.KeyType = "NG";
                            aoiApdData.KeyType = "Null";
                            szLog = $"[TOP CAM] {keyType} FIND FAIL";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        else
                        {
                            aoiApdData.KeyType = keyType;
                            szLog = $"[TOP CAM] {keyType} FIND PASS";
                            Globalo.LogPrint("ManualControl", szLog);
                        }

                        //----------------------------------------------------------------------------------------------------------------------------------------------------
                        //
                        //
                        //동심도 검사 
                        //
                        //
                        //
                        //----------------------------------------------------------------------------------------------------------------------------------------------------

                        FakraCenter.Clear();
                        HousingCenter.Clear();

                        double CamResolX = 0.0;
                        double CamResolY = 0.0;

                        double con1Result = 0.0;
                        double con2Result = 0.0;
                        float dx = 0.0f;
                        float dy = 0.0f;
                        float dist1 = 0.0f;
                        float dist2 = 0.0f;

                        FakraCenter = Globalo.visionManager.aoiTopTester.Housing_Fakra_Test(topCamIndex, TopMatImage, aoiCenterPos[topCamIndex], true); //Fakra 안쪽 원 찾기
                        HousingCenter = Globalo.visionManager.aoiTopTester.Housing_Dent_Test(topCamIndex, TopMatImage, aoiCenterPos[topCamIndex], false,true); //Con1,2(동심도)  / Dent (찌그러짐) 검사 

                        //내원 2개 , 외원 2개씩 찾아야 진행된다.
                        ResultAoiAPdData.Concentrycity_A = string.Empty;
                        ResultAoiAPdData.Concentrycity_D = string.Empty;
                        if (FakraCenter.Count > 1 && HousingCenter.Count > 1)
                        {
                            Console.WriteLine($"In Fakra Find Fail:{FakraCenter.Count}");
                            //return;



                            CamResolX = Globalo.yamlManager.configData.CamSettings.TopResolution.X;   // 0.0186f;
                            CamResolY = Globalo.yamlManager.configData.CamSettings.TopResolution.Y;   //0.0186f;


                            OpenCvSharp.Point c1 = FakraCenter[1];
                            OpenCvSharp.Point c2 = HousingCenter[0];
                            OpenCvSharp.Point c3 = HousingCenter[1];


                            dx = c1.X - c2.X;
                            dy = c1.Y - c2.Y;
                            dist1 = (float)Math.Sqrt(dx * dx + dy * dy);
                            dx = c1.X - c3.X;
                            dy = c1.Y - c3.Y;
                            dist2 = (float)Math.Sqrt(dx * dx + dy * dy);


                            con1Result = dist1 * CamResolX;
                            con2Result = dist2 * CamResolX;

                            aoiApdData.Concentrycity_A = con1Result.ToString("0.00#");
                            aoiApdData.Concentrycity_D = con2Result.ToString("0.00#");
                        }
                        else
                        {
                            aoiApdData.Concentrycity_A = "0.0";
                            aoiApdData.Concentrycity_D = "0.0";
                        }
                        


                        //if (con1Result < con_InMin || con1Result > con_InMax)
                        if (con1Result > con_InMax)
                        {
                            aoiApdData.Result = "NG";
                            ResultAoiAPdData.Concentrycity_A = "NG";
                        }

                        //if (con2Result < con_OutMin || con2Result > con_OutMax)
                        if (con2Result > con_OutMax)
                        {
                            aoiApdData.Result = "NG";
                            ResultAoiAPdData.Concentrycity_D = "NG";
                        }

                        //
                        Globalo.visionManager.aoiTester.FinalJpgImageSave("top", aoiApdData.Barcode, TopMatImage);

                        nRetStep = 900;
                        break;
                    case 900:
                        //Top display

                        //Con1
                        //Con2
                        //Gasket
                        //Key
                        //Dent
                        System.Drawing.Point txtPoint = new System.Drawing.Point();
                        string resultStr = string.Empty;

                        resultStr = aoiApdData.Gasket + "," + aoiApdData.KeyType + "," + aoiApdData.CircleDented + "," + aoiApdData.Concentrycity_A + "," + aoiApdData.Concentrycity_D;
                        
                        _syncContext.Send(_ =>
                        {
                            Globalo.cameraControl.setTopTestResult(int.Parse(aoiApdData.Socket_Num), resultStr);
                        }, null);

                        double con_InMin2 = double.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_IN_MIN"].value);
                        double con_InMax2 = double.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_IN_MAX"].value);
                        double con_OutMin2 = double.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_OUT_MIN"].value);
                        double con_OutMax2 = double.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_OUT_MAX"].value);

                        resultStr = $"Con1 :{aoiApdData.Concentrycity_A} [~{con_InMax2}]";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 800);
                        if (ResultAoiAPdData.Concentrycity_A == "NG")
                        {
                            Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.Red, 13);
                        }
                        else
                        {
                            Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);
                        }
                        

                        resultStr = $"Con2 :{aoiApdData.Concentrycity_D} [~{con_OutMax2}]";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 700);
                        if (ResultAoiAPdData.Concentrycity_D == "NG")
                        {
                            Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.Red, 13);
                        }
                        else
                        {
                            Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);
                        }
                        

                        //dentCount = HousingCenter[0].X;
                        //dentMaxCount = HousingCenter[0].Y;

                        resultStr = $"Dent :{aoiApdData.CircleDented} -[{dentCount} / {dentMaxCount}]";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 600);
                        Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);


                        int Is_Gasket = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET"].value);
                        int specGasket = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET_MIN"].value);
                        resultStr = $"Gasket :{aoiApdData.Gasket} [{Is_Gasket}/{specGasket}]";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 500);
                        if (ResultAoiAPdData.Gasket == "NG")
                        {
                            Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.Red, 13);
                        }
                        else
                        {
                            Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);
                        }


                        string specKey2 = Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["KEYTYPE"].value;

                        //resultStr = $"Key :{aoiApdData.KeyType}";  //$"Key {keyType} - {key1Rtn} / {key2Rtn} ";
                        resultStr = $"Key :{aoiApdData.KeyType} [{specKey2}]";// - {key1Rtn} / {key2Rtn}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 400);
                        if (ResultAoiAPdData.KeyType == "NG")
                        {
                            Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.Red, 13);
                        }
                        else
                        {
                            Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);
                        }
                        


                        Globalo.visionManager.milLibrary.DrawOverlayAll(topCamIndex);


                        Globalo.visionManager.aoiTester.FinalLogSave(aoiApdData);


                        //
                        //
                        //int sizeX2 = Globalo.visionManager.milLibrary.CAM_SIZE_X[topCamIndex];
                        //int sizeY2 = Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex];
                        //int dataSize2 = sizeX2 * sizeY2;
                        //byte[] ImageBuffer2 = new byte[dataSize2];
                        ////
                        //MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[topCamIndex], ImageBuffer2);
                        //Mat src2 = new Mat(sizeY2, sizeX2, MatType.CV_8UC1);
                        //Marshal.Copy(ImageBuffer2, 0, src2.Data, dataSize2);
                        //string sidepath = $"d:\\srcImage_{sidecount}.jpg";
                        //Cv2.ImWrite(sidepath, src2);
                        //sidecount++;
                        //string autostr = "";
                        //string csvLine = $"{aoiApdData.Concentrycity_A}, {aoiApdData.Concentrycity_D}, {aoiApdData.CircleDented}, {aoiApdData.Gasket}, {aoiApdData.KeyType}";

                        //string filePath = "top_test.csv";
                        //// 파일이 없으면 헤더 추가
                        //if (!File.Exists(filePath))
                        //{
                        //    File.AppendAllText(filePath, "CON1, CON2, DENT, GASKET, KEYTYPE" + Environment.NewLine);
                        //}
                        //try
                        //{
                        //    File.AppendAllText(filePath, csvLine + Environment.NewLine);
                        //}
                        //catch (IOException)
                        //{

                        //}
                        Globalo.visionManager.milLibrary.SetGrabOn(topCamIndex, true);
                        nRetStep = 1000;
                        break;
                    default:
                        break;
                }

                if (nRetStep < 0)
                {
                    Console.WriteLine("Top Cam Flow - fail");
                    break;
                }

                if (nRetStep == 1000)
                {
                    Console.WriteLine("Top Cam Flow - end");
                    break;
                }
                Thread.Sleep(10);       //TODO: while문안에서는 최소 10ms 꼭 필요
            }
            if (nRetStep == 1000)
            {
                nRtn = 0;
                Console.WriteLine("Top Cam Flow - ok");
            }
            else
            {
                nRtn = -1;
                Console.WriteLine("Top Cam Flow - ng");
            }
            return nRtn;
        }


        #endregion
        #region [SIDE CAM TEST]
        public int SideCamFlow(bool bAutorun = true)
        {
            int nRtn = -1;
            bool bRtn = false;
            string szLog = "";
            const int sideCamIndex = 1;
            int nRetStep = 10;
            double dOringScore = 0.0;
            double dHeightScore = 0.0;
            double dConeScore = 0.0;
            if (bAutorun == false)
            {
                aoiApdData.Socket_Num = "0";
                CancelToken?.Dispose();
                CancelToken = new CancellationTokenSource();    //
            }
            while (true)
            {
                if (CancelToken.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Side Cam Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                //pauseEvent.Wait();  // 일시정지시 여기서 멈춰 있음
                //Tester에서는 일시정지 없을듯
                switch (nRetStep)
                {
                    case 10:
                        //Side 조명은 Top에서 같이 변경
                        nSideTimeTick = Environment.TickCount;
                        nRetStep = 12;
                        break;
                    case 12:
                        if (bAutorun == false)
                        {
                            nRetStep = 20;
                            break;
                        }
                        if (Globalo.serialPortManager.LightControl.recvCheck == 1)
                        {
                            nRetStep = 20;
                            break;
                        }
                        else if (Environment.TickCount - nSideTimeTick > 5000)
                        {
                            szLog = $"[LIGHT] LIGHT CONTROLLER RECV FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep *= -1;
                            break;
                        }
                        break;
                    case 20:

                        //== 높이 측정 기준 Mark 찾기

                        //Globalo.visionManager.milLibrary.ClearOverlay(sideCamIndex);
                        //Globalo.visionManager.milLibrary.SetGrabOn(sideCamIndex, false);
                        //Globalo.visionManager.milLibrary.GetSnapImage(sideCamIndex);
                        //-------------------------------------------------------------------------------------------

                        int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[sideCamIndex];
                        int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[sideCamIndex];
                        int dataSize = sizeX * sizeY;

                        byte[] ImageBuffer = new byte[dataSize];
                        MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[sideCamIndex], ImageBuffer);
                        Mat SideMatImage = new Mat(sizeY, sizeX, MatType.CV_8UC1);
                        Marshal.Copy(ImageBuffer, 0, SideMatImage.Data, dataSize);
                        // 3채널로 변환
                        Cv2.CvtColor(SideMatImage, SideMatImage, ColorConversionCodes.GRAY2BGR);

                        //Left Height
                        //Center Height
                        //Right Height
                        ////Globalo.visionManager.aoiSideTester.HeightTest(sideCamIndex);

                        OpenCvSharp.Point markPos = new OpenCvSharp.Point();
                        bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(sideCamIndex, VisionClass.eMarkList.SIDE_HEIGHT, ref markPos, ref dHeightScore);

                        System.Drawing.Point OffsetPos = new System.Drawing.Point(0, 0);
                        double[] heightData = new double[3];
                        if (bRtn)
                        {
                            OffsetPos.X = markPos.X - Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[1].X;
                            OffsetPos.Y = markPos.Y - Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[1].Y;

                        }

                        heightData[0] = Globalo.visionManager.aoiSideTester.MilEdgeHeight(sideCamIndex, 0, OffsetPos, SideMatImage, true);
                        heightData[1] = Globalo.visionManager.aoiSideTester.MilEdgeHeight(sideCamIndex, 1, OffsetPos, SideMatImage, true);
                        heightData[2] = Globalo.visionManager.aoiSideTester.MilEdgeHeight(sideCamIndex, 2, OffsetPos, SideMatImage, true);

                        aoiApdData.LH = heightData[0].ToString("0.0##");
                        aoiApdData.MH = heightData[1].ToString("0.0##");
                        aoiApdData.RH = heightData[2].ToString("0.0##");
                        //-------------------------------------------------------------------------------------------
                        //
                        //
                        //%%
                        //Oring 유무
                        //
                        //
                        //-------------------------------------------------------------------------------------------
                        int IsOring = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["O_RING"].value);

                        //bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(sideCamIndex, VisionClass.eMarkList.SIDE_ORING, ref markPos, ref dOringScore);


                        //OffsetPos.X = 0;
                        //OffsetPos.Y = 0;

                        //if (bRtn)
                        //{
                        //    OffsetPos.X = markPos.X - (Globalo.yamlManager.aoiRoiConfig.ORING_ROI[0].X + (Globalo.yamlManager.aoiRoiConfig.ORING_ROI[0].Width / 2));
                        //    OffsetPos.Y = markPos.Y - (Globalo.yamlManager.aoiRoiConfig.ORING_ROI[0].Y + (Globalo.yamlManager.aoiRoiConfig.ORING_ROI[0].Height / 2));

                        //}

                        //bool bOringRtn = Globalo.visionManager.aoiSideTester.MilEdgeOringTest(sideCamIndex, 0, OffsetPos, true);
                        bool bOringRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(sideCamIndex, VisionClass.eMarkList.SIDE_ORING, ref markPos, ref dOringScore);
                        if (IsOring == 1 && bOringRtn)
                        {
                            if (dOringScore > 70.0)
                            {
                                aoiApdData.ORing = "1";
                            }
                            else
                            {
                                aoiApdData.Result = "NG";
                                aoiApdData.ORing = "0";
                            }
                        }
                        else
                        {
                            aoiApdData.ORing = "0";
                        }
                        
                        //-------------------------------------------------------------------------------------------
                        //
                        //
                        //
                        //Cone 유무
                        //
                        //
                        //-------------------------------------------------------------------------------------------
                        int IsCone = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONE"].value);
                        //bRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(sideCamIndex, VisionClass.eMarkList.SIDE_CONE, ref markPos, ref dSideScore);


                        //OffsetPos.X = 0;
                        //OffsetPos.Y = 0;

                        //if (bRtn)
                        //{
                        //    OffsetPos.X = markPos.X - (Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].X + (Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].Width / 2));
                        //    OffsetPos.Y = markPos.Y - (Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].Y + (Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].Height / 2));

                        //}

                        //bool bConeRtn = Globalo.visionManager.aoiSideTester.MilEdgeConeTest(sideCamIndex, 0, OffsetPos, true);//, src);
                        bool bConeRtn = Globalo.visionManager.aoiSideTester.Mark_Pos_Standard(sideCamIndex, VisionClass.eMarkList.SIDE_CONE, ref markPos, ref dConeScore);
                        if (IsCone == 1)
                        {
                            if (dConeScore > 65.0)
                            {
                                aoiApdData.Cone = "1";
                            }
                            else
                            {

                                aoiApdData.Result = "NG";
                                aoiApdData.Cone = "0";
                            }
                        }
                        else
                        {

                            aoiApdData.Cone = "0";
                        }

                        Globalo.visionManager.aoiTester.FinalJpgImageSave("Side", aoiApdData.Barcode, SideMatImage);

                        nRetStep = 50;
                        break;
                    case 50:
                        nRetStep = 900;
                        break;
                    case 900:
                        System.Drawing.Point txtPoint = new System.Drawing.Point();
                        string resultStr = string.Empty;


                        resultStr = aoiApdData.LH + "," + aoiApdData.MH + "," + aoiApdData.RH + "," + aoiApdData.Cone + "," + aoiApdData.ORing;
                        _syncContext.Send(_ =>
                        {
                            Globalo.cameraControl.setSideTestResult(int.Parse(aoiApdData.Socket_Num), resultStr);
                        }, null);
                        

                        resultStr = $"O-Ring :{aoiApdData.ORing} / {dOringScore.ToString("0.0#")}%";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[sideCamIndex] - 600);
                        Globalo.visionManager.milLibrary.DrawOverlayText(sideCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

                        resultStr = $"Cone :{aoiApdData.Cone} / {dConeScore.ToString("0.0#")}%";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[sideCamIndex] - 500);
                        Globalo.visionManager.milLibrary.DrawOverlayText(sideCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

        

                        Globalo.visionManager.milLibrary.DrawOverlayAll(sideCamIndex);
                        Globalo.visionManager.milLibrary.SetGrabOn(sideCamIndex, true);



                        //int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[sideCamIndex];
                        //int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[sideCamIndex];
                        //int dataSize = sizeX * sizeY;
                        //byte[] ImageBuffer = new byte[dataSize];

                        //
                        Globalo.visionManager.milLibrary.SetGrabOn(sideCamIndex, false);//lee
                        Globalo.visionManager.milLibrary.GetSnapImage(sideCamIndex);

                        //
                        //
                        //test
                        //
                        //MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[sideCamIndex], ImageBuffer);
                        //Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
                        //Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);
                        //string sidepath = $"d:\\srcImage_{sidecount}.jpg";
                        //Cv2.ImWrite(sidepath, src);
                        //sidecount++;
                        //string autostr = "";
                        //string csvLine = $"{aoiApdData.LH},{aoiApdData.MH},{aoiApdData.RH},{aoiApdData.Cone},{aoiApdData.ORing}";

                        //string filePath = "side_test.csv";
                        //// 파일이 없으면 헤더 추가
                        //if (!File.Exists(filePath))
                        //{
                        //    File.AppendAllText(filePath, "LH,MH,RH,CONE,ORING" + Environment.NewLine);
                        //}
                        //try
                        //{
                        //    File.AppendAllText(filePath, csvLine + Environment.NewLine);
                        //}
                        //catch (IOException)
                        //{

                        //}
                        nRetStep = 1000;
                        break;
                    default:
                        break;
                }

                if (nRetStep < 0)
                {
                    Console.WriteLine("Side Cam Flow - fail");
                    break;
                }

                if (nRetStep == 1000)
                {
                    Console.WriteLine("Side Cam Flow - end");
                    break;
                }
                Thread.Sleep(10);       //TODO: while문안에서는 최소 10ms 꼭 필요
            }
            if (nRetStep == 1000)
            {
                nRtn = 0;
                Console.WriteLine("Side Cam Flow - ok");
            }
            else
            {
                nRtn = -1;
                Console.WriteLine("Side Cam Flow - ng");
            }
            return nRtn;
        }
        #endregion
    }
}
