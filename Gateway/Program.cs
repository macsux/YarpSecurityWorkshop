using Gateway;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using NMica.SecurityProxy.Middleware.Transforms;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
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
        // config.UsePkce = true;
        config.SaveTokens = true;
        // config.Scope.Add("app1");
        config.Scope.Add("openid");
        config.Scope.Add("profile");
        
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
    });

var services = builder.Services;
services.AddSingleton<ISigningCredentialStore, SigningKeyCredentialsProvider>();
services.AddSingleton<IValidationKeysStore, SigningKeyCredentialsProvider>();
services.AddIdentityServerBuilder()
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
            
services.Configure<IdentityServerOptions>(opt =>
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
app.UseCors(p => p.AllowAnyHeader().WithOrigins("https://localhost:8080").AllowAnyMethod().AllowCredentials());

app.UseIdentityServer();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapReverseProxy();
app.Run();