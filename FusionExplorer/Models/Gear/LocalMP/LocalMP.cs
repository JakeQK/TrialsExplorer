using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.Gear.LocalMP
{
    class LocalMP
    {
        public List<Bike.Bike> Bikes { get; set; } = new List<Bike.Bike>();
        public List<Rider.Rider> Riders { get; set; } = new List<Rider.Rider>();
    }
}
