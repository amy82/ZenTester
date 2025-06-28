using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.Data
{

    public class Resolution
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    public class _CamSettings
    {
        public Resolution TopResolution { get; set; }
        public Resolution SideResolution { get; set; }

        public int KeyEdgeSpecCount { get; set; }
        public double DentLimit { get; set; }
        public int DentTotalCount { get; set; }
    }
    public class _SerialPort
    {
        public string Light { get; set; }
    }
    public class _TeslaData     //fw,eeprom exe 폴더 경로
    {
        public string Fpath { get; set; }
        public string FexeName { get; set; }
        public string Wpath { get; set; }
        public string WexeNameKim { get; set; }
        public string WexeNameTrinity { get; set; }
        public string WexeNameOpal { get; set; }
        public string Vpath { get; set; }
        public string VexeName { get; set; }
    }
    public class _DrivingSettings
    {
        public bool IdleReportPass { get; set; }
        public bool EnableAutoStartBcr { get; set; }
        public bool PinCountUse { get; set; }
        public int PinCountMax { get; set; }
        public int CsvScanMonth { get; set; }
        public bool ImageGrabUse { get; set; }
        public string Language { get; set; }

        public string HandlerIp { get; set; }
        public int HandlerPort { get; set; }
        public string SecsgemIp { get; set; }
        public int SecsgemPort { get; set; }

    }
    public class ConfigData
    {
        public int MachineId { get; set; }
        public _TeslaData TeslaData { get; set; }
        public _SerialPort SerialPort { get; set; }
        public _DrivingSettings DrivingSettings { get; set; }
        public _CamSettings CamSettings { get; set; }
    }
}
