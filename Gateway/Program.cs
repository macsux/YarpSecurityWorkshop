using System.Security.Claims;
using Common;
using Gateway;
using IdentityModel;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using NMica.SecurityProxy.Middleware.Transforms;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddYamlFile("appsettings.yaml");
builder.Services.AddControllers();
builder.Services.AddSingleton<IClaimsTransformation, ClaimsTransformer>();
builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddOpenIdConnect(config =>
    {
        config.SignInScheme  = CookieAuthenticationDefaults.AuthenticationScheme;
        config.Authority = "https://localhost:5001";
        config.ClientId = "gui";
        config.ClientSecret = "password";
        config.ResponseType = "code";
        config.Scope.Add("openid");
        config.Scope.Add("profile");
        config.ClaimActions.Add(new DeleteClaimAction("s_hash"));
        config.ClaimActions.Add(new DeleteClaimAction("sid"));
        config.ClaimActions.Add(new DeleteClaimAction("auth_time"));
        config.ClaimActions.Add(new DeleteClaimAction("amr"));
        config.Events.OnTokenValidated += context =>
        {
            var logger = context.Request.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("TokenValidation");
            logger.LogInformation("Upstream token: {Token}", context.SecurityToken.RawData);
            return Task.CompletedTask;
        };
    })
    .AddCookie(x => x.LoginPath = "/login");
builder.Services.AddAuthorization(c => c
    .AddPolicy("authenticated", p => p.RequireAuthenticatedUser()));
builder.Services.AddSingleton<JwtPrincipalAppender>();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(context =>
    {
        context.RequestTransforms.Add(context.Services.GetRequiredService<JwtPrincipalAppender>());
        context.RequestTransforms.Add(new RequestHeaderRemoveTransform("Cookie"));

    });


// configure identity server helper methods to allow issuing JWT tokens from ClaimPrincipal and publish signing key via OIDC discovery endpoint
builder.Services.AddSingleton<ISigningCredentialStore, SigningKeyCredentialsProvider>();
builder.Services.AddSingleton<IValidationKeysStore, SigningKeyCredentialsProvider>();
builder.Services.AddIdentityServerBuilder()
    .AddRequiredPlatformServices()
    .AddCoreServices()
    .AddDefaultEndpoints()
    .AddPluggableServices()
    .AddValidators()
    .AddResponseGenerators()
    .AddDefaultSecretParsers()
    .AddDefaultSecretValidators()
    .AddInMemoryPersistedGrants()
    .AddInMemoryClients(Enumerable.Empty<Client>())
    .AddInMemoryIdentityResources(Enumerable.Empty<IdentityResource>())
    .AddInMemoryCaching()
    .AddInMemoryApiResources(Enumerable.Empty<ApiResource>())
    .AddInMemoryApiScopes(Enumerable.Empty<ApiScope>());
            
builder.Services.Configure<IdentityServerOptions>(opt =>
{
    opt.Endpoints = new EndpointsOptions()
    {
        EnableAuthorizeEndpoint = false,
        EnableIntrospectionEndpoint = false,
        EnableTokenEndpoint = false,
        EnableCheckSessionEndpoint = false,
        EnableDeviceAuthorizationEndpoint = false,
        EnableEndSessionEndpoint = false,
        EnableJwtRequestUri = false,
        EnableTokenRevocationEndpoint = false,
        EnableUserInfoEndpoint = false
    };
    opt.Discovery = new DiscoveryOptions()
    {
        ShowClaims = false,
        ShowApiScopes = false,
        ShowGrantTypes = false,
        ShowIdentityScopes = false,
        ShowResponseModes = false,
        ShowResponseTypes = false,
        ShowExtensionGrantTypes = false,
        ShowTokenEndpointAuthenticationMethods = false
    };
});

var app = builder.Build();

app.UseCors(p => p
    .AllowAnyHeader()
    .WithOrigins("https://localhost:8080")
    .AllowAnyMethod()
    .AllowCredentials());

app.UseIdentityServer();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapReverseProxy();
app.Run();