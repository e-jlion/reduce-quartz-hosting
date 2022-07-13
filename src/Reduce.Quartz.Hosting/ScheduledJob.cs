using System;

namespace Reduce.Quartz.Hosting
{
    public class ScheduledJob
    {
        public IScheduleStrategyAttribute JobRuleAttribute;

        public Type Type { get; }

        public ScheduledJob(Type type, IScheduleStrategyAttribute ruleAttribute)
        {
            Type = type;
            JobRuleAttribute = ruleAttribute;
        }

    }
}
