using System.Security.Cryptography;
using System.Text;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gateway;

public class SigningKeyCredentialsProvider : ISigningCredentialStore, IValidationKeysStore
{
    private SecurityKey _signingKey;
    public SigningKeyCredentialsProvider(IConfiguration config)
    {
        var rsa = RSA.Create();
        var pem = config.GetValue<string>("SigningKey");
        rsa.ImportFromPem(pem);
        _signingKey = new RsaSecurityKey(rsa);
    }

    public Task<SigningCredentials> GetSigningCredentialsAsync()
    {
        return Task.FromResult(new SigningCredentials(_signingKey, SecurityAlgorithms.RsaSha256));
    }

    public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
    {
        var credentials = await GetSigningCredentialsAsync();
        return new[] {new SecurityKeyInfo {Key = credentials.Key, SigningAlgorithm = credentials.Algorithm}};
    }
}