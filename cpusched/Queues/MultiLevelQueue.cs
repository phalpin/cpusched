using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes;

namespace cpusched.Queues
{
    public abstract class MultiLevelQueue : IMultiLevelQueue
    {
        #region Private Vars

            private List<ProcessQueue> _queues = new List<ProcessQueue>();

        #endregion

        #region Public Accessors

            /// <summary>
            /// List of Queues in this Multilevel queue
            /// </summary>
            public List<ProcessQueue> Queues
            {
                get { return this._queues; }
                set { this._queues = value; }
            }

        #endregion

        /// <summary>
        /// Runs this multilevel queue.
        /// </summary>
        /// <returns>The </returns>
        public QueueExecutionResult Run()
        {
            QueueExecutionResult result = QueueExecutionResult.IDLE;

            return result;
        }

        /// <summary>
        /// Adds a process queue to this MLFQ
        /// </summary>
        /// <param name="pq"></param>
        public void AddProcessQueue(ProcessQueue pq)
        {
            this._queues.Add(pq);
        }

        /// <summary>
        /// Gets a context switch that occurred on the last run.
        /// </summary>
        /// <returns></returns>
        public Process GetContextSwitch()
        {
            Process p = null;

            return p;
        }
    }
}
