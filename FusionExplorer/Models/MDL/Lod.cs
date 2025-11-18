using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.MDL
{
    public class Lod
    {
        public LodHeader Header { get; set; }
        public List<Vertex> VertexData { get; set; } = new List<Vertex>();
        public List<Face> FaceData { get; set; } = new List<Face>();
    }
}
