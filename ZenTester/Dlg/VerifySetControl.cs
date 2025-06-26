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
            Globalo.FxaBoardManager.fxaEEpromVerify.RunEEPROMVerifycation("P1656620-0L-B:SLGM250230D00158", "B825114T1100345");
        }
    }
}
