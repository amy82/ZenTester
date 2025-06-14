using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.TcpSocket
{
    public enum CMD_POPUP_MESSAGE : int {  cpTEMINAL = 1,  cpDefault };    //cpOPCALL = 1,cpFmt_PPReq,

    public class EquipmentParameterInfo
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<EquipmentParameterInfo> ChildItem { get; set; } = new List<EquipmentParameterInfo>();
    }
    public class ResultData
    {
        public string Cmd { get; set; }
    }
    public class EquipmentData
    {
        public string Command { get; set; } //LGIT_LOT_ID_FAIL
        public string DataID { get; set; }
        public int Judge { get; set; }
        public bool bBuzzer { get; set; }
        public string RecipeID { get; set; }
        public string MaterialID { get; set; }
        public string LotID { get; set; }
        public int CallType { get; set; }
        public string ErrCode { get; set; }
        public string ErrText { get; set; }
        public List<EquipmentParameterInfo> CommandParameter { get; set; } = new List<EquipmentParameterInfo>();
    }
    public class TesterData         //검사 시작, 검사 종료 , Apd 보고 , AOI(Z축 이동), 
    {
        public string Name { get; set; }
        public string Cmd { get; set; }
        public int Step { get; set; }
        public int result { get; set; }
        public string[] LotId { get; set; }       //"LOT20240601"
        public int socketNum { get; set; }    //
        public int[] States { get; set; }       //{ 1, 1, 1, 1}  EEPROM ,AOI는 0번 index만 사용
        public List<EquipmentParameterInfo> CommandParameter { get; set; } = new List<EquipmentParameterInfo>();
        //TESTER  --> H /  REQ_APD_REPORT,
        //HANDLER --> T /  RESP_APD_REPORT,
        //TESTER  --> H /  CMD_Z_MOVE_STEP1, CMD_Z_MOVE_STEP2,
        //HANDLER --> T /  CMD_TEST_STEP1, CMD_TEST_STEP2,
    }
    public class MessageWrapper
    {
        public string Type { get; set; }        //공정명: EEPROM_WRITE, EEPROM_VERIFY, AOI, FW
        public object Data { get; set; }        //Data 안에 EquipmentData 또는 SocketTestState 추가해서 전달
    }


    public class AoiApdData
    {
        public string BcrLot { get; set; }
        public string LH { get; set; }
        public string RH { get; set; }
        public string MH { get; set; }
        public string Gasket { get; set; }
        public string KeyType { get; set; }
        public string CircleDented { get; set; }
        public string Concentrycity_A { get; set; }
        public string Concentrycity_D { get; set; }
        public string Cone { get; set; }
        public string ORing { get; set; }
        public string Result { get; set; }
        public string Barcode { get; set; }
        public string Socket_Num { get; set; }
        public void init()
        {
            BcrLot = string.Empty;
            LH = "0.0";
            RH = "0.0";
            MH = "0.0";
            Gasket = "0.0";
            KeyType = "A";
            CircleDented = "0.0";
            Concentrycity_A = "0.0";
            Concentrycity_D = "0.0";
            Cone = "1";
            ORing = "1";
            Result = "1";
            Barcode = "EMPTY";
            Socket_Num = "1";
        }
    }
}
