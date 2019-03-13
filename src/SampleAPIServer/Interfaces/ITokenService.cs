using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPIServer.Interfaces
{
    public interface ITokenService
    {
        Task<AuthenticationResult> GetAccessTokenAsync();
    }
}
