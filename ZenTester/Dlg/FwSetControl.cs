using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

            button_FdSet_Path_Val.Text = Globalo.yamlManager.configData.TeslaData.Fpath;
            button_FdSet_Exe_Val.Text = Globalo.yamlManager.configData.TeslaData.FexeName;
        }
        public void SetData()
        {

        }
        private void button_FdSet_ConfRead_Click(object sender, EventArgs e)
        {
            string fwFileStr = "";
            string fwVerStr = "";
            fwFileStr = Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName("FIRMWARE_FILE");
            fwVerStr = Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName("FIRMWARE_VERSION");

            string szLog = "";


            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fwFileStr);



            szLog = $"[conf]FIRMWARE_FILE:{fileNameWithoutExtension}";
            Globalo.LogPrint("fw :", szLog);
            szLog = $"[conf]FIRMWARE_VERSION:{fwVerStr}";
            Globalo.LogPrint("fw :", szLog);
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

        private void button_FdSet_Path_Set_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "실행 파일을 선택하세요";
                ofd.Filter = "실행 파일 (*.exe)|*.exe"; // ✅ exe만 표시
                ofd.CheckFileExists = true;             // ✅ 실제 존재하는 파일만 선택 가능
                ofd.Multiselect = false;                // 다중 선택 금지 (원하면 true로 변경)
                ofd.InitialDirectory = @button_FdSet_Path_Val.Text;// "D:\EVMS\TP\ENV"; // ✅ 초기 폴더 지정


                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = ofd.FileName;
                    string selectedFolderPath = Path.GetDirectoryName(selectedFilePath);

                    string teslaExeName = string.Empty;

                    
                    if (Program.TEST_PG_SELECT == TESTER_PG.FW)
                    {
                        teslaExeName = "cypress_cam_flashing.exe";
                    }
                    else if (Program.TEST_PG_SELECT == TESTER_PG.EEPROM_WRITE)
                    {
                        teslaExeName = "ThunderEEPROMCreationTool.exe";
                    }
                    else if (Program.TEST_PG_SELECT == TESTER_PG.EEPROM_VERIFY)
                    {
                        teslaExeName = "ThunderEEPROMVerificationTool.exe";
                    }
                    string targetFile = Path.Combine(selectedFolderPath, teslaExeName);  // 찾을 파일 이름

                    if (File.Exists(targetFile))
                    {
                        button_FdSet_Path_Val.Text = selectedFolderPath;
                        Console.WriteLine($"파일 존재: {targetFile}");

                        Globalo.yamlManager.configData.TeslaData.Fpath = selectedFolderPath;
                        Globalo.yamlManager.configData.TeslaData.FexeName = teslaExeName;

                        Globalo.yamlManager.configDataSave();
                    }
                    else
                    {
                        Console.WriteLine($"파일 없음: {targetFile}");
                    }
                    ///MessageBox.Show("선택한 exe 경로:\n" + selectedFilePath +"\n\nexe가 있는 폴더:\n" + selectedFolderPath);
                }
            }
        }
    }
}
