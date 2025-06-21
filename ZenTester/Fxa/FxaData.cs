using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.Fxa
{
    public struct Parameter
    {
        public string name { get; set; }
        public object value { get; set; }
        public string description { get; set; }
        public string result { get; set; }
        public string type { get; set; }
        public double? lowerlimit { get; set; }
        public double? upperlimit { get; set; }
        public string expectedvalue { get; set; }
    }
    public struct TestResult
    {
        public List<Parameter> parameters { get; set; }
        public List<string> errors { get; set; }
        public string thingname { get; set; }
        public string taskname { get; set; }
        public string actorname { get; set; }
        public string tasktimestamp { get; set; }
        public string actorversion { get; set; }
        public string actorlocation { get; set; }
        public string taskversion { get; set; }
        public string sitecode { get; set; }
        public string taskcollectionname { get; set; }
        public string partnumber { get; set; }
    }
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        public IntPtr lpData;
    }
}
