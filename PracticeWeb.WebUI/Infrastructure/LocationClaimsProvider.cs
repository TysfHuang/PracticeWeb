using System.Collections.Generic;
using System.Security.Claims;

namespace PracticeWeb.WebUI.Infrastructure
{
    public static class LocationClaimsProvider
    {
        public static IEnumerable<Claim> GetClaims(ClaimsIdentity user)
        {
            List<Claim> claims = new List<Claim>();
            if (user.Name.ToLower() == "joe")
            {
                claims.Add(CreateClaim(ClaimTypes.PostalCode, "Taipei 100"));
                claims.Add(CreateClaim(ClaimTypes.StateOrProvince, "Taipei"));
            }
            else
            {
                claims.Add(CreateClaim(ClaimTypes.PostalCode, "NewTaipei 207"));
                claims.Add(CreateClaim(ClaimTypes.StateOrProvince, "NewTaipei"));
            }
            return claims;
        }

        private static Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String, "RemoteClaims");
        }
    }
}