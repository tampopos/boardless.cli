using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;

namespace Tmpps.Infrastructure.Common.Claims.Interfaces
{
    public interface IJwtClaimInfo
    {
        string UserId { get; set; }
        string Name { get; set; }
        string Email { get; set; }
        CultureInfo CultureInfo { get; set; }
    }
}