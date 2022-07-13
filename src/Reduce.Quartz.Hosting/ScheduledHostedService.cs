using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;

namespace Reduce.Quartz.Hosting
{
    internal class ScheduledHostedService : IHostedService
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IEnumerable<ScheduledJob> _jobSchedules;
        private readonly IJobFactory _jobFactory;



        public ScheduledHostedService(
            ISchedulerFactory schedulerFactory,
            IJobFactory jobFactory,
            IEnumerable<ScheduledJob> jobSchedules)
        {
            _schedulerFactory = schedulerFactory;
            _jobSchedules = jobSchedules;
            _jobFactory = jobFactory;
        }

        private IScheduler Scheduler { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in _jobSchedules)
            {
                await ScheduleJob(cancellationToken, jobSchedule);
            }

            await Scheduler.Start(cancellationToken);
        }

        private async Task ScheduleJob(CancellationToken cancellationToken, ScheduledJob scheduledJob)
        {
            if (scheduledJob == null || scheduledJob.JobRuleAttribute == null)
                throw new ArgumentNullException("job the ScheduleStrategyAttribute is null");

            var job = scheduledJob.JobRuleAttribute.CreateJob(scheduledJob);
            var trigger = scheduledJob.JobRuleAttribute.CreateTrigger(scheduledJob);

            await Scheduler.ScheduleJob(job, trigger, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (Scheduler != null) await Scheduler.Shutdown(cancellationToken);
        }
    }
}
