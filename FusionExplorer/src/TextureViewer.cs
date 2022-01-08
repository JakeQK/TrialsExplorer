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
    public partial class TextureViewer : Form
    {
        private byte[] dds;

        public TextureViewer(byte[] dds)
        {
            InitializeComponent();
            this.dds = dds ;
            DrawImage();
            checkBox1.Parent = pictureBox1;
        }

        private void DrawImage()
        {
            try
            {
                DirectBitmap bmp = Texture.DDStoDirectBitmap(dds);
                if (bmp != null)
                {

                    if (checkBox1.Checked)
                    {
                        // Get the scale factor of the original image to fit 1280x720
                        float scale = (float)Math.Min(720.0 / (float)bmp.Bitmap.Height, 1280.0 / (float)bmp.Bitmap.Width);
                        // Scale and create new size
                        Size nSize = new Size((int)(bmp.Bitmap.Width * scale), (int)(bmp.Bitmap.Height * scale));
                        // Create new scaled bitmap
                        bmp.Bitmap = new Bitmap(bmp.Bitmap, nSize);
                    }
                    pictureBox1.Size = bmp.Bitmap.Size;
                    pictureBox1.Image = bmp.Bitmap;

                }
                checkBox1.BackColor = Color.Transparent;
            }
            catch
            {

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DrawImage();
        }
    }
}
