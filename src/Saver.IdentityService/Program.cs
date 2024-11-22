using Saver.IdentityService.Api;
using Saver.IdentityService.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultSwagger();
builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapGet("/", () => "Hello World!");
app.MapIdentityApi();

app.UseDefaultSwagger();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.Run();
