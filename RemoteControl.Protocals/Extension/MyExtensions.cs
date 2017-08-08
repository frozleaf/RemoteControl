using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemoteControl.Protocals
{
    public static class MyExtensions
    {
        public static byte[] SplitBytes(this List<byte> data, int start, int length)
        {
            byte[] result = new byte[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = data[i + start];
            }

            return result;
        }
    }
}
