using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

namespace ZenTester
{
    static class Program
    {
        public const string VERSION_INFO = "1.0.0.1";
        public const string BUILD_DATE = "25-04-16";        //16:00

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());


            // 뮤텍스를 생성하여 애플리케이션이 이미 실행 중인지 확인
            string processName = Assembly.GetExecutingAssembly().GetName().Name;
            
            var processes = System.Diagnostics.Process.GetProcessesByName(processName);
            if (processes.Length > 1)
            {
                //MessageBox.Show("이미 실행 중입니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MessageBox.Show($"[{processName}] 애플리케이션은 이미 실행 중입니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Application.Run(new MainForm());

            //bool createdNew;
            //using (Mutex mutex = new Mutex(true, processName, out createdNew))
            //{
            //    if (createdNew)
            //    {
            //        // 애플리케이션이 처음 실행될 때
            //        Application.Run(new MainForm());
            //    }
            //    else
            //    {
            //        // 이미 실행 중이면 사용자에게 메시지 표시
            //        MessageBox.Show("이 애플리케이션은 이미 실행 중입니다.", "경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //}

        }
        public static void SetLanguage(string langCode)
        {
            var ci = new CultureInfo(langCode);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            if (langCode != "en-US")  // 예시로 영어를 기본 언어로 설정한 경우
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            }
            Event.EventManager.RaiseLanguageChanged();
            //ApplyLocalization();
        }
    }
}
//250429 in aps first 21:03
//MEMO: 250509  조명 컨트롤러 100일때 가스켓 있는 제품 , 없는 제품 CON1,2 잘 찾아진다.