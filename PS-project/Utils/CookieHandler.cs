using System;
using System.Net.Http.Headers;

namespace PS_project.Utils
{
    public class CookieHandler
    {
        public static CookieHeaderValue CreateAuthCookie(string provider_email, string req_uri)
        {
            CookieHeaderValue cookie = new CookieHeaderValue("provider_token", provider_email);
            cookie.Expires = DateTimeOffset.Now.AddDays(1);
            cookie.Domain = req_uri;
            cookie.Path = "/";
            return cookie;
        }

        public static bool ValidateToken(CookieHeaderValue cookie)
        {
            return true;
        }
    }
}