using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Diagnostics;
using Renci.SshNet;
namespace ZenTester.Fxa
{
    public class FxaBoardManager
    {
        public FxaFirmwardDw fxaFirmwardDw;
        public FxaEEpromWrite fxaEEpromWrite;
        public FxaEEpromVerify fxaEEpromVerify;

        public FxaBoardManager()
        {
            fxaFirmwardDw = new FxaFirmwardDw();
            fxaEEpromWrite = new FxaEEpromWrite();
            fxaEEpromVerify = new FxaEEpromVerify();
        }
    }
}
