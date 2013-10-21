using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes;

namespace cpusched.Queues
{
    interface IQueue
    {
        /// <summary>
        /// The state of the Queue.
        /// </summary>
        QueueState State { get; }

        /// <summary>
        /// Returns whether or not a context switch occurred on this latest run.
        /// </summary>
        bool ContextSwitch { get; }

        /// <summary>
        /// Returns a list of all Processes completed in the queue.
        /// </summary>
        List<Process> CompleteProcs { get; }

        /// <summary>
        /// Returns a list of all Processes that are in I/O in the queue.
        /// </summary>
        List<Process> IOProcs { get; }

        /// <summary>
        /// Returns a list of all Processes that are ready in the queue.
        /// </summary>
        List<Process> ReadyProcs { get; }
        
        /// <summary>
        /// Total time the queue has run.
        /// </summary>
        int TotalTime { get; }
        
        /// <summary>
        /// CPU Utilization of the queue up to this point.
        /// </summary>
        Decimal CPUUtil { get; }

        /// <summary>
        /// Name of this queue.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Runs this queue.
        /// </summary>
        /// <returns></returns>
        QueueExecutionResult Run();

        /// <summary>
        /// Waits this queue.
        /// </summary>
        void Wait();

        /// <summary>
        /// Gets the context switch that occurred on this queue.
        /// </summary>
        /// <returns>Process indicating what we switched to</returns>
        Process GetContextSwitch();
    }
}
