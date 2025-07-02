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

        public string strConfINIPath = @"D:\EVMS\TP\ENV\fwexe\TeslaEXE\Tesla_FW_exe\Trinity_FW_Download_20250421_1111";

        public FxaFirmwardDw()
        {

            //1.FirmwareDownLoadForCamAsync

                    //2.결과 확인
                    //3.json 파일이 passed , fail 확인
                    //4. json 파일 내용
        }

        public void Manual_Fw_DownLoad()
        {
            int i = 0;
            //1. 스페셜 DATA에서 받은 파일명하고
            //conf.ini에서 FIRMWARE_FILE = TRI_0xA2_interrupt_fix.cyacd  파일명하고 비교해야된다.
            Globalo.LogPrint("fw :", "Firmware Download Start");

            string strLog = string.Empty;
            //lot앞에 CAM1_붙여주기

            // string[] lotarr = { "CAM1_P1637042-00-C-SLGM250434C00283","", "", "" };

            string[] lotarr = { "CAM1_P1637042-00-C-SLGM250434C00283", "", "", "" };
            string result = FirmwareDownLoadForCamAsync(lotarr); //CAM1 = 포트 그 뒤엔 BCR ":" -> "-" 으로 변경해서 넣어야 함 save파일명


            strLog = $"Firmware DownLoad ForCamAsync:{result}";

            if (result == "-1")
            {
                Globalo.LogPrint("fw :", "Firmware Download Fail");
                return;
            }

            string[] receivedParse = result.Split(',');     //총 13개
                                                            //0 = $T
                                                            //1 = index (01)
                                                            //2 = 1번 Lot
                                                            //3 = 1번 결과

            //4 = index (02)
            //5 = 2번 Lot
            //6 = 2번 결과

            //7 = index (03)
            //8 = 3번 Lot
            //9 = 3번 결과

            //10 = index (04)
            //11 = 4번 Lot
            //12 = 4번 결과
            //result 결과 나오면 json 읽기
            //apd 보고
            //"$T,01,CAM1_P1637042-00-C-SLGM250434C00283,05,02,,00,03,,00,04,CAM3_P1637042-00-C-SLGM250434C00283\r,03"
            Globalo.LogPrint("fw :", strLog);//, Globalo.eMessageName.M_INFO);

            string[] rtnBcrArr = new string[4];
            string[] rtnFinalArr = new string[4];
            for (i = 0; i < rtnBcrArr.Length; i++)
            {
                rtnBcrArr[i] = string.Empty;
                rtnFinalArr[i] = string.Empty;
            }

            if (receivedParse.Length >= 13)
            {
                rtnBcrArr[0] = receivedParse[2];
                rtnBcrArr[1] = receivedParse[5];
                rtnBcrArr[2] = receivedParse[8];
                rtnBcrArr[3] = receivedParse[11];
                //
                rtnFinalArr[0] = receivedParse[3];
                rtnFinalArr[1] = receivedParse[6];
                rtnFinalArr[2] = receivedParse[9];
                rtnFinalArr[3] = receivedParse[12];
            }

            //---------------------------------------------------------------------------------------------------
            //
            //
            // Get Json
            //
            //---------------------------------------------------------------------------------------------------
            for (i = 0; i < lotarr.Length; i++)
            {
                int nResult = int.Parse(rtnFinalArr[i]);
                getfwResultFromJson(lotarr[i], nResult);
            }




            string[] _version = new string[4];
            string[] _sensorId = new string[4];

            ReadFirmware(ref _version, ref _sensorId);
            for (i = 0; i < _version.Length; i++)
            {
                string logMsg = $"[version]{_version[i]}";
                Globalo.LogPrint("fw", logMsg);
            }

            for (i = 0; i < _sensorId.Length; i++)
            {
                string logMsg = $"[sensorid]{_sensorId[i]}";
                Globalo.LogPrint("fw", logMsg);
            }
            //ReadFirmware  = VERSION 읽어서 conf.ini  에서 FIRMWARE_VERSION = 0xa2 와 비교하고
            //SENSER ID 읽어서 SKARLRL

            /*
             Result_Code
                Socket_Num
                Version
                Result
                Barcode
                Heater_Current

             */
        }

        public string getHeater_Current(string _Lot, string fwResult)
        {
            string resultLog = "D:\\tmp";
            string final = "";
            string _Bcr = _Lot;

            if (fwResult == "1")
            {
                final = "PASSED_";
            }
            else
            {
                final = "FAILED_";
            }
            string jsonFilePath = Path.Combine(resultLog, final + _Bcr + ".json");

            try
            {
                var result = ReadJsonResult(jsonFilePath); //경로\\제품 BCR   ///"D:\\tmp\\PASSED_CAM1_P1637042-00-C-SLGM250434C00283.json";

                var param = result.parameters.FirstOrDefault(p => p.name == "imager_temperature_sensor_test");

                return param.value.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"host Message Parse 처리 중 예외 발생: {ex.Message}");
            }
            return "-1";
        }
        public void getfwResultFromJson(string lotId, int nResult)      //fw 다운로드 완료시 생성되는 json 파일 불러오는 함수
        {
            //D:\\tmp\\ 는 LOG_PATH = D:\tmp\  여기서 가져와서 붙이고
            //PASSED_CAM1_
            //PASSED_CAM2_
            //PASSED_CAM3_
            //PASSED_CAM4_

            //PASSED
            //FAILED
            string resultLog = "D:\\tmp\\";     //conf.ini에서 가져와라 LOG_PATH = D:\tmp\
            string final = "";

            if (nResult == 0 && lotId.Length > 0)
            {
                final = "PASSED_";
            }
            else
            {
                final = "FAILED_";
            }
            //string _Bcr = "CAM1_P1637042-00-C-SLGM250434C00283";
            string _Bcr = lotId;


            string jsonFilePath = Path.Combine(resultLog, final + _Bcr + ".json");
            if (File.Exists(jsonFilePath) == false)
            {
                Console.WriteLine($"{jsonFilePath} File Empty");
                return;
            }
            var result = ReadJsonResult(jsonFilePath); //경로\\제품 BCR   ///"D:\\tmp\\PASSED_CAM1_P1637042-00-C-SLGM250434C00283.json";

            //string temp = result.parameters["imager_temperature_sensor_test"].name;

            
            // 예: 모든 항목 출력
            foreach (var param in result.parameters)
            {
                //Console.WriteLine($"[{param.result.ToUpper()}] {param.name} = {param.value}");
                string logMsg = $"[json][{param.result.ToUpper()}] {param.name} = {param.value}";
                Globalo.LogPrint("fw", logMsg);
            }
        }

        public void ReadFirmware(ref string[] version , ref string[] sensorid)
        {
            //F/W 버젼 읽기
            var ci = new ConnectionInfo("192.168.90.120", "root", new PasswordAuthenticationMethod("root", "root"));

            var client = new SshClient(ci);
            client.Connect();

            // Power On 12v
            client.CreateCommand("bash /home/root/utils/cam_power/turn_on_cameras.sh 12v 'all' 1 9").Execute();
            Thread.Sleep(200);

            // FPD Link Setup
            string model = "";
            if (true)
            {
                //Trinity: cypress
                model = "'cypress'";
            }
            else
            {
                //Opal: ti
                model = "'ti'";
            }

            string cmddddd = $"bash /home/root/camera_init_codes/fpd_link_setup.sh -d 9 -p '0 1 2 3' -s \"971\" -m {model}";

            //var output = client.CreateCommand("bash /home/root/camera_init_codes/fpd_link_setup.sh -d 9 -p '0 1 2 3' -s \"971\" -m 'cypress'").Execute(); // Trinity: cypress, Opal: ti
            var output = client.CreateCommand(cmddddd).Execute();

            bool[] bCamConn = new bool[4] { false, false, false, false };

            for (int i = 0; i < 4; i++)
            {
                string strFind = string.Format("P{0} Receiver is locked", i);
                if (output.IndexOf(strFind) != -1)
                {
                    bCamConn[i] = true;
                }
            }
            Thread.Sleep(200);

            string[] mcuAddress = new string[4] { "0x2a", "0x2b", "0x2c", "0x2d" };

            string strResult = "";

            for (int i = 0; i < 4; i++)
            {
                if (bCamConn[i] == false)
                {
                    strResult += "false,";
                    continue;
                }

                // FW Version Read
                string strFWVersion = string.Format("i2cget -y -f 9 {0} 0x01", mcuAddress[i]);
                var fwversion = client.CreateCommand(strFWVersion).Execute();

                version[i] = fwversion;
                ///
                ///
                ///
                ///



                // Sensor ID Read
                string readSensorIDCmd = $"bash /home/root/utils/read_imager_id.sh 9 {i}";
                string sensorID = client.CreateCommand(readSensorIDCmd).Execute();

                // 줄바꿈 제거 및 콤마로 구분된 hex string으로 변환
                var idLines = sensorID.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                sensorID = string.Join(",", idLines);
                sensorID = string.Concat(idLines.Select(x => x.Replace("0x", "").PadLeft(2, '0')));

                sensorid[i] = sensorID;
                //strResult += (version + ",");
                //strResult += $"P{i} => FW: {fwversion}, SensorID: {sensorID}\n";
            }
            //szLog = $"[AUTO] PIN COUNT CHECK OVER: {Globalo.yamlManager.taskDataYaml.TaskData.PintCount} / {Globalo.yamlManager.configData.DrivingSettings.PinCountMax} [STEP : {nRetStep}]";
            //Globalo.LogPrint("", szLog, Globalo.eMessageName.M_WARNING);


            //Globalo.LogPrint("FW Version Check", strResult);//, Globalo.eMessageName.M_WARNING);

            client.CreateCommand("bash /home/root/utils/cam_power/turn_on_cameras.sh 0v 'all' 0 9").Execute();

            client.Disconnect();


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

        public string FirmwareDownLoadForCamAsync(string[] lotId)
        {
            //FXA 보드에 업로드 되어있는 펌웨어 CAM 으로 다운로드 

            string exeName = "cypress_cam_flashing"; // 확장자 없이
            //const string workingDir = @"D:\PG\FXABoard\TeslaEXE\Tesla_FW_exe\Trinity_FW_Download_20250421_1111";
            //D:\EVMS\TP\ENV\fwexe\TeslaEXE\FXA.EXE\TeslaEXE\Tesla_FW_exe\Trinity_FW_Download_20250421_1111


            //string workingDir = @"D:\EVMS\TP\ENV\fwexe\TeslaEXE\FXA.EXE\TeslaEXE\Tesla_FW_exe\Trinity_FW_Download_20250421_1111";
            //string exePath = workingDir + @"\cypress_cam_flashing.exe";

            string workingDir = @Globalo.yamlManager.configData.TeslaData.Fpath;
            string exePath = workingDir + @Globalo.yamlManager.configData.TeslaData.FexeName;

            string host = "127.0.0.1";
            int port = 5000;

            //D:\EVMS\TP\ENV\fwexe\TeslaEXE\Tesla_FW_exe\Trinity_FW_Download_20250421_1111
            try
            {
                // EXE가 실행 중인지 확인
                System.Diagnostics.Process proc;
                int cnt = System.Diagnostics.Process.GetProcessesByName(exeName).Length;
                if (cnt < 1)
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = exePath,//"cmd.exe",
                        //Arguments = "/c " + Path.GetFileName(exePath),
                        WorkingDirectory = workingDir,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    proc = System.Diagnostics.Process.Start(psi);
                }
                // Process 실행 및 종료 대기
                string fdrtn = string.Empty;
                    

                Thread.Sleep(100);

                string command = $"$H,01,{lotId[0]},02,{lotId[1]},03,{lotId[2]},04,{lotId[3]}";


                using (TcpClient client = new TcpClient(host, port))
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.ASCII.GetBytes(command + "\n");  //"\r\n");
                        stream.Write(data, 0, data.Length);

                        byte[] buffer = new byte[1024];
                        int bytes = stream.Read(buffer, 0, buffer.Length);
                        fdrtn = Encoding.ASCII.GetString(buffer, 0, bytes).Trim();
                    }
                }

                //if (!process.WaitForExit(5000))  // 5초만 기다림
                //{
                //    Console.WriteLine("프로세스가 종료되지 않음. 강제 종료.");
                //    process.Kill();  // 자식 포함 강제 종료
                //}
                //process.Kill(true);  // 자식 포함 강제 종료
                // 종료 코드 확인 (옵션)
                //int exitCode = process.ExitCode;
                // Console.WriteLine($"프로세스 종료됨. 코드: {exitCode}");

                return fdrtn;
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
        public string getFirmwareFileName(string title)
        {
            StringBuilder fwFileName = new StringBuilder(256);
            string rtnFwName = string.Empty;

            string sourcePath = Path.Combine(strConfINIPath, "conf.ini");
            GetPrivateProfileString("DEFAULT", title, "", fwFileName, fwFileName.Capacity, sourcePath);
            //GetPrivateProfileString("DEFAULT", "FIRMWARE_FILE", "", fwFileName, fwFileName.Capacity, sourcePath);
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
