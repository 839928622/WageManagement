using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;

namespace WageManagementSystem.Jobs
{
    public class SyncEmployeeInfoTrigger
    {
        public ITrigger AddTrigger()
        {
        var trigger = TriggerBuilder.Create()
            .WithIdentity("同步、生成发放费用作业", "作业触发器")
            .WithCronSchedule("0 0 0 1 1/1 ? *")//从1日开始，每月执行1次
            .Build();
        return trigger;
        }

    }
}