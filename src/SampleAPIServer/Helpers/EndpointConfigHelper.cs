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
        internal static string GetDiagnosticEndpoint(string region)
        {
            switch (region)
            {
                default:
                    return "https://shgupgr1.cloudapp.net:1743";
            }
        }
    }
}
