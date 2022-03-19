using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServerDemo
{
    public class SecuritySettings
    {
        public List<Client> Clients { get; set; } = new();
        public List<ApiResource> ApiResources { get; set; } = new();
        public List<ApiScope> ApiScopes { get; set; } = new();
        public List<IdentityResource> IdentityResources { get; set; } = new();
        public List<TestUser> Users { get; set; } = new();
    }
}