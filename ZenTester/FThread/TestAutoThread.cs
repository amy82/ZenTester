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

        public Process.AoiTestFlow aoiTestFlow;
        
        public int m_nCurrentStep = 0;
        public int m_nStartStep = 0;
        public int m_nEndStep = 0;
        public int m_nUnit = 0;
        public TestAutoThread(int index = 0)     //MotionControl.MotorController _parent)
        {
            //this.parent = _parent;
            m_nUnit = index;
            aoiTestFlow = new Process.AoiTestFlow();

            this.name = "testThread";
        }

        private void AoiFlow()
        {
            //소켓 2개 연달아 검사
            this.m_nCurrentStep = aoiTestFlow.AoiAutoProcess(this.m_nCurrentStep);
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
            if (this.m_nCurrentStep >= this.m_nStartStep && this.m_nCurrentStep < this.m_nEndStep)
            {
                if (Program.TEST_PG_SELECT == TESTER_PG.AOI)
                {
                    AoiFlow();
                }
                if (Program.TEST_PG_SELECT == TESTER_PG.EEPROM)
                {
                    Verify_EEpromFlow();
                }
                if (Program.TEST_PG_SELECT == TESTER_PG.FW)
                {
                    FwFlow();
                }
            }
            else
            {
                m_nStartStep = 0;
                m_nEndStep = 0;
                this.Stop();

                Console.WriteLine($" Process Stop");
            }
                
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
