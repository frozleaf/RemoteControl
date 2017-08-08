namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.IO;

    internal class Base64Encoder
    {
        private readonly char[] _charsLine = new char[0x4c];
        private byte[] _leftOverBytes;
        private int _leftOverBytesCount;
        private readonly TextWriter _writer;
        private const int Base64LineSize = 0x4c;
        private const int LineSizeInBytes = 0x39;

        public Base64Encoder(TextWriter writer)
        {
            ValidationUtils.ArgumentNotNull(writer, "writer");
            this._writer = writer;
        }

        public void Encode(byte[] buffer, int index, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (count > (buffer.Length - index))
            {
                throw new ArgumentOutOfRangeException("count");
            }
            if (this._leftOverBytesCount > 0)
            {
                int num = this._leftOverBytesCount;
                while ((num < 3) && (count > 0))
                {
                    this._leftOverBytes[num++] = buffer[index++];
                    count--;
                }
                if ((count == 0) && (num < 3))
                {
                    this._leftOverBytesCount = num;
                    return;
                }
                int num2 = Convert.ToBase64CharArray(this._leftOverBytes, 0, 3, this._charsLine, 0);
                this.WriteChars(this._charsLine, 0, num2);
            }
            this._leftOverBytesCount = count % 3;
            if (this._leftOverBytesCount > 0)
            {
                count -= this._leftOverBytesCount;
                if (this._leftOverBytes == null)
                {
                    this._leftOverBytes = new byte[3];
                }
                for (int i = 0; i < this._leftOverBytesCount; i++)
                {
                    this._leftOverBytes[i] = buffer[(index + count) + i];
                }
            }
            int num4 = index + count;
            int length = 0x39;
            while (index < num4)
            {
                if ((index + length) > num4)
                {
                    length = num4 - index;
                }
                int num6 = Convert.ToBase64CharArray(buffer, index, length, this._charsLine, 0);
                this.WriteChars(this._charsLine, 0, num6);
                index += length;
            }
        }

        public void Flush()
        {
            if (this._leftOverBytesCount > 0)
            {
                int count = Convert.ToBase64CharArray(this._leftOverBytes, 0, this._leftOverBytesCount, this._charsLine, 0);
                this.WriteChars(this._charsLine, 0, count);
                this._leftOverBytesCount = 0;
            }
        }

        private void WriteChars(char[] chars, int index, int count)
        {
            this._writer.Write(chars, index, count);
        }
    }
}

