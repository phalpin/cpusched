using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes;

namespace cpusched.Queues
{
    /// <summary>
    /// Shortest Job First Queue.
    /// </summary>
    class SJF : ProcessQueue
    {

        /// <summary>
        /// Sorting routine for SJF
        /// </summary>
        public override void Sort()
        {
            this._switched = false;

            if (this._readyprocs.Count == 0 && this._ioprocs.Count == 0) this._state = QueueState.COMPLETE;
            else
            {
                if (this._readyprocs.Count == 0 && this._ioprocs.Count > 0)
                {
                    if (this._state == QueueState.READY)
                    {
                        this._state = QueueState.ALLIO;
                        if (this._next != null) this._switched = true;
                        this._next = null;
                    }
                }

                if (this._readyprocs.Count > 0)
                {

                    if (this._state == QueueState.ALLIO) this._state = QueueState.READY;
                    if (this._next == null || this._next.State == ProcessState.IO || this._next.State == ProcessState.COMPLETE)
                    {
                        Process p = findShortestJob();
                        if (this._next != p)
                        {
                            this._switched = true;
                            this._next = p;
                        }
                    }

                }
            }

        }

        /// <summary>
        /// Finds the shortest job in the ready queue and returns it.
        /// </summary>
        /// <returns>Process w/ shortest runtime.</returns>
        private Process findShortestJob()
        {
            Process sjf = null;
            if (this._readyprocs.Count > 0)
            {
                sjf = this._readyprocs[0];
                foreach (Process p in this._readyprocs)
                {
                    if (sjf != p && sjf.Time.Current.Duration > p.Time.Current.Duration)
                    {
                        sjf = p;
                    }
                }

            }
            return sjf;

        }

    }
}
