using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Data
{
    public enum NO_ROI
    {
        LH = 0, CH, RH, CONE, ORING, KEY1, KEY2
    };
    public class Roi
    {
        public string name { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        // Optional: public string type { get; set; }  // 필요 시 추가
    }

    public class AoiRoiConfig
    {
        public List<Roi> HEIGHT_ROI { get; set; }
        public List<Roi> KEY_ROI { get; set; }
        public List<Roi> CONE_ROI { get; set; }
        public List<Roi> ORING_ROI { get; set; }
    }


}
