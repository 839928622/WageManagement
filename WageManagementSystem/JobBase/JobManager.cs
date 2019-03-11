using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using WageManagementSystem.Jobs;

namespace WageManagementSystem.JobBase
{
    public class JobManager
    {
        public static void State()
        {

            //开启调度
            JobBase.Scheduler.Start();

            // 第一个参数是你要执行的工作(job)  第二个参数是这个工作所对应的触发器(Trigger)(例如:几秒或几分钟执行一次)
            JobBase.AddSchedule(new JobServer<SyncEmployeeInfo>(),
                new SyncEmployeeInfoTrigger().AddTrigger(), "同步、生成发放记录", "工作组1");

            // 第一个参数是你要执行的工作(job)  第二个参数是这个工作所对应的触发器(Trigger)



        }
    }
}