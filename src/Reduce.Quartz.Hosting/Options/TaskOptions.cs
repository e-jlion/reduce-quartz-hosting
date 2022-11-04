using System;
using System.Collections.Generic;
using System.Text;

namespace Reduce.Quartz.Hosting.Options
{
    public class TaskOptions
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Group { set; get; }

        /// <summary>
        /// 执行策略表达式
        /// </summary>
        public string Cron { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { set; get; }

        public bool IsVerify()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Group) || string.IsNullOrWhiteSpace(Cron))
            {
                return false;
            }
            return true;
        }
    }
}
