using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPIServer.Helpers
{
    public class UriElements
    {
        public const string ArmResource = "subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/{providerName}/{service}/{resourceName}";
        public const string ListDetectors = "detectors";
        public const string Detector = ListDetectors + "/{detectorId}";
    }
}
