using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpusched.Processes.Execution
{
    public class ExecutionTime
    {

        #region Private Vars

            private List<ExecutionTimeUnit> _timeList = new List<ExecutionTimeUnit>();

        #endregion


        #region Public Accessors

            /// <summary>
            /// Current Time Unit
            /// </summary>
            public ExecutionTimeUnit Current
            {
                get { return this._timeList[0]; }
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

        public ExecutionTime() { }


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
            t.Current.Duration--;
            if (t.Current.Duration == 0)
            {
                t.Advance();
            }
            return t;
        }






    }
}
