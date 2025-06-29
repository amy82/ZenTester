using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ZenTester.Dlg
{
    public partial class ConfigControl : UserControl
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용

        private enum eManualBtn : int
        {
            pcbTab = 0, lensTab
        };
        public ConfigControl(int _w, int _h)
        {
            InitializeComponent();

            this.Paint += new PaintEventHandler(Form_Paint);
            
            this.Width = _w;
            this.Height = _h;

            setInterface();

        }
        public void RefreshConfig()
        {
            ShowDriveSet();
            ShowComPort();
            ShowLanguage();
        }
        public void GetConfigData()
        {
            string Handlerip = label_Handler_Ip1.Text + "." + label_Handler_Ip2.Text + "." + label_Handler_Ip3.Text + "." + label_Handler_Ip4.Text;
            string Secsgemip = label_Secsgem_Ip1.Text + "." + label_Secsgem_Ip2.Text + "." + label_Secsgem_Ip3.Text + "." + label_Secsgem_Ip4.Text;

            string Handlerport = label_Handler_Port.Text;
            string Secsgemport = label_Secsgem_Port.Text;
            Globalo.yamlManager.configData.DrivingSettings.HandlerIp = Handlerip;
            Globalo.yamlManager.configData.DrivingSettings.HandlerPort = int.Parse(Handlerport);
            Globalo.yamlManager.configData.DrivingSettings.SecsgemIp = Secsgemip;
            Globalo.yamlManager.configData.DrivingSettings.SecsgemPort = int.Parse(Secsgemport);

            //
            Globalo.yamlManager.configData.DrivingSettings.PinCountMax = int.Parse(label_PinCountMax.Text);
            Globalo.yamlManager.configData.DrivingSettings.CsvScanMonth = int.Parse(label_CsvScanMax.Text);


            //운전 설정
            Globalo.yamlManager.configData.DrivingSettings.PinCountUse = hopeCheckBox_PinCountUse.Checked;
            Globalo.yamlManager.configData.DrivingSettings.ImageGrabUse = hopeCheckBox_ImageGrabUse.Checked;
            Globalo.yamlManager.configData.DrivingSettings.IdleReportPass = checkBox_IdleReportPass.Checked;
            Globalo.yamlManager.configData.DrivingSettings.EnableAutoStartBcr = checkBox_BcrGo.Checked;

            //Serial Port
            Globalo.yamlManager.configData.SerialPort.Light = poisonComboBox_Light_Port.Text;
            Globalo.yamlManager.configData.DrivingSettings.Language = ComboBox_Language.Text;

            

        }
        public void ShowDriveSet()
        {
            string Handlerip = Globalo.yamlManager.configData.DrivingSettings.HandlerIp;
            string[] Handlerparts = Handlerip.Split('.');

            string Secsgemip = Globalo.yamlManager.configData.DrivingSettings.SecsgemIp;
            string[] Secsgemparts = Secsgemip.Split('.');

            label_Handler_Ip1.Text = Handlerparts[0];
            label_Handler_Ip2.Text = Handlerparts[1];
            label_Handler_Ip3.Text = Handlerparts[2];
            label_Handler_Ip4.Text = Handlerparts[3];

            label_Handler_Port.Text = Globalo.yamlManager.configData.DrivingSettings.HandlerPort.ToString();

            label_Secsgem_Ip1.Text = Secsgemparts[0];
            label_Secsgem_Ip2.Text = Secsgemparts[1];
            label_Secsgem_Ip3.Text = Secsgemparts[2];
            label_Secsgem_Ip4.Text = Secsgemparts[3];

            label_Secsgem_Port.Text = Globalo.yamlManager.configData.DrivingSettings.SecsgemPort.ToString();



            //bool setBool = Globalo.yamlManager.configData.DrivingSettings.IdleReportPass;
            //hopeCheckBox_PinCountUse.Checked = Globalo.yamlManager.configData.DrivingSettings.PinCountUse;
            //hopeCheckBox_ImageGrabUse.Checked = Globalo.yamlManager.configData.DrivingSettings.ImageGrabUse;
            //checkBox_IdleReportPass.Checked = Globalo.yamlManager.configData.DrivingSettings.IdleReportPass;
            //checkBox_BcrGo.Checked = Globalo.yamlManager.configData.DrivingSettings.EnableAutoStartBcr;

            //label_PinCountMax.Text = Globalo.yamlManager.configData.DrivingSettings.PinCountMax.ToString();
            //label_CsvScanMax.Text = Globalo.yamlManager.configData.DrivingSettings.CsvScanMonth.ToString();

            //label_Config_Tray_GapX_Val.Text = Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX.ToString("0.0##");
        }
        public void ShowComPort()
        {
            string comData = Globalo.yamlManager.configData.SerialPort.Light;
            int index = poisonComboBox_Light_Port.Items.IndexOf(comData);
            if (index < 0)
            {
                poisonComboBox_Light_Port.SelectedIndex = 0;  // 첫 번째 항목 선택
            }
            else
            {
                poisonComboBox_Light_Port.SelectedIndex = index;
            }
        }
        private void ShowLanguage()
        {
            string comData = Globalo.yamlManager.configData.DrivingSettings.Language;

            int index = ComboBox_Language.Items.IndexOf(comData);
            if (index < 0)
            {
                ComboBox_Language.SelectedIndex = 0;  // 첫 번째 항목 선택
            }
            else
            {
                ComboBox_Language.SelectedIndex = index;
            }

        }
        public void setInterface()
        {
            int i = 0;
            ManualTitleLabel.ForeColor = ColorTranslator.FromHtml("#6F6F6F");

            for (i = 0; i < 20; i++)
            {
                poisonComboBox_Light_Port.Items.Add("COM" + (i + 1).ToString());
            }

            ComboBox_Language.Items.Add("ko");
            ComboBox_Language.Items.Add("en");
            ComboBox_Language.Items.Add("es");
            ComboBox_Language.SelectedIndex = 0;
            //string selectedValue = poisonComboBox_BcrPort.SelectedItem.ToString();

        }
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            int lineStartY = ManualTitleLabel.Location.Y + Globalo.TabLineY;
            // Graphics 객체 가져오기
            Graphics g = e.Graphics;

            // Pen 객체 생성 (색상과 두께 설정)
            Color color = Color.FromArgb(175, 175, 175);//Color.FromArgb(151, 149, 145);
            Pen pen = new Pen(color, 1);

            // 라인 그리기 (시작점과 끝점 설정)
            g.DrawLine(pen, 0, lineStartY, this.Width, lineStartY);

            // 리소스 해제
            pen.Dispose();



        }
        
        private void ManualBtnChange(eManualBtn index)
        {

        }
        private void BTN_CONFIG_SAVE_Click(object sender, EventArgs e)
        {
            //Save
            GetConfigData();
            Globalo.yamlManager.configDataSave();

            //
            RefreshConfig();


            //언어 변경
            string comData = Globalo.yamlManager.configData.DrivingSettings.Language;
            Program.SetLanguage(comData);   
        }

        private void button_Bcr_Connect_Click(object sender, EventArgs e)
        {
            bool connectRtn = Globalo.serialPortManager.LightControl.Open();

            string logData = "";

            if (connectRtn)
            {
                logData = $"[SERIAL] BCR CONNECT OK:{Globalo.yamlManager.configData.SerialPort.Light}";
            }
            else
            {
                logData = $"[SERIAL] BCR CONNECT FAIL:{Globalo.yamlManager.configData.SerialPort.Light}";
            }

            Globalo.LogPrint("Serial", logData);
        }

        private void button_Bcr_DisConnect_Click(object sender, EventArgs e)
        {
            Globalo.serialPortManager.LightControl.Close();

            string logData = $"[SERIAL] BCR DISCONNECT";

            Globalo.LogPrint("Serial", logData);
        }

        private void ConfigControl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                RefreshConfig();
            }
        }

        private void label_PinCountMax_Click(object sender, EventArgs e)
        {
            string sValue = label_PinCountMax.Text;
            NumPadForm popupForm = new NumPadForm(sValue);

            DialogResult dialogResult = popupForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                if (popupForm.NumPadResult.Contains(".") == true || popupForm.NumPadResult.Length < 1)
                {
                    //Globalo.LogPrint("Recipe", "소수 점이 포함돼 있습니다.", Globalo.eMessageName.M_WARNING);
                    Globalo.LogPrint("Config", "입력이 값 확인바랍니다.", Globalo.eMessageName.M_WARNING);

                }
                else
                {
                    label_PinCountMax.Text = popupForm.NumPadResult;
                }
            }
        }

        private void label_CsvScanMax_Click(object sender, EventArgs e)
        {
            string sValue = label_CsvScanMax.Text;
            NumPadForm popupForm = new NumPadForm(sValue);

            DialogResult dialogResult = popupForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                if (popupForm.NumPadResult.Contains(".") == true || popupForm.NumPadResult.Length < 1)
                {
                    Globalo.LogPrint("Config", "입력이 값 확인바랍니다.", Globalo.eMessageName.M_WARNING);

                }
                else
                {
                    label_CsvScanMax.Text = popupForm.NumPadResult;
                }
            }
        }

        private void ProductSizeInput(Label OffsetLabel)
        {
            string labelValue = OffsetLabel.Text;
            decimal decimalValue = 0;


            if (decimal.TryParse(labelValue, out decimalValue))
            {
                // 소수점 형식으로 변환
                string formattedValue = decimalValue.ToString("0.0##");
                NumPadForm popupForm = new NumPadForm(formattedValue);

                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    double dNumData = Double.Parse(popupForm.NumPadResult);

                    OffsetLabel.Text = dNumData.ToString("0.#");
                }
            }
        }
        private void label_Config_Tray_GapX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }

        private void label_Config_Tray_GapY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }

        private void label_Config_Socket_GapX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }

        private void label_Config_Socket_GapY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }

        private void label_Config_Ng_GapX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }

        private void label_Config_Ng_GapY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            ProductSizeInput(clickedLabel);
        }


        private void tcpIpInput(Label label)
        {
            string labelValue = label.Text;
            int decimalValue = 0;


            if (int.TryParse(labelValue, out decimalValue))
            {
                // 소수점 형식으로 변환
                string formattedValue = decimalValue.ToString();
                NumPadForm popupForm = new NumPadForm(formattedValue);

                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    if (int.TryParse(popupForm.NumPadResult, out int dNumData))
                    {
                        if (dNumData < 0)
                        {
                            dNumData = 0;
                        }
                        if (dNumData > 255)
                        {
                            dNumData = 255;
                        }
                        label.Text = dNumData.ToString();
                    }

                }
            }
        }
        private void label_Handler_Ip1_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            tcpIpInput(clickedLabel);
        }
        private void label_Handler_Ip2_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            tcpIpInput(clickedLabel);
        }

        private void label_Handler_Ip3_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            tcpIpInput(clickedLabel);
        }
        private void label_Handler_Ip4_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            tcpIpInput(clickedLabel);
        }
        private void label_Handler_Port_Click(object sender, EventArgs e)
        {
            string labelValue = label_Handler_Port.Text;
            int decimalValue = 0;


            if (int.TryParse(labelValue, out decimalValue))
            {
                // 소수점 형식으로 변환
                string formattedValue = decimalValue.ToString();
                NumPadForm popupForm = new NumPadForm(formattedValue);

                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    if (int.TryParse(popupForm.NumPadResult, out int dNumData))
                    {
                        if (dNumData < 2000)
                        {
                            dNumData = 2000;
                        }
                        if (dNumData > 60000)
                        {
                            dNumData = 60000;
                        }
                        label_Handler_Port.Text = dNumData.ToString();
                    }

                }
            }
        }

        private void label_Secsgem_Ip1_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            tcpIpInput(clickedLabel);
        }

        private void label_Secsgem_Ip2_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            tcpIpInput(clickedLabel);
        }

        private void label_Secsgem_Ip3_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            tcpIpInput(clickedLabel);

        }

        private void label_Secsgem_Ip4_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            tcpIpInput(clickedLabel);
        }

        private void label_Secsgem_Port_Click(object sender, EventArgs e)
        {
            string labelValue = label_Secsgem_Port.Text;
            int decimalValue = 0;


            if (int.TryParse(labelValue, out decimalValue))
            {
                // 소수점 형식으로 변환
                string formattedValue = decimalValue.ToString();
                NumPadForm popupForm = new NumPadForm(formattedValue);

                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    if (int.TryParse(popupForm.NumPadResult, out int dNumData))
                    {
                        if (dNumData < 2000)
                        {
                            dNumData = 2000;
                        }
                        if (dNumData > 60000)
                        {
                            dNumData = 60000;
                        }
                        label_Secsgem_Port.Text = dNumData.ToString();
                    }

                }
            }
        }
    }
}
