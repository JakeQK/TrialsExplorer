using Reloaded.Memory.Streams;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using FusionExplorer.Models.Math;

namespace FusionExplorer.src.utility
{
    public class PixelArtService
    {
        private PixelArtSize Size;

        private int Width;
        private int Height;
        private Bitmap g_Originalbitmap;
        private Bitmap g_image;

        private byte[] selected_prime;
        private float selected_pixel_offset;

        private byte[] grp_header = new byte[] { 0x44, 0x4C, 0x43, 0x30, 0x00, 0x00, 0x00, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4F, 0x42, 0x4A, 0x41 };
        private byte[] grp_footer = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x54, 0x52, 0x49, 0x79, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x47, 0x52, 0x50, 0x30, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x43, 0x47, 0x4F, 0x31, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x45, 0x4E, 0x44, 0x30, 0x00, 0x00, 0x00, 0x00 };

        private byte[] prime_box_01 = new byte[] { 0x36, 0x1E, 0xF0, 0x25, 0x00, 0x00, 0x00, 0x01, 0x10, 0x30 };
        private byte[] prime_box_04 = new byte[] { 0x36, 0x1E, 0xF0, 0x25, 0x00, 0x00, 0x00, 0x04, 0x10, 0x30 };

        private float pixel_offset_01 = 0.24f;
        private float pixel_offset_04 = 0.97f;


        /// <summary>
        /// Convert an image to pixel art in Trials Fusion
        /// </summary>
        /// <param name="path">path to image</param>
        /// <param name="size">determines which prime box is used</param>
        public PixelArtService(string path, PixelArtSize size = PixelArtSize.Small)
        {
            Bitmap original_image = new Bitmap(path);
            this.Width = original_image.Width;
            this.Height = original_image.Height;

            g_Originalbitmap = original_image;
            g_image = original_image;
            Size = size;
        }

        /// <summary>
        /// Convert a bitmap to pixel art in Trials Fusion
        /// </summary>
        /// <param name="image">image bitmap</param>
        /// <param name="size">determines which prime box is used</param>
        public PixelArtService(Bitmap image, PixelArtSize size = PixelArtSize.Small)
        {
            this.g_Originalbitmap = image;
            this.g_image = image;

            this.Width = g_image.Width;
            this.Height = g_image.Height;
            this.Size = size;
        }

        public void SetWidth(int width) 
        { 
            Width = width; 
        }

        public void SetHeight(int height) 
        { 
            Height = height; 
        }

        public void GenerateFavourites()
        {

        }


        public byte[] GenerateGRP()
        {
            // pixel/object count
            short pixel_count = (short)(Width * Height);
            /*  16 = unknown difference
             *  
             *  1 Pixel = 52 bytes
             *      1 X Object Entry = 10 bytes
             *      1 X Object Translation = 28 bytes
             *      1 X Object Color = 14 bytes
             */
            int OBJA_size = pixel_count * 52 + 16;

            // checks what size is selected, sets the selected prime byte array and appropriate pixel offset
            GetSelectedSize();

            // calculates the positions for each prime object
            List<Vector3> positions = GeneratePositions();

            /*  OBJA Size = 4 bytes
             *  Object Count = 2 bytes
             *  
             *  1 Pixel = 52 bytes
             *      1 X Object Entry = 10 bytes
             *      1 X Object Translation = 28 bytes
             *      1 X Object Color = 14 bytes
             *      
             *  Unknown Value (1 less than object count) = 2 bytes
             *  Object Count = 2 bytes
             */
            byte[] data = new byte[grp_header.Length + (6 + (pixel_count * 52) + 4) + grp_footer.Length];

            /*using(MemoryStream ms = new MemoryStream(grp_header.Length + (6 + (pixel_count * 52) + 4) + grp_footer.Length))
            {
                using(StreamWriter sw = new StreamWriter(ms))
                {
                    // Write GRP header
                    sw.Write(grp_header);

                    // Write OBJA Size
                    sw.Write(BigEndianness(BitConverter.GetBytes(OBJA_size)));

                    // Write pixel/object count
                    sw.Write(BigEndianness(BitConverter.GetBytes(pixel_count)));

                    // Write all object entries
                    for(int i = 0; i < pixel_count; i++)
                    {
                        sw.Write(selected_prime);
                    }

                    // Write object position values
                    foreach(Vector3 pos in positions)
                    {
                        *//*
                         *  Object position values are not stored as a typical vector3
                         *  All objects X values are stored then all Y values and then Z values
                         *  
                         *  So that 3 loops aren't created for each object, write X value, calculate Y position, write it, calculate Z pos, write it
                         *  then go back for next object 
                         *//*
                        sw.Write(BigEndianness(BitConverter.GetBytes(pos.x)));
                        sw.BaseStream.Position += positions.Count * 4 - 4;
                        sw.Write(BigEndianness(BitConverter.GetBytes(pos.y)));
                        sw.BaseStream.Position += positions.Count * 4 - 4;
                        sw.Write(BigEndianness(BitConverter.GetBytes(pos.z)));
                        sw.BaseStream.Position -= positions.Count * 8;
                    }

                    // Skip rotation data, blank is fine
                    sw.BaseStream.Position += pixel_count * 16;

                    // Write unknown value (1 less than pixel count)
                    sw.Write(BigEndianness(BitConverter.GetBytes(pixel_count - 1)));
                    // Write pixel count
                    sw.Write(BigEndianness(BitConverter.GetBytes(pixel_count)));

                    // Generate pixels (the format needed for GRP) from bitmap
                    List<Pixel> pixels = GeneratePixels();

                    // Write all the pixel color data
                    foreach(Pixel pixel in pixels)
                    {
                        sw.Write(pixel.ToGRPBytes());
                    }

                    // Write Footer
                    sw.Write(grp_footer);

                    
                }
            
                return ms.ToArray();
            }
        */

            using (MemoryStream ms = new MemoryStream(grp_header.Length + (6 + (pixel_count * 52) + 4) + grp_footer.Length))
            {
                // Write GRP header
                ms.Write(grp_header);

                // Write OBJA Size
                ms.Write(BigEndianness(BitConverter.GetBytes(OBJA_size)));

                // Write pixel/object count
                ms.Write(BigEndianness(BitConverter.GetBytes(pixel_count)));

                // Write all object entries
                for (int i = 0; i < pixel_count; i++)
                {
                    ms.Write(selected_prime);
                }

                // Write object position values
                foreach (Vector3 pos in positions)
                {
                    /*
                     *  Object position values are not stored as a typical vector3
                     *  All objects X values are stored then all Y values and then Z values
                     *  
                     *  So that 3 loops aren't created for each object, write X value, calculate Y position, write it, calculate Z pos, write it
                     *  then go back for next object 
                     */
                    ms.Write(BigEndianness(BitConverter.GetBytes(pos.x)));
                    ms.Seek(positions.Count * 4 - 4, SeekOrigin.Current);
                    ms.Write(BigEndianness(BitConverter.GetBytes(pos.y)));
                    ms.Seek(positions.Count * 4 - 4, SeekOrigin.Current);
                    ms.Write(BigEndianness(BitConverter.GetBytes(pos.z)));
                    ms.Seek(0 - (positions.Count * 8), SeekOrigin.Current);
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Position: {0}", ms.Position));

                // Skip last object Y and Z values
                ms.Seek(pixel_count * 8, SeekOrigin.Current);

                sb.AppendLine(string.Format("Position: {0}", ms.Position));



                // Skip rotation data, blank is fine
                ms.Position += pixel_count * 16;

                sb.AppendLine(string.Format("Position: {0}", ms.Position));

                File.WriteAllText("debug.txt", sb.ToString());


                // Write unknown value (1 less than pixel count)
                ms.Write(BigEndianness(BitConverter.GetBytes((short)(pixel_count - 1))));
                // Write pixel count
                ms.Write(BigEndianness(BitConverter.GetBytes(pixel_count)));

                // Generate pixels (the format needed for GRP) from bitmap
                List<Pixel> pixels = GeneratePixels();

                // Write all the pixel color data
                foreach (Pixel pixel in pixels)
                {
                    ms.Write(pixel.ToGRPBytes());
                }

                // Write Footer
                ms.Write(grp_footer);

                return ms.ToArray();
            }
        }

        private List<Vector3> GeneratePositions()
        {
            List<Vector3> positions = new List<Vector3>();

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float pos_x = (float)(selected_pixel_offset * x);
                    float pos_y = (float)(selected_pixel_offset * y);

                    Vector3 position = new Vector3(pos_x, 0, pos_y);
                    positions.Add(position);
                }
            }

            return positions;
        }

        private List<Pixel> GeneratePixels()
        {
            List<Pixel> pixels = new List<Pixel>();

            short index = 0;

            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    Color pixel_color = g_image.GetPixel(x, y);
                    pixels.Add(new Pixel(index, pixel_color.R, pixel_color.G, pixel_color.B, 0, 0));

                    index++;
                }
            }

            return pixels;
        }

        private void GetSelectedSize()
        {
            switch (Size)
            {
                case PixelArtSize.Small:
                    selected_pixel_offset = pixel_offset_01;
                    selected_prime = prime_box_01;
                    break;
                case PixelArtSize.ExtraLarge:
                    selected_pixel_offset = pixel_offset_04;
                    selected_prime = prime_box_04;
                    break;
            }
        }

        private byte[] BigEndianness(byte[] data)
        {
            return data.Reverse().ToArray();
        }

        public enum PixelArtSize
        {
            Small,
            Medium,
            Large,
            ExtraLarge
        }
    }

    public class Pixel
    {
        public Pixel(short index, byte r, byte g, byte b, short emission, short roughness)
        {
            this.Index = index;
            this.R = r;
            this.G = g;
            this.B = b;
            this.Emission = emission;
            this.Roughness = roughness;
        }

        public short Index { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public short Emission { get; set; }
        public short Roughness { get; set; }

        public byte[] ToGRPBytes()
        {
            using (MemoryStream ms = new MemoryStream(14))
            {
                ms.Write(BitConverter.GetBytes(Index).Reverse().ToArray());  // 2
                ms.Write(m_size);   // 2
                ms.Write(R); // 1
                ms.Write(G); // 1
                ms.Write(B); // 1
                ms.Write(BitConverter.GetBytes(Emission).Reverse().ToArray()); // 2
                ms.Write(R); // 1
                ms.Write(G); // 1
                ms.Write(B); // 1
                ms.Write(BitConverter.GetBytes(Roughness).Reverse().ToArray()); // 2

                return ms.ToArray();
            }
        }

        private byte[] m_size = new byte[] { 0x00, 0x0A };
    }
}
