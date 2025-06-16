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
        public bool testRun = false;
        public TaskManager()
        {
            aoiTestFlow = new Process.AoiTestFlow();
        }
        public void Aoi_TestRun(int SocketNum)      //1 or 2
        {
            int nStep = 100;
            string szLog = string.Empty;
            aoiTestFlow.socketNumber = SocketNum;
            Console.WriteLine($"Task Start SocketNum-------------------------- {SocketNum}");


            szLog = $"[AOI] TEST START SocketNum :{aoiTestFlow.socketNumber}";
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
        public void Aoi_TopCam_TaskRun(int taskStep)
        {
            int nStep = 100;
            Console.WriteLine($"Task Start -------------------------- {taskStep}");
            _ = Task.Run(async () =>
            {
                
                while (true)
                {
                    switch (nStep)
                    {
                        case 10:
                            nStep = 20;
                            break;
                        case 20:
                            nStep = 30;
                            break;
                        case 30:
                            nStep = 40;
                            break;
                        case 40:
                            nStep = 1000;
                            break;
                        default:

                            break;
                    }

                    if (nStep == 1000)
                    {
                        Console.WriteLine($"Task Complete - {nStep}");
                        break;
                    }
                    if (nStep < 0)
                    {
                        Console.WriteLine($"Task Err - {nStep}");
                        break;
                    }
                    await Task.Delay(50);
                }
                Console.WriteLine($"Task End - {nStep}");
            });
        }
    }
}
