using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpusched.Queues
{
    public class FCFS : ProcessQueue
    {
        
        public FCFS() { }

        /// <summary>
        /// Runs the processes through in a FCFS Fashion.
        /// </summary>
        public override void Run(){
            if (this.ReadyProcs.Count > 0)
            {
                switch (this.Next.State)
                {
                    case Processes.ProcessState.READY:
                        this.Next.Run();
                        break;
                    case Processes.ProcessState.IO:
                        this.IOProcs.Add(this.ReadyProcs.ElementAt(0));
                        this.ReadyProcs.RemoveAt(0);
                        if (this.ReadyProcs.Count > 0) this.Next.Run();

                        break;
                }
            }

        }

    }
}
