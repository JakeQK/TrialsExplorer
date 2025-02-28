using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ookii.Dialogs.WinForms;
using CsvHelper;

namespace FusionExplorer
{
    public partial class ArchiveBuilder : Form
    {
        public ArchiveBuilder()
        {
            InitializeComponent();
        }

        private string compression_csv_path;

        private void BuildCSV()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "pak files (*.pak)|*.pak|all files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                BinaryReader br = new BinaryReader(File.OpenRead(ofd.FileName));
                br.BaseStream.Seek(8, SeekOrigin.Begin);
                int filecount = br.ReadInt32();
                List<(int, int, string)> files = new List<(int, int, string)>();

                int FileNamesOffset = 0;
                // Collect all hashes and zip flags
                for (int i = 0; i < filecount; i++)
                {
                    int fileID = br.ReadInt32();                    // 4    File ID     #
                    br.BaseStream.Seek(8, SeekOrigin.Current);      // 8    Sizes
                    int zipflag = br.ReadByte();                    // 1    Zip Flag    #
                    int dataOffset = br.ReadInt32();                // 4    Data Offset

                    if (fileID == -576875544)
                    {
                        // Store filenames Offset for later
                        FileNamesOffset = dataOffset;
                    }
                    else
                        files.Add((fileID, zipflag, ""));
                    // store file ids, zip flags and temp name
                }

                br.BaseStream.Seek(FileNamesOffset, SeekOrigin.Begin);

                // process filenames
                int count = br.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    Int16 strlen = br.ReadInt16();
                    string filename = new string(br.ReadChars(strlen));
                    (int, int, string) temp = files[i];
                    temp.Item3 = filename;
                    files[i] = temp;
                }
                br.Close();

                VistaSaveFileDialog sfd = new VistaSaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var sw = new StreamWriter(File.Create(sfd.FileName)))
                    {
                        using (var csv = new CsvWriter(sw, System.Globalization.CultureInfo.InvariantCulture))
                        {
                            foreach (var file in files)
                            {
                                csv.WriteField(file.Item1);
                                csv.WriteField(file.Item2);
                                csv.WriteField(file.Item3);
                                csv.NextRecord();
                            }
                        }
                    }
                }
            }
        }

        private PakBuilder.Game GetGame()
        {
            switch(cbGameSelection.SelectedIndex)
            {
                case 0:
                    return PakBuilder.Game.Trials_Fusion;
                case 1:
                    return PakBuilder.Game.Trials_Evolution_GE;
                default:
                    return PakBuilder.Game.Trials_Fusion;
            }
        }

        private void btnLoadCompressionCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "csv files (*.csv)|*.csv|all files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                compression_csv_path = ofd.FileName;
                lLoadedCSV.Text = "Loaded CSV: " + ofd.SafeFileName;
            }
        }

        private void btnCreateCompressionCSV_Click(object sender, EventArgs e)
        {
            BuildCSV();
        }

        private void btnBuildArchive_Click(object sender, EventArgs e)
        {
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                var progress = new Progress<string>(s => toolStripStatusLabel1.Text = "Status: " + s);
                PakBuilder builder = new PakBuilder(fbd.SelectedPath, compression_csv_path, progress, GetGame());

                builder.Build();
            }       
        }
    }
}
