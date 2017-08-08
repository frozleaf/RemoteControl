namespace Sunisoft.IrisSkin
{
    using System;

    internal class xb0f0f2071b49b1cb
    {
        private int xca3516d7e7bf3e88;
        private int[] xd56734c5eb5c07a0;
        private int xda0cfe0865a46405;

        public xb0f0f2071b49b1cb(int defMaxCodeLen)
        {
            this.xda0cfe0865a46405 = defMaxCodeLen;
        }

        public void Build(int[] CodeLenths, int StartInx, int Count, byte[] ExtraBits, int ExtraOffset)
        {
            int num3;
            int[] numArray = new int[0x10];
            int[] numArray2 = new int[0x10];
            this.xca3516d7e7bf3e88 = 0;
            for (int i = 0; i < Count; i++)
            {
                num3 = CodeLenths[i + StartInx];
                if (num3 > this.xca3516d7e7bf3e88)
                {
                    this.xca3516d7e7bf3e88 = num3;
                }
                numArray[num3]++;
            }
            int num5 = x71589e3f55261426.PowerOfTwo[this.xca3516d7e7bf3e88];
            this.xd56734c5eb5c07a0 = new int[num5 * 4];
            int num7 = num5;
            for (int j = 0; j < (num5 * 4); j++)
            {
                this.xd56734c5eb5c07a0[j] = 0;
            }
            int num2 = 0;
            numArray[0] = 0;
            for (int k = 1; k <= this.xda0cfe0865a46405; k++)
            {
                num2 = (num2 + numArray[k - 1]) << 1;
                numArray2[k] = num2;
            }
            int[] numArray3 = this.xd56734c5eb5c07a0;
            for (int m = 0; m < Count; m++)
            {
                num3 = CodeLenths[m + StartInx];
                if (num3 != 0)
                {
                    num2 = numArray2[num3];
                    int num12 = shr(num2 & 0xff00, 8);
                    int index = num2 & 0xff;
                    int num13 = num12;
                    num12 = x71589e3f55261426.ByteRevTable[index];
                    index = x71589e3f55261426.ByteRevTable[num13];
                    num2 = ((num2 - (num2 & 0xffff)) + (num12 << 8)) + index;
                    num2 = shr(num2, 0x10 - num3);
                    int num4 = m + (num3 << 0x10);
                    if (m >= ExtraOffset)
                    {
                        num4 += ExtraBits[m - ExtraOffset] << 0x18;
                    }
                    int num6 = x71589e3f55261426.PowerOfTwo[num3];
                    int num = num2;
                    do
                    {
                        numArray3[num] = num4;
                        num += num6;
                    }
                    while (num < num7);
                    numArray2[num3]++;
                }
            }
        }

        public int Decode(int lookupBits)
        {
            return this.xd56734c5eb5c07a0[lookupBits];
        }

        public static int shr(int i, int j)
        {
            if (i > 0)
            {
                return (i >> j);
            }
            return (i >> j);
        }

        public int MaxCodeLen
        {
            get
            {
                return this.xca3516d7e7bf3e88;
            }
        }
    }
}

