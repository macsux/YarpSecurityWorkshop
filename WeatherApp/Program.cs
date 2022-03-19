using System;
using System.Net.Http;
using Darnton.Blazor.DeviceInterop.Geolocation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WeatherApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<SignOutSessionStateManager>();
builder.Services.AddScoped<AuthenticationStateProvider, BffAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

builder.Services.AddSingleton<Api>();
builder.Services.AddTransient<CookieHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("API"));
builder.Services.AddHttpClient("API", (sp, client) => client.BaseAddress = new Uri($"{sp.GetRequiredService<Api>().BasePath}")).AddHttpMessageHandler<CookieHandler>();
builder.Services.AddSingleton<GeolocationService>();

await builder.Build().RunAsync();