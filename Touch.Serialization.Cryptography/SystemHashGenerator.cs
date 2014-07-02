using System;
using System.Security.Cryptography;

namespace Touch.Serialization
{
    /// <summary>
    /// Default .NET hash generator.
    /// </summary>
    public sealed class SystemHashGenerator : IHashGenerator
    {
        #region .ctor
        public SystemHashGenerator()
        {
            _hashAlgorithm = null;
        }

        public SystemHashGenerator(string hashAlgorithm)
        {
            if (string.IsNullOrWhiteSpace(hashAlgorithm)) throw new ArgumentNullException("hashAlgorithm");
            _hashAlgorithm = hashAlgorithm;
        } 
        #endregion

        private readonly string _hashAlgorithm;

        public string Generate(byte[] data)
        {
            var algorithm = _hashAlgorithm != null ? HashAlgorithm.Create(_hashAlgorithm) : HashAlgorithm.Create();
            var inArray = algorithm.ComputeHash(data);

            return Convert.ToBase64String(inArray);
        }
    }
}
