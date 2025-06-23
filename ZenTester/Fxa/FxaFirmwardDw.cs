using Newtonsoft.Json;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenTester.Fxa
{
    
    public class FxaFirmwardDw
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public const string strConfINIPath = @"D:\EVMS\\TP\\ENV\\fwexe\\TeslaEXE\\Tesla_FW_exe\\Trinity_FW_Download_20250421_1111";
        public FxaFirmwardDw()
        {

            //1.FirmwareDownLoadForCamAsync 
            //2.결과 확인
            //3.json 파일이 passed , fail 확인
            //4. json 파일 내용
        }

        public void test111()
        {
            //lot앞에 CAM1_붙여주기
            string lot1 = "CAM1_P1637042-00-C-SLGM250434C00283";
            string lot2 = "";
            string lot3 = "";
            string lot4 = "CAM3_P1637042-00-C-SLGM250434C00283";

            string result = FirmwareDownLoadForCamAsync(lot1, lot2, lot3, lot4); //CAM1 = 포트 그 뒤엔 BCR ":" -> "-" 으로 변경해서 넣어야 함 save파일명
            //result 결과 나오면 json 읽기
            //apd 보고

            Globalo.LogPrint("FW : ", result, Globalo.eMessageName.M_INFO);
            //Globalo.gFXABoard.ReadFirmware(); //펌웨어 read
        }

        private void test222()      //fw 다운로드 완료시 생성되는 json 파일 불러오는 함수
        {
            var result = ReadJsonResult("D:\\tmp\\PASSED_CAM1_1656620-0L-B-SLGM250230D00158.json"); //경로\\제품 BCR 

            // 예: 모든 항목 출력
            foreach (var param in result.parameters)
            {
                //Console.WriteLine($"[{param.result.ToUpper()}] {param.name} = {param.value}");
                string logMsg = $"[{param.result.ToUpper()}] {param.name} = {param.value}";
                Globalo.LogPrint("FW Upload 결과", logMsg);
            }
        }
        public static TestResult ReadJsonResult(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException("JSON 파일을 찾을 수 없습니다.", jsonFilePath);
            }

            string jsonContent = File.ReadAllText(jsonFilePath);
            var result = JsonConvert.DeserializeObject<TestResult>(jsonContent);

            return result;
        }
        public string FirmwareDownLoadForCamAsync(string lotId1, string lotId2 = "", string lotId3 = "", string lotId4 = "")
        {
            //FXA 보드에 업로드 되어있는 펌웨어 CAM 으로 다운로드 

            const string exeName = "cypress_cam_flashing"; // 확장자 없이
            //const string workingDir = @"D:\PG\FXABoard\TeslaEXE\Tesla_FW_exe\Trinity_FW_Download_20250421_1111";

            const string workingDir = @"D:\EVMS\TP\ENV\fwexe\TeslaEXE\Tesla_FW_exe\Trinity_FW_Download_20250421_1111";
            const string exePath = workingDir + @"\cypress_cam_flashing.exe";
            const string host = "127.0.0.1";
            const int port = 5000;
            //D:\EVMS\TP\ENV\fwexe\TeslaEXE\Tesla_FW_exe\Trinity_FW_Download_20250421_1111
            try
            {
                // EXE가 실행 중인지 확인
                if (System.Diagnostics.Process.GetProcessesByName(exeName).Length == 0)
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c " + Path.GetFileName(exePath),
                        WorkingDirectory = workingDir,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    System.Diagnostics.Process.Start(psi);

                    Thread.Sleep(100);
                }


                string command = $"$H,01,{lotId1},02,{lotId2},03,{lotId3},04,{lotId4}";


                using (TcpClient client = new TcpClient(host, port))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.ASCII.GetBytes(command + "\r\n");
                    stream.Write(data, 0, data.Length);

                    byte[] buffer = new byte[1024];
                    int bytes = stream.Read(buffer, 0, buffer.Length);

                    return Encoding.ASCII.GetString(buffer, 0, bytes).Trim();
                }
            }
            catch (Exception ex)
            {
                return "에러: " + ex.Message;
            }
            // 버전 추출 및 INI 기록
            //string[] split = strFile.Split('_');
            //string strVersion = split.Length > 1 ? split[1].ToLower() : "unknown";

            //WritePrivateProfileString("DEFAULT", "FIRMWARE_VERSION", strVersion, iniPath);
            //WritePrivateProfileString("DEFAULT", "FIRMWARE_FILE", strFile, iniPath);
        }
        public bool sFtpUploadFile(string newFileName)
        {
            bool bCheckFile = false;
            var ci = new ConnectionInfo("192.168.90.120", "root", new PasswordAuthenticationMethod("root", "root"));
            string downFileName = "";
            using (var sftp = new SftpClient(ci))
            {
                sftp.Connect();

                using (var infile = System.IO.File.OpenRead(downFileName))  // openFileDlg.FileName))
                {
                    sftp.UploadFile(infile, "/home/root/utils/flashing/MCU/Common/" + newFileName);// strFile);
                }

                

                foreach (SftpFile f in sftp.ListDirectory("/home/root/utils/flashing/MCU/Common/"))
                {
                    if (f.Name == newFileName)//strFile)
                    {
                        bCheckFile = true;
                        //MessageBox.Show("Upload Success!");
                        Console.WriteLine("Upload Success!");
                        break;
                    }
                }

                sftp.Disconnect();

                setFirmwareFileName(newFileName);

            }
            return bCheckFile;
        }
        public string getFirmwareFileName()
        {
            StringBuilder fwFileName = new StringBuilder(256);
            string rtnFwName = string.Empty;

            string sourcePath = Path.Combine(strConfINIPath, "conf.ini");
            GetPrivateProfileString("DEFAULT", "FIRMWARE_FILE", "", fwFileName, fwFileName.Capacity, sourcePath);
            rtnFwName = fwFileName.ToString();
            return rtnFwName;
        }

        public bool chkfwExeFileCheck(string fineName)      //펌웨어 다운로드 프로그램 파일 유무 체크
        {
            bool bCheck = true;
            string sourcePath = Path.Combine(strConfINIPath, fineName);     //exe 파일 유무 확인
            bCheck = File.Exists(sourcePath);

            return bCheck;
        }
        public bool setFirmwareFileName(string strFile)
        {
            // 파일명에서 펌웨어 버전 추출
            string[] seperate = strFile.Split('_');
            string strVersion = seperate[1].ToLower();

            // INI 파일에 펌웨어 버전 저장
            WritePrivateProfileString("DEFAULT", "FIRMWARE_VERSION", strVersion, strConfINIPath);

            // INI 파일에 펌웨어 파일명 저장
            WritePrivateProfileString("DEFAULT", "FIRMWARE_FILE", strFile, strConfINIPath);

            //MessageBox.Show("Ready to FW Download!");
            Console.WriteLine("Ready to FW Download!");

            return true;

        }
        private void button3_Click()      //conf.ini 내용 확인
        {
            // Upload FW file to FXA board - Trinity
            string strINIPath = "D:\\conf.ini"; // INI 파일 경로

            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.DefaultExt = ".cyacd";
            openFileDlg.Filter = "CYACD File(*.cyacd)|*.cyacd"; // Opal의 경우 FW 파일 확장자가 .hex 임
            openFileDlg.ShowDialog();
            if (openFileDlg.FileName.Length > 0)
            {
                //this.textBox1.Text = openFileDlg.FileName;

                String strFile = Path.GetFileName(openFileDlg.FileName);
                StringBuilder fwFileName = new StringBuilder();

                GetPrivateProfileString("DEFAULT", "FIRMWARE_FILE", "", fwFileName, fwFileName.Capacity, strINIPath);

                if (strFile == fwFileName.ToString())
                {
                    // 현재 동일한 펌웨어 파일이 적용되어 있으므로 업데이트 불필요함
                    return;
                }

                // 펌웨어 파일명이 다를 경우 MES에서 펌웨어 파일을 다운로드

                // SFTP를 통해 FXA보드에 펌웨어 파일 복사
                var ci = new ConnectionInfo("192.168.90.120", "root", new PasswordAuthenticationMethod("root", "root"));

                using (var sftp = new SftpClient(ci))
                {
                    sftp.Connect();

                    using (var infile = System.IO.File.OpenRead(openFileDlg.FileName))
                    {
                        sftp.UploadFile(infile, "/home/root/utils/flashing/MCU/Common/" + strFile);
                    }

                    bool bCheckFile = false;

                    foreach (SftpFile f in sftp.ListDirectory("/home/root/utils/flashing/MCU/Common/"))
                    {
                        if (f.Name == strFile)
                        {
                            bCheckFile = true;
                            MessageBox.Show("Upload Success!");
                            break;
                        }
                    }

                    if (bCheckFile == false)
                    {
                        // 펌웨어 파일 업로드 실패, 재시도
                    }

                    sftp.Disconnect();
                }

                // 파일명에서 펌웨어 버전 추출
                string[] seperate = strFile.Split('_');
                string strVersion = seperate[1].ToLower();

                // INI 파일에 펌웨어 버전 저장
                WritePrivateProfileString("DEFAULT", "FIRMWARE_VERSION", strVersion, strINIPath);

                // INI 파일에 펌웨어 파일명 저장
                WritePrivateProfileString("DEFAULT", "FIRMWARE_FILE", strFile, strINIPath);

                MessageBox.Show("Ready to FW Download!");
            }
        }
    }
}
