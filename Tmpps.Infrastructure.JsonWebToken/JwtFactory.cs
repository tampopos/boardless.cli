using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tmpps.Infrastructure.Common.Claims.Interfaces;
using Tmpps.Infrastructure.Common.Foundation.Interfaces;
using Tmpps.Infrastructure.JsonWebToken.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Tmpps.Infrastructure.JsonWebToken
{
    public class JwtFactory : IJwtFactory
    {
        private ISystemClock systemClock;
        private IJwtConfig jwtConfig;
        private JwtSecurityTokenHandler securityTokenHandler;

        public JwtFactory(
            ISystemClock systemClock,
            IJwtConfig jwtConfig,
            JwtSecurityTokenHandler securityTokenHandler)
        {
            this.systemClock = systemClock;
            this.jwtConfig = jwtConfig;
            this.securityTokenHandler = securityTokenHandler;
        }

        public string Create(IJwtClaimInfo claimInfo)
        {
            var expires = this.systemClock.Now.AddDays(this.jwtConfig.JwtExpiresDate);

            var token = new JwtSecurityToken(
                issuer: this.jwtConfig.JwtIssuer,
                audience: this.jwtConfig.JwtAudience,
                claims: this.GetClaims(claimInfo),
                expires: expires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtConfig.JwtSecret)), SecurityAlgorithms.HmacSha256)
            );
            return this.securityTokenHandler.WriteToken(token);
        }

        private IEnumerable<Claim> GetClaims(IJwtClaimInfo claimInfo)
        {
            // JwtBearerAuthentication 用
            yield return new Claim(JwtRegisteredClaimNames.Jti, claimInfo.UserId);
            yield return new Claim(JwtRegisteredClaimNames.Sub, claimInfo.Name);
            yield return new Claim(JwtRegisteredClaimNames.Email, claimInfo.Email);
            // User.Identity プロパティ用
            yield return new Claim(ClaimTypes.Sid, claimInfo.UserId);
            yield return new Claim(ClaimTypes.Name, claimInfo.Name);
            yield return new Claim(ClaimTypes.Locality, claimInfo.CultureInfo.ToString());
            yield return new Claim(ClaimTypes.Email, claimInfo.Email);
        }

        public IJwtClaimInfo Create(ClaimsPrincipal claimsPrincipal)
        {
            return new JwtClaimInfo(claimsPrincipal);
        }
    }
}