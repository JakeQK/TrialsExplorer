using System;
using System.IO;

namespace FusionExplorer.Services
{
    public class CustomBinaryWriter : BinaryWriter
    {
        private Endianness default_endianness;
        public CustomBinaryWriter(Stream stream) : base(stream) { default_endianness = Endianness.Little; }
        public CustomBinaryWriter(Stream stream, Endianness default_endianness) : base(stream) { this.default_endianness = default_endianness; }

        public enum Endianness
        {
            Little,
            Big
        }

        public override void Write(byte[] value)
        {
            var data = value;
            if (default_endianness == Endianness.Big)
                Array.Reverse(data);
            base.Write(data);
        }

        public void Write(byte[] value, Endianness endianness)
        {
            var data = value;
            if (endianness == Endianness.Big)
                Array.Reverse(data);
            base.Write(data);
        }
    }

}
