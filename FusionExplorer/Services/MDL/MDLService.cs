using FusionExplorer.Models.Math;
using FusionExplorer.Models.MDL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace FusionExplorer.Services
{
    public class MDLService
    {
        const int ObjSignature = 4866639;

        public MDL LoadMDL(string path)
        {
            try
            {
                MDL mdl = new MDL();
                using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(File.ReadAllBytes(path))))
                {
                    ReadAndValidateSignature(binaryReader);
                    mdl.Version = ReadMdlVersion(binaryReader);

                    if (mdl.Version != 11)
                    {
                        throw new Exception($"Invalid MDL version: {mdl.Version}");
                    }

                    int LodCount = ReadMdlLodCount(binaryReader);

                    for (int i = 0; i < LodCount; i++)
                    {
                        Lod lod = ReadLod(binaryReader);
                        mdl.Lods.Add(lod);
                    }

                }
                return mdl;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private Lod ReadLod(BinaryReader binaryReader)
        {
            Lod lod = new Lod();
            lod.Header = ReadLodHeader(binaryReader);
        }

        private LodHeader ReadLodHeader(BinaryReader binaryReader)
        {
            LodHeader lodHeader = new LodHeader();
            binaryReader.BaseStream.Seek(6, SeekOrigin.Current);

            lodHeader.LR005 = new Models.Math.Vector3(BitConverter.ToSingle(binaryReader.ReadBytes(4), 0),
                BitConverter.ToSingle(binaryReader.ReadBytes(4), 0),
                BitConverter.ToSingle(binaryReader.ReadBytes(4), 0));

            int string_len = binaryReader.ReadInt32();
            lodHeader.VisibilityString = Encoding.UTF8.GetString(binaryReader.ReadBytes(string_len));

            lodHeader.LodIndex = binaryReader.ReadInt32();

            lodHeader.Unknown1 = binaryReader.ReadInt32();
            lodHeader.Unknown2 = binaryReader.ReadInt32();

            lodHeader.MinRenderDistance = BitConverter.ToSingle(binaryReader.ReadBytes(4), 0);
            lodHeader.MaxRenderDistance = BitConverter.ToSingle(binaryReader.ReadBytes(4), 0);

            lodHeader.Unknown3 = binaryReader.ReadInt32();

            lodHeader.UnknownVector3_1 = new Models.Math.Vector3(BitConverter.ToSingle(binaryReader.ReadBytes(4), 0),
                BitConverter.ToSingle(binaryReader.ReadBytes(4), 0),
                BitConverter.ToSingle(binaryReader.ReadBytes(4), 0));

            lodHeader.UnknownVector3_2 = new Models.Math.Vector3(BitConverter.ToSingle(binaryReader.ReadBytes(4), 0),
                BitConverter.ToSingle(binaryReader.ReadBytes(4), 0),
                BitConverter.ToSingle(binaryReader.ReadBytes(4), 0));

            lodHeader.Unknown4 = binaryReader.ReadInt32();
            lodHeader.Unknown5 = binaryReader.ReadInt32();

            string_len = (int)binaryReader.ReadInt16();
            lodHeader.ID = Encoding.UTF8.GetString(binaryReader.ReadBytes(string_len));

            lodHeader.Unknown6 = binaryReader.ReadInt32();
            lodHeader.Unknown7 = binaryReader.ReadInt32();

            return lodHeader;
        }

        private Vertex ReadLodVertexData(BinaryReader binaryReader)
        {
            int vertexCount = (int)binaryReader.ReadInt16();
            int compressedDataLength = binaryReader.ReadInt32();

            byte[] compressedVertexData = binaryReader.ReadBytes(compressedDataLength);
            using(BinaryReader br = new BinaryReader(new MemoryStream(compressedVertexData)))
            {
                br.ReadInt16(); // U
                br.ReadInt16(); // V
                br.ReadInt16(); // X
                br.ReadInt16(); // Y
                br.ReadInt16(); // Z
                br.ReadInt16(); //
                br.ReadInt16(); //
                br.ReadInt16(); //
                br.ReadInt16(); //
                br.ReadInt16(); //
            }
        }

        private void ReadAndValidateSignature(BinaryReader binaryReader)
        {
            byte[] objBytes = binaryReader.ReadBytes(3);

            if (BitConverter.ToInt32(objBytes, 0) != ObjSignature)
            {
                throw new InvalidDataException("Invalid file signature");
            }
        }

        private int ReadMdlVersion(BinaryReader binaryReader)
        {
            binaryReader.BaseStream.Seek(0x04, SeekOrigin.Begin);
            string VER01 = Encoding.UTF8.GetString(binaryReader.ReadBytes(5));

            if (VER01 != "VER01")
            {
                throw new InvalidDataException("Failed to read \"VER01\" @ offset 0x04");
            }

            return binaryReader.ReadInt32();
        }
    
        private int ReadMdlLodCount(BinaryReader binaryReader)
        {
            binaryReader.BaseStream.Seek(0x0E, SeekOrigin.Begin);
            string LRS01 = Encoding.UTF8.GetString(binaryReader.ReadBytes(5));

            if (LRS01 != "LRS01")
            {
                throw new InvalidDataException("Failed to read \"LRS01\" @ offset 0x0E");
            }

            return binaryReader.ReadInt32();
        }
    }
}
