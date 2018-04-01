using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;

namespace SlotMachine.Utility
{
    public static class ExtensionUtility
    {
        public static string SerializeToXml<T>(this T value)
        {
            var xmlserializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();

            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, value);
                return stringWriter.ToString();
            }
        }

        public static T DeserializeFromXml<T>(this string value)
        {
            var returnValue = default(T);

            var serial = new XmlSerializer(typeof(T));
            var reader = new StringReader(value);
            var result = serial.Deserialize(reader);

            if (result is T)
                returnValue = (T)result;

            reader.Close();

            return returnValue;
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string ToJSON<T>(this T value)
        {
            return JsonConvert.SerializeObject(value, (Newtonsoft.Json.Formatting)Formatting.None);
        }

        public static string ToJSON<T>(this T value, bool escapeString)
        {
            var json = escapeString ? JsonConvert.SerializeObject(value, (Newtonsoft.Json.Formatting)Formatting.None, new JsonSerializerSettings { StringEscapeHandling = StringEscapeHandling.EscapeNonAscii }) : value.ToJSON();

            return json;
        }

        public static T FromJSON<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static string ToUTF8(this string value)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(value));
        }
    }
}
