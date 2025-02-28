using FusionExplorer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition.Primitives;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace FusionExplorer.src
{
    public partial class Customization : Form
    {
        public Customization()
        {
            InitializeComponent();

            helmets.Add(new ITEM("TECHBOY_HEAD_1", "795375336", "3440826863", "1", "images\\garage\\rider\\Techboy_A_Head.png"));
            helmets.Add(new ITEM("TECHBOY_HEAD_2", "655737260", "3440826863", "2", "images\\garage\\rider\\Techboy_B_Head.png"));
            helmets.Add(new ITEM("TECHBOY_HEAD_4", "197963867", "3440826863", "4", "images\\garage\\rider\\Techboy_C_Head.png"));
            helmets.Add(new ITEM("SQUIRREL_HELMET_1", "0", "1786136959", "1", "images\\garage\\rider\\Squirrel_Head.png"));
            helmets.Add(new ITEM("EVEL_KNIEVEL_HELMETS_1", "191514486", "3917481059", "1", "images\\garage\\rider\\EvelKnievel_Head.png"));
            helmets.Add(new ITEM("DIRTRUNNER_HELMETS_1", "48332168", "4228964176", "1", "images\\garage\\rider\\DirtRunner_A_Head.png"));
            helmets.Add(new ITEM("DIRTRUNNER_HELMETS_2", "117779310", "4228964176", "2", "images\\garage\\rider\\DirtRunner_B_Head.png"));
            helmets.Add(new ITEM("DIRTRUNNER_HELMETS_4", "260842230", "4228964176", "4", "images\\garage\\rider\\DirtRunner_C_Head.png"));
            helmets.Add(new ITEM("XPOLICE_HELMETS_1", "345901791", "2019321888", "1", "images\\garage\\rider\\XPolice_A_Head.png"));
            helmets.Add(new ITEM("XPOLICE_HELMETS_2", "58021060", "2019321888", "2", "images\\garage\\rider\\XPolice_B_Head.png"));
            helmets.Add(new ITEM("XPOLICE_HELMETS_4", "33120296", "2019321888", "4", "images\\garage\\rider\\XPolice_C_Head.png"));
            helmets.Add(new ITEM("FMX_FREAK_HELMET_1", "131029524", "1198491454", "1", "images\\garage\\rider\\FMX_Freak_A_Head.png"));
            helmets.Add(new ITEM("FMX_FREAK_HELMET_2", "463625756", "1198491454", "2", "images\\garage\\rider\\FMX_Freak_B_Head.png"));
            helmets.Add(new ITEM("FMX_FREAK_HELMET_4", "154870870", "1198491454", "4", "images\\garage\\rider\\FMX_Freak_C_Head.png"));
            helmets.Add(new ITEM("GEOLOGIST_HELMET_1", "685581760", "1920525124", "1", "images\\garage\\rider\\Geologist_A_Head.png"));
            helmets.Add(new ITEM("GEOLOGIST_HELMET_2", "136722765", "1920525124", "2", "images\\garage\\rider\\Geologist_B_Head.png"));
            helmets.Add(new ITEM("GEOLOGIST_HELMET_4", "925277925", "1920525124", "4", "images\\garage\\rider\\Geologist_C_Head.png"));
            helmets.Add(new ITEM("HAZMASUIT_HELMET_1", "638201233", "852916207", "1", "images\\garage\\rider\\HazmaSuit_Head.png"));
            helmets.Add(new ITEM("ULCCLOWN_HELMET_1", "0", "3195417312", "1", "images\\garage\\rider\\SuperHero_Head.png"));
            helmets.Add(new ITEM("ULCDRINK_HELMET_1", "0", "2572910634", "1", "images\\garage\\rider\\Clown_Head.png"));
            helmets.Add(new ITEM("ULCRHINO_HELMET_1", "0", "3475818384", "1", "images\\garage\\rider\\Drink_Head.png"));
            helmets.Add(new ITEM("SUPERHERO_HELMET_1", "48391905", "2121301039", "1", "images\\garage\\rider\\Rhino_Head.png"));

            tops.Add(new ITEM("TECHBOY_TOP_1", "318519819", "2729008919", "1", "images\\garage\\rider\\Techboy_A_Body.png"));
            tops.Add(new ITEM("TECHBOY_TOP_2", "243358698", "2729008919", "2", "images\\garage\\rider\\Techboy_B_Body.png"));
            tops.Add(new ITEM("TECHBOY_TOP_4", "190033754", "2729008919", "4", "images\\garage\\rider\\Techboy_C_Body.png"));
            tops.Add(new ITEM("EVEL_KNIEVEL_TOPS_1", "555398537", "2970472106", "1", "images\\garage\\rider\\EvelKnievel_Body.png"));
            tops.Add(new ITEM("DIRTRUNNER_TOPS_1", "758847557", "2205109786", "1", "images\\garage\\rider\\DirtRunner_A_Body.png"));
            tops.Add(new ITEM("DIRTRUNNER_TOPS_2", "589395675", "2205109786", "2", "images\\garage\\rider\\DirtRunner_B_Body.png"));
            tops.Add(new ITEM("DIRTRUNNER_TOPS_4", "578602806", "2205109786", "4", "images\\garage\\rider\\DirtRunner_C_Body.png"));
            tops.Add(new ITEM("XPOLICE_TOPS_1", "290461986", "3836199735", "1", "images\\garage\\rider\\XPolice_A_Body.png"));
            tops.Add(new ITEM("XPOLICE_TOPS_2", "64913407", "3836199735", "2", "images\\garage\\rider\\XPolice_B_Body.png"));
            tops.Add(new ITEM("XPOLICE_TOPS_4", "1862267", "3836199735", "4", "images\\garage\\rider\\XPolice_C_Body.png"));
            tops.Add(new ITEM("FMX_FREAK_TOP_1", "355274144", "3240576765", "1", "images\\garage\\rider\\FMX_Freak_A_Body.png"));
            tops.Add(new ITEM("FMX_FREAK_TOP_2", "719387552", "3240576765", "2", "images\\garage\\rider\\FMX_Freak_B_Body.png"));
            tops.Add(new ITEM("FMX_FREAK_TOP_4", "626267300", "3240576765", "4", "images\\garage\\rider\\FMX_Freak_C_Body.png"));
            tops.Add(new ITEM("GEOLOGIST_TOP_1", "378717200", "3665689525", "1", "images\\garage\\rider\\Geologist_A_Body.png"));
            tops.Add(new ITEM("GEOLOGIST_TOP_2", "128962908", "3665689525", "2", "images\\garage\\rider\\Geologist_B_Body.png"));
            tops.Add(new ITEM("GEOLOGIST_TOP_4", "114588392", "3665689525", "4", "images\\garage\\rider\\Geologist_C_Body.png"));
            tops.Add(new ITEM("HAZMASUIT_TOP_1", "83748544", "1682904899", "1", "images\\garage\\rider\\HazmaSuit_Body.png"));
            tops.Add(new ITEM("SUPERHERO_TORSO_1", "44450438", "1467640519", "1", "images\\garage\\rider\\SuperHero_Body.png"));

            bottoms.Add(new ITEM("TECHBOY_BOTTOM_1", "239769300", "3486890472", "1", "images\\garage\\rider\\Techboy_A_Pants.png"));
            bottoms.Add(new ITEM("TECHBOY_BOTTOM_2", "260479147", "3486890472", "2", "images\\garage\\rider\\Techboy_B_Pants.png"));
            bottoms.Add(new ITEM("TECHBOY_BOTTOM_4", "139120705", "3486890472", "4", "images\\garage\\rider\\Techboy_C_Pants.png"));
            bottoms.Add(new ITEM("EVEL_KNIEVEL_BOTTOMS_1", "46220238", "2886343732", "1", "images\\garage\\rider\\EvelKnievel_Pants.png"));
            bottoms.Add(new ITEM("DIRTRUNNER_BOTTOMS_1", "669180435", "621293020", "1", "images\\garage\\rider\\DirtRunner_A_Pants.png"));
            bottoms.Add(new ITEM("DIRTRUNNER_BOTTOMS_2", "204453090", "621293020", "2", "images\\garage\\rider\\DirtRunner_B_Pants.png"));
            bottoms.Add(new ITEM("DIRTRUNNER_BOTTOMS_4", "216885466", "621293020", "4", "images\\garage\\rider\\DirtRunner_C_Pants.png"));
            bottoms.Add(new ITEM("XPOLICE_BOTTOM_1", "129251080", "730734378", "1", "images\\garage\\rider\\XPolice_A_Pants.png"));
            bottoms.Add(new ITEM("XPOLICE_BOTTOM_2", "76388049", "730734378", "2", "images\\garage\\rider\\XPolice_B_Pants.png"));
            bottoms.Add(new ITEM("XPOLICE_BOTTOM_4", "410249796", "730734378", "4", "images\\garage\\rider\\XPolice_C_Pants.png"));
            bottoms.Add(new ITEM("FMX_FREAK_BOTTOM_1", "117517125", "3641660362", "1", "images\\garage\\rider\\FMX_Freak_A_Pants.png"));
            bottoms.Add(new ITEM("FMX_FREAK_BOTTOM_2", "309031857", "3641660362", "2", "images\\garage\\rider\\FMX_Freak_B_Pants.png"));
            bottoms.Add(new ITEM("FMX_FREAK_BOTTOM_4", "413476822", "3641660362", "4", "images\\garage\\rider\\FMX_Freak_C_Pants.png"));
            bottoms.Add(new ITEM("GEOLOGIST_BOTTOM_1", "286743744", "2795854687", "1", "images\\garage\\rider\\Geologist_A_Pants.png"));
            bottoms.Add(new ITEM("GEOLOGIST_BOTTOM_2", "382613160", "2795854687", "2", "images\\garage\\rider\\Geologist_B_Pants.png"));
            bottoms.Add(new ITEM("GEOLOGIST_BOTTOM_4", "233189762", "2795854687", "4", "images\\garage\\rider\\Geologist_C_Pants.png"));
            bottoms.Add(new ITEM("HAZMASUIT_BOTTOM_1", "254757484", "448314530", "1", "images\\garage\\rider\\HazmaSuit_Pants.png"));
        }

        List<ITEM> helmets = new List<ITEM>();
        List<ITEM> tops = new List<ITEM>();
        List<ITEM> bottoms = new List<ITEM>();

        XmlDocument xDoc;
        MemoryStream ms;
        PAK dataPak;
        PAK miscPak;

        struct ITEM
        {
            public ITEM(string genKey, string colorPartId, string objectId, string variationId, string icon)
            {
                this.genKey = genKey;
                this.colorPartId = colorPartId;
                this.objectId = objectId;
                this.variationId = variationId;
                this.icon = icon;
            }

            public string genKey;
            public string colorPartId;
            public string objectId;
            public string variationId;
            public string icon;
        }

        // node.Attributes[2].Value colorPartId
        // node.Attributes[4].Value objectId
        // node.Attributes[6].Value variationId

        private void cbTops_SelectedIndexChanged(object sender, EventArgs e)
        {
            pbTop.Image = ilTops.Images[cbTops.SelectedIndex];
            XmlNode node = xDoc.SelectSingleNode("gear/items/item[@id='1']");
            node.Attributes[2].Value = tops[cbTops.SelectedIndex].colorPartId;
            node.Attributes[4].Value = tops[cbTops.SelectedIndex].objectId;
            node.Attributes[6].Value = tops[cbTops.SelectedIndex].variationId;
        }

        private void cbHelmets_SelectedIndexChanged(object sender, EventArgs e)
        {
            pbHelmet.Image = ilHelmets.Images[cbHelmets.SelectedIndex];
            XmlNode node = xDoc.SelectSingleNode("gear/items/item[@id='7']");
            node.Attributes[2].Value = helmets[cbHelmets.SelectedIndex].colorPartId;
            node.Attributes[4].Value = helmets[cbHelmets.SelectedIndex].objectId;
            node.Attributes[6].Value = helmets[cbHelmets.SelectedIndex].variationId;
        }

        private void cbBottom_SelectedIndexChanged(object sender, EventArgs e)
        {
            pbBottom.Image = ilBottoms.Images[cbBottom.SelectedIndex];
            XmlNode node = xDoc.SelectSingleNode("gear/items/item[@id='4']");
            node.Attributes[2].Value = bottoms[cbBottom.SelectedIndex].colorPartId;
            node.Attributes[4].Value = bottoms[cbBottom.SelectedIndex].objectId;
            node.Attributes[6].Value = bottoms[cbBottom.SelectedIndex].variationId;
        }

        private XmlAttribute CreateAttribute(XmlDocument xDoc, string name, string value)
        {
            XmlAttribute return_value = xDoc.CreateAttribute(name);
            return_value.Value = value;
            return return_value;
        }

        private void HijackRider(XmlDocument xDoc)
        {
            XmlNode node = xDoc.SelectSingleNode("gear/riders/rider[@id='1']");
            node.Attributes[0].Value = "GRAPHICS_GENERAL_CUSTOM";
            node.Attributes[3].Value = "images/CustomRider.png";

            node = xDoc.SelectSingleNode("gear/riders/rider[@id='1']/groups/group[@name='TOP']/set[@id='2']");
            if(node != null)
                node.ParentNode.RemoveChild(node);

            node = xDoc.SelectSingleNode("gear/riders/rider[@id='1']/groups/group[@name='TOP']/set[@id='3']");
            if (node != null)
                node.ParentNode.RemoveChild(node);

            node = xDoc.SelectSingleNode("gear/riders/rider[@id='1']/groups/group[@name='BOTTOM']/set[@id='5']");
            if (node != null)
                node.ParentNode.RemoveChild(node);

            node = xDoc.SelectSingleNode("gear/riders/rider[@id='1']/groups/group[@name='BOTTOM']/set[@id='6']");
            if (node != null)
                node.ParentNode.RemoveChild(node);

            node = xDoc.SelectSingleNode("gear/riders/rider[@id='1']/groups/group[@name='HELMET']/set[@id='8']");
            if (node != null)
                node.ParentNode.RemoveChild(node);

            node = xDoc.SelectSingleNode("gear/riders/rider[@id='1']/groups/group[@name='HELMET']/set[@id='9']");
            if (node != null)
                node.ParentNode.RemoveChild(node);
        }

        private void HijackSets(XmlDocument xDoc)
        {
            XmlNode node = xDoc.SelectSingleNode("gear/sets/set[@id='1']");
            node.Attributes[3].Value = "images/Gear_01.png";
            node.Attributes[5].Value = "0";
            node.Attributes[6].Value = "0";

            node = xDoc.SelectSingleNode("gear/sets/set[@id='4']");
            node.Attributes[3].Value = "images/Gear_02.png";
            node.Attributes[5].Value = "0";
            node.Attributes[6].Value = "0";

            node = xDoc.SelectSingleNode("gear/sets/set[@id='7']");
            node.Attributes[3].Value = "images/Gear_00.png";
            node.Attributes[5].Value = "0";
            node.Attributes[6].Value = "0";
        }

        private void RetrieveCurrentValues(XmlDocument xDoc)
        {
            XmlNode node = xDoc.SelectSingleNode("gear/items/item[@id='1']"); // SHIRT
            for (int i = 0; i < tops.Count; i++)
            {
                if (tops[i].objectId == node.Attributes[4].Value && tops[i].variationId == node.Attributes[6].Value)
                {
                    pbTop.Image = ilTops.Images[i];
                    cbTops.SelectedIndex = i;
                    break;
                }
            }

            node = xDoc.SelectSingleNode("gear/items/item[@id='4']"); // BOTTOM
            for (int i = 0; i < bottoms.Count; i++)
            {
                if (bottoms[i].objectId == node.Attributes[4].Value && bottoms[i].variationId == node.Attributes[6].Value)
                {
                    pbBottom.Image = ilBottoms.Images[i];
                    cbBottom.SelectedIndex = i;
                    break;
                }
            }

            node = xDoc.SelectSingleNode("gear/items/item[@id='7']"); // HELMET
            for (int i = 0; i < helmets.Count; i++)
            {
                if (helmets[i].objectId == node.Attributes[4].Value && helmets[i].variationId == node.Attributes[6].Value)
                {
                    pbHelmet.Image = ilHelmets.Images[i];
                    cbHelmets.SelectedIndex = i;
                    break;
                }
            }

            // node.Attributes[2].Value colorPartId
            // node.Attributes[4].Value objectId
            // node.Attributes[6].Value variationId
        }

        private void InputCustomRiderImage(string path, string filename)
        {
            string data_miscPath = path.Substring(0, path.Length - 8) + "data_misc.pak";

            miscPak = new PAK(data_miscPath);

            bool found = false;
            foreach(var file in miscPak.GetFiles())
            {
                if (file.filename == "images/CustomRider.png.tex")
                {
                    found = true;
                    break;
                }
            }

            if(!found)
            {
                byte[] tex = Texture.DDStoTEX(Resources.CustomRider, Texture.TextureVersion.T5X);
                miscPak.AddFile("images/CustomRider.png.tex", tex);
            }
        }

        private void AddNewFile(string pak_path, string file_path, string filename)
        {
            PAK pak = new PAK(pak_path);
            byte[] data = File.ReadAllBytes(file_path);
            pak.AddFile(filename, data);
            pak.Save();
        }

        private void AddNewDDS(string pak_path, string dds_path, string filename)
        {
            PAK pak = new PAK(pak_path);
            byte[] data = File.ReadAllBytes(dds_path);
            byte[] tex = Texture.DDStoTEX(data, Texture.TextureVersion.T5X);
            pak.AddFile(filename, tex);
            pak.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tmpfile = "tmp.tmp";
            xDoc.Save(tmpfile);

            BinaryReader br = new BinaryReader(File.OpenRead(tmpfile));
            dataPak.Import(-1899826158, br.ReadBytes((int)br.BaseStream.Length));
            br.Close();
            dataPak.Save();
            miscPak.Save();
            File.Delete(tmpfile);
            MessageBox.Show("Saved");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                dataPak = new PAK(ofd.FileName);
                xDoc = new XmlDocument();
                ms = new MemoryStream(dataPak.Export(-1899826158));
                xDoc.Load(ms);

                HijackRider(xDoc);
                HijackSets(xDoc);
                RetrieveCurrentValues(xDoc);
                //InputCustomRiderImage(ofd.FileName);

                button1.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Restore Rider
            XmlNode node = xDoc.SelectSingleNode("gear/riders/rider[@id='1']");
            node.Attributes[0].Value = "ridergear_category_techboy";
            node.Attributes[3].Value = "images\\garage\\rider\\Tech_Boy.png";

            // Restore TOP sets
            node = xDoc.SelectSingleNode("gear/riders/rider[@id='1']/groups/group[@name='TOP']");

            XmlElement element = xDoc.CreateElement("set");
            element.Attributes.Append(CreateAttribute(xDoc, "id", "2"));
            node.AppendChild(element);

            element = xDoc.CreateElement("set");
            element.Attributes.Append(CreateAttribute(xDoc, "id", "3"));
            node.AppendChild(element);

            // Restore BOTTOM sets
            node = xDoc.SelectSingleNode("gear/riders/rider[@id='1']/groups/group[@name='BOTTOM']");

            element = xDoc.CreateElement("set");
            element.Attributes.Append(CreateAttribute(xDoc, "id", "5"));
            node.AppendChild(element);

            element = xDoc.CreateElement("set");
            element.Attributes.Append(CreateAttribute(xDoc, "id", "6"));
            node.AppendChild(element);

            // Restore HELMET sets
            node = xDoc.SelectSingleNode("gear/riders/rider[@id='1']/groups/group[@name='HELMET']");

            element = xDoc.CreateElement("set");
            element.Attributes.Append(CreateAttribute(xDoc, "id", "8"));
            node.AppendChild(element);

            element = xDoc.CreateElement("set");
            element.Attributes.Append(CreateAttribute(xDoc, "id", "9"));
            node.AppendChild(element);

            // Restore sets
            node = xDoc.SelectSingleNode("gear/sets/set[@id='1']");
            node.Attributes[3].Value = "images\\garage\\rider\\Techboy_A_Body.png";

            node = xDoc.SelectSingleNode("gear/sets/set[@id='4']");
            node.Attributes[3].Value = "images\\garage\\rider\\Techboy_A_Pants.png";

            node = xDoc.SelectSingleNode("gear/sets/set[@id='7']");
            node.Attributes[3].Value = "images\\garage\\rider\\Techboy_A_Head.png";

            // Restore items
            node = xDoc.SelectSingleNode("gear/items/item[@id='1']");
            node.Attributes[2].Value = "318519819";
            node.Attributes[4].Value = "2729008919";
            node.Attributes[6].Value = "1";

            node = xDoc.SelectSingleNode("gear/items/item[@id='4']");
            node.Attributes[2].Value = "239769300";
            node.Attributes[4].Value = "3486890472";
            node.Attributes[6].Value = "1";

            node = xDoc.SelectSingleNode("gear/items/item[@id='7']");
            node.Attributes[2].Value = "795375336";
            node.Attributes[4].Value = "3440826863";
            node.Attributes[6].Value = "1";

            string tmpfile = Path.GetTempFileName();
            xDoc.Save(tmpfile);

            BinaryReader br = new BinaryReader(File.OpenRead(tmpfile));
            dataPak.Import(-1899826158, br.ReadBytes((int)br.BaseStream.Length));
            br.Close();
            dataPak.Save();
            File.Delete(tmpfile);
            MessageBox.Show("Saved");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                string pak = ofd.FileName;
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    string file = ofd.FileName;
                    AddNewDDS(pak, file, textBox1.Text);
                }
            }
        }
    }
}
