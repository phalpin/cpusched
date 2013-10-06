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
    public class ProcessQueue
    {

        #region Private Vars

            private List<Process> _readyprocs = new List<Process>();
            private List<Process> _ioprocs = new List<Process>();

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
            /// Accessor for the next item in the queue.
            /// </summary>
            public Process Next
            {
                get { return this._readyprocs.ElementAt(0); }
            }

        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public ProcessQueue() { }

        /// <summary>
        /// Runs this queue.
        /// </summary>
        public void Run() { }

        /// <summary>
        /// Waits all processes in this queue.
        /// </summary>
        public void Wait() { }

        /// <summary>
        /// Adds a process to this queue.
        /// </summary>
        /// <param name="p">Process to Add</param>
        public void AddProcess(Process p) { }


    }
}