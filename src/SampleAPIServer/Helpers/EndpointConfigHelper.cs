using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPIServer.Helpers
{
    /// <summary>
    /// Note:- This is a temporary workaround until we figure out a better way using Azure Front Door to acheive the same functionality.
    /// For now, In your Code, you can either keep this region to endpoint mapping in code like below, or save it in some config/DB and fetch it from there.
    /// </summary>
    internal static class EndpointConfigHelper
    {
        internal static string GetDiagnosticEndpoint(string regionName)
        {
            if (string.IsNullOrWhiteSpace(regionName))
            {
                throw new ArgumentNullException("regionName");
            }

            string apiEndpoint = string.Empty;
            switch (regionName.ToLower())
            {
                case "ma1": // SouthIndia
                case "sha": // China
                case "bm1": // West India
                case "se1": // Southeast Asia
                case "pn1": // Central India
                    apiEndpoint = ProdDiagnosticEnabledEndpoints.SouthIndia;
                    break;
                case "sg1": // Southeast Asia
                case "os1": // Japan West
                case "ty1": // Japan East
                case "kw1": // Japan East
                case "ps1": // Korea South
                case "sy3": // Australia East
                case "ml1": // Australia SouthEast
                case "hk1": // Hong Kong
                    apiEndpoint = ProdDiagnosticEnabledEndpoints.SouthEastAsia;
                    break;
                case "db3": // North Europe
                case "cw1": // UK West
                case "fr1": // Germany Central
                case "ln1": // UK South
                case "am2": // West Europe
                case "par": // Paris
                    apiEndpoint = ProdDiagnosticEnabledEndpoints.NorthEurope;
                    break;
                case "blu": // East US
                case "bay": // West US
                case "bn1": // East US 2
                case "ch1": // North Central US 
                case "cq1": // Brazil South
                case "cy4": // West Central US
                case "dm1": // Central US
                case "dm3": // USGov Iowa
                case "mwh": // West US 2
                case "sn1": // South Central US
                default:
                    apiEndpoint = ProdDiagnosticEnabledEndpoints.EastUs;
                    break;
            }

            return $"https://{apiEndpoint}.cloudapp.net:1743";
        }
    }

    internal class ProdDiagnosticEnabledEndpoints
    {
        internal const string SouthIndia = "gr-prod-ma1";
        internal const string EastUs = "gr-prod-blu";
        internal const string NorthEurope = "gr-prod-db3";
        internal const string SouthEastAsia = "gr-prod-sg1";
    }
}
