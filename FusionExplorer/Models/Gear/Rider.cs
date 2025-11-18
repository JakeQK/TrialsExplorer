using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.Gear
{
    class Rider
    {
        public string Name { get; set; }
        public string GenKey { get; set; }
        public string Id { get; set; }
        public string Icon { get; set; }
        public List<Group> Groups { get; set; } = new List<Group>();
    }
}
