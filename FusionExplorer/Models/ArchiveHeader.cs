using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models
{
    public class ArchiveHeader
    {
        /// <summary>
        /// File signature/magic number that identifies this as a valid archive file.
        /// </summary>
        public int FileSignature { get; set; }

        /// <summary>
        /// Offset from the beginning of the archive file to where the raw data of the files begins.
        /// </summary>
        public uint OffsetToData { get; set; }

        /// <summary>
        /// Number of file entries in this archive.
        /// </summary>
        public uint FileEntryCount { get; set; }

        /// <summary>
        /// Creates a new instance of the .pak archive header structure
        /// </summary>
        public ArchiveHeader() 
        {
        }

        /// <summary>
        /// Creates a new instance of the .pak archive header structure
        /// </summary>
        public ArchiveHeader(int fileSignature, uint offsetToData, uint fileEntryCount)
        {
            this.FileSignature = fileSignature;
            this.OffsetToData = offsetToData;
            this.FileEntryCount = fileEntryCount;
        }

        /// <summary>
        /// Validates whether this header contains a valid file signature
        /// </summary>
        public bool IsValidSignature()
        {
            return FileSignature == 305419896;
        }

        /// <summary>
        /// Returns a string representation of the header.
        /// </summary>
        public override string ToString()
        {
            return $"Signature: 0x{FileSignature:X8}, Offset: {OffsetToData}, Files: {FileEntryCount}";
        }
    }
}
