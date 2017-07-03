using System.Security.Claims;
using PS_project.Models.Exceptions;

namespace PS_project.Utils
{
    public class ClaimsHandler
    {
        private const string CLAIM_SUB = "sub";

        public static string GetUserNameFromClaim(ClaimsPrincipal principal)
        {
            string username = null;

            foreach (Claim claim in principal.Claims)
            {
                if (claim.Type.Equals(CLAIM_SUB))
                {
                    username = claim.Value;
                    break;
                }
            }

            if (username == null)
            {
                throw new MissingClaimException("The authorization token is missing the UserName claim.");
            }
            return username;
        }
    }
}