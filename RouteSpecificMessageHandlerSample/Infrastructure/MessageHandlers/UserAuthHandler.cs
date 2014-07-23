using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.IdentityModel.Tokens;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RouteSpecificMessageHandlerSample.Infrastructure.MessageHandlers
{
    public class UserAuthHandler : DelegatingHandler
    {
        private const string SCHEME = "Basic";

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                // Authentication Process
                var headers = request.Headers;
                if (headers.Authorization != null && SCHEME.Equals(headers.Authorization.Scheme))
                {
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string credentials = encoding.GetString(Convert.FromBase64String(headers.Authorization.Parameter));
                    string[] credentialParts = credentials.Split(':');
                    string userID = credentialParts[0];
                    string pass = credentialParts[1];

                    if (userID == pass)
                    {
                        var claims = new List<Claim>() { new Claim(ClaimTypes.Name, userID), new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password) };
                        var principal = new ClaimsPrincipal(new[] { new ClaimsIdentity(claims, SCHEME) });
                        Thread.CurrentPrincipal = principal;
                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.User = principal;
                        }
                    }
                }
                else
                {
                    HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Need Authentication Credentials!!!");
                    reply.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(SCHEME));
                    return reply;
                }

                var response = await base.SendAsync(request, cancellationToken);

                //Cleanup

                return response;
            }
            catch (Exception)
            {
                HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Credentials!!!");
                return reply;
            }
        }
    }
}