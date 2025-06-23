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
            "EEPROM_LAST_UPDATED_ENTITY",                   //ASCII
            "IMAGER_NAME",                                  //ASCII
            "Imager exact Color Filter Array",              //ASCII
            "Imager input clock frequency",                 //DEC
            "CAMERA_LOCATION_AT_VEHICLE_LEVEL_MAJOR",
            "CAMERA_LOCATION_AT_VEHICLE_LEVEL_MINOR",
            "MANUFACTURER",                                 //ASCII
            "MANUFACTURER_PART_NUMBER",                     //ASCII
            "TESLA_PART_NUMBER",                            //ASCII
            "MANUFACTURED_LOCATION",                        //ASCII
            "MANUFACTURED_ASY_LOCATION",                    //ASCII
            "LENS_MANUFACTURER",                            //ASCII
            "LENS_PART_NUMBER",                             //ASCII
            "LENS_APERTURE",                                //FLOAT
            "MODULE_ORIENTATION_ADJUSTMENT",
            "MANUFACTURER_INTERNAL_VERSION_CONTROL",
            "SERIALIZER_TYPE",                              //ASCII
            "DIST_VERSION",                                 //DEC 0x가 없는 Hex로 처리 : 김수현선임 250623
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
            "PCBA_PN",                                    //ASCII
            "PCBA_MFG",                                   //ASCII
            "IRCF_PN",                                    //ASCII
            "IRCF_MFG",                                   //ASCII
            "FLAG05_MCU",                                 //DEC
            "FLAG06_Heater"                               //DEC
        };
        private static readonly string[] _cpFormat = new string[]
        {
            HEX,HEX,
            ASCII,ASCII,ASCII,
            DEC,
            HEX,HEX,
            ASCII, ASCII, ASCII,ASCII,ASCII,ASCII,ASCII, 
            FLOAT,
            HEX, HEX,
            ASCII,
            HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,HEX,
            ASCII,ASCII,ASCII,ASCII,
            DEC,DEC
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
        public static byte[] StringToHex(string Input, string Format, string Order, string FixYn = "Y")       //MES 에서 받은 값을 변환
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
                    //Array.Reverse(bytes);

                }
                for (i = 0; i < bytes.Length; i++) // 뒤에서부터 추가
                {
                    hex.AppendFormat("{0:X2}", bytes[i]);      //Little Endian 변환 코드
                }
                //RtnString = hex.ToString().Trim();
                return bytes;
            }
            else if (Format == FLOAT && FixYn == "Y")
            {
                float floatValue = float.Parse(Input);
                byte[] bytes = BitConverter.GetBytes(floatValue); // float → byte[]
                if (Order == "Little")
                {
                    Array.Reverse(bytes); // 빅엔디안으로 변환 (네트워크 전송 시 필요)
                }

                //RtnString = BitConverter.ToString(bytes).Replace("-", "");
                return bytes;
            }
            else if (Format == DOUBLE && FixYn == "Y")
            {
                double doubleValue = double.Parse(Input);
                byte[] bytes = BitConverter.GetBytes(doubleValue); // double → byte[]
                if (Order == "Little")
                {
                    //Array.Reverse(bytes); // 빅엔디안 변환
                }

                //RtnString = BitConverter.ToString(bytes).Replace("-", "");
                return bytes;

            }
            else// (Format == HEX || Format == EMPTY || Format ==  || FixYn == "N")      //N이면 무조건 Hex로 들어온다.
            {
                Input = Input.Replace("0x", "");
                if (Input.Length % 2 != 0)
                {
                    Input = "0" + Input; // 홀수일 경우 앞에 0 붙여서 짝수로
                }
                int len = Input.Length / 2;

                byte[] bytes = new byte[len];

                for (i = 0; i < len; i++)
                {
                    bytes[i] = Convert.ToByte(Input.Substring(i * 2, 2), 16);
                }
                if (FixYn == "Y" && Order == "Little")
                {
                    //Array.Reverse(bytes);
                }
                return bytes;
                //if (FixYn == "Y" && Order == "Little")
                //{
                //    //뒤집어야된다.
                //    // 2자리씩 나누고 역순으로 정렬
                //    // 2자리씩 나누기
                //    string[] bytes = Enumerable.Range(0, Input.Length / 2)
                //                               .Select(j => Input.Substring(j * 2, 2))
                //                               .ToArray();
                //    // Little Endian 변환 (뒤집기)
                //    RtnString = string.Join("", bytes.Reverse());

                //}
                //else
                //{
                //    RtnString = Input;
                //}


            }
            //return RtnString; // 마지막 공백 제거
        }
        public static void ChangeToHex(List<TcpSocket.EquipmentParameterInfo> listData)
        {
            var dict = listData.ToDictionary(e => e.Name, e => e);
            int i = 0;
            foreach (var name in _cpNames)
            {
                if (dict.TryGetValue(name, out var info))
                {
                    Console.WriteLine($"Name: {info.Name}, Value: {info.Value}");
                    byte[] bytes = StringToHex(info.Value, _cpFormat[i], "Little");//"Little"); Big
                    foreach (byte b in bytes)
                    {
                        Globalo.FxaBoardManager.fxaEEpromVerify.mmdEEpromData.Add(b); // 1바이트씩 넣기
                    }
                    i++;

                }
            }
        }

        public static readonly ushort[] Crc16Table_1021 = new ushort[256]
       {
           0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7, 0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C, 0xD1AD, 0xE1CE, 0xF1EF,
           0x1231, 0x0210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6, 0x9339, 0x8318, 0xB37B, 0xA35A, 0xD3BD, 0xC39C, 0xF3FF, 0xE3DE,
           0x2462, 0x3443, 0x0420, 0x1401, 0x64E6, 0x74C7, 0x44A4, 0x5485, 0xA56A, 0xB54B, 0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D,
           0x3653, 0x2672, 0x1611, 0x0630, 0x76D7, 0x66F6, 0x5695, 0x46B4, 0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC,
           0x48C4, 0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861, 0x2802, 0x3823, 0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B,
           0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0x0A50, 0x3A33, 0x2A12, 0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A,
           0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41, 0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49,
           0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0x0E70, 0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78,
           0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F, 0x1080, 0x00A1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046, 0x6067,
           0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E, 0x02B1, 0x1290, 0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256,
           0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D, 0x34E2, 0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
           0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E, 0xC71D, 0xD73C, 0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634,
           0xD94C, 0xC96D, 0xF90E, 0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB, 0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3,
           0xCB7D, 0xDB5C, 0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A, 0x4A75, 0x5A54, 0x6A37, 0x7A16, 0x0AF1, 0x1AD0, 0x2AB3, 0x3A92,
           0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9, 0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0x0CC1,
           0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8, 0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0x0ED1, 0x1EF0
       };

        public static bool CalculateCRC16(byte[] data, out byte[] result)
        {
            ushort crc_value = 0x0000;
            ushort crc_xorValue = 0x0000;
            result = new byte[2];

            // CRC 계산 루프
            for (int count = 0; count < data.Length; count++)
            {
                int nVal1 = crc_value >> 8;
                int nVal2 = data[count];

                int nIndex = (nVal1 ^ nVal2) & 0x00FF;

                crc_value = (ushort)((crc_value << 8) ^ Crc16Table_1021[nIndex]);
            }

            crc_value ^= crc_xorValue;

            result[0] = (byte)((crc_value >> 8) & 0xFF); // 상위 바이트
            result[1] = (byte)(crc_value & 0xFF);        // 하위 바이트

            return true;
        }
    }

}
