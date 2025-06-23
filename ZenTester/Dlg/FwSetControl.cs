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
            Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName();
        }

        private void button_FdSet_Test_Click(object sender, EventArgs e)
        {
            Globalo.FxaBoardManager.fxaFirmwardDw.test111();
        }
    }
}
