using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes;

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

        //Allows us to initialize a RR Queue with a Time Quantum
        public RR(int timequantum)
        {
            this._timequantum = timequantum;
        }

        /// <summary>
        /// Sorting routine for Round Robin
        /// </summary>
        protected override void Sort()
        {            
            //Initial Condition Solution ***NOTE: DO NOT ADD CONTEXT SWITCH HERE***
            if (this._totalTime == 0) this._next = this._readyprocs[0];


            //If there's no processes left at all in ready and IO, set queuestate to complete.
            if (this._readyprocs.Count == 0 && this._ioprocs.Count == 0) this._state = QueueState.COMPLETE;
            //Otherwise, go ahead and assign next process.
            else
            {
                //If there's no processes ready, but processes in the IO queue, this state changes to all IO.
                if (this._readyprocs.Count == 0 && this._ioprocs.Count > 0)
                {
                    this._state = QueueState.ALLIO;
                    this._next = null;
                    this._switched = true;
                }

                //Otherwise, if there's processes in the ready queue, this queue is ready.
                else if (this._readyprocs.Count > 0)
                {
                    this._state = QueueState.READY;
                    if (this.Next != null)
                    {
                        //If the next process is Complete, the next process is up.
                        if (this.Next.State == ProcessState.COMPLETE || this.Next.State == ProcessState.IO) this._next = this._readyprocs[0]; //TODO: Add context switch functionality

                        //Time Quantum Handling.
                        if (this.Next.ActiveTimeOnProcessor >= this.TimeQuantum)
                        {
                            //Remove the item from the top of the list.
                            Process p = this.Next;
                            this._readyprocs.Remove(p);
                            this._readyprocs.Add(p);
                            //Now, set a new next.
                            this._next = this._readyprocs[0];
                            this._switched = true;
                        }
                    }
                    else
                    {
                        this._next = this._readyprocs[0];
                        this._switched = true;
                    }
                }
            }
        }

    }
}
