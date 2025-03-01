using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models
{
    public class ArchiveFile : IArchiveNode
    {
        public ArchiveFileEntry ArchiveFileEntry { get; set; }

        public string Name => Path.GetFileName(FullPath); 
        public bool IsDirectory => false;
        public string FullPath { get; set; }

        public ArchiveFile()
        {

        }

        public ArchiveFile(ArchiveFileEntry archiveFileEntry, string filename)
        {
            ArchiveFileEntry = archiveFileEntry;
            FullPath = filename;
        }

        public string SafeFilename => Name;

        public string DirectoryPath => Path.GetDirectoryName(FullPath);

        public bool IsFilenameTable => ArchiveFileEntry.IsFilenameTableEntry();
    }
}
