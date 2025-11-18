using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.MDL
{
    public struct Vertex
    {
        public Vector2 TextureCoordinates { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }

        public float Unknown1 { get; set; }
        public short Unknown2 { get; set; }
    }
}
