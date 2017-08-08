using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Sunisoft.IrisSkin
{
    internal class x2b13dab2573bcd5e
    {
        private const int x9b3c370aac2bcae3 = 12;

        private static xb0f0f2071b49b1cb x36ab34a1d2fe3af6;

        private static xb0f0f2071b49b1cb x05f724456b7f79d1;

        static x2b13dab2573bcd5e()
        {
            x2b13dab2573bcd5e.x36ab34a1d2fe3af6 = new xb0f0f2071b49b1cb(15);
            x2b13dab2573bcd5e.x36ab34a1d2fe3af6.Build(x71589e3f55261426.CodeLens, 0, 288, x71589e3f55261426.LitExtraBits, 257);
            for (int i = 0; i < 32; i++)
            {
                x71589e3f55261426.CodeLens[i] = 5;
            }
            x2b13dab2573bcd5e.x05f724456b7f79d1 = new xb0f0f2071b49b1cb(15);
            x2b13dab2573bcd5e.x05f724456b7f79d1.Build(x71589e3f55261426.CodeLens, 0, 32, x71589e3f55261426.DistExtraBits, 0);
        }

        public static xfba9214ce91902fb Unzip(Stream readStream, string extractFile, Stream writeStream, string password)
        {
            xfba9214ce91902fb xfba9214ce91902fb = xfba9214ce91902fb.Success;
            x181df638a52dfd1a x181df638a52dfd1a = default(x181df638a52dfd1a);
            bool flag = false;
            try
            {
                BinaryReader binaryReader = new BinaryReader(readStream);
                readStream.Seek(0L, SeekOrigin.Begin);
                uint num = binaryReader.ReadUInt32();
                if ((num & 65535u) != 19283u)
                {
                    throw new Exception("not a zip file");
                }
                long num2 = x2b13dab2573bcd5e.xd54e92c965f241a7(readStream);
                if (num2 < 0L)
                {
                    throw new Exception("invalid zip file");
                }
                xf85cd48011a0ce1f xf85cd48011a0ce1f = (xf85cd48011a0ce1f)x2b13dab2573bcd5e.x49cceb88e1b6126e(binaryReader, typeof(xf85cd48011a0ce1f), Marshal.SizeOf(typeof(xf85cd48011a0ce1f)));
                if (xf85cd48011a0ce1f.ZipfileCommentLength > 0)
                {
                    readStream.Seek((long)xf85cd48011a0ce1f.ZipfileCommentLength, SeekOrigin.Begin);
                }
                readStream.Seek((long)xf85cd48011a0ce1f.DirectoryOffset, SeekOrigin.Begin);
                while (readStream.Position < num2)
                {
                    x181df638a52dfd1a = (x181df638a52dfd1a)x2b13dab2573bcd5e.x49cceb88e1b6126e(binaryReader, typeof(x181df638a52dfd1a), Marshal.SizeOf(typeof(x181df638a52dfd1a)));
                    byte[] bytes = binaryReader.ReadBytes((int)x181df638a52dfd1a.FileNameLength);
                    if (Encoding.Default.GetString(bytes).ToUpper() == extractFile.ToUpper())
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    throw new FileNotFoundException(extractFile);
                }
                readStream.Seek((long)x181df638a52dfd1a.RelativeOffset, SeekOrigin.Begin);
                x895eb13ee3f5b8cc x895eb13ee3f5b8cc = (x895eb13ee3f5b8cc)x2b13dab2573bcd5e.x49cceb88e1b6126e(binaryReader, typeof(x895eb13ee3f5b8cc), Marshal.SizeOf(typeof(x895eb13ee3f5b8cc)));
                readStream.Seek((long)x895eb13ee3f5b8cc.FileNameLength, SeekOrigin.Current);
                if (x895eb13ee3f5b8cc.ExtraFieldLength > 0)
                {
                    readStream.Seek((long)x895eb13ee3f5b8cc.ExtraFieldLength, SeekOrigin.Current);
                }
                if ((x181df638a52dfd1a.GeneralPurposeBitFlag & 1) != 0 && (password == null || password == ""))
                {
                    throw new ArgumentException("Password");
                }
                Stream stream2;
                if (password != null && password != "")
                {
                    int num3 = 305419896;
                    int num4 = 591751049;
                    int num5 = 878082192;
                    int num6 = 134775813;
                    byte[] array = binaryReader.ReadBytes(12);
                    for (int i = 1; i <= password.Length; i++)
                    {
                        int num7 = Convert.ToInt32(password[i - 1]);
                        num3 = x2b13dab2573bcd5e.x19339fe0ba40cd55((long)((ulong)x71589e3f55261426.CRC32Table[(num3 ^ num7) & 255] ^ (ulong)((long)(x2b13dab2573bcd5e.xa52147d0d9c2c73c(num3, 8) & 16777215))));
                        num4 += (num3 & 255);
                        num4 = num4 * num6 + 1;
                        num5 = x2b13dab2573bcd5e.x19339fe0ba40cd55((long)((ulong)x71589e3f55261426.CRC32Table[(num5 ^ x2b13dab2573bcd5e.xa52147d0d9c2c73c(num4, 24)) & 255] ^ (ulong)((long)(x2b13dab2573bcd5e.xa52147d0d9c2c73c(num5, 8) & 16777215))));
                    }
                    byte[] array2 = new byte[12];
                    for (int i = 0; i < 12; i++)
                    {
                        int num8 = (num5 & 65535) | 2;
                        array2[i] = x2b13dab2573bcd5e.x8885d1fc1e00dd5c((int)array[i] ^ num8 * (num8 ^ 1) >> 8);
                        num3 = x2b13dab2573bcd5e.x19339fe0ba40cd55((long)((ulong)x71589e3f55261426.CRC32Table[(num3 ^ (int)array2[i]) & 255] ^ (ulong)((long)(x2b13dab2573bcd5e.xa52147d0d9c2c73c(num3, 8) & 16777215))));
                        num4 += (num3 & 255);
                        num4 = num4 * num6 + 1;
                        num5 = x2b13dab2573bcd5e.x19339fe0ba40cd55((long)((ulong)x71589e3f55261426.CRC32Table[(num5 ^ x2b13dab2573bcd5e.xa52147d0d9c2c73c(num4, 24)) & 255] ^ (ulong)((long)(x2b13dab2573bcd5e.xa52147d0d9c2c73c(num5, 8) & 16777215))));
                    }
                    int val;
                    if ((x181df638a52dfd1a.GeneralPurposeBitFlag & 8) == 8)
                    {
                        val = (int)x181df638a52dfd1a.LastModFileTime << 16;
                    }
                    else
                    {
                        val = x181df638a52dfd1a.CRC32;
                    }
                    if (xe24cbd369cdd075a.FromInt32(val).L4 != array2[11])
                    {
                        throw new ArgumentException("Password");
                    }
                    Stream stream = new MemoryStream();
                    byte[] array3 = new byte[1024];
                    int num9;
                    for (num9 = readStream.Read(array3, 0, 1024); num9 == 1024; num9 = readStream.Read(array3, 0, 1024))
                    {
                        x2b13dab2573bcd5e.xe12a5d7e190699b3(array3, 1024, ref num3, ref num4, ref num5, num6);
                        stream.Write(array3, 0, 1024);
                    }
                    x2b13dab2573bcd5e.xe12a5d7e190699b3(array3, num9, ref num3, ref num4, ref num5, num6);
                    stream.Write(array3, 0, num9);
                    stream.Position = 0L;
                    stream2 = stream;
                }
                else
                {
                    stream2 = readStream;
                }
                byte[] array4 = new byte[16384];
                byte[] array5 = new byte[65794];
                int num10 = 65536;
                int num11 = 0;
                int num12 = 0;
                int x1ef26dbdd5d13d = 0;
                int num13 = 0;
                int xd44988f225497f3a = 0;
                int num14 = 0;
                int lookupBits = 0;
                int num15 = 0;
                int num16 = 0;
                int num17 = 0;
                int num18 = 0;
                int num19 = 0;
                int num20 = 0;
                int num21 = 0;
                int num22 = 0;
                int[] array6 = new int[317];
                int num26;
                int num25;
                int num24;
                int num23 = num24 = (num25 = (num26 = 0));
                while (true)
                {
                    if (num24 == 0)
                    {
                        if (num23 - num25 < 4)
                        {
                            x2b13dab2573bcd5e.x573284c529e7b3c9(stream2, array4, ref num25, ref num23);
                        }
                        num26 = (int)array4[num25] + ((int)array4[num25 + 1] << 8) + ((int)array4[num25 + 2] << 16) + ((int)array4[num25 + 3] << 24);
                        num25 += 4;
                        num24 = 32;
                    }
                    int num27 = num26 & 1;
                    num26 = x2b13dab2573bcd5e.xa52147d0d9c2c73c(num26, 1);
                    num24--;
                    int num8 = x2b13dab2573bcd5e.x02995f229cff83b4(2, ref num24, ref num26, ref num25, array4);
                    if (num8 == 0)
                    {
                        num26 = x2b13dab2573bcd5e.xa52147d0d9c2c73c(num26, num24 % 8);
                        num24 -= num24 % 8;
                        x2b13dab2573bcd5e.xabeeb97aae3dd1a9(stream2, num12, 4, num24, num26, num25, num23, array4);
                        int num28 = num12 & 65535;
                        byte[] array7 = new byte[16384];
                        while (num28 != 0)
                        {
                            int num29;
                            if (num28 > 16384)
                            {
                                num29 = 16384;
                            }
                            else
                            {
                                num29 = num28;
                            }
                            x2b13dab2573bcd5e.xabeeb97aae3dd1a9(stream2, x1ef26dbdd5d13d, num29, num24, num26, num25, num23, array4);
                            int j = num29;
                            if (num11 >= num10)
                            {
                                writeStream.Write(array5, 0, 32768);
                                num13 = 32768;
                                for (int i = 0; i < num11 - num13; i++)
                                {
                                    array7[i] = array5[i];
                                }
                                byte[] array8 = array7;
                                int num30 = num10 - num11;
                                if (num30 > j)
                                {
                                    num30 = j;
                                }
                                for (int i = 0; i < num30; i++)
                                {
                                    array8[i] = array5[num11];
                                }
                                num11 += num30;
                                num30 -= j;
                                int num31 = 0;
                                while (j > 0)
                                {
                                    num31 += num30;
                                    writeStream.Write(array5, 0, 32768);
                                    num13 = 32768;
                                    for (int i = 0; i < num11 - num13; i++)
                                    {
                                        array5[i] = array5[num13 + i];
                                    }
                                    num11 -= 32768;
                                    num30 = num10 - num11;
                                    if (num30 > j)
                                    {
                                        num30 = j;
                                    }
                                    for (int i = 0; i < num30; i++)
                                    {
                                        array5[num11 + i] = array8[num31 + i];
                                    }
                                    num11 += num30;
                                    num30 -= j;
                                }
                                num28 -= num29;
                            }
                        }
                    }
                    else if (num8 == 2)
                    {
                        int num32 = x2b13dab2573bcd5e.x02995f229cff83b4(5, ref num24, ref num26, ref num25, array4) + 257;
                        int num33 = x2b13dab2573bcd5e.x02995f229cff83b4(5, ref num24, ref num26, ref num25, array4) + 1;
                        int num34 = x2b13dab2573bcd5e.x02995f229cff83b4(4, ref num24, ref num26, ref num25, array4) + 4;
                        if (num32 > 286 || num33 > 30)
                        {
                            break;
                        }
                        for (int i = 0; i < 19; i++)
                        {
                            array6[i] = 0;
                        }
                        for (int i = 0; i < num34; i++)
                        {
                            array6[(int)x71589e3f55261426.CodeLengthIndex[i]] = x2b13dab2573bcd5e.x02995f229cff83b4(3, ref num24, ref num26, ref num25, array4);
                        }
                        xb0f0f2071b49b1cb xb0f0f2071b49b1cb = new xb0f0f2071b49b1cb(7);
                        xb0f0f2071b49b1cb.Build(array6, 0, 19, new byte[0], 65535);
                        for (int i = 0; i < array6.Length; i++)
                        {
                            array6[i] = 0;
                        }
                        int k = 0;
                        while (k < num32 + num33)
                        {
                            xd44988f225497f3a = xb0f0f2071b49b1cb.MaxCodeLen + 7;
                            num14 = x2b13dab2573bcd5e.xa13f94e29dbd5d94(stream2, ref xd44988f225497f3a, ref num24, ref num26, ref num25, ref num23, array4);
                            lookupBits = (num14 & x71589e3f55261426.ExtractMaskArray[xb0f0f2071b49b1cb.MaxCodeLen - 1]);
                            num15 = xb0f0f2071b49b1cb.Decode(lookupBits);
                            num22 = (num15 & 65535);
                            num18 = (x2b13dab2573bcd5e.xa52147d0d9c2c73c(num15, 16) & 255);
                            if (num22 <= 15)
                            {
                                array6[k] = num22;
                                k++;
                                x2b13dab2573bcd5e.xa4bb90c5f325d0af(num18, ref num24, ref num26, ref num25, array4);
                            }
                            else if (num22 == 16)
                            {
                                int num35 = 3 + (x2b13dab2573bcd5e.xa52147d0d9c2c73c(num14, num18) & 3);
                                num22 = array6[k - 1];
                                for (int i = 0; i < num35; i++)
                                {
                                    array6[k + i] = num22;
                                }
                                k += num35;
                                xd44988f225497f3a = num18 + 2;
                                x2b13dab2573bcd5e.xa4bb90c5f325d0af(xd44988f225497f3a, ref num24, ref num26, ref num25, array4);
                            }
                            else if (num22 == 17)
                            {
                                int num35 = 3 + (x2b13dab2573bcd5e.xa52147d0d9c2c73c(num14, num18) & 7);
                                k += num35;
                                xd44988f225497f3a = num18 + 3;
                                x2b13dab2573bcd5e.xa4bb90c5f325d0af(xd44988f225497f3a, ref num24, ref num26, ref num25, array4);
                            }
                            else if (num22 == 18)
                            {
                                int num35 = 11 + (x2b13dab2573bcd5e.xa52147d0d9c2c73c(num14, num18) & 127);
                                k += num35;
                                xd44988f225497f3a = num18 + 7;
                                x2b13dab2573bcd5e.xa4bb90c5f325d0af(xd44988f225497f3a, ref num24, ref num26, ref num25, array4);
                            }
                        }
                        xb0f0f2071b49b1cb xb0f0f2071b49b1cb2 = new xb0f0f2071b49b1cb(15);
                        xb0f0f2071b49b1cb2.Build(array6, 0, num32, x71589e3f55261426.LitExtraBits, 257);
                        xb0f0f2071b49b1cb xb0f0f2071b49b1cb3 = new xb0f0f2071b49b1cb(15);
                        xb0f0f2071b49b1cb3.Build(array6, num32, num33, x71589e3f55261426.DistExtraBits, 0);
                        x2b13dab2573bcd5e.xf76803be5e9ee2aa(xb0f0f2071b49b1cb2, xb0f0f2071b49b1cb3, stream2, writeStream, ref xd44988f225497f3a, ref num24, ref num26, ref num14, ref lookupBits, ref num15, ref num22, ref num18, ref num16, ref num17, ref num19, ref num25, ref num23, array4, ref num11, ref num10, array5, ref num13, ref num20, ref num21);
                    }
                    else if (num8 == 1)
                    {
                        x2b13dab2573bcd5e.xf76803be5e9ee2aa(x2b13dab2573bcd5e.x36ab34a1d2fe3af6, x2b13dab2573bcd5e.x05f724456b7f79d1, stream2, writeStream, ref xd44988f225497f3a, ref num24, ref num26, ref num14, ref lookupBits, ref num15, ref num22, ref num18, ref num16, ref num17, ref num19, ref num25, ref num23, array4, ref num11, ref num10, array5, ref num13, ref num20, ref num21);
                    }
                    if (num27 != 0)
                    {
                        goto Block_47;
                    }
                }
                throw new Exception("Unknown error");
            Block_47:
                if (num11 != 0)
                {
                    writeStream.Write(array5, 0, num11);
                }
            }
            catch (ArgumentException ex)
            {
                if (ex.Message == "Password")
                {
                    xfba9214ce91902fb = xfba9214ce91902fb.WorngPassword;
                }
                else
                {
                    xfba9214ce91902fb = xfba9214ce91902fb.Failed;
                }
            }
            catch
            {
                xfba9214ce91902fb = xfba9214ce91902fb.Failed;
            }
            if (writeStream != null && xfba9214ce91902fb == xfba9214ce91902fb.Success)
            {
                if (writeStream.Length == 0L)
                {
                    xfba9214ce91902fb = xfba9214ce91902fb.Failed;
                }
                else if (writeStream.Length != (long)x181df638a52dfd1a.UncompressedSize)
                {
                    xfba9214ce91902fb = xfba9214ce91902fb.WorngPassword;
                }
            }
            return xfba9214ce91902fb;
        }

        private static void xf76803be5e9ee2aa(xb0f0f2071b49b1cb xc82e2be11855266c, xb0f0f2071b49b1cb xd8260a8082223426, Stream x160656af282c4d7b, Stream x5ab6ed99b3041647, ref int xce53a4f2835cab70, ref int x4f0818a28ce2934b, ref int x880e0239b007e4f9, ref int xea2a21438bfbe03d, ref int xa60661b5d01cbcbc, ref int x42f081a275d72d2c, ref int xe59d6d35c76d70aa, ref int x57922afe7d4d09e0, ref int x5c34e2900c6b80d0, ref int x9b229eba46fd51f3, ref int x58316dde3396e982, ref int xf8894778b712e96d, ref int xfcbc8d697ad1a389, byte[] x5614faf5d2637115, ref int xe2b07c285d73cc12, ref int x971144bb4ad7e504, byte[] x7585f73a99663fd2, ref int x81dfe2898a2fdde1, ref int x4996da61da7808c8, ref int x4cc2dc507646ce1a)
        {
            xce53a4f2835cab70 = xc82e2be11855266c.MaxCodeLen + 5;
            xea2a21438bfbe03d = x2b13dab2573bcd5e.xa13f94e29dbd5d94(x160656af282c4d7b, ref xce53a4f2835cab70, ref x4f0818a28ce2934b, ref x880e0239b007e4f9, ref xf8894778b712e96d, ref xfcbc8d697ad1a389, x5614faf5d2637115);
            xa60661b5d01cbcbc = (xea2a21438bfbe03d & x71589e3f55261426.ExtractMaskArray[xc82e2be11855266c.MaxCodeLen - 1]);
            x42f081a275d72d2c = xc82e2be11855266c.Decode(xa60661b5d01cbcbc);
            xe59d6d35c76d70aa = (x42f081a275d72d2c & 65535);
            x57922afe7d4d09e0 = (x2b13dab2573bcd5e.xa52147d0d9c2c73c(x42f081a275d72d2c, 16) & 255);
            while (xe59d6d35c76d70aa != 256)
            {
                if (xe59d6d35c76d70aa < 256)
                {
                    x7585f73a99663fd2[xe2b07c285d73cc12] = (byte)xe59d6d35c76d70aa;
                    xe2b07c285d73cc12++;
                    if (xe2b07c285d73cc12 >= x971144bb4ad7e504)
                    {
                        x5ab6ed99b3041647.Write(x7585f73a99663fd2, 0, 32768);
                        x81dfe2898a2fdde1 = 32768;
                        for (int i = 0; i < xe2b07c285d73cc12 - x81dfe2898a2fdde1; i++)
                        {
                            x7585f73a99663fd2[i] = x7585f73a99663fd2[x81dfe2898a2fdde1 + i];
                        }
                        xe2b07c285d73cc12 -= 32768;
                    }
                    x2b13dab2573bcd5e.xa4bb90c5f325d0af(x57922afe7d4d09e0, ref x4f0818a28ce2934b, ref x880e0239b007e4f9, ref xf8894778b712e96d, x5614faf5d2637115);
                }
                else
                {
                    if (xe59d6d35c76d70aa == 285)
                    {
                        x5c34e2900c6b80d0 = 258;
                        x2b13dab2573bcd5e.xa4bb90c5f325d0af(x57922afe7d4d09e0, ref x4f0818a28ce2934b, ref x880e0239b007e4f9, ref xf8894778b712e96d, x5614faf5d2637115);
                    }
                    else
                    {
                        x9b229eba46fd51f3 = x2b13dab2573bcd5e.xa52147d0d9c2c73c(x42f081a275d72d2c, 24);
                        if (x9b229eba46fd51f3 == 0)
                        {
                            x5c34e2900c6b80d0 = (int)x71589e3f55261426.LengthBase[xe59d6d35c76d70aa - 257];
                            x2b13dab2573bcd5e.xa4bb90c5f325d0af(x57922afe7d4d09e0, ref x4f0818a28ce2934b, ref x880e0239b007e4f9, ref xf8894778b712e96d, x5614faf5d2637115);
                        }
                        else
                        {
                            x5c34e2900c6b80d0 = (int)x71589e3f55261426.LengthBase[xe59d6d35c76d70aa - 257] + (x2b13dab2573bcd5e.xa52147d0d9c2c73c(xea2a21438bfbe03d, x57922afe7d4d09e0) & x71589e3f55261426.ExtractMaskArray[x9b229eba46fd51f3 - 1]);
                            xce53a4f2835cab70 = x57922afe7d4d09e0 + x9b229eba46fd51f3;
                            x2b13dab2573bcd5e.xa4bb90c5f325d0af(xce53a4f2835cab70, ref x4f0818a28ce2934b, ref x880e0239b007e4f9, ref xf8894778b712e96d, x5614faf5d2637115);
                        }
                    }
                    xce53a4f2835cab70 = xd8260a8082223426.MaxCodeLen + 14;
                    xea2a21438bfbe03d = x2b13dab2573bcd5e.xa13f94e29dbd5d94(x160656af282c4d7b, ref xce53a4f2835cab70, ref x4f0818a28ce2934b, ref x880e0239b007e4f9, ref xf8894778b712e96d, ref xfcbc8d697ad1a389, x5614faf5d2637115);
                    xa60661b5d01cbcbc = (xea2a21438bfbe03d & x71589e3f55261426.ExtractMaskArray[xd8260a8082223426.MaxCodeLen - 1]);
                    x42f081a275d72d2c = xd8260a8082223426.Decode(xa60661b5d01cbcbc);
                    xe59d6d35c76d70aa = (x42f081a275d72d2c & 65535);
                    x57922afe7d4d09e0 = (x2b13dab2573bcd5e.xa52147d0d9c2c73c(x42f081a275d72d2c, 16) & 255);
                    x9b229eba46fd51f3 = x2b13dab2573bcd5e.xa52147d0d9c2c73c(x42f081a275d72d2c, 24);
                    if (x9b229eba46fd51f3 == 0)
                    {
                        x58316dde3396e982 = (int)x71589e3f55261426.DistanceBase[xe59d6d35c76d70aa];
                        x2b13dab2573bcd5e.xa4bb90c5f325d0af(x57922afe7d4d09e0, ref x4f0818a28ce2934b, ref x880e0239b007e4f9, ref xf8894778b712e96d, x5614faf5d2637115);
                    }
                    else
                    {
                        x58316dde3396e982 = (int)x71589e3f55261426.DistanceBase[xe59d6d35c76d70aa] + (x2b13dab2573bcd5e.xa52147d0d9c2c73c(xea2a21438bfbe03d, x57922afe7d4d09e0) & x71589e3f55261426.ExtractMaskArray[x9b229eba46fd51f3 - 1]);
                        xce53a4f2835cab70 = x57922afe7d4d09e0 + x9b229eba46fd51f3;
                        x2b13dab2573bcd5e.xa4bb90c5f325d0af(xce53a4f2835cab70, ref x4f0818a28ce2934b, ref x880e0239b007e4f9, ref xf8894778b712e96d, x5614faf5d2637115);
                    }
                    if (x5c34e2900c6b80d0 <= x58316dde3396e982)
                    {
                        for (int i = 0; i < x5c34e2900c6b80d0; i++)
                        {
                            x7585f73a99663fd2[xe2b07c285d73cc12 + i] = x7585f73a99663fd2[xe2b07c285d73cc12 - x58316dde3396e982 + i];
                        }
                    }
                    else
                    {
                        x4996da61da7808c8 = xe2b07c285d73cc12 - x58316dde3396e982;
                        x4cc2dc507646ce1a = xe2b07c285d73cc12;
                        for (int i = 1; i <= x5c34e2900c6b80d0; i++)
                        {
                            x7585f73a99663fd2[x4cc2dc507646ce1a] = x7585f73a99663fd2[x4996da61da7808c8];
                            x4996da61da7808c8++;
                            x4cc2dc507646ce1a++;
                        }
                    }
                    xe2b07c285d73cc12 += x5c34e2900c6b80d0;
                    if (xe2b07c285d73cc12 >= x971144bb4ad7e504)
                    {
                        x5ab6ed99b3041647.Write(x7585f73a99663fd2, 0, 32768);
                        x81dfe2898a2fdde1 = 32768;
                        for (int i = 0; i < xe2b07c285d73cc12 - x81dfe2898a2fdde1; i++)
                        {
                            x7585f73a99663fd2[i] = x7585f73a99663fd2[x81dfe2898a2fdde1 + i];
                        }
                        xe2b07c285d73cc12 -= 32768;
                    }
                }
                xce53a4f2835cab70 = xc82e2be11855266c.MaxCodeLen + 5;
                xea2a21438bfbe03d = x2b13dab2573bcd5e.xa13f94e29dbd5d94(x160656af282c4d7b, ref xce53a4f2835cab70, ref x4f0818a28ce2934b, ref x880e0239b007e4f9, ref xf8894778b712e96d, ref xfcbc8d697ad1a389, x5614faf5d2637115);
                xa60661b5d01cbcbc = (xea2a21438bfbe03d & x71589e3f55261426.ExtractMaskArray[xc82e2be11855266c.MaxCodeLen - 1]);
                x42f081a275d72d2c = xc82e2be11855266c.Decode(xa60661b5d01cbcbc);
                xe59d6d35c76d70aa = (x42f081a275d72d2c & 65535);
                x57922afe7d4d09e0 = (x2b13dab2573bcd5e.xa52147d0d9c2c73c(x42f081a275d72d2c, 16) & 255);
            }
            x2b13dab2573bcd5e.xa4bb90c5f325d0af(x57922afe7d4d09e0, ref x4f0818a28ce2934b, ref x880e0239b007e4f9, ref xf8894778b712e96d, x5614faf5d2637115);
        }

        public static xfba9214ce91902fb Unzip(string ZipFileName, string ExtractFile, string OutputFile, string Password)
        {
            if (!File.Exists(ZipFileName))
            {
                return xfba9214ce91902fb.Failed;
            }
            xfba9214ce91902fb result = xfba9214ce91902fb.Success;
            Stream stream = null;
            Stream stream2 = null;
            try
            {
                stream = File.Open(ZipFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                stream2 = File.Open(OutputFile, FileMode.Create, FileAccess.Write, FileShare.None);
                result = x2b13dab2573bcd5e.Unzip(stream, ExtractFile, stream2, Password);
            }
            catch
            {
                result = xfba9214ce91902fb.Failed;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (stream2 != null)
                {
                    stream2.Close();
                }
            }
            return result;
        }

        private static long xd54e92c965f241a7(Stream xcf18e5243f8d5fd3)
        {
            long num = xcf18e5243f8d5fd3.Seek((long)(-(long)Marshal.SizeOf(typeof(x311a6dc8e19b6c24))), SeekOrigin.End);
            if (num > 0L)
            {
                BinaryReader xe134235b3526fa = new BinaryReader(xcf18e5243f8d5fd3);
                x311a6dc8e19b6c24 x311a6dc8e19b6c = (x311a6dc8e19b6c24)x2b13dab2573bcd5e.x49cceb88e1b6126e(xe134235b3526fa, typeof(x311a6dc8e19b6c24), Marshal.SizeOf(typeof(x311a6dc8e19b6c24)));
                if (x311a6dc8e19b6c.trSig == 101010256 && x311a6dc8e19b6c.trLen == 0)
                {
                    xcf18e5243f8d5fd3.Seek(num, SeekOrigin.Begin);
                }
            }
            return num;
        }

        private static void xe12a5d7e190699b3(byte[] x1ef26dbdd5d13d24, int x10f4d88af727adbc, ref int xf0b4c3c679d18c3c, ref int x3713ef252be01cf5, ref int xd56b578b5e9b09c0, int xccb1eeef6a33e9aa)
        {
            for (int i = 0; i < x10f4d88af727adbc; i++)
            {
                int num = (xd56b578b5e9b09c0 & 65535) | 2;
                x1ef26dbdd5d13d24[i] = x2b13dab2573bcd5e.x8885d1fc1e00dd5c((int)x1ef26dbdd5d13d24[i] ^ x2b13dab2573bcd5e.xa52147d0d9c2c73c(num * (num ^ 1), 8));
                xf0b4c3c679d18c3c = x2b13dab2573bcd5e.x19339fe0ba40cd55((long)((ulong)x71589e3f55261426.CRC32Table[(xf0b4c3c679d18c3c ^ (int)x1ef26dbdd5d13d24[i]) & 255] ^ (ulong)((long)(xf0b4c3c679d18c3c >> 8 & 16777215))));
                x3713ef252be01cf5 += (xf0b4c3c679d18c3c & 255);
                x3713ef252be01cf5 = x3713ef252be01cf5 * xccb1eeef6a33e9aa + 1;
                xd56b578b5e9b09c0 = x2b13dab2573bcd5e.x19339fe0ba40cd55((long)((ulong)x71589e3f55261426.CRC32Table[(xd56b578b5e9b09c0 ^ x3713ef252be01cf5 >> 24) & 255] ^ (ulong)((long)(xd56b578b5e9b09c0 >> 8 & 16777215))));
            }
        }

        private unsafe static object x49cceb88e1b6126e(BinaryReader xe134235b3526fa75, Type x43163d22e8cd5a71, int x0ceec69a97f73617)
        {
            byte[] array = xe134235b3526fa75.ReadBytes(x0ceec69a97f73617);
            object result;
            fixed (byte* ptr = &array[0])
            {
                result = Marshal.PtrToStructure((IntPtr)((void*)ptr), x43163d22e8cd5a71);
            }
            return result;
        }

        private static bool x573284c529e7b3c9(Stream xcf18e5243f8d5fd3, byte[] x11e8eb31c2edb458, ref int x89682117b5f7515d, ref int xdda8a75b25a2876d)
        {
            int num = 0;
            while (x89682117b5f7515d != xdda8a75b25a2876d)
            {
                x11e8eb31c2edb458[num] = x11e8eb31c2edb458[x89682117b5f7515d];
                x89682117b5f7515d++;
                num++;
            }
            int count = 16384 - num;
            int num2 = xcf18e5243f8d5fd3.Read(x11e8eb31c2edb458, num, count);
            x89682117b5f7515d = 0;
            xdda8a75b25a2876d = num + num2;
            int num3 = xdda8a75b25a2876d;
            if (num3 == 0)
            {
                return false;
            }
            if (num2 == 0 && num3 % 4 != 0)
            {
                int num4 = 4 - num3 % 4;
                for (int i = 0; i < num4; i++)
                {
                    x11e8eb31c2edb458[xdda8a75b25a2876d] = 0;
                    xdda8a75b25a2876d++;
                }
            }
            return true;
        }

        private static int x02995f229cff83b4(int x10f4d88af727adbc, ref int x67d7b14657e9f9cd, ref int xff1f79a9916b9886, ref int x89682117b5f7515d, byte[] x11e8eb31c2edb458)
        {
            int num;
            if (x10f4d88af727adbc <= x67d7b14657e9f9cd)
            {
                num = (xff1f79a9916b9886 & x71589e3f55261426.ExtractMaskArray[x10f4d88af727adbc - 1]);
                xff1f79a9916b9886 = x2b13dab2573bcd5e.xa52147d0d9c2c73c(xff1f79a9916b9886, x10f4d88af727adbc);
                x67d7b14657e9f9cd -= x10f4d88af727adbc;
            }
            else
            {
                int num2 = x10f4d88af727adbc - x67d7b14657e9f9cd;
                num = xff1f79a9916b9886;
                xff1f79a9916b9886 = (int)x11e8eb31c2edb458[x89682117b5f7515d] + ((int)x11e8eb31c2edb458[x89682117b5f7515d + 1] << 8) + ((int)x11e8eb31c2edb458[x89682117b5f7515d + 2] << 16) + ((int)x11e8eb31c2edb458[x89682117b5f7515d + 3] << 24);
                x89682117b5f7515d += 4;
                num += (xff1f79a9916b9886 & x71589e3f55261426.ExtractMaskArray[num2 - 1]) << x67d7b14657e9f9cd;
                xff1f79a9916b9886 = x2b13dab2573bcd5e.xa52147d0d9c2c73c(xff1f79a9916b9886, num2);
                x67d7b14657e9f9cd = 32 - num2;
            }
            return num;
        }

        private unsafe static void xabeeb97aae3dd1a9(Stream xcf18e5243f8d5fd3, int x1ef26dbdd5d13d24, int x10f4d88af727adbc, int x475641bf9383557d, int xff1f79a9916b9886, int x89682117b5f7515d, int xdda8a75b25a2876d, byte[] x11e8eb31c2edb458)
        {
            byte* ptr = (byte*)(&x1ef26dbdd5d13d24);
            int num;
            if (x475641bf9383557d > 0)
            {
                num = x475641bf9383557d / 8;
                for (int i = 0; i < num; i++)
                {
                    *ptr = Convert.ToByte(xff1f79a9916b9886 & 255);
                    ptr++;
                    xff1f79a9916b9886 = x2b13dab2573bcd5e.xa52147d0d9c2c73c(xff1f79a9916b9886, 8);
                }
                num -= x10f4d88af727adbc;
            }
            int num2 = xdda8a75b25a2876d - x89682117b5f7515d;
            if (x10f4d88af727adbc <= num2)
            {
                num = x10f4d88af727adbc;
            }
            else
            {
                num = num2;
            }
            for (int j = 0; j < num; j++)
            {
                ptr[j] = x11e8eb31c2edb458[x89682117b5f7515d + j];
            }
            x10f4d88af727adbc -= num;
            x89682117b5f7515d += num;
            while (x10f4d88af727adbc != 0)
            {
                ptr += num;
                if (!x2b13dab2573bcd5e.x573284c529e7b3c9(xcf18e5243f8d5fd3, x11e8eb31c2edb458, ref x89682117b5f7515d, ref xdda8a75b25a2876d))
                {
                    return;
                }
                num2 = xdda8a75b25a2876d - x89682117b5f7515d;
                if (x10f4d88af727adbc <= num2)
                {
                    num = x10f4d88af727adbc;
                }
                else
                {
                    num = num2;
                }
                for (int k = 0; k < num; k++)
                {
                    ptr[k] = x11e8eb31c2edb458[x89682117b5f7515d + k];
                }
                x10f4d88af727adbc -= num;
                x89682117b5f7515d += num;
            }
            xff1f79a9916b9886 = 0;
            x475641bf9383557d = 0;
        }

        private static int xa13f94e29dbd5d94(Stream xcf18e5243f8d5fd3, ref int xd44988f225497f3a, ref int x4f0818a28ce2934b, ref int x880e0239b007e4f9, ref int xf8894778b712e96d, ref int xfcbc8d697ad1a389, byte[] x5614faf5d2637115)
        {
            int num;
            if (xd44988f225497f3a <= x4f0818a28ce2934b)
            {
                num = (x880e0239b007e4f9 & x71589e3f55261426.ExtractMaskArray[xd44988f225497f3a - 1]);
            }
            else
            {
                int num2 = xd44988f225497f3a - x4f0818a28ce2934b;
                num = x880e0239b007e4f9;
                int num3;
                if (xfcbc8d697ad1a389 - xf8894778b712e96d < 4)
                {
                    if (!x2b13dab2573bcd5e.x573284c529e7b3c9(xcf18e5243f8d5fd3, x5614faf5d2637115, ref xf8894778b712e96d, ref xfcbc8d697ad1a389))
                    {
                        num3 = 0;
                    }
                    else
                    {
                        num3 = (int)x5614faf5d2637115[xf8894778b712e96d] + ((int)x5614faf5d2637115[xf8894778b712e96d + 1] << 8) + ((int)x5614faf5d2637115[xf8894778b712e96d + 2] << 16) + ((int)x5614faf5d2637115[xf8894778b712e96d + 3] << 24);
                    }
                }
                else
                {
                    num3 = (int)x5614faf5d2637115[xf8894778b712e96d] + ((int)x5614faf5d2637115[xf8894778b712e96d + 1] << 8) + ((int)x5614faf5d2637115[xf8894778b712e96d + 2] << 16) + ((int)x5614faf5d2637115[xf8894778b712e96d + 3] << 24);
                }
                num += (num3 & x71589e3f55261426.ExtractMaskArray[num2 - 1]) << x4f0818a28ce2934b;
            }
            return num;
        }

        private static void xa4bb90c5f325d0af(int xd44988f225497f3a, ref int x4f0818a28ce2934b, ref int x880e0239b007e4f9, ref int xf8894778b712e96d, byte[] x5614faf5d2637115)
        {
            if (xd44988f225497f3a <= x4f0818a28ce2934b)
            {
                x880e0239b007e4f9 = x2b13dab2573bcd5e.xa52147d0d9c2c73c(x880e0239b007e4f9, xd44988f225497f3a);
                x4f0818a28ce2934b -= xd44988f225497f3a;
                return;
            }
            int num = xd44988f225497f3a - x4f0818a28ce2934b;
            x880e0239b007e4f9 = (int)x5614faf5d2637115[xf8894778b712e96d] + ((int)x5614faf5d2637115[xf8894778b712e96d + 1] << 8) + ((int)x5614faf5d2637115[xf8894778b712e96d + 2] << 16) + ((int)x5614faf5d2637115[xf8894778b712e96d + 3] << 24);
            xf8894778b712e96d += 4;
            x880e0239b007e4f9 = x2b13dab2573bcd5e.xa52147d0d9c2c73c(x880e0239b007e4f9, num);
            x4f0818a28ce2934b = 32 - num;
        }

        private static int xa52147d0d9c2c73c(int x7b28e8a789372508, int x1148d0e8cc982c04)
        {
            if (x7b28e8a789372508 > 0)
            {
                return x7b28e8a789372508 >> x1148d0e8cc982c04;
            }
            return (int)((uint)x7b28e8a789372508 >> x1148d0e8cc982c04);
        }

        private static int x19339fe0ba40cd55(uint v)
        {
            if (v < 2147483647u)
            {
                return (int)v;
            }
            return (int)(-(4294967295u - v) - 1);
        }

        private static int x19339fe0ba40cd55(long x50a18ad2656e7181)
        {
            if (x50a18ad2656e7181 < 2147483647L)
            {
                return (int)x50a18ad2656e7181;
            }
            return x2b13dab2573bcd5e.x19339fe0ba40cd55(Convert.ToUInt32(x50a18ad2656e7181));
        }

        private static byte x8885d1fc1e00dd5c(int x50a18ad2656e7181)
        {
            return (byte)(x50a18ad2656e7181 & 255);
        }
    }
}
