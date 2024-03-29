# reduce-quartz-hosting 简介
Quartz.Net Job 简化启动 ，支持自定义执行策略；通过一次注入启动所有实现的IJob ，并通过标注特性做为Job执行策略，同时可以自定义实现特性策略，可扩展性强，提高开发定时作业的效率。
Reduce-quartz-hosting  对 Quartz.net 组件 Job 的管理更加方便，很少的代码植入，即可管理启动Job
- 目前版本：1.1.0


## 特性
- 【支持执行策略的自定义】 对Quartz.Net Job 的执行策略可以自定义扩展
- 【简化Job启动注册流程】 只需要一次注册即可全部Job 启动
- 【支持动态创建Job服务】基于相同Job执行模板动态创建任务，可以用于调度器和执行器分离方案中的动态创建调度器

## 安装
```
Install-Package Reduce.Quartz.Hosting -Version 1.1.0
```


## 全局注册启动Job
```
public void ConfigureServices(IServiceCollection services)
{
   services.AddHostedStragegyJob(typeof(Startup).Assembly);
}
```

## 全局注册 支持配置方式启动Job
```
services.AddHostStragegyJob(typeof(Startup).Assembly, config =>
            {
                config.NameValueCollection = new NameValueCollection
                {
                    {"quartz.threadPool.threadCount","2" }
                };
            });
```

## 使用说明
### 目前提供的内置执行策略特性
  - [`CronScheduleStrategyAttribute`] 以当前默认时间的Cron表达式做为Job的执行策略
  - [`UtcTimeCronScheduleAttribute`]  以UTC 时间的Cron 表达式做为Job的执行策略
  - [`支持自定义执行策略的实现`] 可以调用方自己实现`IScheduleStrategyAttribute` Job 执行定时执行策略的实现，比较灵活，扩展性强

### Job 实现

- 使用内置常用的 CronScheduleStrategyAttribute 特性去执行Job

```
    [DisallowConcurrentExecution]
    [CronScheduleStrategyAttribute(CronExpress = "* * * * * ? *", Description = "HelloV1", Name = "HelloV1")]
    public class HelloJobV2 : IJob
    {
        static int _count = 0;
        public Test2 _test;
        public HelloJobV2(Test2 test2)
        {
            _test = test2;
        }


        public async Task Execute(IJobExecutionContext context)
        {
            System.Console.WriteLine($"HelloV2执行 {++_count},次 开始");
            await Task.Delay(1000 * 60);
            System.Console.WriteLine($"HelloV2执行 {++_count},次 完成");

            //_logger.LogInformation($"执行中...");
            //throw new NotImplementedException();
        }
    }
```
### 自定义执行策略特性

- 实现 IScheduleStrategyAttribute 策略特性

```
    /// <summary>
    ///  自定义的执行策略
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CustomerScheduleStrategyAttribute : Attribute, IScheduleStrategyAttribute
    {
        /// <summary>
        /// Cron 执行表达式
        /// </summary>
        public string CronExpress { set; get; }

        /// <summary>
        /// 执行Job的名称 （默认Job实例的FullName）
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 执行Job的描述
        /// </summary>
        public string Description { set; get; }


        public IJobDetail CreateJob(ScheduledJob schedule)
        {

            var jobName = string.IsNullOrEmpty(Name) ? schedule.Type.FullName : Name;
            var groupName = jobName + "_group";

            return JobBuilder
                    .Create(schedule.Type)
                    .WithIdentity(jobName, groupName)
                    .WithDescription(Description)
                    .Build();
        }


        public ITrigger CreateTrigger(ScheduledJob schedule)
        {
            var jobName = string.IsNullOrEmpty(Name) ? schedule.Type.FullName : Name;
            var groupName = jobName + "_group";
            var triggerName = jobName + "_trigger";
            var triggerGroupName = groupName;


            return TriggerBuilder
                    .Create()
                    .WithIdentity(triggerName, triggerGroupName)
                    .WithCronSchedule(CronExpress)
                    .WithDescription(Description ?? schedule.Type.FullName)
                    .Build();
        }

    }
```

- 自定义执行策略 Job 实现代码,标注`CustomerScheduleStrategyAttribute` 即可

```
    [DisallowConcurrentExecution]
    [CustomerScheduleStrategyAttribute(CronExpress = "* * * * * ? *", Description = "HelloV4")]
    public class HelloJobV4 : IJob
    {
        static int _count = 0;
        public HelloJobV4()
        {
        }


        public async Task Execute(IJobExecutionContext context)
        {
            System.Console.WriteLine($"HelloV4执行 {++_count},次 开始");
            await Task.Delay(1000 * 60);
            System.Console.WriteLine($"HelloV4执行 {++_count},次 完成");

        }
    }
```

### 动态创建Job
```
 //启动的时候注入统一的调度器调度执行模板
 services.AddTransient(typeof(TemplateJob));//动态创建Job 调度器的模板 这个模板里面可以实现http 、grpc、生产消息、或者执行某个简单的sql 动作，也可以分多个模板实现


 // 可以通过api 动态创建job ，调度器和执行器分离方案比较实用
 //使用的时候需要 通过DI拿到对应的 调度器执行模板 进行创建调度器JOb  
 
 _taskJobBuilder = serviceProvider.GetRequiredService<TaskJobBuilder<TemplateJob>>();

 await _taskJobBuilder.AddJob(new TaskOptions(){
                Cron = "* * * * * ? *",

                Description = "test01",
                Group = "testgroup01",
                Name = "test01"
 }, context.CancellationToken);

```

## 更新说明
- 2022-11-04 v1.1.0
  - 添加动态创建Job 方法，不需要重启服务,应用场景调度器和执行器分离，调度器管理中可以用该方法
- 2022-07-11 v1.0.1
  - Quartz.Reduce.Hosting 更名成Reduce.Quartz.Hosting （由于nuget.org 中Quartz前缀Id 被使用，需要授权故更名）
- 2022-07-10 v1.0.0
第一个版本，依赖Quartz,3.4.0 适用于.net core 3.1+
