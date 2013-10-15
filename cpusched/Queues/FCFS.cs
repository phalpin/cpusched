using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes;

namespace cpusched.Queues
{

    public class FCFS : ProcessQueue
    {
        
        public FCFS() { }

        /// <summary>
        /// Sorts processes in Ready queue in a FCFS fashion.
        /// </summary>
        protected override void Sort()
        {
            //Set switched to false by default.
            if(this._switched) this._switched = false;

            //If we're all out of processes, this queue is complete.
            if (this._readyprocs.Count == 0 && this._ioprocs.Count == 0) this._state = QueueState.COMPLETE;
            //Otherwise, go ahead and assign next process.
            else
            {
                //If there's no processes ready, but processes in the IO queue, this state changes to allIO.
                if (this._readyprocs.Count == 0 && this._ioprocs.Count > 0)
                {
                    if (this._state == QueueState.READY)
                    {
                        this._state = QueueState.ALLIO; 
                        this._next = null;
                        this._switched = true;
                    }

                }

                //Otherwise, if there's processes in the ready queue, this queue is ready.
                else if (this._readyprocs.Count > 0)
                {
                    this._state = QueueState.READY;
                    if (this._next != this._readyprocs[0])
                    {
                        this._next = this._readyprocs[0];
                        this._switched = true;
                    }
                    
                }
            }


        }

    }



}
