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
        private Process.WriteTestFlow writeTestFlow;
        private Process.FwTestFlow fwTestFlow;

        public bool testRun = false;
        public TaskManager()
        {
            aoiTestFlow = new Process.AoiTestFlow();
            verifyTestFlow = new Process.VerifyTestFlow();
            writeTestFlow = new Process.WriteTestFlow();
            fwTestFlow = new Process.FwTestFlow();

        }
        public void Aoi_TestRun(TcpSocket.TesterData data)  //int SocketNum)      //1 or 2
        {
            int nStep = 100;
            string szLog = string.Empty;
            aoiTestFlow.aoitestData.init();     //AOI 결과값 초기화
            aoiTestFlow.aoitestData.Barcode = data.LotId[0];
            aoiTestFlow.aoitestData.Socket_Num = data.socketNum.ToString();     //1,2,3,4 들어올 듯

            Console.WriteLine($"Aoi Task Start SocketNum------------- {aoiTestFlow.aoitestData.Socket_Num}");

            szLog = $"[AOI] TEST START :{aoiTestFlow.aoitestData.Barcode}/{aoiTestFlow.aoitestData.Socket_Num}";
            Globalo.LogPrint("TaskManager", szLog);

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    nStep = aoiTestFlow.AoiAutoProcess(nStep);
                    //
                    if (nStep == 1000) { break; }
                    if (nStep < 0) { break; }
                    await Task.Delay(10);
                }
                testRun = false;
                Console.WriteLine($"TaskManager End - {nStep}");
            });
            
        }
        public void Verify_TestRun(TcpSocket.TesterData data)
        {
            int nStep = 100;
            string szLog = string.Empty;
            verifyTestFlow.verifytestData.init();
            verifyTestFlow.verifytestData.Barcode = data.LotId[0];
            verifyTestFlow.verifytestData.Socket_Num = data.socketNum.ToString();   //1,2,3,4 / 5,6,7,8  다 들어올듯

            //verify는 착공을 secsgem으로 바로 걸기 때문에 special Data는 secsgem으로부터 받아야된다.

            Console.WriteLine($"Verify Task Start SocketNum-------{verifyTestFlow.verifytestData.Socket_Num}");
            
            szLog = $"[VERIFY] TEST START :{verifyTestFlow.verifytestData.Barcode}/{verifyTestFlow.verifytestData.Socket_Num}";
            Globalo.LogPrint("TaskManager", szLog);

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    nStep = verifyTestFlow.VerifyAutoProcess(nStep);
                    //
                    if (nStep == 1000){break;}
                    if (nStep < 0){break;}
                    await Task.Delay(10);
                }
                testRun = false;
                Console.WriteLine($"Verify TaskManager End - {nStep}");
            });

        }

        public void Write_TestRun(TcpSocket.TesterData data)
        {
            int nStep = 100;
            string szLog = string.Empty;
            writeTestFlow.writetestData.init();
            writeTestFlow.writetestData.Barcode = data.LotId[0];
            writeTestFlow.writetestData.Socket_Num = data.socketNum.ToString();   //1,2,3,4 / 5,6,7,8  다 들어올듯
            //foreach (TcpSocket.EquipmentParameterInfo paramInfo in data.CommandParameter)

            writeTestFlow.CommandParameter = data.CommandParameter.Select(item => item.DeepCopy()).ToList();

            Console.WriteLine($"Write Task Start SocketNum-------{writeTestFlow.writetestData.Socket_Num}");

            szLog = $"[WRITE] TEST START :{writeTestFlow.writetestData.Barcode}/{writeTestFlow.writetestData.Socket_Num}";
            Globalo.LogPrint("TaskManager", szLog);

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    nStep = writeTestFlow.WriteAutoProcess(nStep);
                    //
                    if (nStep == 1000) { break; }
                    if (nStep < 0) { break; }
                    await Task.Delay(10);
                }
                testRun = false;
                Console.WriteLine($"Write TaskManager End - {nStep}");
            });

        }

        public void Fw_TestRun(TcpSocket.TesterData data)
        {
            int nStep = 100;
            string szLog = string.Empty;
            fwTestFlow.fwtestData.init();
            fwTestFlow.fwtestData.Barcode = data.LotId[0];

            fwTestFlow.fwtestData.arrBcr[0] = data.LotId[0];
            fwTestFlow.fwtestData.arrBcr[1] = data.LotId[1];
            fwTestFlow.fwtestData.arrBcr[2] = data.LotId[2];
            fwTestFlow.fwtestData.arrBcr[3] = data.LotId[3];


            fwTestFlow.fwtestData.Socket_Num = data.socketNum.ToString();   //1,2,3,4 / 5,6,7,8  다 들어올듯

            foreach (TcpSocket.EquipmentParameterInfo paramInfo in data.CommandParameter)
            {
                if(paramInfo.Name =="FW_FILENAME")
                {
                    fwTestFlow.serverfwFileName = paramInfo.Value;
                }
            }
                Console.WriteLine($"Fw Task Start SocketNum-------{fwTestFlow.fwtestData.Socket_Num}");

            szLog = $"[FW] TEST START :{fwTestFlow.fwtestData.Barcode}/{fwTestFlow.fwtestData.Socket_Num}";
            Globalo.LogPrint("TaskManager", szLog);
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    nStep = fwTestFlow.FwAutoProcess(nStep);
                    //
                    if (nStep == 1000) { break; }
                    if (nStep < 0) { break; }
                    await Task.Delay(10);
                }
                testRun = false;
                Console.WriteLine($"Fw TaskManager End - {nStep}");
            });

        }
    }
}
