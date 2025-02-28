using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reloaded.Memory.Sigscan.Definitions;
using Reloaded.Memory.Sigscan;
using System.Diagnostics;

namespace FusionExplorer
{
    public partial class ImageRipper : Form
    {
        public ImageRipper()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var process = Process.GetProcessesByName("trials_fusion")[0];
            Scanner scanner = new Scanner(process, process.MainModule);
            var multiplePatterns = new string[]
            {
                "54 35 58 ?? ?? ?? ?? 0E",
                "?? 54 38 58 ?? ?? ?? ?? 0E"
            };
            var results = scanner.FindPattern("54 35 58 ?? ?? ?? ?? 0E");
            listBox1.Items.Add(results.Offset.ToString("X"));
        }
    }
}
