using Microsoft.AspNetCore.Mvc;
using SampleAPIServer.Helpers;
using SampleAPIServer.Interfaces;
using System;
using System.Threading.Tasks;

namespace SampleAPIServer.Controllers
{
    [Produces("application/json")]
    [Route(UriElements.ArmResource)]
    public class DiagnosticsController : Controller
    {
        private IAppServiceDiagnosticsClientService diagnosticsClient;
        private CSMHelper csmHelper;

        public DiagnosticsController(IAppServiceDiagnosticsClientService diagnosticsClient)
        {
            this.csmHelper = new CSMHelper();
            this.diagnosticsClient = diagnosticsClient;
        }

        [HttpGet(UriElements.ListDetectors)]
        public async Task<IActionResult> ListDetectors(string subscriptionId, string resourceGroup, string providerName, string service, string resourceName)
        {
            /*
             * IMPORTANT : 
             * Verify your resource and fetch the region where this resource is deployed
             */

            // These are temp values. The real values would be fetched from your resource.
            string tempRegion = "blu";
            string tempLocation = "East US";

            var currentRoute = Request.Path.Value + Request.QueryString.ToUriComponent();
            var response = await this.diagnosticsClient.Execute(currentRoute, tempRegion, Request.Headers);

            response.EnsureSuccessStatusCode();

            object content = null;
            if (response.IsSuccessStatusCode)
            {
                content = await csmHelper.CreateCollectionEnvelopedResponse(response, Request.Path.Value, tempLocation, $"{providerName}/{service}/detectors", csmHelper.GetNameFromJToken);
            }
            else
            {
                // You might want to wrap the error also in an envelope.
                content = await response.Content.ReadAsStringAsync();
            }

            return StatusCode((int)response.StatusCode, content);
        }

        [HttpGet(UriElements.Detector)]
        public async Task<IActionResult> GetDetector(string subscriptionId, string resourceGroup, string providerName, string service, string resourceName, string detectorId)
        {
            /*
             * IMPORTANT : 
             * Verify your resource and fetch the region where this resource is deployed
             */

            string tempRegion = "test";
            string tempLocation = "East US";
            string requestId = Guid.NewGuid().ToString();

            var currentRoute = Request.Path.Value + Request.QueryString.ToUriComponent();
            var response =  await this.diagnosticsClient.Execute(currentRoute, tempRegion, Request.Headers);
            response.EnsureSuccessStatusCode();
            object content = null;
            if (response.IsSuccessStatusCode)
            {
                content = await csmHelper.CreateEnvelopedResponse(response, Request.Path.Value, tempLocation, $"{providerName}/{service}/detectors", detectorId);
            }
            else
            {
                // You might want to wrap the error also in an envelope.
                content = await response.Content.ReadAsStringAsync();
            }

            return StatusCode((int)response.StatusCode, content);
        }
    }
}