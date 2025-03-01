using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Models
{
    public interface IArchiveNode
    {
        string Name { get; }
        bool IsDirectory { get; }
        string FullPath { get; set; }
    }
}
