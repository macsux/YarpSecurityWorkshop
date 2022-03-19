using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Options;

namespace IdentityServerDemo
{
    public class ConfigResourceStore : IResourceStore
    {
        private readonly IOptionsMonitor<SecuritySettings> _securitySettings;

        public ConfigResourceStore(IOptionsMonitor<SecuritySettings> securitySettings)
        {
            _securitySettings = securitySettings;
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var identity = from i in _securitySettings.CurrentValue.IdentityResources.Union(StandardIdentityResources)
                
                where scopeNames.Contains(i.Name)
                select i;

            return Task.FromResult(identity);        }

        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var query =
                from x in _securitySettings.CurrentValue.ApiScopes
                where scopeNames.Contains(x.Name)
                select x;
            
            return Task.FromResult(query);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

            var api = from a in _securitySettings.CurrentValue.ApiResources
                let scopes = (from s in a.Scopes where scopeNames.Contains(s) select s)
                where scopes.Any()
                select a;

            return Task.FromResult(api);
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            if (apiResourceNames == null) throw new ArgumentNullException(nameof(apiResourceNames));

            var query =
                from x in _securitySettings.CurrentValue.ApiResources
                where apiResourceNames.Contains(x.Name)
                select x;

            return Task.FromResult(query);
        }

        /// <summary>
        /// Gets all resources.
        /// </summary>
        /// <returns></returns>
        public Task<Resources> GetAllResourcesAsync()
        {
            var security = _securitySettings.CurrentValue;
            var result = new Resources(security.IdentityResources.Union(StandardIdentityResources), security.ApiResources, security.ApiScopes);
            return Task.FromResult(result);
        }

        public List<IdentityResource> StandardIdentityResources =>
            new()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResources.Profile()
            };

    }
}