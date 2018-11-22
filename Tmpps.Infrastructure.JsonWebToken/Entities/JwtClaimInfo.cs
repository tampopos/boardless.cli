using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Tmpps.Infrastructure.Common.Claims.Interfaces;

namespace Tmpps.Infrastructure.JsonWebToken.Entities
{
    public class JwtClaimInfo : IJwtClaimInfo
    {
        public JwtClaimInfo()
        {

        }
        public JwtClaimInfo(ClaimsPrincipal claimsPrincipal)
        {
            foreach (var claim in claimsPrincipal.Claims)
            {
                switch (claim.Type)
                {
                    case ClaimTypes.Sid:
                        this.UserId = claim.Value;
                        break;
                    case ClaimTypes.Name:
                        this.Name = claim.Value;
                        break;
                    case ClaimTypes.Email:
                        this.Email = claim.Value;
                        break;
                    case ClaimTypes.Locality:
                        this.CultureInfo = CultureInfo.GetCultureInfo(claim.Value);
                        break;
                }
            }
        }

        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public CultureInfo CultureInfo { get; set; }
    }
}