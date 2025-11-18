using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.Math
{
    public class Vector4
    {
        public Vector4() { }
        public Vector4(float w, float x, float y, float z)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float w, x, y, z;

        public byte[] ToByteArray()
        {
            byte[] output = new byte[16];
            Buffer.BlockCopy(BitConverter.GetBytes(x).Reverse().ToArray(), 0, output, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(y).Reverse().ToArray(), 0, output, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(z).Reverse().ToArray(), 0, output, 8, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(w).Reverse().ToArray(), 0, output, 12, 4);
            return output;
        }
    }
}
