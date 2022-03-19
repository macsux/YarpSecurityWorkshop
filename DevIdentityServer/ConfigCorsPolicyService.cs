using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdentityServerDemo
{
    public class ConfigCorsPolicyService : ICorsPolicyService
    {
        /// <summary>
        /// Logger
        /// </summary>
        protected readonly ILogger Logger;

        private readonly IOptionsMonitor<SecuritySettings> _security;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServer4.Services.InMemoryCorsPolicyService"/> class.
        /// </summary>
        /// <param name="logger">The logger</param>
        /// <param name="clients">The clients.</param>
        public ConfigCorsPolicyService(ILogger<ConfigCorsPolicyService> logger, IOptionsMonitor<SecuritySettings> security)
        {
            Logger = logger;
            _security = security;
        }

        /// <summary>
        /// Determines whether origin is allowed.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <returns></returns>
        public virtual Task<bool> IsOriginAllowedAsync(string origin)
        {
            var query = _security.CurrentValue.Clients.SelectMany(x => x.AllowedCorsOrigins);

            var result = query.Contains(origin, StringComparer.OrdinalIgnoreCase);

            if (result)
            {
                Logger.LogDebug("Client list checked and origin: {0} is allowed", origin);
            }
            else
            {
                Logger.LogDebug("Client list checked and origin: {0} is not allowed", origin);
            }

            return Task.FromResult(result);
        }
    }
}