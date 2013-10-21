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

            //Do stuff
            if (this._readyprocs.Count == 0 && this._ioprocs.Count == 0)
            {
                this._state = QueueState.COMPLETE;
            }
            else
            {
                //If there's no processes ready, but processes in the IO queue, this state changes to allIO.
                if (this._readyprocs.Count == 0 && this._ioprocs.Count > 0)
                {
                    this._state = QueueState.ALLIO;
                    this._next = null;
                }
                //Otherwise, if there's processes in the ready queue, this queue is ready.
                else if (this._readyprocs.Count > 0)
                {
                    this._state = QueueState.READY;
                    //this._next = this._readyprocs[this._readyprocs.Min(Process => Process.Time.Current.Duration)];
                    foreach (Processes.Process p in this._readyprocs)
                    {
                        Processes.Process sjf = this._readyprocs[0];
                        if (p.Time.Current.Duration < sjf.Time.Current.Duration)
                        {
                            this._next = p;
                        }
                        else { this._next = sjf; }
                    }
                    //Processes.Process sjf = (from p in this._readyprocs
                    //           select p.Time.Current.Duration).Min();
                    //this._next = sjf;
                }
            }

        }

    }
}
