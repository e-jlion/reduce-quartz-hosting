using System;
using System.Collections.Generic;
using System.Text;

namespace Reduce.Quartz.Hosting.Enums
{
    public enum EnumTaskJobAction
    {
        ///// <summary>
        ///// Job新增
        ///// </summary>
        //JOB_ADD = 1,

        /// <summary>
        /// Job动态移除
        /// </summary>
        JOB_REMOVE = 2,

        /// <summary>
        /// Job 动态修改
        /// </summary>
        JOB_UPDATE = 3,

        /// <summary>
        /// JOB 暂停
        /// </summary>
        JOB_PAUSE = 4,

        /// <summary>
        /// JOB 立即执行
        /// </summary>
        JOB_EXECUTE = 5,

        /// <summary>
        /// 开启
        /// </summary>
        JOB_START = 6,

        /// <summary>
        /// 停止
        /// </summary>
        JOB_STOP = 7,
    }

}
