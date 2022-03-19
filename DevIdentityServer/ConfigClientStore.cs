using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Options;

namespace IdentityServerDemo
{
    public class ConfigClientStore : IClientStore
    {
        private readonly IOptionsMonitor<SecuritySettings> _securitySettings;

        public ConfigClientStore(IOptionsMonitor<SecuritySettings> securitySettings)
        {
            _securitySettings = securitySettings;
        }

        public Task<Client> FindClientByIdAsync(string clientId)
        {
            return Task.FromResult(_securitySettings.CurrentValue.Clients.SingleOrDefault(x => x.ClientId == clientId));
        }
    }
}