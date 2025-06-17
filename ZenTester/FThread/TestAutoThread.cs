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
