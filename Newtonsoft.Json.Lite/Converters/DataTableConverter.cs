namespace Newtonsoft.Json.Converters
{
    using Newtonsoft.Json;
    using System;
    using System.Data;

    public class DataTableConverter : JsonConverter
    {
        public override bool CanConvert(Type valueType)
        {
            return (valueType == typeof(DataTable));
        }

        private static Type GetColumnDataType(JsonToken tokenType)
        {
            switch (tokenType)
            {
                case JsonToken.Integer:
                    return typeof(long);

                case JsonToken.Float:
                    return typeof(double);

                case JsonToken.String:
                case JsonToken.Null:
                case JsonToken.Undefined:
                    return typeof(string);

                case JsonToken.Boolean:
                    return typeof(bool);

                case JsonToken.Date:
                    return typeof(DateTime);
            }
            throw new ArgumentOutOfRangeException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DataTable table;
            if (reader.TokenType == JsonToken.PropertyName)
            {
                table = new DataTable((string) reader.Value);
                reader.Read();
            }
            else
            {
                table = new DataTable();
            }
            reader.Read();
            while (reader.TokenType == JsonToken.StartObject)
            {
                DataRow row = table.NewRow();
                reader.Read();
                while (reader.TokenType == JsonToken.PropertyName)
                {
                    string name = (string) reader.Value;
                    reader.Read();
                    if (!table.Columns.Contains(name))
                    {
                        Type columnDataType = GetColumnDataType(reader.TokenType);
                        table.Columns.Add(new DataColumn(name, columnDataType));
                    }
                    row[name] = reader.Value ?? DBNull.Value;
                    reader.Read();
                }
                row.EndEdit();
                table.Rows.Add(row);
                reader.Read();
            }
            return table;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DataTable table = (DataTable) value;
            writer.WriteStartArray();
            foreach (DataRow row in table.Rows)
            {
                writer.WriteStartObject();
                foreach (DataColumn column in row.Table.Columns)
                {
                    writer.WritePropertyName(column.ColumnName);
                    serializer.Serialize(writer, row[column]);
                }
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
        }
    }
}

