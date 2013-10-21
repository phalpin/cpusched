using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes.Execution;

namespace cpusched.Processes
{
    public class Process
    {

        #region Private Vars

            private ProcessState _state;
            private ExecutionTime _executiontime;
            private string _name;
            private bool _hasRun;
            private int _waitingtime;
            private int _turnaroundtime;
            private int _responsetime;
            private int _activeTimeOnProc = 0;

        #endregion


        #region Public Accessors

            /// <summary>
            /// The state of the Process.
            /// </summary>
            public ProcessState State
            {
                get { return this._state; }
                set { this._state = value; }
            }

            /// <summary>
            /// 
            /// </summary>
            public ExecutionTime Time
            {
                get { return this._executiontime; }
            }

            /// <summary>
            /// Total waiting time for this process.
            /// </summary>
            public int WaitingTime
            {
                get { return this._waitingtime; }
            }

            /// <summary>
            /// Turnaround time for this process.
            /// </summary>
            public int TurnaroundTime
            {
                get { return this._turnaroundtime; }
            }

            /// <summary>
            /// The Response Time for this process.
            /// </summary>
            public int ResponseTime
            {
                get { return this._responsetime; }
            }

            /// <summary>
            /// The name of this process.
            /// </summary>
            public string Name
            {
                get { return this._name; }
                set { this._name = value; }
            }

            /// <summary>
            /// How long this process has been active on the processor (for RR Time Quantum Checks)
            /// </summary>
            public int ActiveTimeOnProcessor
            {
                get { return this._activeTimeOnProc; }
            }

        #endregion


        public Process() { }


        public Process(ExecutionTime t)
        {
            this._executiontime = t;
            this._executiontime.Parent = this;
        }

        /// <summary>
        /// Runs the process. Assesses the correct state based on process' current executiontimeunit type.
        /// </summary>
        public void Run()
        {
            //If the process has never run before, run.
            if (!this._hasRun) this._hasRun = true;
            
            //Decrement the execution time.
            this._executiontime--;
            //Increase turnaround time.
            this._turnaroundtime++;
            //Increase Active Time on Processor
            this._activeTimeOnProc++;
            
            //If there's no time remaining after running, change state to complete.
            if (this._executiontime.Remaining == 0) this._state = ProcessState.COMPLETE;
            
            else
            {
                //Change process state to IO if necessary.
                if (this._executiontime.Current.Type == ExecutionTimeType.IO)
                {
                    this._state = ProcessState.IO;
                    this._activeTimeOnProc = 0;
                }
                //Change process state to Ready if necessary.
                else if (this._executiontime.Current.Type == ExecutionTimeType.BURST) this._state = ProcessState.READY;
  
            }


        }

        /// <summary>
        /// Waits the process.
        /// </summary>
        public void Wait()
        {
            //Increase times as necessary. If this process hasn't run, go ahead and increment response time.
            if (!this._hasRun) this._responsetime++;
            this._waitingtime++;
            this._turnaroundtime++;
            this._activeTimeOnProc = 0;
        }

    }
}
