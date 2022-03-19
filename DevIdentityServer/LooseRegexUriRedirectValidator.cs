using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace DevIdentityServer;

public class LooseRegexUriRedirectValidator : IRedirectUriValidator
{
    protected bool EmptyOrRegexMatch(IEnumerable<string> uris, string requestedUri)
    {
        if (uris.IsNullOrEmpty()) return true;

        return uris.Any(pattern => Regex.IsMatch(requestedUri, pattern));
    }
    public Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
    {
        return Task.FromResult(EmptyOrRegexMatch(client.RedirectUris, requestedUri));
    }

    public Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
    {
        return Task.FromResult(EmptyOrRegexMatch(client.RedirectUris, requestedUri));
    }
}