using System.Threading.Tasks;

namespace Tmpps.Infrastructure.Common.Messaging.Interfaces
{
    public interface IMessageSubscriber
    {
        Task SubscribeAsync();
    }
}