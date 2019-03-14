using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SampleAPIServer.Helpers;
using SampleAPIServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SampleAPIServer.Services
{
    public class AppServiceDiagnosticsClientService : IAppServiceDiagnosticsClientService
    {
        private HttpClient httpClient;
        private ITokenService tokenService;

        public AppServiceDiagnosticsClientService(ITokenService tokenService)
        {
            this.tokenService = tokenService;

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;

            this.httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(60)
            };
        }

        public async Task<HttpResponseMessage> Execute(string resourceUrl, string region, string requestId = null)
        {
            AuthenticationResult authResult = await this.tokenService.GetAccessTokenAsync();

            string requestUri = $"{EndpointConfigHelper.GetDiagnosticEndpoint(region)}/api/invoke";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authResult.AccessTokenType, authResult.AccessToken);
            requestMessage.Headers.Add("x-ms-path-query", resourceUrl);
            requestMessage.Headers.Add("x-ms-verb", "POST");
            requestMessage.Headers.Add("x-ms-request-id", requestId ?? string.Empty);
            return await this.httpClient.SendAsync(requestMessage);
        }
    }
}
