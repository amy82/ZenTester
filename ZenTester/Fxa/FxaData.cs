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
    public static class CrcClass
    {
        //계산된 결과와 비교
        //<A,17 EEPROM_DATA_CRC16 [CPNAME]>
        //  <A,4 4b16 [CEPVAL]>
        public static readonly string HEX = "HEX";
        public static readonly string ASCII = "ASCII";
        public static readonly string DEC = "DEC";
        public static readonly string FLOAT = "FLOAT";
        public static readonly string DOUBLE = "DOUBLE";
        public static readonly string UNIX_TIME = "UNIX_TIME";

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

        //CRF-16 계산 (CCITT)
        public static ushort ComputeCRC16(byte[] data, ushort polynomial, ushort initialValue, ushort xorOut)
        {
            ushort crc = initialValue;
            foreach (byte b in data)
            {
                crc ^= (ushort)(b << 8);
                for (int i = 0; i < 8; i++)
                {
                    crc = (ushort)((crc & 0x8000) != 0 ? (crc << 1) ^ polynomial : (crc << 1));
                }
            }
            return (ushort)(crc ^ xorOut);
        }
        public static string StringToHex(string Input, string Format, string Order, string FixYn)       //MES 에서 받은 값을 변환
        {
            string RtnString = "";
            int i = 0;

            if (Format == ASCII && FixYn == "Y")
            {
                StringBuilder hex = new StringBuilder();
                byte[] bytes = Encoding.ASCII.GetBytes(Input); // 문자열 → 바이트 배열 변환

                //위에서 N일 경우가 있어서 무조건
                //Y만 들어온다.
                if (Order == "Little")
                {
                    Array.Reverse(bytes);

                }

                for (i = 0; i < bytes.Length; i++) // 뒤에서부터 추가
                {
                    hex.AppendFormat("{0:X2}", bytes[i]);      //Little Endian 변환 코드
                }

                //foreach (char c in Input)
                //{
                //    hex.AppendFormat("{0:X2} ", (byte)c); // 각 문자를 16진수 2자리로 변환
                //}
                RtnString = hex.ToString().Trim();
            }
            else if (Format == FLOAT && FixYn == "Y")
            {
                float floatValue = float.Parse(Input);
                byte[] bytes = BitConverter.GetBytes(floatValue); // float → byte[]
                if (Order == "Little")
                {
                    Array.Reverse(bytes); // 빅엔디안으로 변환 (네트워크 전송 시 필요)
                }

                RtnString = BitConverter.ToString(bytes).Replace("-", "");
            }
            else if (Format == DOUBLE && FixYn == "Y")
            {
                double doubleValue = double.Parse(Input);
                byte[] bytes = BitConverter.GetBytes(doubleValue); // double → byte[]
                if (Order == "Little")
                {
                    Array.Reverse(bytes); // 빅엔디안 변환
                }

                RtnString = BitConverter.ToString(bytes).Replace("-", "");
            }
            else// (Format == HEX || Format == EMPTY || Format ==  || FixYn == "N")      //N이면 무조건 Hex로 들어온다.
            {
                Input = Input.Replace("0x", "");
                if (FixYn == "Y" && Order == "Little")
                {
                    //뒤집어야된다.
                    // 2자리씩 나누고 역순으로 정렬

                    //char[] charArray = Input.ToCharArray();
                    //Array.Reverse(charArray);
                    //RtnString = new string(charArray);

                    // 2자리씩 나누기
                    string[] bytes = Enumerable.Range(0, Input.Length / 2)
                                               .Select(j => Input.Substring(j * 2, 2))
                                               .ToArray();

                    // Little Endian 변환 (뒤집기)
                    RtnString = string.Join("", bytes.Reverse());

                }
                else
                {
                    RtnString = Input;
                }

            }




            //MES_EEPROM_VALUE = BitConverter.ToString(Globalo.mCCdPanel.CcdEEpromReadData.GetRange(startAddress, readCount).ToArray()).Replace("-", " ");
            return RtnString; // 마지막 공백 제거
        }
        public static void ChangeToHex(List<TcpSocket.EquipmentParameterInfo> listData)
        {
            var dict = listData.ToDictionary(e => e.Name, e => e);

            foreach (var name in _cpNames)
            {
                if (dict.TryGetValue(name, out var info))
                {
                    Console.WriteLine($"Name: {info.Name}, Value: {info.Value}");
                }
            }
        }
    }

}
