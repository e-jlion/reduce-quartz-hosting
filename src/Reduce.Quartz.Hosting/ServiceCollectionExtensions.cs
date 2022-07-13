using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Reduce.Quartz.Hosting.Extensions;
using Reduce.Quartz.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 通过IScheduleStrategyAttribute 标识注册启动Job
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        public static void AddHostedStragegyJob(this IServiceCollection services, Assembly assembly)
        {
            TryAddScheduledHostedService(services);

            //初始化CronRule
            var cronRuleAttributes = assembly.GetStragegyAttributes<IJob>();//获得所有的IJobRuleAttribute
            foreach (var attributeitem in cronRuleAttributes)
            {
                var type = attributeitem.Item1;
                var attribute = attributeitem.Item2;

                services.Add(new ServiceDescriptor(type, type, ServiceLifetime.Singleton));
                services.AddSingleton(new ScheduledJob(type, attribute));
            }
        }

        private static void TryAddScheduledHostedService(this IServiceCollection services)
        {
            services.TryAddSingleton<IJobFactory, QuartzJobFactory>();
            services.TryAddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddHostedService<ScheduledHostedService>();
        }
    }
}
