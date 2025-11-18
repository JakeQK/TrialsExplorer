using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.Gear
{
    class Set
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GenKey { get; set; }
        public string Icon { get; set; }
        public string DefaultColor { get; set; }
        public string Type { get; set; }
        public string Price { get; set; }
        public string ExpLevelReq { get; set; }
        public string MedalReq { get; set; }
        public string AlwaysLocked { get; set; }
        public string Hidden { get; set; }
        public string CameraTarget { get; set; }
        public string ColorSlotId { get; set; }
        public string ColorCategory { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }
}
