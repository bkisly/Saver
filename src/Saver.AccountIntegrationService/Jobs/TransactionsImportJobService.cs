using Quartz;
using Saver.AccountIntegrationService.Models;

namespace Saver.AccountIntegrationService.Jobs;

public class TransactionsImportJobService(ISchedulerFactory schedulerFactory) : ITransactionsImportJobService
{
    public async Task RegisterJobAsync(AccountIntegration integration, int interval)
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
            .WithSimpleSchedule(x => x.WithIntervalInMinutes(interval))
            .Build();

        await scheduler.ScheduleJob(job, trigger);
    }

    public async Task UnregisterJobAsync(Guid integrationId)
    {
        var scheduler = await schedulerFactory.GetScheduler();
        await scheduler.UnscheduleJob(new TriggerKey($"transactionsImport-trigger-{integrationId}"));
    }
}