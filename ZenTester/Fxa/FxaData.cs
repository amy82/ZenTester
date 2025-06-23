using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.Fxa
{
    public struct Parameter
    {
        public string name { get; set; }
        public object value { get; set; }
        public string description { get; set; }
        public string result { get; set; }
        public string type { get; set; }
        public double? lowerlimit { get; set; }
        public double? upperlimit { get; set; }
        public string expectedvalue { get; set; }
    }
    public struct TestResult
    {
        public List<Parameter> parameters { get; set; }
        public List<string> errors { get; set; }
        public string thingname { get; set; }
        public string taskname { get; set; }
        public string actorname { get; set; }
        public string tasktimestamp { get; set; }
        public string actorversion { get; set; }
        public string actorlocation { get; set; }
        public string taskversion { get; set; }
        public string sitecode { get; set; }
        public string taskcollectionname { get; set; }
        public string partnumber { get; set; }
    }
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        public IntPtr lpData;
    }
    public static class CpNameList
    {
        //public static readonly string[] CpNames { get; } = new string[]
        private static readonly string[] _cpNames = new string[]
        {
            "EEPROM_VERSION_MAJOR",
            "EEPROM_VERSION_MINOR",
            "EEPROM_LAST_UPDATED_ENTITY",
            "IMAGER_NAME",
            "Imager exact Color Filter Array",
            "Imager input clock frequency",
            "CAMERA_LOCATION_AT_VEHICLE_LEVEL_MAJOR",
            "CAMERA_LOCATION_AT_VEHICLE_LEVEL_MINOR",
            "MANUFACTURER",
            "MANUFACTURER_PART_NUMBER",
            "TESLA_PART_NUMBER",
            "MANUFACTURED_LOCATION",
            "MANUFACTURED_ASY_LOCATION",
            "LENS_MANUFACTURER",
            "LENS_PART_NUMBER",
            "LENS_APERTURE",
            "MODULE_ORIENTATION_ADJUSTMENT",
            "MANUFACTURER_INTERNAL_VERSION_CONTROL",
            "SERIALIZER_TYPE",
            "DIST_VERSION",
            "LSC_MAP_B1_Bb_META_version",
            "LSC_MAP_B1_Gb_META_version",
            "LSC_MAP_B1_Gr_META_version",
            "LSC_MAP_B1_Rr_META_version",
            "LSC_MAP_B2_Bb_META_version",
            "LSC_MAP_B2_Gb_META_version",
            "LSC_MAP_B2_Gr_META_version",
            "LSC_MAP_B2_Rr_META_version",
            "LSC_MAP_D65_Bb_META_version",
            "LSC_MAP_D65_Gb_META_version",
            "LSC_MAP_D65_Gr_META_version",
            "LSC_MAP_D65_Rr_META_version",
            "LSC_MAP_G1_Bb_META_version",
            "LSC_MAP_G1_Gb_META_version",
            "LSC_MAP_G1_Gr_META_version",
            "LSC_MAP_G1_Rr_META_version",
            "LSC_MAP_G2_Bb_META_version",
            "LSC_MAP_G2_Gb_META_version",
            "LSC_MAP_G2_Gr_META_version",
            "LSC_MAP_G2_Rr_META_version",
            "LSC_MAP_R1_Bb_META_version",
            "LSC_MAP_R1_Gb_META_version",
            "LSC_MAP_R1_Gr_META_version",
            "LSC_MAP_R1_Rr_META_version",
            "LSC_MAP_R2_Bb_META_version",
            "LSC_MAP_R2_Gb_META_version",
            "LSC_MAP_R2_Gr_META_version",
            "LSC_MAP_R2_Rr_META_version",
            "PCBA_PN",
            "PCBA_MFG",
            "IRCF_PN",
            "IRCF_MFG",
            "FLAG05_MCU",
            "FLAG06_Heater"
        };
    }

}
