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

        #endregion

            
        public Process() { }

        public Process(ExecutionTime t)
        {
            this._executiontime = t;
        }

        /// <summary>
        /// Runs the process.
        /// </summary>
        public void Run()
        {
            this._executiontime--;
            if (this._executiontime.Remaining == 0)
            {
                this._state = ProcessState.COMPLETE;
            }
        }
        


    }
}
