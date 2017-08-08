namespace Newtonsoft.Json.Converters
{
    using Newtonsoft.Json;
    using System;
    using System.Data;

    public class DataSetConverter : JsonConverter
    {
        public override bool CanConvert(Type valueType)
        {
            return (valueType == typeof(DataSet));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DataSet set = new DataSet();
            DataTableConverter converter = new DataTableConverter();
            reader.Read();
            while (reader.TokenType == JsonToken.PropertyName)
            {
                DataTable table = (DataTable) converter.ReadJson(reader, typeof(DataTable), null, serializer);
                set.Tables.Add(table);
                reader.Read();
            }
            return set;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DataSet set = (DataSet) value;
            DataTableConverter converter = new DataTableConverter();
            writer.WriteStartObject();
            foreach (DataTable table in set.Tables)
            {
                writer.WritePropertyName(table.TableName);
                converter.WriteJson(writer, table, serializer);
            }
            writer.WriteEndObject();
        }
    }
}

