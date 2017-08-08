namespace Newtonsoft.Json.Linq
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.IO;

    public class JRaw : JValue
    {
        public JRaw(JRaw other) : base((JValue) other)
        {
        }

        public JRaw(object rawJson) : base(rawJson, JTokenType.Raw)
        {
        }

        internal override JToken CloneToken()
        {
            return new JRaw(this);
        }

        public static JRaw Create(JsonReader reader)
        {
            JRaw raw;
            using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
            {
                using (JsonTextWriter writer2 = new JsonTextWriter(writer))
                {
                    writer2.WriteToken(reader);
                    raw = new JRaw(writer.ToString());
                }
            }
            return raw;
        }
    }
}

