using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using PS_project.Utils.DB;
using PS_project.Models;
using PS_project.Models.Exceptions;

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
                
                try
                {
                    DB_UserActions.LogUser(email, password);
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim("sub", context.UserName));
                    identity.AddClaim(new Claim("role", "user"));

                    ServiceUserModel user = DB_ServiceUserActions.GetServiceUser(email);
                    if (user == null)
                    {
                        context.Validated(identity);

                    }
                    else
                    {
                        var props = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            {  "user_name", user.name }
                        });
                        var ticket = new AuthenticationTicket(identity, props);
                        context.Validated(ticket);
                    }
                }
                catch(PS_Exception e)
                {
                    context.SetError("invalid_grant", e.GetError().detail);
                }
            });            
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }
            return Task.FromResult<object>(null);
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