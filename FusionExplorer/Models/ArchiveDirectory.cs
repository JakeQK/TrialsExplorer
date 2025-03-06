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

        private Lazy<int> _totalFileCount;
        private List<IArchiveNode> _children = new List<IArchiveNode>();

        public int TotalFileCount => _totalFileCount.Value;
        public List<IArchiveNode> Children => _children;

        public ArchiveDirectory(string fullPath)
        {
            FullPath = fullPath;
            Name = Path.GetFileName(fullPath);
            _totalFileCount = new Lazy<int>(CalculateTotalFileCount);

            if (string.IsNullOrEmpty(Name))
            {
                Name = "Root";
            }
        }

        public void AddChild(IArchiveNode child)
        {
            _children.Add(child);
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

        private int CalculateTotalFileCount() 
        { 
            int count = 0;

            foreach(var child in Children)
            {
                if (child.IsDirectory)
                {
                    count += ((ArchiveDirectory)child).TotalFileCount;
                }
                else
                {
                    count++;
                }
            }

            return count;
        }
    }
}
