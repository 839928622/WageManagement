using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WageManagementSystem.Jobs;

namespace WageManagementSystem.JobBase
{
    public class JobBase
    {
        public static IScheduler Scheduler
        {
            get
            {
                var properties = new NameValueCollection();

                // 设置线程池
                properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
                //设置线程池的最大线程数量
                properties["quartz.threadPool.threadCount"] = "5";
                //设置作业中每个线程的优先级
                properties["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString();

                // 远程输出配置
                properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
                properties["quartz.scheduler.exporter.port"] = "555"; //配置端口号
                properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
                properties["quartz.scheduler.exporter.channelType"] = "tcp"; //协议类型

                //创建一个工厂
                var schedulerFactory = new StdSchedulerFactory(properties);
                //启动
                var scheduler = schedulerFactory.GetScheduler();

                return scheduler;
            
        }
        //try
            //{
            //    var properties = new NameValueCollection
            //    {


            //        ["quartz.scheduler.instanceName" ]= "MyScheduler",
            //        ["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore.Quartz",                    
            //        ["quartz.threadPool.threadCount"] = "3",

            //        // 设置线程池
            //        ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
            //        //设置线程池的最大线程数量
            //      //  ["quartz.threadPool.threadCount"] = "5",
            //        //设置作业中每个线程的优先级
            //        ["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString(),

            //        // 远程输出配置
            //        ["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz",
            //        ["quartz.scheduler.exporter.port"] = "555",  //配置端口号
            //        ["quartz.scheduler.exporter.bindName"] = "QuartzScheduler",
            //        ["quartz.scheduler.exporter.channelType"] = "tcp" //协议类型
            //    };

            //    StdSchedulerFactory schedulerFactory = new StdSchedulerFactory(properties);

            //    IScheduler scheduler = await schedulerFactory.GetScheduler();

            //    await scheduler.Start();

            //    IJobDetail job = JobBuilder.Create<SyncEmployeeInfo>()
            //        .WithIdentity("job1", "group1")
            //        .Build();

            //var trigger = TriggerBuilder.Create()
            //    .WithIdentity("同步、生成发放费用作业", "作业触发器")
            //    .WithCronSchedule("0 0 0 0 1/1 ?")//从1日开始，每月执行1次
            //    .Build();

            //await scheduler.ScheduleJob(job, trigger);

            //}
            //catch (SchedulerException se)
            //{
            //    Console.WriteLine(se);
            //    throw;
            //}




        }

        public static void AddSchedule<T>(JobServer<T> jobServer, ITrigger trigger, string jobName, string jobGroup) where T : IJob
        {
            jobServer.JobName = jobName;
            jobServer.JobGroup = jobGroup;
            Scheduler.
                ScheduleJob(jobServer.CrateJob(), trigger);
        }
    }
}