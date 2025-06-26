using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public FxaEEpromWrite()
        {
            //ipc 통신으로 넘어오는 부분
            //[10:17:44.2] D:\test\P1656620-0L-B-SLGM250230D00158_20250620_011740.dat@1750349860@1750349860@434209840218070890010D@A7FC@4AEF@01DC@6AE9@E9FE@BEF8@10F0@10F0
        }
        public async void test111()
        {
            //EEPROM Write I2C Flash
            string datfilename = "P1656620-0L-B-SLGM250230D00158_20250620_072939"; //.dat 파일명 바코드 뒤에 생성 시간까지 포함 시켜야함 
            //string datfilename = "P1656620-0R-B-SLGM250230D00169_20250619_051049"; //.dat 파일명 바코드 뒤에 생성 시간까지 포함 시켜야함 

            string result = await RunEEPROMWriteCommandAsync(datfilename);

            if (result.StartsWith("[ERROR]"))
            {
                string errorDetail = result.Replace("[ERROR]", "").Trim();  // 에러 메시지 원문 추출

                Globalo.LogPrint("EEPROM I2C Write Flash 실패", errorDetail, Globalo.eMessageName.M_ERROR);

                // → 필요 시: 에러 유형별 분기
                if (errorDetail.Contains("Can't open config"))
                    Globalo.LogPrint("원인", "flash_conf.ini 접근 실패", Globalo.eMessageName.M_WARNING);
                else if (errorDetail.Contains("I2C"))
                    Globalo.LogPrint("원인", "I2C 통신 오류", Globalo.eMessageName.M_WARNING);
            }
            else
            {
                string successLog = result.Replace("[SUCCESS]", "").Trim();
                Globalo.LogPrint("EEPROM I2C Write Flash 성공", successLog, Globalo.eMessageName.M_INFO);
            }
        }

        //RunEEPROMWriteCommandAsync  = eeprom i2c 보내는 것 flash
        public async Task<string> RunEEPROMWriteCommandAsync(string datFileName)
        {

            //string args = $"/c cd /d D:\\EVMS\\TP\\ENV\\ti_cam_eeprom_flasher && ti_cam_eeprom_flasher.exe {datFileName}";  //OPAL

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
        public void RunEEPROMWriteDatCreation()
        {
            //MES 정보가 담긴 .txt 파일을 .dat 파일로 변환 ThunderEEPROMCreationTool.exe <- 김수현 선임 제공
            string msg = "P1656620-0L-B:SLGM250230D00158,B825114T1100345,A05S002X"; //P1656620-0L-B-SLGM250230D00158_20250618_053822
            //string msg = "P1656620-0R-B:SLGM250230D00169,C825116T1100008,A05S002X";
            System.Diagnostics.Process[] pro = System.Diagnostics.Process.GetProcessesByName("ThunderEEPROMCreationTool");
            //System.Diagnostics.Process[] pro = System.Diagnostics.Process.GetProcessesByName("ThunderEEPROMVerificationTool");
            if (pro.Length > 0)
            {
                byte[] buff = System.Text.Encoding.Unicode.GetBytes(msg);

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
    }
}
