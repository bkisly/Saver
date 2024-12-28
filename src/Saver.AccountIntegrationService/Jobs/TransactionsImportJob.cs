using Quartz;
using Saver.AccountIntegrationService.BankServices;

namespace Saver.AccountIntegrationService.Jobs;

public class TransactionsImportJob(IBankServicesRegistry providersRegistry) : IJob
{
    public const string ProviderTypeJobDataKey = "BankServiceType";
    public const string IntegrationIdJobDataKey = "IntegrationId";

    public async Task Execute(IJobExecutionContext context)
    {
        var providerType = (BankServiceType)context.JobDetail.JobDataMap[ProviderTypeJobDataKey];
        var integrationId = (Guid)context.JobDetail.JobDataMap[IntegrationIdJobDataKey];

        var provider = providersRegistry.GetByBankServiceType(providerType);
        await provider.ImportTransactionsAsync(integrationId, context.PreviousFireTimeUtc?.UtcDateTime);
    }
}