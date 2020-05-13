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
        private string resourceId;
        private AuthenticationContext authContext;

        public TokenService(IConfiguration configuration)
        {
            LoadConfigurations(configuration);
            authContext = new AuthenticationContext(authority, true, TokenCache.DefaultShared);
        }

        public async Task<AuthenticationResult> GetAccessTokenAsync()
        {
            AuthenticationResult authResult = null;

            try
            {
                authResult = await authContext.AcquireTokenSilentAsync(resourceId, appId);
            }
            catch (AdalException adalException)
            {
                if (adalException.ErrorCode == AdalError.FailedToAcquireTokenSilently
                    || adalException.ErrorCode == AdalError.InteractionRequired)
                {
                    authResult = await authContext.AcquireTokenAsync(resourceId, new ClientCredential(appId, appKey));
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
            resourceId = config["AADSettings:ResourceId"].ToString();
        }
    }
}
