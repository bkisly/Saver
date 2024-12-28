using Quartz;
using Saver.AccountIntegrationService.BankServices;
using Saver.AccountIntegrationService.Models;

namespace Saver.AccountIntegrationService.Jobs;

public class TransactionImportJobService(ISchedulerFactory schedulerFactory, IProviderConfiguration providerConfiguration) : ITransactionImportJobService
{
    public async Task RegisterJobAsync(AccountIntegration integration)
    {
        var scheduler = await schedulerFactory.GetScheduler();
        var integrationId = integration.Id;
        var bankServiceType = integration.BankServiceType;

        var job = JobBuilder.Create<TransactionsImportJob>()
            .WithIdentity($"transactionsImport-job-{integrationId}")
            .UsingJobData(TransactionsImportJob.IntegrationIdJobDataKey, integrationId)
            .UsingJobData(TransactionsImportJob.ProviderTypeJobDataKey, bankServiceType.Id)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"transactionsImport-trigger-{integrationId}")
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(providerConfiguration.GetTransactionsImportInterval(bankServiceType)))
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }

    public async Task UnregisterJobAsync(Guid integrationId)
    {
        var scheduler = await schedulerFactory.GetScheduler();
        await scheduler.UnscheduleJob(new TriggerKey($"transactionsImport-trigger-{integrationId}"));
    }
}