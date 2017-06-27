using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using PS_project.Utils.DB;

namespace PS_project.Providers
{
    public class OAuthAppProvider : OAuthAuthorizationServerProvider
    {
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            await Task.Run(() =>
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                var email = context.UserName;
                var password = context.Password;

                if (DB_UserActions.LogUser(email, password))
                {
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("UserEmail", email));

                    context.Validated(identity);
                }
                else
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                }
            });            
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() =>
            {
                context.Validated();
            });                
        }
    }
}