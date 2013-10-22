﻿using System;
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

namespace cpusched
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string _contextSwitchString = "";
        private ContextSwitchManager _contextSwitchManager = new ContextSwitchManager();

        public MainWindow()
        {
            InitializeComponent();
            //Testing Branch Commit.
        }

        /// <summary>
        /// USE THIS FOR TESTING. It will prepare all of the times for you. All you have to input is commented.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            //this.MAINWINDOW.Effect = new BlurEffect();


            //CHANGE THE BELOW LINE TO TEST YOUR QUEUE.
            //EXAMPLE:
            //ProcessQueue testqueue = new FCFS();
            //Becomes...
            //ProcessQueue testqueue = new SJF(); //etc
            ProcessQueue testqueue = new FCFS();
            //ProcessQueue testqueue = new SJF();
            //ProcessQueue testqueue = new RR(6);

            ContextSwitchManager csm = new ContextSwitchManager();

            #region Process Instantiation.
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
                testqueue.AddProcess(proc);
                pnum++;
            }

            MLFQ testmlqueue = new MLFQ();
            testqueue.Name = "Q1";
            testmlqueue.AddProcessQueue(testqueue);
            testmlqueue.AddProcessQueue(new RR(11) { Name = "Q2" });
            testmlqueue.AddProcessQueue(new FCFS() { Name = "Q3" });




            #endregion

            Thread thread = new Thread(() => RunQueue(testqueue, csm));
            IQueue displayQueue = testqueue;
            //Thread thread = new Thread(() => RunQueue(testmlqueue, csm));
            //IQueue displayQueue = testmlqueue;

            thread.Start();
            thread.Join();

            this.btn_contextswitchview.IsEnabled = true;

            #region Display crap for debugging

            //Datatable to display results.
            DataTable dt = new DataTable();
            dt.Columns.Add("Process");
            dt.Columns.Add("WT");
            dt.Columns.Add("TT");
            dt.Columns.Add("RT");

            displayQueue.CompleteProcs.Sort((x, y) => x.Name.CompareTo(y.Name));

            foreach (Process p in displayQueue.CompleteProcs)
            {
                dt.Rows.Add(p.Name, p.WaitingTime, p.TurnaroundTime, p.ResponseTime);
            }


            
            double avgWaitingTime = 0;
            double avgTurnaroundTime = 0;
            double avgResponseTime = 0;

            foreach (Process p in displayQueue.CompleteProcs)
            {
                avgWaitingTime += p.WaitingTime;
                avgTurnaroundTime += p.TurnaroundTime;
                avgResponseTime += p.ResponseTime;
            }

            avgWaitingTime /= displayQueue.CompleteProcs.Count;
            avgTurnaroundTime /= displayQueue.CompleteProcs.Count;
            avgResponseTime /= displayQueue.CompleteProcs.Count;

            dt.Rows.Add("Avg", avgWaitingTime, avgTurnaroundTime, avgResponseTime);

            gridResults.ItemsSource = dt.DefaultView;
            gridResults.HorizontalGridLinesBrush = gridResults.VerticalGridLinesBrush = new SolidColorBrush(Colors.LightGray);

            this.lblUtilization.Content = "CPU Utilization: " + Decimal.Round((displayQueue.CPUUtil * 100), 2) + "%";
            #endregion

            this._contextSwitchString = csm.ToString();
            this._contextSwitchManager = csm;

            //Debug breakpoint sp
            object r = new Object();

            
        }

        private void RunQueue(IQueue q, ContextSwitchManager csm)
        {
            while (q.State != QueueState.COMPLETE)
            {
                q.Run();
                if (q.ContextSwitch || q.State == QueueState.COMPLETE) csm.Switches.Add(new ContextSwitchRecord(q));
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GanttView gv = new GanttView(this._contextSwitchManager, this);
        }
    }
}
