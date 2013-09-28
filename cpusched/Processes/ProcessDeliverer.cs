using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpusched.Execution
{
    
    /// <summary>
    /// Helper class to implement a realtime process delivery routine.
    /// </summary>
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

        public ProcessDeliverer() { }

        /// <summary>
        /// Gets arrivals at this current point.
        /// </summary>
        /// <returns></returns>
        public List<Process> GetArrivals()
        {
            List<Process> result = new List<Process>();

            foreach (Process p in this._incoming)
            {
                
            }

            return result;
        }


    }
}
