using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpusched.Processes.Execution
{
    class ExecutionTimeUnit
    {

        #region Private Vars

            private ExecutionTimeType _type;
            private int _duration;

        #endregion

        #region Public Accessors

            /// <summary>
            /// Accessor to see what type of execution time unit this is.
            /// </summary>
            public ExecutionTimeType Type
            {
                get { return this._type; }
                set { this._type = value; }
            }

            /// <summary>
            /// Duration of this ExecutionTimeUnit.
            /// </summary>
            public int Duration
            {
                get { return this._duration; }
                set { this._duration = value; }
            }

        #endregion

        public ExecutionTimeUnit() { }
    }
}
