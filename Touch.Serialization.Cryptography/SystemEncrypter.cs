using System;
using System.Security.Cryptography;

namespace Touch.Serialization
{
    /// <summary>
    /// Default .NET encrypter.
    /// </summary>
    public sealed class SystemEncrypter : IEncrypter
    {
        #region .ctor
        public SystemEncrypter()
        {
            _hashAlgorithm = null;
        }

        public SystemEncrypter(string hashAlgorithm)
        {
            if (string.IsNullOrWhiteSpace(hashAlgorithm)) throw new ArgumentNullException("hashAlgorithm");
            _hashAlgorithm = hashAlgorithm;
        }
        #endregion

        private readonly string _hashAlgorithm;

        public byte[] Crypt(byte[] data, byte[] salt)
        {
            var dst = new byte[salt.Length + data.Length];

            Buffer.BlockCopy(salt, 0, dst, 0, salt.Length);
            Buffer.BlockCopy(data, 0, dst, salt.Length, data.Length);

            var algorithm = _hashAlgorithm != null ? HashAlgorithm.Create(_hashAlgorithm) : HashAlgorithm.Create();
            return algorithm.ComputeHash(dst);
        }

        public byte[] Crypt(byte[] data)
        {
            var algorithm = _hashAlgorithm != null ? HashAlgorithm.Create(_hashAlgorithm) : HashAlgorithm.Create();
            return algorithm.ComputeHash(data);
        }
    }
}
