using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Processes;

namespace cpusched.Queues
{
    public class MLFQ : MultiLevelQueue
    {
        /// <summary>
        /// Sorts out processes, tells which queues to run and which to wait.
        /// </summary>
        public override void Sort()
        {
            this._switched = false;

            if (this.ReadyProcs.Count == 0 && this.IOProcs.Count == 0) this._state = QueueState.COMPLETE;
            else
            {
                //Sort all ProcessQueues.
                foreach (ProcessQueue pq in this._queues) pq.Sort();

                #region ProcessQueue Traversal for Demotions
                foreach (ProcessQueue pq in this._queues)
                {
                    if (pq.ContextSwitch)
                    {
                        Process demotedproc = pq.GetDemotedProcess();
                        if (demotedproc != null)
                        {
                            int index = this._queues.IndexOf(pq);
                            if (index != this._queues.Count - 1)
                            {
                                this._queues[index + 1].AddProcess(demotedproc);
                            }
                            pq.Cleanup();
                        }
                    }
                }
                #endregion

                

                //Traverse, starting from the top of this._queues
                foreach (ProcessQueue pq in this._queues)
                {
                    //if it has processes available, we've found our match.
                    if (pq.ReadyProcs.Count > 0 && pq.State != QueueState.COMPLETE)
                    {
                        this.State = QueueState.READY;
                        if (pq != this._next || pq.ContextSwitch) this._switched = true;
                        this._next = pq;
                        return;
                    }
                }

                //If we get out of that block, there's nothing there, so we go to all IO;
                if (this._state == QueueState.READY) this._switched = true;
                this._state = QueueState.ALLIO;
                this._next = null;


            }





        }




    }
}
