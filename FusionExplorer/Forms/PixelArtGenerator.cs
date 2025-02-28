using FusionExplorer.src.utility;
using Ionic.Zlib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static FusionExplorer.EditorFavouritesTool;

namespace FusionExplorer.src
{
    public partial class PixelArtGenerator : Form
    {
        public PixelArtGenerator()
        {
            InitializeComponent();
        }

        string[] fav_locs = {"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade9c-4.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade97-4.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade95-4.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade92-4.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade8e-4.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade78-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1adf50-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade77-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade76-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1adf4e-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade75-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1adf4b-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade74-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade73-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1adf49-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade72-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1adf47-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade71-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1adf44-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade70-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade6f-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1adf42-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade6e-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1adf3e-3.o",
"C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d1ade6c-3.o"};

        string loadedImage;

        public byte[] prime_box_01 = new byte[] { 0x36, 0x1E, 0xF0, 0x25, 0x00, 0x00, 0x00, 0x01, 0x10, 0x30 };
        public byte[] prime_box_02 = new byte[] { 0x36, 0x1E, 0xF0, 0x25, 0x00, 0x00, 0x00, 0x02, 0x10, 0x30 };
        public byte[] prime_box_03 = new byte[] { 0x36, 0x1E, 0xF0, 0x25, 0x00, 0x00, 0x00, 0x03, 0x10, 0x30 };
        public byte[] prime_box_04 = new byte[] { 0x36, 0x1E, 0xF0, 0x25, 0x00, 0x00, 0x00, 0x04, 0x10, 0x30 };
        public float pixel_offset_01 = 0.24f;
        public float pixel_offset_04 = 0.97f;

        public Bitmap ConvertToPixelArt(Bitmap image, int pixelWidth, int pixelHeight)
        {
            Bitmap resizedImage = new Bitmap(pixelWidth, pixelHeight);
            using(Graphics g = Graphics.FromImage(resizedImage))
            {
                g.DrawImage(image, 0, 0, pixelWidth, pixelHeight);
            }

            Bitmap pixelatedImage = new Bitmap(pixelWidth, pixelHeight);
            for(int x = 0; x < pixelWidth; x++)
            {
                for(int y = 0; y < pixelHeight; y++)
                {
                    int pixelSize = 1;
                    Rectangle block = new Rectangle(x * pixelSize, y * pixelSize, pixelSize, pixelSize);
                    Color averageColor = GetAverageColor(resizedImage, block);
                    pixelatedImage.SetPixel(x, y, averageColor);
                }
            }

            return pixelatedImage;
        }

        private Color GetAverageColor(Bitmap image, Rectangle rect)
        {
            int redSum = 0;
            int greenSum = 0;
            int blueSum = 0;
            int pixelCount = 0;

            for (int x = rect.Left; x < rect.Right; x++)
            {
                for (int y = rect.Top; y < rect.Bottom; y++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    redSum += pixelColor.R;
                    greenSum += pixelColor.G;
                    blueSum += pixelColor.B;
                    pixelCount++;
                }
            }

            int averageRed = redSum / pixelCount;
            int averageGreen = greenSum / pixelCount;
            int averageBlue = blueSum / pixelCount;

            return Color.FromArgb(averageRed, averageGreen, averageBlue);
        }

        private Bitmap ScaleForPreview(Bitmap image)
        {
            Bitmap scaledImage = new Bitmap(500, 500);
            using(Graphics g = Graphics.FromImage(scaledImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(image, 0, 0, 500, 500);
            }

            return scaledImage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                int width = (int)numericUpDown1.Value;
                int height = (int)numericUpDown2.Value;
                Bitmap img = new Bitmap(ofd.FileName);
                Bitmap pixelImg = ConvertToPixelArt(img, width, height);
                pictureBox1.Image = ScaleForPreview(pixelImg);

                loadedImage = ofd.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int width = (int)numericUpDown1.Value;
            int height = (int)numericUpDown2.Value;
            Bitmap img = new Bitmap(loadedImage);
            Bitmap pixelImg = ConvertToPixelArt(img, width, height);
            pictureBox1.Image = ScaleForPreview(pixelImg);



            int count = width / 15;

            List<Bitmap> bitmaps = new List<Bitmap>();
            for (int x = 0; x < count; x++)
            {
                for (int y = 0; y < count; y++)
                {
                    Bitmap bitmap = new Bitmap(15, 15);
                    using(Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.DrawImage(pixelImg, 0, 0, new Rectangle(x * 15, y * 15, 15, 15), GraphicsUnit.Pixel);
                    }

                    bitmaps.Add(bitmap);
                }
            }

            for(int i = 0; i < 25; i++)
            {
                PixelArt art = new PixelArt(bitmaps[i]);
                byte[] data = art.GenerateGRP();

                File.WriteAllBytes(fav_locs[i] + "\\objectgroup.grp", GRP.Compress(data));
            }

            MessageBox.Show("Done");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap character = PreviewCharacter();

            List<Bitmap> sections = new List<Bitmap>();
            sections.Add(new Bitmap(12, 12));
            sections.Add(new Bitmap(12, 12));
            sections.Add(new Bitmap(12, 12));
            sections.Add(new Bitmap(12, 12));

            using (Graphics g1 = Graphics.FromImage(sections[0]), g2 = Graphics.FromImage(sections[1]), g3 = Graphics.FromImage(sections[2]), g4 = Graphics.FromImage(sections[3]))
            {
                g1.DrawImage(character, 0, 0, new Rectangle(0, 0, 12, 12), GraphicsUnit.Pixel);
                g2.DrawImage(character, 0, 0, new Rectangle(12, 0, 12, 12), GraphicsUnit.Pixel);
                g3.DrawImage(character, 0, 0, new Rectangle(0, 12, 12, 12), GraphicsUnit.Pixel);
                g4.DrawImage(character, 0, 0, new Rectangle(12, 12, 12, 12), GraphicsUnit.Pixel);
            }

            List<byte[]> groups = new List<byte[]>();

            PixelArt.PixelArtSize pixelArtSize = PixelArt.PixelArtSize.Small; ;

            switch(comboBox1.SelectedIndex)
            {
                case 0: pixelArtSize = PixelArt.PixelArtSize.Small; break;
                case 1: pixelArtSize = PixelArt.PixelArtSize.Medium; break;
                case 2: pixelArtSize = PixelArt.PixelArtSize.Large; break;
                case 3: pixelArtSize = PixelArt.PixelArtSize.ExtraLarge; break;
            }

            foreach(Bitmap bitmap in sections)
            {
                PixelArt art = new PixelArt(bitmap, pixelArtSize);
                groups.Add(art.GenerateGRP());
            }

            
            File.WriteAllBytes("debug_pixelart/1", groups[0]);
            File.WriteAllBytes("debug_pixelart/2", groups[1]);
            File.WriteAllBytes("debug_pixelart/3", groups[2]);
            File.WriteAllBytes("debug_pixelart/4", groups[3]);

            File.WriteAllBytes("C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d185410-4.o\\objectgroup.grp", GRP.Compress(groups[0]));
            File.WriteAllBytes("C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d185417-4.o\\objectgroup.grp", GRP.Compress(groups[1]));
            File.WriteAllBytes("C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d18541c-4.o\\objectgroup.grp", GRP.Compress(groups[2]));
            File.WriteAllBytes("C:\\Users\\IS_PL\\Documents\\TrialsFusion\\SavedGames\\b74d10bfeaf104112d185422-4.o\\objectgroup.grp", GRP.Compress(groups[3]));
                
        }

        private Bitmap PreviewCharacter()
        {
            int index = (int)numericUpDown3.Value;
            int x_offset = (index % 10) * 24;
            int y_offset = (index / 10) * 24;

            Bitmap bitmap = new Bitmap("scifipunks-24x24.png");
            Bitmap character = new Bitmap(24, 24);

            using (Graphics g = Graphics.FromImage(character))
            {
                g.DrawImage(bitmap, 0, 0, new Rectangle(x_offset, y_offset, 24, 24), GraphicsUnit.Pixel);
            }

            pictureBox1.Image = ScaleForPreview(character);

            return character;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PreviewCharacter();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Bitmap test = new Bitmap(1, 1);
            test.SetPixel(0, 0, Color.Red);

            PixelArt test_pixel = new PixelArt(test);
            byte[] data = test_pixel.GenerateGRP();

            File.WriteAllBytes("debug_pixelart/1", data);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            PreviewCharacter();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = File.ReadAllBytes(ofd.FileName);
                int count = data.Length - 4;

                //byte[] compressed = new byte[data.Length - 4];

                using(BinaryReader br = new BinaryReader(new MemoryStream(data)))
                {
                    br.BaseStream.Seek(4, SeekOrigin.Begin);

                    byte[] compressed = br.ReadBytes(count);
                    File.WriteAllBytes("test.bin", ZlibStream.UncompressBuffer(compressed));

                    ZlibStream zlib = new ZlibStream(new MemoryStream(1), CompressionMode.Decompress);
                }
            }
        }
    }
}
