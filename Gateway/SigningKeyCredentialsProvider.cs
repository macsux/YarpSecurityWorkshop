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
    public SigningKeyCredentialsProvider()
    {
        var rsa = RSA.Create();
        var pem = @"
    -----BEGIN RSA PRIVATE KEY-----
    MIIEpAIBAAKCAQEAt0y4Hvx3TZHwct7T5NV6oG0EqNf9W311rVeRCXsDpFgwrDn/MoB
    xShrqw588SeM/ETiGZs4zMJ3nRL6pBk58xbLk8rc1LTCVQj5X4PbnEIioARmUw+9/ah
    uCt9wXrb/engsLz7weUyX15G9VxrnUFagy6O3F2NYBbENpijNO9pvF08xPgvVFGabIu
    sQGU8jV1eYPrPGt0+e59I10XxpSpt+pf+A6BV8h9HLTriZuryNeRaWSbdT6HdsPXlhz
    L7AgpcUNdbbkcqVKn9r8ycAoVscbuncbNtj48S44uVcLelgXKT7eSFIdJdf1LPIyWi9
    BwJylNW2JD8dt+IDn8QHOEQIDAQABAoIBAHyut/OD4hcmtTs97T6UI/SqE2hSe2lXXs
    uJbAPZ5HFO99S/IqMUkXtJ8fUdBKJx7H1nSKz3iASC9ERjuI+spdzIwkmDa62QhYbo5
    1gEOsqZWkZdQz9AUxg3HGS6VnT+tYwlkWQ93xbKFIfNX7hswSH2JqMg0dqcP52IAWtb
    WVQPHK0rgjDmETycL3qwX9AxSVKZkx4q7jiO/XMY6cZsXt0/NDMo0P/iAixEzcijNX2
    3fY2P7cObqunYobGwhmwsWtFGuAmFYqxPnDGwhv386Un1AaFwdC0v8quxDk3uSevukh
    /6nKsQuC973h1D28u452FMhOKWNU/9VPDjetYszi0CgYEA4vsAn53W38KDRodKYjF2H
    uOCUXoh5EU1RSRQjFfNWGLGugVuHWHS/ZEgZQ49rCHE4HFa/djBSuYLvYEXxODTHS0A
    djDS2hDGWVmOxQpkdnKA9ngG/9Uv8GJdWFzpI0H7CH61UrTXnPM7avHK/Bjlk8gFNlX
    e1v5VwuR+PuEwrvcCgYEAzrwO/QtwDhg+QF5Tz8lCpNLkWD1mmP67+j4ZCHGSmur3RQ
    qKEbNuc9dYja8UzKGFIcw+P8cvORhgnE3Of/YOGfqrJMEW8SUv4bjNIrKmfDABgx635
    0CJQqb4jtJeI4RjHBPnfxT+xlNJYit52oH39iqBxVnG5LSTlaU9GsR8wTcCgYEAttGY
    tPd6xIt1FO+PV/uiukphAqMufR/JQkF3pzJpMNGOYvJQuNW8DYRA5WRNBEHGw0hKE7/
    sIBmeRyqdLHQxVoSSpJ+6lO2B9SGPPuGZ/VVIzjvq11Cs5h80NCHRnhZczYDRJyaFq+
    K1bvQFnHupHhizKgqMC/qatPk5Pgg/IA0CgYAcuYCTpE0ziCbOJs0aZ+p8oFjd8doZk
    /tmb85mn/Ew2Uj2LNq2Tuof6mIBfbw0GpU29vwHPJPRKRPzY9Q3b2bSMUQqXTHk27fl
    cxn9ojkDtF/hahk/ZnYr7qtGnPA7mx9yPUnDHJWx1MyzeTr7I2fiqlFRrIG4MWb4Ofd
    disOSzQKBgQC2rWlJ1aAHBy/8QWxEs6ieg8YmCGKqG5q8jQ0y6ES0bG2u5PMBCaPaxn
    T4vCYdSNHEvFqJYGfGHUzT3xCTSWv42BKLrQsSThAvkbavQTlJLUjcz64wGj1xnFeQc
    B9PrV/s7WfGoEnMO++TpC3DEc6cvRcTmjLd2CeeYpiwi382MA==
    -----END RSA PRIVATE KEY-----";
        rsa.ImportFromPem(pem);
        _signingKey = new RsaSecurityKey(rsa);
    }

    public Task<SigningCredentials> GetSigningCredentialsAsync()
    {
        var rsa = RSA.Create();
        var key = new RsaSecurityKey(rsa);
        return Task.FromResult(new SigningCredentials(_signingKey, SecurityAlgorithms.RsaSha256));
    }

    public async Task<IEnumerable<SecurityKeyInfo>> GetValidationKeysAsync()
    {
        var credentials = await GetSigningCredentialsAsync();
        return new[] {new SecurityKeyInfo {Key = credentials.Key, SigningAlgorithm = credentials.Algorithm}};
    }
}