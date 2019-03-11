using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Quartz;

namespace WageManagementSystem.Jobs
{
    public class Job:IJob
        
    {
        
            public Task Execute(IJobExecutionContext context)
            {
                throw new NotImplementedException();
            }
        
        
    }
}