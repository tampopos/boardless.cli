using System;
using System.Security.Cryptography;
using System.Text;
using Tmpps.Infrastructure.Common.Cryptography.Interfaces;

namespace Tmpps.Infrastructure.Common.Cryptography
{
    public class MD5HashComputer : IHashComputer
    {
        private Lazy<MD5> md5 = new Lazy<MD5>(() => MD5.Create(), true);
        public string Compute(string target)
        {
            var targetData = Encoding.UTF8.GetBytes(target);
            var hash = this.md5.Value.ComputeHash(targetData);
            return BitConverter.ToString(hash);
        }

        #region IDisposable Support
        private bool disposedValue = false;
        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (this.md5.IsValueCreated)
                    {
                        this.md5.Value.Dispose();
                    }
                }
                disposedValue = true;
            }
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
        }
        #endregion
    }
}