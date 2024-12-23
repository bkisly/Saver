using Saver.AccountIntegrationService.Api;
using Saver.AccountIntegrationService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultSwagger();
builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/", () => "Hello World!");
app.MapProvidersInfoApi()
    .MapIntegrationsApi();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.Run();
