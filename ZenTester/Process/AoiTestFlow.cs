using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            switch (nStep)
            {
                case 100:
                    TopCamTask = null;
                    SideCamTask = null;
                    CancelToken?.Dispose();
                    CancelToken = new CancellationTokenSource();
                    aoitestData.init();
                    nRetStep = 120;
                    break;
                case 120:
                    //TOP 최초 조명 변경
                    //SIDE 최초 조명 변경
                    nRetStep = 140;
                    break;
                case 140:
                    //TOP 조명 변경 확인?
                    //SIDE 조명 변경 확인?

                    nRetStep = 160;
                    break;
                case 160:
                    waitTopCam = -1;
                    waitSideCam = -1;

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

                    //if (TopCamTask == null || TopCamTask.IsCompleted)

                    nRetStep = 200;
                    nTimeTick = Environment.TickCount;
                    break;
                case 200:
                    //Top, Side 둘다 검사 대기
                    if (waitTopCam == 1 || waitSideCam == 1)
                    {
                        if (Environment.TickCount - nTimeTick > 60000)
                        {
                            //타임아웃 추가?
                            Console.WriteLine("Timeout - {waitTopCam},{waitSideCam}");
                            nRetStep = -1;
                            break;
                        }
                        break;
                    }

                    //Top, Side 둘다 검사 완료
                    nRetStep = 300;
                    break;

                case 300:
                    //Apd 보고
                    Http.HttpService.LotApdReport(aoitestData);
                    nRetStep = 900;
                    break;
                case 900:
                    nRetStep = 1900;    //1000이상이면 종료
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
            int nRetStep = 10;
            while (true)
            {
                if (CancelToken.Token.IsCancellationRequested)      //정지시 while 빠져나가는 부분
                {
                    Console.WriteLine("Top Cam Flow cancelled!");
                    nRtn = -1;
                    break;
                }
                pauseEvent.Wait();  // 일시정지시 여기서 멈춰 있음
                //Tester에서는 일시정지 없을듯
                switch (nRetStep)
                {
                    case 10:
                        nRetStep = 20;
                        break;
                    case 900:
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
                        nRetStep = 20;
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
