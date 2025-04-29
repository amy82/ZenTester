﻿using System.Collections.Generic;

namespace ZenHandler.Data
{
    public struct RcmdCtrlStateChgReq
    {
        public string ConfirmFlag;
        public int ControlState;
        public string Change_Code;
        public string Change_Text;
        public string Result_Code;
        public string Result_Text;
    }
    public struct RcmdOpCall
    {
        public int CallType;
        public string OpCall_Code;
        public string OpCall_Text;
    }
    public struct RcmdMaterialIdConfirm
    {
        public string MaterialId;
        public string MaterialType;
        public string MaterialType_Text;
        public int UnitNo;
        public int SlotNo;
        public string RemainData;
    }
    public struct RcmdMaterialIdFail
    {
        public string MaterialId;
        public string MaterialType;
        public string MaterialType_Text;
        public int UnitNo;
        public int SlotNo;
        public string Code;
        public string Text;

    }
    public struct RcmdEEpromFail
    {
        public string LotIdValue;
        public string CodeValue;
        public string TextValue;
    }
    public struct RcmdLgitPause
    {
        public string PauseCode;
        public string PauteText;
    }

    public struct RcmdPPUploadFail
    {
        public string RECIPEID;
        public string CODE;
        public string TEXT;
        public List<RcmdParameter> Children;    // 하위 LIST 저장
    }
    public struct RcmdLotIdFail
    {
        public string CODE;
        public string TEXT;
        public List<RcmdParameter> Children;    // 하위 LIST 저장
    }
    

    public struct RcmdParam1
    {
        public string CpName;
        public string CepVal;
    }
    public struct RcmdParameter
    {
        public string name;                     // CPNAME
        public string value;                    // CEPVAL
        public List<RcmdParameter> Children;    // 하위 LIST 저장
    }
    
    public class CMesData
    {
        public string m_sEquipmentID { get; set; } = "";
        public string m_sEquipmentName { get; set; } = "";
        public string m_sMesOperatorID { get; set; } = "";
        public string m_sRecipeId { get; set; } = "";
        public string m_sErcmdCode { get; set; } = "";
        public string m_sErcmdText { get; set; } = "";
        public string m_sMesPPID { get; set; } = "";
        public string m_sMesRecipeRevision { get; set; } = "";
        public string m_dEqpDefectCode { get; set; } = "";
        public bool IdleReasonReportUsage { get; set; } = false;
        public int IdleSetTimeInterval { get; set; } = 0;
        public int m_dControlStateChangeOrder { get; set; } = 0;
        public int m_dLotProcessingState { get; set; } = 0;
        public int m_nMesFinalResult { get; set; } = 0;
        public bool m_bLgit_Pause_req { get; set; } = false;
        
        public int EstablishCommunicationsTimeout { get; set; } = 0;    //ECID 101
        public int HeartBeatRate { get; set; } = 0;                     //ECID 102
        public int DefaultCommState { get; set; } = 0;                  //ECID 103
        public int DefaultCtrlState { get; set; } = 0;                  //ECID 104
        public int DefaultOfflineSubstate { get; set; } = 0;            //ECID 105
        public int DefCtrlOfflineState { get; set; } = 0;               //ECID 106
        public int TimeFormat { get; set; } = 0;                        //ECID 107
        public int DefaultOnlineSubState { get; set; } = 0;             //ECID 108
        public int ConversationTimeoutCount { get; set; } = 0;          //ECID 109

        public const int APD_REPORT_MAX_COUNT = 5000;//9998;       //40002 ~ 49999

        //int EstablishCommunicationsTimeout;     //ECID 101
        //int HeartBeatRate;                      //ECID 102
        //int DefaultCommState;                   //ECID 103
        //int DefaultCtrlState;                   //ECID 104
        //int DefaultOfflineSubstate;             //ECID 105
        //int DefCtrlOfflineState;                //ECID 106
        //int TimeFormat;                         //ECID 107
        //int DefaultOnlineSubState;              //ECID 108
        //int ConversationTimeoutCount;           //ECID 109


        public List<string> vMesApdData = new List<string>();
        public List<int> m_uAlarmList = new List<int>();

        public int[] m_dProcessState = new int[2];
        public int[] m_dEqupControlState = new int[2];
        public int[] m_dPPChangeArr = new int[2];
        public int[] m_dEqupOperationMode = new int[2];

        public string LastSentMessage { get; set; } = "";// string.Empty;



        //Report Data
        public RcmdCtrlStateChgReq rCtrlState_Chg_Req = new RcmdCtrlStateChgReq();         //LGIT_CTRLSTATE_CHG_REQ
        public RcmdOpCall rCtrlOp_Call = new RcmdOpCall();                        //LGIT_OP_CALL
        public RcmdMaterialIdConfirm rMaterial_Id_Confirm = new RcmdMaterialIdConfirm();     //LGIT_MATERIAL_ID_CONFIRM
        public RcmdMaterialIdFail rMaterial_Id_Fail = new RcmdMaterialIdFail();           //LGIT_MATERIAL_ID_FAIL
        public RcmdLgitPause rLgit_Pause = new RcmdLgitPause();					   //LGIT_PAUSE
        public RcmdEEpromFail rEEprom_Fail = new RcmdEEpromFail();                    //LGIT_EEPROM_FAIL
        public RcmdPPUploadFail vPPUploadFail = new RcmdPPUploadFail();                 //LGIT_PP_UPLOAD_FAIL
        public RcmdLotIdFail vLotIdFail = new RcmdLotIdFail();                       //LGIT_LOT_ID_FAIL

        public List<RcmdParam1> vIdleReason { get; set; } = new List<RcmdParam1>();
        public List<RcmdParam1> vOfflineReason { get; set; } = new List<RcmdParam1>();
        public List<RcmdParam1> vModelLIst { get; set; } = new List<RcmdParam1>();
        //public List<RcmdPPUploadFail> vPPUploadFail { get; set; } = new List<RcmdPPUploadFail>();



        //Set.2
        //public List<RcmdParameter> vLotIdFail { get; set; } = new List<RcmdParameter>();
        //public List<RcmdParam1> vLotIdFail { get; set; } = new List<RcmdParam1>();      //TODO: 클래스를 바꾸자 RcmdPPUploadFail 이거처럼 250310 18:32
        public List<RcmdParameter> vPPSelect { get; set; } = new List<RcmdParameter>();
        public List<RcmdParameter> vPPUploadConfirm { get; set; } = new List<RcmdParameter>();

        //Set.3
        public List<RcmdParameter> vLotStart { get; set; } = new List<RcmdParameter>();
        public List<RcmdParameter> vMaterialExchange { get; set; } = new List<RcmdParameter>();



        //EEPROM_DATA = RcmdParameter 하고 비슷하다
        //public List<RcmdParameter> vLotIdFail 

        public List<MesEEpromCsvData> VMesEEpromData { get; set; } = new List<MesEEpromCsvData>();


        ///private CMesData() { } // 외부 생성 방지
        public CMesData()
        {
            m_dEqupControlState[0] = (int)Ubisam.eCURRENT_CONTROL_STATE.eEquipmentOffline;
            m_dEqupControlState[1] = (int)Ubisam.eCURRENT_CONTROL_STATE.eEquipmentOffline;


            VMesEEpromData.Clear();


            vPPUploadFail.Children = new List<RcmdParameter>();
            vLotIdFail.Children = new List<RcmdParameter>();
            //VMesEEpromData.Add(new EEpromCsvData
            //{
            //    SHOPID = "aaaa0",
            //    PRODID = "aaaa1",
            //    PROCID = "aaaa2",
            //    EEP_ITEM = "aaaa3",
            //    ADDRESS = 44,
            //    DATA_SIZE = 55,
            //    DATA_FORMAT = "aaaa6",
            //    BYTE_ORDER = "aaaa7",
            //    FIX_YN = "aaaa8",
            //    ITEM_CODE = "aaaa9",
            //    ITEM_VALUE = "aaaa10",
            //    CRC_START = "aaaa11",
            //    CRC_END = "aaaa12",
            //    PAD_VALUE = "aaaa13",
            //    PAD_POSITION = "aaaa14"
            //});


        }
    }
}
