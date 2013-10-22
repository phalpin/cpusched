using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cpusched.Queues;

namespace cpusched.Processes
{
    public class ContextSwitchRecord
    {

        #region Properties
            /// <summary>
            /// The current Overall Time of the process.
            /// </summary>
            public int Time { get; set; }

            /// <summary>
            /// Complete Processes.
            /// </summary>
            public List<ProcessRecord> Complete { get; set; }

            /// <summary>
            /// IO Processes.
            /// </summary>
            public List<ProcessRecord> IO { get; set; }
            
            /// <summary>
            /// Ready Processes.
            /// </summary>
            public List<ProcessRecord> Ready { get; set; }
            
            /// <summary>
            /// What was switched to.
            /// </summary>
            public ProcessRecord Running;
        #endregion

        /// <summary>
        /// Record from a queue. The only real use case for this record as of now.
        /// </summary>
        /// <param name="queue">The queue to create this record from.</param>
        public ContextSwitchRecord(IQueue queue)
        {
            
            Process next = queue.GetContextSwitch();
            this.Ready = new List<ProcessRecord>();
            this.IO = new List<ProcessRecord>();
            this.Complete = new List<ProcessRecord>();

            this.Running = null;
            if (next != null && queue.State != QueueState.COMPLETE) this.Running = new ProcessRecord(next.Name, next.Parent.Name, next.Time.Current.Duration);
            this.Time = queue.TotalTime - 1;        //-1 because we're checking this AFTER it has incremented times.

            foreach (Process p in queue.CompleteProcs)
            {
                ProcessRecord add = new ProcessRecord()
                {
                    Name = p.Name,
                    Parent = p.Parent.Name,
                    CurrentTime = p.Time.Current == null ? 0 : p.Time.Current.Duration
                };
                this.Complete.Add(add);
            }

            foreach (Process p in queue.IOProcs) this.IO.Add(new ProcessRecord(p.Name, p.Parent.Name, p.Time.Current.Duration));

            foreach (Process p in queue.ReadyProcs)
            {
                if(p != next) this.Ready.Add(new ProcessRecord(p.Name, p.Parent.Name, p.Time.Current.Duration));
            }
        }
    }
}
