using FusionExplorer.Models.Gear.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.Gear
{
    class GearData
    {
        public List<Rider> Riders { get; set; } = new List<Rider>();
        public List<Bike> Bikes { get; set; } = new List<Bike>();
        public List<Set> Sets { get; set; } = new List<Set>();
        public List<Item> Items { get; set; } = new List<Item>();
        public RiderDefaults RiderDefaults { get; set; } = new RiderDefaults();
        public List<Default.Bike> BikeDefaults { get; set; } = new List<Default.Bike>();
        public LocalMP.LocalMP LocalMP { get; set; } = new LocalMP.LocalMP();
    }
}
