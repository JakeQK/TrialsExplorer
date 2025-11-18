using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.MDL
{
    public class MDL
    {
        public int Version { get; set; }
        public List<Lod> Lods { get; private set; } = new List<Lod>();
    }
}
