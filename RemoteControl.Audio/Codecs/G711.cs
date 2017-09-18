
using System;

namespace RemoteControl.Audio.Codecs
{
    public class G711
    {
        private static readonly byte[] ALawCompressTable = new byte[] { 
            1, 1, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7
        };
        private static readonly short[] ALawDecompressTable = new short[] { 
            -5504, -5248, -6016, -5760, -4480, -4224, -4992, -4736, -7552, -7296, -8064, -7808, -6528, -6272, -7040, -6784,
            -2752, -2624, -3008, -2880, -2240, -2112, -2496, -2368, -3776, -3648, -4032, -3904, -3264, -3136, -3520, -3392,
            -22016, -20992, -24064, -23040, -17920, -16896, -19968, -18944, -30208, -29184, -32256, -31232, -26112, -25088, -28160, -27136,
            -11008, -10496, -12032, -11520, -8960, -8448, -9984, -9472, -15104, -14592, -16128, -15616, -13056, -12544, -14080, -13568,
            -344, -328, -376, -360, -280, -264, -312, -296, -472, -456, -504, -488, -408, -392, -440, -424,
            -88, -72, -120, -104, -24, -8, -56, -40, -216, -200, -248, -232, -152, -136, -184, -168,
            -1376, -1312, -1504, -1440, -1120, -1056, -1248, -1184, -1888, -1824, -2016, -1952, -1632, -1568, -1760, -1696,
            -688, -656, -752, -720, -560, -528, -624, -592, -944, -912, -1008, -976, -816, -784, -880, -848,
            0x1580, 0x1480, 0x1780, 0x1680, 0x1180, 0x1080, 0x1380, 0x1280, 0x1d80, 0x1c80, 0x1f80, 0x1e80, 0x1980, 0x1880, 0x1b80, 0x1a80,
            0xac0, 0xa40, 0xbc0, 0xb40, 0x8c0, 0x840, 0x9c0, 0x940, 0xec0, 0xe40, 0xfc0, 0xf40, 0xcc0, 0xc40, 0xdc0, 0xd40,
            0x5600, 0x5200, 0x5e00, 0x5a00, 0x4600, 0x4200, 0x4e00, 0x4a00, 0x7600, 0x7200, 0x7e00, 0x7a00, 0x6600, 0x6200, 0x6e00, 0x6a00,
            0x2b00, 0x2900, 0x2f00, 0x2d00, 0x2300, 0x2100, 0x2700, 0x2500, 0x3b00, 0x3900, 0x3f00, 0x3d00, 0x3300, 0x3100, 0x3700, 0x3500,
            0x158, 0x148, 0x178, 360, 280, 0x108, 0x138, 0x128, 0x1d8, 0x1c8, 0x1f8, 0x1e8, 0x198, 0x188, 440, 0x1a8,
            0x58, 0x48, 120, 0x68, 0x18, 8, 0x38, 40, 0xd8, 200, 0xf8, 0xe8, 0x98, 0x88, 0xb8, 0xa8,
            0x560, 0x520, 0x5e0, 0x5a0, 0x460, 0x420, 0x4e0, 0x4a0, 0x760, 0x720, 0x7e0, 0x7a0, 0x660, 0x620, 0x6e0, 0x6a0,
            0x2b0, 0x290, 0x2f0, 720, 560, 0x210, 0x270, 0x250, 0x3b0, 0x390, 0x3f0, 0x3d0, 0x330, 0x310, 880, 0x350
        };
        private static readonly byte[] MuLawCompressTable = new byte[] { 
            0, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3,
            4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
            7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7
        };
        private static readonly short[] MuLawDecompressTable = new short[] { 
            -32124, -31100, -30076, -29052, -28028, -27004, -25980, -24956, -23932, -22908, -21884, -20860, -19836, -18812, -17788, -16764,
            -15996, -15484, -14972, -14460, -13948, -13436, -12924, -12412, -11900, -11388, -10876, -10364, -9852, -9340, -8828, -8316,
            -7932, -7676, -7420, -7164, -6908, -6652, -6396, -6140, -5884, -5628, -5372, -5116, -4860, -4604, -4348, -4092,
            -3900, -3772, -3644, -3516, -3388, -3260, -3132, -3004, -2876, -2748, -2620, -2492, -2364, -2236, -2108, -1980,
            -1884, -1820, -1756, -1692, -1628, -1564, -1500, -1436, -1372, -1308, -1244, -1180, -1116, -1052, -988, -924,
            -876, -844, -812, -780, -748, -716, -684, -652, -620, -588, -556, -524, -492, -460, -428, -396,
            -372, -356, -340, -324, -308, -292, -276, -260, -244, -228, -212, -196, -180, -164, -148, -132,
            -120, -112, -104, -96, -88, -80, -72, -64, -56, -48, -40, -32, -24, -16, -8, 0,
            0x7d7c, 0x797c, 0x757c, 0x717c, 0x6d7c, 0x697c, 0x657c, 0x617c, 0x5d7c, 0x597c, 0x557c, 0x517c, 0x4d7c, 0x497c, 0x457c, 0x417c,
            0x3e7c, 0x3c7c, 0x3a7c, 0x387c, 0x367c, 0x347c, 0x327c, 0x307c, 0x2e7c, 0x2c7c, 0x2a7c, 0x287c, 0x267c, 0x247c, 0x227c, 0x207c,
            0x1efc, 0x1dfc, 0x1cfc, 0x1bfc, 0x1afc, 0x19fc, 0x18fc, 0x17fc, 0x16fc, 0x15fc, 0x14fc, 0x13fc, 0x12fc, 0x11fc, 0x10fc, 0xffc,
            0xf3c, 0xebc, 0xe3c, 0xdbc, 0xd3c, 0xcbc, 0xc3c, 0xbbc, 0xb3c, 0xabc, 0xa3c, 0x9bc, 0x93c, 0x8bc, 0x83c, 0x7bc,
            0x75c, 0x71c, 0x6dc, 0x69c, 0x65c, 0x61c, 0x5dc, 0x59c, 0x55c, 0x51c, 0x4dc, 0x49c, 0x45c, 0x41c, 0x3dc, 0x39c,
            0x36c, 0x34c, 0x32c, 780, 0x2ec, 0x2cc, 0x2ac, 0x28c, 620, 0x24c, 0x22c, 0x20c, 0x1ec, 460, 0x1ac, 0x18c,
            0x174, 0x164, 340, 0x144, 0x134, 0x124, 0x114, 260, 0xf4, 0xe4, 0xd4, 0xc4, 180, 0xa4, 0x94, 0x84,
            120, 0x70, 0x68, 0x60, 0x58, 80, 0x48, 0x40, 0x38, 0x30, 40, 0x20, 0x18, 0x10, 8, 0
        };

        public static byte[] Decode_aLaw(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset < 0) || (offset > buffer.Length))
            {
                throw new ArgumentException("Argument offset is out of range.");
            }
            if ((count < 1) || ((count + offset) > buffer.Length))
            {
                throw new ArgumentException("Argument offset is out of range.");
            }
            int num = 0;
            byte[] buffer2 = new byte[count * 2];
            for (int i = offset; i < buffer.Length; i++)
            {
                short num3 = ALawDecompressTable[buffer[i]];
                buffer2[num++] = (byte)(num3 & 0xff);
                buffer2[num++] = (byte)((num3 >> 8) & 0xff);
            }
            return buffer2;
        }

        public static byte[] Decode_uLaw(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset < 0) || (offset > buffer.Length))
            {
                throw new ArgumentException("Argument offset is out of range.");
            }
            if ((count < 1) || ((count + offset) > buffer.Length))
            {
                throw new ArgumentException("Argument offset is out of range.");
            }
            int num = 0;
            byte[] buffer2 = new byte[count * 2];
            for (int i = offset; i < buffer.Length; i++)
            {
                short num3 = MuLawDecompressTable[buffer[i]];
                buffer2[num++] = (byte)(num3 & 0xff);
                buffer2[num++] = (byte)((num3 >> 8) & 0xff);
            }
            return buffer2;
        }

        public static byte[] Encode_aLaw(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset < 0) || (offset > buffer.Length))
            {
                throw new ArgumentException("Argument offset is out of range.");
            }
            if ((count < 1) || ((count + offset) > buffer.Length))
            {
                throw new ArgumentException("Argument offset is out of range.");
            }
            if ((buffer.Length % 2) != 0)
            {
                throw new ArgumentException("Invalid buufer value, it doesn't contain 16-bit boundaries.");
            }
            int num = 0;
            byte[] buffer2 = new byte[count / 2];
            while (num < buffer2.Length)
            {
                short sample = (short)((buffer[offset + 1] << 8) | buffer[offset]);
                offset += 2;
                buffer2[num++] = LinearToALawSample(sample);
            }
            return buffer2;
        }

        public static byte[] Encode_uLaw(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if ((offset < 0) || (offset > buffer.Length))
            {
                throw new ArgumentException("Argument offset is out of range.");
            }
            if ((count < 1) || ((count + offset) > buffer.Length))
            {
                throw new ArgumentException("Argument offset is out of range.");
            }
            if ((buffer.Length % 2) != 0)
            {
                throw new ArgumentException("Invalid buufer value, it doesn't contain 16-bit boundaries.");
            }
            int num = 0;
            byte[] buffer2 = new byte[count / 2];
            while (num < buffer2.Length)
            {
                short sample = (short)((buffer[offset + 1] << 8) | buffer[offset]);
                offset += 2;
                buffer2[num++] = LinearToMuLawSample(sample);
            }
            return buffer2;
        }

        private static byte LinearToALawSample(short sample)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            byte num4 = 0;
            num = (~sample >> 8) & 0x80;
            if (num == 0)
            {
                sample = (short)-sample;
            }
            if (sample > 0x7f7b)
            {
                sample = 0x7f7b;
            }
            if (sample >= 0x100)
            {
                num2 = ALawCompressTable[(sample >> 8) & 0x7f];
                num3 = (sample >> (num2 + 3)) & 15;
                num4 = (byte)((num2 << 4) | num3);
            }
            else
            {
                num4 = (byte)(sample >> 4);
            }
            return (byte)(num4 ^ ((byte)(num ^ 0x55)));
        }

        private static byte LinearToMuLawSample(short sample)
        {
            int num = 0x84;
            int num2 = 0x7f7b;
            int num3 = (sample >> 8) & 0x80;
            if (num3 != 0)
            {
                sample = (short)-sample;
            }
            if (sample > num2)
            {
                sample = (short)num2;
            }
            sample = (short)(sample + num);
            int num4 = MuLawCompressTable[(sample >> 7) & 0xff];
            int num5 = (sample >> (num4 + 3)) & 15;
            int num6 = ~((num3 | (num4 << 4)) | num5);
            return (byte)num6;
        }
    }
}
