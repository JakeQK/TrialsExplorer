using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer
{
    class GRP
    {
        private byte[] beginning_Bytes;
        private byte[] ending_Bytes;

        private int data_size;
        private short color_Count;
        public List<Object> Objects = new List<Object>();

        /// <summary>
        /// Constructs a GRP object from Trials Fusion GRP file
        /// </summary>
        /// <param name="compressed_GRP">Trials Fusion LZMA Compressed GRP file</param>
        public GRP(byte[] compressed_GRP)
        {
            byte[] decompressed_GRP = Decompress(compressed_GRP);
            using (MemoryStream ms = new MemoryStream(decompressed_GRP))
            {
                using (Utility.MyBinaryReader br = new Utility.MyBinaryReader(ms, Utility.MyBinaryReader.Endianness.Big))
                {
                    beginning_Bytes = br.ReadBytes(0x11);
                    ParseObjectEntries(br);
                    ParseObjectTranslations(br);

                    // unknown, always 1 less than Color Count?
                    br.ReadInt16();
                    color_Count = br.ReadInt16();

                    ParseObjectColors(br);
                    ending_Bytes = br.ReadBytes(0x3E);
                }
            }
        }

        private void ParseObjectEntries(Utility.MyBinaryReader br)
        {
            Dictionary<int, string> ObjectInfo = Utility.LoadObjectInfo();
            data_size = br.ReadInt32();
            short object_Count = br.ReadInt16();

            for (short i = 0; i < object_Count; i++)
            {
                int objectID = br.ReadInt32();
                int objectVariation = br.ReadInt32();
                Properties prop = new Properties(br.ReadInt16());

                Entry entry = new Entry();
                entry.ID = objectID;
                entry.Variation = objectVariation;
                entry.Properties = prop;

                Object obj = new Object();
                obj.Name = ObjectInfo[objectID];
                obj.Entry = entry;

                Objects.Add(obj);
            }
        }

        private void ParseObjectTranslations(Utility.MyBinaryReader br)
        {
            for (short i = 0; i < Objects.Count; i++)
            {
                Translation translation = new Translation(br.ReadVector3(), br.ReadVector4());
                Objects[i].Translation = translation;
            }
        }

        private void ParseObjectColors(Utility.MyBinaryReader br)
        {
            for (short j = 0; j < color_Count; j++)
            {
                short index = br.ReadInt16();
                short color_size = br.ReadInt16();

                Objects[index].HasColor = true;
                Objects[index].Color_Index = index;
                Objects[index].Color_Size = color_size;

                for (short i = 0; i < (color_size / 10); i++)
                {
                    Color color = new Color(br.ReadRGB(), br.ReadInt16(), br.ReadRGB(), br.ReadInt16());
                    Objects[index].colors.Add(color);
                }
            }
        }


        /// <summary>
        /// Converts current GRP object to compressed Trials Fusion GRP file format
        /// </summary>
        /// <param name="Decompressed">Returns Trials Fusion GRP file format in decompressed state</param>
        /// <returns>Trials Fusion GRP file format byte array</returns>
        public byte[] BuildGRP(bool Decompressed = false)
        {
            /// Calculating Output Data Size
            // Data Len (4)
            // Object Count (2)
            // Unknown (2)
            // Color Count (2)
            int len = 10;
            foreach (Object obj in Objects)
            {
                // Object Entry (0xA)
                // Object Translation (0x1C)
                len += 0xA + 0x1C;
                // Color Index (2)
                // Color Size (2)
                if(obj.HasColor)
                    len += 4 + obj.Color_Size;
            }
            byte[] return_data = new byte[beginning_Bytes.Length + len + ending_Bytes.Length];


            using (MemoryStream ms = new MemoryStream(return_data))
            {
                using (Utility.MyBinaryWriter bw = new Utility.MyBinaryWriter(ms, Utility.MyBinaryWriter.Endianness.Big))
                {
                    bw.Write(beginning_Bytes, Utility.MyBinaryWriter.Endianness.Little);
                    bw.Write(BitConverter.GetBytes(data_size));
                    bw.Write(BitConverter.GetBytes((short)Objects.Count));

                    WriteEntries(bw);
                    WriteTranslations(bw);

                    bw.Write(BitConverter.GetBytes((short)(color_Count - 1)));
                    bw.Write(BitConverter.GetBytes(color_Count));

                    WriteColors(bw);

                    bw.Write(ending_Bytes, Utility.MyBinaryWriter.Endianness.Little);
                }
            }

            if (Decompressed)
                return return_data;
            return Compress(return_data);
        }

        private void WriteEntries(Utility.MyBinaryWriter bw)
        {
            foreach (Object obj in Objects)
            {
                // Object Entries
                bw.Write(BitConverter.GetBytes(obj.Entry.ID));
                bw.Write(BitConverter.GetBytes(obj.Entry.Variation));
                bw.Write(BitConverter.GetBytes(obj.Entry.Properties.ToInt16()));
            }
        }

        private void WriteTranslations(Utility.MyBinaryWriter bw)
        {
            foreach (Object obj in Objects)
            {
                bw.Write(BitConverter.GetBytes(obj.Translation.Position.x));
                bw.Write(BitConverter.GetBytes(obj.Translation.Position.y));
                bw.Write(BitConverter.GetBytes(obj.Translation.Position.z));

                bw.Write(BitConverter.GetBytes(obj.Translation.Rotation.x));
                bw.Write(BitConverter.GetBytes(obj.Translation.Rotation.y));
                bw.Write(BitConverter.GetBytes(obj.Translation.Rotation.z));
                bw.Write(BitConverter.GetBytes(obj.Translation.Rotation.w));

            }
        }

        private void WriteColors(Utility.MyBinaryWriter bw)
        {
            foreach (Object obj in Objects)
            {
                if (obj.HasColor)
                {
                    bw.Write(BitConverter.GetBytes(obj.Color_Index));
                    bw.Write(BitConverter.GetBytes(obj.Color_Size));

                    foreach (Color color in obj.colors)
                    {
                        bw.Write(color.Primary.R);
                        bw.Write(color.Primary.G);
                        bw.Write(color.Primary.B);

                        bw.Write(BitConverter.GetBytes(color.Emission));

                        bw.Write(color.Secondary.R);
                        bw.Write(color.Secondary.G);
                        bw.Write(color.Secondary.B);

                        bw.Write(BitConverter.GetBytes(color.Roughness));
                    }
                }
            }
        }



        /// <summary>
        /// LZMA Compression for GRP file format
        /// </summary>
        /// <param name="data">Decompressed GRP Data</param>
        /// <returns>LZMA Compressed GRP</returns>
        public static byte[] Compress(byte[] data)
        {
            byte[] header = { 0xBA, 0xBE, 0xBA, 0xBE, 0x02, 0x4C, 0x5A, 0x4D, 0x41 };
            byte[] properties = { 0x5D, 0x00, 0x00, 0x02, 0x00 };
            byte[] decompressed_size = BitConverter.GetBytes(data.Length);

            SevenZip.CoderPropID[] coderPropIDs = { SevenZip.CoderPropID.DictionarySize, SevenZip.CoderPropID.LitContextBits, SevenZip.CoderPropID.LitPosBits, SevenZip.CoderPropID.PosStateBits };
            object[] _properties = Utility.LzmaPropertiesFromBytes(properties);
            SevenZip.Compression.LZMA.Encoder encoder = new SevenZip.Compression.LZMA.Encoder();
            encoder.SetCoderProperties(coderPropIDs, _properties);

            using (MemoryStream outStream = new MemoryStream())
            {
                using (MemoryStream ms = new MemoryStream(data))
                {
                    encoder.Code(ms, outStream, ms.Length, -1, null);
                }

                byte[] LZMA_Data = outStream.ToArray();
                byte[] CompressedGRP = new byte[header.Length + properties.Length + decompressed_size.Length + LZMA_Data.Length];

                using (MemoryStream ms = new MemoryStream(CompressedGRP))
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write(header);
                        bw.Write(properties);
                        bw.Write(decompressed_size);
                        bw.Write(LZMA_Data);
                    }
                }

                return CompressedGRP;
            }
        }

        /// <summary>
        /// LZMA Decompression for GRP file format
        /// </summary>
        /// <param name="data">LZMA Compressed GRP</param>
        /// <returns>Decompressed GRP</returns>
        public static byte[] Decompress(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    br.BaseStream.Seek(9, SeekOrigin.Begin);
                    byte[] properties = br.ReadBytes(5);
                    int decompressed_size = br.ReadInt32();

                    SevenZip.Compression.LZMA.Decoder decoder = new SevenZip.Compression.LZMA.Decoder();
                    decoder.SetDecoderProperties(properties);

                    using (MemoryStream inStream = new MemoryStream(br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position))))
                    {
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            decoder.Code(inStream, outStream, inStream.Length, decompressed_size, null);

                            return outStream.ToArray();
                        }

                    }
                }
            }
        }





        // Classes
        public class Object
        {
            public Object()
            {
                Name = "";
                Entry = new Entry();
                Translation = new Translation();
                colors = new List<Color>();
            }

            public Object(string name, bool hasColor, Entry entry, Translation translation, short color_Index, short color_Size, List<Color> colors)
            {
                Name = name;
                HasColor = hasColor;
                Entry = entry;
                Translation = translation;
                Color_Index = color_Index;
                Color_Size = color_Size;
                this.colors = colors;
            }

            public string Name              { get; set; }
            public bool HasColor            { get; set; }

            public Entry Entry              { get; set; }
            public Translation Translation  { get; set; }

            public short Color_Index        { get; set; }
            public short Color_Size         { get; set; }
            public List<Color> colors       { get; set; }
        }


        public class Entry
        {
            public Entry()
            {
                ID = 0;
                Variation = 0;
                Properties = new Properties();
            }

            public Entry(int iD, int variation, Properties properties)
            {
                ID = iD;
                Variation = variation;
                Properties = properties;
            }

            public int ID { get; set; }
            public int Variation { get; set; }
            public Properties Properties { get; set; }
        }

        public class Properties
        {
            public Properties() 
            {

            }

            public Properties(short properties)
            {
                Import(properties);
            }

            public Properties(bool nonDefaultPhysics, bool noCollisionSound, bool fastObject, bool dontResetPosition, bool flag_5, bool flag_6, bool flag_7, bool flag_8, bool physics, bool flag_10, bool lockToDrivingLine, bool invisible, bool flag_13, bool flag_14, bool noContactResponse, bool noCollision)
            {
                NonDefaultPhysics = nonDefaultPhysics;
                NoCollisionSound = noCollisionSound;
                FastObject = fastObject;
                DontResetPosition = dontResetPosition;
                this.flag_5 = flag_5;
                this.flag_6 = flag_6;
                this.flag_7 = flag_7;
                this.flag_8 = flag_8;
                Physics = physics;
                this.flag_10 = flag_10;
                LockToDrivingLine = lockToDrivingLine;
                Invisible = invisible;
                this.flag_13 = flag_13;
                this.flag_14 = flag_14;
                NoContactResponse = noContactResponse;
                NoCollision = noCollision;
            }

            public short ToInt16()
            {
                short return_value = 0;
                Utility.SetBit(ref return_value, 0, NonDefaultPhysics);
                Utility.SetBit(ref return_value, 1, NoCollisionSound);
                Utility.SetBit(ref return_value, 2, FastObject);
                Utility.SetBit(ref return_value, 3, Invisible);
                Utility.SetBit(ref return_value, 4, flag_5);
                Utility.SetBit(ref return_value, 5, flag_6);
                Utility.SetBit(ref return_value, 6, flag_7);
                Utility.SetBit(ref return_value, 7, flag_8);
                Utility.SetBit(ref return_value, 8, Physics);
                Utility.SetBit(ref return_value, 9, flag_10);
                Utility.SetBit(ref return_value, 10, LockToDrivingLine);
                Utility.SetBit(ref return_value, 11, DontResetPosition);
                Utility.SetBit(ref return_value, 12, flag_13);
                Utility.SetBit(ref return_value, 13, flag_14);
                Utility.SetBit(ref return_value, 14, NoContactResponse);
                Utility.SetBit(ref return_value, 15, NoCollision);
                return return_value;
            }

            public void Import(short properties)
            {
                NonDefaultPhysics = (properties & (1 << 0)) == (1 << 0);
                NoCollisionSound = (properties & (1 << 1)) == (1 << 1);
                FastObject = (properties & (1 << 2)) == (1 << 2);
                Invisible = (properties & (1 << 3)) == (1 << 3);
                flag_5 = (properties & (1 << 4)) == (1 << 4);
                flag_6 = (properties & (1 << 5)) == (1 << 5);
                flag_7 = (properties & (1 << 6)) == (1 << 6);
                flag_8 = (properties & (1 << 7)) == (1 << 7);
                Physics = (properties & (1 << 8)) == (1 << 8);
                flag_10 = (properties & (1 << 9)) == (1 << 9);
                LockToDrivingLine = (properties & (1 << 10)) == (1 << 10);
                DontResetPosition = (properties & (1 << 11)) == (1 << 11);
                flag_13 = (properties & (1 << 12)) == (1 << 12);
                flag_14 = (properties & (1 << 13)) == (1 << 13);
                NoContactResponse = (properties & (1 << 14)) == (1 << 14);
                NoCollision = (properties & (1 << 15)) == (1 << 15);
            }


            public bool NonDefaultPhysics { get; set; }
            public bool NoCollisionSound { get; set; }
            public bool FastObject { get; set; }
            public bool Invisible { get; set; }
            public bool flag_5 { get; set; }
            public bool flag_6 { get; set; }
            public bool flag_7 { get; set; }
            public bool flag_8 { get; set; }
            public bool Physics { get; set; }
            public bool flag_10 { get; set; }
            public bool LockToDrivingLine { get; set; }
            public bool DontResetPosition { get; set; }
            public bool flag_13 { get; set; }
            public bool flag_14 { get; set; }
            public bool NoContactResponse { get; set; }
            public bool NoCollision { get; set; }
        }


        public class Translation
        {
            public Translation()
            {
                Position = new Vector3();
                Rotation = new Vector4();
            }

            public Translation(Vector3 position, Vector4 quaternionRotation)
            {
                Position = position;
                Rotation = quaternionRotation;
            }

            public Vector3 Position { get; set; }
            public Vector4 Rotation { get; set; }
        }

        public class Color
        {
            public Color()
            {
                Primary = new RGB();
                Emission = 0;
                Secondary = new RGB();
                Roughness = 0;
            }

            public Color(RGB primary, short emission, RGB secondary, short roughness)
            {
                Primary = primary;
                Emission = emission;
                Secondary = secondary;
                Roughness = roughness;
            }

            public RGB Primary { get; set; }
            public short Emission { get; set; }
            public RGB Secondary { get; set; }
            public short Roughness { get; set; }
        }
    }
}
