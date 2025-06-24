using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;

namespace ZenTester  //ApsMotionControl
{
    public partial class MainForm : Form
    {
        public const int PG_WIDTH = 1920;
        public const int PG_HEIGHT = 1080;

        public const int RunGuideWidth = 330;//164;
        public const int RunButtonWidth = 146;//164;
        

        public const int CamHeight = 330;
        public const int ProductionHeight = 100;
        public const int LogViewHeight = 350;
        
        private int testNum = 0;
        public KeyMessageFilter keyMessageFilter;

        public MainForm()
        {
            InitializeComponent();
            if (Program.TEST_PG_SELECT == TESTER_PG.AOI)
            {
                ProgramState.ON_LINE_MIL = true;
            }
            else
            {
                ProgramState.ON_LINE_MIL = false;
            }
            //ON_LINE_MIL
            //this.TopMost = true;
            keyMessageFilter = new KeyMessageFilter();
            Application.AddMessageFilter(keyMessageFilter);

            
            Event.EventManager.LanguageChanged += OnLanguageChanged;



            this.Size = new System.Drawing.Size(PG_WIDTH, PG_HEIGHT);
            this.Padding = new Padding(0); // 부모 컨트롤의 여백 제거
            this.Location = new System.Drawing.Point(0, 0);

            Globalo.MainForm = this;
            int dRightPanelW = LeftPanel.Width;
            int dRightPanelH = LeftPanel.Height;



            Globalo.threadControl = new ThreadControl();    //<--log Thread 생성후 로그 출력 가능
            Globalo.mAlarmPanel = new Dlg.AlarmControl(dRightPanelW, dRightPanelH);

            Globalo.yamlManager.AlarmLoad();


            Globalo.yamlManager.secsGemDataYaml.MesLoad();



            Globalo.yamlManager.configDataLoad();
            Globalo.yamlManager.taskDataYaml.TaskDataLoad();


            Globalo.yamlManager.modelLIstData.ModelLoad();
            Globalo.yamlManager.aoiRoiConfig = Data.TaskDataYaml.Load_AoiConfig();      //ModelLoad 다음에 로드해라


            //if (Program.TEST_PG_SELECT == TESTER_PG.AOI)
            //Globalo.yamlManager.imageDataLoad();
            //Globalo.yamlManager.RecipeYamlListLoad();

            //string fileName = string.Format(@"{0}\iomap.xlsx", Application.StartupPath); //file path
            //Globalo.dataManage.ioData.ReadEpplusData(fileName);

            // KeyEvent 이벤트 핸들러 추가
            //keyMessageFilter.KeyEvent += KeyMessageFilter_KeyEvent;
            Globalo.mlogControl = new Dlg.LogControl();// dRightPanelW, dRightPanelH);
            
            Globalo.mManualPanel = new Dlg.ModelControl(dRightPanelW, dRightPanelH);
            Globalo.mConfigPanel = new Dlg.ConfigControl(dRightPanelW, dRightPanelH);
            Globalo.setTestControl = new Dlg.SetTestControl();
            Globalo.cameraControl = new Dlg.CameraControl();
            Globalo.markViewer = new VisionClass.MarkViewerForm();
            Globalo.fwSetControl = new Dlg.FwSetControl();
            Globalo.WriteSetControl = new Dlg.WriteSetControl();
            Globalo.VerifySetControl = new Dlg.VerifySetControl();
            //Globalo.mioPanel = new Dlg.IoControl(dRightPanelW, dRightPanelH);

            Globalo.productionInfo = new Dlg.ProductionInfo();
            Globalo.tabMenuForm = new Dlg.TabMenuForm(RightPanel.Width, RightPanel.Height);
            
            
            this.RightPanel.Controls.Add(Globalo.tabMenuForm);



            
            BTN_TOP_LOG.Location = new System.Drawing.Point(this.TopPanel.Width - BTN_TOP_LOG.Width, 0);
            BTN_TOP_LOG.BackColor = ColorTranslator.FromHtml("#ED6C44");


            Globalo.threadControl.AllThreadStart();     //< - Log , Time Thread

            //레시피 요청하기
            Globalo.yamlManager.vPPRecipeSpecEquip = Globalo.yamlManager.RecipeLoad(Globalo.yamlManager.secsGemDataYaml.ModelData.CurrentRecipe);   //init

            if (Globalo.yamlManager.vPPRecipeSpecEquip == null)
            {
                Globalo.LogPrint("ManualControl", $"[{Globalo.dataManage.mesData.m_sMesPPID}] Recipe Load Fail");
            }
            //CLIENT 연결되면  APS_RECIPE_CMD 받고 Recipe Load() 호출한다.






            if (ProgramState.ON_LINE_MIL == true)
            {
                Globalo.visionManager = new VisionClass.VisionManager();

                Globalo.visionManager.SetPanelSize(
                    Globalo.cameraControl.getWidth(), Globalo.cameraControl.getHeight(),
                    Globalo.setTestControl.getWidth(), Globalo.setTestControl.getHeight());

                Globalo.visionManager.RegisterDisplayHandle(0, Globalo.cameraControl.panelCam1.Handle);
                Globalo.visionManager.RegisterDisplayHandle(1, Globalo.cameraControl.panelCam2.Handle);
                Globalo.visionManager.RegisterDisplayHandle(2, Globalo.setTestControl.Set_panelCam.Handle);
                Globalo.visionManager.RegisterDisplayHandle(3, Globalo.setTestControl.manualTest.panel_Mark.Handle);
                Globalo.visionManager.RegisterDisplayHandle(4, Globalo.markViewer.panel_MarkZoomImage.Handle);

                Globalo.visionManager.MilSet();


                Globalo.setTestControl.setCamCenter();
                Globalo.cameraControl.drawCenterCross();
            }
            Globalo.FxaBoardManager = new Fxa.FxaBoardManager();

            Globalo.tcpManager = new TcpSocket.TcpManager();

            Globalo.tcpManager.SetClient(Globalo.yamlManager.configData.DrivingSettings.HandlerIp, Globalo.yamlManager.configData.DrivingSettings.HandlerPort);

            Globalo.tcpManager.SetVerifyClient("127.0.0.1", 5000);

            Globalo.taskManager = new TaskClass.TaskManager();


            Globalo.mConfigPanel.BackColor = ColorTranslator.FromHtml("#F8F3F0");
            Globalo.mAlarmPanel.BackColor = ColorTranslator.FromHtml("#F8F3F0");
            Globalo.mlogControl.BackColor = ColorTranslator.FromHtml("#F8F3F0");

            //Globalo.mIoPanel.eLogSender += eLogPrint;
            //Globalo.mCCdPanel.SetSensorIni();

            MainUiSet();

            SerialConnect();

           //// serverStart();      //SECS - GEM 연결

            Globalo.tcpManager.ReqRecipeToSecsgem();
            Globalo.tcpManager.ReqModelToSecsgem();
            //AOI 공정일 경우 시작할때, Secsgem으로 레시피 요청하기
            //

            TopPanel.Paint += new PaintEventHandler(Form_Paint);
            eLogPrint("Main", "PG START");

            Globalo.productionInfo.BcrSet(Globalo.dataManage.TaskWork.m_szChipID);
            Globalo.productionInfo.ProductionInfoSet();
            Globalo.productionInfo.PinCountInfoSet();
            Globalo.productionInfo.ShowModelName();
            Globalo.productionInfo.ShowRecipeName();

            //Globalo.pickerInfo.SetPickerInfo();

            Globalo.tabMenuForm.MenuButtonSet(Dlg.TabMenuForm.TABFORM.MAIN_FORM);

            Program.SetLanguage(Globalo.yamlManager.configData.DrivingSettings.Language);


            //LeeTestForm popupForm = new LeeTestForm();
            //popupForm.Show();
            //popupForm.WindowState = FormWindowState.Minimized;
        }
        
        private void OnLanguageChanged(object sender, EventArgs e)
        {
            // 이벤트 처리
            //this.Text = Resource.Strings.OP_ORIGIN;
            Console.WriteLine("MainForm - OnLanguageChanged");
        }
        private async void serverStart()
        {
            await Globalo.tcpManager.StartServerAsync();
        }
        public void InitMilLib()
        {
            
            //
            //Globalo.vision.AllocMilApplication();
            //Globalo.vision.AllocMilCamBuffer();
            //Globalo.vision.AllocMilCCdBuffer(0, Globalo.mLaonGrabberClass.m_nWidth, Globalo.mLaonGrabberClass.m_nHeight);

            //Globalo.vision.AllocMilCcdDisplay(Globalo.camControl.CcdPanel.Handle);
            //Globalo.vision.AllocMilCamDisplay(Globalo.camControl.CamPanel.Handle);

            //Globalo.vision.EnableCamOverlay();
            //Globalo.vision.EnableCcdOverlay();
            //Globalo.vision.DrawOverlay();


            ///Globalo.vision.GrabRun();        //기존 cam grab thread
        }

        private void SerialConnect()
        {
            // 바코드 리더기 Serial Port 설정
            string portData = "";
            bool connectRtn = false;
            


            if(Globalo.yamlManager.configData != null)
            {
                portData = Globalo.yamlManager.configData.SerialPort.Light;
            }
            else
            {
                portData = "COM1";
            }

            Globalo.serialPortManager.LightControl = new Serial.SerialCommunicator(portData);
            Globalo.serialPortManager.LightControl.myName = "Light";
;            //barcodePort.DataReceived += (sender, data) =>
            //{
            //    Console.WriteLine("Barcode Reader Data: " + data);
            //};
            connectRtn = Globalo.serialPortManager.LightControl.Open();

        }
        
        
        
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            int lineStartY = TopPanel.Height - 1;
            // Graphics 객체 가져오기
            Graphics g = e.Graphics;

            // Pen 객체 생성 (색상과 두께 설정)
            Color color = Color.FromArgb(75, 75, 75);//(86, 86, 86);
            Pen pen = new Pen(color, 2);

            // 라인 그리기 (시작점과 끝점 설정)
            g.DrawLine(pen, 0, lineStartY, TopPanel.Width, lineStartY);

            // 리소스 해제
            pen.Dispose();
        }
        private void MainUiSet()
        {
            //int i = 0;

            int MainBtnHGap = 2;

            //-----------------------------------------------
            //상단 패널
            //-----------------------------------------------
            TopPanel.BackColor = ColorTranslator.FromHtml("#FAFAFA");
            MainTitleLabel.ForeColor = ColorTranslator.FromHtml("#8F949F");
            MainTitleLabel.BackColor = Color.Transparent;
            string _pgModel = string.Empty;
            if (Program.TEST_PG_SELECT == TESTER_PG.AOI)
            {
                _pgModel = "Aoi";
            }
            if (Program.TEST_PG_SELECT == TESTER_PG.EEPROM_WRITE)
            {
                _pgModel = "Write";
            }
            if (Program.TEST_PG_SELECT == TESTER_PG.EEPROM_VERIFY)
            {
                _pgModel = "Verify";
            }
            if (Program.TEST_PG_SELECT == TESTER_PG.FW)
            {
                _pgModel = "fw";
            }
            MainTitleLabel.Text = "Zen Tester :"+ _pgModel;

            //-----------------------------------------------
            int MidPanelHeight = LeftPanel.Height;          //Left Middle 패널 높이
            //int ViewPanelHeight =  LeftPanel.Height - CamHeight - MainBtnHGap - RunButtonHeight - MainBtnHGap;      // 전체 높이에서 -카메라높이 - 버튼 높이 - 생상정보       //로그창 높이
            int nBottomPanelY = RightPanel.Location.Y;     //Bottom 패널 Position Y

            Console.WriteLine($"LeftPanel W:{LeftPanel.Width}, H:{LeftPanel.Height}");
            //-----------------------------------------------
            //우측 패널
            //-----------------------------------------------
            RightPanel.BackColor = ColorTranslator.FromHtml("#4C4743");


            //-----------------------------------------------
            //중단 우 패널
            //-----------------------------------------------

            //CenterPanel.BackColor = ColorTranslator.FromHtml("#F8F3F0");


            //-----------------------------------------------
            //좌측 패널
            //-----------------------------------------------
            
            LeftPanel.Controls.Add(Globalo.productionInfo);
            if (Program.TEST_PG_SELECT == TESTER_PG.AOI)
            {
                LeftPanel.Controls.Add(Globalo.cameraControl);
                LeftPanel.Controls.Add(Globalo.setTestControl);
                Globalo.cameraControl.Location = new System.Drawing.Point(0, Globalo.productionInfo.Height + MainBtnHGap);
                Globalo.setTestControl.Location = new System.Drawing.Point(0, Globalo.productionInfo.Height + MainBtnHGap);
                Globalo.setTestControl.Visible = false;

                
            }
            if (Program.TEST_PG_SELECT == TESTER_PG.FW)
            {
                LeftPanel.Controls.Add(Globalo.fwSetControl);
                Globalo.fwSetControl.Location = new System.Drawing.Point(0, Globalo.productionInfo.Height + MainBtnHGap);
                Globalo.fwSetControl.Visible = false;

                
            }
            if (Program.TEST_PG_SELECT == TESTER_PG.EEPROM_WRITE)
            {
                LeftPanel.Controls.Add(Globalo.fwSetControl);
                Globalo.WriteSetControl.Location = new System.Drawing.Point(0, Globalo.productionInfo.Height + MainBtnHGap);
                Globalo.WriteSetControl.Visible = false;
            }
            if (Program.TEST_PG_SELECT == TESTER_PG.EEPROM_VERIFY)
            {
                LeftPanel.Controls.Add(Globalo.fwSetControl);
                Globalo.VerifySetControl.Location = new System.Drawing.Point(0, Globalo.productionInfo.Height + MainBtnHGap);
                Globalo.VerifySetControl.Visible = false;
            }

            int logStartX = 0;
            int logStartY = 0;
            logStartX = Globalo.cameraControl.Width-10;
            logStartY = Globalo.cameraControl.Location.Y+6;

            LeftPanel.Controls.Add(Globalo.mlogControl);
            //LeftPanel.Controls.Add(Globalo.operationPanel);
            //LeftPanel.Controls.Add(Globalo.trayStateInfo);
            //LeftPanel.Controls.Add(Globalo.socketStateInfo);
            //LeftPanel.Controls.Add(Globalo.pickerInfo);

            Globalo.productionInfo.Location = new System.Drawing.Point(0, 0);
           
            Globalo.mConfigPanel.Location = new System.Drawing.Point(0, Globalo.productionInfo.Height + MainBtnHGap);
            Globalo.mAlarmPanel.Location = new System.Drawing.Point(0 , Globalo.productionInfo.Height + MainBtnHGap);
            Globalo.mManualPanel.Location = new System.Drawing.Point(0 , Globalo.productionInfo.Height + MainBtnHGap);
            Globalo.mlogControl.Location = new System.Drawing.Point(logStartX, logStartY);// Globalo.productionInfo.Height + MainBtnHGap);
            
            //Globalo.pickerInfo.Location = new System.Drawing.Point(0, Globalo.operationPanel.Location.Y + MainBtnHGap);
            //Globalo.socketStateInfo.Location = new System.Drawing.Point(0, Globalo.pickerInfo.Location.Y + Globalo.pickerInfo.Height + MainBtnHGap);
            //Globalo.trayStateInfo.Location = new System.Drawing.Point(Globalo.socketStateInfo.Location.X + Globalo.socketStateInfo.Width+10, Globalo.pickerInfo.Location.Y + Globalo.pickerInfo.Height + MainBtnHGap);

            //Globalo.mCCdPanel.Visible = false;
            Globalo.mConfigPanel.Visible = false;
            Globalo.mAlarmPanel.Visible = false;
            Globalo.mlogControl.Visible = true;

            //CenterPanel.Controls.Add(Globalo.mMainPanel);
            //CenterPanel.Controls.Add(Globalo.mTeachPanel);
            //CenterPanel.Controls.Add(Globalo.mioPanel);
            //CenterPanel.Controls.Add(Globalo.mCCdPanel);
            LeftPanel.Controls.Add(Globalo.mManualPanel);
            LeftPanel.Controls.Add(Globalo.mConfigPanel);
            LeftPanel.Controls.Add(Globalo.mAlarmPanel);




            
        }
        
        public void MainTitleChange()
        {
            if (ProgramState.CurrentRunMode == ProgramState.eRunMode.ENGINEER)
            {
                //엔지니어 모드일대 상단 배경 색 바꾸자.
                //TopPanel.BackColor = Color.LimeGreen;
                //MainTitleLabel.ForeColor = Color.Black; ;//ColorTranslator.FromHtml("#8F949F");
            }
            else
            {
                //TopPanel.BackColor = Color.MintCream; ;// ColorTranslator.FromHtml("#FAFAFA");
                //MainTitleLabel.ForeColor = Color.DarkCyan;
            }


            string title = ProgramState.PG_TITLE;//// + " - " + ProgramState.CurrentRunMode.ToString() + " MODE";
           
            MainTitleLabel.BackColor = Color.Transparent;
            MainTitleLabel.Text = title;

        }
        
        private void eLogPrint(object oSender, string LogDesc, Globalo.eMessageName bPopUpView = Globalo.eMessageName.M_NULL)
        {
            DateTime dTime = DateTime.Now;
            string LogInfo = $"[{dTime:hh:mm:ss.f}] {LogDesc}";
            Globalo.threadControl.logThread.logQueue.Enqueue(LogInfo);

            if (bPopUpView != Globalo.eMessageName.M_NULL)
            {
                MessagePopUpForm messagePopUp3 = new MessagePopUpForm();
                messagePopUp3.MessageSet(Globalo.eMessageName.M_ERROR, LogDesc);
                messagePopUp3.Show();
            }
        }

        private void BTN_BOTTOM_EXIT_Click(object sender, EventArgs e)
        {
            
        }
        public void ClientConnected(bool state)
        {
            if (state == true)
            {
                BTN_TOP_CLIENT.BackColor = Color.Green;
            }
            else
            {
                BTN_TOP_CLIENT.BackColor = Color.White;
            }
            ProgramState.STATE_CLINET_CONNECT = state;
        }
        public void DriverConnected(bool state)
        {
            if (state == true)
            {
                BTN_TOP_MES.BackColor = Color.Green;
            }
            else
            {
                BTN_TOP_MES.BackColor = Color.White;
            }

            ProgramState.STATE_DRIVER_CONNECT = state;
        }
        public void FuncExit()
        {
            Event.EventManager.RaisePgExit();
            Globalo.threadControl.AllClose();
            //Globalo.motionManager.ioController.Close();
            //Time Thread End
            //oGlobal.mDioControl.DioEnd();

            //oGlobal.mtimeThread.mTimeThreadRun = false;
            //if (oGlobal.mtimeThread != null)
            //{
            //oGlobal.mtimeThread.Interrupt();   //스레도 실행 정지
            //oGlobal.mtimeThread.Join();
            //}
            //Vision End
            //oGlobal.vision.ThreadEnd();
            //if (Globalo.GrabberDll.mIsGrabStarted())
            //{
            //    Globalo.GrabberDll.mGrabStop();
            //}
            //Globalo.GrabberDll.mCloseBoard();

            

            if (ProgramState.ON_LINE_MOTOR)
            {
                Globalo.motionManager.MotionClose();
            }

            Globalo.GrabberDll = null;
            //
            //foreach (Form form in Application.OpenForms)
            //{
            //    form.Close();
            //}
            //Globalo.dataManage.teachingData.eLogSender -= eLogPrint;
            //Globalo.motorControl.eLogSender -= eLogPrint;
            //Globalo.dIoControl.eLogSender -= eLogPrint;
            // 다이얼로그
            //
            //mTeachPanel = new Dlg.TeachingControl();
            //mManualPanel = new Dlg.ManualControl();
            // Thread Main
            //

            //Globalo.mLaonGrabberClass.eLogSender -= eLogPrint;
            Globalo.mLaonGrabberClass.Dispose();

            //foreach (var thread in System.Diagnostics.Process.GetCurrentProcess().Threads)
            //{
            //    ((System.Diagnostics.ProcessThread)thread).Dispose();
            //}

            //System.Diagnostics.Process.GetCurrentProcess().Kill();

            Application.Exit();

        }

        

        private void BTN_TOP_LOG_Click(object sender, EventArgs e)
        {

            if (Debugger.IsAttached)
            {
                //string tempLot = "Z23DC24327000030V3WT-13A997-A*";
                string lotname = "Z23DC24327000095V3WT-13A997-A_113410.csv";
                string prefix = "Z23DC24327000095V3WT-13A997-A";

            //    string csvFolderPath = "D:\\EVMS\\LOG\\MMD_DATA\\2025\\02\\28";

            //    // 해당 폴더 안의 모든 파일 리스트 가져오기
            //    string[] files = Directory.GetFiles(csvFolderPath);

            //    // prefix가 포함된 파일명 리스트 필터링
            //    List<string> matchedFiles = files
            //        .Where(file => Path.GetFileName(file).Contains(prefix))
            //        .Select(Path.GetFileName) // 경로가 아닌 파일명만 추출
            //        .ToList();

            //    string earliestFile = matchedFiles
            //        .OrderByDescending(Data.CEEpromData.GetTimeFromFileName) // 시간 기준으로 내림차순 정렬 가장 늦은 시간 출력
            //        //.OrderBy(Data.CEEpromData.GetTimeFromFileName) // // 시간 기준으로 오름차순 정렬
            //        .FirstOrDefault(); // 가장 빠른 시간의 파일 선택




                if (lotname.Contains(prefix))
                {
                    Console.WriteLine("lotname에 prefix가 포함되어 있습니다.");
                }
                else
                {
                    Console.WriteLine("lotname에 prefix가 포함되어 있지 않습니다.");
                }
                testNum++;

                TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();

                //sendEqipData.Command = "OBJECT_ID_REPORT";
                //sendEqipData.LotID = "aaaaaaB" + testNum.ToString();

                sendEqipData.Command = "LOT_APD_REPORT";
                sendEqipData.CommandParameter = new List<TcpSocket.EquipmentParameterInfo>();
                for (int i = 0; i < 10; i++)
                {
                    TcpSocket.EquipmentParameterInfo pInfo = new TcpSocket.EquipmentParameterInfo();
                    pInfo.Name = (i+1).ToString();
                    pInfo.Value = (i + 1).ToString() + "value";
                    sendEqipData.CommandParameter.Add(pInfo);
                }
                

                Globalo.tcpManager.SendMessageToClient(sendEqipData);


                //
            }


        }


        
        
        private void BTN_BOTTOM_LIGHT_Click(object sender, EventArgs e)
        {
            //MenuButtonSet(4);
        }

        private void BTN_TOP_MES_Click(object sender, EventArgs e)
        {
            
        }

        
        
        private void BTN_MAIN_JUDGE_RESET_Click(object sender, EventArgs e)
        {
            
        }

        private void BTN_TOP_CCD_Click(object sender, EventArgs e)
        {
            //CCD ON
            //if (ProgramState.CurrentState == OperationState.AutoRunning)
            //{
            //    Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
            //    return;
            //}
            //if (ProgramState.CurrentState == OperationState.ManualTesting)
            //{
            //    Globalo.LogPrint("ManualControl", "[INFO] MANUAL 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
            //    return;
            //}
            //if (Globalo.threadControl.manualThread.GetThreadRun() == false)
            //{
            //    Globalo.LogPrint("", "[CCD] MANUAL CCD START");
            //    Globalo.threadControl.manualThread.runfn(FThread.ManualThread.eManualType.M_CCD_START);//4);
            //}
        }
        
        

        private void BTN_MAIN_PINCOUNT_RESET_Click(object sender, EventArgs e)
        {
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true; // 폼에서 키 이벤트를 받기 위해 설정
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.RemoveMessageFilter(keyMessageFilter);  // 메시지 필터 제거
        }

        private void BTN_TOP_CLIENT_Click(object sender, EventArgs e)
        {
            
        }

        private void BTN_TOP_LOG_Click_1(object sender, EventArgs e)
        {
            if (ProgramState.NORINDA_MODE == true)
            {
                LeeTestForm popupForm = new LeeTestForm();
                popupForm.Show();
            }
                
        }
    }
}
