﻿using System;
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
            protected bool _switched = false;
            protected int _totalTime = 0;
            protected int _idleTime = 0;
            protected string _queueName = "";

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

            /// <summary>
            /// The total time that this queue has been running.
            /// </summary>
            public int TotalTime
            {
                get { return this._totalTime; }
            }

            /// <summary>
            /// Indicates whether or not there was a context switch on the last run.
            /// </summary>
            public bool ContextSwitch
            {
                get { return this._switched; }
            }

            /// <summary>
            /// The name of this Queue.
            /// </summary>
            public string Name
            {
                get { return this._queueName; }
                set { this._queueName = value; }
            }

        #endregion

        /// <summary>
        /// Runs this ProcessQueue
        /// </summary>
        /// <returns>QueueExecutionResult with what happened with the queue.</returns>
        public QueueExecutionResult Run()
        {
            QueueExecutionResult result = QueueExecutionResult.IDLE;

            this.Sort();

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

            //Before running anything, sort.
            this.RemoveIOFromReady();
            this.RemoveReadyFromIO();
            

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

        /// <summary>
        /// Removes IO Processes from the Ready Queue.
        /// </summary>
        protected void RemoveIOFromReady()
        {
            List<Process> removeFromReady = new List<Process>();
            foreach (Process p in this._readyprocs)
            {
                if (p.State == ProcessState.IO)
                {
                    this._ioprocs.Add(p);
                    removeFromReady.Add(p);
                }
                if (p.State == ProcessState.COMPLETE)
                {
                    this._completeprocs.Add(p);
                    removeFromReady.Add(p);
                }
            }
            
            //Remove processes from ready as necessary.
            foreach (Process p in removeFromReady)
            {
                this._readyprocs.Remove(p); 
            }
        }

        /// <summary>
        /// Removes Ready Processes from IO Queue.
        /// </summary>
        protected void RemoveReadyFromIO()
        {
            List<Process> removeFromIO = new List<Process>();
            
            //Iterate through IOProcs
            foreach (Process p in this._ioprocs)
            {
                if (p.State == ProcessState.READY)
                {
                    this._readyprocs.Add(p);
                    removeFromIO.Add(p);
                }
                else if (p.State == ProcessState.COMPLETE)
                {
                    this._completeprocs.Add(p);
                    removeFromIO.Add(p);
                }
            }

            //remove processes from IO as necessary.
            foreach (Process p in removeFromIO)
            {
                this._ioprocs.Remove(p);
            }

        }

        /// <summary>
        /// Returns context switches that occur.
        /// </summary>
        /// <returns>Process that was switched to</returns>
        public Process GetContextSwitch()
        {
            Process Result = new Process();

            Result = this._next;

            return Result;

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