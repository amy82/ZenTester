﻿using System;
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
        public string[] LotId { get; set; }       //"LOT20240601"
        public int socketIndex { get; set; }    //
        public int[] States { get; set; }       //{ 1, 1, 1, 1}  EEPROM ,AOI는 0번 index만 사용
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
}
