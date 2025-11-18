using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.Gear
{
    class Group
    {
        public string Name { get; set; }
        public List<string> Sets { get; set; } = new List<string>();
    }
}
