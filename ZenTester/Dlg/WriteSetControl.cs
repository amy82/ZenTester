using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenTester.Dlg
{
    public partial class WriteSetControl : UserControl
    {
        public const int WM_COPYDATA = 0x004A;

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref Fxa.COPYDATASTRUCT lParam);
        public bool bManualWriteRun = false;
        public WriteSetControl()
        {
            InitializeComponent();
        }

        private void button_FdSet_ConfRead_Click(object sender, EventArgs e)
        {
            //Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName();
            string txtpath = Globalo.FxaBoardManager.fxaEEpromWrite.gettxtFilePath();

            string szLog = $"[WRITE] PATH3 PATH : {txtpath}";
            Globalo.LogPrint("ManualControl", szLog);


            //test
            string _date = DateTime.Now.ToString("yyyyMMddHH");
            string filename = $"{"D125227T2100059"}_{"P1656620-0L-B-SLGM250230D00159"}_{_date}_EEPROM-MES.txt";
            string folder = Globalo.FxaBoardManager.fxaEEpromWrite.gettxtFilePath();
            string txtFilePath = Path.Combine(folder, filename);
        }

        private void button_WSet_Crc_Cal_Click(object sender, EventArgs e)
        {

        }

        private async void button_WSet_Run_Click(object sender, EventArgs e)
        {
            //스페셜 DATA를 TXT파일로 만들어서 PATH3= 에 저장해야된다.
            //[D125227T2100059_P1656620-0L-B-SLGM250230D00158_2025040119_EEPROM-MES.txt]
            //
            //PATH3=  이경로에서 TXT파일을 갖고온다.
            //DAT를 만들기하면
            //SAVE_PATH=D:\test = 이경로에 DAT파일이 생성된다.

            if (bManualWriteRun)
            {
                Console.WriteLine("Manual Write Run...");
                return;
            }
            //_ = Task.Run(async () =>
            await Task.Run(async () =>
            {

                bManualWriteRun = true;
                //---------------------------------------------------------------------------------------------------------------------------
                //
                //
                //
                //  1.Dat만들기
                //  Run_DatCreation_EEPROMWrite  <---에서 결과 바로 리턴
                //
                //
                //---------------------------------------------------------------------------------------------------------------------------
                //D125227T2100059_P1656620-0R-B-SLGM250230D00169_2025040119_EEPROM-MES
                //Globalo.FxaBoardManager.fxaEEpromWrite.Run_DatCreation_EEPROMWrite("P1656620-0L-B:SLGM250230D00159", "D125227T2100059");

                Globalo.FxaBoardManager.fxaEEpromWrite.Run_DatCreation_EEPROMWrite("P1656620-0R-B:SLGM250230D00169", "D125227T2100059");
                Globalo.FxaBoardManager.fxaEEpromWrite.recvDataCreate = -1;
                int nWriteTimeTick = 0;
                nWriteTimeTick = Environment.TickCount;
                while (true)
                {
                    if (Globalo.FxaBoardManager.fxaEEpromWrite.recvDataCreate == 0)
                    {
                        break;
                    }
                    if (Environment.TickCount - nWriteTimeTick > 30000)
                    {
                        Globalo.LogPrint("fxaEEpromWrite", "Dat File Create Timeout 30s.");
                        break;
                    }
                }

                if (Globalo.FxaBoardManager.fxaEEpromWrite.recvDataCreate == -1)
                {
                    bManualWriteRun = false;
                    Globalo.LogPrint("fxaEEpromWrite", "Dat File Create Fail.");
                    //Dat 파일 생성 실패
                    return;
                }
                Thread.Sleep(300);
                //---------------------------------------------------------------------------------------------------------------------------
                //
                //
                //
                //  2.생성된 Dat파일로 Write 진행
                //RunEEPROMWriteCommandAsync  <---에서 결과 바로 리턴
                //
                //
                //---------------------------------------------------------------------------------------------------------------------------
                //2.write 진행
                //cam_eeprom_flasher.exe 같은 폴더에 있는 flash_conf.ini 파일안에
                // dat_path = D:\lgit_eeprom\dat_files   이경로에 dat가 생성이 돼야된다.
                //ThunderEEPROMCreationTool.exe 같은 폴더에 잇는 Configuration.ini 파일안에
                //SAVE_PATH=D:\lgit_eeprom\dat_files  하고 맞혀야 될듯?
                //EEPROM Write I2C Flash
                // 파일 이름만 가져오고 확장자는 제거

                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Globalo.FxaBoardManager.fxaEEpromWrite.datFullPath);

                string datfilename = fileNameWithoutExtension;/// "P1656620-0L-B-SLGM250230D00159_20250627_013133"; //.dat 파일명 바코드 뒤에 생성 시간까지 포함 시켜야함 
                //string datfilename = "P1656620-0R-B-SLGM250230D00169_20250619_051049"; //.dat 파일명 바코드 뒤에 생성 시간까지 포함 시켜야함 

                //string result = await Globalo.FxaBoardManager.fxaEEpromWrite.RunEEPROMWriteCommandAsync(datfilename);
                string result = Globalo.FxaBoardManager.fxaEEpromWrite.RunEEPROMWriteCommandAsync(datfilename);
                //"[SUCCESS]\nCamera EEPROM flash: PASS"
                // 대소문자 구분 없이 포함 여부 확인


                bool hasSuccess = result.Contains("SUCCESS");
                bool hasPass = result.Contains("PASS");
                if (hasSuccess || hasPass)
                {
                    //EEPROM I2C Write Flash 성공
                    //string successLog = result.Replace("[SUCCESS]", "").Trim();
                    Globalo.LogPrint("Write Flash", result);//, Globalo.eMessageName.M_INFO);
                }
                else
                {
                    string errorDetail = result.Replace("[ERROR]", "").Trim();  // 에러 메시지 원문 추출

                    Globalo.LogPrint("EEPROM I2C Write Flash 실패", errorDetail);//, Globalo.eMessageName.M_ERROR);

                    // → 필요 시: 에러 유형별 분기

                    if (errorDetail.Contains("Can't open config"))
                    {
                        Globalo.LogPrint("fxaEEpromWrite", "flash_conf.ini 접근 실패");//, Globalo.eMessageName.M_WARNING);
                    }
                    else if (errorDetail.Contains("I2C"))
                    {
                        Globalo.LogPrint("fxaEEpromWrite", "I2C 통신 오류");//, Globalo.eMessageName.M_WARNING);
                    }
                }
                //if (result.StartsWith("[ERROR]"))

                //---------------------------------------------------------------------------------------------------------------------------
                //
                //
                //
                //
                //  3.dat 확장자를 txt로 만들기
                //생성된 dat파일을 그위치에서 txt로 확장자 바꿔준다.
                //
                //
                //
                //---------------------------------------------------------------------------------------------------------------------------
                if (File.Exists(Globalo.FxaBoardManager.fxaEEpromWrite.datFullPath))
                {
                    string newPath = Path.ChangeExtension(Globalo.FxaBoardManager.fxaEEpromWrite.datFullPath, ".txt");
                    if (File.Exists(Globalo.FxaBoardManager.fxaEEpromWrite.datFullPath))
                    {
                        File.Move(Globalo.FxaBoardManager.fxaEEpromWrite.datFullPath, newPath); // 실제로 이름 변경 (확장자 변경)
                    }
                }
                else
                {
                    Console.WriteLine($"{Globalo.FxaBoardManager.fxaEEpromWrite.datFullPath} 파일이 존재하지 않습니다.");
                }
                // 확장자를 .txt로 바꾸기

                await Task.Delay(10);
            });
            Console.WriteLine("Manual Write End...");
            bManualWriteRun = false;
        }

        

        private void button_WSet_Dat_Create_Click(object sender, EventArgs e)
        {
            //스페셜 DATA를 TXT파일로 만들어서 PATH3= 에 저장해야된다.
            //[D125227T2100059_P1656620-0L-B-SLGM250230D00158_2025040119_EEPROM-MES.txt]
            //
            //PATH3=  이경로에서 TXT파일을 갖고온다.
            //DAT를 만들기하면
            //SAVE_PATH=D:\test = 이경로에 DAT파일이 생성된다.
            //생성된 dat 풀경로는 WndProc 이쪽으로 들어온다. Globalo.FxaBoardManager.fxaEEpromWrite.datFullPath

            //1.Dat만들기
            Globalo.FxaBoardManager.fxaEEpromWrite.Run_DatCreation_EEPROMWrite("P1656620-0L-B:SLGM250230D00159", "D125227T2100059");
        }

        private void button_WSet_Path_Set_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "실행 파일을 선택하세요";
                ofd.Filter = "실행 파일 (*.exe)|*.exe"; //  exe만 표시
                ofd.CheckFileExists = true;             //  실제 존재하는 파일만 선택 가능
                ofd.Multiselect = false;                // 다중 선택 금지 (원하면 true로 변경)
                ofd.InitialDirectory = @button_WSet_Path_Val.Text;      // "D:\EVMS\TP\ENV"; //  초기 폴더 지정


                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = ofd.FileName;
                    string selectedFolderPath = Path.GetDirectoryName(selectedFilePath);

                    string teslaExeName1 = "ThunderEEPROMCreationTool.exe";       //Write Trinity 공정
                    string teslaExeName2 = "cam_eeprom_flasher.exe";              //Write Trinity 공정
                    string teslaExeName3 = "ti_cam_eeprom_flasher.exe";           //Write Opal 공정

                    string targetFile = Path.Combine(selectedFolderPath, teslaExeName1);  // 찾을 파일 이름

                    if (File.Exists(targetFile))
                    {
                        //button_WSet_Exe_Val1.Text = selectedFolderPath;
                        Console.WriteLine($"Verify exe File exists: {targetFile}");

                        Globalo.yamlManager.configData.TeslaData.Wpath = selectedFolderPath;
                        Globalo.yamlManager.configData.TeslaData.WexeNameKim = teslaExeName1;
                        Globalo.yamlManager.configData.TeslaData.WexeNameTrinity = teslaExeName2;
                        Globalo.yamlManager.configData.TeslaData.WexeNameOpal = teslaExeName3;

                        Globalo.yamlManager.configDataSave();
                    }
                    else
                    {
                        Console.WriteLine($"Verify exe No file: {targetFile}");
                    }

                    ///MessageBox.Show("선택한 exe 경로:\n" + selectedFilePath +"\n\nexe가 있는 폴더:\n" + selectedFolderPath);
                }
            }
        }
    }
}
