using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer {
    class ArchiveExporter {
        private byte[] archive_signature = { 0x78, 0x56, 0x34, 0x12 };
        private string archive_location;
        private bool display_info_only;
        public ArchiveExporter(string archive_location, bool display_info_only = false) {
            this.archive_location = archive_location;
            this.display_info_only = display_info_only;
        }

        public void ParseFileEntries() {
            BinaryReader br = new BinaryReader(File.OpenRead(archive_location));
            byte[] file_signature = br.ReadBytes(4);
            if (file_signature.SequenceEqual(archive_signature)) {
                int Data_Offset = br.ReadInt32();
                int File_Count = br.ReadInt32();
                for(int i = 0; i < File_Count; i++) {
                    FileEntry fileEntry = new FileEntry(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadByte(), br.ReadInt32());
                }
            }
            br.Close();
        }



        public class ArchiveFile {
            public FileEntry fileEntry;
            public string filename;
        }

        public class FileEntry {
            public FileEntry(int Compressed_Size, int Decompressed_Size, byte Compression_Flag, int Data_Offset) {
                this.Compressed_Size = Compressed_Size;
                this.Decompressed_Size = Decompressed_Size;
                this.Compression_Flag = Compression_Flag;
                this.Data_Offset = Data_Offset;
            }

            public FileEntry(int Unk1, int Compressed_Size, int Decompressed_Size, byte Compression_Flag, int Data_Offset) {
                this.Unk1 = Unk1;
                this.Compressed_Size = Compressed_Size;
                this.Decompressed_Size = Decompressed_Size;
                this.Compression_Flag = Compression_Flag;
                this.Data_Offset = Data_Offset;
            }

            public int Unk1;
            public int Compressed_Size;
            public int Decompressed_Size;
            public byte Compression_Flag;
            public int Data_Offset;
        }


    }
}
