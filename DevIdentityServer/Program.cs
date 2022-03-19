// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using DevIdentityServer;
using IdentityServer4.Validation;
using IdentityServerDemo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
    .CreateLogger();

try
{
    Log.Information("Starting host...");
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.AddSerilog();
    builder.Configuration.AddYamlFile("appsettings.yml", false);

    var services = builder.Services;
    
    services.AddControllersWithViews().AddRazorRuntimeCompilation();
    services.AddIdentityServer(options =>
        {
            options.Cors.CorsPolicyName = "AllowAllOrigins";
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;
        })
        .AddResourceStore<ConfigResourceStore>()
        .AddClientStore<ConfigClientStore>()
        .AddSecretValidator<PlainTextSharedSecretValidator>()
        .AddProfileService<ConfigUserProfileService>()
        .AddRedirectUriValidator<LooseRegexUriRedirectValidator>()
        .AddResourceOwnerValidator<ConfigUserResourceOwnerPasswordValidator>()
        .AddCorsPolicyService<ConfigCorsPolicyService>()
        .AddDeveloperSigningCredential();
    services.AddSingleton<ConfigUserStore>();
    services.AddCors(options => options
        .AddPolicy("AllowAllOrigins", policy => policy
            .WithOrigins("https://localhost:5001")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin()));
    services.Add(ServiceDescriptor.Transient<ICorsPolicyProvider, DefaultCorsPolicyProvider>());
    services.AddOptions();
    services.Configure<SecuritySettings>(builder.Configuration.GetSection("security"));

    var app = builder.Build();
    app.UseDeveloperExceptionPage();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseIdentityServer();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapDefaultControllerRoute();
    });
    await app.RunAsync();
    // builder.WebHost.UseStartup<Startup>();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly.");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

