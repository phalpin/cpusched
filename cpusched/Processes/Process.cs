using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes.Execution;

namespace cpusched.Execution
{
    public class Process
    {

        #region Private Vars

            private ProcessState _state;
            private ExecutionTime _executiontimes;

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

        #endregion

            
        public Process() { }

        


    }
}
