using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reduce.Quartz.Hosting.Extensions
{
    internal static class AssemblyExtensions
    {
        public static List<(Type, IScheduleStrategyAttribute)> GetStragegyAttributes<T>(this Assembly assembly)
        {
            var assemblys = assembly.GetTypes().AsEnumerable()
                        .Where(type => typeof(T).IsAssignableFrom(type)).ToList();

            var list = new List<(Type, IScheduleStrategyAttribute)>();
            assemblys.ForEach(t =>
            {
                foreach (Attribute attribute in t.GetCustomAttributes())
                {
                    if (attribute is IScheduleStrategyAttribute)
                    {
                        list.Add((t, (IScheduleStrategyAttribute)attribute));
                    }
                }
            });
            return list;
        }
    }
}
