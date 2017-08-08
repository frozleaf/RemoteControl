namespace Newtonsoft.Json.Utilities
{
    using System;
    using System.IO;

    internal static class JavaScriptUtils
    {
        public static string ToEscapedJavaScriptString(string value)
        {
            return ToEscapedJavaScriptString(value, '"', true);
        }

        public static string ToEscapedJavaScriptString(string value, char delimiter, bool appendDelimiters)
        {
            int? length = StringUtils.GetLength(value);
            using (StringWriter writer = StringUtils.CreateStringWriter(length.HasValue ? length.GetValueOrDefault() : 0x10))
            {
                WriteEscapedJavaScriptString(writer, value, delimiter, appendDelimiters);
                return writer.ToString();
            }
        }

        public static void WriteEscapedJavaScriptString(TextWriter writer, string value, char delimiter, bool appendDelimiters)
        {
            if (appendDelimiters)
            {
                writer.Write(delimiter);
            }
            if (value != null)
            {
                int index = 0;
                int count = 0;
                char[] buffer = null;
                for (int i = 0; i < value.Length; i++)
                {
                    string str;
                    char c = value[i];
                    char ch2 = c;
                    if (ch2 <= '\'')
                    {
                        switch (ch2)
                        {
                            case '\b':
                                str = @"\b";
                                goto Label_0124;

                            case '\t':
                                str = @"\t";
                                goto Label_0124;

                            case '\n':
                                str = @"\n";
                                goto Label_0124;

                            case '\f':
                                str = @"\f";
                                goto Label_0124;

                            case '\r':
                                str = @"\r";
                                goto Label_0124;

                            case '"':
                                goto Label_00FF;

                            case '\'':
                                goto Label_00EE;
                        }
                        goto Label_0110;
                    }
                    if (ch2 != '\\')
                    {
                        switch (ch2)
                        {
                            case '\u2028':
                                str = @"\u2028";
                                goto Label_0124;

                            case '\u2029':
                                str = @"\u2029";
                                goto Label_0124;

                            case '\x0085':
                                goto Label_00D3;
                        }
                        goto Label_0110;
                    }
                    str = @"\\";
                    goto Label_0124;
                Label_00D3:
                    str = @"\u0085";
                    goto Label_0124;
                Label_00EE:
                    str = (delimiter == '\'') ? @"\'" : null;
                    goto Label_0124;
                Label_00FF:
                    str = (delimiter == '"') ? "\\\"" : null;
                    goto Label_0124;
                Label_0110:
                    str = (c <= '\x001f') ? StringUtils.ToCharAsUnicode(c) : null;
                Label_0124:
                    if (str != null)
                    {
                        if (buffer == null)
                        {
                            buffer = value.ToCharArray();
                        }
                        if (count > 0)
                        {
                            writer.Write(buffer, index, count);
                            count = 0;
                        }
                        writer.Write(str);
                        index = i + 1;
                    }
                    else
                    {
                        count++;
                    }
                }
                if (count > 0)
                {
                    if (index == 0)
                    {
                        writer.Write(value);
                    }
                    else
                    {
                        writer.Write(buffer, index, count);
                    }
                }
            }
            if (appendDelimiters)
            {
                writer.Write(delimiter);
            }
        }
    }
}

