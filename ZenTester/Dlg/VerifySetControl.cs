using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenTester.Dlg
{
    public partial class VerifySetControl : UserControl
    {
        public VerifySetControl()
        {
            InitializeComponent();
        }

        private void button_FdSet_ConfRead_Click(object sender, EventArgs e)
        {
            //Globalo.FxaBoardManager.fxaFirmwardDw.getFirmwareFileName();
            string txtpath = Globalo.FxaBoardManager.fxaEEpromVerify.gettxtFilePath();

            string szLog = $"[VERIFY] TXT PATH : {txtpath}";
            Globalo.LogPrint("ManualControl", szLog);
        }

        private void button_VSet_Crc_Cal_Click(object sender, EventArgs e)
        {

        }

        private void button_VSet_Run_Click(object sender, EventArgs e)
        {
            //D125227T2100059_P1656620-0R-B-SLGM250230D00169_2025040119
            //호출시 : 세미콜론으로 호출
            Globalo.FxaBoardManager.fxaEEpromVerify.RunEEPROMVerifycation("P1656620-0R-B:SLGM250230D00169", "D125227T2100059");


            /*
            // Verification
            EEPRROM_LAST_UPDATED_UTC
            MANUFACTURED_TIMESTAMP_UTC
            IMAGER_UUID
            CHECKSUM_EEPROM_VERSION          - apd0
            CHECKSUM_IMAGER_INFO             - apd1
            CHECKSUM_PART_INFO               - apd2
            CHECKSUM_INTRINSIC_PARAMETER     - apd3
            CHECKSUM_LSC                     - apd4
            CHECKSUM_FLAG_BLOCK
            CHECKSUM_TBD1_BLOCK
            CHECKSUM_TBD2_BLOCK

             */
        }

        private void button_VSet_Path_Set_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "실행 파일을 선택하세요";
                ofd.Filter = "실행 파일 (*.exe)|*.exe"; // ✅ exe만 표시
                ofd.CheckFileExists = true;             // ✅ 실제 존재하는 파일만 선택 가능
                ofd.Multiselect = false;                // 다중 선택 금지 (원하면 true로 변경)
                ofd.InitialDirectory = @button_VSet_Path_Val.Text;// "D:\EVMS\TP\ENV"; // ✅ 초기 폴더 지정


                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = ofd.FileName;
                    string selectedFolderPath = Path.GetDirectoryName(selectedFilePath);

                    string teslaExeName = "ThunderEEPROMVerificationTool.exe";      //Verify 공정



                    string targetFile = Path.Combine(selectedFolderPath, teslaExeName);  // 찾을 파일 이름

                    if (File.Exists(targetFile))
                    {
                        button_VSet_Path_Val.Text = selectedFolderPath;
                        Console.WriteLine($"Verify exe File exists: {targetFile}");

                        Globalo.yamlManager.configData.TeslaData.Fpath = selectedFolderPath;
                        Globalo.yamlManager.configData.TeslaData.FexeName = teslaExeName;

                        Globalo.yamlManager.configDataSave();
                    }
                    else
                    {
                        Console.WriteLine($"Verify exe No file: {targetFile}");
                    }
                    ///MessageBox.Show("선택한 exe 경로:\n" + selectedFilePath +"\n\nexe가 있는 폴더:\n" + selectedFolderPath);
                }
            }
        }
    }
}
