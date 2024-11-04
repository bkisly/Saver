using Saver.FinanceService.Api;
using Saver.FinanceService.Extensions;
using Saver.FinanceService.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDefaultSwagger();
builder.AddApplicationServices();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapGet("/", () => "Hello World!");

app.MapAccountsApi()
   .MapTransactionsApi()
   .MapCategoriesApi()
   .MapReportsApi();

app.UseDefaultSwagger();
app.UseMiddleware<ValidationExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}

app.Run();
