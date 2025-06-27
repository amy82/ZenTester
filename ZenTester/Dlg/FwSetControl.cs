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
    public partial class FwSetControl : UserControl
    {
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

        private void button_FdSet_Test_Click(object sender, EventArgs e)
        {
            Globalo.FxaBoardManager.fxaFirmwardDw.test111();
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

    }
}
