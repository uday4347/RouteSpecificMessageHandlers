using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RouteSpecificMessageHandlerSample.Infrastructure.MessageHandlers
{
    public class ApiAuthHandler : DelegatingHandler
    {
       

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                // Authentication Process
                var headers = request.Headers;
                
                if (true)
                {
                    
                }
                else
                {
                    HttpResponseMessage reply = request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Need Authentication Credentials!!!");                    
                    return reply;
                }

                var response = await base.SendAsync(request, cancellationToken);
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