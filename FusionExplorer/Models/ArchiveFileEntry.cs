using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models
{
    public class ArchiveFileEntry
    {
        public int Id { get; set; }

        public uint CompressedSize { get; set; }

        public uint DecompressedSize { get; set; }

        public byte CompressionFlag { get; set; }

        public uint OffsetToData { get; set; }

        public ArchiveFileEntry()
        {
        }

        public ArchiveFileEntry(int id, uint compressedSize, uint decompressedSize, byte compressionFlag, uint offsetToData)
        {
            Id = id;
            CompressedSize = compressedSize;
            DecompressedSize = decompressedSize;
            CompressionFlag = compressionFlag;
            OffsetToData = offsetToData;
        }

        public bool IsCompressed => CompressionFlag != 0;

        public static int SizeInBytes => sizeof(int) + sizeof(uint) * 3 + sizeof(byte);

        public bool IsFilenameTableEntry()
        {
            return Id == -576875544;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Size: {CompressedSize}/{DecompressedSize}, Offset: {OffsetToData}, Compression Flag: {CompressionFlag:X}";
        }
    }
}
