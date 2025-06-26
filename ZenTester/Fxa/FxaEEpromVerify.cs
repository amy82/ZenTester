using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.Fxa
{
    public class FxaEEpromVerify
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        //public const string strVerifyINIPath = @"D:\EVMS\TP\ENV\fwexe\TeslaEXE\Tesla_FW_exe\ThunderEEPROMVerificationTool_250526_1111";


        public const string strVerifyINIPath = @"D:\EVMS\TP\ENV\fwexe\ThunderEEPROMVerificationTool_250526_1111";

        //D:\EVMS\TP\ENV\fwexe\ThunderEEPROMVerificationTool_250526_1111
        public const int WM_COPYDATA = 0x004A;

        [DllImport("user32.dll", SetLastError = true)]

        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref COPYDATASTRUCT lParam);
        public List<byte> mmdEEpromData;    //Special Data -> Hex Data
        public string defaultCrc;

        public FxaEEpromVerify()
        {
            mmdEEpromData = new List<byte>();
            defaultCrc = string.Empty;
        }


        public void RunEEPROMVerifycation(string caseId , string lotid , string temp = "A05S002X")
        {
            //string msg = "P1656620-0L-B:SLGM250230D00158,B825114T1100345,A05S002X"; //P1656620-0L-B-SLGM250230D00158_20250618_053822 C342090304180708DA000F
            //msg = 바코드 , 섹젬에서 받은 LOTID , 뒤에는 아무거나

            string verifyMsg = caseId + "," + lotid + "," + temp;

            //string msg = "P1656620-0R-B:SLGM250230D00169,C825116T1100008,A05S002X";
            //System.Diagnostics.Process[] pro = System.Diagnostics.Process.GetProcessesByName("ThunderEEPROMCreationTool");


            System.Diagnostics.Process[] pro = System.Diagnostics.Process.GetProcessesByName("ThunderEEPROMVerificationTool");
            if (pro.Length > 0)
            {
                byte[] buff = System.Text.Encoding.Unicode.GetBytes(verifyMsg);

                GCHandle gch = GCHandle.Alloc(buff, GCHandleType.Pinned);

                try
                {
                    COPYDATASTRUCT cds = new COPYDATASTRUCT();
                    cds.dwData = IntPtr.Zero;
                    cds.cbData = buff.Length + 2;
                    cds.lpData = gch.AddrOfPinnedObject();      //Barcode,LotID, StationID

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

            string sourcePath = Path.Combine(strVerifyINIPath, "Configuration.ini");

            GetPrivateProfileString("CONFIG", "PATH3", "", fwFileName, fwFileName.Capacity, sourcePath);
            //GetPrivateProfileString("DEFAULT", "FIRMWARE_FILE", "", fwFileName, fwFileName.Capacity, sourcePath);
            //D:\EVMS\TP\ENV\fwexe\ThunderEEPROMVerificationTool_250526_1111

            rtnFwName = fwFileName.ToString();
            return rtnFwName;
        }

    }
}
