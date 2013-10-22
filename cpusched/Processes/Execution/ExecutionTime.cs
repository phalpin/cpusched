using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpusched.Processes.Execution
{
    /// <summary>
    /// Container for ExecutionTimeUnits.
    /// </summary>
    public class ExecutionTime
    {

        #region Private Vars

        private List<ExecutionTimeUnit> _timeList;
        private Process _parent;

        #endregion


        #region Public Accessors

            /// <summary>
            /// Current Time Unit
            /// </summary>
            public ExecutionTimeUnit Current
            {
                get {
                    if (this._timeList.Count > 0)
                    {
                        return this._timeList[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            public Process Parent
            {
                get { return this._parent; }
                set { this._parent = value; }
            }

            /// <summary>
            /// The total remaining time (Burst + IO)
            /// </summary>
            public int Remaining
            {
                get
                {
                    int result = 0;
                    foreach (ExecutionTimeUnit t in this._timeList)
                    {
                        result += t.Duration;
                    }
                    return result;
                }
            }

        #endregion


        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ExecutionTime() { }

        /// <summary>
        /// Full constructor for ExecutionTime.
        /// </summary>
        /// <param name="t">List of ExecutionTimeUnits.</param>
        public ExecutionTime(List<ExecutionTimeUnit> t)
        {
            this._timeList = t;
        }

        /// <summary>
        /// Advances this ExecutionTime queue.
        /// </summary>
        public void Advance(){
            this._timeList.RemoveAt(0);
        }

        /// <summary>
        /// Decrements the Remaining Execution Time for an ExecutionTime object.
        /// </summary>
        /// <param name="t">Object to decrement time from.</param>
        /// <returns></returns>
        public static ExecutionTime operator--(ExecutionTime t)
        {
            if (t.Current != null)
            {
                t.Current.Duration--;
                if (t.Current.Duration == 0)
                {
                    t.Advance();
                }
            }

            return t;
        }






    }

    /// <summary>
    /// Types of ExecutionTimeUnits.
    /// </summary>
    public enum ExecutionTimeType
    {
        BURST,
        IO
    }
}
