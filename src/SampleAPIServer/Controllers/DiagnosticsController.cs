using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleAPIServer.Helpers;
using SampleAPIServer.Interfaces;

namespace SampleAPIServer.Controllers
{
    [Produces("application/json")]
    [Route(UriElements.ArmResource)]
    public class DiagnosticsController : Controller
    {
        private IAppServiceDiagnosticsClientService diagnosticsClient;

        public DiagnosticsController(IAppServiceDiagnosticsClientService diagnosticsClient)
        {
            this.diagnosticsClient = diagnosticsClient;
        }

        [HttpGet(UriElements.ListDetectors)]
        public async Task<HttpResponseMessage> ListDetectors(string subscriptionId, string resourceGroup, string providerName, string service, string resourceName)
        {
            /*
             * IMPORTANT : 
             * Verify your resource and fetch the region where this resource is deployed
             */

            string tempRegion = "blu";

            var currentRoute = Request.Path.Value + Request.QueryString.ToUriComponent();
            return await this.diagnosticsClient.Execute(currentRoute, tempRegion);
        }

        [HttpGet(UriElements.Detector)]
        public async Task<HttpResponseMessage> GetDetector(string subscriptionId, string resourceGroup, string providerName, string service, string resourceName, string detectorId)
        {
            /*
             * IMPORTANT : 
             * Verify your resource and fetch the region where this resource is deployed
             */

            string tempRegion = "blu";

            var currentRoute = Request.Path.Value + Request.QueryString.ToUriComponent();
            return await this.diagnosticsClient.Execute(currentRoute, tempRegion);
        }
    }
}