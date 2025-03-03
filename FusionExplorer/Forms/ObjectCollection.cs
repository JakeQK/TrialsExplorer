﻿using Ookii.Dialogs.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FusionExplorer
{
    public partial class ObjectCollection : Form
    {
        public ObjectCollection()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                byte[] data = File.ReadAllBytes(ofd.FileName);


                //Notepad.ShowMessage(ParseOC2(data));

                VistaSaveFileDialog sfd = new VistaSaveFileDialog();
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    using(StreamWriter sw = new StreamWriter(File.Create(sfd.FileName)))
                    {
                        sw.Write(ParseOC2(data));
                    }
                }

            }


        }

        string AttributeNameParser(int id)
        {
            switch (id)
            {
                case 2448146:
                    return "objectId";
                case -2060945186:
                    return "colorPartId";
                default: 
                    return id.ToString("X8");
            }
        }

        string ParseOC(byte[] data)
        {
            bool closed = true;
            StringBuilder sb = new StringBuilder();
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    //for (int i = 0; i < int.Parse(textBox1.Text); i++)
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        switch (reader.ReadByte())
                        {
                            case 0x00:
                                {
                                    int count = 1;
                                    while (reader.ReadByte() != 0x01)
                                    {
                                        count++;
                                        if (reader.BaseStream.Position == reader.BaseStream.Length)
                                            break;
                                    }
                                    closed = true;

                                    //File.ReadAllText()

                                    for (int j = 0; j < count; j++)
                                        sb.Append(">\n");
                                }
                                break;
                            case 0x01:
                                if (reader.ReadByte() == 0x00)
                                {
                                    // open
                                    if (closed == true)
                                        sb.Append("<");
                                    else
                                        sb.Append(" ");
                                    int id = reader.ReadInt32();
                                    sb.Append(id.ToString("X"));
                                    closed = false;
                                }
                                break;

                            case 0x03:
                                {
                                    reader.ReadInt16();
                                    int idk = reader.ReadInt32();
                                    sb.Append(string.Format(" = {0}", idk));
                                }
                                break;

                            case 0x04:
                                {
                                    reader.ReadInt16();
                                    int idk = reader.ReadInt32();
                                    sb.Append(string.Format(" = {0}", idk));
                                }
                                break;

                            case 0x06:
                                {
                                    Int16 strlen = reader.ReadInt16();
                                    string str = new string(reader.ReadChars(strlen - 1));
                                    reader.ReadByte(); // null terminator
                                    sb.Append(string.Format(" = \"{0}\"", str));
                                }
                                break;
                            case 0x05:
                                {
                                    reader.ReadInt16();
                                    float idk = reader.ReadSingle();
                                    sb.Append(string.Format(" = {0}", idk));
                                }
                                break;
                            default:
                                {
                                    Int16 size = reader.ReadInt16();
                                    byte[] idk = reader.ReadBytes(size);
                                    sb.Append(string.Format(" = {0}", idk));
                                }
                                break;
                        }
                    }
                }
            }

            return sb.ToString();
        }

        string ParseOC2(byte[] data)
        {
            bool closed = true;
            StringBuilder sb = new StringBuilder();
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    //for (int i = 0; i < int.Parse(textBox1.Text); i++)
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        switch (reader.ReadByte())
                        {
                            case 0x00: // 00 proceeded by 01 = ">", sometimes there is muliple 00's which I don't understand
                                {
                                    int count = 1;
                                    while (reader.ReadByte() != 0x01)
                                    {
                                        count++;
                                        if (reader.BaseStream.Position == reader.BaseStream.Length)
                                            break;
                                    }
                                    closed = true;

                                    //File.ReadAllText()

                                    for (int j = 0; j < count; j++)
                                        sb.Append(">\n");
                                }
                                break;
                            case 0x01: // 01 proceeded by 00 = "<" or " " depending on if it's in a state of closed
                                if (reader.ReadByte() == 0x00)
                                {
                                    // open
                                    if (closed == true)
                                        sb.Append("<");
                                    else
                                        sb.Append(" ");
                                    int id = reader.ReadInt32();
                                    string attributeName = AttributeNameParser(id);
                                    sb.Append(attributeName);
                                    closed = false;
                                }
                                break;

                            case 0x03:
                                {
                                    /*
                                    reader.ReadInt16();
                                    string value = reader.ReadInt32().ToString();
                                     */
                                    Int16 len = reader.ReadInt16();
                                    byte[] value_bytes = reader.ReadBytes(len);
                                    //Array.Reverse(value_bytes);
                                    string value = BitConverter.ToInt32(value_bytes, 0).ToString();

                                    //sb.Append(string.Format(" = {0}{1}'{2}'", "3", value.Length, value));
                                    sb.Append(string.Format("=\"{0}\"", value));
                                }
                                break;

                            case 0x04:
                                {
                                    Int16 len = reader.ReadInt16();
                                    byte[] value_bytes = reader.ReadBytes(len);
                                    //Array.Reverse(value_bytes);
                                    string value = BitConverter.ToInt32(value_bytes, 0).ToString();

                                    //sb.Append(string.Format(" = {0}{1}'{2}'", "4", value.Length, value));
                                    sb.Append(string.Format("=\"{0}\"", value));
                                }
                                break;

                            case 0x06:
                                {
                                    Int16 strlen = reader.ReadInt16();
                                    string str = new string(reader.ReadChars(strlen - 1));
                                    reader.ReadByte(); // null terminator
                                    //sb.Append(string.Format(" = {0}{1}'{2}'", "6", str.Length, str));
                                    sb.Append(string.Format("=\"{0}\"", str));
                                }
                                break;
                            case 0x05:
                                {
                                    Int16 len = reader.ReadInt16();
                                    byte[] value_bytes = reader.ReadBytes(len);
                                    //Array.Reverse(value_bytes);
                                    string value = BitConverter.ToSingle(value_bytes, 0).ToString();

                                    //sb.Append(string.Format(" = {0}{1}'{2}'", "5", value.Length, value));
                                    sb.Append(string.Format("=\"{0}\"", value));
                                }
                                break;
                            default:
                                {
                                    Int16 len = reader.ReadInt16();
                                    byte[] value_bytes = reader.ReadBytes(len);
                                    //Array.Reverse(value_bytes);
                                    string value = BitConverter.ToInt32(value_bytes, 0).ToString();

                                    //sb.Append(string.Format(" = {0}{1}'{2}'", "3", value.Length, value));
                                    sb.Append(string.Format("=\"{0}\"", value));
                                }
                                break;
                        }
                    }
                }
            }

            return sb.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
