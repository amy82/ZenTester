using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.TaskClass
{
    public class TaskManager
    {
        private Process.AoiTestFlow aoiTestFlow;
        private Process.VerifyTestFlow verifyTestFlow;

        public bool testRun = false;
        public TaskManager()
        {
            aoiTestFlow = new Process.AoiTestFlow();
        }
        public void Aoi_TestRun(TcpSocket.TesterData data)  //int SocketNum)      //1 or 2
        {
            int nStep = 100;
            string szLog = string.Empty;
            aoiTestFlow.aoitestData.Barcode = data.LotId[0];
            aoiTestFlow.aoitestData.Socket_Num = data.socketNum.ToString();
            Console.WriteLine($"Aoi Task Start SocketNum-------------------------- {aoiTestFlow.aoitestData.Socket_Num}");


            szLog = $"[AOI] TEST START :{aoiTestFlow.aoitestData.Barcode}/{aoiTestFlow.aoitestData.Socket_Num}";
            Globalo.LogPrint("PcbProcess", szLog);

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    nStep = aoiTestFlow.AoiAutoProcess(nStep);
                    if (nStep == 1000)
                    {
                        break;
                    }
                    if (nStep < 0)
                    {
                        break;
                    }
                    await Task.Delay(10);
                }
                testRun = false;
                Console.WriteLine($"Task End - {nStep}");
            });
            
        }
        public void Verify_TestRun(TcpSocket.TesterData data)
        {
            int nStep = 100;
            string szLog = string.Empty;
            verifyTestFlow.verifytestData.Barcode = data.LotId[0];
            verifyTestFlow.verifytestData.Socket_Num = data.socketNum.ToString();
            Console.WriteLine($"Verify Task Start SocketNum-------------------------- {verifyTestFlow.verifytestData.Socket_Num}");


            //szLog = $"[AOI] TEST START SocketNum :{verifyTestFlow.socketNumber}";
            szLog = $"[VERIFY] TEST START :{verifyTestFlow.verifytestData.Barcode}/{verifyTestFlow.verifytestData.Socket_Num}";
            Globalo.LogPrint("PcbProcess", szLog);

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    nStep = verifyTestFlow.VerifyAutoProcess(nStep);

                    if (nStep == 1000){break;}
                    if (nStep < 0){break;}
                    await Task.Delay(10);
                }
                testRun = false;
                Console.WriteLine($"Task End - {nStep}");
            });

        }
    }
}
