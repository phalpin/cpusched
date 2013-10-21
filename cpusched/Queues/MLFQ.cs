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
            //First, run through the processes from the last round, see if there's any demotions.
            foreach (ProcessQueue p in this._queues)
            {
                if (p.ContextSwitch)
                {
                    Process demotedproc = p.GetDemotedProcess();
                    if (demotedproc != null)
                    {
                        if (demotedproc.State == ProcessState.IO) { };
                    }
                }
            }
            
            foreach (ProcessQueue p in this._queues)
            {
                if (p.Next != null) this._next = null;
            }
        }




    }
}
