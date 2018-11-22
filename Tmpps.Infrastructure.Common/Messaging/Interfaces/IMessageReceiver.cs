using System.Threading.Tasks;

namespace Tmpps.Infrastructure.Common.Messaging.Interfaces
{
    public interface IMessageReceiver
    {
        Task<int> ExecuteAsync();
    }
    public interface IMessageReceiver<T> : IMessageReceiver where T : class
    { }
}