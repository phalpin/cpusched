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
        /// Runs the processes through in a FCFS Fashion.
        /// </summary>
        public override QueueExecutionResult Run()
        {
            QueueExecutionResult result = QueueExecutionResult.IDLE;
            //Before running anything, sort.
            this.Sort();
            if(this._state != QueueState.COMPLETE){
                //Check what Sort has to say about the state of the Queue.
                switch (this._state)
                {
                    //Case for the queue being ready to run.
                    case QueueState.READY:
                        if (this.Next != null) this.Next.Run();
                        this.IncrementTimes();
                        break;
                    case QueueState.ALLIO:
                        this.IncrementTimes();
                        this._idleTime++;
                        break;
                }

                this._totalTime++;
            }
            
            return result;
        }

        /// <summary>
        /// Waits all processes in this queue. Stuff in IO runs, stuff in Ready waits.
        /// </summary>
        public override void Wait()
        {
            //Run all procs in IO.
            foreach (Process p in this._ioprocs) p.Run();
            //Wait all procs in the ready queue.
            foreach (Process p in this._readyprocs) p.Wait();
        }

        /// <summary>
        /// Sorts processes in Ready queue in a FCFS fashion.
        /// </summary>
        protected override void Sort()
        {
            //If we're all out of processes, this queue is complete. Special case in each remove block.
            if (this._readyprocs.Count == 0 && this._ioprocs.Count == 0) this._state = QueueState.COMPLETE;
            //Otherwise, go ahead and sort.
            else
            {
                #region Shift Processes out of IOProcs to Ready.
                List<Process> removeFromIO = new List<Process>();
                foreach (Process p in this._ioprocs)
                {
                    if (p.State == ProcessState.READY)
                    {
                        this._readyprocs.Add(p);
                        removeFromIO.Add(p);
                    }
                    else if (p.State == ProcessState.COMPLETE)
                    {
                        this._completeprocs.Add(p);
                        removeFromIO.Add(p);
                    }
                }
                foreach (Process p in removeFromIO)
                {
                    this._ioprocs.Remove(p);
                    //Special case in case we're removing the last process.
                    if (p.State == ProcessState.COMPLETE && this._readyprocs.Count == 0 && this._ioprocs.Count == 0) this._state = QueueState.COMPLETE;
                }
                #endregion

                #region Shift Processes out of Ready to IOProcs.
                List<Process> removeFromReady = new List<Process>();
                foreach (Process p in this._readyprocs)
                {
                    if (p.State == ProcessState.IO)
                    {
                        this._ioprocs.Add(p);
                        removeFromReady.Add(p);
                    }
                    else if (p.State == ProcessState.COMPLETE)
                    {
                        this._completeprocs.Add(p);
                        removeFromReady.Add(p);
                    }
                }
                foreach (Process p in removeFromReady)
                {
                    this._readyprocs.Remove(p);
                    //Special case in case we're removing the last process.
                    if (p.State == ProcessState.COMPLETE && this._readyprocs.Count == 0 && this._ioprocs.Count == 0) this._state = QueueState.COMPLETE;
                }
                #endregion


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
                    this._next = this._readyprocs[0];
                }
            }


        }

        /// <summary>
        /// Increments times for everything except Next.
        /// </summary>
        public override void IncrementTimes()
        {
            foreach (Process p in this._readyprocs) if (p != this._next) p.Wait();
            foreach (Process p in this._ioprocs) p.Run();
            
        }


        /// <summary>
        /// Adds a process to this queue.
        /// </summary>
        /// <param name="p">Process to add.</param>
        public override void AddProcess(Processes.Process p)
        {
            this._readyprocs.Add(p);
        }

    }



}
