using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SampleAPIServer.Interfaces
{
    public interface IAppServiceDiagnosticsClientService
    {
        Task<HttpResponseMessage> Execute(string resourceUrl, string region, IHeaderDictionary requestHeaders);
    }
}
