# quartz-reduce-hosting
Quartz.Net 实现Job 简化启动实现，支持自定义执行策略，简化Job初始化的复杂逻辑，只需要一个特性标注即可自动启动全部实现的Job
- 目前版本：1.0.0


## 特性
- 【支持执行策略的自定义】 对Quartz.Net Job 的执行策略可以自定义扩展
- 【简化Job启动注册流程】 只需要一次注册即可全部Job 启动

## 安装
```
Install-Package Quartz.Reduce.Hosting -Version 1.0.0
```

## 使用说明
- 目前提供的特性说明
  - [CronScheduleStrategyAttribute] 以当前默认时间的Cron表达式做为Job的执行策略
  - [UtcTimeCronScheduleAttribute]  以UTC 时间的Cron 表达式做为Job的执行策略

- Job 实现
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

- 全局注册启动Job
```
public void ConfigureServices(IServiceCollection services)
{
   services.AddHostedStragegyJob(typeof(Startup).Assembly);
}
```

## 更新说明
- 2022-07-10 v1.0.0
第一个版本，依赖Quartz,3.4.0 适用于.net core 3.1+
