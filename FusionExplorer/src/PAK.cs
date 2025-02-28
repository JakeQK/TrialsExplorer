using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FusionExplorer.FusionExplorer;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FusionExplorer.src
{
    internal class PAK
    {
        private byte[] m_data;
        private int m_dataOffset;
        private int m_entryCount;
        private string m_filename;
        private bool m_changesMade = false;

        private static List<PakFile> m_archiveFiles = new List<PakFile>();

        public PAK(string filename) 
        {
            m_filename = filename;
            m_data = System.IO.File.ReadAllBytes(m_filename);
            ParseHeader();
            ParseFileEntries();
            GetFilenames();
        }

        public List<PakFile> GetFiles() { return m_archiveFiles; }

        private void ParseHeader()
        {
            BinaryReader br = new BinaryReader(new MemoryStream(m_data));
            if (br.ReadInt32() == Constants.pakSignature)
            {
                m_dataOffset = br.ReadInt32();
                m_entryCount = br.ReadInt32();
            }
            br.Close();
        }

        private void ParseFileEntries()
        {
            m_archiveFiles.Clear();
            BinaryReader br = new BinaryReader(new MemoryStream(m_data));
            br.BaseStream.Seek(12, SeekOrigin.Begin);
            for (int i = 0; i < m_entryCount; i++)
            {
                long fileEntryOffset = br.BaseStream.Position;
                PakFile archiveFile = new PakFile(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadByte(), br.ReadInt32(), fileEntryOffset, "");
                m_archiveFiles.Add(archiveFile);
            }
            br.Close();
        }

        private void GetFilenames()
        {
            BinaryReader br = new BinaryReader(new MemoryStream(m_data));
            for(int i = m_entryCount - 1; i != 0; i--)
            {
                if (m_archiveFiles[i].ID == Constants.filenamesEntryID)
                {
                    m_archiveFiles[i].filename = "filenames";
                    br.BaseStream.Seek(m_archiveFiles[i].dataOffset, SeekOrigin.Begin);
                    int fileCount = br.ReadInt32();
                    for(int j = 0; j < m_entryCount; j++)
                    {
                        if (m_archiveFiles[j].ID != Constants.filenamesEntryID)
                        {
                            Int16 strlen = br.ReadInt16();
                            string str = new string(br.ReadChars(strlen));
                            m_archiveFiles[j].filename = str;
                        }
                    }
                }
            }
        }

        public byte[] Export(PakFile archiveFile)
        {
            BinaryReader br = new BinaryReader(new MemoryStream(m_data));
            br.BaseStream.Seek(archiveFile.dataOffset, SeekOrigin.Begin);
            switch (archiveFile.compressionFlag)
            {
                // Uncompressed
                case 0:
                    return br.ReadBytes(archiveFile.decompressedSize);
                // zlib Compressed
                case 1:
                    return ZlibStream.UncompressBuffer(br.ReadBytes(archiveFile.compressedSize));
                // Trials Evolution (Xbox 360) compressed
                case 4:
                    MessageBox.Show("Trials Evolution: Xbox 360 compression unsupported", "Fusion Explorer");
                    break;
                // Trials Evolution: Gold Edition zlib compressed
                case 8:
                    return ZlibStream.UncompressBuffer(br.ReadBytes(archiveFile.compressedSize));
                // Unknown Case (Found in data_patch.pak)
                case 16:
                    //bw.Write(DeflateStream.UncompressBuffer(br.ReadBytes(selected_archive_file.file_entry.Decompressed_Size)));
                    return br.ReadBytes(archiveFile.compressedSize);
                // Unknown Case (Found in data_patch.pak)
                case 17:
                    //bw.Write(DeflateStream.UncompressBuffer(br.ReadBytes(selected_archive_file.file_entry.Decompressed_Size)));
                    return br.ReadBytes(archiveFile.compressedSize);
            }
            br.Close();
            return null;
        }

        public byte[] Export(int fileID)
        {
            foreach(var file in m_archiveFiles)
            {
                if (file.ID == fileID)
                    return Export(file);
                    
            }
            return null;
        }

        private void RecalculateDataOffsets(MemoryStream ms)
        {
            int offset = m_archiveFiles.Count * Constants.fileEntrySize + Constants.pakHeaderSize;

            // Update pak header data offset
            ms.Seek(4, SeekOrigin.Begin);
            ms.Write(BitConverter.GetBytes(offset), 0, 4);
            ms.Write(BitConverter.GetBytes(m_archiveFiles.Count), 0, 4);

            // Update first file entry before loop, as the calculation for data offset is different to calculation in the loop
            // First file data offset is after PAK Header and File Entries
            /*
            ms.Seek(13, SeekOrigin.Current);
            ms.Write(BitConverter.GetBytes(offset), 0, 4);

            for (int i = 1; i < m_entryCount; i++)
            {
                // Calculate offset
                offset += m_archiveFiles[i - 1].compressedSize;
                // Update data offset in m_files entry
                m_archiveFiles[i].dataOffset = offset;

                // Seek to the Data Offset value of the file entry
                ms.Seek(13, SeekOrigin.Current);
                // Write data offset
                ms.Write(BitConverter.GetBytes(offset), 0, 4);
            }
             */

            for(int i = 0; i < m_entryCount; i++)
            {
                m_archiveFiles[i].dataOffset = offset;
                ms.Seek(13, SeekOrigin.Current);
                ms.Write(BitConverter.GetBytes(offset), 0, 4);

                offset += m_archiveFiles[i].compressedSize;
            }
        }

        public void Import(PakFile archiveFile, byte[] data)
        {
            int indexOf = m_archiveFiles.IndexOf(archiveFile);

            int uncompressedLength = data.Length;
            if (archiveFile.compressionFlag == 1 || archiveFile.compressionFlag == 8)
                data = ZlibStream.CompressBuffer(data);

            MemoryStream ms = new MemoryStream(m_data);
            ms.Position = archiveFile.fileEntryOffset + 4;

            ms.Write(BitConverter.GetBytes(data.Length), 0, 4);

            ms.Write(BitConverter.GetBytes(uncompressedLength), 0, 4);

            // Set the archive sizes as we use them in the RecalculateDataOffsets function to recalculate the data offsets
            archiveFile.compressedSize = data.Length;
            archiveFile.decompressedSize = uncompressedLength;

            m_archiveFiles[indexOf] = archiveFile;

            ms.Position = archiveFile.dataOffset + archiveFile.compressedSize;

            // Step 1. Read ending stride [c]
            // Must calculate end_stride length BEFORE updating offsets, as we're taking data from the unaltered pak first
            byte[] end_stride = new byte[m_data.Length - (archiveFile.dataOffset + archiveFile.compressedSize)];
            ms.Read(end_stride, 0, end_stride.Length);

            // Step 2. Recalculate Data Offsets
            RecalculateDataOffsets(ms);

            // Step 3. Read beginning stride [a], data offset of PakFile file is the ending of the beginning stride
            // Must do this AFTER calling RecalculateDataOffsets, as we want the updated data offsets
            ms.Position = 0;
            byte[] beginning_stride = new byte[archiveFile.dataOffset];
            ms.Read(beginning_stride, 0, beginning_stride.Length);
            ms.Close();

            // Step 4. Construct new pak data like
            // [a] - Beginning stride with recalculated data offsets and file sizes
            // [b] - New file data
            // [c] - Ending stride

            byte[] newData = new byte[beginning_stride.Length + data.Length + end_stride.Length];
            Buffer.BlockCopy(beginning_stride, 0, newData, 0, beginning_stride.Length);
            Buffer.BlockCopy(data, 0, newData, beginning_stride.Length, data.Length);
            Buffer.BlockCopy(end_stride, 0, newData, beginning_stride.Length + data.Length, end_stride.Length);

            m_data = newData;
            m_changesMade = true;
        }

        public void Import(int fileID, byte[] data)
        {
            PakFile archiveFile = null;
            foreach(var file in m_archiveFiles)
            {
                if(file.ID == fileID)
                {
                    archiveFile = file;
                    break;
                }
            }

            if (archiveFile == null)
                return;

            Import(archiveFile, data);
        }

        private void AddFilename(string filename)
        {
            byte[] filenames = Export(Constants.filenamesEntryID);

            Buffer.BlockCopy(BitConverter.GetBytes(m_entryCount), 0, filenames, 0, 4);

            byte[] newFilenames = new byte[filenames.Length + filename.Length + 2];
            Buffer.BlockCopy(filenames, 0, newFilenames, 0, filenames.Length);
            MemoryStream ms = new MemoryStream(newFilenames);
            ms.Position = filenames.Length;
            ms.Write(BitConverter.GetBytes(Convert.ToInt16(filename.Length)), 0, 2);
            ms.Write(Encoding.UTF8.GetBytes(filename), 0, filename.Length);
            ms.Close();
            Import(Constants.filenamesEntryID, newFilenames);
        }

        private byte[] GetData()
        {
            int stride_start = m_dataOffset; 
            int stride_end = m_archiveFiles.Last().dataOffset + m_archiveFiles.Last().compressedSize;

            BinaryReader br = new BinaryReader(new MemoryStream(m_data));
            br.BaseStream.Seek(stride_start, SeekOrigin.Begin);
            return br.ReadBytes(stride_end - stride_start);
        }

        private byte[] GetFooter()
        {
            PakFile last = m_archiveFiles.Last();
            int stride_start = last.dataOffset + last.compressedSize;
            int stride_end = m_data.Length;

            BinaryReader br = new BinaryReader(new MemoryStream(m_data));
            br.BaseStream.Seek(stride_start, SeekOrigin.Begin);
            return br.ReadBytes(stride_end - stride_start);
        }

        private void RecalculateEntryOffsets(ref List<PakFile> entries)
        {
            int offset = Constants.pakHeaderSize + (Constants.fileEntrySize * entries.Count);
            for(int i = 0; i < entries.Count; i++)
            {
                entries[i].dataOffset = offset;
                entries[i].fileEntryOffset = Constants.pakHeaderSize + (Constants.fileEntrySize * i);
                offset += entries[i].compressedSize;
            }
        }

        public void AddFile(string filename, byte[] data)
        {
            int decompressedSize = data.Length;
            byte[] compressedData = ZlibStream.CompressBuffer(data);
            byte[] footer = GetFooter();
            AddFilename(filename);
            byte[] oldData = GetData();

            List<PakFile> newEntries = new List<PakFile>(m_archiveFiles);
            newEntries.Add(new PakFile(newEntries.Count, compressedData.Length, decompressedSize, 1, 0, 0, filename));

            RecalculateEntryOffsets(ref newEntries);

            byte[] newData = new byte[Constants.pakHeaderSize + (Constants.fileEntrySize * newEntries.Count) + 
                oldData.Length + compressedData.Length + footer.Length];

            MemoryStream ms = new MemoryStream(newData);
            ms.Write(BitConverter.GetBytes(Constants.pakSignature), 0, 4);
            ms.Write(BitConverter.GetBytes(newEntries.First().dataOffset), 0, 4);
            ms.Write(BitConverter.GetBytes(newEntries.Count), 0, 4);

            foreach(var entry in newEntries)
            {
                ms.Write(BitConverter.GetBytes(entry.ID), 0, 4);
                ms.Write(BitConverter.GetBytes(entry.compressedSize), 0, 4);
                ms.Write(BitConverter.GetBytes(entry.decompressedSize), 0, 4);
                ms.Write(BitConverter.GetBytes(entry.compressionFlag), 0, 1);
                ms.Write(BitConverter.GetBytes(entry.dataOffset), 0, 4);
            }

            ms.Write(oldData, 0, oldData.Length);
            ms.Write(compressedData, 0, compressedData.Length);
            ms.Write(footer, 0, footer.Length);

            ms.Close();

            m_archiveFiles = new List<PakFile>(newEntries);
            m_data = newData;

            m_dataOffset = m_archiveFiles.First().dataOffset;
            m_entryCount = m_archiveFiles.Count;
            m_changesMade= true;
        }

        public bool Save()
        {
            if(!m_changesMade) return false;

            System.IO.File.WriteAllBytes(m_filename, m_data);
            m_changesMade = false;
            return true;
        }

        public static class Constants
        {
            public const int pakSignature = 305419896;
            public const int filenamesEntryID = -576875544;

            // Signature[4], Data Offset[4], File Count[4]
            public const int pakHeaderSize = 12;
            // ID[4], Compressed Size[4], Uncompressed Size[4], Compression Flag[1], Data Offset[4]
            public const int fileEntrySize = 17;
        }

        public class PakFile
        {
            public PakFile(int CompressedSize, int DecompressedSize, byte CompressionFlag, int DataOffset, long FileEntryOffset)
            {
                this.compressedSize = CompressedSize;
                this.decompressedSize = DecompressedSize;
                this.compressionFlag = CompressionFlag;
                this.dataOffset = DataOffset;
                this.fileEntryOffset = FileEntryOffset;
            }

            public PakFile(int ID, int CompressedSize, int DecompressedSize, byte CompressionFlag, int DataOffset, long FileEntryOffset, string filename)
            {
                this.ID = ID;
                this.compressedSize = CompressedSize;
                this.decompressedSize = DecompressedSize;
                this.compressionFlag = CompressionFlag;
                this.dataOffset = DataOffset;
                this.fileEntryOffset = FileEntryOffset;
                this.filename = filename;
            }

            public int ID;
            public int compressedSize;
            public int decompressedSize;
            public byte compressionFlag;
            public int dataOffset;
            public long fileEntryOffset;
            public string filename;
        }
    }
}
