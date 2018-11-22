using System;

namespace Tmpps.Infrastructure.Common.Data.Interfaces
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}