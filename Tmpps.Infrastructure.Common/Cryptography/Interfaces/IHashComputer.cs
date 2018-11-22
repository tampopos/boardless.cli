using System;

namespace Tmpps.Infrastructure.Common.Cryptography.Interfaces
{
    public interface IHashComputer : IDisposable
    {
        string Compute(string target);
    }
}