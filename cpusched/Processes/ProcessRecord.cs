using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpusched.Processes
{
    public class ProcessRecord
    {
        #region Properties
            /// <summary>
            /// The name of the Process
            /// </summary>
            public string Name { get; set; }
        
            /// <summary>
            /// The name of the parent Queue
            /// </summary>
            public string Parent { get; set; }

            /// <summary>
            /// The current ExecutionTime duration of the Process
            /// </summary>
            public int CurrentTime { get; set; }
        #endregion

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ProcessRecord() { }

        /// <summary>
        /// Full creation Constructor
        /// </summary>
        /// <param name="name">The name of the process</param>
        /// <param name="parent">The name of the parent queue</param>
        /// <param name="time">The current ExecutionTime duration of the process</param>
        public ProcessRecord(string name, string parent, int time)
        {
            this.Name = name;
            this.Parent = parent;
            this.CurrentTime = time;
        }
    }
}
