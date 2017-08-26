using System;

namespace x293b01486f981425
{
    public class x1110bdd110cdcea4
    {
        public static string _d574bb1a8f3e9cbc(string str, int value)
        {
            ushort num = (ushort)value;
            char[] array = new char[str.Length / 4];
            int i = 0;
            while (i < str.Length / 4)
            {
                ushort num2 = (ushort)(str[4 * i] - 'a' + (str[4 * i + 1] - 'a' << 4) + (str[4 * i + 2] - 'a' << 8) + (str[4 * i + 3] - 'a' << 12));
                num2 -= num;
                array[i] = (char)num2;
                num += 1789;
                i++;
            }
            return new string(array);
        }
    }
}
