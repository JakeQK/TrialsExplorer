using Ookii.Dialogs.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace FusionExplorer
{
    class Utility
    {
        public static bool SaveFile(byte[] data, string initialDirectory = null)
        {
            VistaSaveFileDialog sfd = new VistaSaveFileDialog();
            if (initialDirectory != null)
            {
                sfd.FileName = initialDirectory;
            }

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (BinaryWriter bw = new BinaryWriter(File.Create(sfd.FileName)))
                {
                    bw.Write(data);
                    return true;
                }
            }
            return false;
        }

        public class MyBinaryWriter : BinaryWriter
        {
            private Endianness default_endianness;
            public MyBinaryWriter(Stream stream) : base(stream) { default_endianness = Endianness.Little; }
            public MyBinaryWriter(Stream stream, Endianness default_endianness) : base(stream) { this.default_endianness = default_endianness; }

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

        public class MyBinaryReader : BinaryReader
        {
            private Endianness default_endianness;
            public MyBinaryReader(System.IO.Stream stream) : base(stream) { }
            public MyBinaryReader(System.IO.Stream stream, Endianness default_endianness) : base(stream) { this.default_endianness = default_endianness; }

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

        // Read LZMA compression properties from a byte array
        public static object[] LzmaPropertiesFromBytes(byte[] properties)
        {
            Int32 lc = properties[0] % 9;
            Int32 remainder = properties[0] / 9;
            Int32 lp = remainder % 5;
            Int32 pb = remainder / 5;
            Int32 dictionarySize = 0;
            for (int i = 0; i < 4; i++)
                dictionarySize += ((Int32)(properties[1 + i])) << (i * 8);
            return new object[] { dictionarySize, lc, lp, pb };
        }

        public static Dictionary<int, string> LoadObjectInfo()
        {
            Dictionary<int, string> ret = new Dictionary<int, string>();
            using (FileStream stream = File.Open("Object IDs.csv", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while(!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] split = line.Split(',');
                        int id = int.Parse(split[0]);
                        ret.Add(id, split[1]);
                    }
                }
            }

            return ret;
        }

        public static void SetBit(ref byte data, byte bit, bool value)
        {
            if (!value)
                data &= (byte)~(1 << bit);
            else
                data |= (byte)(1 << bit);
        }

        public static void SetBit(ref short data, short bit, bool value)
        {
            if (!value)
                data &= (short)~(1 << bit);
            else
                data |= (short)(1 << bit);
        }

        public static void SetBit(ref int data, int bit, bool value)
        {
            if (!value)
                data &= (int)~(1 << bit);
            else
                data |= (int)(1 << bit);
        }

        public static string GetConfigValue(string key)
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;
            return confCollection[key].Value;
        }

        public static void SetConfigValue(string key, string value)
        {
            var configManager = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection confCollection = configManager.AppSettings.Settings;
            confCollection[key].Value = value; ;
        }
    }

    public class Vector3
    {
        public Vector3() {

        }

        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float x, y, z;
    }

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
    }

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
    }
}
