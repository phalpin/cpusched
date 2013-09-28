using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpusched.Processes.Execution
{
    class ExecutionTime
    {

        #region Private Vars

            private List<ExecutionTimeUnit> _timeList = new List<ExecutionTimeUnit>();

        #endregion

        #region Public Accessors

            public ExecutionTimeUnit Current
            {
                get { return this._timeList[0]; }
            }

        #endregion

    }
}
