using Quartz;
using Saver.AccountIntegrationService.BankServiceProviders;

namespace Saver.AccountIntegrationService.Jobs;

public class TransactionsImportJob(IBankServiceProvidersRegistry providersRegistry) : IJob
{
    public const string ProviderTypeJobDataKey = "ProviderType";
    public const string IntegrationIdJobDataKey = "IntegrationId";

    public async Task Execute(IJobExecutionContext context)
    {
        var providerType = (BankServiceProviderType)context.JobDetail.JobDataMap[ProviderTypeJobDataKey];
        var integrationId = (Guid)context.JobDetail.JobDataMap[IntegrationIdJobDataKey];

        var provider = providersRegistry.GetByProviderType(providerType);
        await provider.ImportTransactionsAsync(integrationId, context.PreviousFireTimeUtc?.UtcDateTime);
    }
}