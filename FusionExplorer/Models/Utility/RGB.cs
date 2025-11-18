using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models.Utility
{
    public class RGB
    {
        public RGB() { }
        public RGB(byte R, byte G, byte B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }

        public byte R, G, B;

        public byte[] ToByteArray()
        {
            byte[] output = new byte[3];
            Buffer.BlockCopy(BitConverter.GetBytes(this.R), 0, output, 0, 1);
            Buffer.BlockCopy(BitConverter.GetBytes(this.G), 0, output, 1, 1);
            Buffer.BlockCopy(BitConverter.GetBytes(this.B), 0, output, 2, 1);
            return output;
        }
    }
}
