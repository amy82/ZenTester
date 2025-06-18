using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenTester.Dlg
{
    public partial class ManualConfig : UserControl
    {
        private SetTestControl parentDlg;
        private Button[] TopLightButtons;                // Top 검사 조명 버튼 배열
        private Button[] SideLightButtons;               // Side 검사 조명 버튼 배열

        public int TopLIghtDataNo = 0;
        public int SideLIghtDataNo = 0;
        //private bool m_bDrawMeasureLine = false;

        //public int CamIndex = 0;
        //private int[] CamW = new int[2];
        //private int[] CamH = new int[2];
        //public List<Data.Roi> tempRoi = new List<Data.Roi>();
        //private System.Drawing.Point[,] DistLineX = new System.Drawing.Point[2, 2];
        //private int isRoiChecked = -1;
        //private int isRoiNo = -1;

        public ManualConfig(SetTestControl _parent)
        {
            InitializeComponent();
            parentDlg = _parent;

            TopLightButtons = new Button[] { label_SetTest_Manual_Top_Light_Val1, label_SetTest_Manual_Top_Light_Val2, label_SetTest_Manual_Top_Light_Val3 };
            SideLightButtons = new Button[] { label_SetTest_Manual_Side_Light_Val1 };

            //this.Set_panelCam.
            checkBox_Roi_Key.CheckedChanged += checkBox_CheckedChanged;
            checkBox_Roi_ORing.CheckedChanged += checkBox_CheckedChanged;
            checkBox_Roi_Cone.CheckedChanged += checkBox_CheckedChanged;
            checkBox_Roi_Height.CheckedChanged += checkBox_CheckedChanged;


            trackBar_Top_Light.Minimum = 0;
            trackBar_Top_Light.Maximum = 255;
            trackBar_Top_Light.Value = 0;
            trackBar_Top_Light.TickFrequency = 10;
            trackBar_Top_Light.Scroll += trackBar_Top_Light_Scroll;

            trackBar_Side_Light.Minimum = 0;
            trackBar_Side_Light.Maximum = 255;
            trackBar_Side_Light.Value = 0;
            trackBar_Side_Light.TickFrequency = 10;
            trackBar_Side_Light.Scroll += trackBar_Side_Light_Scroll;
        }
        public void checkBox_AllRelease()
        {
            


            if (this.InvokeRequired)
            {
                parentDlg.Invoke(new Action(() => parentDlg.isRoiChecked = -1));
                checkBox_Roi_Key.Invoke(new Action(() => checkBox_Roi_Key.Checked = false));
                checkBox_Roi_ORing.Invoke(new Action(() => checkBox_Roi_ORing.Checked = false));
                checkBox_Roi_Cone.Invoke(new Action(() => checkBox_Roi_Cone.Checked = false));
                checkBox_Roi_Height.Invoke(new Action(() => checkBox_Roi_Height.Checked = false));
            }
            else
            {
                parentDlg.isRoiChecked = -1;
                checkBox_Roi_Key.Checked = false;
                checkBox_Roi_ORing.Checked = false;
                checkBox_Roi_Cone.Checked = false;
                checkBox_Roi_Height.Checked = false;
            }
        }
        private void button_Set_Top_Resol_Save_Click(object sender, EventArgs e)
        {
            Globalo.yamlManager.configData.CamSettings.TopResolution.X = double.Parse(label_Set_TopCam_ResolX_Val.Text);
            Globalo.yamlManager.configData.CamSettings.TopResolution.Y = double.Parse(label_Set_TopCam_ResolY_Val.Text);
            Globalo.yamlManager.configData.CamSettings.KeyEdgeSpecCount = int.Parse(label_Set_TopCam_Key_EdgeCount_Val.Text);
            Globalo.yamlManager.configData.CamSettings.DentLimit = double.Parse(label_Set_TopCam_Dent_Limit_Val.Text);
            Globalo.yamlManager.configData.CamSettings.DentTotalCount = int.Parse(label_Set_TopCam_Dent_Count_Val.Text);


            Globalo.yamlManager.configDataSave();
            if (checkBox_Measure.Checked)
            {
                DrawDistnace();
            }
        }
        public void ClearCheckbox()
        {
            checkBox_Roi_Key.Checked = false;
            checkBox_Roi_ORing.Checked = false;
            checkBox_Roi_Cone.Checked = false;
            checkBox_Roi_Height.Checked = false;

        }
        public void showCamResol()
        {
            label_Set_TopCam_ResolX_Val.Text = Globalo.yamlManager.configData.CamSettings.TopResolution.X.ToString();
            label_Set_TopCam_ResolY_Val.Text = Globalo.yamlManager.configData.CamSettings.TopResolution.Y.ToString();

            label_Set_SideCam_ResolX_Val.Text = Globalo.yamlManager.configData.CamSettings.SideResolution.X.ToString();
            label_Set_SideCam_ResolY_Val.Text = Globalo.yamlManager.configData.CamSettings.SideResolution.Y.ToString();

            label_Set_TopCam_Key_EdgeCount_Val.Text = Globalo.yamlManager.configData.CamSettings.KeyEdgeSpecCount.ToString();
            label_Set_TopCam_Dent_Limit_Val.Text = Globalo.yamlManager.configData.CamSettings.DentLimit.ToString("0.0");
            label_Set_TopCam_Dent_Count_Val.Text = Globalo.yamlManager.configData.CamSettings.DentTotalCount.ToString();


        }
        public void showLight()
        {
            label_SetTest_Manual_Top_Light_Data.Text = Globalo.yamlManager.aoiRoiConfig.topLightData[TopLIghtDataNo].data.ToString();
            label_SetTest_Manual_Side_Light_Data.Text = Globalo.yamlManager.aoiRoiConfig.sideLightData[SideLIghtDataNo].data.ToString();

            trackBar_Top_Light.Value = Globalo.yamlManager.aoiRoiConfig.topLightData[TopLIghtDataNo].data;
            trackBar_Side_Light.Value = Globalo.yamlManager.aoiRoiConfig.sideLightData[SideLIghtDataNo].data;
        }
        private void button_Set_Side_Resol_Save_Click(object sender, EventArgs e)
        {
            Globalo.yamlManager.configData.CamSettings.SideResolution.X = double.Parse(label_Set_SideCam_ResolX_Val.Text);
            Globalo.yamlManager.configData.CamSettings.SideResolution.Y = double.Parse(label_Set_SideCam_ResolY_Val.Text);

            Globalo.yamlManager.configDataSave();
            if (checkBox_Measure.Checked)
            {
                DrawDistnace();
            }
        }
        private void trackBar_Top_Light_Scroll(object sender, EventArgs e)
        {
            int currentValue = ((TrackBar)sender).Value;
            TopLightChange(1, TopLIghtDataNo, currentValue);
        }
        private void trackBar_Side_Light_Scroll(object sender, EventArgs e)
        {
            int currentValue = ((TrackBar)sender).Value;
            SideLightChange(2, SideLIghtDataNo, currentValue);
        }
        public void drawTestRoi(int index)
        {
            Data.Roi targetRoi;
            Rectangle m_clRect;
            System.Drawing.Point textPoint;
            Globalo.visionManager.milLibrary.ClearOverlay(index);

            int boxLine = 1;
            if (index == 0)
            {
                targetRoi = parentDlg.tempRoi[0];// Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[0];
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, "LH ROI", Color.BlueViolet, 15);

                targetRoi = parentDlg.tempRoi[1];//Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[1];
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, "CH ROI", Color.BlueViolet, 15);

                targetRoi = parentDlg.tempRoi[2];//Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[2];
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, "RH ROI", Color.BlueViolet, 15);
            }
            if (index == 1)
            {
                targetRoi = parentDlg.tempRoi[0];//Globalo.yamlManager.aoiRoiConfig.CONE_ROI.FirstOrDefault(r => r.name == Data.NO_ROI.CONE.ToString());
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, "CONE ROI", Color.BlueViolet, 15);
            }
            if (index == 2)
            {
                targetRoi = parentDlg.tempRoi[0];//Globalo.yamlManager.aoiRoiConfig.ORING_ROI.FirstOrDefault(r => r.name == Data.NO_ROI.ORING.ToString());
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, "ORING ROI", Color.BlueViolet, 15);
            }
            if (index == 3)
            {
                targetRoi = parentDlg.tempRoi[0];//Globalo.yamlManager.aoiRoiConfig.KEY_ROI.FirstOrDefault(r => r.name == Data.NO_ROI.KEY1.ToString());

                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, "KEY1 ROI", Color.BlueViolet, 15);

                targetRoi = parentDlg.tempRoi[1];//Globalo.yamlManager.aoiRoiConfig.KEY_ROI.FirstOrDefault(r => r.name == Data.NO_ROI.KEY2.ToString());
                m_clRect = new Rectangle((int)(targetRoi.X), (int)(targetRoi.Y), targetRoi.Width, targetRoi.Height);
                Globalo.visionManager.milLibrary.DrawOverlayBox(index, m_clRect, Color.Blue, boxLine);
                textPoint = new System.Drawing.Point(targetRoi.X, targetRoi.Y - 100);
                Globalo.visionManager.milLibrary.DrawOverlayText(index, textPoint, "KEY2 ROI", Color.BlueViolet, 15);
            }
        }
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox changed = sender as CheckBox;
            parentDlg.isRoiChecked = -1;
            if (changed.Checked)
            {
                parentDlg.tempRoi.Clear();
                // 모든 체크박스를 순회하면서
                foreach (var ctrl in this.Controls)
                {
                    if (ctrl is CheckBox cb && cb != changed)
                    {
                        cb.Checked = false;
                    }
                }

                Console.WriteLine($"{changed.Name} Checked");

                if (changed.Name == "checkBox_Roi_Height")
                {
                    parentDlg.isRoiChecked = 0;

                    parentDlg.tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[0].Clone());
                    parentDlg.tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[1].Clone());
                    parentDlg.tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[2].Clone());

                    drawTestRoi(parentDlg.isRoiChecked);
                }


                if (changed.Name == "checkBox_Roi_Cone")
                {
                    parentDlg.isRoiChecked = 1;

                    parentDlg.tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.CONE_ROI[0].Clone());
                    drawTestRoi(parentDlg.isRoiChecked);
                }
                if (changed.Name == "checkBox_Roi_ORing")
                {
                    parentDlg.isRoiChecked = 2;
                    parentDlg.tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.ORING_ROI[0].Clone());
                    drawTestRoi(parentDlg.isRoiChecked);
                }
                if (changed.Name == "checkBox_Roi_Key")
                {
                    parentDlg.isRoiChecked = 3;
                    parentDlg.tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.KEY_ROI[0].Clone());
                    parentDlg.tempRoi.Add(Globalo.yamlManager.aoiRoiConfig.KEY_ROI[1].Clone());
                    drawTestRoi(parentDlg.isRoiChecked);
                }

            }
            if (parentDlg.isRoiChecked < 0)
            {
                Globalo.visionManager.milLibrary.ClearOverlay(0);
            }
        }
        private void CamResolutionInput(Label OffsetLabel)
        {
            string labelValue = OffsetLabel.Text;
            decimal decimalValue = 0;


            if (decimal.TryParse(labelValue, out decimalValue))
            {
                // 소수점 형식으로 변환
                string formattedValue = decimalValue.ToString("0.000###");
                NumPadForm popupForm = new NumPadForm(formattedValue, false);

                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    double dNumData = Double.Parse(popupForm.NumPadResult);

                    OffsetLabel.Text = dNumData.ToString("0.000###");
                }
            }
        }
        private void label_Set_TopCam_ResolX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            CamResolutionInput(clickedLabel);
        }

        private void label_Set_TopCam_ResolY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            CamResolutionInput(clickedLabel);
        }

        private void label_Set_SideCam_ResolX_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            CamResolutionInput(clickedLabel);
        }

        private void label_Set_SideCam_ResolY_Val_Click(object sender, EventArgs e)
        {
            Label clickedLabel = sender as Label;
            CamResolutionInput(clickedLabel);
        }
        private void button_Set_Roi_Save_Click(object sender, EventArgs e)
        {
            int i = 0;
            if (parentDlg.isRoiChecked == 0)      //Height
            {
                for (i = 0; i < Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI.Count; i++)
                {
                    Globalo.yamlManager.aoiRoiConfig.HEIGHT_ROI[i] = parentDlg.tempRoi[i].Clone();
                }
            }
            else if (parentDlg.isRoiChecked == 1)      //Cone
            {
                for (i = 0; i < Globalo.yamlManager.aoiRoiConfig.CONE_ROI.Count; i++)
                {
                    Globalo.yamlManager.aoiRoiConfig.CONE_ROI[i] = parentDlg.tempRoi[i].Clone();
                }
            }
            else if (parentDlg.isRoiChecked == 2)      //Oring
            {
                for (i = 0; i < Globalo.yamlManager.aoiRoiConfig.ORING_ROI.Count; i++)
                {
                    Globalo.yamlManager.aoiRoiConfig.ORING_ROI[i] = parentDlg.tempRoi[i].Clone();
                }
            }
            else if (parentDlg.isRoiChecked == 3)      //Key
            {
                for (i = 0; i < Globalo.yamlManager.aoiRoiConfig.KEY_ROI.Count; i++)
                {
                    Globalo.yamlManager.aoiRoiConfig.KEY_ROI[i] = parentDlg.tempRoi[i].Clone();
                }
            }
            Data.TaskDataYaml.Save_AoiConfig();     //Roi Set Save
        }
        private void checkBox_Measure_CheckedChanged(object sender, EventArgs e)
        {
            parentDlg.m_bDrawMeasureLine = false;
            if (checkBox_Measure.Checked)
            {
                parentDlg.isRoiChecked = -1;
                parentDlg.isRoiNo = -1;
                parentDlg.m_bDrawMeasureLine = true;
                DrawDistnace();
            }
            else
            {
                drawCenterCross();
            }
        }
        public void drawCenterCross()
        {
            Globalo.visionManager.milLibrary.ClearOverlay(0);
            int cx = Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex];
            int cy = Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex];
            Globalo.visionManager.milLibrary.DrawOverlayCross(0, cx / 2, cy / 2, 1000, Color.Yellow, 1, System.Drawing.Drawing2D.DashStyle.Solid);
        }
        public void DrawDistnace()
        {
            Globalo.visionManager.milLibrary.ClearOverlay(parentDlg.CamIndex);

            //DistLineX[0] = new System.Drawing.Point(500, 500);
            //DistLineX[1] = new System.Drawing.Point(sizeX - 500, sizeY - 500);

            Globalo.visionManager.milLibrary.ClearOverlay(parentDlg.CamIndex);
            Globalo.visionManager.milLibrary.DrawOverlayLine(0, (int)(parentDlg.DistLineX[parentDlg.CamIndex, 0].X), 0, (int)(parentDlg.DistLineX[parentDlg.CamIndex, 0].X), (int)Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex], Color.Red, 1);
            Globalo.visionManager.milLibrary.DrawOverlayLine(0, (int)(parentDlg.DistLineX[parentDlg.CamIndex, 1].X), 0, (int)(parentDlg.DistLineX[parentDlg.CamIndex, 1].X), (int)Globalo.visionManager.milLibrary.CAM_SIZE_Y[parentDlg.CamIndex], Color.Red, 1);

            Globalo.visionManager.milLibrary.DrawOverlayLine(0, 0, (int)(parentDlg.DistLineX[parentDlg.CamIndex, 0].Y), (int)Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex], (int)(parentDlg.DistLineX[parentDlg.CamIndex, 0].Y), Color.Blue, 1);
            Globalo.visionManager.milLibrary.DrawOverlayLine(0, 0, (int)(parentDlg.DistLineX[parentDlg.CamIndex, 1].Y), (int)Globalo.visionManager.milLibrary.CAM_SIZE_X[parentDlg.CamIndex], (int)(parentDlg.DistLineX[parentDlg.CamIndex, 1].Y), Color.Blue, 1);

            double CamResolX = 0.0;
            double CamResolY = 0.0;
            //CamResolX = Globalo.yamlManager.aoiRoiConfig.SideResolution.X;   // 0.02026f;
            //CamResolY = Globalo.yamlManager.aoiRoiConfig.SideResolution.Y;   //0.02026f;//0.0288f;
            CamResolX = Globalo.yamlManager.configData.CamSettings.TopResolution.X;   // 0.02026f;
            CamResolY = Globalo.yamlManager.configData.CamSettings.TopResolution.Y;   //0.02026f;//0.0288f;

            Console.WriteLine($"CamResolX:{CamResolX}");
            Console.WriteLine($"CamResolY:{CamResolY}");
            //
            System.Drawing.Point textPoint;

            string str = $"[Distance x:{Math.Abs(parentDlg.DistLineX[parentDlg.CamIndex, 0].X - parentDlg.DistLineX[parentDlg.CamIndex, 1].X) * CamResolX}";
            textPoint = new System.Drawing.Point(10, parentDlg.CamH[parentDlg.CamIndex] - 250);
            Globalo.visionManager.milLibrary.DrawOverlayText(parentDlg.CamIndex, textPoint, str, Color.Blue, 15);

            str = $"[Distance y:{Math.Abs(parentDlg.DistLineX[parentDlg.CamIndex, 0].Y - parentDlg.DistLineX[parentDlg.CamIndex, 1].Y) * CamResolY}";
            textPoint = new System.Drawing.Point(10, parentDlg.CamH[parentDlg.CamIndex] - 150);
            Globalo.visionManager.milLibrary.DrawOverlayText(parentDlg.CamIndex, textPoint, str, Color.Blue, 15);

        }
        private void label_SetTest_Manual_Top_Light_Data_Click(object sender, EventArgs e)
        {
            string formattedValue = label_SetTest_Manual_Top_Light_Data.Text;
            NumPadForm popupForm = new NumPadForm(formattedValue);

            DialogResult dialogResult = popupForm.ShowDialog();


            if (dialogResult == DialogResult.OK)
            {
                int dNumData = int.Parse(popupForm.NumPadResult);
                if (dNumData < 0)
                {
                    dNumData = 0;
                }
                if (dNumData > 255)
                {
                    dNumData = 255;
                }
                TopLightChange(1, TopLIghtDataNo, dNumData);
            }
        }
        private void label_SetTest_Manual_Side_Light_Data_Click(object sender, EventArgs e)
        {
            string formattedValue = label_SetTest_Manual_Side_Light_Data.Text;
            NumPadForm popupForm = new NumPadForm(formattedValue);

            DialogResult dialogResult = popupForm.ShowDialog();


            if (dialogResult == DialogResult.OK)
            {
                int dNumData = int.Parse(popupForm.NumPadResult);
                if (dNumData < 0)
                {
                    dNumData = 0;
                }
                if (dNumData > 255)
                {
                    dNumData = 255;
                }
                SideLightChange(2, SideLIghtDataNo, dNumData);
            }
        }
        private void label_SetTest_Manual_Light_Val1_Click(object sender, EventArgs e)
        {
            int data = Globalo.yamlManager.aoiRoiConfig.topLightData[0].data;
            TopLightChange(1, 0, data);
        }

        private void label_SetTest_Manual_Light_Val2_Click(object sender, EventArgs e)
        {
            int data = Globalo.yamlManager.aoiRoiConfig.topLightData[1].data;
            TopLightChange(1, 1, data);
        }

        private void label_SetTest_Manual_Light_Val3_Click(object sender, EventArgs e)
        {
            int data = Globalo.yamlManager.aoiRoiConfig.topLightData[2].data;
            TopLightChange(1, 2, data);
        }

        private void label_SetTest_Manual_Top_Light_Val4_Click(object sender, EventArgs e)
        {
            int data = Globalo.yamlManager.aoiRoiConfig.topLightData[3].data;
            TopLightChange(1, 3, data);
        }

        private void label_SetTest_Manual_Top_Light_Val5_Click(object sender, EventArgs e)
        {
            int data = Globalo.yamlManager.aoiRoiConfig.topLightData[4].data;
            TopLightChange(1, 4, data);
        }
        private void label_SetTest_Manual_Side_Light_Val1_Click(object sender, EventArgs e)
        {
            int data = Globalo.yamlManager.aoiRoiConfig.sideLightData[0].data;
            SideLightChange(2, 0, data);
        }

        private void label_SetTest_Manual_Side_Light_Val2_Click(object sender, EventArgs e)
        {
            int data = Globalo.yamlManager.aoiRoiConfig.sideLightData[1].data;
            SideLightChange(2, 1, data);
        }

        private void label_SetTest_Manual_Side_Light_Val3_Click(object sender, EventArgs e)
        {
            int data = Globalo.yamlManager.aoiRoiConfig.sideLightData[2].data;
            SideLightChange(2, 2, data);
        }
        private void label_SetTest_Manual_Side_Light_Val4_Click(object sender, EventArgs e)
        {
            int data = Globalo.yamlManager.aoiRoiConfig.sideLightData[3].data;
            SideLightChange(2, 3, data);
        }

        private void label_SetTest_Manual_Side_Light_Val5_Click(object sender, EventArgs e)
        {
            int data = Globalo.yamlManager.aoiRoiConfig.sideLightData[4].data;
            SideLightChange(2, 4, data);
        }
        private void TopLightChange(int channel, int NoData, int data)
        {
            Globalo.serialPortManager.LightControl.ctrlLedVolume(channel, data);
            //
            label_SetTest_Manual_Top_Light_Data.Text = data.ToString();
            TopLIghtDataNo = NoData;
            trackBar_Top_Light.Value = data;


            for (int i = 0; i < TopLightButtons.Length; i++)
            {
                if (i == NoData)
                {
                    TopLightButtons[i].ForeColor = Color.Black;
                    TopLightButtons[i].BackColor = Color.Gold;
                }
                else
                {
                    TopLightButtons[i].ForeColor = Color.White;
                    TopLightButtons[i].BackColor = Color.Tan;
                }
            }
        }

        private void SideLightChange(int channel, int NoData, int data)      //사이드는 채널 2개다
        {
            Globalo.serialPortManager.LightControl.ctrlLedVolume(channel, data);


            //
            label_SetTest_Manual_Side_Light_Data.Text = data.ToString();
            SideLIghtDataNo = NoData;
            trackBar_Side_Light.Value = data;


            for (int i = 0; i < SideLightButtons.Length; i++)
            {
                if (i == NoData)
                {
                    SideLightButtons[i].ForeColor = Color.Black;
                    SideLightButtons[i].BackColor = Color.Gold;
                }
                else
                {
                    SideLightButtons[i].ForeColor = Color.White;
                    SideLightButtons[i].BackColor = Color.Tan;
                }
            }
        }

        private void getLightData(int LightCh)
        {
            int lightData = 0;
            if (LightCh == 0)
            {
                //0 = Top Light
                lightData = int.Parse(label_SetTest_Manual_Top_Light_Data.Text);

                Globalo.yamlManager.aoiRoiConfig.topLightData[TopLIghtDataNo].data = lightData;
            }
            else
            {
                //1 = Top Light
                lightData = int.Parse(label_SetTest_Manual_Side_Light_Data.Text);

                Globalo.yamlManager.aoiRoiConfig.sideLightData[SideLIghtDataNo].data = lightData;
            }
            Data.TaskDataYaml.Save_AoiConfig();


        }
        private void label_SetTest_Manual_Top_Cam_Save_Click(object sender, EventArgs e)
        {
            //Top Camera Light Save
            getLightData(0);


            Globalo.serialPortManager.LightControl.reqLightVal();
        }

        private void label_SetTest_Manual_Side_Light_Save_Click(object sender, EventArgs e)
        {
            //Side Camera Light Save
            getLightData(1);
            Globalo.serialPortManager.LightControl.reqLightVal();
        }

        private void ManualConfig_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                showCamResol();
                showLight();
                drawCenterCross();
            }
            else
            {
                checkBox_Measure.Checked = false;
                ClearCheckbox();
                parentDlg.m_bDrawMeasureLine = false;
                parentDlg.isRoiChecked = -1;
                parentDlg.isRoiNo = -1;
            }
        }
        public void hidePanel()
        {

        }

        private void label_Set_TopCam_Key_EdgeCount_Val_Click(object sender, EventArgs e)
        {
            string labelValue = label_Set_TopCam_Key_EdgeCount_Val.Text;
            decimal decimalValue = 0;


            string formattedValue = label_SetTest_Manual_Top_Light_Data.Text;
            NumPadForm popupForm = new NumPadForm(formattedValue);

            DialogResult dialogResult = popupForm.ShowDialog();


            if (dialogResult == DialogResult.OK)
            {
                int dNumData = int.Parse(popupForm.NumPadResult);
                if (dNumData < 1)
                {
                    dNumData = 1;
                }
                if (dNumData > 250)
                {
                    dNumData = 250;
                }
                label_Set_TopCam_Key_EdgeCount_Val.Text = dNumData.ToString();
            }

        }

        private void label_Set_TopCam_Dent_Limit_Val_Click(object sender, EventArgs e)
        {
            string labelValue = label_Set_TopCam_Dent_Limit_Val.Text;
            decimal decimalValue = 0;


            if (decimal.TryParse(labelValue, out decimalValue))
            {
                // 소수점 형식으로 변환
                string formattedValue = decimalValue.ToString("0.0");
                NumPadForm popupForm = new NumPadForm(formattedValue, false);

                DialogResult dialogResult = popupForm.ShowDialog();


                if (dialogResult == DialogResult.OK)
                {
                    double dNumData = Double.Parse(popupForm.NumPadResult);

                    label_Set_TopCam_Dent_Limit_Val.Text = dNumData.ToString("0.0");
                }
            }
        }

        private void label_Set_TopCam_Dent_Count_Val_Click(object sender, EventArgs e)
        {
            string labelValue = label_Set_TopCam_Dent_Count_Val.Text;
            decimal decimalValue = 0;


            string formattedValue = label_SetTest_Manual_Top_Light_Data.Text;
            NumPadForm popupForm = new NumPadForm(formattedValue);

            DialogResult dialogResult = popupForm.ShowDialog();


            if (dialogResult == DialogResult.OK)
            {
                int dNumData = int.Parse(popupForm.NumPadResult);
                if (dNumData < 100)
                {
                    dNumData = 100;
                }
                if (dNumData > 500)
                {
                    dNumData = 500;
                }
                label_Set_TopCam_Dent_Count_Val.Text = dNumData.ToString();
            }
        }
    }
}
