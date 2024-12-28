using Quartz;
using Saver.AccountIntegrationService.BankServiceProviders;

namespace Saver.AccountIntegrationService.Jobs;

public class TransactionImportJobService(ISchedulerFactory schedulerFactory, IProviderConfiguration providerConfiguration) : ITransactionImportJobService
{
    public async Task RegisterJobAsync(Guid integrationId, BankServiceProviderType providerType)
    {
        var scheduler = await schedulerFactory.GetScheduler();

        var job = JobBuilder.Create<TransactionsImportJob>()
            .WithIdentity($"transactionsImport-job-{integrationId}")
            .UsingJobData(TransactionsImportJob.IntegrationIdJobDataKey, integrationId)
            .UsingJobData(TransactionsImportJob.ProviderTypeJobDataKey, (int)providerType)
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"transactionsImport-trigger-{integrationId}")
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInSeconds(providerConfiguration.GetTransactionsImportInterval(providerType)))
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }

    public async Task UnregisterJobAsync(Guid integrationId)
    {
        var scheduler = await schedulerFactory.GetScheduler();
        await scheduler.UnscheduleJob(new TriggerKey($"transactionsImport-trigger-{integrationId}"));
    }
}