using System;
using System.Text;
using System.Text.Json;

namespace VoiceAssistant.Server
{
    public static class JSON
    {
        private readonly static JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public static byte[] Serialize(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var json = JsonSerializer.Serialize(obj, _options);
            return Encoding.UTF8.GetBytes(json);
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));

            return JsonSerializer.Deserialize<T>(bytes, _options);
        }

        public static string ReadRootProperty(byte[] bytes, string propertyName)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException(nameof(bytes));

            return JsonDocument.Parse(bytes).RootElement.GetProperty(propertyName).GetString();
        }
    }
}
