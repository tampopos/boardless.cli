using System;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;

namespace Tmpps.Infrastructure.Common.Foundation
{
    /// <inheritDoc/>
    public class GuidFactory : IGuidFactory
    {
        /// <inheritDoc/>
        public Guid CreateNew()
        {
            return Guid.NewGuid();
        }
    }
}