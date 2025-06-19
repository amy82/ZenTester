using System;
using System.Collections.Generic;
using System.Drawing;
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

        public TcpSocket.AoiApdData aoitestData = new TcpSocket.AoiApdData();
        private OpenCvSharp.Point[] aoiCenterPos = new OpenCvSharp.Point[2];
        private TcpSocket.MessageWrapper EqipData = new TcpSocket.MessageWrapper();
        private TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
        private int m_nTestFinalResult;
        public AoiTestFlow()
        {
            _syncContext = SynchronizationContext.Current;

            TopCamTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
            SideCamTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
        }

        public int AoiAutoProcess(int nStep)
        {
            int nRetStep = nStep;

            switch (nRetStep)
            {
                case 100:
                    Globalo.serialPortManager.LightControl.recvCheck = -1;
                    m_nTestFinalResult = 1;
                    Globalo.visionManager.milLibrary.RunModeChange(true);
                    waitTopCam = -1;
                    waitSideCam = -1;
                    TopCamTask = null;
                    SideCamTask = null;
                    CancelToken?.Dispose();
                    CancelToken = new CancellationTokenSource();    //
                    
                    TopCamTask = Task.Run(() =>
                    {
                        waitTopCam = 1;
                        waitTopCam = TopCamFlow();      //0 or -1 Return
                        Console.WriteLine($"-------------- TopCam Task - end {waitTopCam}");
                        return waitTopCam;
                    }, CancelToken.Token);

                    SideCamTask = Task.Run(() =>
                    {
                        waitSideCam = 1;
                        waitSideCam = SideCamFlow();      //0 or -1 Return
                        Console.WriteLine($"-------------- SideCam Task - end {waitSideCam}");
                        return waitSideCam;
                    }, CancelToken.Token);

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
                    //Http.HttpService.LotApdReport(aoitestData);     //<----바꾸자 HANDLER로 보내는걸로

                    EqipData.Type = "EquipmentData";
                    sendEqipData.Command = "LOT_APD_REPORT";
                    sendEqipData.LotID = aoitestData.Barcode;
                    sendEqipData.Judge = m_nTestFinalResult;

                    string[] apdList = { 
                        "LH", "RH", "MH",  "Gasket", "KeyType", "CircleDented" , "Concentrycity_A", "Concentrycity_D", "Cone", "ORing"
                        , "Result" , "Barcode", "Socket_Num" };

                    string[] apdResult = { aoitestData.LH, aoitestData.RH, aoitestData.MH, 
                        aoitestData.Gasket, aoitestData.KeyType,aoitestData.CircleDented, aoitestData.Concentrycity_A, aoitestData.Concentrycity_D,
                        aoitestData.Cone, aoitestData.ORing, aoitestData.Result ,aoitestData.Barcode, aoitestData.Socket_Num};

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

                    nRetStep = 220;    //1000이상이면 종료
                    break;
                case 220:
                    //Verify 공정은 Secsgem으로 apd보고해야된다 . 나머지는 Handler로
                    //완공다되면 Handler로도 보내줘야된다.


                    TcpSocket.MessageWrapper objectData = new TcpSocket.MessageWrapper();
                    objectData.Type = "EquipmentData";

                    TcpSocket.EquipmentData LotstartData = new TcpSocket.EquipmentData();
                    LotstartData.LotID = aoitestData.Barcode;
                    LotstartData.Command = "APS_LOT_FINISH";
                    LotstartData.Judge = Globalo.tcpManager.nRecv_Ack;
                    //LotstartData.CommandParameter = Globalo.dataManage.TaskWork.SpecialDataParameter.Select(item => item.DeepCopy()).ToList();

                    objectData.Data = LotstartData;
                    Globalo.tcpManager.SendMessage_To_Handler(objectData);
                    break;
            }
            return nRetStep;

        }

        #region [TOP CAM TEST]
        private int TopCamFlow()
        {
            int nRtn = -1;
            bool bRtn = false;
            string szLog = "";
            int data1 = 0;
            int data2 = 0;
            const int topCamIndex = 0;
            int nRetStep = 10;
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
                        aoitestData.Gasket = "1";           //없거나,못찾으면 0 , 있으면 1
                        aoitestData.KeyType = "A";          //찾으면 타입기록 , 못 찾으면 null기록
                        aoitestData.CircleDented = "1";     //찌그러진 개수
                        aoitestData.Concentrycity_A = "0.1";
                        aoitestData.Concentrycity_D = "0.1";
                        //조명 변경
                        Globalo.setTestControl.manualConfig.checkBox_AllRelease();
                        //Globalo.visionManager.milLibrary.SetGrabOn(topCamIndex, true);

                        //Top Light Set, Ch:1
                        //Val 0 : Housing 약간 어둡게
                        //Val 1 : Key/Gasket  밝게
                        //Val 2 : Dent 0번과 비슷하게?

                        data1 = Globalo.yamlManager.aoiRoiConfig.topLightData[0].data;
                        data2 = Globalo.yamlManager.aoiRoiConfig.sideLightData[0].data;
                        //Globalo.serialPortManager.LightControl.ctrlLedVolume(1, data1);
                        
                        Globalo.serialPortManager.LightControl.AllctrlLedVolume(data1, data2);      //1,2 채널 동시 변경

                        szLog = $"[LIGHT] LIGHT CH1,2 CHANGE COMMAND[STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        //Side Light Set, Ch:2
                        //Val 0: Side Common - 사용 안 할 수도
                        nTopTimeTick = Environment.TickCount;
                        nRetStep = 50;
                        break;

                    case 50:
                        if (Globalo.serialPortManager.LightControl.recvCheck == -1)
                        {
                            break;
                        }
                        else if (Environment.TickCount - nTopTimeTick > 3000)
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
                        nRetStep = 100;
                        break;

                    case 100:
                        int sizeX = Globalo.visionManager.milLibrary.CAM_SIZE_X[topCamIndex];
                        int sizeY = Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex];
                        int dataSize = sizeX * sizeY;

                        Globalo.visionManager.milLibrary.ClearOverlay(topCamIndex);
                        Globalo.visionManager.milLibrary.SetGrabOn(topCamIndex, false);
                        Globalo.visionManager.milLibrary.GetSnapImage(topCamIndex);

                        byte[] ImageBuffer = new byte[dataSize];
                        MIL.MbufGet(Globalo.visionManager.milLibrary.MilProcImageChild[topCamIndex], ImageBuffer);
                        Mat src = new Mat(sizeY, sizeX, MatType.CV_8UC1);
                        Marshal.Copy(ImageBuffer, 0, src.Data, dataSize);

                        //Globalo.visionManager.milLibrary.SetGrabOn(topCamIndex, true);
                        //Gasket - 유무 검사
                        //Dent - 찌그러짐
                        //Key - 유무 검사
                        //Housing Out - Center xy 차이
                        //Housing In - Center xy 차이
                        List<OpenCvSharp.Point> FakraCenter = new List<OpenCvSharp.Point>();
                        List<OpenCvSharp.Point> HousingCenter = new List<OpenCvSharp.Point>();

                        int IsOring = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["O_RING"].value);
                        int IsCone = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONE"].value);
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

                        //----------------------------------------------------------------------------------------------------------------------------------------------------
                        //
                        //
                        //
                        //중심찾기
                        //
                        //
                        //
                        //----------------------------------------------------------------------------------------------------------------------------------------------------

                        bool rtn = Globalo.visionManager.aoiTopTester.FindCircleCenter(topCamIndex, src, ref aoiCenterPos[topCamIndex], true);
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
                        int gasketLight = Globalo.visionManager.aoiTopTester.GasketTest(topCamIndex, src, aoiCenterPos[topCamIndex], true);


                        if (gasketLight < specGasketMin)// || gasketLight > specGasketMax)
                        {
                            //검사 결과 : 없다. X
                            if (IsGasket == 1)
                            {
                                //ng
                                aoitestData.Result = "0";

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
                                aoitestData.Result = "0";

                                szLog = $"[TOP CAM] GASKET LIGHT FAIL: {gasketLight} ({specGasketMin})";//({specGasketMin} ~ {specGasketMax})";
                                Globalo.LogPrint("ManualControl", szLog);
                            }
                            else
                            {
                                szLog = $"[TOP CAM] GASKET LIGHT PASS: {gasketLight} ({specGasketMin})";//({specGasketMin} ~ {specGasketMax})";
                                Globalo.LogPrint("ManualControl", szLog);
                            }
                            
                        }

                        aoitestData.Gasket = gasketLight.ToString();

                        //----------------------------------------------------------------------------------------------------------------------------------------------------
                        //
                        //
                        //
                        //Dent (찌그러짐) 검사 
                        //
                        //
                        //
                        //----------------------------------------------------------------------------------------------------------------------------------------------------

                        HousingCenter = Globalo.visionManager.aoiTopTester.Housing_Dent_Test(topCamIndex, src, true, true);   //true 일때 Dent(찌그러짐)검사
                        if (HousingCenter.Count > 0)
                        {
                            int denUnderCnt = HousingCenter[0].X;
                            if (denUnderCnt < specDentMin || denUnderCnt > specDentMax)
                            {
                                aoitestData.CircleDented = "0";
                            }
                            else
                            {
                                aoitestData.CircleDented = "1";
                            }
                        }
                        else
                        {
                            aoitestData.CircleDented = "0";
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

                        key1Rtn = Globalo.visionManager.aoiTopTester.MilEdgeKeytest(topCamIndex, 0, keyType, offsetx, offsety, true);        //키검사

                        if (keyType != "E")
                        {
                            key2Rtn = Globalo.visionManager.aoiTopTester.MilEdgeKeytest(topCamIndex, 1, keyType, offsetx, offsety, true);        //키검사
                        }

                        if (key1Rtn == 0 || key2Rtn == 0)
                        {
                            //ng
                            aoitestData.Result = "0";
                            aoitestData.KeyType = "null";
                            szLog = $"[TOP CAM] {keyType} FIND FAIL";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        else
                        {
                            aoitestData.KeyType = keyType;
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

                        FakraCenter = Globalo.visionManager.aoiTopTester.Housing_Fakra_Test(topCamIndex, src, true); //Fakra 안쪽 원 찾기
                        HousingCenter = Globalo.visionManager.aoiTopTester.Housing_Dent_Test(topCamIndex, src, false,true); //Con1,2(동심도)  / Dent (찌그러짐) 검사 

                        //내원 2개 , 외원 2개씩 찾아야 진행된다.
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

                            aoitestData.Concentrycity_A = con1Result.ToString("0.00#");
                            aoitestData.Concentrycity_D = con2Result.ToString("0.00#");
                        }
                        else
                        {
                            aoitestData.Concentrycity_A = "0.0";
                            aoitestData.Concentrycity_D = "0.0";
                        }
                        


                        if (con1Result < con_InMin || con1Result > con_InMax)
                        {
                            aoitestData.Result = "0";
                        }

                        if (con2Result < con_OutMin || con2Result > con_OutMax)
                        {
                            aoitestData.Result = "0";
                        }

                        
                        
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


                        resultStr = $"Con1 :{aoitestData.Concentrycity_A}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 600);
                        Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

                        resultStr = $"Con2 :{aoitestData.Concentrycity_D}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 500);
                        Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

                        resultStr = $"Dent :{aoitestData.CircleDented}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 400);
                        Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

                        resultStr = $"Gasket :{aoitestData.Gasket}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 300);
                        Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

                        //resultStr = $"Key :{aoitestData.KeyType}";  //$"Key {keyType} - {key1Rtn} / {key2Rtn} ";
                        resultStr = $"Key :{aoitestData.KeyType}";// - {key1Rtn} / {key2Rtn}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 200);
                        Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);


                        Globalo.visionManager.milLibrary.DrawOverlayAll(topCamIndex);

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
        private int SideCamFlow()
        {
            int nRtn = -1;
            bool bRtn = false;
            string szLog = "";
            const int sideCamIndex = 1;
            int nRetStep = 10;
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


                        //-------------------------------------------------------------------------------------------
                        //Left Height
                        //Center Height
                        //Right Height
                        Globalo.visionManager.aoiSideTester.HeightTest(sideCamIndex);

                        double[] heightData = new double[3];

                        heightData[0] = Globalo.visionManager.aoiSideTester.MilEdgeHeight(sideCamIndex, 0);
                        heightData[1] = Globalo.visionManager.aoiSideTester.MilEdgeHeight(sideCamIndex, 1);
                        heightData[2] = Globalo.visionManager.aoiSideTester.MilEdgeHeight(sideCamIndex, 2);

                        aoitestData.LH = heightData[0].ToString("0.0##");
                        aoitestData.MH = heightData[0].ToString("0.0##");
                        aoitestData.RH = heightData[0].ToString("0.0##");
                        //-------------------------------------------------------------------------------------------
                        //
                        //
                        //
                        //Oring 유무
                        //
                        //-------------------------------------------------------------------------------------------
                        bool bOringRtn = Globalo.visionManager.aoiSideTester.MilEdgeOringTest(sideCamIndex, 0);
                        if (bOringRtn)
                        {
                            aoitestData.ORing = "1";
                        }
                        else
                        {
                            aoitestData.ORing = "0";
                        }
                        //-------------------------------------------------------------------------------------------
                        //
                        //
                        //
                        //Cone 유무
                        //
                        //-------------------------------------------------------------------------------------------
                        bool bConeRtn = Globalo.visionManager.aoiSideTester.MilEdgeConeTest(sideCamIndex, 0);//, src);
                        if (bConeRtn)
                        {
                            aoitestData.Cone = "1";
                        }
                        else
                        {
                            aoitestData.Cone = "0";
                        }
                        
                        
                        nRetStep = 50;
                        break;
                    case 50:
                        nRetStep = 900;
                        break;
                    case 900:
                        System.Drawing.Point txtPoint = new System.Drawing.Point();
                        string resultStr = string.Empty;


                        resultStr = $"O-Ring :{aoitestData.ORing}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[sideCamIndex] - 600);
                        Globalo.visionManager.milLibrary.DrawOverlayText(sideCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

                        resultStr = $"Cone :{aoitestData.Cone}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[sideCamIndex] - 500);
                        Globalo.visionManager.milLibrary.DrawOverlayText(sideCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

        

                        Globalo.visionManager.milLibrary.DrawOverlayAll(sideCamIndex);
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
