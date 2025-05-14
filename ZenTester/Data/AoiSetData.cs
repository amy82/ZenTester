using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Data
{
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
        public List<Roi> AOI_ROI { get; set; }
    }


}
