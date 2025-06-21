using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenTester.Fxa
{
    
    public class FxaFirmwardDw
    {
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
    }
}
