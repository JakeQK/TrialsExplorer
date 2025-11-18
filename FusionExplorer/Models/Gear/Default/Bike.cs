using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.Gear.Default
{
    class Bike
    {
        public string BikeId { get; set; }
        public List<DefaultSet> DefaultSets { get; set; } = new List<DefaultSet>();
        public List<DefaultColor> DefaultColors { get; set; } = new List<DefaultColor>();
    }
}
