var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Saver_Gateway>("saver-gateway");

builder.AddProject<Projects.Saver_IdentityService>("saver-identityservice");

builder.AddProject<Projects.Saver_BudgetService>("saver-budgetservice");

builder.AddProject<Projects.Saver_FinanceService>("saver-financeservice");

builder.AddProject<Projects.Saver_PredictionsService>("saver-predictionsservice");

builder.AddProject<Projects.Saver_Client>("saver-client");

builder.Build().Run();
