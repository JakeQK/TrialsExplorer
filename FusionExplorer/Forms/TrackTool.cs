using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SevenZip;
using Ookii.Dialogs.WinForms;

namespace FusionExplorer
{
    public partial class TrackTool : Form
    {
        public TrackTool()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = DecompressGRP(File.ReadAllBytes(ofd.FileName));
                VistaSaveFileDialog sfd = new VistaSaveFileDialog();
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    using(BinaryWriter bw = new BinaryWriter(File.Create(sfd.FileName)))
                    {
                        bw.Write(data);
                    }
                }
            }
        }

        enum File_Types
        {
            GRP = -1095057734,
            TRK = 196148938,
            EditorTRK = -1095062050
        }

        private long PatternSearch(BinaryReader br, byte[] pattern)
        {
            bool found = false;
            int len = pattern.Length;

            while (!found)
            {
                byte[] search = br.ReadBytes(len);
                if (search.SequenceEqual(pattern))
                    found = true;

                br.BaseStream.Seek(br.BaseStream.Position - len + 1, SeekOrigin.Begin);
            }

            return br.BaseStream.Position - 1;
        }

        private byte[] Decompress(byte[] data)
        {
            using(MemoryStream ms = new MemoryStream(data))
            {
                using(BinaryReader br = new BinaryReader(ms))
                {
                    int FileType = br.ReadInt32();

                    if(FileType == (int)File_Types.GRP)
                    {
                        int index = (int)PatternSearch(br, new byte[] { 0x4C, 0x5A, 0x4D, 0x41 }) + 4;
                        br.BaseStream.Seek(0, SeekOrigin.Begin);
                        byte[] header = br.ReadBytes(index);

                        byte[] LZMA_Data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
                        byte[] DecompressedLZMA = DecompressLZMA(LZMA_Data);

                        byte[] DecompressedGRP = new byte[header.Length + DecompressedLZMA.Length + 9 + 3];
                        Buffer.BlockCopy(new byte[] { 0x47, 0x52, 0x50 }, 0, DecompressedGRP, 0, 3);
                        Buffer.BlockCopy(DecompressedLZMA, 0, DecompressedGRP, 3, DecompressedLZMA.Length);
                        Buffer.BlockCopy(header, 0, DecompressedGRP, DecompressedLZMA.Length + 3, header.Length);
                        Buffer.BlockCopy(LZMA_Data, 0, DecompressedGRP, DecompressedLZMA.Length + header.Length + 3, 9);

                        return DecompressedGRP;
                    }
                    else if (FileType == (int)File_Types.TRK || FileType == (int)File_Types.EditorTRK)
                    {
                        int index = (int)PatternSearch(br, new byte[] { 0x4C, 0x5A, 0x4D, 0x41 }) + 4;
                        br.BaseStream.Seek(0, SeekOrigin.Begin);
                        byte[] header = br.ReadBytes(index);

                        byte[] LZMA_Data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
                        byte[] DecompressedLZMA = DecompressLZMA(LZMA_Data);

                        byte[] DecompressedTRK = new byte[header.Length + DecompressedLZMA.Length + 9 + 3];
                        Buffer.BlockCopy(new byte[] { 0x54, 0x52, 0x4B }, 0, DecompressedTRK, 0, 3);
                        Buffer.BlockCopy(DecompressedLZMA, 0, DecompressedTRK, 3, DecompressedLZMA.Length);
                        Buffer.BlockCopy(header, 0, DecompressedTRK, DecompressedLZMA.Length + 3, header.Length);
                        Buffer.BlockCopy(LZMA_Data, 0, DecompressedTRK, DecompressedLZMA.Length + header.Length + 3, 9);

                        return DecompressedTRK;
                    }
                    return null;
                }
            }
        }

        private byte[] DecompressLZMA(byte[] LZMA_Data)
        {
            using (MemoryStream ms = new MemoryStream(LZMA_Data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    byte[] properties = br.ReadBytes(5);
                    int decompressed_size = br.ReadInt32(); // i think

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




        void ParseOBJA(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                        int index = (int)PatternSearch(br, new byte[] { 0x4C, 0x5A, 0x4D, 0x41 }) + 4;
                        br.BaseStream.Seek(0, SeekOrigin.Begin);
                        byte[] header = br.ReadBytes(index);

                        byte[] LZMA_Data = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
                        byte[] DecompressedLZMA = DecompressLZMA(LZMA_Data);

                        ParseOBJA2(DecompressedLZMA);
                }
            }
        }

        List<OBJ> OBJs = new List<OBJ>();

        public class OBJ
        {
            public OBJ(int objID, int vari, byte idk1, byte idk2, string obj_name)
            {
                this.ObjectID = objID;
                this.Variation = vari;
                //this.PhysicsType = phyType;
                this.unk1 = idk1;
                this.unk2 = idk2;
                this.ObjectName = obj_name;
            }

            public int ObjectID {  get; set; }
            public int Variation {  get; set; }
            public byte unk1 { get; set; }
            public byte unk2 { get; set; }
            //public string PhysicsType {  get; set; }
            public string ObjectName { get; set; }
        }


        void ParseOBJA2(byte[] decompressedLZMA)
        {
            Dictionary<int, string> objIDS = LoadOBJIds();

            using (MemoryStream ms = new MemoryStream(decompressedLZMA))
            {
                using(BinaryReader2 br = new BinaryReader2(ms))
                {
                    //br.BaseStream.Seek(0x3CA, SeekOrigin.Begin);
                    br.BaseStream.Seek(0x16E, SeekOrigin.Begin);
                    Int16 obj_count = br.ReadInt16();

                    for(int i = 0; i < obj_count; i++)
                    {

                        int id = br.ReadInt32();
                        int variation = br.ReadInt32();

                        byte PhysicsType = br.ReadByte();
                        byte unk = br.ReadByte();
                        string phyType = PhysicsType.ToString();

                        /*
                        switch(PhysicsType)
                        {
                            case 4144:
                                phyType = "Default";
                                break;
                            case 4400:
                                phyType = "Contact Response";
                                break;
                            case 4464:
                                phyType = "No contact response";
                                break;
                            case 4592:
                                phyType = "Decoration Only";
                                break;
                        }
                        */

                        string name = "";
                        objIDS.TryGetValue(id, out name);
                        OBJs.Add(new OBJ(id, variation, PhysicsType, unk, name)); // I'm retarded
                    }
                }
            }
            dataGridView1.DataSource = OBJs;
        }

        Dictionary<int, string> LoadOBJIds()
        {
            //List<(int, string)> ret = new List<(int, string)>();
            Dictionary<int, string> ret = new Dictionary<int, string>();
            string[] objIDS = File.ReadAllLines(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Trials Fusion Object IDs.csv");
            foreach(string line in objIDS)
            {
                string[] split = line.Split(',');
                int id = int.Parse(split[0], System.Globalization.NumberStyles.HexNumber);
                string name = split[1];
                ret.Add(id, name);
            }
            return ret;
        }


        private byte[] CompressGRP(byte[] data)
        {
            byte[] header = { 0xBA, 0xBE, 0xBA, 0xBE, 0x02, 0x4C, 0x5A, 0x4D, 0x41 };
            byte[] properties = { 0x5D, 0x00, 0x00, 0x02, 0x00 };
            byte[] decompressed_size = BitConverter.GetBytes(data.Length);



            SevenZip.CoderPropID[] coderPropIDs = { SevenZip.CoderPropID.DictionarySize, SevenZip.CoderPropID.LitContextBits, SevenZip.CoderPropID.LitPosBits, SevenZip.CoderPropID.PosStateBits };
            object[] _properties = LzmaPropertiesFromBytes(properties);
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

                using(MemoryStream ms = new MemoryStream(CompressedGRP))
                {
                    using(BinaryWriter bw = new BinaryWriter(ms))
                    {
                        bw.Write(header);
                        bw.Write(properties);
                        bw.Write(decompressed_size);
                        bw.Write(LZMA_Data);
                    }
                }

                return CompressedGRP;
            }



            //Buffer.BlockCopy()

        }

        private byte[] DecompressGRP(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    br.BaseStream.Seek(9, SeekOrigin.Begin);
                    byte[] properties = br.ReadBytes(5);
                    int decompressed_size = br.ReadInt32(); // i think

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

        // Read LZMA compression properties from a byte array
        private static object[] LzmaPropertiesFromBytes(byte[] properties)
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

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = CompressGRP(File.ReadAllBytes(ofd.FileName));
                VistaSaveFileDialog sfd = new VistaSaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (BinaryWriter bw = new BinaryWriter(File.Create(sfd.FileName)))
                    {
                        bw.Write(data);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ParseOBJA(File.ReadAllBytes(ofd.FileName));

                //byte[] decompressed = Decompress(File.ReadAllBytes(ofd.FileName));
                //SaveFile(decompressed);
            }
        }

        class BinaryReader2 : BinaryReader
        {
            public BinaryReader2(System.IO.Stream stream) : base(stream) { }

            public override int ReadInt32()
            {
                var data = base.ReadBytes(4);
                Array.Reverse(data);
                return BitConverter.ToInt32(data, 0);
            }

            public Int16 ReadInt16()
            {
                var data = base.ReadBytes(2);
                Array.Reverse(data);
                return BitConverter.ToInt16(data, 0);
            }

            public Int64 ReadInt64()
            {
                var data = base.ReadBytes(8);
                Array.Reverse(data);
                return BitConverter.ToInt64(data, 0);
            }

            public UInt32 ReadUInt32()
            {
                var data = base.ReadBytes(4);
                Array.Reverse(data);
                return BitConverter.ToUInt32(data, 0);
            }

        }
    }
}
