using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenTester.Dlg
{
    public partial class WriteSetControl : UserControl
    {
        public const int WM_COPYDATA = 0x004A;

        [DllImport("user32.dll", SetLastError = true)]

        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref Fxa.COPYDATASTRUCT lParam);
        public WriteSetControl()
        {
            InitializeComponent();
        }

        private void button_FdSet_ConfRead_Click(object sender, EventArgs e)
        {
            //Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName();
        }

        private void button_WSet_Crc_Cal_Click(object sender, EventArgs e)
        {

        }

        private async void button_WSet_Run_Click(object sender, EventArgs e)
        {
            //1.Dat만들기
            Globalo.FxaBoardManager.fxaEEpromWrite.RunEEPROMWriteDatCreation("P1656620-0L-B:SLGM250230D00158", "B825114T1100345");


            //2.write 진행
            //EEPROM Write I2C Flash
            string datfilename = "P1656620-0L-B-SLGM250230D00158_20250620_072939"; //.dat 파일명 바코드 뒤에 생성 시간까지 포함 시켜야함 
            //string datfilename = "P1656620-0R-B-SLGM250230D00169_20250619_051049"; //.dat 파일명 바코드 뒤에 생성 시간까지 포함 시켜야함 

            string result = await Globalo.FxaBoardManager.fxaEEpromWrite.RunEEPROMWriteCommandAsync(datfilename);

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

        private async void button1_Click(object sender, EventArgs e)            //lim Write 버튼
        {
            //EEPROM Write I2C Flash
            string datfilename = "P1656620-0L-B-SLGM250230D00158_20250620_072939"; //.dat 파일명 바코드 뒤에 생성 시간까지 포함 시켜야함 
            //string datfilename = "P1656620-0R-B-SLGM250230D00169_20250619_051049"; //.dat 파일명 바코드 뒤에 생성 시간까지 포함 시켜야함 

            string result = await Globalo.FxaBoardManager.fxaEEpromWrite.RunEEPROMWriteCommandAsync(datfilename);

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
        
    }
}
