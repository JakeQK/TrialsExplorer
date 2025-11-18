using System;
using System.IO;
using FusionExplorer.Models.Math;
using FusionExplorer.Models.Utility;

namespace FusionExplorer.Services
{
    public class CustomBinaryReader : BinaryReader
    {
        private Endianness default_endianness;
        public CustomBinaryReader(System.IO.Stream stream) : base(stream) { }
        public CustomBinaryReader(System.IO.Stream stream, Endianness default_endianness) : base(stream) { this.default_endianness = default_endianness; }

        public enum Endianness
        {
            Little,
            Big
        }

        public override float ReadSingle()
        {
            var data = base.ReadBytes(4);
            if (default_endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToSingle(data, 0);
        }

        public float ReadSingle(Endianness endianness)
        {
            var data = base.ReadBytes(4);
            if (endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToSingle(data, 0);
        }

        public Vector3 ReadVector3()
        {
            return new Vector3(ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Vector3 ReadVector3(Endianness endianness)
        {
            return new Vector3(ReadSingle(endianness), ReadSingle(endianness), ReadSingle(endianness));
        }

        public Vector4 ReadVector4()
        {
            return new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
        }

        public Vector4 ReadVector4(Endianness endianness)
        {
            return new Vector4(ReadSingle(endianness), ReadSingle(endianness), ReadSingle(endianness), ReadSingle(endianness));
        }

        public RGB ReadRGB()
        {
            return new RGB(ReadByte(), ReadByte(), ReadByte());
        }

        public override Int16 ReadInt16()
        {
            var data = base.ReadBytes(2);
            if (default_endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }

        public Int16 ReadInt16(Endianness endianness)
        {
            var data = base.ReadBytes(2);
            if (endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }

        public override int ReadInt32()
        {
            var data = base.ReadBytes(4);
            if (default_endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }

        public int ReadInt32(Endianness endianness)
        {
            var data = base.ReadBytes(4);
            if (endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }

        public override Int64 ReadInt64()
        {
            var data = base.ReadBytes(8);
            if (default_endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToInt64(data, 0);
        }

        public Int64 ReadInt64(Endianness endianness)
        {
            var data = base.ReadBytes(8);
            if (endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToInt64(data, 0);
        }

        public override UInt16 ReadUInt16()
        {
            var data = base.ReadBytes(2);
            if (default_endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToUInt16(data, 0);
        }

        public UInt16 ReadUInt16(Endianness endianness)
        {
            var data = base.ReadBytes(2);
            if (endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToUInt16(data, 0);
        }

        public override UInt32 ReadUInt32()
        {
            var data = base.ReadBytes(4);
            if (default_endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }

        public UInt32 ReadUInt32(Endianness endianness)
        {
            var data = base.ReadBytes(4);
            if (endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToUInt32(data, 0);
        }

        public override UInt64 ReadUInt64()
        {
            var data = base.ReadBytes(8);
            if (default_endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToUInt64(data, 0);
        }

        public UInt64 ReadUInt64(Endianness endianness)
        {
            var data = base.ReadBytes(8);
            if (endianness == Endianness.Big)
                Array.Reverse(data);
            return BitConverter.ToUInt64(data, 0);
        }
    }
}
