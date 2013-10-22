using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cpusched.Processes
{
    public class ContextSwitchManager
    {
        /// <summary>
        /// The switches to work with on this manager.
        /// </summary>
        public List<ContextSwitchRecord> Switches { get; set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ContextSwitchManager()
        { 
            this.Switches = new List<ContextSwitchRecord>(); 
        }

        /// <summary>
        /// Creates the massive string for output to a text file.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string result = "";


            foreach (ContextSwitchRecord cs in this.Switches)
            {
                result += "Current Time: " + cs.Time.ToString() + System.Environment.NewLine + System.Environment.NewLine;

                result += "Now Running: ";
                if (cs.Ready.Count == 0 && cs.IO.Count == 0) result += "[complete]";
                else result += cs.Running == null ? "[idle]" : cs.Running.Name;
                result += System.Environment.NewLine;


                //Get all processes in ready.
                result += "........................................................" + System.Environment.NewLine + System.Environment.NewLine;
                result += "Ready Queue:\tProcess\tBurst\tQueue" + System.Environment.NewLine;
                if (cs.Ready.Count == 0 || (cs.Ready.Count == 1 && cs.IO.Count == 0)) result += "\t\t\t\t[empty]" + System.Environment.NewLine;
                else foreach (ProcessRecord p in cs.Ready) if (p != cs.Running) result += "\t\t\t\t" + p.Name + "\t\t" + p.CurrentTime.ToString() + "\t\t" + p.Parent + System.Environment.NewLine;
                
                //Get all processes in IO
                result += "........................................................" + System.Environment.NewLine + System.Environment.NewLine;
                result += "Now in I/O:\t\tProcess\tRemaining I/O Time" + System.Environment.NewLine;
                if (cs.IO.Count == 0) result += "\t\t\t\t[empty]" + System.Environment.NewLine;
                else foreach (ProcessRecord p in cs.IO) if (p != cs.Running) result += "\t\t\t\t" + p.Name + "\t\t" + p.CurrentTime.ToString() + System.Environment.NewLine;


                if (cs.Complete.Count > 0)
                {
                    result += "........................................................" + System.Environment.NewLine + System.Environment.NewLine;
                    result += "Completed:\t\t\t\t";
                    int iter = 1;
                    foreach (ProcessRecord p in cs.Complete)
                    {
                        result += p.Name;
                        if (iter != cs.Complete.Count) result += ", ";
                        else result += System.Environment.NewLine + System.Environment.NewLine;
                        iter++;
                    }
                }

                result += "::::::::::::::::::::::::::::::::::::::::::::::::::::::::" + System.Environment.NewLine;





                result += System.Environment.NewLine + System.Environment.NewLine;
            }


            return result;
        }
    }
}
