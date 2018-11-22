using System.Security.Claims;
using Tmpps.Infrastructure.Common.Claims.Interfaces;

namespace Tmpps.Infrastructure.Common.Claims.Interfaces
{
    public interface IJwtFactory
    {
        string Create(IJwtClaimInfo claimInfo);
        IJwtClaimInfo Create(ClaimsPrincipal claimsPrincipal);
    }
}