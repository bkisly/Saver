using Microsoft.Extensions.Configuration;
using Saver.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");
var rabbitMq = builder.AddRabbitMQ("rabbitmq");
var postgres = builder.AddPostgres("postgres");

var identityServiceDb = postgres.AddDatabase("identityservice-db");
var budgetServiceDb = postgres.AddDatabase("budgetservice-db");
var financeServiceDb = postgres.AddDatabase("financeservice-db");
var predictionsServiceDb = postgres.AddDatabase("predictionsservice-db");

var publicKey = builder.Configuration.GetValue<string>("Identity:PublicKey")
                ?? throw new InvalidOperationException("Could not find Identity:PublicKey configuration value.");

var identityService = builder.AddProject<Projects.Saver_IdentityService>("identityservice")
    .WithReference(identityServiceDb)
    .WithReference(rabbitMq)
    .WithEnvironment("Identity__PublicKey", publicKey);

var identityEndpoint = identityService.GetEndpoint("https");

identityService.WithIdentityEnvironment(identityEndpoint, publicKey);
    
var budgetService = builder.AddProject<Projects.Saver_BudgetService>("budgetservice")
    .WithReference(budgetServiceDb)
    .WithReference(rabbitMq)
    .WithIdentityEnvironment(identityEndpoint, publicKey);

var financeService = builder.AddProject<Projects.Saver_FinanceService>("financeservice")
    .WithReference(financeServiceDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WithIdentityEnvironment(identityEndpoint, publicKey);

var predictionsService = builder.AddProject<Projects.Saver_PredictionsService>("predictionsservice")
    .WithReference(predictionsServiceDb)
    .WithReference(rabbitMq)
    .WithIdentityEnvironment(identityEndpoint, publicKey);

builder.AddProject<Projects.Saver_Client>("client")
    .WithReference(identityService)
    .WithReference(budgetService)
    .WithReference(financeService)
    .WithReference(predictionsService);

builder.Build().Run();
  