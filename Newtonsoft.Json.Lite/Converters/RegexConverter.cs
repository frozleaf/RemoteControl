namespace Newtonsoft.Json.Converters
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public class RegexConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Regex));
        }

        private bool HasFlag(RegexOptions options, RegexOptions flag)
        {
            return ((options & flag) == flag);
        }

        private Regex ReadJson(JsonReader reader)
        {
            reader.Read();
            reader.Read();
            string pattern = (string) reader.Value;
            reader.Read();
            reader.Read();
            int num = Convert.ToInt32(reader.Value, CultureInfo.InvariantCulture);
            reader.Read();
            return new Regex(pattern, (RegexOptions) num);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return this.ReadJson(reader);
        }

        private void WriteJson(JsonWriter writer, Regex regex)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Pattern");
            writer.WriteValue(regex.ToString());
            writer.WritePropertyName("Options");
            writer.WriteValue(regex.Options);
            writer.WriteEndObject();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Regex regex = (Regex) value;
            this.WriteJson(writer, regex);
        }
    }
}

