using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpusched.Processes
{
    public class Process
    {

        #region Private Vars

            private ProcessState _state;

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
