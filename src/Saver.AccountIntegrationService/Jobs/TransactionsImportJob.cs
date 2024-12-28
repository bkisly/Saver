using Quartz;
using Saver.AccountIntegrationService.BankServices;
using Saver.Common.DDD;

namespace Saver.AccountIntegrationService.Jobs;

public class TransactionsImportJob(IBankServicesResolver resolver) : IJob
{
    public const string BankServiceTypeJobDataKey = "BankServiceType";
    public const string IntegrationIdJobDataKey = "IntegrationId";

    public async Task Execute(IJobExecutionContext context)
    {
        var providerType = Enumeration.FromName<BankServiceType>((string)context.JobDetail.JobDataMap[BankServiceTypeJobDataKey]);
        var integrationId = Guid.Parse((string)context.JobDetail.JobDataMap[IntegrationIdJobDataKey]);

        var provider = resolver.GetByBankServiceType(providerType);
        await provider.ImportTransactionsAsync(integrationId);
    }
}