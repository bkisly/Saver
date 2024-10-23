using Saver.FinanceService.Api;
using Saver.FinanceService.Extensions;

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

app.Run();
