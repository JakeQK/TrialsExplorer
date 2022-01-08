using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using Ionic.Zlib;
using System.Runtime.Serialization.Formatters.Binary;
using Ookii.Dialogs.WinForms;

// Collect all file paths
// Go through files and match name to ID

namespace FusionExplorer
{
    class PakBuilder
    {
        private static string _folder_path;
        private static string _compression_csv_path;
        private static Game _game;

        private static IProgress<string> _progress;

        public enum Game
        {
            Trials_Fusion,
            Trials_Evolution_GE
        }

        public PakBuilder(string folder_path, string compression_csv_path, IProgress<string> progress, Game game = Game.Trials_Fusion)
        {
            _folder_path = folder_path;
            _compression_csv_path = compression_csv_path;
            _progress = progress;
            _game = game;
        }

        public async void Build()
        {
            byte[] data = null;
            await Task.Factory.StartNew(() => data = _Build(), TaskCreationOptions.LongRunning);

            VistaSaveFileDialog sfd = new VistaSaveFileDialog();
            sfd.Filter = "pak archive (*.pak)|*.pak";
            int index = _folder_path.LastIndexOf("\\");
            string folder = _folder_path.Substring(index + 1, _folder_path.Length - index - 1);
            sfd.FileName = folder + ".pak";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BinaryWriter bw = new BinaryWriter(File.Create(sfd.FileName));
                bw.Write(data);
                bw.Close();
                _progress.Report("complete");
            }
        }



        private static byte[] _Build()
        {
            string[] Filepaths = CollectFilepaths();                        // Collect all filepaths
            List<FileEntry> FileHeaders = BuildFileHeaders(Filepaths);     // Build all files headers
            return BuildArchive(Filepaths, FileHeaders);                // Build Data
        }

        private static string[] CollectFilepaths()
        {
            return Directory.GetFiles(_folder_path, "*.*", System.IO.SearchOption.AllDirectories);
        }

        private static List<FileEntry> BuildFileHeaders(string[] Filepaths)
        {
            Dictionary<string, int[]> CSV = LoadCompressionCSV();
            List<FileEntry> FileHeaders = new List<FileEntry>();
            int CollectiveOffset = 12 + 17 * Filepaths.Length + 17;         // Header Size (12) + File Header Size * File Count (17 * x) + Filenames File Header (17)
            int FilenamesSize = 4;                                          // File Count (4)   -   for building filenames file header, need to calculate size
            int CustomFileCount = 0;                                        // For creating ID for custom files
            
            // Construct FileEntry for each file
            for(int i = 0; i < Filepaths.Length; i++)
            {
                // Size for the file names FileEntry {int16 strlen, string}
                FilenamesSize += Filepaths[i].Length - _folder_path.Length + 1;         // c:\asdas\asdsa\Areavolumes.xml - c:\asdas\asdsa\ + 2 (string length)

                int[] info;
                bool verification = VerifyFile(CSV, Filepaths[i], out info);         // Check if file is a game file and if so, get it's ID and Zip Flag

                // File is game file
                if (verification)
                {
                    FileEntry temp = new FileEntry();
                    if (info[0] != 0)
                    {
                        switch(_game)
                        {
                            case Game.Trials_Fusion:
                                temp.ID = 1;
                                break;
                            case Game.Trials_Evolution_GE:
                                temp.ID = 8;
                                break;
                            default:
                                temp.ID = 1;
                                break;
                                
                        }
                    }
                    else
                        temp.ID = 0;                                                // ID
                    temp.ZipFlag = BitConverter.GetBytes(info[1])[0];               // Zip Flag
                    temp.Size = (int)new System.IO.FileInfo(Filepaths[i]).Length;       // Size

                    if (temp.ZipFlag == 0x00)
                        temp.ZipSize = temp.Size;                                   // Zip Size - No compression
                    else
                    {
                        int zSize = ZlibStream.CompressBuffer(File.ReadAllBytes(Filepaths[i])).Length;
                        temp.ZipSize = zSize;                                       // Zip Size - Compression
                    }

                    temp.DataOffset = CollectiveOffset;                             // Data Offset
                    FileHeaders.Add(temp);
                    CollectiveOffset += temp.ZipSize;
                }
                else // File is custom file
                {
                    FileEntry temp = new FileEntry();
                    CustomFileCount++;
                    temp.ID = CustomFileCount;
                    temp.ZipSize = temp.Size = (int)new System.IO.FileInfo(Filepaths[i]).Length;
                    temp.ZipFlag = 0x00;
                    temp.DataOffset = CollectiveOffset;
                    FileHeaders.Add(temp);
                    CollectiveOffset += temp.ZipSize;
                }

                _progress.Report(string.Format("{0} / {1} File entries constructed", i, Filepaths.Length));
            }

            FileEntry filenamesHeader = new FileEntry(); // Create filenames file header
            filenamesHeader.ID = -576875544;
            filenamesHeader.ZipSize = FilenamesSize;
            filenamesHeader.Size = FilenamesSize;
            filenamesHeader.ZipFlag = 0x00;
            filenamesHeader.DataOffset = CollectiveOffset;
            FileHeaders.Add(filenamesHeader);

            return FileHeaders;
        }

        private static byte[] BuildArchive(string[] Filepaths, List<FileEntry> FileHeaders, bool NoFooter = false)
        {
            int DataSize = 0;

            DataSize += 12;                                     // Archive header size
            foreach (FileEntry fileHeader in FileHeaders)
                DataSize += fileHeader.ZipSize + 17;            // File data section size + File header section size
            
            if(NoFooter == false)
                DataSize += 0x80;                                   // Footer Size

            byte[] Data = new byte[DataSize];

            _progress.Report("Archive Size Calculated");


            // Archive Header
            Buffer.BlockCopy(new byte[] { 0x78, 0x56, 0x34, 0x12 }, 0, Data, 0, 4);             // File Signature
            Buffer.BlockCopy(BitConverter.GetBytes(FileHeaders[0].DataOffset), 0, Data, 4, 4);  // Data Offset
            Buffer.BlockCopy(BitConverter.GetBytes(FileHeaders.Count), 0, Data, 8, 4);          // File Count

            _progress.Report("Header written");

            // File Entries Section
            for (int i = 0; i < FileHeaders.Count; i++)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(FileHeaders[i].ID), 0, Data, 12 + i * 17, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(FileHeaders[i].ZipSize), 0, Data, 16 + i * 17, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(FileHeaders[i].Size), 0, Data, 20 + i * 17, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(FileHeaders[i].ZipFlag), 0, Data, 24 + i * 17, 1);
                Buffer.BlockCopy(BitConverter.GetBytes(FileHeaders[i].DataOffset), 0, Data, 25 + i * 17, 4);
            }

            _progress.Report("File entries written");

            // File Data Section
            for (int i = 0; i < Filepaths.Length; i++)
            {
                if (FileHeaders[i].ZipFlag == 0x00)
                {
                    Buffer.BlockCopy(File.ReadAllBytes(Filepaths[i]), 0, Data, FileHeaders[i].DataOffset, FileHeaders[i].ZipSize);
                }
                else
                {
                    Buffer.BlockCopy(ZlibStream.CompressBuffer(File.ReadAllBytes(Filepaths[i])), 0, Data, FileHeaders[i].DataOffset, FileHeaders[i].ZipSize);
                }

                _progress.Report(string.Format("{0} / {1} file data written", i, Filepaths.Length));
            }

            // Filenames
            Buffer.BlockCopy(BitConverter.GetBytes(Filepaths.Length), 0, Data, FileHeaders[FileHeaders.Count - 1].DataOffset, 4);
            int FilenamesCurPos = FileHeaders[FileHeaders.Count - 1].DataOffset + 4;
            for (int i = 0; i < Filepaths.Length; i++)
            {
                string fp = Filepaths[i].Replace(_folder_path + "\\", "").Replace("\\", "/");
                UInt16 strlen = (UInt16)fp.Length;
                Buffer.BlockCopy(BitConverter.GetBytes(strlen), 0, Data, FilenamesCurPos, 2);
                Buffer.BlockCopy(Encoding.UTF8.GetBytes(fp), 0, Data, FilenamesCurPos + 2, strlen);
                FilenamesCurPos += 2 + strlen;
                _progress.Report(string.Format("{0} / {1} file names written", i, Filepaths.Length));
            }

            // Archive Footer
            if (NoFooter == false)
            {
                byte[] footer = { 0x0B, 0xC6, 0xAD, 0x20, 0x73, 0x71, 0x06, 0x8E, 0xDD, 0x85, 0x56, 0xBA, 0x00, 0x8A,
                0x06, 0x9D, 0x93, 0x59, 0x14, 0x84, 0x2A, 0x90, 0xD5, 0x06, 0x74, 0x70, 0xBC, 0xFF, 0x45, 0xBE,
                0xF9, 0x51, 0xED, 0xDE, 0xE8, 0x2F, 0xA4, 0x43, 0x3D, 0x9E, 0x65, 0x6D, 0x18, 0x10, 0x1C, 0x3B,
                0xFE, 0xD6, 0x29, 0xF8, 0x71, 0x9B, 0xD4, 0x40, 0xD4, 0xC1, 0x05, 0xC0, 0x5D, 0x58, 0xCE, 0xE7,
                0xBA, 0x74, 0x67, 0x00, 0x7E, 0x78, 0xCB, 0xBD, 0xEB, 0xA8, 0xD2, 0xCA, 0x4C, 0x95, 0xDF, 0x63,
                0xCF, 0xA9, 0x21, 0x82, 0xE7, 0xFD, 0x86, 0xDB, 0x51, 0x8B, 0x41, 0x92, 0xE8, 0x9C, 0x46, 0xC9,
                0xFE, 0x07, 0xA6, 0x87, 0xE3, 0x25, 0xBB, 0xE1, 0xC0, 0x74, 0xAB, 0x37, 0x13, 0x63, 0x06, 0xBB,
                0x50, 0x40, 0xBB, 0x1A, 0x1B, 0xFE, 0x57, 0xDA, 0xAD, 0xCF, 0x27, 0x69, 0xDE, 0xC3, 0xD5, 0xF1,
                0x41, 0x76 };
                Buffer.BlockCopy(footer, 0, Data, Data.Length - 0x80, 0x80);
            }

            return Data;
        }



        private static Dictionary<string, int[]> LoadCompressionCSV()
        {
            if (_compression_csv_path != null)
            {
                Dictionary<string, int[]> CSV = new Dictionary<string, int[]>();
                string[] lines = File.ReadAllLines(_compression_csv_path);
                TextFieldParser parser;
                Stream lineStream;
                foreach (string line in lines)
                {
                    lineStream = GenerateStreamFromString(line);
                    parser = new TextFieldParser(lineStream);
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    string[] fields = parser.ReadFields();
                    parser.Close();
                    CSV.Add(fields[2], new int[] { int.Parse(fields[0]), int.Parse(fields[1]) });
                }
                return CSV;
            }
            else
            {
                Dictionary<string, int[]> ret = new Dictionary<string, int[]>();
                ret.Add("", new int[] { 0, 0 });
                return ret;
            }
        }

        private static bool VerifyFile(Dictionary<string, int[]> CSVDict, string filename, out int[] data)
        {
            string safefilename = filename.Replace(_folder_path + "\\", "").Replace("\\", "/");
            int[] values;
            if (CSVDict.TryGetValue(safefilename, out values))
            {
                data = values;
                return true;
            }

            data = null;
            return false;
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private class FileEntry
        {
            public int ID { get; set; }
            public int ZipSize { get; set; }
            public int Size { get; set; }
            public byte ZipFlag { get; set; }
            public int DataOffset { get; set; }

            public FileEntry()
            {

            }

            public FileEntry(int id, int zipsize, int size, byte zipflag, int dataoffset)
            {
                ID = id;
                ZipSize = zipsize;
                Size = size;
                ZipFlag = zipflag;
                DataOffset = dataoffset;
            }
        }
    }
}
