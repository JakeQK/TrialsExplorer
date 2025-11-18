using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.Gear
{
    class Bike
    {
        public string Name { get; set; }
        public string GenKey { get; set; }
        public string Id { get; set; }
        public string ObjectId { get; set; }
        public List<Group> Groups { get; set; } = new List<Group>();
    }
}
