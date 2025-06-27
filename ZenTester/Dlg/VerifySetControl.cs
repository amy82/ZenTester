using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenTester.Dlg
{
    public partial class VerifySetControl : UserControl
    {
        public VerifySetControl()
        {
            InitializeComponent();
        }

        private void button_FdSet_ConfRead_Click(object sender, EventArgs e)
        {
            //Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName();
            string txtpath = Globalo.FxaBoardManager.fxaEEpromVerify.gettxtFilePath();

            string szLog = $"[VERIFY] TXT PATH : {txtpath}";
            Globalo.LogPrint("ManualControl", szLog);
        }

        private void button_VSet_Crc_Cal_Click(object sender, EventArgs e)
        {

        }

        private void button_VSet_Run_Click(object sender, EventArgs e)
        {
            //D125227T2100059_P1656620-0R-B-SLGM250230D00169_2025040119
            //호출시 : 세미콜론으로 호출
            Globalo.FxaBoardManager.fxaEEpromVerify.RunEEPROMVerifycation("P1656620-0R-B:SLGM250230D00169", "D125227T2100059");


            /*
            // Verification
            EEPRROM_LAST_UPDATED_UTC
            MANUFACTURED_TIMESTAMP_UTC
            IMAGER_UUID
            CHECKSUM_EEPROM_VERSION          - apd0
            CHECKSUM_IMAGER_INFO             - apd1
            CHECKSUM_PART_INFO               - apd2
            CHECKSUM_INTRINSIC_PARAMETER     - apd3
            CHECKSUM_LSC                     - apd4
            CHECKSUM_FLAG_BLOCK
            CHECKSUM_TBD1_BLOCK
            CHECKSUM_TBD2_BLOCK

             */
        }
    }
}
