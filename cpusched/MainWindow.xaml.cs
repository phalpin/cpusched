using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using cpusched.Queues;
using cpusched.Processes;
using cpusched.Processes.Execution;
using System.Data;
using System.Threading;
using System.Windows.Media.Effects;
using cpusched;

namespace cpusched
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// A very unfortunately named function. Handles the logic for the demo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            //FCFS Instantiation
            ProcessQueue fcfsQueue = new FCFS();
            this.PopulateDemoQueue(fcfsQueue);
            ContextSwitchManager csmFCFS = new ContextSwitchManager();
            Grid fcfsparent = this.tabfcfs;
            Thread threadfcfs = new Thread(() => RunQueue(fcfsQueue, csmFCFS));

            //SJF Instantiation
            ProcessQueue sjfQueue = new SJF();
            this.PopulateDemoQueue(sjfQueue);
            ContextSwitchManager csmSJF = new ContextSwitchManager();
            Grid sjfparent = this.tabsjf;
            Thread threadsjf = new Thread(() => RunQueue(sjfQueue, csmSJF));


            //MLFQ Instantiation
            MultiLevelQueue mlfqQueue = new MLFQ();
            ProcessQueue mlfqRR6 = new RR(6);
            this.PopulateDemoQueue(mlfqRR6);
            ProcessQueue mlfqRR11 = new RR(11);
            ProcessQueue mlfqFCFS = new FCFS();
            mlfqQueue.AddProcessQueue(mlfqRR6);
            mlfqQueue.AddProcessQueue(mlfqRR11);
            mlfqQueue.AddProcessQueue(mlfqFCFS);
            ContextSwitchManager csmMLFQ = new ContextSwitchManager();
            Grid mlfqparent = this.tabmlfq;
            Thread threadmlfq = new Thread(() => RunQueue(mlfqQueue, csmMLFQ));


            
            
            

            //Spawn off the threads.
            threadfcfs.Start();
            threadsjf.Start();
            threadmlfq.Start();

            //Join back up with the threads.
            threadfcfs.Join();
            threadsjf.Join();
            threadmlfq.Join();

            this.PopulateResults(fcfsQueue, csmFCFS, fcfsparent);
            this.PopulateResults(sjfQueue, csmSJF, sjfparent);
            this.PopulateResults(mlfqQueue, csmMLFQ, mlfqparent);

            this.GenerateTextFile("fcfsFall2013.txt", csmFCFS);
            this.GenerateTextFile("sjfFall2013.txt", csmSJF);
            this.GenerateTextFile("mlfqFall2013.txt", csmMLFQ);
 

        }


        /// <summary>
        /// Generates the Required Text Files.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="csm"></param>
        private void GenerateTextFile(string fileName, ContextSwitchManager csm)
        {
            string fulluri = System.Environment.CurrentDirectory + "\\" + fileName;
            System.IO.File.WriteAllText(@fulluri, csm.ToString());
        }

        /// <summary>
        /// Populates the results of a TabItem
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="csm"></param>
        /// <param name="g"></param>
        private void PopulateResults(IQueue queue, ContextSwitchManager csm, Grid g)
        {
            //Determine the grid view.
            DataGrid dg = (DataGrid)g.Children[0];

            //Determine the CPU Util View
            Label cpuutil = (Label)g.Children[1];

            //Determine the GanttView button.
            Button ganttView = (Button)g.Children[2];

            #region Display Logic

            //Datatable to display results.
            DataTable dt = new DataTable();
            dt.Columns.Add("Process");
            dt.Columns.Add("WT");
            dt.Columns.Add("TT");
            dt.Columns.Add("RT");

            queue.CompleteProcs.Sort((x, y) => x.Name.CompareTo(y.Name));

            foreach (Process p in queue.CompleteProcs)
            {
                dt.Rows.Add(p.Name, p.WaitingTime, p.TurnaroundTime, p.ResponseTime);
            }



            double avgWaitingTime = 0;
            double avgTurnaroundTime = 0;
            double avgResponseTime = 0;

            foreach (Process p in queue.CompleteProcs)
            {
                avgWaitingTime += p.WaitingTime;
                avgTurnaroundTime += p.TurnaroundTime;
                avgResponseTime += p.ResponseTime;
            }

            avgWaitingTime /= queue.CompleteProcs.Count;
            avgTurnaroundTime /= queue.CompleteProcs.Count;
            avgResponseTime /= queue.CompleteProcs.Count;

            dt.Rows.Add("Avg", avgWaitingTime, avgTurnaroundTime, avgResponseTime);

            dg.ItemsSource = dt.DefaultView;
            dg.HorizontalGridLinesBrush = dg.VerticalGridLinesBrush = new SolidColorBrush(Colors.LightGray);

            cpuutil.Content = "CPU Utilization: " + Decimal.Round((queue.CPUUtil * 100), 2) + "%";

            ganttView.IsEnabled = true;
            ganttView.Click += delegate { GetGanttView(csm); };
            #endregion
        }

        /// <summary>
        /// Populates a demo queue with the demo processes.
        /// </summary>
        /// <param name="q"></param>
        private void PopulateDemoQueue(ProcessQueue q)
        {
            List<Process> procs = this.GetDemoProcesses();
            foreach (Process p in procs) q.AddProcess(p);
        }

        /// <summary>
        /// Gets the default demo process stuff.
        /// </summary>
        /// <returns></returns>
        private List<Process> GetDemoProcesses()
        {
            List<Process> results = new List<Process>();

            int[][] time = new int[8][];
            time[0] = new int[] { 4, 24, 5, 73, 3, 31, 5, 27, 4, 33, 6, 43, 4, 64, 5, 19, 2 };
            time[1] = new int[] { 18, 31, 19, 35, 11, 42, 18, 43, 19, 47, 18, 43, 17, 51, 19, 32, 10 };
            time[2] = new int[] { 6, 18, 4, 21, 7, 19, 4, 16, 5, 29, 7, 21, 8, 22, 6, 24, 5 };
            time[3] = new int[] { 17, 42, 19, 55, 20, 54, 17, 52, 15, 67, 12, 72, 15, 66, 14 };
            time[4] = new int[] { 5, 81, 4, 82, 5, 71, 3, 61, 5, 62, 4, 51, 3, 77, 4, 61, 3, 42, 5 };
            time[5] = new int[] { 10, 35, 12, 41, 14, 33, 11, 32, 15, 41, 13, 29, 11 };
            time[6] = new int[] { 21, 51, 23, 53, 24, 61, 22, 31, 21, 43, 20 };
            time[7] = new int[] { 11, 52, 14, 42, 15, 31, 17, 21, 16, 43, 12, 31, 13, 32, 15 };



            int pnum = 1;
            for (int i = 0; i < time.Count(); i++)
            {
                List<ExecutionTimeUnit> t = new List<ExecutionTimeUnit>();
                for (int j = 0; j < time[i].Count(); j++)
                {
                    ExecutionTimeType type;
                    if (j % 2 == 0) type = ExecutionTimeType.BURST;
                    else type = ExecutionTimeType.IO;
                    t.Add(new ExecutionTimeUnit(time[i][j], type));
                }
                ExecutionTime et = new ExecutionTime(t);
                Process proc = new Process(et);
                proc.Name = "P" + pnum.ToString();
                results.Add(proc);
                pnum++;
            }

            return results;

        }

        /// <summary>
        /// Runs a queue & provides info to a contextswitchmanager.
        /// </summary>
        /// <param name="q"></param>
        /// <param name="csm"></param>
        private void RunQueue(IQueue q, ContextSwitchManager csm)
        {
            while (q.State != QueueState.COMPLETE)
            {
                q.Run();
                if (q.ContextSwitch || q.State == QueueState.COMPLETE) csm.Switches.Add(new ContextSwitchRecord(q));
            }
        }

        private void GetGanttView(ContextSwitchManager csm)
        {
            GanttView gv = new GanttView(csm, this);
        }
    }
}
