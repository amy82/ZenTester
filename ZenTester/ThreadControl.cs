using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester
{
    public class ThreadControl
    {
        public FThread.LogThread logThread;
        public FThread.TimeThread timeThread;
        
        public FThread.AutoRunthread autoRunthread;
        public FThread.TestAutoThread testAutoThread;

        public ThreadControl()
        {
            logThread = new FThread.LogThread();
            timeThread = new FThread.TimeThread();
            autoRunthread = new FThread.AutoRunthread();

            testAutoThread = new FThread.TestAutoThread();



            // 이벤트 핸들러 등록
            //autoRunthread.ThreadCompleted += (bool result) =>
            //{
            //    Console.WriteLine($"[Event] Thread 종료됨, 결과: {result}");
            //    //if(Globalo.MainForm != null)
            //    //{
            //    //    Globalo.MainForm.StopAutoProcess();
            //    //}
            //};

            if (ProgramState.ON_LINE_OPENCV_IMAGE)
            {
            }
                
            if (ProgramState.ON_LINE_MIL)
            {
                if (ProgramState.ON_LINE_CAM)
                {
                }
            }
        }
        public void AllThreadStart()
        {
            logThread.Start();
            timeThread.Start();

            if (ProgramState.ON_LINE_MOTOR)
            {
            }
            
        }
        public void AllClose()
        {
            logThread.Close();
            timeThread.Close();
            autoRunthread.Close();

            testAutoThread.Close();

            if (ProgramState.ON_LINE_MIL)
            {
;
            }
            if (ProgramState.ON_LINE_CAM)
            {

            }
            if (ProgramState.ON_LINE_MOTOR)
            {

            }
        }
    }
}
