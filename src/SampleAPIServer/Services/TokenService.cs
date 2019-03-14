using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SampleAPIServer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPIServer.Services
{
    public class TokenService : ITokenService
    {
        private string authority;
        private string appId;
        private string appKey;
        private string resource;
        private AuthenticationContext authContext;

        public TokenService(IConfiguration configuration)
        {
            LoadConfigurations(configuration);
            authContext = new AuthenticationContext(authority, true);
        }

        public async Task<AuthenticationResult> GetAccessTokenAsync()
        {
            AuthenticationResult authResult = null;

            try
            {
                authResult = await authContext.AcquireTokenSilentAsync(resource, appId);
            }
            catch (AdalException adalException)
            {
                if (adalException.ErrorCode == AdalError.FailedToAcquireTokenSilently
                    || adalException.ErrorCode == AdalError.InteractionRequired)
                {
                    authResult = await authContext.AcquireTokenAsync(resource, new ClientCredential(appId, appKey));
                }
                else
                {
                    throw;
                }
            }

            return authResult;
        }

        private void LoadConfigurations(IConfiguration config)
        {
            authority = config["AADSettings:Authority"].ToString();
            appId = config["AADSettings:AppId"].ToString();
            appKey = config["AADSettings:AppKey"].ToString();
            resource = config["AADSettings:Resource"].ToString();
        }
    }
}
