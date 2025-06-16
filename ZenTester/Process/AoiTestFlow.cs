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
        public int nLoadTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯
        public int nUnloadTimeTick = 0;           //<-----동시 동작일대 같이 쓰면 안될듯

        private TcpSocket.AoiApdData aoitestData = new TcpSocket.AoiApdData();
        private OpenCvSharp.Point[] aoiCenterPos = new OpenCvSharp.Point[2];
        public AoiTestFlow()
        {
            _syncContext = SynchronizationContext.Current;

            TopCamTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
            SideCamTask = Task.FromResult(1);      //<--실제 실행하지않고,즉시 완료된 상태로 반환
        }

        public int AoiAutoProcess(int nStep)
        {
            int localSocketNumber = 0;
            int nRetStep = nStep;

            switch (nRetStep)
            {
                case 100:
                    waitTopCam = -1;
                    waitSideCam = -1;
                    TopCamTask = null;
                    SideCamTask = null;
                    CancelToken?.Dispose();
                    CancelToken = new CancellationTokenSource();    //

                    aoitestData.init();     //AOI 결과값 초기화

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
                        if (Environment.TickCount - nTimeTick > 10000)
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
                    Http.HttpService.LotApdReport(aoitestData);
                    nRetStep = 1000;    //1000이상이면 종료
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
            int topCamIndex = 0;
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
                        Globalo.visionManager.milLibrary.SetGrabOn(topCamIndex, true);

                        //Top Light Set, Ch:1
                        //Val 0 : Housing 약간 어둡게
                        //Val 1 : Key/Gasket  밝게
                        //Val 2 : Dent 0번과 비슷하게?

                        data1 = Globalo.yamlManager.aoiRoiConfig.topLightData[0].data;
                        data2 = Globalo.yamlManager.aoiRoiConfig.sideLightData[0].data;
                        //Globalo.serialPortManager.LightControl.ctrlLedVolume(1, data1);
                        Globalo.serialPortManager.LightControl.recvCheck = -1;
                        Globalo.serialPortManager.LightControl.AllctrlLedVolume(data1, data2);      //1,2 채널 동시 변경

                        szLog = $"[LIGHT] LIGHT CH1,2 CHANGE COMMAND[STEP : {nRetStep}]";
                        Globalo.LogPrint("ManualControl", szLog);

                        //Side Light Set, Ch:2
                        //Val 0: Side Common - 사용 안 할 수도
                        nRetStep = 50;
                        break;

                    case 50:
                        if (Globalo.serialPortManager.LightControl.recvCheck == -1)
                        {
                            break;
                        }
                        else if (Environment.TickCount - nTimeTick > 3000)
                        {
                            szLog = $"[LIGHT] LIGHT CONTROLLER RECV FAIL [STEP : {nRetStep}]";
                            Globalo.LogPrint("ManualControl", szLog);
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

                        Globalo.visionManager.milLibrary.SetGrabOn(topCamIndex, true);
                        //Gasket - 유무 검사
                        //Dent - 찌그러짐
                        //Key - 유무 검사
                        //Housing Out - Center xy 차이
                        //Housing In - Center xy 차이
                        List<OpenCvSharp.Point> FakraCenter = new List<OpenCvSharp.Point>();
                        List<OpenCvSharp.Point> HousingCenter = new List<OpenCvSharp.Point>();
                        string specKey = Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["KEYTYPE"].value;

                        int specGasketMin = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET_MIN"].value);
                        int specGasketMax = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET_MAX"].value);
                        int specDentMin = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["DENT_MIN"].value);
                        int specDentMax = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["DENT_MAX"].value);
                        int con_InMin = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_IN_MIN"].value);
                        int con_InMax = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_IN_MAX"].value);
                        int con_OutMin = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_OUT_MIN"].value);
                        int con_OutMax = int.Parse(Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_OUT_MAX"].value);


                        //중심찾기
                        //
                        //
                        bool rtn = Globalo.visionManager.aoiTopTester.FindCircleCenter(topCamIndex, src, ref aoiCenterPos[topCamIndex]);
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
                        //가스켓 검사
                        //
                        //
                        int gasketLight = Globalo.visionManager.aoiTopTester.GasketTest(topCamIndex, src, aoiCenterPos[topCamIndex]);


                        if (gasketLight < specGasketMin || gasketLight > specGasketMax)
                        {
                            //ng
                            aoitestData.Result = "0";

                            szLog = $"[TOP CAM] GASKET LIGHT FAIL: {gasketLight} ({specGasketMin} ~ {specGasketMax})";
                            Globalo.LogPrint("ManualControl", szLog);
                        }
                        else
                        {
                            szLog = $"[TOP CAM] GASKET LIGHT PASS: {gasketLight} ({specGasketMin} ~ {specGasketMax})";
                            Globalo.LogPrint("ManualControl", szLog);
                        }

                        aoitestData.Gasket = gasketLight.ToString();
                        //Dent (찌그러짐) 검사 
                        //
                        //
                        HousingCenter = Globalo.visionManager.aoiTopTester.Housing_Dent_Test(topCamIndex, src, true);   //true 일때 Dent(찌그러짐)검사

                        //Key 검사 
                        //
                        //
                        int key1Rtn = 0;
                        int key2Rtn = 0;
                        string keyType = Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["KEYTYPE"].value;
                        int cx = Globalo.visionManager.milLibrary.CAM_SIZE_X[topCamIndex];
                        int cy = Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex];

                        double offsetx = aoiCenterPos[topCamIndex].X - cx;
                        double offsety = aoiCenterPos[topCamIndex].Y - cy;

                        key1Rtn = Globalo.visionManager.aoiTopTester.MilEdgeKeytest(topCamIndex, 0, keyType, offsetx, offsety);        //키검사

                        if (keyType != "E")
                        {
                            key2Rtn = Globalo.visionManager.aoiTopTester.MilEdgeKeytest(topCamIndex, 1, keyType, offsetx, offsety);        //키검사
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


                        //동심도 검사 
                        //
                        //
                        FakraCenter.Clear();
                        HousingCenter.Clear();

                        FakraCenter = Globalo.visionManager.aoiTopTester.Housing_Fakra_Test(topCamIndex, src); //Fakra 안쪽 원 찾기
                        HousingCenter = Globalo.visionManager.aoiTopTester.Housing_Dent_Test(topCamIndex, src); //Con1,2(동심도)  / Dent (찌그러짐) 검사 
                        if (FakraCenter.Count < 2)
                        {
                            Console.WriteLine($"In Fakra Find Fail:{FakraCenter.Count}");
                            //return;
                        }
                        if (HousingCenter.Count < 2)
                        {
                            Console.WriteLine($"Out Fakra Find Fail:{HousingCenter.Count}");
                            //return;
                        }
                        double CamResolX = 0.0;
                        double CamResolY = 0.0;

                        double con1Result = 0.0;
                        double con2Result = 0.0;
                        CamResolX = Globalo.yamlManager.configData.CamSettings.TopResolution.X;   // 0.0186f;
                        CamResolY = Globalo.yamlManager.configData.CamSettings.TopResolution.Y;   //0.0186f;


                        OpenCvSharp.Point c1 = FakraCenter[1];
                        OpenCvSharp.Point c2 = HousingCenter[0];
                        OpenCvSharp.Point c3 = HousingCenter[1];
                        float dx = 0.0f;
                        float dy = 0.0f;
                        float dist1 = 0.0f;
                        float dist2 = 0.0f;

                        dx = c1.X - c2.X;
                        dy = c1.Y - c2.Y;
                        dist1 = (float)Math.Sqrt(dx * dx + dy * dy);
                        dx = c1.X - c3.X;
                        dy = c1.Y - c3.Y;
                        dist2 = (float)Math.Sqrt(dx * dx + dy * dy);


                        con1Result = dist1 * CamResolX;
                        con2Result = dist2 * CamResolX;

                        aoitestData.Concentrycity_A = con1Result.ToString();
                        aoitestData.Concentrycity_D = con2Result.ToString();


                        if (con1Result < con_InMin || con1Result > con_InMax)
                        {
                            aoitestData.Result = "0";
                        }
                        else
                        {

                        }
                        if (con2Result < con_OutMin || con2Result > con_OutMax)
                        {
                            aoitestData.Result = "0";
                        }
                        else
                        {

                        }

                        aoitestData.CircleDented = "1";
                        
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
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 500);
                        Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

                        resultStr = $"Con2 :{aoitestData.Concentrycity_D}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 450);
                        Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

                        resultStr = $"Dent :{aoitestData.CircleDented}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 400);
                        Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

                        resultStr = $"Gasket :{aoitestData.Gasket}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 350);
                        Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);

                        resultStr = $"Key :{aoitestData.KeyType}";
                        txtPoint = new System.Drawing.Point(100, Globalo.visionManager.milLibrary.CAM_SIZE_Y[topCamIndex] - 300);
                        Globalo.visionManager.milLibrary.DrawOverlayText(topCamIndex, txtPoint, resultStr, Color.GreenYellow, 13);




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
            int nRetStep = 10;
            while (true)
            {
                if (CancelToken.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Side Cam Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                pauseEvent.Wait();  // 일시정지시 여기서 멈춰 있음
                //Tester에서는 일시정지 없을듯
                switch (nRetStep)
                {
                    case 10:
                        //조명 변경
                        nRetStep = 20;
                        break;
                    case 20:
                        //Left Height
                        //Center Height
                        //Right Height

                        //Oring 유무
                        //Cone 유무

                        aoitestData.LH = "0.0";
                        aoitestData.MH = "0.0";
                        aoitestData.RH = "0.0";
                        aoitestData.ORing = "1";
                        aoitestData.Cone = "1";

                        nRetStep = 30;
                        break;
                    case 30:
                        nRetStep = 40;
                        break;
                    case 40:
                        nRetStep = 50;
                        break;
                    case 50:
                        nRetStep = 900;
                        break;
                    case 900:
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
