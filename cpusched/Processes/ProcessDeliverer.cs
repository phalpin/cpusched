using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpusched.Processes
{
    class ProcessDeliverer
    {

        #region Private Vars

            private List<Process> _incoming;

        #endregion



        #region Public Accessors

            /// <summary>
            /// Processes in the queue that have yet to arrive.
            /// </summary>
            public List<Process> Processes
            {
                get { return this._incoming; }
                set { this._incoming = value; }
            }

        #endregion
    }
}
