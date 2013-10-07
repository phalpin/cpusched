using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpusched.Queues
{
    /// <summary>
    /// Round Robin Queue.
    /// </summary>
    class RR : ProcessQueue
    {
        private int _timequantum = 0;
        
        /// <summary>
        /// The Time Quantum for this Round Robin queue.
        /// </summary>
        public int TimeQuantum
        {
            get { return this._timequantum; }
            set { this._timequantum = value; }
        }

        /// <summary>
        /// Sorting routine for Round Robin
        /// </summary>
        protected override void Sort()
        {
            //Do stuff
        }

    }
}
