using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenTester.Dlg
{
    public partial class FwSetControl : UserControl
    {
        public bool bManualFwRun = false;
        public FwSetControl()
        {
            InitializeComponent();
        }

        private void button_FdSet_ConfRead_Click(object sender, EventArgs e)
        {
            string fwStr = "";
            fwStr = Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName("FIRMWARE_FILE");
            fwStr = Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName("FIRMWARE_VERSION");
        }

        private async void button_FdSet_Test_Click(object sender, EventArgs e)
        {
            if (bManualFwRun)
            {
                Console.WriteLine("Manual FirmwareDw Run...");
                return;
            }
            bManualFwRun = true;
            Globalo.LogPrint("fw :", "Manual FirmwareDw Start");

            await Task.Run(async () =>
            {
                int i = 0;
                string strLog = string.Empty;
                string[] lotarr = { "CAM1_P1637042-00-C-SLGM250434C00283", "-1", "-2", "CAM4_P1637042-00-C-SLGM250434C00283" };
                string result = Globalo.FxaBoardManager.fxaFirmwardDw.FirmwareDownLoadForCamAsync(lotarr); //CAM1 = 포트 그 뒤엔 BCR ":" -> "-" 으로 변경해서 넣어야 함 save파일명


                

                //if (result == "-1")
                //{
                //    Globalo.LogPrint("fw :", "Firmware Download Fail");
                //    return;
                //}
                strLog = $"Firmware DownLoad ForCamAsync:{result}";
                Globalo.LogPrint("fw :", strLog);

                string[] receivedParse = result.Split(',');     //총 13개
                Globalo.LogPrint("fw :", strLog);//, Globalo.eMessageName.M_INFO);

                string[] rtnBcrArr = { "", "", "", "" };
                string[] rtnFinalArr = { "", "", "", "" };
                

                if (receivedParse.Length >= 13)
                {
                    rtnBcrArr[0] = receivedParse[2];
                    rtnBcrArr[1] = receivedParse[5];
                    rtnBcrArr[2] = receivedParse[8];
                    rtnBcrArr[3] = receivedParse[11];
                    //
                    rtnFinalArr[0] = receivedParse[3];
                    rtnFinalArr[1] = receivedParse[6];
                    rtnFinalArr[2] = receivedParse[9];
                    rtnFinalArr[3] = receivedParse[12];
                }
                Thread.Sleep(300);
                for (i = 0; i < rtnBcrArr.Length; i++)
                {
                    if (int.Parse(rtnFinalArr[i]) == 0)
                    {
                        //ok
                        strLog = $"Cam{i+1} Firmware Download ok -{rtnFinalArr[i]}";

                        int nResult = int.Parse(rtnFinalArr[i]);
                        Globalo.FxaBoardManager.fxaFirmwardDw.getfwResultFromJson(lotarr[i], nResult);
                    }
                    else
                    {
                        //fail
                        strLog = $"Cam{i + 1} Firmware Download fail -{rtnFinalArr[i]}";
                    }

                    
                    Globalo.LogPrint("fw :", strLog);
                }

                //Globalo.FxaBoardManager.fxaFirmwardDw.Manual_Fw_DownLoad();
                bManualFwRun = false;
                await Task.Delay(10);
            });
            
            
        }

        private void button_FdSet_JsonRead_Click(object sender, EventArgs e)
        {
            //CAM다음 숫자는 소켓 번호일수도...
            string tempLot = "CAM1_P1637042-00-C-SLGM250434C00283";
            string heart = Globalo.FxaBoardManager.fxaFirmwardDw.getHeater_Current(tempLot, "1");

            string strlog = string.Empty;

            strlog = $"getHeater_Current:{heart}";

            Globalo.LogPrint("fxaEEpromWrite", strlog); 
        }

        private void button_FdSet_Read_Click(object sender, EventArgs e)
        {
            int i = 0;
            string[] _version = new string[4];
            string[] _sensorId = new string[4];

            Globalo.FxaBoardManager.fxaFirmwardDw.ReadFirmware(ref _version, ref _sensorId);
            for (i = 0; i < _version.Length; i++)
            {
                string logMsg = $"[version]{_version[i]}";
                Globalo.LogPrint("fw", logMsg);
            }

            for (i = 0; i < _sensorId.Length; i++)
            {
                string logMsg = $"[sensorid]{_sensorId[i]}";
                Globalo.LogPrint("fw", logMsg);
            }
        }
    }
}
