using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
