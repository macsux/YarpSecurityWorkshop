using System;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace IdentityServerDemo
{
    public class ConfigUserResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IOptionsMonitor<SecuritySettings> _securitySettings;
        private readonly ISystemClock _clock;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServer4.Test.TestUserResourceOwnerPasswordValidator"/> class.
        /// </summary>
        /// <param name="users">The users.</param>
        /// <param name="clock">The clock.</param>
        public ConfigUserResourceOwnerPasswordValidator(IOptionsMonitor<SecuritySettings> securitySettings, ISystemClock clock)
        {
            _securitySettings = securitySettings;
            _clock = clock;
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (new TestUserStore(_securitySettings.CurrentValue.Users).ValidateCredentials(context.UserName, context.Password))
            {
                var user = new TestUserStore(_securitySettings.CurrentValue.Users).FindByUsername(context.UserName);
                context.Result = new GrantValidationResult(
                    user.SubjectId ?? throw new ArgumentException("Subject ID not set", nameof(user.SubjectId)),
                    OidcConstants.AuthenticationMethods.Password, _clock.UtcNow.UtcDateTime,
                    user.Claims);
            }

            return Task.CompletedTask;
        }
    }
}