using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.Math
{
    public class Vector3
    {
        public Vector3()
        {

        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float x, y, z;

        public byte[] ToByteArray()
        {
            byte[] output = new byte[12];
            Buffer.BlockCopy(BitConverter.GetBytes(x).Reverse().ToArray(), 0, output, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(y).Reverse().ToArray(), 0, output, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(z).Reverse().ToArray(), 0, output, 8, 4);
            return output;
        }
    }
}
