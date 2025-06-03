using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.FThread
{
    public class TestAutoThread : BaseThread
    {
        //private MotionControl.MotorController parent;
        public int m_nCurrentStep = 0;
        public int m_nStartStep = 0;
        public int m_nEndStep = 0;

        public TestAutoThread()     //MotionControl.MotorController _parent)
        {
            //this.parent = _parent;
            this.name = "testThread";
        }

        private void AoiFlow()
        {
            //소켓 2개 연달아 검사
        }
        private void Write_EEpromFlow()
        {
            //소켓 1개씩 Write
        }
        private void Verify_EEpromFlow()
        {
            //소켓 1개씩 Write
        }
        private void FwFlow()
        {
            //소켓 4개를 한번에 Firmware Download
        }
        protected override void ThreadRun()
        {

        }
        protected override void ThreadInit()
        {
            Console.WriteLine($"ThreadInit");
            //Console.WriteLine($"{this.parent.MachineName} ThreadInit");
        }

        protected override void ThreadDestructor()
        {
            Console.WriteLine($"ThreadDestructor");
            //Console.WriteLine($"{this.parent.MachineName} ThreadDestructor");
        }
    }
}
