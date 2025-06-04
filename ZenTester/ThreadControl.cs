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
        public FThread.TestAutoThread[] testAutoThread = new FThread.TestAutoThread[2];

        public ThreadControl()
        {
            logThread = new FThread.LogThread();
            timeThread = new FThread.TimeThread();
            autoRunthread = new FThread.AutoRunthread();

            testAutoThread[0] = new FThread.TestAutoThread(0);
            testAutoThread[1] = new FThread.TestAutoThread(1);

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

            testAutoThread[0].Close();
            testAutoThread[1].Close();

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
