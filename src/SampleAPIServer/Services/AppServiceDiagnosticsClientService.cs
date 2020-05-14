using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SampleAPIServer.Helpers;
using SampleAPIServer.Interfaces;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SampleAPIServer.Services
{
    public class AppServiceDiagnosticsClientService : IAppServiceDiagnosticsClientService
    {
        private HttpClient httpClient;
        private ITokenService tokenService;
        private X509Certificate2 clientCertificate;
        private string authenticationMode;
        private string apiEndpoint;
        
        public AppServiceDiagnosticsClientService(ITokenService tokenService, IConfiguration config)
        {
            this.tokenService = tokenService;
            authenticationMode = config["DiagnosticServer:AuthenticationMode"].ToString();
            apiEndpoint = config["DiagnosticServer:ApiEndpoint"].ToString();
            InitializeClientCertificate(config["ClientCertficateSettings:Thumbprint"].ToString());
            
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback +=
                (sender, cert, chain, sslPolicyErrors) => true;

            this.httpClient = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(60)
            };
        }
        
        public async Task<HttpResponseMessage> Execute(string resourceUrl, string region, IHeaderDictionary requestHeaders)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiEndpoint);
            if(authenticationMode.Equals("AAD", StringComparison.OrdinalIgnoreCase))
            {
                AuthenticationResult authResult = await this.tokenService.GetAccessTokenAsync();
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authResult.AccessTokenType, authResult.AccessToken);
            }
            else if(authenticationMode.Equals("ClientCertificate", StringComparison.OrdinalIgnoreCase))
            {
                requestMessage.Headers.Add("x-ms-diagcert", Convert.ToBase64String(clientCertificate.Export(X509ContentType.Cert)));
            }
            else
            {
                throw new Exception($"Authentication Mode : {authenticationMode} not supported. Make sure to update appsettings to either AAD or ClientCertificate option"); 
            }
            
            requestMessage.Headers.Add("x-ms-path-query", resourceUrl);
            requestMessage.Headers.Add("x-ms-verb", "POST");
            AddAdditionalRequestHeaders(requestHeaders, ref requestMessage);
            return await this.httpClient.SendAsync(requestMessage);
        }

        private void AddAdditionalRequestHeaders(IHeaderDictionary incomingRequestHeaders, ref HttpRequestMessage request)
        {
            foreach (var header in incomingRequestHeaders)
            {
                if (header.Key.StartsWith("x-ms", StringComparison.OrdinalIgnoreCase) && !request.Headers.Contains(header.Key))
                {
                    request.Headers.Add(header.Key, header.Value.FirstOrDefault());
                }
            }
        }

        private void InitializeClientCertificate(string thumbprint)
        {
            if(!authenticationMode.Equals("ClientCertificate", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint.ToUpper(), true);

            if (certificates == null || certificates.Count == 0)
            {
                throw new InvalidOperationException(String.Format("Cannot find Client Certificate with thumbprint {0}", thumbprint));
            }

            clientCertificate = certificates[0];
            store.Close();
        }

    }
}
