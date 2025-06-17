using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenTester.Http
{
    public class HttpService
    {
        static string recipeFolder = @"D:\Recipes"; // 실제 경로로 바꾸세요
        private static CancellationTokenSource _HttpCts;
        private static HttpListener _listener;
        private static Task _serverTask;
        private static readonly object _fileLock = new object();


        private static readonly string[] RecipeFiles = new string[]
        {
            "RecipeA.json", "RecipeB.json", "RecipeC.json"
        };


        public async static void ReqModel()
        {
            
            try
            {

                //PC Ip:
                //Handler : 192.168.100.100
                //SecsGem : 192.168.100.100
                //Verify Handler = 192.168.100.101

                int port = 3001;    //SecsGem Pg
                string url = string.Empty;

                if (ProgramState.NORINDA_MODE == true)
                {
                    url = $"http://127.0.0.1:{port}/reqModel";
                }
                else
                {
                    url = $"http://192.168.100.100:{port}/reqModel";       //aoi만 사용하니깐 192.168.100.100
                }

                //string json = JsonConvert.SerializeObject("");
                int _idNum = Globalo.yamlManager.configData.MachineId;
                var json = JsonConvert.SerializeObject(new { ipNumber = _idNum });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Req Model Complete!");
                }
                else
                {
                    Console.WriteLine($"Req Model Send Fail: ErrCode {(int)response.StatusCode} {response.ReasonPhrase}");
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Server Response Content: " + errorContent);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Req Model HTTP 요청 중 오류 발생: " + ex.Message);
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("Req Model 요청 시간이 초과되었습니다: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Req Model 알 수 없는 예외: " + ex.Message);
            }
        }
        public async static void ReqRecipe()
        {
            ////string json = JsonConvert.SerializeObject("");
            int _idNum = Globalo.yamlManager.configData.MachineId;
            var json = JsonConvert.SerializeObject(new { ipNumber = _idNum });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            try
            {
                //PC Ip:
                //Handler : 192.168.100.100
                //SecsGem : 192.168.100.100
                //Verify Handler = 192.168.100.101

                int port = 3001;    //SecsGem Pg
                string url = string.Empty;

                if (ProgramState.NORINDA_MODE == true)
                {
                    url = $"http://127.0.0.1:{port}/reqRecipe";
                }
                else
                {
                    url = $"http://192.168.100.100:{port}/reqRecipe";       //aoi만 사용하니깐 192.168.100.100
                }
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Recipe Complete!");
                }
                else
                {
                    Console.WriteLine($"Recipe Send Fail: ErrCode {(int)response.StatusCode} {response.ReasonPhrase}");
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Server Response Content: " + errorContent);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP 요청 중 오류 발생: " + ex.Message);
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("요청 시간이 초과되었습니다: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("알 수 없는 예외: " + ex.Message);
            }
        }
        public async static void LotApdReport(TcpSocket.AoiApdData apdData)
        {
            var AoiApdList = new
            {
                LH = apdData.LH,
                RH = apdData.RH,
                MH = apdData.MH,
                Gasket = apdData.Gasket,
                KeyType = apdData.KeyType,
                CircleDented = apdData.CircleDented,
                Concentrycity_A = apdData.Concentrycity_A,
                Concentrycity_D = apdData.Concentrycity_D,
                Cone = apdData.Cone,
                ORing = apdData.ORing,
                Result = apdData.Result,
                Barcode = apdData.Barcode,
                Socket_Num = apdData.Socket_Num

            };


            string json = JsonConvert.SerializeObject(AoiApdList);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient client = new HttpClient();
            try
            {

                int port = 3001;    //T ------->>>  SecsGem Pg 3001 고정
                string url = string.Empty;

                if (Program.TEST_PG_SELECT == TESTER_PG.EEPROM_VERIFY)
                {
                    //url = $"http://192.168.100.101:{port}/ApdReport";      //VERIFY 설비 : 101
                }
                else
                {
                    //url = $"http://192.168.100.100:{port}/ApdReport";      //FW , WRITE , AOI : 100
                }
                url = $"http://127.0.0.1:{port}/ApdReport";


                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Recipe Complete!");
                }
                else
                {
                    Console.WriteLine($"Recipe Send Fail: ErrCode {(int)response.StatusCode} {response.ReasonPhrase}");
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Server Response Content: " + errorContent);
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("HTTP 요청 중 오류 발생: " + ex.Message);
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("요청 시간이 초과되었습니다: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("알 수 없는 예외: " + ex.Message);
            }
        }
        public HttpService()
        {
            Event.EventManager.PgExitCall += OnPgExit;
        }
        private void OnPgExit(object sender, EventArgs e)
        {
            //await HttpService.Stop();
            _ = HttpService.Stop();
        }
        public static void Start()
        {
            _HttpCts = new CancellationTokenSource();
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://+:4001/"); // PC1 주소에서 4001 포트로 요청 받음
            _listener.Start();
            //Start에서 오류나면 관리자 권한으로 시작하거나
            //cmd 관리자 권한에서 실행하기=>netsh http add urlacl url=http://+:3001/ user=Everyone
            //netsh http add urlacl url=http://+:4001/ user=Everyone

            Console.WriteLine("HTTP 서버 시작됨: http://+:4001/");

            _serverTask = Task.Run(() => RunServerLoop(_HttpCts.Token));
        }
        private static async Task RunServerLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var contextTask = _listener.GetContextAsync();

                    // 취소 가능하도록 Task.WhenAny 사용
                    var completedTask = await Task.WhenAny(contextTask, Task.Delay(-1, token));

                    if (completedTask == contextTask)
                    {
                        var context = contextTask.Result;
                        _ = Task.Run(() => HandleRequest(context)); // 비동기로 응답 처리
                    }
                    else
                    {
                        // 취소 요청됨
                        break;
                    }
                }
            }
            catch (HttpListenerException ex) when (ex.ErrorCode == 995 || ex.ErrorCode == 500)
            {
                Console.WriteLine("HTTP 리스너 중단됨.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("서버 루프 종료 요청 받음.");
            }
            finally
            {
                _listener?.Close();
            }

            Console.WriteLine("HTTP 서버 루프 종료.");
        }
        private static void HandleRequest(HttpListenerContext context)
        {
            string path = context.Request.Url.AbsolutePath;
            var query = context.Request.QueryString;
            lock (_fileLock)
            {
                if (path == "/recipes")
                {
                    string json = JsonConvert.SerializeObject(RecipeFiles);
                    byte[] buffer = Encoding.UTF8.GetBytes(json);
                    context.Response.ContentType = "application/json";
                    context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                }
                else if (path == "/set-Model")
                {
                    string szLog = string.Empty;
                    szLog = $"[Http] Recv /set-Model";
                    Globalo.LogPrint("LotProcess", szLog);
                    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        string body = reader.ReadToEnd();
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);

                        Globalo.yamlManager.secsGemDataYaml.ModelData.CurrentModel = Convert.ToString(data["MODEL"]);



                        Globalo.yamlManager.secsGemDataYaml.MesSave();
                        Globalo.productionInfo.ShowModelName();


                        szLog = $"[Http] Recv Model : {Globalo.yamlManager.secsGemDataYaml.ModelData.CurrentModel}";
                        Globalo.LogPrint("LotProcess", szLog);
                    }
                }
                else if (path == "/set-recipe")     //[AOI] 레시피 설정값 받는 부분 
                {
                    string szLog = string.Empty;
                    szLog = $"[Http] Recv /set-recipe";
                    Globalo.LogPrint("LotProcess", szLog);
                    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        string body = reader.ReadToEnd();
                        var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);

                        // 예시: 특정 값 확인
                        int useChk = 0;
                        double specData = 0.0;
                        string key_type = "";
                        string recipePPid = "";

                        
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.Ppid = Convert.ToString(data["RECIPE"]);

                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["O_RING"].value = data["O_RING"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONE"].value = data["CONE"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["KEYTYPE"].value = data["KEYTYPE"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET"].value = data["GASKET"].ToString();
                        //
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["HEIGHT_LH_MIN"].value = data["HEIGHT_LH_MIN"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["HEIGHT_LH_MAX"].value = data["HEIGHT_LH_MAX"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["HEIGHT_MH_MIN"].value = data["HEIGHT_MH_MIN"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["HEIGHT_MH_MAX"].value = data["HEIGHT_MH_MAX"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["HEIGHT_RH_MIN"].value = data["HEIGHT_RH_MIN"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["HEIGHT_RH_MAX"].value = data["HEIGHT_RH_MAX"].ToString();
                        //
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_IN_MIN"].value = data["CONCENTRICITY_IN_MIN"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_IN_MAX"].value = data["CONCENTRICITY_IN_MAX"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_OUT_MIN"].value = data["CONCENTRICITY_OUT_MIN"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["CONCENTRICITY_OUT_MAX"].value = data["CONCENTRICITY_OUT_MAX"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET_MIN"].value = data["GASKET_MIN"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["GASKET_MAX"].value = data["GASKET_MAX"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["DENT_MIN"].value = data["DENT_MIN"].ToString();
                        Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["DENT_MAX"].value = data["DENT_MAX"].ToString();


                        //useChk = Convert.ToInt32(data["O_RING"]);        //오링 유무
                        //key_type = Convert.ToString(data["KEYTYPE"]);       //Key Type
                        //specData = Convert.ToDouble(data["HEIGHT_LH_MIN"]);             //LH MIN SPEC

                        Console.WriteLine($"HEIGHT_LH_MIN:{specData}");

                        Globalo.yamlManager.secsGemDataYaml.ModelData.CurrentRecipe = Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.Ppid;
                        Globalo.yamlManager.aoiRoiConfig = Data.TaskDataYaml.Load_AoiConfig();     //roi load
                                                                                                   //TODO: 받아서 레시피 파일로 저장을 하자


                        Globalo.yamlManager.RecipeSave(Globalo.yamlManager.vPPRecipeSpecEquip);
                        Globalo.yamlManager.secsGemDataYaml.MesSave();
                        
                        Globalo.productionInfo.ShowRecipeName();


                        szLog = $"[Http] Recv Recipe : {Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.Ppid}";
                        Globalo.LogPrint("LotProcess", szLog);
                    }
                }
                else if (path == "/recipe")
                {
                    string name = query["name"];

                    if (!string.IsNullOrEmpty(name))
                    {
                        string filePath = Path.Combine("C:\\Recipes", name); // 실제 레시피 폴더 경로

                        if (File.Exists(filePath))
                        {
                            string content = File.ReadAllText(filePath);
                            byte[] buffer = Encoding.UTF8.GetBytes(content);
                            context.Response.ContentType = "application/json";
                            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            context.Response.StatusCode = 404;
                            byte[] buffer = Encoding.UTF8.GetBytes("File not found..");
                            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
            context.Response.Close();
        }

        public static async Task Stop()
        {
            if (_HttpCts != null)
            {
                Console.WriteLine("HTTP 서버 종료 중...");
                _HttpCts.Cancel();

                try
                {
                    await _serverTask;
                }
                catch (TaskCanceledException)
                {
                    // 무시해도 됨
                }

                _HttpCts.Dispose();
                _HttpCts = null;

                Console.WriteLine("HTTP 서버 종료 완료.");
            }
        }
    }
}
