var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");
var rabbitMq = builder.AddRabbitMQ("rabbitmq");
var postgres = builder.AddPostgres("postgres");

var identityServiceDb = postgres.AddDatabase("identityservice-db");
var budgetServiceDb = postgres.AddDatabase("budgetservice-db");
var financeServiceDb = postgres.AddDatabase("financeservice-db");
var predictionsServiceDb = postgres.AddDatabase("predictionsservice-db");

var identityService = builder.AddProject<Projects.Saver_IdentityService>("identityservice")
    .WithReference(identityServiceDb)
    .WithReference(rabbitMq);

var budgetService = builder.AddProject<Projects.Saver_BudgetService>("budgetservice")
    .WithReference(budgetServiceDb)
    .WithReference(rabbitMq);

var financeService = builder.AddProject<Projects.Saver_FinanceService>("financeservice")
    .WithReference(financeServiceDb)
    .WithReference(rabbitMq)
    .WithReference(redis);

var predictionsService = builder.AddProject<Projects.Saver_PredictionsService>("predictionsservice")
    .WithReference(predictionsServiceDb)
    .WithReference(rabbitMq);

builder.AddProject<Projects.Saver_Client>("client")
    .WithReference(identityService)
    .WithReference(budgetService)
    .WithReference(financeService)
    .WithReference(predictionsService);

builder.Build().Run();
  