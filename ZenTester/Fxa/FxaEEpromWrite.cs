using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.Fxa
{
    public class FxaEEpromWrite
    {
        public const int WM_COPYDATA = 0x004A;
        [DllImport("user32.dll", SetLastError = true)]

        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        //public const string strWriteINIPath = @"D:\EVMS\TP\ENV\fwexe\ThunderEEPROMCreationTool_250526_1111";

        public List<byte> mmdEEpromData;    //Special Data -> Hex Data
        public string defaultCrc;
        public string datFullPath;
        public int recvDataCreate;

        public List<string> checksumDataList;
        public FxaEEpromWrite()
        {
            mmdEEpromData = new List<byte>();
            checksumDataList = new List<string>();
            defaultCrc = string.Empty;
            datFullPath = string.Empty;
            recvDataCreate = -1;
            //ipc 통신으로 넘어오는 부분
            //[10:17:44.2] D:\test\P1656620-0L-B-SLGM250230D00158_20250620_011740.dat@1750349860@1750349860@434209840218070890010D@A7FC@4AEF@01DC@6AE9@E9FE@BEF8@10F0@10F0
        }
        
        public string RunEEPROMWriteCommandAsync(string datFileName)
        {

            //string args = $"/c cd /d D:\\EVMS\\TP\\ENV\\ti_cam_eeprom_flasher && ti_cam_eeprom_flasher.exe {datFileName}";  //OPAL

            //string workingDir = @"D:\EVMS\TP\ENV\lgit_eeprom_flash";        //trinity
            //string exeName = "cam_eeprom_flasher.exe";
            //--------------------------------------------------------------------------------------------------------------
            //
            // trinity
            //
            //
            //--------------------------------------------------------------------------------------------------------------
            string workingDir = @Globalo.yamlManager.configData.TeslaData.Wpath;        //trinity
            string exeName = Globalo.yamlManager.configData.TeslaData.WexeNameTrinity;

            //--------------------------------------------------------------------------------------------------------------
            //
            // Opal
            //
            //
            //--------------------------------------------------------------------------------------------------------------

            exeName = Globalo.yamlManager.configData.TeslaData.WexeNameOpal;
            //string workingDir = @"D:\EVMS\TP\ENV\ti_cam_eeprom_flasher";          //opal
            //string exeName = "ti_cam_eeprom_flasher.exe";

            string args = $"/c cd /d {workingDir} && {exeName} {datFileName}";

            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = new System.Diagnostics.Process { StartInfo = psi })
            {
                process.Start();

                //Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                //Task<string> errorTask = process.StandardError.ReadToEndAsync();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                process.WaitForExit();

                //string output = await outputTask;
                //string error = await errorTask;

                if (!string.IsNullOrWhiteSpace(error))
                {
                    return $"[ERROR]\n{error.Trim()}";
                }

                return $"[SUCCESS]\n{output.Trim()}";
            }
        }
        //RunEEPROMWriteCommandAsync  = eeprom i2c 보내는 것 flash
        public async Task<string> Old__RunEEPROMWriteCommandAsync(string datFileName)
        {

            //string args = $"/c cd /d D:\\EVMS\\TP\\ENV\\ti_cam_eeprom_flasher && ti_cam_eeprom_flasher.exe {datFileName}";  //OPAL

            //D:\EVMS\TP\ENV\fwexe\TeslaEXE\Tesla_EEPROM_exe
            string workingDir = @"D:\EVMS\TP\ENV\lgit_eeprom_flash";        //trinity
            string exeName = "cam_eeprom_flasher.exe";

            //string workingDir = @"D:\EVMS\TP\ENV\ti_cam_eeprom_flasher";          //opal
            //string exeName = "ti_cam_eeprom_flasher.exe";

            string args = $"/c cd /d {workingDir} && {exeName} {datFileName}";

            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = new System.Diagnostics.Process { StartInfo = psi })
            {
                process.Start();

                Task<string> outputTask = process.StandardOutput.ReadToEndAsync();
                Task<string> errorTask = process.StandardError.ReadToEndAsync();

                await Task.WhenAll(outputTask, errorTask);

                process.WaitForExit();

                string output = await outputTask;
                string error = await errorTask;

                if (!string.IsNullOrWhiteSpace(error))
                {
                    return $"[ERROR]\n{error.Trim()}";
                }

                return $"[SUCCESS]\n{output.Trim()}";
            }
        }
        //Dat 파일 만들기
        public void Run_DatCreation_EEPROMWrite(string caseid, string lotid)
        {
            //MES 정보가 담긴 .txt 파일을 .dat 파일로 변환 ThunderEEPROMCreationTool.exe <- 김수현 선임 제공
            //string msg = "P1656620-0L-B:SLGM250230D00158,B825114T1100345,A05S002X"; //P1656620-0L-B-SLGM250230D00158_20250618_053822

            string writeMsg = caseid + ","+ lotid + ",A05S002X";



            //string msg = "P1656620-0R-B:SLGM250230D00169,C825116T1100008,A05S002X";
            System.Diagnostics.Process[] pro = System.Diagnostics.Process.GetProcessesByName("ThunderEEPROMCreationTool");
            //System.Diagnostics.Process[] pro = System.Diagnostics.Process.GetProcessesByName("ThunderEEPROMVerificationTool");
            if (pro.Length > 0)
            {
                byte[] buff = System.Text.Encoding.Unicode.GetBytes(writeMsg);// msg);

                GCHandle gch = GCHandle.Alloc(buff, GCHandleType.Pinned);

                try
                {
                    COPYDATASTRUCT cds = new COPYDATASTRUCT();
                    cds.dwData = IntPtr.Zero;
                    cds.cbData = buff.Length + 2;
                    cds.lpData = gch.AddrOfPinnedObject();

                    SendMessage(pro[0].MainWindowHandle, WM_COPYDATA, IntPtr.Zero, ref cds);
                }
                finally
                {
                    gch.Free();
                }
            }
        }
        public string gettxtFilePath()
        {
            StringBuilder fwFileName = new StringBuilder(256);
            string rtnFwName = string.Empty;

            string sourcePath = Path.Combine(Globalo.yamlManager.configData.TeslaData.Wpath, "Configuration.ini");

            GetPrivateProfileString("CONFIG", "PATH3", "", fwFileName, fwFileName.Capacity, sourcePath);
            //GetPrivateProfileString("DEFAULT", "FIRMWARE_FILE", "", fwFileName, fwFileName.Capacity, sourcePath);
            //D:\EVMS\TP\ENV\fwexe\ThunderEEPROMVerificationTool_250526_1111

            rtnFwName = fwFileName.ToString();
            return rtnFwName;
        }
    }
}
