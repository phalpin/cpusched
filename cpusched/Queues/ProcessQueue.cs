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
    public abstract class ProcessQueue : IProcessQueue
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
            public Decimal CPUUtil
            {
                get
                {
                    if (_idleTime > 0) return ((this._totalTime - this._idleTime) / (Decimal)this._totalTime);
                    else return 0;
                }
            }

        #endregion

        /// <summary>
        /// Runs this ProcessQueue
        /// </summary>
        /// <returns>QueueExecutionResult with what happened with the queue.</returns>
        public QueueExecutionResult Run()
        {
            QueueExecutionResult result = QueueExecutionResult.IDLE;
            //Before running anything, sort.
            this.Sort();
            if (this._state != QueueState.COMPLETE)
            {
                //Check what Sort has to say about the state of the Queue.
                switch (this._state)
                {
                    //Case for the queue being ready to run.
                    case QueueState.READY:
                        if (this.Next != null)
                        {
                            this.Next.Run();
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
        /// Waits all processes in this queue. Stuff in IO runs, stuff in Ready waits.
        /// </summary>
        public void Wait()
        {
            //Run all procs in IO.
            foreach (Process p in this._ioprocs) p.Run();
            //Wait all procs in the ready queue.
            foreach (Process p in this._readyprocs) p.Wait();
        }

        /// <summary>
        /// Increments times for everything except Next.
        /// </summary>
        public void IncrementTimes()
        {
            foreach (Process p in this._readyprocs) if (p != this._next) p.Wait();
            foreach (Process p in this._ioprocs) p.Run();
        }

        /// <summary>
        /// Each ProcessQueue must implement its own Sort(). At the end of Sort(), this.Next should point to the next process to go, or null.
        /// </summary>
        protected abstract void Sort();

        /// <summary>
        /// Adds a process to this queue.
        /// </summary>
        /// <param name="p">Process to add.</param>
        public void AddProcess(Processes.Process p)
        {
            this._readyprocs.Add(p);
        }


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

    /// <summary>
    /// The result of a Run() directive on a ProcessQueue, this will pass back what the queue did, either ran or stayed idle.
    /// </summary>
    public enum QueueExecutionResult
    {
        IDLE,
        RUN
    }
}