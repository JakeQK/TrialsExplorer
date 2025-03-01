using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models
{
    public class ArchiveDirectory : IArchiveNode
    {
        public string Name { get; set; }
        public bool IsDirectory => true;
        public string FullPath { get; set; }

        public List<IArchiveNode> Children { get; } = new List<IArchiveNode>();

        public ArchiveDirectory(string fullPath)
        {
            FullPath = fullPath;
            Name = Path.GetFileName(fullPath);

            if (string.IsNullOrEmpty(Name))
            {
                Name = "Root";
            }
        }

        public void AddChild(IArchiveNode child)
        {
            Children.Add(child);
        }

        public IEnumerable<ArchiveFile> GetAllFiles() 
        {
            foreach (var node in Children)
            {
                if(!node.IsDirectory)
                {
                    yield return (ArchiveFile)node;
                }
                else
                {
                    foreach (var file in ((ArchiveDirectory)node).GetAllFiles())
                    {
                        yield return file;
                    }
                }
            }
        }
    }
}
