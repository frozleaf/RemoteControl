using System;

namespace x293b01486f981425
{
    public class x1110bdd110cdcea4
    {
        public static string _d574bb1a8f3e9cbc(string x5e99b576d2530d13, int x2710752c36f2d14b)
        {
            ushort num = (ushort)x2710752c36f2d14b;
            char[] array = new char[x5e99b576d2530d13.Length / 4];
            int i = 0;
            while (i < x5e99b576d2530d13.Length / 4)
            {
                ushort num2 = (ushort)(x5e99b576d2530d13[4 * i] - 'a' + (x5e99b576d2530d13[4 * i + 1] - 'a' << 4) + (x5e99b576d2530d13[4 * i + 2] - 'a' << 8) + (x5e99b576d2530d13[4 * i + 3] - 'a' << 12));
                num2 -= num;
                array[i] = (char)num2;
                num += 1789;
                int arg_3A_0 = i++;
            }
            return new string(array);
        }
    }
}
