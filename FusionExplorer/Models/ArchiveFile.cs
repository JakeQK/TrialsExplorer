using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models
{
    public class ArchiveFile
    {
        public ArchiveFileEntry ArchiveFileEntry { get; set; }

        public string Filename { get; set; }

        public ArchiveFile()
        {

        }

        public ArchiveFile(ArchiveFileEntry archiveFileEntry, string filename)
        {
            ArchiveFileEntry = archiveFileEntry;
            Filename = filename;
        }

        public string SafeFilename => Path.GetFileName(Filename);

        public string FileDirectory => Path.GetDirectoryName(Filename);
    }
}
