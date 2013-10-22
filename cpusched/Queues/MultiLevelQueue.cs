using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes;

namespace cpusched.Queues
{
    public abstract class MultiLevelQueue : IQueue
    {
        #region Private Vars

            protected List<ProcessQueue> _queues = new List<ProcessQueue>();
            protected ProcessQueue _next = null;
            protected QueueState _state = QueueState.READY;
            protected bool _switched = false;
            protected int _totalTime = 0;
            protected int _idleTime = 0;
            protected string _name = "";

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

            /// <summary>
            /// Determines the state of this MultiLevelQueue.
            /// </summary>
            public QueueState State
            {
                get { return this._state; }
                set { this._state = value; }
            }

            /// <summary>
            /// Returns whether or not a context switch occurred on this run.
            /// </summary>
            public bool ContextSwitch
            {
                get { return this._switched; }
            }

            /// <summary>
            /// Complete Processes in all queues.
            /// </summary>
            public List<Process> CompleteProcs
            {
                get
                {
                    List<Process> result = new List<Process>();
                    foreach(ProcessQueue pq in this._queues) result.AddRange(pq.CompleteProcs);
                    result.Sort((x, y) => x.Name.CompareTo(y.Name));
                    return result;
                }
            }

            /// <summary>
            /// All processes ready in all queues.
            /// </summary>
            public List<Process> ReadyProcs
            {
                get
                {
                    List<Process> result = new List<Process>();
                    foreach (ProcessQueue pq in this._queues) result.AddRange(pq.ReadyProcs);
                    return result;
                }
            }

            /// <summary>
            /// All processes in I/O in all queues.
            /// </summary>
            public List<Process> IOProcs
            {
                get
                {
                    List<Process> result = new List<Process>();
                    foreach (ProcessQueue pq in this._queues) result.AddRange(pq.IOProcs);
                    result.Sort((x, y) => x.Name.CompareTo(y.Name));
                    return result;
                }
            }

            /// <summary>
            /// Total Time this queue has run.
            /// </summary>
            public int TotalTime
            {
                get { return this._totalTime; }
            }

            /// <summary>
            /// Returns the total amount of CPU Utilization, expressed as a decimal value (EX: 82% = .82)
            /// </summary>
            public Decimal CPUUtil
            {
                   get
                   {
                       if (this._idleTime > 0) return ((this._totalTime - this._idleTime) / (Decimal)this._totalTime);
                       else return 0;   
                   }
            }

            /// <summary>
            /// Returns the next process to run.
            /// </summary>
            public ProcessQueue Next{
                get{ return this._next; }
            }

            /// <summary>
            /// Name of the queue.
            /// </summary>
            public string Name
            {
                get { return this._name; }
            }

        #endregion

        /// <summary>
        /// Runs this multilevel queue.
        /// </summary>
        /// <returns>The </returns>
        public QueueExecutionResult Run()
        {
            QueueExecutionResult result = QueueExecutionResult.IDLE;
            this.Sort();
            result = this.RunWithoutSort();
            return result;
        }

        /// <summary>
        /// Simply runs the queue without Sort()ing first.
        /// </summary>
        /// <returns></returns>
        public QueueExecutionResult RunWithoutSort()
        {
            QueueExecutionResult result = QueueExecutionResult.IDLE;

            //Now, actually run the damn thing.
            if (this._state != QueueState.COMPLETE)
            {
                //Check what Sort has to say about the state of the Queue.
                switch (this._state)
                {
                    //Case for the queue being ready to run.
                    case QueueState.READY:
                        if (this.Next != null)
                        {
                            this.Next.RunWithoutSort();
                            result = QueueExecutionResult.RUN;
                        }
                        else
                        {
                            result = QueueExecutionResult.IDLE;
                        }
                        this.IncrementTimes();
                        break;
                    case QueueState.ALLIO:
                        result = QueueExecutionResult.IDLE;
                        this.IncrementTimes();
                        this._idleTime++;
                        break;
                }

                this._totalTime++;
            }


            return result;
        }

        /// <summary>
        /// Waits all ProcessQueues in this MultilevelQueue
        /// </summary>
        public void Wait()
        {
            foreach (ProcessQueue pq in this._queues) pq.Wait();
        }

        /// <summary>
        /// To be defined in the individual MLQ
        /// </summary>
        public abstract void Sort();

        /// <summary>
        /// Increments times in all queues that don't run.
        /// </summary>
        private void IncrementTimes()
        {
            foreach (ProcessQueue pq in this._queues)
            {
                if (pq != this._next)
                {
                    pq.Wait();
                }
            }
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
            if (this._next != null)
            {
                if (this._next.Next != null)
                {
                    p = this._next.Next;
                }
            }
            return p;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Cleanup() { }
    }
}
