using FusionExplorer.Models.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.MDL
{
    public class LodHeader
    {
        public Vector3 LR005 { get; set; } // unknown function, I've only encountered 1.0, 1.0, 1.0, following LR
        public string VisibilityString { get; set; }
        public int LodIndex { get; set; }

        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }

        public float MinRenderDistance { get; set; }
        public float MaxRenderDistance { get; set; }

        public int Unknown3 { get; set; }

        public Vector3 UnknownVector3_1 { get; set; }
        public Vector3 UnknownVector3_2 { get; set; }

        public int Unknown4 { get; set; }
        public int Unknown5 { get; set; }

        public string ID { get; set; } // Unknown ID

        public int Unknown6 { get; set; }
        public int Unknown7 { get; set; }
    }
}
