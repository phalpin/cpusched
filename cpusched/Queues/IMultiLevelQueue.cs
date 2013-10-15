using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes;

namespace cpusched.Queues
{
    interface IMultiLevelQueue
    {
        QueueExecutionResult Run();

        void AddProcessQueue(ProcessQueue pq);

        Process GetContextSwitch();
    }
}
