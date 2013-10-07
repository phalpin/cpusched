using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes;
using cpusched.Processes.Execution;

namespace cpusched.Queues
{
    /// <summary>
    /// The ProcessQueue object is used within FCFS, etc objects to run Processes. Abstraction on the Process Level.
    /// </summary>
    public abstract class ProcessQueue
    {

        #region Private Vars

            protected List<Process> _readyprocs = new List<Process>();
            protected List<Process> _ioprocs = new List<Process>();
            protected List<Process> _completeprocs = new List<Process>();
            protected Process _next = null;
            protected QueueState _state = QueueState.READY;
            protected int _totalTime = 0;
            protected int _idleTime = 0;

        #endregion

        #region Public Accessors

            /// <summary>
            /// List of All Processes ready/waiting in the queue
            /// </summary>
            public List<Process> ReadyProcs
            {
                get { return this._readyprocs; }
                set { this._readyprocs = value; }
            }

            /// <summary>
            /// All processes currently in IO
            /// </summary>
            public List<Process> IOProcs
            {
                get { return this._ioprocs; }
                set { this._ioprocs = value; }
            }

            /// <summary>
            /// The complete processes for this queue.
            /// </summary>
            public List<Process> CompleteProcs
            {
                get { return this._completeprocs; }
            }

            /// <summary>
            /// Accessor for the next item in the queue.
            /// </summary>
            public Process Next
            {
                get { return this._next; }
            }

            /// <summary>
            /// Refers to the current state of this queue.
            /// </summary>
            public QueueState State
            {
                get { return this._state; }
            }

            /// <summary>
            /// Represents the CPU Utilization of this Queue.
            /// </summary>
            public Double CPUUtil
            {
                get
                {
                    if (_idleTime > 0) return ((this._totalTime - this._idleTime) / (double)this._totalTime);
                    else return 0;
                }
            }

        #endregion

        public abstract QueueExecutionResult Run();

        public abstract void Wait();

        public abstract void IncrementTimes();

        protected abstract void Sort();

        public abstract void AddProcess(Process p);


    }

    /// <summary>
    /// Possible states for this queue to be in.
    /// </summary>
    public enum QueueState
    {
        READY,
        ALLIO,
        COMPLETE
    }

    public enum QueueExecutionResult
    {
        IDLE,
        RUN
    }
}