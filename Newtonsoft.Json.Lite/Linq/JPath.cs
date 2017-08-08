namespace Newtonsoft.Json.Linq
{
    using Newtonsoft.Json.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.CompilerServices;

    internal class JPath
    {
        private int _currentIndex;
        private readonly string _expression;

        public JPath(string expression)
        {
            ValidationUtils.ArgumentNotNull(expression, "expression");
            this._expression = expression;
            this.Parts = new List<object>();
            this.ParseMain();
        }

        internal JToken Evaluate(JToken root, bool errorWhenNoMatch)
        {
            JToken token = root;
            foreach (object obj2 in this.Parts)
            {
                string str = obj2 as string;
                if (str != null)
                {
                    JObject obj3 = token as JObject;
                    if (obj3 == null)
                    {
                        if (errorWhenNoMatch)
                        {
                            throw new Exception("Property '{0}' not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, new object[] { str, token.GetType().Name }));
                        }
                        return null;
                    }
                    token = obj3[str];
                    if ((token == null) && errorWhenNoMatch)
                    {
                        throw new Exception("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.InvariantCulture, new object[] { str }));
                    }
                }
                else
                {
                    int num = (int) obj2;
                    JArray array = token as JArray;
                    if (array != null)
                    {
                        if (array.Count <= num)
                        {
                            if (errorWhenNoMatch)
                            {
                                throw new IndexOutOfRangeException("Index {0} outside the bounds of JArray.".FormatWith(CultureInfo.InvariantCulture, new object[] { num }));
                            }
                            return null;
                        }
                        token = array[num];
                    }
                    else
                    {
                        if (errorWhenNoMatch)
                        {
                            throw new Exception("Index {0} not valid on {1}.".FormatWith(CultureInfo.InvariantCulture, new object[] { num, token.GetType().Name }));
                        }
                        return null;
                    }
                }
            }
            return token;
        }

        private void ParseIndexer(char indexerOpenChar)
        {
            this._currentIndex++;
            char ch = (indexerOpenChar == '[') ? ']' : ')';
            int startIndex = this._currentIndex;
            int length = 0;
            bool flag = false;
            while (this._currentIndex < this._expression.Length)
            {
                char c = this._expression[this._currentIndex];
                if (char.IsDigit(c))
                {
                    length++;
                }
                else
                {
                    if (c != ch)
                    {
                        throw new Exception("Unexpected character while parsing path indexer: " + c);
                    }
                    flag = true;
                    break;
                }
                this._currentIndex++;
            }
            if (!flag)
            {
                throw new Exception("Path ended with open indexer. Expected " + ch);
            }
            if (length == 0)
            {
                throw new Exception("Empty path indexer.");
            }
            string str = this._expression.Substring(startIndex, length);
            this.Parts.Add(Convert.ToInt32(str, CultureInfo.InvariantCulture));
        }

        private void ParseMain()
        {
            string str;
            int startIndex = this._currentIndex;
            bool flag = false;
            while (this._currentIndex < this._expression.Length)
            {
                char indexerOpenChar = this._expression[this._currentIndex];
                switch (indexerOpenChar)
                {
                    case '(':
                    case '[':
                        if (this._currentIndex > startIndex)
                        {
                            str = this._expression.Substring(startIndex, this._currentIndex - startIndex);
                            this.Parts.Add(str);
                        }
                        this.ParseIndexer(indexerOpenChar);
                        startIndex = this._currentIndex + 1;
                        flag = true;
                        break;

                    case ')':
                    case ']':
                        throw new Exception("Unexpected character while parsing path: " + indexerOpenChar);

                    case '.':
                        if (this._currentIndex > startIndex)
                        {
                            str = this._expression.Substring(startIndex, this._currentIndex - startIndex);
                            this.Parts.Add(str);
                        }
                        startIndex = this._currentIndex + 1;
                        flag = false;
                        break;

                    default:
                        if (flag)
                        {
                            throw new Exception("Unexpected character following indexer: " + indexerOpenChar);
                        }
                        break;
                }
                this._currentIndex++;
            }
            if (this._currentIndex > startIndex)
            {
                str = this._expression.Substring(startIndex, this._currentIndex - startIndex);
                this.Parts.Add(str);
            }
        }

        public List<object> Parts { get; private set; }
    }
}

