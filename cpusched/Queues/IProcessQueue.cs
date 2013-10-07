using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes;

namespace cpusched.Queues
{
    interface IProcessQueue
    {
        QueueExecutionResult Run();

        void Wait();

        void IncrementTimes();

        void AddProcess(Process p);
    }
}
