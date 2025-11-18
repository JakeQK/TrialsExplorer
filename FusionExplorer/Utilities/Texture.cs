using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FusionExplorer
{
    /// <summary>
    /// Trials Texture Class
    /// </summary>
    public static class Texture
    {
        /// <summary>
        /// TEX format version
        /// </summary>
        public enum TextureVersion
        {
            T5X,
            T8X
        }

        private static byte[] ConstructDDS(int width, int height, int mipmap_count, byte[] texture_data)
        {
            byte[] dds_texture = new byte[0x80 + texture_data.Length];

            byte[] dds_part_1 = { 0x44, 0x44, 0x53, 0x20, 0x7C, 0x00, 0x00, 0x00, 0x07, 0x10 };
            byte[] height_bytes = BitConverter.GetBytes(height);
            byte[] width_bytes = BitConverter.GetBytes(width);
            byte[] size = BitConverter.GetBytes(height * width);
            byte[] dds_part_2 = { 0x47, 0x49, 0x4D, 0x50, 0x2D, 0x44, 0x44, 0x53, 0x5C, 0x09, 0x03, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20,
                    0x00, 0x00, 0x00, 0x04, 0x00, 0x00, 0x00, 0x44, 0x58, 0x54, 0x35, 0x00, 0x00, 0x00, 0x00,
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                    0x00 };

            // Signature + DDS_Header
            // Signature, dwSize, dwFlags
            Buffer.BlockCopy(dds_part_1, 0, dds_texture, 0, dds_part_1.Length);
            // Mipmap Flag
            if (mipmap_count > 1)
                Buffer.BlockCopy(new byte[] { 0x0A }, 0, dds_texture, dds_part_1.Length, 1);
            else
                Buffer.BlockCopy(new byte[] { 0x08 }, 0, dds_texture, dds_part_1.Length, 1);
            // dwHeight
            Buffer.BlockCopy(height_bytes, 0, dds_texture, dds_part_1.Length + 2, 4);
            // dwWidth
            Buffer.BlockCopy(width_bytes, 0, dds_texture, dds_part_1.Length + 6, 4);
            // dwPitchOrLinearSize
            Buffer.BlockCopy(size, 0, dds_texture, dds_part_1.Length + 10, 4);
            // skips dwDepth (00 00 00 00)
            // dwMipMapCount
            Buffer.BlockCopy(BitConverter.GetBytes(mipmap_count), 0, dds_texture, dds_part_1.Length + 18, 4);
            // dwReserved1[11], ddspf, dwCaps, dwCaps2, dwCaps3, dwCaps4, dwReserved2
            Buffer.BlockCopy(dds_part_2, 0, dds_texture, dds_part_1.Length + 22, dds_part_2.Length);

            if (mipmap_count > 1)
                Buffer.BlockCopy(new byte[] { 0x08, 0x10, 0x40 }, 0, dds_texture, dds_part_1.Length + dds_part_2.Length + 22, 3);
            else
                Buffer.BlockCopy(new byte[] { 0x00, 0x10, 0x00 }, 0, dds_texture, dds_part_1.Length + dds_part_2.Length + 22, 3);

            // texture data
            Buffer.BlockCopy(texture_data, 0, dds_texture, 0x80, texture_data.Length);
            return dds_texture;
        }

        private static void DecompressBlockDXT5(int x, int y, int width, byte[] blockStorage, DirectBitmap image)
        {
            try
            {
                byte alpha0 = blockStorage[0];
                byte alpha1 = blockStorage[1];

                int bitOffset = 2;
                uint alphaCode1 = (uint)(blockStorage[bitOffset + 2] | (blockStorage[bitOffset + 3] << 8) | (blockStorage[bitOffset + 4] << 16) | (blockStorage[bitOffset + 5] << 24));
                ushort alphaCode2 = (ushort)(blockStorage[bitOffset + 0] | (blockStorage[bitOffset + 1] << 8));

                ushort color0 = (ushort)(blockStorage[8] | blockStorage[9] << 8);
                ushort color1 = (ushort)(blockStorage[10] | blockStorage[11] << 8);

                int temp;

                temp = (color0 >> 11) * 255 + 16;
                byte r0 = (byte)((temp / 32 + temp) / 32);
                temp = ((color0 & 0x07E0) >> 5) * 255 + 32;
                byte g0 = (byte)((temp / 64 + temp) / 64);
                temp = (color0 & 0x001F) * 255 + 16;
                byte b0 = (byte)((temp / 32 + temp) / 32);

                temp = (color1 >> 11) * 255 + 16;
                byte r1 = (byte)((temp / 32 + temp) / 32);
                temp = ((color1 & 0x07E0) >> 5) * 255 + 32;
                byte g1 = (byte)((temp / 64 + temp) / 64);
                temp = (color1 & 0x001F) * 255 + 16;
                byte b1 = (byte)((temp / 32 + temp) / 32);

                uint code = (uint)(blockStorage[12] | blockStorage[13] << 8 | blockStorage[14] << 16 | blockStorage[15] << 24);

                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        int alphaCodeIndex = 3 * (4 * j + i);
                        int alphaCode;

                        if (alphaCodeIndex <= 12)
                        {
                            alphaCode = (alphaCode2 >> alphaCodeIndex) & 0x07;
                        }
                        else if (alphaCodeIndex == 15)
                        {
                            alphaCode = (int)((alphaCode2 >> 15) | ((alphaCode1 << 1) & 0x06));
                        }
                        else
                        {
                            alphaCode = (int)((alphaCode1 >> (alphaCodeIndex - 16)) & 0x07);
                        }

                        byte finalAlpha;
                        if (alphaCode == 0)
                        {
                            finalAlpha = alpha0;
                        }
                        else if (alphaCode == 1)
                        {
                            finalAlpha = alpha1;
                        }
                        else
                        {
                            if (alpha0 > alpha1)
                            {
                                finalAlpha = (byte)(((8 - alphaCode) * alpha0 + (alphaCode - 1) * alpha1) / 7);
                            }
                            else
                            {
                                if (alphaCode == 6)
                                    finalAlpha = 0;
                                else if (alphaCode == 7)
                                    finalAlpha = 255;
                                else
                                    finalAlpha = (byte)(((6 - alphaCode) * alpha0 + (alphaCode - 1) * alpha1) / 5);
                            }
                        }

                        byte colorCode = (byte)((code >> 2 * (4 * j + i)) & 0x03);

                        Color finalColor = new Color();
                        switch (colorCode)
                        {
                            case 0:
                                finalColor = Color.FromArgb(finalAlpha, r0, g0, b0);
                                break;
                            case 1:
                                finalColor = Color.FromArgb(finalAlpha, r1, g1, b1);
                                break;
                            case 2:
                                finalColor = Color.FromArgb(finalAlpha, (2 * r0 + r1) / 3, (2 * g0 + g1) / 3, (2 * b0 + b1) / 3);
                                break;
                            case 3:
                                finalColor = Color.FromArgb(finalAlpha, (r0 + 2 * r1) / 3, (g0 + 2 * g1) / 3, (b0 + 2 * b1) / 3);
                                break;
                        }

                        if (x + i < width)
                            image.SetPixel(x + i, y + j, finalColor);
                        //image[(y + j)*width + (x + i)] = finalColor;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private static DirectBitmap UncompressDXT5(byte[] data, int w, int h)
        {
            try
            {
                DirectBitmap res = new DirectBitmap(w, h);
                using (MemoryStream ms = new MemoryStream(data))
                {
                    using (BinaryReader r = new BinaryReader(ms))
                    {
                        int blockCountX = (w + 3) / 4;
                        int blockCountY = (h + 3) / 4;
                        int blockWidth = (w < 4) ? w : 4;
                        int blockHeight = (h < 4) ? w : 4;

                        for (int j = 0; j < blockCountY; j++)
                        {
                            //byte[] blockStorage = r.ReadBytes(blockCountX * 16);
                            for (int i = 0; i < blockCountX; i++)
                            {
                                byte[] blockStorage = r.ReadBytes(16);
                                //DecompressBlockDXT5(i * 4, j * 4, w, blockStorage + i * 16, res);
                                DecompressBlockDXT5(i * 4, j * 4, w, blockStorage, res);
                            }
                        }
                    }
                }
                return res;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }

        }

        /// <summary>
        /// Converts DDS image to DirectBitmap
        /// </summary>
        /// <param name="DDS">DDS Image</param>
        /// <returns>DirectBitmap</returns>
        public static DirectBitmap DDStoDirectBitmap(byte[] DDS)
        {
            using (MemoryStream ms = new MemoryStream(DDS))
            {
                using (BinaryReader r = new BinaryReader(ms))
                {
                    try
                    {
                        int dwMagic = r.ReadInt32();
                        if (dwMagic != 0x20534444)
                        {
                            throw new Exception("This is not a DDS!");
                        }

                        r.BaseStream.Seek(8, SeekOrigin.Current);
                        int dwHeight = r.ReadInt32();
                        int dwWidth = r.ReadInt32();
                        int dwPitchOrLinearSize = r.ReadInt32();

                        r.BaseStream.Seek(0x80, SeekOrigin.Begin);
                        byte[] bdata = r.ReadBytes(dwPitchOrLinearSize);
                        return UncompressDXT5(bdata, dwWidth, dwHeight);
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Converts TEX texture to DDS image
        /// </summary>
        /// <param name="tex">TEX texture</param>
        /// <returns>DDS image</returns>
        public static byte[] TEXToDDS(byte[] tex)
        {
            using (MemoryStream ms = new MemoryStream(tex))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    byte[] signature = br.ReadBytes(3); // signature
                    byte[] t8x = { 0x54, 0x38, 0x58 };
                    byte[] t5x = { 0x54, 0x35, 0x58 };

                    if (signature.SequenceEqual(t8x) || signature.SequenceEqual(t5x))
                    {
                        // Skip unknown byte only found in T8X tex files
                        if (signature.SequenceEqual(t8x))
                            br.BaseStream.Seek(0x04, SeekOrigin.Begin);

                        // Get main image width and height
                        Int16 width = br.ReadInt16();
                        Int16 height = br.ReadInt16();

                        // Skip unknown byte
                        br.BaseStream.Seek(0x01, SeekOrigin.Current);

                        // Get mipmap count
                        Int16 mipmap_count = br.ReadByte();

                        // Calculate texture data length
                        int data_size = 0;
                        if (mipmap_count > 1)
                        {
                            int t_width = width;
                            int t_height = height;
                            for (int i = 0; i < mipmap_count; i++)
                            {
                                // Each mipmap texture is half the size of the previous texture (ex. 64x64, 32x32, 16x16, 8x8)
                                // The texture data length will be the sum of all texture sizes
                                data_size += t_width * t_height;
                                t_width = t_width / 2;
                                t_height = t_height / 2;
                            }
                        }
                        else
                            data_size = width * height;


                        // Seek to the beginning of the texture data for each file type
                        if (signature.SequenceEqual(t8x))
                            br.BaseStream.Seek(0x1F, SeekOrigin.Begin);
                        else if (signature.SequenceEqual(t5x))
                            br.BaseStream.Seek(0x1D, SeekOrigin.Begin);

                        //  Get texture data
                        byte[] texture_data = br.ReadBytes(data_size);

                        // Construct and return the DDS format texture
                        return ConstructDDS(width, height, mipmap_count, texture_data);
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// Converts DDS image to TEX texture
        /// </summary>
        /// <param name="DDS">DDS image</param>
        /// <param name="textureVersion">The tex version to be converted to (T5X or T8X)</param>
        /// <returns>TEX texture</returns>
        public static byte[] DDStoTEX(byte[] DDS, TextureVersion textureVersion)
        {
            using (MemoryStream ms = new MemoryStream(DDS))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    // Get height, width and size
                    br.BaseStream.Seek(0x0C, SeekOrigin.Begin);
                    Int16 height = (Int16)br.ReadInt32();
                    Int16 width = (Int16)br.ReadInt32();
                    int size = br.ReadInt32();

                    // Get texture data
                    br.BaseStream.Seek(0x80, SeekOrigin.Begin);
                    byte[] dds_texture_data = br.ReadBytes(height * width);


                    if (textureVersion == TextureVersion.T5X)
                    {
                        byte[] T5X = { 0x54, 0x35, 0x58 };
                        byte[] width_bytes = BitConverter.GetBytes(width);
                        byte[] height_bytes = BitConverter.GetBytes(height);
                        byte[] T5X_part2 = { 0x0E, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                        // height, width, height, width
                        // texture data

                        // T5X header size + texture data size
                        byte[] texture = new byte[0x1D + size];

                        Buffer.BlockCopy(T5X, 0, texture, 0, 3);
                        Buffer.BlockCopy(width_bytes, 0, texture, 3, 2);
                        Buffer.BlockCopy(height_bytes, 0, texture, 5, 2);
                        Buffer.BlockCopy(T5X_part2, 0, texture, 7, T5X_part2.Length);
                        Buffer.BlockCopy(width_bytes, 0, texture, T5X_part2.Length + 7, 2);
                        Buffer.BlockCopy(height_bytes, 0, texture, T5X_part2.Length + 9, 2);
                        Buffer.BlockCopy(width_bytes, 0, texture, T5X_part2.Length + 11, 2);
                        Buffer.BlockCopy(height_bytes, 0, texture, T5X_part2.Length + 13, 2);
                        Buffer.BlockCopy(dds_texture_data, 0, texture, 0x1D, size);

                        return texture;
                    }
                    else
                    {
                        byte[] T8X = { 0x54, 0x38, 0x58, 0x02 };
                        byte[] width_bytes = BitConverter.GetBytes(width);
                        byte[] height_bytes = BitConverter.GetBytes(height);
                        byte[] T8X_part2 = { 0x0E, 0x01, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                        // height, width
                        // texture data @ 0x1F

                        // T5X header size + texture data size
                        byte[] texture = new byte[0x1F + height * width];

                        Buffer.BlockCopy(T8X, 0, texture, 0, 4);
                        Buffer.BlockCopy(width_bytes, 0, texture, 4, 2);
                        Buffer.BlockCopy(height_bytes, 0, texture, 6, 2);
                        Buffer.BlockCopy(T8X_part2, 0, texture, 8, T8X_part2.Length);
                        Buffer.BlockCopy(width_bytes, 0, texture, T8X_part2.Length + 8, 2);
                        Buffer.BlockCopy(height_bytes, 0, texture, T8X_part2.Length + 10, 2);
                        Buffer.BlockCopy(dds_texture_data, 0, texture, 0x1F, height * width);

                        return texture;
                    }
                }
            }
        }
    }
}
