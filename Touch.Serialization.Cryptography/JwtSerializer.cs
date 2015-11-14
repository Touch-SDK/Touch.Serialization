using System.IO;
using System.Text;
using Newtonsoft.Json;
using System;
using JWT;

namespace Touch.Serialization
{
    /// <summary>
    /// JWT serializer.
    /// </summary>
    sealed public class JwtSerializer : ISerializer
    {
        public JwtSerializer(string secret)
        {
            if (string.IsNullOrEmpty(secret)) throw new ArgumentNullException("secret");
            _secret = secret;

            JsonWebToken.JsonSerializer = new CustomJsonSerializer();
        }

        private readonly string _secret;

        #region ISerializer implementation
        public void Serialize<T>(T obj, Stream output)
            where T : class, new()
        {
            var data = Serialize(obj);
            var bytes = Encoding.UTF8.GetBytes(data);

            var length = bytes.Length;
            var i = 0;

            while (i < length)
            {
                var count = Math.Min(bytes.Length - i, 32);
                output.Write(bytes, i, count);
                i += 32;
            }

            output.Flush();
        }

        public T Deserialize<T>(Stream stream)
            where T : class, new()
        {
            var bytes = new byte[stream.Length];
            var i = 0;

            while (i < bytes.Length)
            {
                var count = Math.Min(bytes.Length - i, 32);
                i += stream.Read(bytes, i, count);
            }

            var data = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

            return Deserialize<T>(data);
        }

        public string Serialize<T>(T obj)
            where T : class, new()
        {
            return JsonWebToken.Encode(obj, _secret, JwtHashAlgorithm.HS256);
        }

        public T Deserialize<T>(string data)
            where T : class, new()
        {
            return JsonWebToken.DecodeToObject<T>(data, _secret, true);
        }
        #endregion

        #region Helper classes
        private class CustomJsonSerializer : IJsonSerializer
        {
            public string Serialize(object obj)
            {
                return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            }

            public T Deserialize<T>(string json)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
        } 
        #endregion
    }
}
