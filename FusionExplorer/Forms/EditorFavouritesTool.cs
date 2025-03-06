using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FusionExplorer
{
    public partial class EditorFavouritesTool : Form
    {
        public EditorFavouritesTool()
        {
            InitializeComponent();
            DisplayFavourites();
        }

        GRP SelectedGRP;
        List<Favourite> FavouritesList = new List<Favourite>();

        public struct Favourite
        {
            public Favourite(string name, string directory, int groupIndex)
            {
                Directory = directory;
                Name = name;
                GroupIndex = groupIndex;
            }

            public string Directory { get; }
            public string Name { get; }
            public int GroupIndex { get; }


        }

        private void btnRefreshFavouritesList_Click(object sender, EventArgs e)
        {
            DisplayFavourites();
        }

        private void DisplayFavourites()
        {
            try
            {
                FavouritesList.Clear();
                lbFavouritesList.Items.Clear();
                string TrialsFusionSavedGamesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\TrialsFusion\\SavedGames";
                string[] Directories = Directory.GetDirectories(TrialsFusionSavedGamesPath);
                foreach (string dir in Directories)
                {
                    // Editor Favourites are saved in directories with .o suffix
                    if (dir.Contains(".o"))
                    {
                        int groupIndex = 0;
                        int.TryParse(dir[dir.Length - 3].ToString(), out groupIndex);
                        string name = File.ReadAllText(dir + "\\displayname");
                        FavouritesList.Add(new Favourite(name, dir, groupIndex + 1));
                    }
                }

                FavouritesList.Sort((s1, s2) => s1.GroupIndex.CompareTo(s2.GroupIndex));

                foreach (Favourite fav in FavouritesList)
                {
                    lbFavouritesList.Items.Add(string.Format("{0} - {1}", fav.GroupIndex, fav.Name));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOpenDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(FavouritesList[lbFavouritesList.SelectedIndex].Directory);
        }

        private void btnDecompress_Click(object sender, EventArgs e)
        {
            try
            {
                string dir = FavouritesList[lbFavouritesList.SelectedIndex].Directory + "\\";
                byte[] data = File.ReadAllBytes(dir + "objectgroup.grp");
                byte[] Decompressed = GRP.Decompress(data);
                Utility.SaveFile(Decompressed, dir);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCompressGRP_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    byte[] data = GRP.Compress(File.ReadAllBytes(ofd.FileName));
                    Utility.SaveFile(data, ofd.FileName);

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lbFavouritesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string dir = FavouritesList[lbFavouritesList.SelectedIndex].Directory; /* + "\\"*/
                Clipboard.SetText(dir);
                /*byte[] data = File.ReadAllBytes(dir + "objectgroup.grp");
                SelectedGRP = new GRP(data);
                UpdateObjectsList();*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void UpdateObjectsList()
        {
            try
            {
                lbObjectsList.Items.Clear();
                foreach (GRP.Object obj in SelectedGRP.Objects)
                {
                    lbObjectsList.Items.Add(obj.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        private void btnAddObject_Click(object sender, EventArgs e)
        {
            try
            {
                int ID = 0;
                if (int.TryParse(tbObjectID.Text, out ID))
                {
                    Dictionary<int, string> Info = Utility.LoadObjectInfo();
                    string ObjectName = "";
                    if (Info.TryGetValue(ID, out ObjectName))
                    {
                        GRP.Object newObj = new GRP.Object();
                        newObj.Name = ObjectName;
                        newObj.Entry.ID = ID;
                        SelectedGRP.Objects.Add(newObj);
                        UpdateObjectsList();
                    }
                    else
                    {
                        //MessageBox.Show(Properties.FusionExplorer.FE_TRY_FIND_OBJECT_FAIL_TEXT, Properties.FusionExplorer.FE_TRY_FIND_OBJECT_FAIL_TITLE);
                    }
                }
                else
                {
                    //MessageBox.Show(Properties.FusionExplorer.FE_TRY_READ_ID_FAIL);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSaveDecompressed_Click(object sender, EventArgs e)
        {
            byte[] decompressedData = SelectedGRP.BuildGRP(true);
            Utility.SaveFile(decompressedData);
        }

        private void btnSaveCompressed_Click(object sender, EventArgs e)
        {
            byte[] compressedData = SelectedGRP.BuildGRP();
            Utility.SaveFile(compressedData);
        }


        private void lbObjectsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Load_Object();
        }

        private void Load_Object()
        {
            try
            {
                GRP.Object OBJ = SelectedGRP.Objects[lbObjectsList.SelectedIndex];

                // Properties
                GRP.Properties PROP = OBJ.Entry.Properties;
                cbFlag1.Checked = PROP.NonDefaultPhysics;
                cbFlag2.Checked = PROP.NoCollisionSound;
                cbFlag3.Checked = PROP.FastObject;
                cbFlag4.Checked = PROP.DontResetPosition;
                cbFlag5.Checked = PROP.flag_5;
                cbFlag6.Checked = PROP.flag_6;
                cbFlag7.Checked = PROP.flag_7;
                cbFlag8.Checked = PROP.flag_8;
                cbFlag9.Checked = PROP.Physics;
                cbFlag10.Checked = PROP.flag_10;
                cbFlag11.Checked = PROP.LockToDrivingLine;
                cbFlag12.Checked = PROP.Invisible;
                cbFlag13.Checked = PROP.flag_13;
                cbFlag14.Checked = PROP.flag_14;
                cbFlag15.Checked = PROP.NoContactResponse;
                cbFlag16.Checked = PROP.NoCollision;

                nColorPart.Maximum = OBJ.Color_Size / 10;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            Load_Color_Data();

        }


        private void Load_Color_Data()
        {
            try
            {
                GRP.Object OBJ = SelectedGRP.Objects[lbObjectsList.SelectedIndex];
                if (OBJ.HasColor)
                {
                    nPrimaryR.Value = OBJ.colors[(int)nColorPart.Value - 1].Primary.R;
                    nPrimaryG.Value = OBJ.colors[(int)nColorPart.Value - 1].Primary.G;
                    nPrimaryB.Value = OBJ.colors[(int)nColorPart.Value - 1].Primary.B;

                    nSecondaryR.Value = OBJ.colors[(int)nColorPart.Value - 1].Secondary.R;
                    nSecondaryG.Value = OBJ.colors[(int)nColorPart.Value - 1].Secondary.G;
                    nSecondaryB.Value = OBJ.colors[(int)nColorPart.Value - 1].Secondary.B;

                    nEmission.Value = OBJ.colors[(int)nColorPart.Value - 1].Emission;
                    nRoughness.Value = OBJ.colors[(int)nColorPart.Value - 1].Roughness;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void nColorPart_ValueChanged(object sender, EventArgs e)
        {
            Load_Color_Data();
        }


        private void btnSetSettings_Click(object sender, EventArgs e)
        {
            try
            {
                GRP.Properties prop = new GRP.Properties();
                prop.NonDefaultPhysics = cbFlag1.Checked;
                prop.NoCollisionSound = cbFlag2.Checked;
                prop.FastObject = cbFlag3.Checked;
                prop.DontResetPosition = cbFlag4.Checked;
                prop.flag_5 = cbFlag5.Checked;
                prop.flag_6 = cbFlag6.Checked;
                prop.flag_7 = cbFlag7.Checked;
                prop.flag_8 = cbFlag8.Checked;
                prop.Physics = cbFlag9.Checked;
                prop.flag_10 = cbFlag10.Checked;
                prop.LockToDrivingLine = cbFlag11.Checked;
                prop.Invisible = cbFlag12.Checked;
                prop.flag_13 = cbFlag13.Checked;
                prop.flag_14 = cbFlag14.Checked;
                prop.NoContactResponse = cbFlag15.Checked;
                prop.NoCollision = cbFlag16.Checked;
                SelectedGRP.Objects[lbObjectsList.SelectedIndex].Entry.Properties = prop;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSetColor_Click(object sender, EventArgs e)
        {
            //try
            {
                RGB Primary = new RGB((byte)nPrimaryR.Value, (byte)nPrimaryG.Value, (byte)nPrimaryB.Value);
                RGB Secondary = new RGB((byte)nSecondaryR.Value, (byte)nSecondaryG.Value, (byte)nSecondaryB.Value);
                GRP.Color newColor = new GRP.Color(Primary, (short)nEmission.Value, Secondary, (short)nRoughness.Value);
                SelectedGRP.Objects[lbObjectsList.SelectedIndex].colors[(int)nColorPart.Value - 1] = newColor;
            }
            //catch(Exception ex)
            {
              //  MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string path = FavouritesList[lbFavouritesList.SelectedIndex].Directory + "\\objectgroup.grp";
                if (!File.Exists(path))
                    File.Create(path);

                File.WriteAllBytes(path, SelectedGRP.BuildGRP());
                //MessageBox.Show(Properties.FusionExplorer.FE_SAVE_DONE);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void UpdatePrimaryColor()
        {
            Color color = Color.FromArgb((int)nPrimaryR.Value, (int)nPrimaryG.Value, (int)nPrimaryB.Value);
            
            Bitmap Bmp = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            using (Graphics gfx = Graphics.FromImage(Bmp))
            using (SolidBrush brush = new SolidBrush(color))
            {
                gfx.FillRectangle(brush, 0, 0, pictureBox1.Size.Width, pictureBox1.Size.Height);
            }
            pictureBox1.Image = Bmp;
        }

        private void UpdateSecondaryColor()
        {
            Color color = Color.FromArgb((int)nSecondaryR.Value, (int)nSecondaryG.Value, (int)nSecondaryB.Value);

            Bitmap Bmp = new Bitmap(pictureBox2.Size.Width, pictureBox2.Size.Height);
            using (Graphics gfx = Graphics.FromImage(Bmp))
            using (SolidBrush brush = new SolidBrush(Color.FromArgb((int)nSecondaryR.Value, (int)nSecondaryG.Value, (int)nSecondaryB.Value)))
            {
                gfx.FillRectangle(brush, 0, 0, pictureBox2.Size.Width, pictureBox2.Size.Height);
            }
            pictureBox2.Image = Bmp;
        }

        private void nPrimaryR_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrimaryColor();
        }

        private void nPrimaryG_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrimaryColor();
        }

        private void nPrimaryB_ValueChanged(object sender, EventArgs e)
        {
            UpdatePrimaryColor();
        }

        private void nSecondaryR_ValueChanged(object sender, EventArgs e)
        {
            UpdateSecondaryColor();
        }

        private void nSecondaryG_ValueChanged(object sender, EventArgs e)
        {
            UpdateSecondaryColor();
        }

        private void nSecondaryB_ValueChanged(object sender, EventArgs e)
        {
            UpdateSecondaryColor();
        }

        private void btnPrimaryColorPicker_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Color.FromArgb((int)nPrimaryR.Value, (int)nPrimaryG.Value, (int)nPrimaryB.Value);
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                nPrimaryR.Value = colorDialog1.Color.R;
                nPrimaryG.Value = colorDialog1.Color.G;
                nPrimaryB.Value = colorDialog1.Color.B;
            }
        }

        private void btnSecondaryColorPicker_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = Color.FromArgb((int)nSecondaryR.Value, (int)nSecondaryG.Value, (int)nSecondaryB.Value);
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                nSecondaryR.Value = colorDialog1.Color.R;
                nSecondaryG.Value = colorDialog1.Color.G;
                nSecondaryB.Value = colorDialog1.Color.B;
            }
        }

        private void cbFlag4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var idk in FavouritesList)
            {
                if (idk.GroupIndex == 5)
                {
                    sb.AppendLine(string.Format("{0} {1}", idk.Name, idk.Directory));
                }
            }

            Clipboard.SetText(sb.ToString());
        }

    }
}
