using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenTester.Data
{
    public enum NO_ROI
    {
        LH = 0, CH, RH, CONE, ORING, KEY1, KEY2
    };
    public class MarkData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Smooth { get; set; }
    }
    public class LightData
    {
        public string name { get; set; }
        public int data { get; set; }
    }
    public class Roi
    {
        public string name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Roi Clone()
        {
            return new Roi
            {
                X = this.X,
                Y = this.Y,
                Width = this.Width,
                Height = this.Height
            };
        }
        // Optional: public string type { get; set; }  // 필요 시 추가
    }

    public class AoiRoiConfig
    {
        public List<Roi> HEIGHT_ROI { get; set; }
        public List<Roi> KEY_ROI { get; set; }
        public List<Roi> CONE_ROI { get; set; }
        public List<Roi> ORING_ROI { get; set; }
        public List<MarkData> markData { get; set; } = new List<MarkData>();
        public List<LightData> topLightData { get; set; } = new List<LightData>();
        public List<LightData> sideLightData { get; set; } = new List<LightData>();
    }


}
