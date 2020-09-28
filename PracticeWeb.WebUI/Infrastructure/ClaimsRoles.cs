using System.Collections.Generic;
using System.Security.Claims;

namespace PracticeWeb.WebUI.Infrastructure
{
    public class ClaimsRoles
    {
        public static IEnumerable<Claim> CreateRolesFromClaims(ClaimsIdentity user)
        {
            List<Claim> claims = new List<Claim>();
            if (user.HasClaim(x => x.Type == ClaimTypes.StateOrProvince
                        && x.Issuer == "RemoteClaims" && x.Value == "Taipei")
                    && user.HasClaim(x => x.Type == ClaimTypes.Role
                        && x.Value == "Employees"))
            {
                claims.Add(new Claim(ClaimTypes.Role, "TaipeiStaff"));
            }
            return claims;
        }

        public static IEnumerable<Claim> CreateRoleForGoogle()
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "GoogleAccount"));
            return claims;
        }
    }
}