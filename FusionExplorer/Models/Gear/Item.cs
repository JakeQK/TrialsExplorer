using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.Gear
{
    class Item
    {
        public string Id { get; set; }
        public string GenKey { get; set; }
        public string ColorPartId { get; set; }
        public string BindId { get; set; }
        public string ObjectId { get; set; }
        public string Shared { get; set; }
        public string VariationId { get; set; }
    }
}
